using DoAnCuoiKiCSDL;
using DoAnCuoiKiCSDL.Category;
using DoAnCuoiKiCSDL.Dishes;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
namespace DoAnCuoiKiCSDL
{
    public partial class ManageFood : Form
    {
        // Cho phép cuộn nếu nội dung vượt quá chiều cao

        DBConnection dbConnect = new DBConnection();
        MonAn monAn = new MonAn();
        DanhMuc danhMuc = new DanhMuc();
        public ManageFood()
        {
            InitializeComponent();
            tableLayoutPanel2.AutoScroll = true;
            tableLayoutPanelDanhMuc.AutoScroll = true;
            LoadCategoryData();
            LoadFoodData1();
            this.Size = new Size(1250, 700);
            // Đặt kích thước cố định cho form lớn hơn
            //this.Height = 800; // Tăng chiều cao của Form (có thể thay đổi số này theo nhu cầu)

            // Đảm bảo TableLayoutPanel lấp đầy Form
            // tableLayoutPanel2.Dock = DockStyle.Fill; // Giúp TableLayoutPanel chiếm toàn bộ không gian còn lại

            // Hoặc đảm bảo TableLayoutPanel có chiều cao tăng khi form tăng
            //tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Nếu cần, bật cuộn tự động
            this.AutoScroll = true; // Form tự cuộn khi nội dung vượt chiều cao
           // tableLayoutPanelDanhMuc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
          
        }

        private void ManageFood_Load(object sender, EventArgs e)
        {
            //LoadFoodData1();
        }

        private void LoadCategoryData()
        {
            byte[] imageBytes = Resource.Resources.image_with_white_pen_and_paper;
            System.Drawing.Image image; // Sử dụng System.Drawing.Image để tránh mơ hồ

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                image = System.Drawing.Image.FromStream(ms); // Chuyển đổi từ byte[] thành System.Drawing.Image
            }

            try
            {
                DataTable dt = danhMuc.LietKeDanhSachDanhMuc();
                if (dt != null)
                {
                    // Xóa các control trước khi thêm mới (tránh trùng lặp khi load lại)
                    tableLayoutPanelDanhMuc.Controls.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        FlowLayoutPanel  panel = new FlowLayoutPanel
                        {
                            Size = new Size(310, 60), // Kích thước của panel chứa các nút
                            Margin = new Padding(10), // Khoảng cách giữa các panel
                           // panel.AutoSize= true
                        };

                        // Tạo nút phụ bên trái để chứa icon
                        Guna.UI2.WinForms.Guna2Button btnIcon = new Guna.UI2.WinForms.Guna2Button
                        {
                            Size = new Size(50, 50), // Kích thước của nút icon
                            Image = image,  // Đặt icon cho nút (cập nhật namespace nếu cần)
                            ImageSize = new Size(20, 20), // Điều chỉnh kích thước của icon
                            BorderRadius = 25, // Làm nút có hình tròn
                            FillColor = Color.DodgerBlue, // Làm nút trong suốt hoặc có thể là màu khác
                            Margin = new Padding(5),
                        };

                        // Tạo nút chính bên phải
                        Guna.UI2.WinForms.Guna2Button btnCategory = new Guna.UI2.WinForms.Guna2Button
                        {
                            Text = row["Ten"].ToString(),
                            Tag = row["Id_DanhMuc"], // Gán ID danh mục vào Tag
                            Size = new Size(150, 50), // Căn chỉnh kích thước chiều rộng và chiều cao
                            Margin = new Padding(5),
                            Font = new Font("Arial", 10, FontStyle.Bold), // Chỉnh cỡ chữ và font
                            BorderRadius = 10, // Tạo bo góc cho button
                            FillColor = Color.DodgerBlue, // Màu nền cho button
                            ForeColor = Color.White // Màu chữ của button
                        };

                        // Thêm sự kiện click cho nút chính
                        btnCategory.Click += BtnCategory_Click;

                        // Thêm sự kiện click cho nút icon
                        btnIcon.Click += (s, e) => BtnIcon_Click(s, e, btnCategory.Tag);

                        // Thêm cả hai nút vào Panel
                        panel.Controls.Add(btnIcon);
                        panel.Controls.Add(btnCategory);

                        // Căn chỉnh vị trí của các nút bên trong Panel
                        btnIcon.Location = new Point(0, 5); // Đặt vị trí của btnIcon
                        btnCategory.Location = new Point(60, 5); // Đặt vị trí của btnCategory

                        // Thêm Panel vào TableLayoutPanel
                        tableLayoutPanelDanhMuc.Controls.Add(panel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }
        private void BtnCategory_Click(object sender, EventArgs e)
        {
            // Khi nhấn vào danh mục, gọi lại hàm LoadFoodData để tải món ăn theo danh mục đã chọn
            Guna.UI2.WinForms.Guna2Button clickedButton = sender as Guna.UI2.WinForms.Guna2Button;
            if (clickedButton != null)
            {
                int selectedCategoryId = (int)clickedButton.Tag;
                LoadFoodData(selectedCategoryId);
            }
        }
        private void BtnIcon_Click(object sender, EventArgs e, object categoryId)
        {
            try
            {
                // Sử dụng categoryId để xác định form cần mở
                string idDanhMuc = categoryId.ToString();
                EditCategoryForm form1= new EditCategoryForm(int.Parse(idDanhMuc));
                form1.ShowDialog();
                LoadFoodData1();
                LoadCategoryData();
                // xử lí danh mục:
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form: " + ex.Message);
            }
        }

        private void LoadFoodData1()
        {
            try
            {
                // Sử dụng phương thức LietKeDanhSachMonAn từ lớp MonAn
                DataTable foodData = monAn.LietKeDanhSachMonAn();

                // Xóa các control trước khi thêm mới (tránh trùng lặp khi load lại)
                tableLayoutPanel2.Controls.Clear();

                if (foodData != null)
                {
                    foreach (DataRow row in foodData.Rows)
                    {
                        int idMonAn = (int)row["Id_MonAn"]; // Lấy Id_MonAn
                        string ten = row["Ten"].ToString(); // Lấy tên món ăn
                        decimal gia = (decimal)row["Gia"]; // Lấy giá món ăn

                        // Lấy hình ảnh từ cơ sở dữ liệu
                        byte[] hinhAnhData = row.IsNull("HinhAnh") ? null : (byte[])row["HinhAnh"];
                        System.Drawing.Image hinhAnh = hinhAnhData != null ? ByteArrayToImage(hinhAnhData) : null; // Chuyển đổi byte array sang Image nếu không NULL

                        // Thêm từng món ăn vào giao diện
                        AddFoodToLayout(idMonAn, ten, gia, hinhAnh);
                    }
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu món ăn.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }
        private void LoadFoodData(int? categoryId = null)
        {

            try
            {
                DataTable foodData;

                if (categoryId.HasValue)
                {
                    // Nếu có ID danh mục, lọc món ăn theo danh mục
                    foodData = monAn.DanhSachMonAnTheoDanhMuc(categoryId.Value);
                }
                else
                {
                    // Nếu không có ID danh mục, lấy toàn bộ món ăn
                    foodData = monAn.LietKeDanhSachMonAn();
                }

                // Xóa các control trước khi thêm mới (tránh trùng lặp khi load lại)
                tableLayoutPanel2.Controls.Clear();

                if (foodData != null)
                {
                    foreach (DataRow row in foodData.Rows)
                    {
                        int idMonAn = (int)row["Id_MonAn"]; // Lấy Id_MonAn
                        string ten = row["Ten"].ToString(); // Lấy tên món ăn
                        decimal gia = (decimal)row["Gia"]; // Lấy giá món ăn

                        // Lấy hình ảnh từ cơ sở dữ liệu
                        byte[] hinhAnhData = row.IsNull("HinhAnh") ? null : (byte[])row["HinhAnh"];
                        System.Drawing.Image hinhAnh = hinhAnhData != null ? ByteArrayToImage(hinhAnhData) : null; // Chuyển đổi byte array sang Image nếu không NULL

                        // Thêm từng món ăn vào giao diện
                        AddFoodToLayout(idMonAn, ten, gia, hinhAnh);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }


        private System.Drawing.Image ByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        private void AddFoodToLayout(int idMonAn, string ten, decimal gia, System.Drawing.Image hinhAnh)
        {
            // Định nghĩa panel với các thuộc tính đã có
            System.Windows.Forms.FlowLayoutPanel foodPanel = new System.Windows.Forms.FlowLayoutPanel
            {
                Width = 150,
                Height = 220,
                AutoSize = false,
                FlowDirection = System.Windows.Forms.FlowDirection.TopDown,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                Padding = new System.Windows.Forms.Padding(5),
                Margin = new System.Windows.Forms.Padding(5),
                BackColor = System.Drawing.Color.LightBlue // Màu nền để dễ thấy đường viền bo tròn
            };

            // Thêm các control vào panel
            foodPanel.Controls.Add(new System.Windows.Forms.PictureBox
            {
                Image = hinhAnh,
                SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage,
                Width = 132,
                Height =115,
                Margin = new System.Windows.Forms.Padding(3)
            });

            foodPanel.Controls.Add(new System.Windows.Forms.Label
            {
                Text = ten,
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Width = 140,
                Height = 20,
                AutoSize = false
            });

            foodPanel.Controls.Add(new System.Windows.Forms.Label
            {
                Text = $"{gia:N0} VND",
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Width = 140,
                Height = 20,
                AutoSize = false
            });
            // Chuyển đổi byte[] từ tài nguyên thành System.Drawing.Image
            byte[] imageBytes = Resource.Resources.image_with_white_pen_and_paper;
            System.Drawing.Image image; // Sử dụng System.Drawing.Image để tránh mơ hồ

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                image = System.Drawing.Image.FromStream(ms); // Chuyển đổi từ byte[] thành System.Drawing.Image
            }
            Guna2Button btnEdit = new Guna2Button
            {
                Tag = idMonAn,
                Size = new System.Drawing.Size(100, 25),
                Margin = new System.Windows.Forms.Padding(3),
                TextAlign = HorizontalAlignment.Center,
                Image = image, // Kiểm tra đúng namespace của hình ảnh này
                ImageAlign = HorizontalAlignment.Center,
                ImageSize = new System.Drawing.Size(15, 15),
                Anchor = System.Windows.Forms.AnchorStyles.Top,
                FillColor = System.Drawing.Color.DodgerBlue,
                ForeColor = System.Drawing.Color.White,
                BorderColor = System.Drawing.Color.DarkBlue,
                BorderThickness = 1
            };

            btnEdit.Click += (s, e) =>
            {
                EditDishForm editFoodForm = new EditDishForm((int)btnEdit.Tag);
                editFoodForm.ShowDialog();
                LoadCategoryData();
                LoadFoodData1();
            };

            foodPanel.Controls.Add(btnEdit);

            // Thêm panel vào TableLayoutPanel
            tableLayoutPanel2.Controls.Add(foodPanel);

            // Áp dụng góc bo tròn cho foodPanel
            ApplyRoundedCorners(foodPanel, 10);
        }




        private void ApplyRoundedCorners(System.Windows.Forms.Control control, int borderRadius)
        {
            // Tạo GraphicsPath với góc bo tròn
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(new System.Drawing.Rectangle(0, 0, borderRadius, borderRadius), 180, 90);
            path.AddArc(new System.Drawing.Rectangle(control.Width - borderRadius, 0, borderRadius, borderRadius), 270, 90);
            path.AddArc(new System.Drawing.Rectangle(control.Width - borderRadius, control.Height - borderRadius, borderRadius, borderRadius), 0, 90);
            path.AddArc(new System.Drawing.Rectangle(0, control.Height - borderRadius, borderRadius, borderRadius), 90, 90);
            path.CloseFigure();

            // Thiết lập Region của Control theo GraphicsPath đã tạo
            control.Region = new System.Drawing.Region(path);
        }


        private void btnRestart_Click(object sender, EventArgs e)
       {
            LoadFoodData1();
            LoadCategoryData();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim(); // Giả sử bạn đã tạo một TextBox để nhập tên món ăn

            if (!string.IsNullOrEmpty(searchText))
            {
                DataTable searchResults = monAn.TimKiemMonAn(searchText); // Gọi hàm tìm kiếm

                // Hiển thị kết quả tìm kiếm
                DisplayFoodResults(searchResults);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên món ăn để tìm kiếm.");
            }
        }
        private void DisplayFoodResults(DataTable foodResults)
        {
            tableLayoutPanel2.Controls.Clear(); // Xóa các món ăn hiện có

            if (foodResults != null && foodResults.Rows.Count > 0)
            {
                foreach (DataRow row in foodResults.Rows)
                {
                    int idMonAn = (int)row["Id_MonAn"]; // Lấy Id_MonAn
                    string ten = row["Ten"].ToString(); // Lấy tên món ăn
                    decimal gia = (decimal)row["Gia"]; // Lấy giá món ăn

                    // Lấy hình ảnh từ cơ sở dữ liệu
                    byte[] hinhAnhData = row.IsNull("HinhAnh") ? null : (byte[])row["HinhAnh"];
                    System.Drawing.Image hinhAnh = hinhAnhData != null ? ByteArrayToImage(hinhAnhData) : null; // Chuyển đổi byte array sang Image nếu không NULL

                    // Thêm từng món ăn vào giao diện
                    AddFoodToLayout(idMonAn, ten, gia, hinhAnh);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy món ăn nào với tên đã nhập.");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddCategoryForm form1= new AddCategoryForm();
            form1.ShowDialog();
            LoadFoodData1();
            LoadCategoryData();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddDishesForm form1= new AddDishesForm();
            form1.ShowDialog();
            LoadFoodData1();
            LoadCategoryData();
        }
    }
}

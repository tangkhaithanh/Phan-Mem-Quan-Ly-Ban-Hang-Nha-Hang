using DoAnCuoiKiCSDL.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.EmployeeFolder
{
    public partial class SelectDishesForm : Form
    {
        ChiTietDonHang chiTietDonHang = new ChiTietDonHang();
        DonHang donHang = new DonHang();
        private int idBan;
        private string phoneNumber;
        DBConnection dbConnect = new DBConnection();
        MonAn monAn = new MonAn();
        DanhMuc danhMuc = new DanhMuc();
        private HashSet<int> selectedDishes = new HashSet<int>();
        private HashSet<int> selectedDelete = new HashSet<int>();
        private bool checkDungDiem;

        public SelectDishesForm(int id,string phoneNumber)
        {
            InitializeComponent();
            this.idBan = id;
            this.phoneNumber = phoneNumber;
            SetupDataGridView();
            tableLayoutPanel2.AutoScroll = true;
            tableLayoutPanelDanhMuc.AutoScroll = true;
            LoadCategoryData();
            // Đặt kích thước cố định cho form lớn hơn
            this.Height = 800; // Tăng chiều cao của Form (có thể thay đổi số này theo nhu cầu)

            // Đảm bảo TableLayoutPanel lấp đầy Form
            tableLayoutPanel2.Dock = DockStyle.Fill; // Giúp TableLayoutPanel chiếm toàn bộ không gian còn lại

            // Hoặc đảm bảo TableLayoutPanel có chiều cao tăng khi form tăng
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Nếu cần, bật cuộn tự động
            this.AutoScroll = true; // Form tự cuộn khi nội dung vượt chiều cao
            tableLayoutPanelDanhMuc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            LoadSelectedDishes(idBan);
        }
        /*private void LoadSelectedDishes(int idBan)
        {
            DataTable selectedDishesData = donHang.GetSelectedDishesByTableId(idBan); // Giả sử bạn đã viết phương thức trong class DonHang

            if (selectedDishesData != null && selectedDishesData.Rows.Count > 0)
            {
                foreach (DataRow row in selectedDishesData.Rows)
                {
                    int idMonAn = Convert.ToInt32(row["Id_MonAn"]);
                    int quantity = Convert.ToInt32(row["SoLuong"]);
                    decimal price = GetPriceByIdMonAn(idMonAn); // Lấy giá dựa trên ID món ăn

                    // Thêm món ăn vào DataGridView
                    AddDishToSelectedGrid(idMonAn, row["Ten_MonAn"].ToString(), price);
                    guna2DataGridViewSelectedDishes.Rows[guna2DataGridViewSelectedDishes.Rows.Count - 1].Cells[1].Value = quantity; // Cập nhật số lượng

                    selectedDishes.Add(idMonAn); // Thêm vào HashSet
                }
                UpdateTotalAmountLabel(); // Cập nhật hiển thị tổng tiền
            }
        }*/
        private void LoadSelectedDishes(int idBan)
        {
            DataTable selectedDishesData = donHang.GetSelectedDishesByTableId(idBan); // Giả sử bạn đã viết phương thức trong class DonHang

            if (selectedDishesData != null && selectedDishesData.Rows.Count > 0)
            {
                guna2DataGridViewSelectedDishes.Rows.Clear(); // Xóa các món cũ trước khi tải lại

                foreach (DataRow row in selectedDishesData.Rows)
                {
                    int idMonAn = Convert.ToInt32(row["Id_MonAn"]);
                    int quantity = Convert.ToInt32(row["SoLuong"]);
                    decimal price = GetPriceByIdMonAn(idMonAn); // Lấy giá dựa trên ID món ăn

                    // Tính tổng giá cho từng món ăn
                    decimal totalPrice = quantity * price;

                    // Thêm món ăn vào DataGridView
                    int rowIndex = guna2DataGridViewSelectedDishes.Rows.Add(
                        row["Ten_MonAn"].ToString(), // Tên món ăn
                        quantity,                    // Số lượng
                        price,                       // Giá
                        totalPrice                   // Tổng giá (quantity * price)
                    );

                    guna2DataGridViewSelectedDishes.Rows[rowIndex].Tag = idMonAn;
                    // Cập nhật danh sách đã chọn
                    selectedDishes.Add(idMonAn);
                }

                // Sau khi tải lại dữ liệu, tính toán lại tổng số tiền
                RecalculateTotalAmount();
            }
        }
        private decimal GetPriceByIdMonAn(int idMonAn)
        {
            DataTable dt = monAn.TimKiemMonAnTheoId(idMonAn); // Giả sử bạn có phương thức để lấy món ăn theo ID
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0]["Gia"]); // Trả về giá
            }
            return 0; // Nếu không tìm thấy
        }
        private void LoadCategoryData()
        {
            try
            {
                DataTable dt = danhMuc.LietKeDanhSachDanhMuc();
                if (dt != null)
                {
                    // Xóa các control trước khi thêm mới (tránh trùng lặp khi load lại)
                    tableLayoutPanelDanhMuc.Controls.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        

                        // Tạo nút phụ bên trái để chứa icon


                        // Tạo nút chính bên phải
                        Guna.UI2.WinForms.Guna2Button btnCategory = new Guna.UI2.WinForms.Guna2Button
                        {
                            Text = row["Ten"].ToString(),
                            Tag = row["Id_DanhMuc"], // Gán ID danh mục vào Tag
                            Size = new Size(120, 50), // Căn chỉnh kích thước chiều rộng và chiều cao
                            Margin = new Padding(5),
                            Font = new Font("Arial", 10, FontStyle.Bold), // Chỉnh cỡ chữ và font
                            BorderRadius = 10, // Tạo bo góc cho button
                            FillColor = Color.DodgerBlue, // Màu nền cho button
                            ForeColor = Color.White // Màu chữ của button
                        };

                        // Thêm sự kiện click cho nút chính
                        btnCategory.Click += BtnCategory_Click;

                        // Thêm sự kiện click cho nút icon

                        // Thêm cả hai nút vào Panel
                        //panel.Controls.Add(btnCategory);

                        // Căn chỉnh vị trí của các nút bên trong Panel
                        btnCategory.Location = new Point(60, 5); // Đặt vị trí của btnCategory

                        // Thêm Panel vào TableLayoutPanel
                        tableLayoutPanelDanhMuc.Controls.Add(btnCategory);
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
                Width = 158,
                Height = 260,
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
                Width = 140,
                Height = 120,
                Margin = new System.Windows.Forms.Padding(5)
            });

            foodPanel.Controls.Add(new System.Windows.Forms.Label
            {
                Text = ten,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Width = 140,
                Height = 40,
                AutoSize = false
            });

            foodPanel.Controls.Add(new System.Windows.Forms.Label
            {
                Text = $"{gia:N0} VND",
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Width = 140,
                Height = 30,
                AutoSize = false
            });

            // Tạo nút chọn món
            Guna.UI2.WinForms.Guna2Button btnSelect = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Chọn",
                Tag = new { IdMonAn = idMonAn, TenMonAn = ten, GiaMonAn = gia }, // Gán thông tin món ăn vào Tag
                Width = 80, // Tăng chiều rộng để chữ "Chọn" hiển thị thoải mái và dễ nhấn
                Height = 35, // Tăng chiều cao để giúp nút nhìn cân đối và dễ nhấn hơn
                Margin = new System.Windows.Forms.Padding(30, 5, 30, 5), // Đặt khoảng cách bên trái và bên phải bằng nhau để nút nằm giữa
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), // Font chữ đậm để dễ đọc
                BorderRadius = 10, // Đặt bo góc cho nút để tạo vẻ mềm mại hơn
                //BackColor = System.Drawing.Color.DodgerBlue, // Màu nền xanh nổi bật
                ForeColor = System.Drawing.Color.White, // Màu chữ trắng để tương phản với nền xanh
                Cursor = Cursors.Hand, // Đổi con trỏ thành hình bàn tay khi rê chuột để tăng tính tương tác
                TextAlign = System.Windows.Forms.HorizontalAlignment.Center, // Canh giữa văn bản
                Padding = new System.Windows.Forms.Padding(5), // Thêm Padding để chữ không quá sát viền
                Anchor = AnchorStyles.None // Đặt Anchor thành None để không cố định vào cạnh nào, giúp căn giữa dễ hơn
            };
            btnSelect.Click += BtnSelect_Click; // Thêm sự kiện click cho nút chọn món

            foodPanel.Controls.Add(btnSelect); // Thêm nút chọn vào panel

            // Thêm panel vào TableLayoutPanel
            tableLayoutPanel2.Controls.Add(foodPanel);

            // Áp dụng góc bo tròn cho foodPanel
            ApplyRoundedCorners(foodPanel,20);
        }


        private void BtnSelect_Click(object sender, EventArgs e)
        {
            // Khi nhấn nút chọn, lấy thông tin món ăn từ Tag
            var button = sender as Guna.UI2.WinForms.Guna2Button;
            dynamic data = button.Tag;
            if (data != null)
            {
                // Kiểm tra xem món ăn đã được chọn chưa
                if (selectedDishes.Contains(data.IdMonAn))
                {
                    MessageBox.Show("Món ăn này đã được chọn rồi, qua kia thiết lập số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Không thực hiện thêm món ăn nếu đã được chọn
                }

                // Thêm món ăn vào Guna2DataGridView
                AddDishToSelectedGrid(data.IdMonAn, data.TenMonAn, data.GiaMonAn);

                // Thêm ID món ăn vào danh sách đã chọn
                selectedDishes.Add(data.IdMonAn);
            }
        }
        private decimal totalAmount = 0; // Tổng tiền
        private void AddDishToSelectedGrid(int idMonAn, string ten, decimal gia)
        {
            int quantity = 1;
            decimal totalPrice = quantity * gia;
            int rowIndex = guna2DataGridViewSelectedDishes.Rows.Add(
                ten,       // Tên món
                1,         // Số lượng mặc định
                gia,       // Giá
                totalPrice       // Tổng giá mặc định (1 * giá)
            );
            guna2DataGridViewSelectedDishes.Rows[rowIndex].Tag = idMonAn;
            totalAmount += totalPrice; // Cộng giá của món ăn vào tổng tiền
            UpdateTotalAmountLabel(); // Cập nhật hiển thị tổng tiền
        }
        private void SetupDataGridView()
        {
            // Đặt số lượng cột
            guna2DataGridViewSelectedDishes.ColumnCount = 4;

            // Định nghĩa các tên cột
            guna2DataGridViewSelectedDishes.Columns[0].Name = "Tên Món";
            guna2DataGridViewSelectedDishes.Columns[1].Name = "Số Lượng"; // Cột Số Lượng
            guna2DataGridViewSelectedDishes.Columns[2].Name = "Giá"; // Cột Giá
            guna2DataGridViewSelectedDishes.Columns[3].Name = "Tổng Giá"; // Cột Tổng Giá

            // Đặt chế độ chỉnh sửa cho cột Số Lượng
            guna2DataGridViewSelectedDishes.Columns[1].ReadOnly = false;
            guna2DataGridViewSelectedDishes.Columns[0].ReadOnly = true;// Để người dùng có thể nhập số lượng
            guna2DataGridViewSelectedDishes.Columns[3].ReadOnly = true;
            guna2DataGridViewSelectedDishes.Columns[2].ReadOnly = true;// Tổng Giá không được chỉnh sửa

            guna2DataGridViewSelectedDishes.AllowUserToAddRows = false; // Ngăn chặn việc thêm hàng mới


            guna2DataGridViewSelectedDishes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động điều chỉnh kích thước cột
            guna2DataGridViewSelectedDishes.RowTemplate.Height = 50; // Điều chỉnh chiều cao hàng
            guna2DataGridViewSelectedDishes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            foreach (DataGridViewColumn column in guna2DataGridViewSelectedDishes.Columns)
            {
                column.HeaderCell.Style.BackColor = Color.DodgerBlue;
                column.HeaderCell.Style.ForeColor = Color.White;
                column.HeaderCell.Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                column.HeaderCell.Style.Padding = new Padding(5); // Thêm Padding cho Header
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Padding = new Padding(5);
            }
            DataGridViewImageColumn deleteImageColumn = new DataGridViewImageColumn();
            deleteImageColumn.Name = "Thao Tác";
            deleteImageColumn.HeaderText = "Thao Tác";
            deleteImageColumn.Image = Resource.Resources.dark_red_icon_fixed; // Đường dẫn đến file icon xóa
            deleteImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; // Đặt chế độ hiển thị icon
            deleteImageColumn.DefaultCellStyle.Padding = new Padding(10); // Tạo khoảng cách để thu nhỏ hình ảnh

            // Áp dụng phong cách cho header của cột icon
            deleteImageColumn.HeaderCell.Style.BackColor = Color.DodgerBlue;
            deleteImageColumn.HeaderCell.Style.ForeColor = Color.White;
            deleteImageColumn.HeaderCell.Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            deleteImageColumn.HeaderCell.Style.Padding = new Padding(5);
            deleteImageColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Thêm cột icon vào DataGridView
            guna2DataGridViewSelectedDishes.Columns.Add(deleteImageColumn);

            // Sự kiện xử lý khi nhấn vào icon xóa
            guna2DataGridViewSelectedDishes.CellContentClick += guna2DataGridViewSelectedDishes_CellContentClick;
        }


        private void SelectDishesForm_Load(object sender, EventArgs e)
        {
            if (donHang.CheckIfOrderExists(idBan, out int idDonHang))
            {
                // Lấy PhuongThucThanhToan từ idDonHang
                DataTable phuongThucThanhToan = donHang.GetDonHangByIdDonHang(idDonHang);

                // Gán PhuongThucThanhToan vào ComboBox
                PhuongThucThanhToanComboBox.SelectedItem = phuongThucThanhToan.Rows[0]["PhuongThucThanhToan"].ToString(); // hoặc comboBoxPhuongThucThanhToan.Text = phuongThucThanhToan;
            }    
            LoadFoodData1();
            LoadSelectedDishes(idBan);
            guna2DataGridViewSelectedDishes.CellValueChanged += guna2DataGridViewSelectedDishes_CellValueChanged; // 
        }
        private void guna2DataGridViewSelectedDishes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1) // Nếu cột số lượng được thay đổi
            {
                if (int.TryParse(guna2DataGridViewSelectedDishes.Rows[e.RowIndex].Cells[1].Value.ToString(), out int quantity))
                {
                    // Lấy giá của món ăn
                    decimal price = Convert.ToDecimal(guna2DataGridViewSelectedDishes.Rows[e.RowIndex].Cells[2].Value);
                    decimal totalPrice = price * quantity; // Tính tổng giá

                    // Cập nhật tổng giá vào cột Tổng Giá
                    guna2DataGridViewSelectedDishes.Rows[e.RowIndex].Cells[3].Value = totalPrice;
                    RecalculateTotalAmount();
                }
            }
        }
        private void RecalculateTotalAmount()
        {
            totalAmount = 0; // Reset tổng tiền
            foreach (DataGridViewRow row in guna2DataGridViewSelectedDishes.Rows)
            {
                if (row.Cells[3].Value != null)
                {
                    totalAmount += Convert.ToDecimal(row.Cells[3].Value);
                }
            }
            UpdateTotalAmountLabel(); // Cập nhật hiển thị tổng tiền
        }
        private void UpdateTotalAmountLabel()
        {
            lblTotalAmount.Text = $"Tổng Tiền: {totalAmount:N0} VND"; // Hiển thị tổng tiền
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = Searchtxt.Text.Trim(); // Giả sử bạn đã tạo một TextBox để nhập tên món ăn

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

        private void btnRestart_Click(object sender, EventArgs e)
        {
            LoadFoodData1();
            LoadCategoryData();
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            string sdtKhachHang = phoneNumber;
            int idNhanVien = Global.id;
            if (PhuongThucThanhToanComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu không có phương thức thanh toán
            }

            string phuongThucThanhToan = PhuongThucThanhToanComboBox.SelectedItem.ToString();
            DateTime ngayGio = DateTime.Now; // Ngày giờ đã chọn
            decimal tongTien = totalAmount;
            if (string.IsNullOrEmpty(tongTien.ToString()))
            {
                MessageBox.Show("Vui lòng chọn món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu không có phương thức thanh toán
            }

            // Lưu đơn hàng vào cơ sở dữ liệu
            if (guna2DataGridViewSelectedDishes.Rows.Count == 0)
            {
                MessageBox.Show("Không có món ăn nào được chọn. Vui lòng thêm món ăn trước khi lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng lại nếu không có món ăn nào
            }
            int idDonHang;
            // Kiểm tra xem bàn đã có đơn hàng chưa
            if (donHang.CheckIfOrderExists(idBan, out idDonHang))
            {
                donHang.UpdateOrderTotal(idDonHang, tongTien, PhuongThucThanhToanComboBox.Text);
                MessageBox.Show("Đơn hàng đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Nếu không có đơn hàng, thêm đơn hàng mới
                if (!donHang.ThemDonHang(sdtKhachHang, idNhanVien, phuongThucThanhToan, idBan, ngayGio, tongTien))
                {
                    MessageBox.Show("Có lỗi xảy ra khi lưu đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Đơn hàng đã được lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Kiểm tra các món ăn cần xóa trong selectedDelete
            foreach (int idMonAn in selectedDelete)
            {
                // Kiểm tra nếu món ăn đã tồn tại trong đơn hàng
                if (donHang.CheckIfDishExists(idBan, idMonAn))
                {
                    // Xóa món ăn khỏi đơn hàng
                    if (!donHang.DeleteSelectedDish(idBan, idMonAn))
                    {
                        MessageBox.Show($"Có lỗi xảy ra khi xóa món ăn với ID {idMonAn}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Dừng lại nếu có lỗi xảy ra khi xóa
                    }
                }
            }

            foreach (DataGridViewRow row in guna2DataGridViewSelectedDishes.Rows)
            {
                if (row.Cells[0].Value != null) // Kiểm tra ô Tên Món không null
                {
                    int idMonAn = GetIdMonAnByName(row.Cells[0].Value.ToString()); // Hàm để lấy ID món ăn từ tên món
                    int quantity = Convert.ToInt32(row.Cells[1].Value); // Số lượng

                    // Kiểm tra món ăn đã tồn tại trong SelectedDishes chưa
                    if (donHang.CheckIfDishExists(idBan, idMonAn))
                    {
                        // Nếu món ăn đã tồn tại thì cập nhật số lượng
                        donHang.UpdateSelectedDishQuantity(idBan, idMonAn, quantity);
                    }
                    else
                    {
                        // Nếu món ăn chưa có thì thêm vào
                        if (!donHang.InsertSelectedDish(idBan, idMonAn, quantity))
                        {
                            MessageBox.Show("Có lỗi xảy ra khi lưu món ăn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            selectedDelete.Clear();
        }

        private int GetIdMonAnByName(string tenMonAn)
        {
            DataTable dt = monAn.TimKiemMonAn(tenMonAn); // Giả sử bạn đã có phương thức tìm kiếm món ăn theo tên
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["Id_MonAn"]); // Trả về ID của món ăn đầu tiên
            }
            return -1; // Nếu không tìm thấy
        }

        private void Print_Click(object sender, EventArgs e)
        {
            int idDonHang; // Biến để lưu IdDonHang

            RecalculateTotalAmount();
            // Lấy ID đơn hàng nếu đã có
            if (donHang.CheckIfOrderExists(idBan, out idDonHang))
            {
                DataTable donHangData = donHang.GetDonHangByIdDonHang(idDonHang);
                // Kiểm tra nếu dữ liệu không rỗng và có cột `TongTien`
                if (donHangData != null && donHangData.Rows.Count > 0)
                {
                    decimal tongTienDonHang = Convert.ToDecimal(donHangData.Rows[0]["TongTien"]); // Lấy giá trị `TongTien` từ DataTable

                    // So sánh `totalAmount` với `TongTien`
                    if (totalAmount != tongTienDonHang)
                    {
                        MessageBox.Show("Vui lòng lưu đơn hàng trước khi in hóa đơn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Dừng lại nếu tổng tiền không khớp
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin đơn hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Thêm từng món vào ChiTietDonHang
                foreach (DataGridViewRow row in guna2DataGridViewSelectedDishes.Rows)
                {
                    if (row.Cells[0].Value != null) // Kiểm tra ô Tên Món không null
                    {
                        int idMonAn = GetIdMonAnByName(row.Cells[0].Value.ToString());
                        int quantity = Convert.ToInt32(row.Cells[1].Value);
                        decimal thanhTien = quantity * Convert.ToDecimal(row.Cells[2].Value); // Tính thành tiền

                        // Thêm vào ChiTietDonHang
                        if (!chiTietDonHang.ThemChiTietDonHang(idDonHang, idMonAn, quantity, thanhTien))
                        {
                            MessageBox.Show("Có lỗi xảy ra khi lưu chi tiết đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                       // totalAmount += thanhTien; // Cộng dồn tổng tiền
                    }
                }

                // Xóa tất cả món ăn trong SelectedDishes theo IdBan
                if (donHang.DeleteAllSelectedDishesByTableId(idBan))
                {

                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa món ăn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật trạng thái đơn hàng thành "Hoàn thành"
                if (donHang.UpdateOrderStatus(idDonHang, "Hoan thanh"))
                {
                    MessageBox.Show("Trạng thái đơn hàng đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi cập nhật trạng thái đơn hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Mở ReceiptForm để hiển thị hóa đơn
                DataTable receiptData = donHang.GetReceiptData(idDonHang); // Lấy dữ liệu hóa đơn (giả sử bạn có phương thức này)
                if(guna2CustomRadioButton1.Checked) // Yes
                {
                    checkDungDiem = true;
                    donHang.CapNhatTongTienVaTienGiam(idDonHang);
                }
                else
                {
                    checkDungDiem=false;
                    
                }
                ReceiptForm receiptForm = new ReceiptForm(receiptData, checkDungDiem, idDonHang);
                this.Refresh();
                receiptForm.Show(); // Hiển thị hóa đơn
                guna2DataGridViewSelectedDishes.CellValueChanged += guna2DataGridViewSelectedDishes_CellValueChanged; // 
            }
            else
            {
                MessageBox.Show("Không có đơn hàng nào tồn tại cho bàn này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            guna2DataGridViewSelectedDishes.Rows.Clear();
            selectedDishes.Clear();
            selectedDelete.Clear();
            totalAmount = 0;
            UpdateTotalAmountLabel();

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

        private void lblTotalAmount_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridViewSelectedDishes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu nhấp vào cột "Thao Tác"
            if (e.RowIndex >= 0 && e.RowIndex < guna2DataGridViewSelectedDishes.Rows.Count &&
                e.ColumnIndex == guna2DataGridViewSelectedDishes.Columns["Thao Tác"].Index)
            {
                guna2DataGridViewSelectedDishes.SuspendLayout(); // Tạm dừng giao diện để tránh làm mới

                try
                {
                    var selectedRow = guna2DataGridViewSelectedDishes.Rows[e.RowIndex];

                    if (!selectedRow.IsNewRow)
                    {
                        int dishId = (int)selectedRow.Tag;

                        // Kiểm tra nếu món ăn này có trong danh sách đã chọn
                        if (selectedDishes.Contains(dishId))
                        {
                            selectedDishes.Remove(dishId);
                            selectedDelete.Add(dishId);
                            decimal totalPrice = Convert.ToDecimal(selectedRow.Cells[3].Value);
                            totalAmount -= totalPrice;

                            // Dùng BeginInvoke để trì hoãn xóa hàng
                            this.BeginInvoke(new Action(() =>
                            {
                                guna2DataGridViewSelectedDishes.Rows.RemoveAt(e.RowIndex);
                                UpdateTotalAmountLabel(); // Cập nhật lại tổng tiền
                            }));
                        }
                    }
                }
                finally
                {
                    guna2DataGridViewSelectedDishes.ResumeLayout(); // Kích hoạt lại giao diện sau khi hoàn tất
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public event EventHandler BackButtonClicked;
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Kích hoạt sự kiện BackButtonClicked khi nút Back được nhấn
            BackButtonClicked?.Invoke(this, EventArgs.Empty);

            // Đóng form con
            this.Close();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CustomRadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

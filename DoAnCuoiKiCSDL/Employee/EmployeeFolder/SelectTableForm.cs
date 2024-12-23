using DoAnCuoiKiCSDL.Class;
using DoAnCuoiKiCSDL.Employee;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.EmployeeFolder
{
    public partial class SelectTableForm : Form
    {
        DBConnection dbConnect = new DBConnection();
        private Ban ban;
        KhachHang khachhang = new KhachHang();
        public SelectTableForm()
        {
            InitializeComponent();
            ban = new Ban();
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.AutoSize = false;
        }
        private void LoadTableData()
        {

            try
            {
                dbConnect.OpenConnection();
                string query = "SELECT Id_Ban, So_Cho_Ngoi, Trang_Thai FROM Ban"; // Truy vấn lấy thông tin bàn
                SqlCommand cmd = new SqlCommand(query, dbConnect.GetConnection());
                SqlDataReader reader = cmd.ExecuteReader();

                // Xóa các control trước khi thêm mới (tránh trùng lặp khi load lại)
                tableLayoutPanel1.Controls.Clear();

                while (reader.Read())
                {
                    int idBan = reader.GetInt32(0); // Lấy Id_Ban
                    int soChoNgoi = reader.GetInt32(1); // Lấy So_Cho_Ngoi
                    string trangThai = reader.GetString(2); // Lấy trạng thái bàn

                    // Thêm từng bàn vào giao diện
                    AddTableToLayout(idBan, soChoNgoi, trangThai);
                }

                reader.Close();
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
        private void AddTableToLayout(int idBan, int soChoNgoi, string trangThai)
        {
            Guna2Button tableButton = new Guna2Button();
            //tableButton.Text = $"{idBan}\n{trangThai}\n {soChoNgoi} Chairs";
            tableButton.Name = "ban" + idBan; // Đặt tên cho button
            tableButton.Tag = idBan; // Lưu trữ số bàn
            tableButton.Width = 148; // Chiều rộng của nút
            tableButton.Height = 123; // Chiều cao của nút

            // Xóa thuộc tính Text, chúng ta sẽ vẽ thủ công
            tableButton.Text = string.Empty;

            // Đăng ký sự kiện Paint để vẽ văn bản tùy chỉnh
            tableButton.Paint += (sender, e) =>
            {
                Guna2Button btn = sender as Guna2Button;
                Graphics g = e.Graphics;

                // Định dạng phông chữ
                Font fontLarge = new Font("Arial", 16, FontStyle.Bold); // Chữ to cho idBan
                Font fontSmall = new Font("Arial", 10); // Chữ nhỏ cho trangThai và soChoNgoi

                // Kích thước của văn bản idBan
                SizeF idBanSize = g.MeasureString(idBan.ToString(), fontLarge);

                // Tính toán vị trí để idBan nằm giữa nút
                float idBanX = (tableButton.Width - idBanSize.Width) / 2; // Căn giữa theo chiều ngang
                float idBanY = 10; // Khoảng cách từ trên xuống (bạn có thể điều chỉnh)

                // Vẽ idBan với chữ to và căn giữa
                g.DrawString(idBan.ToString(), fontLarge, Brushes.Black, new PointF(idBanX, idBanY));

                // Kích thước của văn bản trangThai và soChoNgoi
                SizeF trangThaiSize = g.MeasureString($"{trangThai}\n{soChoNgoi} Chairs", fontSmall);

                // Tính toán vị trí để trangThai và soChoNgoi nằm ở dưới
                float trangThaiX = 10; // Đặt giá trị nhỏ để căn sát mép trái của Button (có thể điều chỉnh)
                float trangThaiY = idBanY + idBanSize.Height + 50; // Đặt vị trí dưới idBan, khoảng cách 5px

                // Vẽ trangThai và soChoNgoi với chữ nhỏ
                g.DrawString($"{trangThai} - {soChoNgoi} Chairs", fontSmall, Brushes.Black, new PointF(trangThaiX, trangThaiY));
                Image img = Resource.Resources.chair;
                int imageWidth = 50;
                int imageHeight = 50;
                float imageX = (tableButton.Width - imageWidth) / 2; // Căn giữa hình ảnh theo chiều ngang
                float imageY = idBanY + idBanSize.Height; // Đặt vị trí dưới idBan, khoảng cách 10px
                g.DrawImage(img, new RectangleF(imageX, imageY, imageWidth, imageHeight));
            };


            // Tùy chỉnh màu sắc dựa trên trạng thái của bàn
            if (trangThai == "Empty")
            {
                tableButton.FillColor = System.Drawing.Color.LightGreen; // Màu cho bàn trống
            }
            else
            {
                tableButton.FillColor = System.Drawing.Color.LightCoral; // Màu cho bàn đã đặt
            }

            // Tùy chỉnh font chữ và kiểu dáng của nút
            tableButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            tableButton.ForeColor = System.Drawing.Color.White;
            tableButton.BorderRadius = 10; // Bo góc nút
            tableButton.Margin = new Padding(5);
            // Thêm sự kiện click cho nút
            tableButton.Click += TableButton_Click;

            // Thêm nút vào TableLayoutPanel (giả sử bạn đã có TableLayoutPanel đặt tên là tableLayoutPanel1)
            tableLayoutPanel1.Controls.Add(tableButton);
        }
        private void TableButton_Click(object sender, EventArgs e)
        {
            string phoneNumber=txtPhoneNumber.Text;
            Guna2Button clickedButton = sender as Guna2Button;
            if (clickedButton != null)
            {
                int idBan = (int)clickedButton.Tag; // Lấy idBan từ nút được nhấn
                SelectDishesForm selectDishesForm = new SelectDishesForm(idBan,phoneNumber);
                selectDishesForm.TopLevel = false;
                selectDishesForm.FormBorderStyle = FormBorderStyle.None;
                selectDishesForm.Dock = DockStyle.Fill;

                // Đăng ký sự kiện BackButtonClicked
                selectDishesForm.BackButtonClicked += (s, args) =>
                {
                    // Khi nút Back được nhấn trong form con, chúng ta sẽ làm sạch nội dung panel
                    gunaPanelContainer1.Controls.Clear();

                    // Hiển thị lại form cha hoặc một form khác vào panel
                    SelectTableForm formCha = new SelectTableForm(); // Form cha hoặc một form khác
                    formCha.TopLevel = false;
                    formCha.FormBorderStyle = FormBorderStyle.None;
                    formCha.Dock = DockStyle.Fill;

                    // Thêm lại form cha vào panel
                    gunaPanelContainer1.Controls.Add(formCha);
                    formCha.Show();
                };

                // Thêm form con vào panel và hiển thị
                gunaPanelContainer1.Controls.Clear();
                gunaPanelContainer1.Controls.Add(selectDishesForm);
                selectDishesForm.Show();

                // Thêm form con vào panel và hiển thị
                /*gunaPanelContainer1.Controls.Clear();
                gunaPanelContainer1.Controls.Add(selectDishesForm);
                gunaPanelContainer1.Show();
                selectDishesForm.Show();
                */


                // Ẩn hoặc đóng form hiện tại nếu cần (tùy theo yêu cầu của bạn)
                //this.Hide(); // hoặc this.Close();
                /*ManageEmployee employeeForm = new ManageEmployee();
                employeeForm.TopLevel = false;
                employeeForm.FormBorderStyle = FormBorderStyle.None;
                employeeForm.Dock = DockStyle.Fill;

                // Đảm bảo kích thước của form con khớp với panel chứa
                employeeForm.Size = gunaPanelContainer.Size;

                gunaPanelContainer.Controls.Clear();
                gunaPanelContainer.Controls.Add(employeeForm);
                employeeForm.Show();*/
            }
        }


        private void SelectTableForm_Load(object sender, EventArgs e)
        {
            LoadTableData();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            LoadTableData();
        }

        private void gunaPanelContainer1_Paint(object sender, PaintEventArgs e)
        {

        }
        //public event EventHandler LogoutClicked;
        public event EventHandler logoutClicked;
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DBConnection.connectionString = "Data Source=DESKTOP-IVRVPSI\\TANGTHANH;Initial Catalog=QuanLiNhaHang1;Integrated Security=True";
            logoutClicked?.Invoke(this, EventArgs.Empty);


            this.Close();
           
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lấy số điện thoại từ TextBox
            string phoneNumber = txtPhoneNumber.Text;

            // Kiểm tra xem người dùng có nhập dữ liệu không
            if (string.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi hàm LayThongTinKhachhang để lấy thông tin khách hàng
            DataTable customerInfo = khachhang.LayThongTinKhachhang(phoneNumber);

            // Kiểm tra kết quả trả về
            if (customerInfo != null && customerInfo.Rows.Count > 0)
            {
                guna2TextBox4.Text = customerInfo.Rows[0]["Ho_Ten"].ToString();
                guna2TextBox6.Text = customerInfo.Rows[0]["DiemTichLuy"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin khách hàng với số điện thoại này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}

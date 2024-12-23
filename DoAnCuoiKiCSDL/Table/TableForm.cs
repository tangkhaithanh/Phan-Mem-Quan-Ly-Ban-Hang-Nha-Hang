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

namespace DoAnCuoiKiCSDL.Table
{
    public partial class TableForm : Form
    {
        private DBConnection dbConnect;
        private Ban ban;
        public TableForm()
        {
            InitializeComponent();
            dbConnect = new DBConnection();
            ban = new Ban();
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.AutoSize = false;
            this.Size = new Size(1720, 796);
        }
       

        private void ManageTable_Load(object sender, EventArgs e)
        {
            LoadTableData();
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
            tableButton.Width = 145; // Chiều rộng của nút
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
            Guna2Button clickedButton = sender as Guna2Button;
            if (clickedButton != null)
            {
                guna2Panel1.Visible = true;
                int idBan = (int)clickedButton.Tag;
                LoadTableDetails(idBan);
            }
        }

        private void LoadTableDetails(int idBan)
        {
            try
            {
                Ban ban = new Ban(); // Tạo đối tượng Ban
                DataTable dt = ban.LietKeBanTheoId(idBan); // Gọi hàm LietKeBanTheoId để lấy thông tin bàn

                if (dt.Rows.Count > 0)
                {
                    // Lấy thông tin từ DataTable và hiển thị trong các TextBox

                    txtTableNumber.Text = dt.Rows[0]["Id_Ban"].ToString();
                    txtNumberOfChairs.Text = dt.Rows[0]["So_Cho_Ngoi"].ToString();
  
                    // txtStatus.Text = dt.Rows[0]["Trang_Thai"].ToString(); // Nếu cần hiển thị trạng thái
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin bàn với Id_Ban: " + idBan);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int idBan = int.Parse(txtTableNumber.Text);
            int soChoNgoi = int.Parse(txtNumberOfChairs.Text);
            //string trangThai = txtStatus.Text;

            if (ban.SuaBan(idBan, soChoNgoi))
            {
                MessageBox.Show("Cập nhật thông tin bàn thành công!");
                LoadTableData();
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
           
        }

        private void Table_Load(object sender, EventArgs e)
        {
            LoadTableData();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SoChoNgoi.Text))
            {
                MessageBox.Show("Vui lòng nhập số chỗ ngồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng thực hiện nếu SoChoNgoi là null hoặc rỗng
            }
            int soChoNgoi = Convert.ToInt32(SoChoNgoi.Text);
            /*if (!int.TryParse(SoChoNgoi.Text, out soChoNgoi) || soChoNgoi <= 0)
            {
                MessageBox.Show("Vui lòng nhập số chỗ ngồi hợp lệ không âm và khác 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng thực hiện nếu SoChoNgoi không hợp lệ hoặc là số âm
            }*/

                // Chuyển đổi giá trị từ TextBox sang kiểu int
                /* if (int.TryParse(SoChoNgoi.Text, out soChoNgoi))
                 {*/
                // Gọi hàm thêm bàn nếu số lượng chỗ ngồi hợp lệ
                ban.ThemBan(soChoNgoi, "Empty");
                /*MessageBox.Show("Thêm bàn mới thành công!");*/
                LoadTableData(); // Tải lại dữ liệu bàn sau khi thêm mới
                
                /*else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thêm bàn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/
            
            /*else
            {
                MessageBox.Show("Vui lòng nhập số chỗ ngồi hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int idBan = int.Parse(txtTableNumber.Text);

            if (ban.XoaBan(idBan))
            {
                MessageBox.Show("Xóa bàn thành công!");
                LoadTableData();
            }
            else
            {
                MessageBox.Show("Xóa bàn không thành công");
            }
        }

        private void SoChoNgoi_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.Dishes
{
    public partial class EditDishForm : Form
    {
       
        private int idMonAn;
        private MonAn monAn;
        private byte[] hinhAnhData;
        DBConnection dbConnect = new DBConnection();
        public EditDishForm(int idMonAn)
        {
            InitializeComponent();
            this.idMonAn = idMonAn;
            txtDishID.Text= idMonAn.ToString();
            monAn = new MonAn(); // Khởi tạo đối tượng MonAn
            LoadCategoryData(); // Gọi hàm để tải danh mục
            LoadFoodDetails(); // Gọi hàm để tải thông tin món ăn
            

        }
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
        private void LoadCategoryData()
        {
            try
            {
                // Tương tự như trước, tải danh sách danh mục
                dbConnect.OpenConnection();
                string query = "SELECT Id_DanhMuc, Ten FROM DanhMuc"; // Lấy danh sách danh mục
                SqlCommand cmd = new SqlCommand(query, dbConnect.GetConnection());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Thiết lập ComboBox
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Ten"; // Tên hiển thị
                comboBox1.ValueMember = "Id_DanhMuc"; // Giá trị
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
       
        private void LoadFoodDetails()
        {
            // Sử dụng hàm TimKiemMonAnTheoId để lấy thông tin món ăn từ cơ sở dữ liệu
            DataTable foodDetails = monAn.TimKiemMonAnTheoId(idMonAn);
            if (foodDetails != null && foodDetails.Rows.Count > 0)
            {
                // Lấy thông tin từ DataTable và hiển thị trong các TextBox
                txtName.Text = foodDetails.Rows[0]["Ten"].ToString();
                txtDes.Text = foodDetails.Rows[0]["MoTa"].ToString();
                txtPrice.Text = foodDetails.Rows[0]["Gia"].ToString();

                // Lấy Id_DanhMuc và thiết lập giá trị cho ComboBox
                int idDanhMuc = Convert.ToInt32(foodDetails.Rows[0]["Id_DanhMuc"]);
                comboBox1.SelectedValue = idDanhMuc; // Thiết lập giá trị của ComboBox
                //comboBox1.Text = LoadCategoryName(idMonAn);
                hinhAnhData = foodDetails.Rows[0]["HinhAnh"] as byte[]; // Lấy hình ảnh từ cơ sở dữ liệu
                if (hinhAnhData != null)
                {
                    guna2PictureBox1.Image = ByteArrayToImage(hinhAnhData);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy món ăn với Id_MonAn: " + idMonAn);
            }
        }

        private void uploadBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Đọc hình ảnh từ tệp
                    hinhAnhData = File.ReadAllBytes(openFileDialog.FileName);
                    guna2PictureBox1.Image = Image.FromFile(openFileDialog.FileName); // Hiển thị hình ảnh lên PictureBox
                }
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các TextBox và ComboBox
            int idDanhMuc = (int)comboBox1.SelectedValue;
            string ten = txtName.Text.Trim();
            string moTa = txtDes.Text.Trim();
            decimal gia;

            if (!decimal.TryParse(txtPrice.Text.Trim(), out gia))
            {
                MessageBox.Show("Giá không hợp lệ.");
                return;
            }
            
            // Sử dụng hàm SuaMonAn để cập nhật thông tin món ăn
            if (monAn.SuaMonAn(idMonAn, idDanhMuc, ten, moTa, gia, hinhAnhData))
            {
                MessageBox.Show("Cập nhật món ăn thành công!");
                
                this.Close(); // Đóng form sau khi cập nhật
            }
            else
            {
                MessageBox.Show("Cập nhật món ăn thất bại.");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Hiện thông báo xác nhận trước khi xóa
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa món ăn này không?", "Xác nhận xóa", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                // Gọi hàm xóa từ lớp MonAn
                if (monAn.XoaMonAn(idMonAn)) // Sử dụng hàm đã có để xóa
                {
                    MessageBox.Show("Xóa món ăn thành công!");
                    this.Close(); // Đóng form sau khi xóa
                }
                else
                {
                    MessageBox.Show("Xóa món ăn thất bại.");
                }
            }
        }

        private void EditDishForm_Load(object sender, EventArgs e)
        {

        }
    }
}

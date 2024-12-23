using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.Dishes
{
    public partial class AddDishesForm : Form
    {
        public AddDishesForm()
        {
            InitializeComponent();
        }
        DBConnection dbConnect= new DBConnection();
        MonAn dishes= new MonAn();  
        private void LoadtoCombobox(ComboBox comboBox)
        {
            try
            {
                dbConnect.OpenConnection();
                string query = "SELECT * FROM DanhMuc";
                SqlCommand cmd = new SqlCommand(query, dbConnect.GetConnection());
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataTable);
                if(dataTable.Rows.Count > 0 )
                {
                    comboBox.Items.Clear();
                    comboBox.DataSource = dataTable;
                    comboBox.DisplayMember = "Ten";
                    comboBox.ValueMember = "Id_DanhMuc";
                    comboBox.SelectedValue= dataTable.Rows[0]["Id_DanhMuc"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }
        private void AddDishesForm_Load(object sender, EventArgs e)
        {
            LoadtoCombobox(comboBox1);
        }
        public byte[] ImageToByteArray(Image image)
        {
            if (image == null)
            {
                return null; 
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); 
                return ms.ToArray(); 
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng nhập tên món ăn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string name = txtName.Text.Trim();
                if (string.IsNullOrEmpty(txtDes.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng nhập mô tả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string description = txtDes.Text.Trim();
                if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng nhập giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng thực hiện nếu SoChoNgoi là null hoặc rỗng
                }
                if (decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
                {
                    //price = Convert.ToDecimal(txtPrice.Text.Trim());
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập giá hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Image image = null;
                if (!(guna2PictureBox1.Image is null))
                {
                    image = guna2PictureBox1.Image;
                }
                else
                {
                    MessageBox.Show("Không được để trống hình ảnh. ");
                    return;
                }

                byte[] convertedImage = ImageToByteArray(image);
                int idCategory = int.Parse(comboBox1.SelectedValue.ToString().Trim());
                if (dishes.ThemMonAn(idCategory, name, description, price, convertedImage))
                {
                    MessageBox.Show("Thêm món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thêm món ăn không thành công. Vui lòng kiểm tra lại thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Định dạng không hợp lệ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show($"Thông tin không được để trống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void uploadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Chọn hình ảnh"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    Image selectedImage = Image.FromFile(filePath);
                    guna2PictureBox1.Image = selectedImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi khi tải hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void labelPrice_Click(object sender, EventArgs e)
        {

        }
    }
}

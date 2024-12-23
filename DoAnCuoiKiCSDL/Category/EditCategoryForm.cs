using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.Category
{
    public partial class EditCategoryForm : Form
    {
        private int idDanhMuc;
        private DanhMuc danhMuc = new DanhMuc();
        public EditCategoryForm(int id)
        {
            InitializeComponent();
            this.idDanhMuc = id;
        }

        private void EditCategoryForm_Load(object sender, EventArgs e)
        {
            txtID.Text = idDanhMuc.ToString();
            LoadCategoryDetails();
        }
        private void LoadCategoryDetails()
        {
            DataTable dt = danhMuc.TimKiemDanhMucTheoID(idDanhMuc);

            DataRow categoryRow = dt.AsEnumerable().FirstOrDefault(row => row.Field<int>("Id_DanhMuc") == idDanhMuc);
            if (categoryRow != null)
            {
                txtName.Text = categoryRow["Ten"].ToString();
                guna2TextBox1.Text = categoryRow["MoTa"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy danh mục.");
                this.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string tenDanhMuc = txtName.Text.Trim();
            string moTa = guna2TextBox1.Text.Trim();

            if (string.IsNullOrEmpty(tenDanhMuc))
            {
                MessageBox.Show("Tên danh mục không được để trống.");
                return;
            }
            if (string.IsNullOrEmpty(moTa))
            {
                MessageBox.Show("Mô tả danh mục không được để trống.");
                return;
            }

            if (danhMuc.SuaDanhMuc(idDanhMuc, tenDanhMuc, moTa))
            {
                MessageBox.Show("Cập nhật danh mục thành công!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật danh mục thất bại. Vui lòng thử lại.");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này?",
                                                 "Xác nhận xóa",
                                                 MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (danhMuc.XoaDanhMuc(idDanhMuc))
                {
                    MessageBox.Show("Xóa danh mục thành công!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Xóa danh mục thất bại. Vui lòng thử lại.");
                }
            }
        }
    }
}

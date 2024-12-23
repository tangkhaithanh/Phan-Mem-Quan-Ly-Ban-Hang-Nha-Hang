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
    public partial class AddCategoryForm : Form
    {
        public AddCategoryForm()
        {
            InitializeComponent();
        }
        DanhMuc danhMuc = new DanhMuc();
        private void AddCategoryForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
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
            if (danhMuc.ThemDanhMuc(tenDanhMuc, moTa))
            {
                MessageBox.Show("Thêm danh mục thành công!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại. Vui lòng thử lại.");
            }
        }
    }
}

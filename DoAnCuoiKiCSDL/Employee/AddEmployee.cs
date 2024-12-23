using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.Employee
{
    public partial class AddEmployee : Form
    {
        User user = new User();
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            // Lấy thông tin từ các TextBox
            string hoTen = txtHoTen.Text.Trim();
            //string chucVu = guna2ComboBox1.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(sdt) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            if (user.ThemNhanVien(hoTen, "Employee", sdt, matKhau))
            {
                user.TaoTaiKhoanNhanVienTrenSQL(sdt, matKhau);
                MessageBox.Show("Thêm nhân viên thành công!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm nhân viên thất bại. Vui lòng thử lại.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            txtMatKhau.Text = user.TaoMatKhauManh().ToString().Trim();
        }
    }
}

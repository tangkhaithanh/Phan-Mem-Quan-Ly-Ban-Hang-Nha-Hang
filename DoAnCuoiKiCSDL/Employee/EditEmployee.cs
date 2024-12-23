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
    public partial class EditEmployee : Form
    {
        private User user;
        private int idNhanVien;
        private string oldSdt;
        public EditEmployee(int idNhanVien)
        {
            InitializeComponent();
            this.idNhanVien = idNhanVien;
            txtId.Text = idNhanVien.ToString();
            user = new User();
            LoadEmployeeData();
            oldSdt = txtSDT.Text;
        }
        private void LoadEmployeeData()
        {
            // Tải thông tin nhân viên từ cơ sở dữ liệu
            DataTable dt = user.TimKiemNhanVienTheoID(idNhanVien);
            if (dt.Rows.Count > 0)
            {
                txtHoTen.Text = dt.Rows[0]["Ho_Ten"].ToString();
                txtPassword.Text = dt.Rows[0]["MatKhau"].ToString().Trim();
                //guna2ComboBox1.Text = dt.Rows[0]["Chuc_Vu"].ToString();
                txtSDT.Text = dt.Rows[0]["SDT"].ToString();
            }
        }
        private void EditEmployee_Load(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Gọi hàm sửa nhân viên
            if (user.SuaNhanVien(idNhanVien, txtHoTen.Text, "Employee", txtSDT.Text, txtPassword.Text)) // Password sẽ cần quản lý thêm
            {
                user.CapNhatTaiKhoanNhanVienTrenSQL(oldSdt, txtSDT.Text, txtPassword.Text);
                MessageBox.Show("Cập nhật thành công.");
                //this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }
    }
}

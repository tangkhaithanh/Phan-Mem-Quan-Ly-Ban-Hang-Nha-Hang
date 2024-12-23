using DoAnCuoiKiCSDL.Admin;
using DoAnCuoiKiCSDL.EmployeeFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            // Đặt kích thước form bằng cách gán một đối tượng Size
            this.Size = new Size(1250, 800); // Chiều rộng: 800, Chiều cao: 600
            
        }
        User user= new User();
        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string username = TextBoxUserName.Text;
            string password= TextBoxPassword.Text;
            DataTable dt= new DataTable();  
            dt= user.DangNhap(username,password);
            if(dt.Rows.Count > 0 )
            {
                DataRow dr= dt.Rows[0];
                string postion= dr["Chuc_Vu"].ToString();
                Global.username = dr["Ho_Ten"].ToString();
                if(postion=="Admin")
                {
                   AdminHome adminForm= new AdminHome();
                   adminForm.TopLevel = false;
                   adminForm.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền và thanh tiêu đề của Form2
                   adminForm.Dock = DockStyle.Fill; // Để Form2 chiếm toàn bộ gunaPanel
                   adminForm.logoutClicked += (s, args) =>
                   {
                       // Khi nút Back được nhấn trong form con, chúng ta sẽ làm sạch nội dung panel
                       guna2Panel1.Controls.Clear();

                        // Hiển thị lại form cha hoặc một form khác vào panel
                        Login formCha = new Login(); // Form cha hoặc một form khác
                        formCha.TopLevel = false;
                        formCha.FormBorderStyle = FormBorderStyle.None;
                        formCha.Dock = DockStyle.Fill;

                       // Thêm lại form cha vào panel
                        guna2Panel1.Controls.Add(formCha);
                        formCha.Show();
                   };
                   guna2Panel1.Controls.Clear();
                   guna2Panel1.Controls.Add(adminForm);
                   adminForm.Show();
                }
                else if(postion=="Employee")
                {
                    DBConnection.connectionString="Data Source=DESKTOP-IVRVPSI\\TANGTHANH;Initial Catalog=QuanLiNhaHang1;User Id=user" + username + ";Password=" + password + ";";
                    
                    SelectTableForm a = new SelectTableForm();
                    a.TopLevel = false;
                    a.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền và thanh tiêu đề của Form2
                    a.Dock = DockStyle.Fill; // Để Form2 chiếm toàn bộ gunaPanel
                    a.logoutClicked += (s, args) =>
                    {
                        Login login = new Login();
                        login.Show();
                        this.Hide();
                    };
                    guna2Panel1.Controls.Clear();
                    guna2Panel1.Controls.Add(a);
                    Global.id = int.Parse(dr["Id_NhanVien"].ToString());
                    guna2Panel1.Refresh(); // Làm mới giao diện
                    a.Show();
                }
            }
            else
            {
                MessageBox.Show("Incorrect phone number or password. Please try again.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}

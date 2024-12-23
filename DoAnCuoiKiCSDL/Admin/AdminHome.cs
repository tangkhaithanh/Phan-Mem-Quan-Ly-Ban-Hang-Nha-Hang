using DoAnCuoiKiCSDL.Analyst;
using DoAnCuoiKiCSDL.Employee;
using DoAnCuoiKiCSDL.Order;
using DoAnCuoiKiCSDL.Table;
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

namespace DoAnCuoiKiCSDL.Admin
{
    public partial class AdminHome : Form
    {
        public AdminHome()
        {
            InitializeComponent();
        }
        private void ResetButtonStyles()
        {
            // Reset lại tất cả các nút về trạng thái không được chọn
            btnHome.BorderThickness = 0;
            btnEmployee.BorderThickness = 0;
            btnDishes.BorderThickness = 0;
            btnTable.BorderThickness = 0;

            btnHome.FillColor = Color.Transparent;
            btnEmployee.FillColor = Color.Transparent;
            btnDishes.FillColor = Color.Transparent;
            btnTable.FillColor = Color.Transparent;

            btnHome.Image = Resource.Resources.home;
            btnEmployee.Image = Resource.Resources.employee;
            btnDishes.Image = Resource.Resources.restaurant;
            btnTable.Image = Resource.Resources.chair;
            btnReceipt.Image = Resource.Resources.receipt;
            // Chuyển đổi byte[] từ tài nguyên thành System.Drawing.Image
            byte[] imageBytes = Resource.Resources.analyst;
            Image image;

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                image = Image.FromStream(ms); // Chuyển đổi từ byte[] thành System.Drawing.Image
            }

            // Gán ảnh đã chuyển đổi cho btnAnalyst
            btnAnalyst.Image = image;

        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            ResetButtonStyles(); ;  // Reset lại tất cả các nút và biểu tượng về trạng thái bình thường
            highlightPanel.Height = btnHome.Height;
            highlightPanel.Width = 2;
            highlightPanel.Top = btnHome.Top;
            highlightPanel.Visible = true;
            // Thay đổi màu và biểu tượng của nút đang được chọn
            btnHome.Image = Resource.Resources.home_filled_colored_darker_blue;
            // chuyển form:
            HomeAdmin adminform = new HomeAdmin();

            // Đặt Form2 vào trong gunaPanelContainer
            adminform.TopLevel = false; // Để Form2 không mở cửa sổ riêng mà hiển thị trong panel
            adminform.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền và thanh tiêu đề của Form2
            adminform.Dock = DockStyle.Fill; // Để Form2 chiếm toàn bộ gunaPanel
            gunaPanelContainer.Controls.Clear(); // Xóa mọi control trong gunaPanel trước
            gunaPanelContainer.Controls.Add(adminform); // Thêm Form2 vào gunaPanel
            adminform.Show(); // Hiển thị Form2

        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            highlightPanel.Height = btnEmployee.Height;
            highlightPanel.Top = btnEmployee.Top;
            highlightPanel.Width = 2;
            highlightPanel.Visible = true;
            btnEmployee.Image = Resource.Resources.employee_filled_colored_darker_blue;

            // Chuyển form:
            ManageEmployee employeeForm = new ManageEmployee();
            employeeForm.TopLevel = false;
            employeeForm.FormBorderStyle = FormBorderStyle.None;
            employeeForm.Dock = DockStyle.Fill;

            // Đảm bảo kích thước của form con khớp với panel chứa
            employeeForm.Size = gunaPanelContainer.Size;

            gunaPanelContainer.Controls.Clear();
            gunaPanelContainer.Controls.Add(employeeForm);
            employeeForm.Show();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            
        }

        private void btnDishes_Click(object sender, EventArgs e)
        {
            highlightPanel.Height = btnDishes.Height;
            highlightPanel.Top = btnDishes.Top;
            highlightPanel.Width = 2;
            highlightPanel.Visible = true;
            ResetButtonStyles();
            btnDishes.Image = Resource.Resources.restaurant_filled_colored_darker_blue;

            // chuyển form:
            // Tạo một instance của ManageFood
            ManageFood foodForm = new ManageFood();

            // Đảm bảo form con không phải là cửa sổ top-level
            foodForm.TopLevel = false;

            // Thiết lập FormBorderStyle của form con thành None để loại bỏ viền
            foodForm.FormBorderStyle = FormBorderStyle.None;

            // Thiết lập Dock cho form con để lấp đầy Panel chứa
            foodForm.Dock = DockStyle.Fill;

            // Xóa các điều khiển hiện có trong Panel trước khi thêm form mới
            gunaPanelContainer.Controls.Clear();
            gunaPanelContainer.Size= foodForm.Size ;
            // Thêm form con vào gunaPanelContainer
            gunaPanelContainer.Controls.Add(foodForm);
           // gunaPanelContainer.Size = new Size(1400, 1200); // Đặt kích thước của Panel là rộng 800 và cao 600
            //this.Size = new Size(1400, 1222);

            // Hiển thị form con
            foodForm.Show();

        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            highlightPanel.Height = btnTable.Height;
            highlightPanel.Top = btnTable.Top;
            highlightPanel.Width = 2;
            highlightPanel.Visible = true;
            ResetButtonStyles();
            btnTable.Image = Resource.Resources.chair_filled_colored_darker_blue;
            // Chuyển form:
            TableForm tableform = new TableForm();

            // Đặt Form2 vào trong gunaPanelContainer
            tableform.TopLevel = false; // Để Form2 không mở cửa sổ riêng mà hiển thị trong panel
            tableform.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền và thanh tiêu đề của Form2
            tableform.Dock = DockStyle.Fill; // Để Form2 chiếm toàn bộ gunaPanel
            gunaPanelContainer.Controls.Clear(); // Xóa mọi control trong gunaPanel trước
            gunaPanelContainer.Controls.Add(tableform); // Thêm Form2 vào gunaPanel
            tableform.Show(); // Hiển thị Form2
        }

        private void labelHomePage_Click(object sender, EventArgs e)
        {

        }

        private void btnHomePage_Click(object sender, EventArgs e)
        {
            highlightPanel2.Width = btnHomePage.Width;      // Đặt chiều rộng của highlightPanel2 bằng với btnTable
            highlightPanel2.Left = btnHomePage.Left;        // Căn ngang theo vị trí của btnTable
            highlightPanel2.Top = btnHomePage.Bottom;       // Đặt highlightPanel2 ngay dưới btnTable
            highlightPanel2.Height = 2;                  // Đặt chiều cao của highlightPanel2
            highlightPanel2.Visible = true;
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            ResetButtonStyles();
            highlightPanel.Height = btnReceipt.Height;
            highlightPanel.Top = btnReceipt.Top;
            highlightPanel.Width = 2;
            highlightPanel.Visible = true;
            btnReceipt.Image = Resource.Resources.updated_receipt_blue_image_no_bg;

            ManageOrder Orderform = new ManageOrder();

            // Đặt Form2 vào trong gunaPanelContainer
            Orderform.TopLevel = false; // Để Form2 không mở cửa sổ riêng mà hiển thị trong panel
            Orderform.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền và thanh tiêu đề của Form2
            Orderform.Dock = DockStyle.Fill; // Để Form2 chiếm toàn bộ gunaPanel
            gunaPanelContainer.Controls.Clear(); // Xóa mọi control trong gunaPanel trước
            gunaPanelContainer.Controls.Add(Orderform); // Thêm Form2 vào gunaPanel
            Orderform.Show(); // Hiển thị Form2

        }
        public event EventHandler logoutClicked;
        private void btnLogout_Click(object sender, EventArgs e)
        {
            logoutClicked?.Invoke(this, EventArgs.Empty);

            // Đóng form con
            this.Close();
        }

        private void btnAnalyst_Click(object sender, EventArgs e)
        {
            byte[] imageBytes = Resource.Resources.image_corrected_dark_blue;
            System.Drawing.Image image; // Sử dụng System.Drawing.Image để tránh mơ hồ

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                image = System.Drawing.Image.FromStream(ms); // Chuyển đổi từ byte[] thành System.Drawing.Image
            }
            ResetButtonStyles();
            highlightPanel.Height = btnAnalyst.Height;
            highlightPanel.Top = btnAnalyst.Top;
            highlightPanel.Width = 2;
            highlightPanel.Visible = true;
            btnAnalyst.Image = image;

            AnalystForm analystForm = new AnalystForm();

            // Đặt Form2 vào trong gunaPanelContainer
            analystForm.TopLevel = false; // Để Form2 không mở cửa sổ riêng mà hiển thị trong panel
            analystForm.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền và thanh tiêu đề của Form2
            analystForm.Dock = DockStyle.Fill; // Để Form2 chiếm toàn bộ gunaPanel
            gunaPanelContainer.Controls.Clear(); // Xóa mọi control trong gunaPanel trước
            gunaPanelContainer.Controls.Add(analystForm); // Thêm Form2 vào gunaPanel
            analystForm.Show(); // Hiển thị Form2
        }
    }
}

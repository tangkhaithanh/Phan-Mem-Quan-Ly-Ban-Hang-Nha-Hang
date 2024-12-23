using DoAnCuoiKiCSDL.Class;
using Guna.UI2.WinForms;
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
    public partial class HomeAdmin : Form
    {

        DonHang donHang = new DonHang();
        MonAn monAn = new MonAn();
        User nhanVien = new User();
        Ban ban = new Ban();

        public HomeAdmin()
        {
            InitializeComponent();
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Size = new Size(1800, 1500);
        }

        private void HomeAdmin_Load(object sender, EventArgs e)
        {
            LoadStatistics();
            LoadBillsToday();
            LoadTopSellingProducts();
            // Thiết lập tiêu đề cho DataGridView2 (bills today)
            DataGridView2.Columns["Ho_Ten"].HeaderText = "Name's Employee";
            DataGridView2.Columns["SDT_KhachHang"].HeaderText = "Phone number";
            DataGridView2.Columns["PhuongThucThanhToan"].HeaderText = "Payment method";
            DataGridView2.Columns["NgayGio"].HeaderText = "Time";
            DataGridView2.Columns["TongTien"].HeaderText = "Total";
            DataGridView2.Columns["TrangThai"].HeaderText = "Status";
            DataGridView2.Columns["Id_DonHang"].HeaderText = "Order ID";
            DataGridView2.Columns["Id_Ban"].Visible = false;
            DataGridView2.Columns["SDT_KhachHang"].Visible = false;
            DataGridView2.AllowUserToAddRows = false;
            DataGridView2.ReadOnly = true;
            // Thiết lập tiêu đề cho dataGridView1 (Top-selling product)
            dataGridView1.Columns["HinhAnh"].HeaderText = "Image";
            dataGridView1.Columns["Id_MonAn"].HeaderText = "Food ID";
            dataGridView1.Columns["Ten_Mon_An"].HeaderText = "Name";
            dataGridView1.Columns["SoLuongDaOrder"].HeaderText = "Quantity";
            dataGridView1.Columns["Gia"].HeaderText = "Price";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            // Thiết kế DataGridView2
            foreach (DataGridViewColumn column in DataGridView2.Columns)
            {
                // Màu nền và chữ cho tiêu đề cột
                column.HeaderCell.Style.BackColor = Color.DodgerBlue;
                column.HeaderCell.Style.ForeColor = Color.White;
                column.HeaderCell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                column.HeaderCell.Style.Padding = new Padding(5); // Thêm Padding cho Header

                // Căn giữa tiêu đề và dữ liệu trong cột
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Padding = new Padding(5); // Thêm Padding cho Cell Data
            }

            // Điều chỉnh chiều rộng của các cột và chiều cao dòng cho DataGridView2
            DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridView2.AutoResizeColumns();
            DataGridView2.RowTemplate.Height = 30;
            DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            DataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Thiết kế dataGridView1
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                // Màu nền và chữ cho tiêu đề cột
                column.HeaderCell.Style.BackColor = Color.DodgerBlue;
                column.HeaderCell.Style.ForeColor = Color.White;
                column.HeaderCell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                column.HeaderCell.Style.Padding = new Padding(1); // Thêm Padding cho Header

                // Căn giữa tiêu đề và dữ liệu trong cột
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Padding = new Padding(1); // Thêm Padding cho Cell Data
            }

            // Điều chỉnh chiều rộng của các cột và chiều cao dòng cho dataGridView1
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoResizeColumns();
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            //dataGridView1.Columns["Quantity"].Width = 100; // Điều chỉnh giá trị 100 theo kích thước bạn muốn
            // Tạo độ cong cho các panel:
            CreateRoundedBorderPanel(guna2Panel3, 10);
            CreateRoundedBorderPanel(guna2Panel4,10);
            CreateRoundedBorderPanel(guna2Panel5,10);
            CreateRoundedBorderPanel(guna2Panel6,10);
            CreateRoundedBorderPanel(guna2Panel7, 10);

        }
        private void CreateRoundedBorderPanel(Guna.UI2.WinForms.Guna2Panel panel, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            panel.CustomizableEdges.BottomLeft = true;
            panel.CustomizableEdges.BottomRight = true;
            panel.CustomizableEdges.TopLeft = true;
            panel.CustomizableEdges.TopRight = true;
            panel.Region = new Region(path);
        }

        private void LoadStatistics()
        {
            // Hiển thị tổng doanh thu
            decimal totalRevenue = donHang.TinhTongTienDoanhThu(); // Bạn cần tạo thêm hàm tính tổng doanh thu
            lblTotalRevenue.Text = totalRevenue.ToString("N0") + " VND";

            // Hiển thị tổng số nhân viên
            DataTable employees = nhanVien.LayDanhSachNhanVien(); // Sử dụng hàm tìm kiếm nhân viên, có thể để trống các tham số để lấy tất cả
            lblEmployees.Text = employees.Rows.Count.ToString();

            // Hiển thị tổng số đơn hàng
            int bills = donHang.TongSoLuongDonHang();// Lọc đơn hàng theo ngày hiện tại
            lblBills.Text = bills.ToString();

            // Hiển thị tổng số món ăn
            Int32 dishes = monAn.LayTongMonAn();
            lblDishes.Text = dishes.ToString();

            // Hiển thị tổng số bàn
            Int32 tables = ban.LayTongSoBan(); // Lấy tất cả bàn
            lblTables.Text = tables.ToString();
        }

        private void LoadBillsToday()
        {
            DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // Hiển thị danh sách hóa đơn trong ngày hôm nay
            DataTable billsToday = donHang.LocDonHangTheoNgay(DateTime.Now.ToString("dd/MM/yyyy"));

            if (billsToday != null)
            {
                DataGridView2.DataSource = billsToday; // Thiết lập nguồn dữ liệu cho DataGridView
            }
            else
            {
                MessageBox.Show("Không có món nào bán chạy trong ngày đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Giả sử bạn đã thêm một DataGridView tên là dataGridView1 vào form.
        private void LoadTopSellingProducts()
        {
            DataTable topSellingProducts = monAn.ThongKeMonAnBanChay();

            if (topSellingProducts != null && topSellingProducts.Rows.Count > 0)
            {
                // Sắp xếp theo SoLuongDaOrder từ lớn đến bé
                DataView sortedView = topSellingProducts.DefaultView;
                sortedView.Sort = "SoLuongDaOrder DESC"; // Sắp xếp giảm dần
                DataTable sortedTable = sortedView.ToTable();

                // Giới hạn chỉ hiển thị 10 hàng
                if (sortedTable.Rows.Count > 10)
                {
                    sortedTable = sortedTable.AsEnumerable().Take(10).CopyToDataTable();
                }
                dataGridView1.DataSource = sortedTable;

                // Điều chỉnh cột HinhAnh để hiển thị vừa vặn
                dataGridView1.Columns["HinhAnh"].Width = 150; // Đặt chiều rộng cột HinhAnh
                dataGridView1.RowTemplate.Height = 150;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["HinhAnh"].Value != null && row.Cells["HinhAnh"].Value is byte[])
                    {
                        byte[] imgData = (byte[])row.Cells["HinhAnh"].Value;
                        using (MemoryStream ms = new MemoryStream(imgData))
                        {
                            Image img = Image.FromStream(ms);
                            // Điều chỉnh kích thước hình ảnh
                            Image resizedImg = new Bitmap(img, new Size(200, 100)); // Thay đổi kích thước cho vừa với khung
                            row.Cells["HinhAnh"].Value = resizedImg; // Gán hình ảnh vào cột HinhAnh
                        }
                    }
                    else
                    {
                        //row.Cells["HinhAnh"].Value = Properties.Resources.NoImage; // Gán hình ảnh mặc định nếu không có
                    }
                }
                // Hiển thị hình ảnh trong DataGridView
                //dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            }
            else
            {
                MessageBox.Show("Không có món nào bán chạy trong ngày đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblBills_Click(object sender, EventArgs e)
        {

        }
        

    }
}

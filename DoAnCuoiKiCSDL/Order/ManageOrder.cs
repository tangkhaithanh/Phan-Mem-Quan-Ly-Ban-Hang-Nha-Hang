using DoAnCuoiKiCSDL.Class;
using DoAnCuoiKiCSDL.OrderDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.Order
{
    public partial class ManageOrder : Form
    {
        DonHang donHang = new DonHang();    
        public ManageOrder()
        {
            InitializeComponent();
            comboBoxTimeFilter.Items.Add("Ngày");
            comboBoxTimeFilter.Items.Add("Tháng");
            comboBoxTimeFilter.Items.Add("Năm");
            comboBoxTimeFilter.SelectedIndex = 0; // Mặc định chọn "Ngày"
            guna2DataGridViewOrders.BorderStyle = BorderStyle.None;
           /* guna2DataGridViewOrders.BackgroundColor = Color.LightGray; // Đặt màu nền
            guna2DataGridViewOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkBlue; // Đặt màu tiêu đề
            guna2DataGridViewOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Đặt màu chữ tiêu đề
            guna2DataGridViewOrders.RowsDefaultCellStyle.BackColor = Color.White; // Đặt màu hàng
            guna2DataGridViewOrders.RowsDefaultCellStyle.ForeColor = Color.Black; // Đặt màu chữ hàng*/
            CreateRoundedBorderPanel(guna2Panel2, 20);
            CreateRoundedBorderPanel(guna2Panel3 ,20);
            ApplyRoundedCorners(guna2Panel1,10);
            //ApplyRoundedCorners(guna2DataGridViewOrders, 20);
        }
        // Tạo độ cong cho panel:
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
        private void ManageOrder_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            UpdateDateTimePickerFormat();
        }

        private void comboBoxTimeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDateTimePickerFormat();
        }
        private void UpdateDateTimePickerFormat()
        {
            switch (comboBoxTimeFilter.SelectedItem.ToString())
            {
                case "Ngày":
                    guna2DateTimePicker1.ShowUpDown = false; // Tắt chế độ chọn lên/xuống
                    guna2DateTimePicker1.CustomFormat = ""; // Reset CustomFormat để loại bỏ định dạng trước
                    guna2DateTimePicker1.Format = DateTimePickerFormat.Long; // Hiển thị ngày, tháng, năm
                    guna2DateTimePicker1.Refresh(); // Cập nhật lại DateTimePicker
                    break;
                case "Tháng":
                    guna2DateTimePicker1.ShowUpDown = true; // Bật chế độ chọn tháng
                    guna2DateTimePicker1.Format = DateTimePickerFormat.Custom;
                    guna2DateTimePicker1.CustomFormat = "MM/yyyy"; // Hiển thị chỉ tháng và năm
                    guna2DateTimePicker1.Refresh(); // Cập nhật lại DateTimePicker
                    break;
                case "Năm":
                    guna2DateTimePicker1.ShowUpDown = true; // Bật chế độ chọn năm
                    guna2DateTimePicker1.Format = DateTimePickerFormat.Custom;
                    guna2DateTimePicker1.CustomFormat = "yyyy"; // Chỉ hiển thị năm
                    guna2DateTimePicker1.Refresh(); // Cập nhật lại DateTimePicker
                    break;
            }
        }
        private void SetupDataGridView()
        {
            // Thiết lập các cột cho Guna2DataGridView
            guna2DataGridViewOrders.ColumnCount = 8;

            guna2DataGridViewOrders.Columns[0].Name = "Số Đơn Hàng";
            guna2DataGridViewOrders.Columns[1].Name = "Họ tên Nhân Viên";
            guna2DataGridViewOrders.Columns[2].Name = "Phương Thức Thanh Toán";
            guna2DataGridViewOrders.Columns[3].Name = "Số Bàn";
            guna2DataGridViewOrders.Columns[4].Name = "Thời Gian";
            guna2DataGridViewOrders.Columns[5].Name = "Tổng Tiền";
            guna2DataGridViewOrders.Columns[6].Name = "Trạng Thái";
            guna2DataGridViewOrders.ReadOnly = true;

            // Định dạng cột
            guna2DataGridViewOrders.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"; // Định dạng thời gian
            guna2DataGridViewOrders.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            guna2DataGridViewOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            foreach (DataGridViewColumn column in guna2DataGridViewOrders.Columns)
            {
                column.HeaderCell.Style.BackColor = Color.DodgerBlue;
                column.HeaderCell.Style.ForeColor = Color.White;
                column.HeaderCell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                column.HeaderCell.Style.Padding = new Padding(5); // Thêm Padding cho Header
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Padding = new Padding(5);
            }

        }
        private void DisplayOrders(DataTable orders)
        {
            // Xóa dữ liệu cũ
            guna2DataGridViewOrders.Rows.Clear();

            foreach (DataRow row in orders.Rows)
            {
                // Thêm hàng vào Guna2DataGridView
                guna2DataGridViewOrders.Rows.Add(
                    row["Id_DonHang"].ToString(),
                    row["Ho_Ten"].ToString(),
                    row["PhuongThucThanhToan"].ToString(),
                    row["Id_Ban"].ToString(),
                    Convert.ToDateTime(row["NgayGio"]).ToString("dd/MM/yyyy HH:mm"),
                    row["TongTien"].ToString() + " VND",
                    row["TrangThai"].ToString()
                );
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string selectedDate;

            // Kiểm tra lựa chọn trong comboBoxTimeFilter
            if (comboBoxTimeFilter.SelectedItem.ToString() == "Ngày")
            {
                selectedDate = guna2DateTimePicker1.Value.ToString("dd/MM/yyyy");
            }
            else if (comboBoxTimeFilter.SelectedItem.ToString() == "Tháng")
            {
                selectedDate = guna2DateTimePicker1.Value.ToString("MM/yyyy");
            }
            else // Năm
            {
                selectedDate = guna2DateTimePicker1.Value.ToString("yyyy");
            }
            DataTable orders = donHang.LocDonHangTheoNgay(selectedDate); // Lấy dữ liệu đơn hàng theo ngày

            // Nếu orders là null, hiển thị thông báo và kết thúc phương thức
            if (orders == null)
            {
                guna2DataGridViewOrders.Rows.Clear();
                MessageBox.Show("Không tìm thấy đơn hàng cho ngày đã chọn.");
                return;
            }

            // Lọc theo giá
            decimal minPrice = 0;
            decimal maxPrice = 1000000;

            if (!string.IsNullOrWhiteSpace(txtMin.Text))
            {
                if (!decimal.TryParse(txtMin.Text, out minPrice))
                {
                    MessageBox.Show("Giá tối thiểu không hợp lệ.");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtMax.Text))
            {
                if (!decimal.TryParse(txtMax.Text, out maxPrice))
                {
                    MessageBox.Show("Giá tối Đa không hợp lệ.");
                    return;
                }
            }

            // Lọc đơn hàng theo giá tiền
            DataTable filteredOrders = donHang.LocHoaDonTheoGiaTien(minPrice, maxPrice);

            // Nếu filteredOrders là null, hiển thị thông báo và kết thúc phương thức
            if (filteredOrders == null)
            {
                guna2DataGridViewOrders.Rows.Clear();
                MessageBox.Show("Không tìm thấy đơn hàng trong khoảng giá đã nhập.");
                return;
            }

            // Kết hợp kết quả theo ngày và giá
            if (orders.Rows.Count > 0 && filteredOrders.Rows.Count > 0)
            {
                // Lọc các dòng
                var matchingRows = orders.AsEnumerable()
                    .Where(row => filteredOrders.AsEnumerable().Any(fRow => fRow.Field<int>("Id_DonHang") == row.Field<int>("Id_DonHang")));

                // Kiểm tra nếu matchingRows không rỗng
                if (matchingRows.Any())
                {
                    DataTable resultOrders = matchingRows.CopyToDataTable();

                    // Kiểm tra kết quả của resultOrders có dữ liệu hay không
                    if (resultOrders.Rows.Count == 0)
                    {
                        guna2DataGridViewOrders.Rows.Clear();
                        MessageBox.Show("Không tìm thấy đơn hàng nào thỏa mãn điều kiện.");
                        return;
                    }

                    // Hiển thị các đơn hàng đã tìm thấy
                    DisplayOrders(resultOrders);
                }
                else
                {
                    guna2DataGridViewOrders.Rows.Clear();
                    MessageBox.Show("Không tìm thấy đơn hàng nào thỏa mãn điều kiện.");
                }
            }
            else
            {
                guna2DataGridViewOrders.Rows.Clear();

                MessageBox.Show("Không có dữ liệu.");
            }

        }

        private void guna2DataGridViewOrders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
           
        }

        private void guna2DataGridViewOrders_DoubleClick(object sender, EventArgs e)
        {
            if (guna2DataGridViewOrders.CurrentRow != null)
            {
                // Lấy Id_DonHang từ hàng hiện tại
                int idDonHang = Convert.ToInt32(guna2DataGridViewOrders.CurrentRow.Cells["Số Đơn Hàng"].Value);

                // Tạo và hiển thị form ChiTietDonHangForm, truyền Id_DonHang vào
                ChiTietDonHangForm detailForm = new ChiTietDonHangForm(idDonHang);
                detailForm.ShowDialog(); // Hoặc Show() tùy thuộc vào cách bạn muốn hiển thị
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMax_TextChanged(object sender, EventArgs e)
        {

        }
        private void ApplyRoundedCorners(System.Windows.Forms.Control control, int borderRadius)
        {
            // Tạo GraphicsPath với góc bo tròn
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(new System.Drawing.Rectangle(0, 0, borderRadius, borderRadius), 180, 90);
            path.AddArc(new System.Drawing.Rectangle(control.Width - borderRadius, 0, borderRadius, borderRadius), 270, 90);
            path.AddArc(new System.Drawing.Rectangle(control.Width - borderRadius, control.Height - borderRadius, borderRadius, borderRadius), 0, 90);
            path.AddArc(new System.Drawing.Rectangle(0, control.Height - borderRadius, borderRadius, borderRadius), 90, 90);
            path.CloseFigure();

            // Thiết lập Region của Control theo GraphicsPath đã tạo
            control.Region = new System.Drawing.Region(path);
        }
    }
}

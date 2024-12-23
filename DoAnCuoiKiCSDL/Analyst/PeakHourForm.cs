using DoAnCuoiKiCSDL.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL.Analyst
{
    public partial class PeakHourForm : Form
    {
        PhanTich phantich= new PhanTich();
        public PeakHourForm()
        {
            InitializeComponent();
            comboBoxTimeFilter.Items.Add("Ngày");
            comboBoxTimeFilter.Items.Add("Tháng");
            comboBoxTimeFilter.Items.Add("Năm");
            comboBoxTimeFilter.SelectedIndex = 0; // Mặc định chọn "Ngày"
            guna2DataGridView1.BorderStyle = BorderStyle.None;
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
        private void SetupDataGridView()
        {
            guna2DataGridView1.ColumnCount = 3;

            guna2DataGridView1.Columns[0].Name = "Giờ Cao Điểm";
            guna2DataGridView1.Columns[1].Name = "Số lượng hóa đơn";
            guna2DataGridView1.Columns[2].Name = "Doanh thu";
            guna2DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            guna2DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
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
        private void DisplayOrders(DataTable orders)
        {
            // Xóa dữ liệu cũ
           guna2DataGridView1.Rows.Clear();

            foreach (DataRow row in orders.Rows)
            {
                // Thêm hàng vào Guna2DataGridView
                guna2DataGridView1.Rows.Add(
                    row["HourOfDay"].ToString(),
                    row["SoLuongHoaDon"].ToString(),
                    row["TongDanhThu"].ToString()
                );
            }
        }
        private void PeakHourForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            UpdateDateTimePickerFormat();
        }

        private void comboBoxTimeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDateTimePickerFormat();
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

            // Lấy dữ liệu đơn hàng theo ngày
            DataTable orders = phantich.GiocaoDiem(selectedDate);

            // Kiểm tra nếu không có dữ liệu
            if (orders == null || orders.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy dữ liệu cho khoảng thời gian đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                guna2DataGridView1.Rows.Clear();
            }
            else
            {
                DisplayOrders(orders); // Hiển thị dữ liệu nếu có
            }
        }
    }
}

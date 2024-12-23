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
    public partial class AnalystForm : Form
    {
        public AnalystForm()
        {
            InitializeComponent();

        }

        private void btnPeakHour_Click(object sender, EventArgs e)
        {
            int textWidth = TextRenderer.MeasureText(btnPeakHour.Text, btnPeakHour.Font).Width;

            // Đặt chiều rộng của highlightPanel2 bằng với chiều rộng của văn bản
            highlightPanel2.Width = textWidth;
            highlightPanel2.Left = btnPeakHour.Left + (btnPeakHour.Width - textWidth) / 2; // Căn giữa highlightPanel2 với văn bản trong btnPeakHour
            highlightPanel2.Top = btnPeakHour.Bottom;  // Đặt highlightPanel2 ngay dưới btnPeakHour
            highlightPanel2.Height = 2;                // Đặt chiều cao của highlightPanel2
            highlightPanel2.Visible = true;

            PeakHourForm peakHourForm = new PeakHourForm();

            // Đặt Form2 vào trong gunaPanelContainer
            peakHourForm.TopLevel = false; // Để Form2 không mở cửa sổ riêng mà hiển thị trong panel
            peakHourForm.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền và thanh tiêu đề của Form2
            peakHourForm.Dock = DockStyle.Fill; // Để Form2 chiếm toàn bộ gunaPanel
            guna2Panel2.Controls.Clear(); // Xóa mọi control trong gunaPanel trước
            guna2Panel2.Controls.Add(peakHourForm); // Thêm Form2 vào gunaPanel
            peakHourForm.Show(); // Hiển thị Form2

        }

        private void AnalystForm_Load(object sender, EventArgs e)
        {

        }

        private void btnPerform_Click(object sender, EventArgs e)
        {
            int textWidth = TextRenderer.MeasureText(btnPerform.Text, btnPerform.Font).Width;

            // Đặt chiều rộng của highlightPanel2 bằng với chiều rộng của văn bản
            highlightPanel2.Width = textWidth;
            highlightPanel2.Left = btnPerform.Left + (btnPerform.Width - textWidth) / 2; // Căn giữa highlightPanel2 với văn bản trong btnPeakHour
            highlightPanel2.Top = btnPerform.Bottom;  // Đặt highlightPanel2 ngay dưới btnPeakHour
            highlightPanel2.Height = 2;                // Đặt chiều cao của highlightPanel2
            highlightPanel2.Visible = true;
        }
    }
}

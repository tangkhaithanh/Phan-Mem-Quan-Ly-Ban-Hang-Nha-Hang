using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using QRCoder;
using DoAnCuoiKiCSDL.Class;
namespace DoAnCuoiKiCSDL.EmployeeFolder
{
    public partial class ReceiptForm : Form
    {
        private DataTable receiptData; // Biến lưu trữ dữ liệu hóa đơn
        decimal totalAmount = 0;
        private bool checkDungDiem;
        private int idDonHang;
        DonHang donHang= new DonHang();
        public ReceiptForm(DataTable receiptData,bool checkDungDiem,int idDonHang)
        {
            InitializeComponent();
            this.receiptData = receiptData;
            guna2DataGridViewReceipt.AllowUserToAddRows = false;
            guna2DataGridViewReceipt.AllowUserToDeleteRows= false;
            guna2DataGridViewReceipt.ReadOnly = true;
            this.checkDungDiem = checkDungDiem;
            this.idDonHang = idDonHang;
        }

        private void ReceiptForm_Load(object sender, EventArgs e)
        {
            // Kiểm tra nếu có dữ liệu hóa đơn
            if (receiptData != null && receiptData.Rows.Count > 0)
            {
                // Gán dữ liệu hóa đơn vào Guna2DataGridView
                guna2DataGridViewReceipt.DataSource = receiptData;

                // Ẩn cột đầu tiên là IdMonAn
                if (guna2DataGridViewReceipt.Columns["Id_MonAn"] != null)
                {
                    guna2DataGridViewReceipt.Columns["Id_MonAn"].Visible = false;
                }

                // Tùy chỉnh hiển thị nếu cần thiết
                guna2DataGridViewReceipt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                guna2DataGridViewReceipt.ColumnHeadersHeight = 40;
                foreach (DataGridViewColumn column in guna2DataGridViewReceipt.Columns)
                {
                    column.HeaderCell.Style.BackColor = Color.DodgerBlue;
                    column.HeaderCell.Style.ForeColor = Color.White;
                    column.HeaderCell.Style.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
                    column.HeaderCell.Style.Padding = new Padding(5); // Thêm Padding cho Header
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Padding = new Padding(5);
                }
                CalculateAndDisplayTotal();
                // Load thông tin của nhân viên:
                labelName.Text += " " + Global.username.ToString();
                labelTime.Text += DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                if(checkDungDiem)
                {
                    
                    DataTable dt= new DataTable();  
                    dt= donHang.GetDonHangByIdDonHang(idDonHang);
                    labelTienGiam.Visible =true;
                    labelTienTra.Visible = true;
                    decimal TienGiam = 0;
                    if (decimal.TryParse(dt.Rows[0]["TienGiam"]?.ToString(), out TienGiam))
                    {
                        labelTienGiam.Text = $"Tiền đã giảm: {TienGiam:N0} VND"; // Định dạng tổng tiền
                    }
                    else
                    {
                        labelTienGiam.Text = "Dữ liệu không hợp lệ";
                    }

                    decimal TienTra = 0;
                    if (decimal.TryParse(dt.Rows[0]["TongTien"]?.ToString(), out TienTra))
                    {
                        labelTienTra.Text = $"Tiền Phải Trả: {TienTra:N0} VND"; // Định dạng tổng tiền
                    }
                    else
                    {
                        labelTienTra.Text = "Dữ liệu không hợp lệ";
                    }


                }

            }
            else
            {
                MessageBox.Show("Không có dữ liệu hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void CalculateAndDisplayTotal()
        {
            

            // Duyệt qua các hàng trong DataGridView và tính tổng tiền
            foreach (DataGridViewRow row in guna2DataGridViewReceipt.Rows)
            {
                // Giả sử bạn có cột "Giá" và cột "Số Lượng" để tính tổng tiền
                if (row.Cells["Gia"].Value != null && row.Cells["SoLuong"].Value != null)
                {
                    decimal price = Convert.ToDecimal(row.Cells["Gia"].Value);
                    int quantity = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    totalAmount += price * quantity; // Cộng dồn tổng tiền
                }
            }

            // Hiển thị tổng tiền vào một Label (lblTotalAmount)
            lblTotalAmount.Text = $"Tổng Tiền: {totalAmount:N0} VND"; // Định dạng tổng tiền
        }

        private void lblTotalAmount_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
           
        }

        private void btnExportPDF_Click_1(object sender, EventArgs e)
        {
            // Ẩn nút Export To PDF trước khi chụp ảnh
            btnExportPDF.Visible = false;

            // Làm mới và hiển thị lại panelReceipt
            panelReceipt.Refresh();
            panelReceipt.Update();

            // Tạo Bitmap và Graphics để chụp lại khu vực của panelReceipt
            Bitmap bitmap = new Bitmap(panelReceipt.Width, panelReceipt.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Chụp hình ảnh từ panelReceipt
                g.CopyFromScreen(panelReceipt.PointToScreen(Point.Empty), Point.Empty, panelReceipt.Size);
            }

            // Hiển thị lại nút Export To PDF sau khi chụp
            btnExportPDF.Visible = true;

            // Tạo file PDF và thêm ảnh từ panelReceipt vào PDF
            using (FileStream stream = new FileStream("Receipt.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Chuyển đổi Bitmap thành iTextSharp image
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    pdfImage.ScaleToFit(pdfDoc.PageSize.Width - 50, pdfDoc.PageSize.Height - 50);
                    pdfDoc.Add(pdfImage);
                }

                pdfDoc.Close();
            }

            MessageBox.Show("Form đã được xuất ra file PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Mở file PDF sau khi tạo thành công
            Process.Start("Receipt.pdf");
        }

        private void panelReceipt_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTotalAmount_Click_1(object sender, EventArgs e)
        {

        }
    }
}

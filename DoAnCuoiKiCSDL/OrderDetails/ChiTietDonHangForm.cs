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

namespace DoAnCuoiKiCSDL.OrderDetails
{
    public partial class ChiTietDonHangForm : Form
    {
        private int idDonHang;
        private ChiTietDonHang chiTietDonHang = new ChiTietDonHang();

        // Constructor với tham số
        public ChiTietDonHangForm(int idDonHang)
        {
            InitializeComponent();
            this.idDonHang = idDonHang; // Gán idDonHang vào thuộc tính
        }

        private void ChiTietDonHangForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            // Gọi hàm để lấy chi tiết đơn hàng và hiển thị trên DataGridView
            DataTable chiTietDonHangData = chiTietDonHang.LietKeCTDonHangTheoDonHang(idDonHang);
            DisplayChiTietDonHang(chiTietDonHangData);
        }

        private void SetupDataGridView()
        {
            // Xóa các cột hiện tại (nếu có)
            guna2DataGridView1.Columns.Clear();

            // Thêm cột vào DataGridView
            guna2DataGridView1.Columns.Add("Id_DonHang", "ID Đơn Hàng");
            guna2DataGridView1.Columns.Add("Ten", "Tên Món Ăn");
            guna2DataGridView1.Columns.Add("SoLuong", "Số Lượng");
            guna2DataGridView1.Columns.Add("ThanhTien", "Thành Tiền");

            guna2DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            guna2DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            guna2DataGridView1.AutoGenerateColumns = false; // Nếu bạn đã tự tay tạo cột
            guna2DataGridView1.ReadOnly = true;

            foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                column.HeaderCell.Style.BackColor = Color.DodgerBlue;
                column.HeaderCell.Style.ForeColor = Color.White;
                column.HeaderCell.Style.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                column.HeaderCell.Style.Padding = new Padding(5); // Thêm Padding cho Header
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Padding = new Padding(5);
            }
        }

        private void DisplayChiTietDonHang(DataTable chiTietData)
        {
            // Xóa dữ liệu cũ
            guna2DataGridView1.Rows.Clear();

            foreach (DataRow row in chiTietData.Rows)
            {
                // Thêm hàng vào DataGridView
                guna2DataGridView1.Rows.Add(
                    row["Id_DonHang"].ToString(),
                    row["Ten"].ToString(),
                    row["SoLuong"].ToString(),
                    row["ThanhTien"].ToString() + " VND"
                );
            }
        }
    }
}

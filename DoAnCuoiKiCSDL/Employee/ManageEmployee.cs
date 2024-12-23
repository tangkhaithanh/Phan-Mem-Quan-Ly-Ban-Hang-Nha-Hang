using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DoAnCuoiKiCSDL.Employee
{
    public partial class ManageEmployee : Form
    {
        User user = new User();
        public ManageEmployee()
        {

            InitializeComponent();
            //guna2DataGridViewEmployees.CellContentClick += guna2DataGridViewEmployees_CellContentClick;
            SetupDataGridView();
            LoadEmployees();
        }
        private void SetupDataGridView()
        {
            guna2DataGridViewEmployees.ColumnCount = 4;

            guna2DataGridViewEmployees.Columns[0].Name = "ID";
            guna2DataGridViewEmployees.Columns[1].Name = "Họ Tên";
            guna2DataGridViewEmployees.Columns[2].Name = "Chức Vụ";
            guna2DataGridViewEmployees.Columns[3].Name = "SĐT";


            // Thêm cột thao tác, nhưng không thêm button trực tiếp
            DataGridViewTextBoxColumn actionColumn = new DataGridViewTextBoxColumn();
            actionColumn.Name = "Thao Tác";
            actionColumn.HeaderText = "Thao Tác";
            actionColumn.ReadOnly = true;
            guna2DataGridViewEmployees.Columns.Add(actionColumn);

            // Cấu hình cho các cột
            guna2DataGridViewEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            guna2DataGridViewEmployees.RowTemplate.Height = 40;
            guna2DataGridViewEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridViewEmployees.Columns[0].ReadOnly = true;
            guna2DataGridViewEmployees.Columns[1].ReadOnly = true;
            guna2DataGridViewEmployees.Columns[2].ReadOnly = true;
            guna2DataGridViewEmployees.Columns[3].ReadOnly = true;
            guna2DataGridViewEmployees.AllowUserToAddRows = false;


            // Thiết lập màu sắc và font
            foreach (DataGridViewColumn column in guna2DataGridViewEmployees.Columns)
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
        private void LoadEmployees()
        {
            DataTable dt = user.LayDanhSachNhanVien();

            // Lọc danh sách những nhân viên có Chuc_Vu là "Employee"
            DataRow[] filteredRows = dt.Select("Chuc_Vu = 'Employee'");

            // Tạo một DataTable mới từ các hàng đã lọc
            if (filteredRows.Length > 0)
            {
                DataTable filteredTable = filteredRows.CopyToDataTable();
                // Gọi hàm hiển thị danh sách nhân viên đã lọc
                DisplayEmployees(filteredTable);
            }
            else
            {
                // Nếu không có kết quả nào, hiển thị thông báo hoặc bảng rỗng
                MessageBox.Show("Không tìm thấy nhân viên có chức vụ Employee.");
            }
        }
        private void DisplayEmployees(DataTable employees)
        {
            guna2DataGridViewEmployees.Rows.Clear();

            foreach (DataRow row in employees.Rows)
            {
                // Thêm hàng vào DataGridView
                int rowIndex = guna2DataGridViewEmployees.Rows.Add(
                    row["Id_NhanVien"].ToString(),
                    row["Ho_Ten"].ToString(),
                    row["Chuc_Vu"].ToString(),
                    row["SDT"].ToString()
                );
            }
        }
        private void guna2DataGridViewEmployees_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
           
        }

        private void ManageEmployee_Load(object sender, EventArgs e)
        {
            // Gọi hàm để lấy danh sách nhân viên
            LoadEmployees();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEmployee addemployee = new AddEmployee();
            addemployee.ShowDialog();
            LoadEmployees();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lấy các giá trị từ các TextBox (thay thế txtSearchName bằng tên TextBox của bạn)
            string hoTen = txtSearchName.Text.Trim(); // Hoặc theo tên, chức vụ, SĐT mà bạn muốn tìm kiếm

            // Gọi hàm tìm kiếm
            DataTable result = user.TimKiemNhanVien(hoTen); // Bạn có thể thêm các tham số khác nếu cần

            if (result != null && result.Rows.Count > 0)
            {
                DisplayEmployees(result); // Hiển thị kết quả tìm kiếm lên DataGridView
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên nào.");
            }
        }

        private void guna2DataGridViewEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        // design
        private void guna2DataGridViewEmployees_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Kiểm tra hàng hiện tại có dữ liệu hay không
            if (e.RowIndex >= 0 && e.ColumnIndex == 4 && guna2DataGridViewEmployees.Rows[e.RowIndex].Cells[0].Value != null)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Lấy ảnh từ resources
                Image editImage = Resource.Resources.pencil_colored_blue; // Icon "Sửa" từ Resources
                Image removeImage = Resource.Resources.dark_red_icon_fixed; // Icon "Xóa" từ Resources

                // Kích thước nút
                int buttonWidth = 30; // Chiều rộng của mỗi nút
                int buttonHeight = 30; // Chiều cao của mỗi nút
                int paddingBetweenButtons = 10; // Khoảng cách giữa hai nút

                // Tính tổng chiều rộng chiếm bởi hai nút và khoảng cách giữa chúng
                int totalButtonsWidth = 2 * buttonWidth + paddingBetweenButtons;

                // Tính toán vị trí x bắt đầu sao cho hai nút nằm ở giữa cột theo chiều ngang
                int startX = e.CellBounds.Left + (e.CellBounds.Width - totalButtonsWidth) / 2;

                // Vị trí y để căn giữa nút theo chiều dọc
                int startY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2;

                // Tạo các `Rectangle` cho nút Sửa và Xóa
                var editButtonRect = new Rectangle(startX, startY, buttonWidth, buttonHeight);
                var removeButtonRect = new Rectangle(startX + buttonWidth + paddingBetweenButtons, startY, buttonWidth, buttonHeight);

                // Vẽ ảnh nút Sửa
                e.Graphics.DrawImage(editImage, editButtonRect);

                // Vẽ ảnh nút Xóa
                e.Graphics.DrawImage(removeImage, removeButtonRect);

                e.Handled = true; // Ngăn không cho DataGridView vẽ thêm phần nội dung
            }
        }

        private void guna2DataGridViewEmployees_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 4) // Cột "Thao Tác"
            {
                // Lấy vị trí nhấp chuột
                var clickLocation = guna2DataGridViewEmployees.PointToClient(Cursor.Position);

                // Lấy kích thước của ô tại dòng và cột hiện tại
                var cellRect = guna2DataGridViewEmployees.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Tạo vị trí cho nút Sửa và Xóa
                var editButton = new Rectangle(cellRect.Left +50, cellRect.Top + 5, 50, cellRect.Height - 10);
                var removeButton = new Rectangle(cellRect.Left + 90, cellRect.Top + 5, 50, cellRect.Height - 10);

                // Kiểm tra nếu người dùng nhấn vào nút Sửa
                if (editButton.Contains(clickLocation))
                {
                    int idNhanVien = Convert.ToInt32(guna2DataGridViewEmployees.Rows[e.RowIndex].Cells[0].Value);
                    EditEmployee editForm = new EditEmployee(idNhanVien);
                    editForm.ShowDialog();
                    LoadEmployees(); // Làm mới danh sách nhân viên
                }
                // Kiểm tra nếu người dùng nhấn vào nút Xóa
                else if (removeButton.Contains(clickLocation))
                {
                    int idNhanVien = Convert.ToInt32(guna2DataGridViewEmployees.Rows[e.RowIndex].Cells[0].Value);
                    string sdt = guna2DataGridViewEmployees.Rows[e.RowIndex].Cells[3].Value.ToString();
                    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        if (user.XoaNhanVien(idNhanVien))
                        {
                            user.XoaTaiKhoanNhanVienTrenSQL(sdt);
                            MessageBox.Show("Xóa nhân viên thành công.");
                            LoadEmployees(); // Làm mới danh sách nhân viên
                        }
                        else
                        {
                            MessageBox.Show("Xóa nhân viên thất bại.");
                        }
                    }
                }
            }
        }
    }
}

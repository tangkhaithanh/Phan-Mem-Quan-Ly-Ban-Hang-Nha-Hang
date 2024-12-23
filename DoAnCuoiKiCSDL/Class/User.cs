using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL
{
    internal class User
    {
        DBConnection dbConnect= new DBConnection();
        public bool TaoTaiKhoanNhanVienTrenSQL(string sdt, string matKhau)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("TaoTaiKhoanNhanVienTrenSQL", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        public bool CapNhatTaiKhoanNhanVienTrenSQL(string oldSdt,string newSdt, string matKhau)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("CapNhatTaiKhoanMatKhauNguoiDungSql", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OldSDT", oldSdt);
                cmd.Parameters.AddWithValue("@NewSDT", newSdt);
                cmd.Parameters.AddWithValue("@matKhau", matKhau);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        public bool XoaTaiKhoanNhanVienTrenSQL(string sdt)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("XoaTaiKhoanNguoiDung", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        public bool ThemNhanVien(string hoTen, string chucVu, string sdt, string matKhau)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("ThemNhanVien", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số
                cmd.Parameters.AddWithValue("@Ho_Ten", hoTen);
                cmd.Parameters.AddWithValue("@Chuc_Vu", chucVu);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException sqlEx) // Bắt lỗi SQL Server
            {
                MessageBox.Show($"Lỗi SQL Server: {sqlEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            { 
                dbConnect.CloseConnection(); 
            }
        }

        // Hàm sửa thông tin nhân viên
        public bool SuaNhanVien(int idNhanVien, string hoTen, string chucVu, string sdt, string matKhau)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SuaNhanVien", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_NhanVien", idNhanVien);
                cmd.Parameters.AddWithValue("@Ho_Ten", hoTen);
                cmd.Parameters.AddWithValue("@Chuc_Vu", chucVu);
                cmd.Parameters.AddWithValue("@SDT", sdt);
                cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        // Hàm xóa nhân viên khỏi cơ sở dữ liệu
        public bool XoaNhanVien(int idNhanVien)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("XoaNhanVien", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_NhanVien", idNhanVien);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        // Hàm tìm kiếm nhân viên theo tên, sdt, hoặc chức vụ
        public DataTable TimKiemNhanVien(string hoTen)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("Select * from dbo.TimKiemNhanVienTheoTen(@Ho_Ten)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

                // Thêm tham số tìm kiếm (cho phép null)
                cmd.Parameters.AddWithValue("@Ho_Ten", hoTen);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        // Hàm đăng nhập
        public DataTable DangNhap(string sdt, string matKhau)
        {
            DataTable dt = new DataTable();

            try
            {
                dbConnect.OpenConnection();  

                SqlCommand cmd = new SqlCommand("LoginUser", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sdt", sdt);
                cmd.Parameters.AddWithValue("@Password", matKhau);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt); 
                return dt;  
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;  
            }
            finally
            {
                dbConnect.CloseConnection(); 
            }
        }
        public DataTable LayDanhSachNhanVien()
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.LayDanhSachNhanVien()", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text; // Sử dụng CommandType.Text cho câu lệnh SQL

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt); // Lấy dữ liệu vào DataTable
                return dt; // Trả về DataTable chứa danh sách nhân viên
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null; // Trả về null nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }

        public DataTable TimKiemNhanVienTheoID(int idNhanVien)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.TimKiemNhanVienTheoID(@Id_NhanVien)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Id_NhanVien", idNhanVien);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt; // Trả về DataTable chứa thông tin nhân viên
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null; // Trả về null nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }
        public string TaoMatKhauManh()
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("GenerateStrongPassword", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                string generatedPassword = (string)cmd.ExecuteScalar();
                return generatedPassword;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null; // Trả về null nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }
    }
}

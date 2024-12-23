using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace DoAnCuoiKiCSDL
{
    internal class MonAn
    {
        DBConnection dbConnect = new DBConnection();

        // Hàm thêm món ăn vào cơ sở dữ liệu thông qua stored procedure ThemMonAn
        public bool ThemMonAn(int idDanhMuc, string ten, string moTa, decimal gia, byte[] hinhAnhData)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("ThemMonAn", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_DanhMuc", idDanhMuc);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@MoTa", moTa);
                cmd.Parameters.AddWithValue("@Gia", gia);
                cmd.Parameters.AddWithValue("@HinhAnh",hinhAnhData);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException sqlEx) // Bắt lỗi SQL Server
            {
                System.Windows.Forms.MessageBox.Show($"{sqlEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        // Hàm sửa món ăn trong cơ sở dữ liệu thông qua stored procedure SuaMonAn
        public bool SuaMonAn(int idMonAn, int idDanhMuc, string ten, string moTa, decimal gia, byte[] hinhAnhData)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SuaMonAn", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_MonAn", idMonAn);
                cmd.Parameters.AddWithValue("@Id_DanhMuc", idDanhMuc);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@MoTa", moTa);
                cmd.Parameters.AddWithValue("@Gia", gia);
                cmd.Parameters.AddWithValue("@HinhAnh", hinhAnhData);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        // Hàm xóa món ăn khỏi cơ sở dữ liệu thông qua stored procedure XoaMonAn
        public bool XoaMonAn(int idMonAn)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("XoaMonAn", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_MonAn", idMonAn);

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

        // Hàm liệt kê danh sách món ăn theo danh mục
        public DataTable DanhSachMonAnTheoDanhMuc(int idDanhMuc)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM DanhSachMonAnTheoDanhMuc(@Id_DanhMuc)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Id_DanhMuc", idDanhMuc);

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
        
        public int LayTongMonAn()
        {
            try
            {
                dbConnect.OpenConnection(); // Mở kết nối với cơ sở dữ liệu

                // Tạo câu lệnh SQL để gọi hàm TongSoBan
                string sqlQuery = "SELECT dbo.TongMonAn()";

                SqlCommand cmd = new SqlCommand(sqlQuery, dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text; // Sử dụng CommandType.Text cho câu lệnh SQL
                // Thực thi câu lệnh và lấy kết quả
                object result = cmd.ExecuteScalar(); // Dùng ExecuteScalar để lấy giá trị đơn lẻ

                // Kiểm tra và trả về giá trị
                return result != null ? Convert.ToInt32(result) : 0; // Trả về tổng số bàn hoặc 0 nếu không có bàn
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message); // Hiển thị lỗi nếu có
                return 0; // Trả về 0 nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }

        public DataTable TimKiemMonAnTheoId(int idMonAn)
        {
            try
            {
                dbConnect.OpenConnection(); // Mở kết nối với cơ sở dữ liệu

                // Tạo câu lệnh SQL để gọi hàm tìm kiếm món ăn
                string sqlQuery = "SELECT * FROM dbo.TimKiemMonAnTheoId(@Id_MonAn)";

                SqlCommand cmd = new SqlCommand(sqlQuery, dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text; // Sử dụng CommandType.Text cho câu lệnh SQL

                // Thêm tham số
                cmd.Parameters.AddWithValue("@Id_MonAn", idMonAn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt); // Lấy dữ liệu vào DataTable

                return dt; // Trả về DataTable chứa thông tin món ăn
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message); // Hiển thị lỗi nếu có
                return null; // Trả về null nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }

        public DataTable LietKeDanhSachMonAn()
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("Select * From dbo.DanhSachTatCaMonAn()", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

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
        public DataTable TimKiemMonAn(string tenMonAn)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.TimKiemMonAn(@Ten)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

                // Thêm tham số tìm kiếm
                cmd.Parameters.AddWithValue("@Ten", tenMonAn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt); // Lấy dữ liệu vào DataTable
                return dt; // Trả về DataTable chứa kết quả tìm kiếm
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
        public DataTable ThongKeMonAnBanChay()
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.ThongKeMonAnBanChay()", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text; // Sử dụng CommandType.Text cho câu lệnh SQL

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt); // Lấy dữ liệu vào DataTable
                return dt; // Trả về DataTable chứa danh sách món ăn đã đặt
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
    }

}

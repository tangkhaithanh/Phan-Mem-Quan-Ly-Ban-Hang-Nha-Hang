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
    internal class Ban
    {
        DBConnection dbConnect = new DBConnection();

        // Hàm thêm bàn vào cơ sở dữ liệu thông qua stored procedure ThemBan
        public bool ThemBan(int soChoNgoi, string trangThai)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("ThemBan", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@So_Cho_Ngoi", soChoNgoi);
                cmd.Parameters.AddWithValue("@Trang_Thai", trangThai);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException sqlEx) // Bắt lỗi từ SQL Server
            {
                // Hiển thị thông báo lỗi từ trigger hoặc câu lệnh SQL
                System.Windows.Forms.MessageBox.Show($" {sqlEx.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex) // Bắt các lỗi khác
            {
                // Hiển thị lỗi khác (nếu có)
                System.Windows.Forms.MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        // Hàm sửa bàn trong cơ sở dữ liệu thông qua stored procedure SuaBan
        public bool SuaBan(int idBan, int soChoNgoi)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SuaBan", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);
                cmd.Parameters.AddWithValue("@So_Cho_Ngoi", soChoNgoi);

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

        // Hàm xóa bàn khỏi cơ sở dữ liệu thông qua stored procedure XoaBan
        public bool XoaBan(int idBan)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("XoaBan", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@Id_Ban", idBan);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);    
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }
        public DataTable LietKeBanTheoId(int idBan)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("LietKeBanTheoId", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);

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
        public int LayTongSoBan()
        {
            try
            {
                dbConnect.OpenConnection(); // Mở kết nối với cơ sở dữ liệu

                // Tạo câu lệnh SQL để gọi hàm TongSoBan
                string sqlQuery = "SELECT dbo.TongSoBan()";

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

        // Hàm cập nhật trạng thái bàn "đã đặt"
        /*public bool CapNhatTrangThaiBanDaDat(int idBan)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("CapNhatTrangThaiBanDaDat", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);

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

        // Hàm cập nhật trạng thái bàn "trống"
        public bool CapNhatTrangThaiBanTrong(int idBan)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("CapNhatTrangThaiBanTrong", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);

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
        }*/

    }
}

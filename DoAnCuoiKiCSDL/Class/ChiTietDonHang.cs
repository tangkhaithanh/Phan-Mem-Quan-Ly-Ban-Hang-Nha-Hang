using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiCSDL.Class
{
    internal class ChiTietDonHang
    {
        DBConnection dbConnect = new DBConnection();

        // Hàm thêm chi tiết đơn hàng vào cơ sở dữ liệu thông qua stored procedure ThemChiTietDonHang
        public bool ThemChiTietDonHang(int idDonHang, int idMonAn, int soLuong, decimal thanhTien)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("ThemChiTietDonHang", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_DonHang", idDonHang);
                cmd.Parameters.AddWithValue("@Id_MonAn", idMonAn);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                cmd.Parameters.AddWithValue("@ThanhTien", thanhTien);

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


        public DataTable LietKeCTDonHangTheoDonHang(int idDonHang)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.ChiTietDonHangCoTenMonAn(@Id_DonHang)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text; // Sử dụng CommandType.Text cho câu lệnh SQL

                cmd.Parameters.AddWithValue("@Id_DonHang", idDonHang);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt; // Trả về DataTable chứa chi tiết đơn hàng
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

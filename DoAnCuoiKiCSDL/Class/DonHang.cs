using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoAnCuoiKiCSDL.Class
{
    internal class DonHang
    {
        DBConnection dbConnect = new DBConnection();
        // Hàm thêm đơn hàng vào cơ sở dữ liệu thông qua stored procedure ThemDonHang
        public bool ThemDonHang(string sdtKhachHang, int idNhanVien, string phuongThucThanhToan, int idBan, DateTime ngayGio, decimal tongTien)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("ThemDonHang", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SDT_KhachHang", sdtKhachHang);
                cmd.Parameters.AddWithValue("@Id_NhanVien", idNhanVien);
                cmd.Parameters.AddWithValue("@PhuongThucThanhToan", phuongThucThanhToan);
                cmd.Parameters.AddWithValue("@Id_Ban", idBan);
                cmd.Parameters.AddWithValue("@NgayGio", ngayGio);
                cmd.Parameters.AddWithValue("@TongTien", tongTien);
                cmd.Parameters.AddWithValue("@TrangThai", "Dang xu ly");

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

        // Hàm lọc đơn hàng theo ngày tháng năm thông qua stored procedure LocDonHangTheoNgayThangNam
        public DataTable LocDonHangTheoNgay(String thoiGian)
        {
            try
            {
                dbConnect.OpenConnection();

                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.LocDonHangTheoThoiGian(@ThoiGian)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text; // Sử dụng CommandType.Text cho câu lệnh SQL

                cmd.Parameters.AddWithValue("@ThoiGian", thoiGian);
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

        //Lọc hóa đơn theo giá tiền
        public DataTable LocHoaDonTheoGiaTien(decimal minGia, decimal maxGia)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM LocHoaDonTheoGiaTien(@Min, @Max)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Min", minGia);
                cmd.Parameters.AddWithValue("@Max", maxGia);

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

        public decimal TinhTongTienDoanhThu()
        {
            try
            {
                dbConnect.OpenConnection(); // Mở kết nối
                SqlCommand cmd = new SqlCommand("SELECT dbo.TinhTongTienDoanhThu()", dbConnect.GetConnection());

                // Thực thi câu lệnh SQL và lấy kết quả
                decimal totalRevenue = (decimal)cmd.ExecuteScalar();

                return totalRevenue; // Trả về tổng doanh thu
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0; // Trả về 0 nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }

        public int TongSoLuongDonHang()
        {
            try
            {
                dbConnect.OpenConnection(); // Mở kết nối
                SqlCommand cmd = new SqlCommand("SELECT dbo.TongSoLuongDonHang()", dbConnect.GetConnection());

                // Thực thi câu lệnh SQL và lấy kết quả
                object result = cmd.ExecuteScalar(); // Dùng ExecuteScalar để lấy giá trị đơn lẻ

                // Kiểm tra và trả về giá trị
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0; // Trả về 0 nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }
        public bool InsertSelectedDish(int idBan, int idMonAn, int soLuong)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("InsertSelectedDish", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);
                cmd.Parameters.AddWithValue("@Id_MonAn", idMonAn);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                cmd.ExecuteNonQuery(); // Thực thi lệnh chèn
                return true; // Trả về true nếu thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Trả về false nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }
        public DataTable GetSelectedDishesByTableId(int idBan)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.GetSelectedDishesByTableId(@Id_Ban)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

                // Thêm tham số cho hàm
                cmd.Parameters.AddWithValue("@Id_Ban", idBan);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt; // Trả về DataTable chứa danh sách món ăn đã chọn
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

        public bool UpdateSelectedDishQuantity(int idBan, int idMonAn, int soLuong)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("dbo.UpdateSelectedDishQuantity", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);
                cmd.Parameters.AddWithValue("@Id_MonAn", idMonAn);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                cmd.ExecuteNonQuery();
                return true; // Cập nhật thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Cập nhật thất bại
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }

        public bool DeleteAllSelectedDishesByTableId(int idBan)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("dbo.DeleteAllSelectedDishesByTableId", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);

                cmd.ExecuteNonQuery();
                return true; // Xóa thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Xóa thất bại
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }

        public bool DeleteSelectedDish(int idBan, int idMonAn)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("dbo.DeleteSelectedDish", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_Ban", idBan);
                cmd.Parameters.AddWithValue("@Id_MonAn", idMonAn);

                cmd.ExecuteNonQuery();
                return true; // Xóa thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Xóa thất bại
            }
            finally
            {
                dbConnect.CloseConnection(); // Đảm bảo kết nối được đóng
            }
        }

        public DataTable GetReceiptData(int idDonHang)
        {
            DataTable dt = new DataTable();
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM GetReceiptData(@Id_DonHang)", dbConnect.GetConnection());
                cmd.Parameters.AddWithValue("@Id_DonHang", idDonHang);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt); // Điền dữ liệu vào DataTable
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnection();
            }
            return dt; // Trả về DataTable chứa thông tin hóa đơn
        }

        public bool CheckIfOrderExists(int idBan, out int idDonHang)
        {
            try
            {
                dbConnect.OpenConnection();

                // Tạo SqlCommand để gọi Stored Procedure
                SqlCommand cmd = new SqlCommand("sp_CheckIfOrderExists", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số đầu vào cho Stored Procedure
                cmd.Parameters.AddWithValue("@IdBan", idBan);
                cmd.Parameters.AddWithValue("@TrangThai", "Dang xu ly");

                // Thêm tham số đầu ra cho Stored Procedure
                SqlParameter outputIdDonHang = new SqlParameter("@IdDonHang", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdDonHang);

                // Thực thi Stored Procedure
                cmd.ExecuteNonQuery();

                // Lấy giá trị của tham số đầu ra
                idDonHang = (int)outputIdDonHang.Value;

                return idDonHang > 0;  // Trả về true nếu có IdDonHang
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                idDonHang = 0;
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        public bool UpdateOrderTotal(int idDonHang, decimal tongTien,string PhuongThucThanhToan)
        {
            try
            {
                dbConnect.OpenConnection();

                // Tạo SqlCommand để gọi Stored Procedure
                SqlCommand cmd = new SqlCommand("sp_UpdateOrderTotal", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số cho Stored Procedure
                cmd.Parameters.AddWithValue("@IdDonHang", idDonHang);
                cmd.Parameters.AddWithValue("@TongTien", tongTien);
                cmd.Parameters.AddWithValue("@phuongthucthanhtoan", PhuongThucThanhToan);
                // Sử dụng ExecuteNonQuery để thực thi Stored Procedure
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;  // Trả về true nếu có ít nhất một hàng bị cập nhật
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;  // Trả về false nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        public bool CheckIfDishExists(int idBan, int idMonAn)
        {
            try
            {
                dbConnect.OpenConnection();

                // Tạo SqlCommand để gọi hàm SQL
                SqlCommand cmd = new SqlCommand("SELECT dbo.fn_CheckIfDishExists(@IdBan, @IdMonAn)", dbConnect.GetConnection());

                // Thêm các tham số cho hàm
                cmd.Parameters.AddWithValue("@IdBan", idBan);
                cmd.Parameters.AddWithValue("@IdMonAn", idMonAn);

                // Thực thi câu lệnh và nhận kết quả
                object result = cmd.ExecuteScalar();

                // Nếu kết quả không null và khác DBNull, trả về true nếu món ăn đã tồn tại
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToBoolean(result);
                }

                return false; // Trả về false nếu không tìm thấy món ăn
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Trả về false nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        public bool UpdateOrderStatus(int idDonHang, string newStatus)
        {
            try
            {
                dbConnect.OpenConnection();

                // Tạo SqlCommand để gọi Stored Procedure
                SqlCommand cmd = new SqlCommand("sp_UpdateOrderStatus", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các tham số cho Stored Procedure
                cmd.Parameters.AddWithValue("@IdDonHang", idDonHang);
                cmd.Parameters.AddWithValue("@TrangThai", newStatus);

                // Sử dụng ExecuteNonQuery để thực thi Stored Procedure
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;  // Trả về true nếu có ít nhất một hàng bị cập nhật
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;  // Trả về false nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        public DataTable GetDonHangByIdDonHang(int idDonHang)
        {
            DataTable dt = new DataTable();
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM fn_GetDonHangById(@Id_DonHang)", dbConnect.GetConnection());
                cmd.Parameters.AddWithValue("@Id_DonHang", idDonHang);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt); // Điền dữ liệu vào DataTable
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                dbConnect.CloseConnection();
            }
            return dt; // Trả về DataTable chứa thông tin hóa đơn
        }

        public bool CapNhatTongTienVaTienGiam(int  idDonHang)
        {
            try
            {
                dbConnect.OpenConnection();

                // Tạo SqlCommand để gọi Stored Procedure
                SqlCommand cmd = new SqlCommand("CapNhatTongTienVaTienGiam", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                // Thêm các tham số cho Stored Procedure
                cmd.Parameters.AddWithValue("Id_DonHang", idDonHang);
                // Sử dụng ExecuteNonQuery để thực thi Stored Procedure
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;  // Trả về true nếu có ít nhất một hàng bị cập nhật
            }
            catch (Exception ex)
            {
               MessageBox.Show("Error: " + ex.Message);
                return false;  // Trả về false nếu có lỗi
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }
    }
}

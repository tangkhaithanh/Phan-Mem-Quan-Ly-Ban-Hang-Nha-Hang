using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoAnCuoiKiCSDL.Class
{
    internal class KhachHang
    {
        DBConnection dbConnect= new DBConnection();
        public DataTable LayThongTinKhachhang(string sdt)
        {
            try
            {
                dbConnect.OpenConnection();

                SqlCommand cmd = new SqlCommand("SELECT * FROM GetCustomerInfo(@sdt)", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text; // Sử dụng CommandType.Text cho câu lệnh SQL

                cmd.Parameters.AddWithValue("@sdt", sdt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
               MessageBox.Show("Error: " + ex.Message);
                return null;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiCSDL.Class
{
    internal class PhanTich
    {
        DBConnection dbConnect = new DBConnection();
        public DataTable GiocaoDiem(String thoiGian)
        {
            try
            {
                dbConnect.OpenConnection();

                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.GetPeakSalesHour(@ThoiGian)", dbConnect.GetConnection());
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
    }
}

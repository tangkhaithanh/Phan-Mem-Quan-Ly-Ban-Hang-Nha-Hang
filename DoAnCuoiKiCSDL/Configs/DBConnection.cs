using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnCuoiKiCSDL
{
    internal class DBConnection
    {
        public static string connectionString= "Data Source=DESKTOP-IVRVPSI\\TANGTHANH;Initial Catalog=QuanLiNhaHang1;Integrated Security=True";
        private SqlConnection sqlConnection = null;
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        // Hàm mở kết nối
        public void OpenConnection()
        {
            
            if (sqlConnection == null)
            {
                sqlConnection = new SqlConnection(connectionString);
            }

            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        // Hàm đóng kết nối
        public void CloseConnection()
        {
            if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        // Hàm trả về SqlConnection hiện tại
        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}

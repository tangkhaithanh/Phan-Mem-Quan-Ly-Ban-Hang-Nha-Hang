using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoAnCuoiKiCSDL
{
    internal class DanhMuc
    {
        DBConnection dbConnect = new DBConnection();

        // Hàm thêm danh mục vào cơ sở dữ liệu thông qua stored procedure ThemDanhMuc
        public bool ThemDanhMuc(string ten, string moTa)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("ThemDanhMuc", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@MoTa", moTa);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }

        // Hàm sửa danh mục trong cơ sở dữ liệu thông qua stored procedure SuaDanhMuc
        public bool SuaDanhMuc(int idDanhMuc, string ten, string moTa)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SuaDanhMuc", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id_DanhMuc", idDanhMuc);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@MoTa", moTa);

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

        // Hàm xóa danh mục khỏi cơ sở dữ liệu thông qua stored procedure XoaDanhMuc
        public bool XoaDanhMuc(int idDanhMuc)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("XoaDanhMuc", dbConnect.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@Id_DanhMuc", idDanhMuc);

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

        // Hàm liệt kê danh mục
        public DataTable LietKeDanhSachDanhMuc()
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("Select * From LietKeDanhSachDanhMuc()", dbConnect.GetConnection());
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            finally
            {
                dbConnect.CloseConnection();
            }
        }
        public DataTable TimKiemDanhMucTheoID(int idDanhMuc)
        {
            try
            {
                dbConnect.OpenConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.TimKiemDanhMucTheoID(@Id_DanhMuc)", dbConnect.GetConnection());
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
    }
}

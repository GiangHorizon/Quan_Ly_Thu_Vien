
using Microsoft.Data.SqlClient;

namespace WinFormsApp
{
    internal class DatabaseHelper
    {
        private readonly string connString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=QuanLyThuVien;
              Integrated Security=True;
              TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connString);
        }
    }
}

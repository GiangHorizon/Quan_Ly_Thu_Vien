using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using Microsoft.Data.SqlClient;
namespace WinFormsApp
{
    internal class DatabaseHelper
    {
        private readonly string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyThuVien;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=""SQL Server Management Studio"";Command Timeout=0";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connString);
        }
    }
}

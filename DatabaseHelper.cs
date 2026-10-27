using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
namespace WinFormsApp
{
    internal class DatabaseHelper
    {
        private readonly string connString = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"SQL Server Management Studio\";Command Timeout=0";
        
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connString);
        }
    }
}

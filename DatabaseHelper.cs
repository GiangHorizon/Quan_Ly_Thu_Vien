using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
namespace WinFormsApp
{
    internal class DatabaseHelper
    {
        private readonly string connString = "Host=localhost;Port=5432;Database=QuanLyTheSV_TV;Username=postgres;Password=giang2k6";
        
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connString);
        }
    }
}

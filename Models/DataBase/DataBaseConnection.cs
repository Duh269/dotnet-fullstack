using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TestFullStack.Models.DataBase
{
    public class DataBaseConnection
    {
        public DbConnection _db { get; }
        public string _TipoDB { get;  }

        public DataBaseConnection() {

           string TipoDb = Properties.Settings.Default.Db;
            _TipoDB = TipoDb;
            if (TipoDb == "SQLSERVER")
            {
                SqlConnection conn = new SqlConnection(Properties.Settings.Default.ConnectionString);
                _db = conn;
            }
            else if (TipoDb == "MYSQL")
            {
                MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);

                _db = conn;
            }
           


            
        }
    }
}
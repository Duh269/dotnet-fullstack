using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFullStack.Models.DataBase;

namespace TestFullStack.Models
{
    public  class DBContext
    {
        public DbConnection conn { get; set; }
       
       public static DataTable DataTable(string query)
        {
            try
            {
                DataBaseConnection conn = new DataBaseConnection();
                DataTable t1 = new DataTable();

                if (conn._TipoDB == "SQLSERVER")
                {
                    SqlCommand cmd = new SqlCommand(query, (SqlConnection)conn._db);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(t1);
                    }

                }
                else if (conn._TipoDB == "MYSQL")
                {
                    MySqlCommand cmd = new MySqlCommand(query, (MySqlConnection)conn._db);

                    using (MySqlDataAdapter a = new MySqlDataAdapter(cmd))
                    {
                        a.Fill(t1);
                    }
                }

                return t1;
            }
            catch (Exception ex)
            {

                throw ex;
            }

          
        }

      

        public static int Executar(string sql)
        {
            DataBaseConnection conn = new DataBaseConnection();
            var retorno = 0;
            conn._db.Open();
            if (conn._TipoDB == "SQLSERVER")
            {
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)conn._db);
                retorno = cmd.ExecuteNonQuery();

            } else if (conn._TipoDB == "MYSQL")
            {
                MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)conn._db);
                retorno =cmd.ExecuteNonQuery();
            }

            conn._db.Close();
            return retorno;
        }

       
    }
}
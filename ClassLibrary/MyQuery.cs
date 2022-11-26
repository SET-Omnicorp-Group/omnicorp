﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClassLibrary
{
    public class MyQuery
    {
        public MySqlConnection conn;

        public MyQuery()
        {
            string server = ConfigurationManager.AppSettings["server"];
            string database = ConfigurationManager.AppSettings["database"];
            string uid = ConfigurationManager.AppSettings["uid"];
            string password = ConfigurationManager.AppSettings["password"];

            string connString = $"server={server};"+
                                $"database={database};"+
                                $"uid={uid};"+
                                $"pwd={password};";
            conn = new MySqlConnection(connString);

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                conn = null;
                // @TODO log this to a file
                return;
            }
        }

        public MySqlDataReader DataReader(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);  // Enter command into omnicorp database
            MySqlDataReader rdr = cmd.ExecuteReader();  // Read rows in in user table
            return rdr;
        }

        public void Close()
        {
            conn.Close();
        }
    }
}
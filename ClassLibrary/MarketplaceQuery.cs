using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClassLibrary
{
    public class MarketplaceQuery
    {
        public MySqlConnection conn;

        public MarketplaceQuery()
        {
            string server = ConfigurationManager.AppSettings["server"];
            string database = ConfigurationManager.AppSettings["database"];
            string uid = ConfigurationManager.AppSettings["uid"];
            string password = ConfigurationManager.AppSettings["password"];

            string connString = $"server=159.89.117.198;" +
                                $"database=cmp;" +
                                $"uid=DevOSHT;" +
                                $"pwd=Snodgr4ss!;";
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

        public string GetLogFileFromDatabase()
        {
            string query = "SELECT content FROM configs WHERE name = 'logFile';";
            MySqlCommand cmd = new MySqlCommand(query, conn);  // Enter command into omnicorp database
            MySqlDataReader rdr = cmd.ExecuteReader();  // Read rows in in user table
            rdr.Read();

            string logFile = rdr.GetString(0);
            return logFile;
        }

        public void UpdateLogFileToDatabase(string logFile)
        {
            string updateQuery = $"UPDATE content SET content = {logFile} WHERE name = logFile;";
            MySqlCommand cmd = new MySqlCommand(updateQuery, conn);  // Enter command into omnicorp database
            cmd.ExecuteNonQuery();
        }

        public void Close()
        {
            conn.Close();
        }
    }
}

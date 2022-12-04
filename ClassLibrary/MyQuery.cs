/*
* FILE          :   MyQuery.cs
* PROJECT       :   SENG2020 - Omnicorp  project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to declare the MyQuery Class
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClassLibrary
{
   /*
   * CLASS NAME	:   MyQuery
   * DESCRIPTION	:   The purpose of this class is to perform the query operation 
   *
   */
    public class MyQuery
    {
        public MySqlConnection conn;


     /*
     * METHOD		:  MyQuery
     * DESCRIPTION	:  perform the database query 
     * PARAMETERS    : None
     *                  
     * RETURNS       :
     *                   None
     */
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

   /*
   * METHOD		:  DataReader
   * DESCRIPTION	:  perform the database query 
   * PARAMETERS    : -string query, as it represent the query
   *                  
   * RETURNS       :
   *                   None
   */
        public MySqlDataReader DataReader(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);  // Enter command into omnicorp database
            MySqlDataReader rdr = cmd.ExecuteReader();  // Read rows in in user table
            return rdr;
        }

        /*
        * METHOD		:  GetLogFileFromDatabase
        * DESCRIPTION	:  get log data from the database 
        * PARAMETERS    : None
        *                  
        * RETURNS       :
        *                   logfile
        */
        public string GetLogFileFromDatabase()
        {
            string query = "SELECT content FROM configs WHERE name = 'logFile';";
            MySqlCommand cmd = new MySqlCommand(query, conn);  // Enter command into omnicorp database
            MySqlDataReader rdr = cmd.ExecuteReader();  // Read rows in in user table
            rdr.Read();

            string logFile = rdr.GetString(0);
            logFile = logFile.Replace("/", "\\");
            return logFile;
        }

        /*
        * METHOD		:  UpdateLogFileToDatabase
        * DESCRIPTION	:  update log data from the database 
        * PARAMETERS    : -string logfile, as it represent the log file
        *                  
        * RETURNS       :
        *                  none
        */

        public void UpdateLogFileToDatabase(string logFile)
        {
            string updateQuery = $"UPDATE content SET content = {logFile} WHERE name = logFile;";
            MySqlCommand cmd = new MySqlCommand(updateQuery, conn);  // Enter command into omnicorp database
            cmd.ExecuteNonQuery();
        }


        /*
        * METHOD		:  Close()
        * DESCRIPTION	:  close the connection
        * PARAMETERS    : None
        *                  
        * RETURNS       :
        *                  none
        */
        public void Close()
        {
            conn.Close();
        }



        /*
        * METHOD		:  GetMarketplaceConfigs
        * DESCRIPTION	:  get all marketplace configs from the TMS Database
        * PARAMETERS    : 
        *                   - None
        * RETURNS       :
        *                   - Dictionary<string, string>    configs, as a dictionary with all configs
        */
        public Dictionary<string, string> GetMarketplaceConfigs()
        {
            string query = "SELECT * FROM configs WHERE name LIKE 'marketplace%';";
            Dictionary<string, string> configs = new Dictionary<string, string>();

            
            MySqlDataReader rdr = DataReader(query);

            while (rdr.Read())
            {
                string key = rdr.GetString(0).ToString();
                string value = rdr.GetString(1).ToString();

                configs.Add(key, value);
            }
            return configs;
        }
    }
}

/*
* FILE          :   MarketplaceQuery.cs
* PROJECT       :   SENG2020 - Omnicorp  project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to declare the MarketplaceQuery Class
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
    * CLASS NAME	:  MarketplaceQuery
    * DESCRIPTION	:   The purpose of this class is to perform the marketplace query operation 
    *
    */
    public class MarketplaceQuery
    {
        public MySqlConnection conn;

        /*
        * METHOD		:  MarketplaceQuery
        * DESCRIPTION	:  perform the marketplace database query 
        * PARAMETERS    : None
        *                  
        * RETURNS       :
        *                   None
        */
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
    }
}

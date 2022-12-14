/*
* FILE          :   AdminHandler.cs
* PROJECT       :   SENG2020 - Omnicorp  project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to declare the  AdminHandler Class
*/
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Windows.Controls;
using System.Data;
using System.Windows;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Omnicorp.Admin
{
    /*
    * CLASS NAME	:   AdminHandler
    * DESCRIPTION	:   The purpose of this class is to get the rates,routs ,carrier data from the database and validate and updates in the class
    *
    */
    public class AdminHandler
    {


        /*
        * METHOD		:   GetRatesFromDatabase
        * DESCRIPTION	:   to create get the data from the rates table in databse using dictionary
        * PARAMETERS    :
        *                   - None
        * RETURNS       :
        *                   - Dictionary<string, string>
        */
        public Dictionary<string, string> GetRatesFromDatabase()
        {
            MyQuery myQuery = new MyQuery();
            Dictionary<string, string> returnData = new Dictionary<string, string>();

            string ftlRateQuery = "SELECT category, amount FROM rates;";
            MySqlDataReader rdr = myQuery.DataReader(ftlRateQuery);

            while (rdr.Read())
            {
                string category = rdr.GetString(0).ToString();
                string amount = rdr.GetString(1).ToString();

                returnData.Add(category, amount);
            }
            myQuery.Close();

            return returnData;
        }

      /*
      * METHOD		:  UpdateRatesToDatabase
      * DESCRIPTION	:   try to updates rates value to the database
      * PARAMETERS    :
      *                   - decimal   amount, as the it store amount from rates table from database
      *                   - string    amount, as it store category from the rates table from database
      * RETURNS       :
      *                   - category,amount
      */
        public void UpdateRatesToDatabase(decimal amount, string category)
        {
            ValidatePositiveAmount(amount);

            MyQuery myQuery = new MyQuery();
            string updateQuery = $"UPDATE rates SET amount = {amount} WHERE category = '{category}';";
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

            Logger.Log("Update Rates", "Admin updates rates in rates table", Application.Current.Resources["logFile"].ToString());
        }


      /*
      * METHOD		:  GetCarriersFromDatabase
      * DESCRIPTION	: try to get Carriers data from the database
      * PARAMETERS    : None
      *
      * RETURNS       :
      *                   - data in form of data table
      */
        public DataTable GetCarriersFromDatabase()
        {
            string query = @"SELECT DISTINCT CompanyName AS 'Name', ftlRate AS 'FTLRate', ltlRate AS 'LTLRate', reefCharge AS 'reefCharge' FROM carriers";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

      /*
      * METHOD		:  GetCarriersDepotsFromDatabase
      * DESCRIPTION	: try to get Carriers depots data from the database using mySql query command
      * PARAMETERS    : string carrierName,
      *
      * RETURNS       :
      *                   - data in form of data table
      */
        public DataTable GetCarriersDepotsFromDatabase(string carrierName)
        {
            string query = $"SELECT depotCity, ftlAvailable, ltlAvailable FROM carriers WHERE companyName = \"{carrierName}\" ";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

        /*
       * METHOD		:  GetCorridorsFromDatabase()
       * DESCRIPTION	: try to get corridors data from the database using mySql query command
       * PARAMETERS    : None
       * RETURNS       : routs data in form of data table
       *
       */
        public DataTable GetCorridorsFromDatabase()
        {
            string query = $"SELECT destination, kms, time, west, east FROM corridors ORDER BY `sequence`;";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

      /*
      * METHOD		:  DeleteCarrierCityFromDatabase(string carrierName, string depotCity)
      * DESCRIPTION	: try to delete carrier city and name data from the database using mySql query command
      * PARAMETERS    : string carrierName, as name of the carrier
      *               : string depotCity, as the city of carrier depot
      * RETURNS       : Nothing
      *
      */
        public void DeleteCarrierCityFromDatabase(string carrierName, string depotCity)
        {
            string deleteQuery = $"DELETE FROM carriers WHERE companyName = \"{carrierName}\" AND depotCity = \"{depotCity}\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(deleteQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

            Logger.Log("Delete Carrier City", "Admin delete carrier city in carrier table", System.Windows.Application.Current.Resources["logFile"].ToString());
        }


      /*
      * METHOD		: AddCarrierCityToDatabase(
            string carrierName,
            string depotCity,
            decimal ftlAval,
            decimal ltlAval,
            decimal ftlRate,
            decimal ltlRate,
            decimal reefCharge
        )
      * DESCRIPTION	: try to add carrier city to the database using mySql query command
      * PARAMETERS    : string carrierName, as name of the carrier
      *               : string depotCity, as the city of carrier depot
      *               : decimal ftlAval, as the ftl availability for the carrier
      *               : decimal ltlAval, as the ltl availability for thr carrier
      *               : decimal ftlRate, as the ftl rates for the carrier
      *               : decimal ltlRates, as the ltl rates for the carrier
      *               : decimal reefCharge, as the carrier has the refrigarator facility
      * RETURNS       : Nothing
      *
      */
        public void AddCarrierCityToDatabase(
            string carrierName,
            string depotCity,
            decimal ftlAval,
            decimal ltlAval,
            decimal ftlRate,
            decimal ltlRate,
            decimal reefCharge
        )
        {
            // Check if parameters are valid positive
            ValidatePositiveAmount(ftlAval);
            ValidatePositiveAmount(ltlAval);
            ValidatePositiveAmount(ftlRate);
            ValidatePositiveAmount(ltlRate);
            ValidatePositiveAmount(reefCharge);

            string addQuery =   $"INSERT INTO carriers "+
                                $"(companyName, depotCity, ftlAvailable, ltlAvailable, ftlRate, ltlRate, reefCharge) " +
                                $"VALUES  (\"{carrierName}\", \"{depotCity}\", {ftlAval}, {ltlAval}, {ftlRate}, {ltlRate}, {reefCharge});";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(addQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

            Logger.Log("Add Carrier City", "Admin add carrier city in carrier table", System.Windows.Application.Current.Resources["logFile"].ToString());
        }


      /*
      * METHOD		: UpdateCarrierToDatabase(
            string carrierName,
            decimal ftlRate,
            decimal ltlRate,
            decimal reefCharge
        )
      * DESCRIPTION	: try to update carrier city to the database using mySql query command
      * PARAMETERS    : string carrierName, as name of the carrier
      *               : decimal ftlRate, as the ftl rates for the carrier
      *               : decimal ltlRates, as the ltl rates for the carrier
      *               : decimal reefCharge, as the carrier has the refrigarator facility
      * RETURNS       : Nothing
      *
      */
        public void UpdateCarrierToDatabase(string carrierName, decimal ftlRate, decimal ltlRate, decimal reefCharge)
        {
            ValidatePositiveAmount(ftlRate);
            ValidatePositiveAmount(ltlRate);
            ValidatePositiveAmount(reefCharge);

            string updateQuery =$"UPDATE carriers SET " +
                                $"reefCharge = {reefCharge}, ftlRate = {ftlRate}, ltlRate = {ltlRate} " +
                                $"WHERE companyName = \"{carrierName}\"; ";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

            Logger.Log("Update Carrier", "Admin update carrier in carrier table", System.Windows.Application.Current.Resources["logFile"].ToString());
        }


        /*
      * METHOD		: UpdateCorridorToDatabase(
            string destination,
            decimal distance,
            decimal time,

        )
      * DESCRIPTION	: try to update corridor to the database using mySql query command
      * PARAMETERS    : string destination, as name of the carrier
      *               : decimal distance, as the distance
      *               : decimal time, as the time to complete the order
      *
      * RETURNS       : Nothing
      *
      */
        public void UpdateCorridorToDatabase(string destination, decimal distance, decimal time)
        {
            ValidatePositiveAmount(distance);
            ValidatePositiveAmount(time);

            string updateQuery = $"UPDATE corridors SET kms = {distance}, time = {time} WHERE destination = \"{destination}\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

            Logger.Log("Update Corridor", "Admin update corridor in corridor table", System.Windows.Application.Current.Resources["logFile"].ToString());
        }

        /*
        * METHOD		:BackupDatabase(
              string filePath
          )
        * DESCRIPTION	: try to take backup of all the database data
        * PARAMETERS    : string filePath, as path of the file that store the database backup data
        * RETURNS       : Nothing
        *
        */
        public void BackupDatabase(string filePath)
        {
            MyQuery myQuery = new MyQuery();

            MySqlCommand cmd = new MySqlCommand("", myQuery.conn);
            MySqlBackup mb = new MySqlBackup(cmd);

            mb.ExportToFile(filePath);

            myQuery.conn.Close();

            Logger.Log("Omnicorp database backup executed", "Admin performed backup on omnicorp database", System.Windows.Application.Current.Resources["logFile"].ToString());
        }

        /*
        * METHOD		:ValidatePositiveAmount(decimal amount)
        * DESCRIPTION	: try to validate amount
        * PARAMETERS    : decimal amount, as check amount of ftl and ltl rates
        * RETURNS       : Nothing
        *
        */
        public void ValidatePositiveAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Parameter cannot be negative.");
            }
        }



        /*
        * METHOD		: UpdateMarketplaceConfig
        * DESCRIPTION	: update the marketplace configs
        * PARAMETERS    :
        *                   - string    server
        *                   - string    database
        *                   - string    port
        *                   - string    username
        *                   - string    password
        * RETURNS       :
        *                   - None
        */
        public void UpdateMarketplaceConfig(string server, string database, string port, string username, string password)
        {
            string query = $"INSERT INTO configs (name, content) " +
                            $"VALUES " +
                            $"  ('marketplaceServer', '{server}')," +
                            $"  ('marketplaceDatabase', '{database}')," +
                            $"  ('marketplacePort', '{port}')," +
                            $"  ('marketplaceUsername', '{username}')," +
                            $"  ('marketplacePassword', '{password}') " +
                            $"ON DUPLICATE KEY UPDATE name=VALUES(name), content=VALUES(content);";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }

        public void UpdateLogfile(string path)
        {
            string[] splited = path.Split('\\');
            string newPath = String.Join("/", splited);
            MyQuery myQuery = new MyQuery();

            string updateQuery = $"UPDATE configs SET content = '{newPath}' WHERE name = 'logFile';";
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path).Close();
            }
            System.IO.File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
        }
    }
}

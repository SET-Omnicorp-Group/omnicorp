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

namespace Omnicorp.Admin
{
    public class AdminHandler
    {
        

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

        public void UpdateRatesToDatabase(decimal amount, string category)
        {
            ValidatePositiveAmount(amount);

            MyQuery myQuery = new MyQuery();
            string updateQuery = $"UPDATE rates SET amount = {amount} WHERE category = '{category}';";
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }


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

        public DataTable GetRoutesFromDatabase()
        {
            string query = $"SELECT destination, distance, time, west, east FROM routes;";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

        public void DeleteCarrierCityFromDatabase(string carrierName, string depotCity)
        {
            string deleteQuery = $"DELETE FROM carriers WHERE companyName = \"{carrierName}\" AND depotCity = \"{depotCity}\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(deleteQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }

        
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
        }


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
        }

        public void UpdateRouteToDatabase(string destination, decimal distance, decimal time)
        {
            ValidatePositiveAmount(distance);
            ValidatePositiveAmount(time);

            string updateQuery = $"UPDATE routes SET distance = {distance}, time = {time} WHERE destination = \"{destination}\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }


        public void BackupDatabase(string filePath)
        {
            MyQuery myQuery = new MyQuery();

            MySqlCommand cmd = new MySqlCommand("", myQuery.conn);
            MySqlBackup mb = new MySqlBackup(cmd);
            
            mb.ExportToFile(filePath);
            
            myQuery.conn.Close();

        }

        public void ValidatePositiveAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Parameter cannot be negative.");
            }
        }
    }
}

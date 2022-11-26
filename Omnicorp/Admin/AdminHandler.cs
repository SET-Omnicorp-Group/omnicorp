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

        public void UpdateRatesToDatabase(double amount, string category)
        {
            ValidateRateAmount(amount);

            MyQuery myQuery = new MyQuery();
            string updateQuery = $"UPDATE rates SET amount = {amount} WHERE category = '{category}';";
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }

        public void ValidateRateAmount(double amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Rate amount cannot be negative.");
            }
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
            string query = $"SELECT depotCity, ftlAvailable, ltlAvailable FROM carriers WHERE companyName = '{carrierName}' ";

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
    }
}

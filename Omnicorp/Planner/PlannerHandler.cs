using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;

namespace Omnicorp.Planner
{
    internal class PlannerHandler
    {
        public DataTable GetOrdersFromDatabaseWhere(string status)
        {
            // Query data from contracts
            string activeOrderQuery = $"SELECT * FROM orders \r\nWHERE status = \"{status}\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(activeOrderQuery, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

        public DataTable GetAvailableCarriersForOrder(string orderId)
        {
            MyQuery myQuery = new MyQuery();
            
            // Query data from contract
            string orderDetailQuery = $"SELECT jobType, quantity, origin FROM orders \r\nWHERE id = \"{orderId}\";";
            MySqlDataReader rdr = myQuery.DataReader(orderDetailQuery);
            rdr.Read();

            string jobType = rdr.GetString(0).ToString();
            string quantity = rdr.GetString(1).ToString();
            string origin = rdr.GetString(2).ToString();
            
            myQuery.Close();

            string availableCarriers = string.Empty;

            if (jobType == "FTL")
            {
                availableCarriers = $"SELECT id, companyName as 'Company', ftlAvailable as 'FTL Available', ftlRate as 'FTL Rate', reefCharge as 'Reef Charge' FROM carriers WHERE " +
                                    $"depotCity = '{origin}' AND " +
                                    $"ftlAvailable >= 0;";
            }
            else
            {
                availableCarriers = $"SELECT id, companyName as 'Company', ltlAvailable as 'LTL Available', ltlRate as 'LTL Rate', reefCharge as 'Reef Charge' FROM carriers WHERE " +
                                    $"depotCity = '{origin}' AND " +
                                    $"ltlAvailable >= {quantity};";
            }

            myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(availableCarriers, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

        public int GetOrderQuantity(string orderId)
        {
            MyQuery myQuery = new MyQuery();

            string orderDetailQuery = $"SELECT quantity FROM orders WHERE id = '{orderId}';";
            MySqlDataReader rdr = myQuery.DataReader(orderDetailQuery);
            rdr.Read();

            int quantity = int.Parse(rdr.GetString(0).ToString());
            myQuery.Close();

            return quantity;
        }


        public decimal GetCurrentRate(string orderId)
        {
            MyQuery myQuery = new MyQuery();

            string orderDetailQuery = $"SELECT r.amount FROM rates r INNER JOIN orders o ON o.jobType = r.category WHERE o.id = '{orderId}';";
            MySqlDataReader rdr = myQuery.DataReader(orderDetailQuery);
            rdr.Read();

            decimal amount = decimal.Parse(rdr.GetString(0).ToString());
            myQuery.Close();

            return amount;
        }


        public void UpdateCarrierAvailability(string carrierId, string orderId)
        {
            int orderQuantity = GetOrderQuantity(orderId);

            string updateQuery = string.Empty;

            if (orderQuantity == 0)
            {
                updateQuery = $"UPDATE carriers SET ftlAvailable = ftlAvailable - 1 WHERE id = '{carrierId}'";
            }
            else
            {
                updateQuery = $"UPDATE carriers SET ltlAvailable = ltlAvailable - {orderQuantity} WHERE id = '{carrierId}'";
            }

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();

        }
        

        public void InsertRoute(string carrierId, string orderId)
        {
            decimal currentRate = GetCurrentRate(orderId);
            string query = $"INSERT INTO routes (orderId, carrierId, currentRate) VALUE ('{orderId}', '{carrierId}', '{currentRate}');";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
            
        }


        public void SetOrderToOnRoute(string orderId)
        {
            string updateQuery = $"UPDATE orders SET status = 'On Route' WHERE id = '{orderId}'";
            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(updateQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }

        public void CreateNewRoute(string orderId, string carrierId)
        {
            // decrement the avail. of the selected carrier
            UpdateCarrierAvailability(carrierId, orderId);

            // create a route
            InsertRoute(carrierId, orderId);

            // change status of order
            SetOrderToOnRoute(orderId);

        }
    }
}

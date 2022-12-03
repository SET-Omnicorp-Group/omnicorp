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
using Org.BouncyCastle.Utilities.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace Omnicorp.Planner
{
    internal class PlannerHandler
    {
        public DataTable GetOrdersFromDatabaseWhere(string status)
        {
            // Query data from contracts

            string query =  $"SELECT " +
                            $"    o.id, " +
                            $"    o.customer as 'Customer', " +
                            $"    o.jobType as 'Job Type', " +
                            $"    o.quantity as 'Pallets', " +
                            $"    o.vanType as 'Van Type', " +
                            $"    o.origin as 'Origin', " +
                            $"    o.destination as 'Destination', " +
                            $"    o.status as 'Status', " +
                            $"    r.progress as 'Progress' " +
                            $"FROM orders o " +
                            $"LEFT JOIN routes r ON o.id = r.orderId " +
                            $"WHERE o.status = '{status}';";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);

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
            MyQuery myQuery = new MyQuery();
            string orderQuery = $"SELECT origin, destination, jobType FROM orders WHERE id = '{orderId}';";
            MySqlDataReader rdr = myQuery.DataReader(orderQuery);
            rdr.Read();
            string origin = rdr.GetString(0);
            string destionation = rdr.GetString(1);
            string jobType = rdr.GetString(2);
            myQuery.Close();


            decimal totalHours = CalculateTotalHours(origin, destionation, jobType);
            int distance = CalculateDistance(origin, destionation);
            decimal currentRate = GetCurrentRate(orderId);

            string query =  $"INSERT INTO routes (orderId, distance, carrierId, currentRate, totalHours) " + 
                            $"VALUE ('{orderId}', '{distance}', '{carrierId}', '{currentRate}', '{totalHours}');";

            myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
            
        }


        public int GetCorridorSequence(string cityName)
        {
            MyQuery myQuery = new MyQuery();

            string query = $"SELECT sequence FROM corridors WHERE destination = '{cityName}';";
            MySqlDataReader rdr = myQuery.DataReader(query);
            rdr.Read();

            return int.Parse(rdr.GetString(0));
        }


        public decimal CalculateTotalHours(string origin, string destination, string jobType)
        {

            int originSequence = GetCorridorSequence(origin);
            int destinationSequence = GetCorridorSequence(destination);
            string orderBy = string.Empty;
            string where = string.Empty;

            if (originSequence < destinationSequence)
            {
                where = $"WHERE sequence >= '{originSequence}' AND sequence < '{destinationSequence}'";
                orderBy = "order by sequence asc";

            }
            else
            {
                where = $"WHERE sequence >= '{destinationSequence}' AND sequence < '{originSequence}'";
                orderBy = "order by sequence desc";
            }

            decimal time = 0;
            int numCities = 0;

            MyQuery myQuery = new MyQuery();
            string corridors = $"SELECT kms, time " +
                               $"FROM corridors " +
                               $"{where} " +
                               $"{orderBy};";
            MySqlDataReader rdr = myQuery.DataReader(corridors);

            while (rdr.Read())
            {
                time = time + rdr.GetDecimal(1);
                numCities = numCities + 1;
            }
            myQuery.Close();

            if(jobType == "FTL")
            {
                time = time + 2 + 2;
            }
            else
            {
                time = time + 2 + (2 * numCities);
            }

            return time;
        }


        public int CalculateDistance(string origin, string destination)
        {

            int originSequence = GetCorridorSequence(origin);
            int destinationSequence = GetCorridorSequence(destination);
            string direction = string.Empty;
            string orderBy = string.Empty;
            string where = string.Empty;

            if (originSequence < destinationSequence)
            {
                where = $"WHERE sequence >= '{originSequence}' AND sequence < '{destinationSequence}'";
            }
            else
            {
                where = $"WHERE sequence >= '{destinationSequence}' AND sequence < '{originSequence}'";
            }

            int distance = 0;

            MyQuery myQuery = new MyQuery();
            string corridors = $"SELECT kms, time " +
                               $"FROM corridors " +
                               $"{where};";
            MySqlDataReader rdr = myQuery.DataReader(corridors);

            while (rdr.Read())
            {
                distance = distance + rdr.GetInt32(0);
            }
            myQuery.Close();

            return distance;
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
    

        public void SimulateDay()
        {
            MyQuery myQuery = new MyQuery();
            string updateRoutes =   "UPDATE routes " +
                                    "SET drivenHours = CASE " +
                                        "WHEN totalHours - drivenHours <= 12    THEN totalHours " +
                                        "WHEN totalHours - drivenHours > 12     THEN drivenHours + 12 " +
                                        "END, " +
                                    "progress = drivenHours / totalHours * 100 " +
                                    "WHERE totalHours != drivenHours;";
            MySqlCommand cmd = new MySqlCommand(updateRoutes, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();


            myQuery = new MyQuery();
            string updateOrders = "UPDATE orders " +
                                    "INNER JOIN routes ON orders.id = routes.orderId " +
                                    "SET orders.status = 'Delivered'" +
                                    "WHERE routes.progress = 100;";
            cmd = new MySqlCommand(updateOrders, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();


        }
    
    
    }
}

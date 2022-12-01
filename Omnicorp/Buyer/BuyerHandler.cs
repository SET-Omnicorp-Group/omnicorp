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

namespace Omnicorp.Buyer
{
    internal class BuyerHandler
    {

        public DataTable GetContractsFromDatabase()
        {
            // Query data from contracts
            string query = $"SELECT Client_Name, Origin, Destination, Quantity," +
                           $"CASE " +
                           $"WHEN Job_Type = 0 THEN \"FTL\"" +
                           $"WHEN Job_Type = 1 THEN \"LTL\"" +
                           $"END AS 'Job Type'," +
                           $"CASE " +
                           $"WHEN Van_Type = 0 THEN \"Dry\"" +
                           $"WHEN Van_Type = 1 THEN \"Reefer\"" +
                           $"END AS 'Van Type'" +
                           $"FROM Contract;";

            MarketplaceQuery mpQuery = new MarketplaceQuery();
            MySqlCommand cmd = new MySqlCommand(query, mpQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            mpQuery.Close();

            return dt;
        }


        public DataTable GetActiveOrdersFromDatabase()
        {
            // Query data from contracts
            string activeOrderQuery = $"SELECT * FROM orders \r\nWHERE status = \"Active\";" ;

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(activeOrderQuery, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }


        public DataTable GetOnRouteOrdersFromDatabase()
        {
            // Query data from contracts
            string activeOrderQuery = $"SELECT * FROM orders\r\nWHERE status = \"Processing\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(activeOrderQuery, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }


        public DataTable GetCompletedOrdersFromDatabase()
        {
            // Query data from contracts
            string activeOrderQuery = $"SELECT * FROM orders \r\nWHERE status = \"Completed\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(activeOrderQuery, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }



        public void InsertContractsToOrderDatabase(
            string customer,
            string jobType,
            int quantity,
            string vanType,
            string origin,
            string destination,
            string status
        )
        {
            // Query data from contracts
            string addQuery = $"INSERT INTO orders " +
                              $"(customer, jobType, quantity, vanType, origin, destination, status) " +
                              $"VALUES  (\"{customer}\", \"{jobType}\", {quantity}, \"{vanType}\", \"{origin}\", \"{destination}\", \"{status}\");";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(addQuery, myQuery.conn);
            cmd.ExecuteNonQuery();
            myQuery.Close();
        }


        // Delete accepted contract row
        public DataTable GetOrdersFromDatabase()
        {
            string query = $"SELECT * FROM `orders`;";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(query, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }


        
    }
}

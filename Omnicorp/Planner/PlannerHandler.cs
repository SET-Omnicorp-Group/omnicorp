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

namespace Omnicorp.Planner
{
    internal class PlannerHandler
    {

        // Load the orders to the planner order datagrid
        public DataTable GetActiveOrdersFromDatabase()
        {
            // Query data from contracts
            string activeOrderQuery = $"SELECT * FROM orders \r\nWHERE status = \"Active\";";

            MyQuery myQuery = new MyQuery();
            MySqlCommand cmd = new MySqlCommand(activeOrderQuery, myQuery.conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            myQuery.Close();

            return dt;
        }

    }
}

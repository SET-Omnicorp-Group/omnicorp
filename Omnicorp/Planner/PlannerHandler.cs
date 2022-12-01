using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

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
    }
}

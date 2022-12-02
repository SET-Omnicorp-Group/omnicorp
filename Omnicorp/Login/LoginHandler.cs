using ClassLibrary;
using Omnicorp.Admin;
using Omnicorp.Buyer;
using Omnicorp.Exceptions;
using Omnicorp.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Input;

namespace Omnicorp
{
    public class LoginHandler
    {

        public void TryLogin(string usernameInput, string passwordInput)
        {
            
            MyQuery myQuery = new MyQuery();

            string query = $"SELECT role, password FROM users WHERE username = '{usernameInput}';";
            MySqlDataReader rdr = myQuery.DataReader(query);

            if (rdr.Read())                                         // If the row exists in Db
            {
                string roleDb = rdr.GetString(0).ToString();        // store role from users table
                string passwordDb = rdr.GetString(1).ToString();    // store password form users table


                // Validate if user and password entered in textbox match query
                VerifyPassword(passwordDb, passwordInput);      // verify user password
                VerifyUserRole(roleDb);                         // Validate role of user
                
                myQuery.Close();
            }
            else                                                    // If the row does not exists in Db
            {
                throw new InvalidUserException("Incorrect username or password");
            }
            return; // Exit function
        }

        // Validate password of the user
        public void VerifyPassword(string passwordDb, string passwordInput)
        {
            if (passwordInput != passwordDb)
            {
                throw new InvalidUserException("Incorrect username or password");
            }
        }


        // Validate role of the user
        public void VerifyUserRole(string role)
        {
            // If role of user is admin, open admin panel
            if (role == "admin")
            {
                AdminPanel admin = new AdminPanel();
                admin.Show();
            }

            // If role of user is planner, open planner panel
            else if (role == "planner")
            {
                PlannerPanel planner = new PlannerPanel();
                planner.Show();
            }

            // If role of user is buyer, open planner panel
            else if (role == "buyer")
            {
                BuyerPanel buyer = new BuyerPanel();
                buyer.Show();
            }

            else
            {
                throw new InvalidUserException("Role does not exist! Please select from: admin, buyer, planner");
            }
        }

      
    }
}

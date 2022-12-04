/*
* FILE          :   LoginHandler.cs
* PROJECT       :   SENG2020 - Omnicorp Project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to  declare the LoginHandler Class
*/
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
    /*
    * CLASS NAME	:   LoginHandler
    * DESCRIPTION	:   The purpose of this class is to develop the login functionality for buyer,admin and planer  
    *
    */
    public class LoginHandler
    {

        /*
        * METHOD		:  TryLogin
        * DESCRIPTION	:   this method try login according to role 
        * PARAMETERS    : -string usernameInput, as it represent the username of the user as its admin,buyer or planner
        *                  -string passwordInput, as it represent the password of the user
        * RETURNS       :
        *                   None
        */
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

            SetLogPathToCurrentApplication();
            return;
        }


        /*
        * METHOD		:  SetLogPathToCurrentApplication
        * DESCRIPTION	:  this method reads the logpath from the database and set to the current resources for all aplication
        * PARAMETERS    : 
        *                   - None
        * RETURNS       :
        *                   - None
        */
        public void SetLogPathToCurrentApplication()
        {
            MyQuery myQuery = new MyQuery();
            try
            {
                Application.Current.Resources["logFile"] = myQuery.GetLogFileFromDatabase();
            }
            catch (Exception)
            {
                string defaultLogFile = "c:\\omnicorp\\log.txt";
                MessageBox.Show($"Unable to find log file directory config.\nSetting default value to {defaultLogFile}");
                Application.Current.Resources["logFile"] = defaultLogFile;

                defaultLogFile = defaultLogFile.Replace("\\", "\\\\");
                string addQuery = $"INSERT INTO configs (name, content) VALUES ('logFile', '{defaultLogFile}');";
                MySqlCommand cmd = new MySqlCommand(addQuery, myQuery.conn);
                cmd.ExecuteNonQuery();
            }
            myQuery.Close();
        }




        /*
        * METHOD		:  VerifyPassword
        * DESCRIPTION	:   this method try varify the password  
        * PARAMETERS    : 
        *                   -string passwordDb, as it represent the passwordDb that store in database of the user as its admin,buyer or planner
        *                   -string passwordInput, as it represent the password of the user
        * RETURNS       :
        *                   None
        */
        public void VerifyPassword(string passwordDb, string passwordInput)
        {
            if (passwordInput != passwordDb)
            {
                throw new InvalidUserException("Incorrect username or password");
            }
        }


        /*
        * METHOD		:  VerifyUserRole
        * DESCRIPTION	:   this method try varify the the role of the user  
        * PARAMETERS    : -string =role, as it represent the role of the user that is admin,buyer and planner 
        *                  
        * RETURNS       :
        *                   None
        */
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

using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;
using Omnicorp.Admin;
using Omnicorp.Planner;
using Omnicorp.Buyer;


namespace Omnicorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Accessor fields
        string username;
        string password;
        bool isLoginBtnClicked;


        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public bool IsLoginBtnClicked
        {
            get { return isLoginBtnClicked; }
            set { isLoginBtnClicked = value; }
        }


        // Login submit button
        private void Login_Button(object sender, RoutedEventArgs e)
        {

            Username = Username_Text.Text;
            Password = Password_Text.Text;

            IsLoginBtnClicked = true; // Set click value to true
            string connetionString = @"server=localhost;database=omnicorp;uid=root;pwd=;"; // Write server, database, root, and password into string
            MySqlConnection connection = new MySqlConnection(connetionString);  // Establish connection between server and MySQL

            // Exception handler to open connection to MySQL database
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to database");
                return;
            }


            string query = $"SELECT * FROM users WHERE username = '{Username}' and password = '{Password}'; ";
            MySqlCommand cmd = new MySqlCommand(query, connection);  // Enter command into omnicorp database
            MySqlDataReader rdr = cmd.ExecuteReader();  // Read rows in in user table

            if (rdr.Read())
            {
                string user = rdr.GetString(1).ToString();      // store user from users table
                string password = rdr.GetString(2).ToString();  // store password from users table
                string role = rdr.GetString(3).ToString();      // store role from users table

                // Validate if user and password entered in textbox match query
                VerifyUserRole(role); // Validate role of user
                this.Close(); // Close login window
                connection.Close();
                return; // Exit function
            }

            // If while loop does not exit the function (return;), this indicates either the username or password was incorrect.
            MessageBox.Show("Incorrect username or password.");
            Username_Text.Clear();
            Password_Text.Clear();
        }


        // Validate role of the user
        private void VerifyUserRole(string role)
        {
            // If role of user is admin, open admin panel
            if (role == "admin")
            {
                admin_panel admin = new admin_panel();
                admin.Show();
            }

            // If role of user is planner, open planner panel
            else if (role == "planner")
            {
                planner_panel planner = new planner_panel();
                planner.Show();
            }

            // If role of user is buyer, open planner panel
            else if (role == "buyer")
            {
                buyer_panel buyer = new buyer_panel();
                buyer.Show();
            }

            else
            {
                MessageBox.Show("Role does not exist! Please select from: admin, buyer, planner");
            }
        }

        private void LoginEnabled(object sender, TextChangedEventArgs e)
        {
            int userNameBuff = Username_Text.Text.Length;
            int passwordBuff = Password_Text.Text.Length;

            if (userNameBuff > 0 && passwordBuff > 0)
            {
                LoginBtn.IsEnabled = true;
            }

            else
            {
                LoginBtn.IsEnabled = false;
            }
        }

    }
}
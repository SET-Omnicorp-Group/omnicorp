/*
* FILE          :   LoginWindow.xaml.cs
* PROJECT       :   SENG2020 - Omnicorp project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to  declare the MainWindow Class
*/
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
using Omnicorp.Exceptions;
using ClassLibrary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace Omnicorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
     /*
    * CLASS NAME	:   MainWindow
    * DESCRIPTION	:   The purpose of this class is to develop the main window for login 
    *
    */
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Username_Text.Focus();
        }

        // Login submit button

        /*
        * METHOD		: LoginBtn_Click
        * DESCRIPTION	:   try to perform login configuration 
        * PARAMETERS    :
        *                  - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        *                   
        * RETURNS       :
        *                   - None
        */

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginHandler loginHandler = new LoginHandler();
            string usernameInput = Username_Text.Text;
            string passwordInput = Password_Text.Password;

            try
            {
                loginHandler.TryLogin(usernameInput, passwordInput);
                this.Close();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Invalid directory for logfile. \nIf this is the first time using the software, create folder c:\\omnicorp. \nAdmin can change the logfile after first access.", "Error");
            }
            catch (InvalidUserException ex)
            {
                MessageBox.Show(ex.Message, "Login Error");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to connect to database. \nCheck App.config file.", "Error");
            }

            Username_Text.Clear();
            Username_Text.Focus();
            Password_Text.Clear();
        }


        /*
       * METHOD		: LoginEnabled(object sender, RoutedEventArgs e)
       * DESCRIPTION	:   try to perform login configuration 
       * PARAMETERS    :
       *                  - object   sender, as the sender of the object
       *                   - RoutedEventArgs    e, events
       *                   
       * RETURNS       :
       *                   - None
       */
        private void LoginEnabled(object sender, TextChangedEventArgs e)
        {
            int userNameBuff = Username_Text.Text.Length;
            int passwordBuff = Password_Text.Password.Length;

            if (userNameBuff > 0 && passwordBuff > 0)
            {
                LoginBtn.IsEnabled = true;
            }

            else
            {
                LoginBtn.IsEnabled = false;
            }
        }


        private void Password_Text_PasswordChanged(object sender, RoutedEventArgs e)
        {
            int userNameBuff = Username_Text.Text.Length;
            int passwordBuff = Password_Text.Password.Length;

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
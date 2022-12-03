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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Username_Text.Focus();
        }

        // Login submit button
        private void Login_Button(object sender, RoutedEventArgs e)
        {
            LoginHandler loginHandler = new LoginHandler();
            string usernameInput = Username_Text.Text;
            string passwordInput = Password_Text.Password;

            try
            {
                loginHandler.TryLogin(usernameInput, passwordInput);
                this.Close();
            }
            catch (InvalidUserException ex)
            {
                MessageBox.Show(ex.Message, "Login Error");
            }

            Username_Text.Clear();
            Username_Text.Focus();
            Password_Text.Clear();
        }
        

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

        private void Window_MouseDown()
        {

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
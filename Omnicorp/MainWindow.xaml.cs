using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
            set { password = value;  }
        }

        public bool IsLoginBtnClicked
        {
            get { return isLoginBtnClicked; }
            set { isLoginBtnClicked = value; }
        }


        // Username textbox
        private void Username_Textbox(object sender, TextChangedEventArgs e)
        {
            Username = Username_Text.Text; 
        }


        // Password textbox
        private void Password_Textbox(object sender, TextChangedEventArgs e)
        {
            Password = Password_Text.Text;
        }


        // Login submit button
        private void Login_Button(object sender, RoutedEventArgs e)
        {
            IsLoginBtnClicked = true;
            MessageBox.Show($"{Username.ToString()} - {Password.ToString()}");
        }

    
    }
}

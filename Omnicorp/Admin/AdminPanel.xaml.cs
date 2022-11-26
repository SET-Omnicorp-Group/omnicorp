using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using MySql.Data;
using System.Diagnostics;
using MySqlX.XDevAPI;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;

namespace Omnicorp.Admin
{

    /// <summary>
    /// Interaction logic for admin_panel.xaml
    /// </summary>
    /// 
    
    public partial class AdminPanel : Window
    {

        // Accessor fields
        AdminHandler handler;


        public string FltRate { get; set; }
        public string LtlRate { get; set; }
        public string CarrierName { get; set; }
        public decimal FtlRateCarriers { get; set; }
        public decimal LtlRateCarriers { get; set; }
        public decimal ReefCharge { get; set; }
        public string DepotCity { get; set; }
        public decimal FtlAval { get; set; }
        public decimal LtlAval { get; set; }

        public AdminPanel()
        {
            InitializeComponent();
            handler = new AdminHandler();
        }

        // Database button
        private void DatabaseBtnClick(object sender, RoutedEventArgs e)
        {
            RatesBtn.Visibility = Visibility.Visible;
            CarriersBtn.Visibility = Visibility.Visible;
            RoutesBtn.Visibility = Visibility.Visible;
            RatesGrid_Click.Visibility = Visibility.Hidden;
            CarriersGrid_Click.Visibility = Visibility.Hidden;
            RoutesGrid_Click.Visibility = Visibility.Hidden;
            BackupGrid.Visibility = Visibility.Hidden;
            LogFileGrid.Visibility = Visibility.Hidden;
        }

        

        
        
        
        // MANAGING RATES
        private void RatesBtnClick(object sender, RoutedEventArgs e)
        {
            HideBtns();
            RatesGrid_Click.Visibility = Visibility.Visible;

            UpdateRatesDatagridContent();
        }
        
        
        
        
        private void UpdateRatesDatagridContent()
        {
            Dictionary<string, string> data = handler.GetRatesFromDatabase();
            Ftl_Textbox.Text = data["FTL"].ToString();
            Ltl_Textbox.Text = data["LTL"].ToString();
        }




        private void SaveFTLRate(object sender, RoutedEventArgs e)
        {
            double amount = double.Parse(Ftl_Textbox.Text);
            string message = "FTL value has been updated.";
            string title = "Success";
            try
            {
                handler.UpdateRatesToDatabase(amount, "FTL");
            }
            catch (ArgumentException err)
            {
                message = err.Message;
                title = "Error: invalid entry";
            }
            MessageBox.Show(message, title);
            UpdateRatesDatagridContent();
        }


        
        
        private void SaveLTLRate(object sender, RoutedEventArgs e)
        {
            double amount = double.Parse(Ltl_Textbox.Text);
            string message = "LTL value has been updated.";
            string title = "Success";
            try
            {
                handler.UpdateRatesToDatabase(amount, "LTL");
            }
            catch (ArgumentException err)
            {
                message = err.Message;
                title = "Error: invalid entry";
            }
            MessageBox.Show(message, title);
            UpdateRatesDatagridContent();
        }






        // MANAGING CARRIERS
        private void CarriersBtnClick(object sender, RoutedEventArgs e)
        {
            HideBtns();
            CarriersGrid_Click.Visibility = Visibility.Visible;
            
            UpdateCarriersDatagridContent();
        }




        private void UpdateCarriersDatagridContent()
        {
            Carriers_Data.DataContext = handler.GetCarriersFromDatabase();
        }



        private void CarriersDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            // If no row is selected, clear the details and skip
            if (rowSelected == null)
            {
                CarrierDetailName.Content = String.Empty;
                ReeferCharge_Textbox.Text = String.Empty;
                FTL_Textbox.Text = String.Empty;
                LTL_Textbox.Text = String.Empty;
                return;
            }

            CarrierName = (string)rowSelected[0];
            FtlRateCarriers = (decimal)rowSelected[1];
            LtlRateCarriers = (decimal)rowSelected[2];
            ReefCharge = (decimal)rowSelected[3];
            CarriersCity_Datagrid();

            CarrierDetailName.Content = CarrierName.ToString();
            ReeferCharge_Textbox.Text = ReefCharge.ToString();
            FTL_Textbox.Text = FtlRateCarriers.ToString();
            LTL_Textbox.Text = LtlRateCarriers.ToString();
        }




        private void Carriers_City_Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            if (rowSelected == null)
            {
                return;
            }

            DepotCity = (string)rowSelected[0];
            FtlAval = (int)rowSelected[1];
            LtlAval = (int)rowSelected[2];
            CityOfCarrierDetails();
        }

        


        // Carriers City
        private void CarriersCity_Datagrid()
        {
            Carriers_City_Data.DataContext = handler.GetCarriersDepotsFromDatabase(CarrierName);
        }





        // Routes button
        private void Routes_Btn(object sender, RoutedEventArgs e)
        {
            HideBtns();
            RoutesGrid_Click.Visibility = Visibility.Visible;
            
            UpdateRoutesDatagridContent();
        }


        // Hide database btns
        private void HideBtns()
        {
            RatesBtn.Visibility = Visibility.Hidden;
            CarriersBtn.Visibility = Visibility.Hidden;
            RoutesBtn.Visibility = Visibility.Hidden;
        }

        


        // Carriers Data
        


        
        

        // Fill text box with city of carrier details
        private void CityOfCarrierDetails()
        {
            Departure_Textbox.Text = DepotCity.ToString();
            FTLAval_Textbox.Text = FtlAval.ToString();
            LTLAval_Textbox.Text = LtlAval.ToString();
        }

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            ReeferCharge_Textbox.Text = "";
            FTL_Textbox.Text = "";
            LTL_Textbox.Text = "";
        }

        private void UpdateCarrierDetails(object sender, RoutedEventArgs e)
        {

            // If carrier detail fields are empty, prompt an error message
            if (ReeferCharge_Textbox.Text == "" ||
                FTL_Textbox.Text == "" || LTL_Textbox.Text == "")
            {
                MessageBox.Show("Please make sure that all carrier textfields are filled before saving.");
            }
            else
            {
                UpdateCarrierDetails();
            }

        }

        // Update carrier details
        private void UpdateCarrierDetails()
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string reef = ReeferCharge_Textbox.Text;
            decimal reefCharge = decimal.Parse(reef);

            string ftl = FTL_Textbox.Text;
            decimal ftlRate = decimal.Parse(ftl);

            string ltl = LTL_Textbox.Text;
            decimal ltlRate = decimal.Parse(ltl);

            string carrierName = (string)CarrierDetailName.Content;
            string updateQuery = $"UPDATE carriers SET reefCharge = {reefCharge}, ftlRate = {ftlRate}, ltlRate = {ltlRate} WHERE companyName = \"{carrierName}\" ";

            MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            MessageBox.Show("Carrier values have been updated.");
            UpdateCarriersDatagridContent();
        }


        // Add carrier button
        private void AddCarrier(object sender, RoutedEventArgs e)
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string carrierName = CarrierDetailName.Content.ToString();
            string depotCity = Departure_Textbox.Text;

            string ftl = FTLAval_Textbox.Text;
            decimal ftlAval = decimal.Parse(ftl);

            string ltl = LTLAval_Textbox.Text;
            decimal ltlAval = decimal.Parse(ltl);

            string ftlR = FTL_Textbox.Text;
            decimal ftlRate = decimal.Parse(ftlR);

            string ltlR = LTL_Textbox.Text;
            decimal ltlRate = decimal.Parse(ltlR);

            string reefC = ReeferCharge_Textbox.Text;
            decimal reefCharge = decimal.Parse(reefC);

            string addQuery = $"INSERT INTO carriers (companyName, depotCity, ftlAvailable, ltlAvailable, ftlRate, ltlRate, reefCharge) " +
                              $"VALUES  (\"{carrierName}\", \"{depotCity}\", {ftlAval}, {ltlAval}, {ftlRate}, {ltlRate}, {reefCharge}) ";

            MessageBox.Show($"Record for {carrierName} has been updated");

            MySqlCommand cmd = new MySqlCommand(addQuery, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            CarriersCity_Datagrid();
        }

        private void DeleteCarrier(object sender, RoutedEventArgs e)
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string depotCity = Departure_Textbox.Text;

            string ftl = FTLAval_Textbox.Text;
            decimal ftlAval = decimal.Parse(ftl);

            string ltl = LTLAval_Textbox.Text;
            decimal ltlAval = decimal.Parse(ltl);

            string deleteQuery = $"DELETE FROM carriers WHERE depotCity = \"{depotCity}\" AND ftlAvailable = {ftlAval} AND ltlAvailable = {ltlAval};";

            MessageBox.Show($"Record for {depotCity} been deleted");

            MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            CarriersCity_Datagrid();

            Departure_Textbox.Text = "";
            FTLAval_Textbox.Text = "";
            LTLAval_Textbox.Text = "";

        }

        // Routes datagrid
        private void UpdateRoutesDatagridContent()
        {
            Routes_Data.DataContext = handler.GetRoutesFromDatabase();
        }

        private void Routes_Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            if (rowSelected == null)
            {
                return;
            }

            string Destination = (string)rowSelected[0];
            int Distance = (int)rowSelected[1];
            decimal Time = (decimal)rowSelected[2];
            string East = (string)rowSelected[3];
            string West = (string)rowSelected[4];

            Destination_Textbox.Text = Destination;
            Distance_Textbox.Text = Distance.ToString();
            Time_Textbox.Text = Time.ToString();
            East_Textbox.Text = East.ToString();
            West_Textbox.Text = West.ToString();

        }

        // Clear routes fields
        private void Routes_ClearFields(object sender, RoutedEventArgs e)
        {
            Distance_Textbox.Text = "";
            Time_Textbox.Text = "";
        }

        // Update routes fields
        private void Routes_UpdateBtn(object sender, RoutedEventArgs e)
        {
            // If carrier detail fields are empty, prompt an error message
            if (Distance_Textbox.Text == "" || Time_Textbox.Text == "")
            {
                MessageBox.Show("Please make sure that all route textfields are filled before saving.");
            }
            else
            {
                UpdateRouteDetails();
            }
        }

        private void UpdateRouteDetails()
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string destination = Destination_Textbox.Text;

            string dist= Distance_Textbox.Text;
            decimal distance = decimal.Parse(dist);

            string time = Time_Textbox.Text;
            decimal timeField = decimal.Parse(time);

            string updateQuery = $"UPDATE routes SET distance = {distance}, time = {timeField} WHERE destination = \"{destination}\" ";

            MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            MessageBox.Show("Routes values have been updated.");
            UpdateRoutesDatagridContent();

        }



        // Left hand backup button
        private void Backup_Btn(object sender, RoutedEventArgs e)
        {
            LogFileGrid.Visibility = Visibility.Hidden;
            BackupGrid.Visibility = Visibility.Visible;
        }


        // Select folder directory
        private void SelectDirectory_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();  // Open file dialog
            System.Windows.Forms.FolderBrowserDialog folderDlg = new System.Windows.Forms.FolderBrowserDialog();

            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            System.Windows.Forms.DialogResult result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                backup_textbox.Text = folderDlg.SelectedPath + "\\";
            }
        }


        // Intiate backup
        private void InitBackupBtn_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "mysqldump.exe";
            process.StartInfo.Arguments = @"mysqldump -uroot -p omnicorp > C:\User\aanwa\Desktop\backup.sql";
            process.Start();
            process.WaitForExit();

        }



        // Log file button
        private void Logfile_Btn(object sender, RoutedEventArgs e)
        {
            LogFileGrid.Visibility = Visibility.Visible;
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result

            // This will get the current PROJECT bin directory (ie ../bin/)
            string logfileDirectory = Directory.GetParent(workingDirectory).Parent.FullName + @"\Admin\logfile.txt";

            LogFileText.Text = File.ReadAllText(logfileDirectory);
        }


        // General Config button
        private void GeneralConfig_Btn(object sender, RoutedEventArgs e)
        {

        }
    }
}
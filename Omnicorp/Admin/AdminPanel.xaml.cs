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
        private void DatabaseBtn_Click(object sender, RoutedEventArgs e)
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
        private void RatesBtn_Click(object sender, RoutedEventArgs e)
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
            decimal amount = decimal.Parse(Ftl_Textbox.Text);
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
            decimal amount = decimal.Parse(Ltl_Textbox.Text);
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
        private void CarriersBtn_Click(object sender, RoutedEventArgs e)
        {
            HideBtns();
            CarriersGrid_Click.Visibility = Visibility.Visible;
            
            UpdateCarriersDatagridContent();
        }


        private void UpdateCarriersDatagridContent()
        {
            CarriersData.DataContext = handler.GetCarriersFromDatabase();
        }


        private void CarriersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            // If no row is selected, clear the details, cities and skip
            if (rowSelected == null)
            {
                CarrierDetailName.Content = String.Empty;
                ReeferCharge_Textbox.Text = String.Empty;
                FTL_Textbox.Text = String.Empty;
                LTL_Textbox.Text = String.Empty;
                UpdateCarriersCityDatagridContent(null);
                return;
            }

            // Update cities datagrid
            string carrierName = rowSelected[0].ToString();
            UpdateCarriersCityDatagridContent(carrierName);

            // Update details
            CarrierDetailName.Content = rowSelected[0].ToString();
            FTL_Textbox.Text = rowSelected[1].ToString();
            LTL_Textbox.Text = rowSelected[2].ToString();
            ReeferCharge_Textbox.Text = rowSelected[3].ToString();
        }


        private void UpdateCarriersCityDatagridContent(string carrierName)
        {
            if (carrierName == null)
            {
                CarriersCityData.DataContext = null;
            }
            CarriersCityData.DataContext = handler.GetCarriersDepotsFromDatabase(carrierName);
        }


        private void CarriersCityData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            // If no row is selected, clear the details and skip
            if (rowSelected == null)
            {
                Departure_Textbox.Text = String.Empty;
                FTLAval_Textbox.Text = String.Empty;
                LTLAval_Textbox.Text = String.Empty;
                return;
            }

            Departure_Textbox.Text = rowSelected[0].ToString();
            FTLAval_Textbox.Text = rowSelected[1].ToString();
            LTLAval_Textbox.Text = rowSelected[2].ToString();
        }


        private void DeleteCarrierCityBtn_Click(object sender, RoutedEventArgs e)
        {
            string carrierName = CarrierDetailName.Content.ToString();
            string depotCity = Departure_Textbox.Text;
            
            string message = $"Record for {depotCity} been deleted.";
            string title = "Success";
            try
            {
                handler.DeleteCarrierCityFromDatabase(carrierName, depotCity);
            }
            catch (Exception err)
            {
                message = err.Message;
                title = "Error: invalid entry";
            }
            MessageBox.Show(message, title);

            Departure_Textbox.Text = string.Empty;
            FTLAval_Textbox.Text = string.Empty;
            LTLAval_Textbox.Text = string.Empty;

        }

        
        private void AddCarrierCityBtn_Click(object sender, RoutedEventArgs e)
        {

            string carrierName = CarrierDetailName.Content.ToString();
            string depotCity = Departure_Textbox.Text;
            string ftl = FTLAval_Textbox.Text;
            string ltl = LTLAval_Textbox.Text;
            string ftlR = FTL_Textbox.Text;
            string ltlR = LTL_Textbox.Text;
            string reefC = ReeferCharge_Textbox.Text;

            string message = $"Record for {carrierName} has been added";
            string title = "Success";
            try
            {
                decimal ftlAval = decimal.Parse(ftl);
                decimal ltlAval = decimal.Parse(ltl);
                decimal ftlRate = decimal.Parse(ftlR);
                decimal ltlRate = decimal.Parse(ltlR);
                decimal reefCharge = decimal.Parse(reefC);
                handler.AddCarrierCityToDatabase(carrierName, depotCity, ftlAval, ltlAval, ftlRate, ltlRate, reefCharge);
                UpdateCarriersCityDatagridContent(carrierName);
            }
            catch (FormatException)
            {
                message = "Rates, Charge and Available must be numeric.";
                title = "Error: invalid entry";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Error: invalid entry";
            }
            
            MessageBox.Show(message, title);
        }


        private void UpdateCarrierBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = "Carrier values have been updated.";
            string title = "Success";

            // If carrier detail fields are empty, prompt an error message
            if (ReeferCharge_Textbox.Text == string.Empty ||
                FTL_Textbox.Text == string.Empty ||
                LTL_Textbox.Text == string.Empty
            )
            {
                message = "Please make sure that all carrier textfields are filled before saving.";
                title = "Error: invalid entry";
            }
            // If carrier detail fields are not empty, try to parse and save it
            else
            {
                string carrierName = CarrierDetailName.Content.ToString();
                string reef = ReeferCharge_Textbox.Text;
                string ftl = FTL_Textbox.Text;
                string ltl = LTL_Textbox.Text;

                try
                {
                    decimal reefCharge = decimal.Parse(reef);
                    decimal ftlRate = decimal.Parse(ftl);
                    decimal ltlRate = decimal.Parse(ltl);
                    handler.UpdateCarrierToDatabase(carrierName, ftlRate, ltlRate, reefCharge);
                    UpdateCarriersDatagridContent();
                }
                catch (FormatException)
                {
                    message = "Rates and Charge must be numeric.";
                    title = "Error: invalid entry";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    title = "Error: invalid entry";
                }

            }

            MessageBox.Show(message, title);

        }


        private void ClearCarrierFieldsBtn_Click(object sender, RoutedEventArgs e)
        {
            ReeferCharge_Textbox.Text = string.Empty;
            FTL_Textbox.Text = string.Empty;
            LTL_Textbox.Text = string.Empty;
        }







        // MANAGING ROUTES
        private void Routes_Btn(object sender, RoutedEventArgs e)
        {
            HideBtns();
            RoutesGrid_Click.Visibility = Visibility.Visible;
            
            UpdateRoutesDatagridContent();
        }

                
        private void UpdateRoutesDatagridContent()
        {
            Routes_Data.DataContext = handler.GetRoutesFromDatabase();
        }


        private void HideBtns()
        {
            RatesBtn.Visibility = Visibility.Hidden;
            CarriersBtn.Visibility = Visibility.Hidden;
            RoutesBtn.Visibility = Visibility.Hidden;
        }
      

        private void RoutesData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            // If selected row is empty, clean the details
            if (rowSelected == null)
            {
                Destination_Textbox.Text = string.Empty;
                Distance_Textbox.Text = string.Empty;
                Time_Textbox.Text = string.Empty;
                East_Textbox.Text = string.Empty;
                West_Textbox.Text = string.Empty;
                return;
            }

            Destination_Textbox.Text = rowSelected[0].ToString();
            Distance_Textbox.Text = rowSelected[1].ToString();
            Time_Textbox.Text = rowSelected[2].ToString();
            East_Textbox.Text = rowSelected[3].ToString();
            West_Textbox.Text = rowSelected[4].ToString();

        }

        
        private void ClearRoutesFieldsBtn_Click(object sender, RoutedEventArgs e)
        {
            Distance_Textbox.Text = string.Empty;
            Time_Textbox.Text = string.Empty;
        }


        private void UpdateRoutesBtn_Click(object sender, RoutedEventArgs e)
        {
            
            string message = "Carrier values have been updated.";
            string title = "Success";

            // If routes detail fields are empty, prompt an error message
            if (Distance_Textbox.Text == string.Empty || Time_Textbox.Text == string.Empty)
            {
                message = "Please make sure that all route textfields are filled before saving.";
                title = "Error: invalid entry";
            }
            // If route detail fields are not empty, try to parse and save it
            else
            {
                string destination = Destination_Textbox.Text;
                string dist = Distance_Textbox.Text;
                string time = Time_Textbox.Text;

                try
                {
                    decimal distance = decimal.Parse(dist);
                    decimal timeField = decimal.Parse(time);

                    handler.UpdateRouteToDatabase(destination, distance, timeField);
                    UpdateCarriersDatagridContent();
                }
                catch (FormatException)
                {
                    message = "Distance and Time must be numeric.";
                    title = "Error: invalid entry";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    title = "Error: invalid entry";
                }

                MessageBox.Show(message, title);
            }
        }







        // MANAGING LOG
        private void LogFileBtn_Click(object sender, RoutedEventArgs e)
        {
            LogFileGrid.Visibility = Visibility.Visible;
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result

            // This will get the current PROJECT bin directory (ie ../bin/)
            string logfileDirectory = Directory.GetParent(workingDirectory).Parent.FullName + @"\Admin\logfile.txt";

            LogFileText.Text = File.ReadAllText(logfileDirectory);
        }







        // MANAGING BACKUP
        private void BackupBtn_Click(object sender, RoutedEventArgs e)
        {
            LogFileGrid.Visibility = Visibility.Hidden;
            BackupGrid.Visibility = Visibility.Visible;
        }


        private void SelectDirectoryBtn_Click(object sender, RoutedEventArgs e)
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


        private void InitBackupBtn_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "mysqldump.exe";
            process.StartInfo.Arguments = @"mysqldump -uroot -p omnicorp > C:\User\aanwa\Desktop\backup.sql";
            process.Start();
            process.WaitForExit();

        }







        // MANAGING BACKUP
        private void GeneralConfig_Btn(object sender, RoutedEventArgs e)
        {

        }
    }
}
/*
* FILE          :   AdminPanel.xaml.cs
* PROJECT       :   SENG2020 - Omnicorp  project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to design the admin panel window
*/
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
using System.Windows.Media;
using ClassLibrary;

namespace Omnicorp.Admin
{

    /// <summary>
    /// Interaction logic for admin_panel.xaml
    /// </summary>
    /// 

    /*
    * CLASS NAME	:   AdminPanel
    * DESCRIPTION	:   The purpose of this class is to perform the admin user functionality.
    * 
    * DATA MEMBERS  :   
    *                   - Dictionary    activeGameSessions, as the sessions active on the current server
    *                   - string        FltRate, full loaded truck rate
    *                   - string        LtlRate, half loaded truck rate
    *                   - string        CarrierName, name of the carrier that delievery the package
    *                   - decimal       FtlRateCarriers, as the Full loaded trucs carriers
    *                   - decimal       LtlRateCarriers, as the half loaded trucks carrier
    *                   - decimal       ReefCharge, as the refrigerator charge of the carrier
    *                   - string        DepotCity, as the depotcity of the carrier
    *                   - decimal       FtlAval, as a Ftl availability
    *                   - decimal       LtlAval, as the Ltl availability
    *                   
    */
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
        
        /*
        * METHOD		:  DatabaseBtn_Click
        * DESCRIPTION	:   try to visible the various button functionality
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void DatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivateButton("DatabaseBtn");
            HideAllElements();

            RatesBtn.Visibility = Visibility.Visible;
            CarriersBtn.Visibility = Visibility.Visible;
            CorridorsBtn.Visibility = Visibility.Visible;
        }






        // MANAGING RATES
       

        /*
        * METHOD		: RatesBtn_Click
        * DESCRIPTION	:   try to perform rates functionality
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void RatesBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAllElements();
            RatesGrid.Visibility = Visibility.Visible;

            UpdateRatesDatagridContent();
        }


        /*
         * METHOD		: UpdateRatesDatagridContent()
         * DESCRIPTION	:   try to the grid view
         * PARAMETERS    :None
         *                  
         * RETURNS       :
         *                   - None
         */
        private void UpdateRatesDatagridContent()
        {
            Dictionary<string, string> data = handler.GetRatesFromDatabase();
            Ftl_Textbox.Text = data["FTL"].ToString();
            Ltl_Textbox.Text = data["LTL"].ToString();
        }

        /*
         * METHOD		: SaveFTLRate(object sender, RoutedEventArgs e)
         * DESCRIPTION	:   try to save Ftl rates to datbase
         * PARAMETERS    :
         *                   - object   sender, as the sender of the object
         *                   - RoutedEventArgs    e, events
         * RETURNS       :
         *                   - None
         */
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

        /*
        * METHOD		: SaveLTLRate(object sender, RoutedEventArgs e)
        * DESCRIPTION	:   try to save Ltl rates to database
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
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
        /*
        * METHOD		: CarriersBtn_Click(object sender, RoutedEventArgs e)
        * DESCRIPTION	:   try to perform Carriers functionality and hide the button and visible carrier grid view
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void CarriersBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAllElements();
            CarriersGrid.Visibility = Visibility.Visible;
            
            UpdateCarriersDatagridContent();
        }


        /*
        * METHOD		: UpdateCarriersDatagridContent
        * DESCRIPTION	:   try to update Carriers functionality 
        * PARAMETERS    :
        *                  None
        * RETURNS       :
        *                   - None
        */
        private void UpdateCarriersDatagridContent()
        {
            CarriersData.DataContext = handler.GetCarriersFromDatabase();
        }


        /*
        * METHOD		: CarriersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        * DESCRIPTION	:   try to changed the selection of carrier grid view
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
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


        /*
        * METHOD		: UpdateCarriersCityDatagridContent(string carrierName)
        * DESCRIPTION	:   try to update carrier city data content
        * PARAMETERS    :
        *                   - string   carrierName, as the name of the carrier
        *                   
        * RETURNS       :
        *                   - None
        */
        private void UpdateCarriersCityDatagridContent(string carrierName)
        {
            if (carrierName == null)
            {
                CarriersCityData.DataContext = null;
            }
            CarriersCityData.DataContext = handler.GetCarriersDepotsFromDatabase(carrierName);
        }


        /*
       * METHOD		:  CarriersCityData_SelectionChanged(object sender, SelectionChangedEventArgs e)
       * DESCRIPTION	:   try to changed the selection view of carrier city data
       * PARAMETERS    :
       *                  - object   sender, as the sender of the object
       *                   - RoutedEventArgs    e, events
       *                   
       * RETURNS       :
       *                   - None
       */
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


        /*
        * METHOD		:  DeleteCarrierCityBtn_Click(object sender, RoutedEventArgs e)
        * DESCRIPTION	:   try to delete the selection view of carrier city data
        * PARAMETERS    :
        *                  - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        *                   
        * RETURNS       :
        *                   - None
        */
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
            UpdateCarriersCityDatagridContent(carrierName);

        }


       /*
       * METHOD		:  AddCarrierCityBtn_Click(object sender, RoutedEventArgs e)
       * DESCRIPTION	:   try to add carrier city 
       * PARAMETERS    :
       *                  - object   sender, as the sender of the object
       *                   - RoutedEventArgs    e, events
       *                   
       * RETURNS       :
       *                   - None
       */
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
            catch (FormatException fe)
            {
                message = "Rates, Charge and Available must be numeric.";
                title = "Error: invalid entry";
            }
            catch (MySqlException)
            {
                message = "Invalid departure city.";
                title = "Error: invalid entry";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Error: invalid entry";
            }
            
            MessageBox.Show(message, title);
        }

        /*
       * METHOD		:  UpdateCarrierBtn_Click(object sender, RoutedEventArgs e)
       * DESCRIPTION	:   try to update carrier city value
       * PARAMETERS    :
       *                  - object   sender, as the sender of the object
       *                   - RoutedEventArgs    e, events
       *                   
       * RETURNS       :
       *                   - None
       */
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


        /*
       * METHOD		:  ClearCarrierFieldsBtn_Click(object sender, RoutedEventArgs e)
       * DESCRIPTION	:   try to clear carrier value
       * PARAMETERS    :
       *                  - object   sender, as the sender of the object
       *                   - RoutedEventArgs    e, events
       *                   
       * RETURNS       :
       *                   - None
       */
        private void ClearCarrierFieldsBtn_Click(object sender, RoutedEventArgs e)
        {
            ReeferCharge_Textbox.Text = string.Empty;
            FTL_Textbox.Text = string.Empty;
            LTL_Textbox.Text = string.Empty;
        }







        // MANAGING CORRIDORS
        /*
      * METHOD		:  Corridors_Btn(object sender, RoutedEventArgs e)
      * DESCRIPTION	:   try to manage corridors
      * PARAMETERS    :
      *                  - object   sender, as the sender of the object
      *                   - RoutedEventArgs    e, events
      *                   
      * RETURNS       :
      *                   - None
      */
        private void Corridors_Btn(object sender, RoutedEventArgs e)
        {
            HideAllElements();
            CorridorsGrid.Visibility = Visibility.Visible;
            
            UpdateCorridorsDatagridContent();
        }

    /*
    * METHOD		:  UpdateCorridorsDatagridContent()
    * DESCRIPTION	:   try to updates corridors value
    * PARAMETERS    :
    *                  - object   sender, as the sender of the object
    *                   - RoutedEventArgs    e, events
    *                   
    * RETURNS       :
    *                   - None
    */
        private void UpdateCorridorsDatagridContent()
        {
            Corridors_Data.DataContext = handler.GetCorridorsFromDatabase();
        }



        /*
      * METHOD		:  CorridorsData_SelectionChanged(object sender, SelectionChangedEventArgs e)
      * DESCRIPTION	:   try to perform actions on corridors data
      * PARAMETERS    :
      *                  - object   sender, as the sender of the object
      *                   - RoutedEventArgs    e, events
      *                   
      * RETURNS       :
      *                   - None
      */
        private void CorridorsData_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        /*
        * METHOD		:  ClearCorridorsFieldsBtn_Click(object sender, RoutedEventArgs e)
        * DESCRIPTION	:   try to clear corridors data
        * PARAMETERS    :
        *                  - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        *                   
        * RETURNS       :
        *                   - None
        */
        private void ClearCorridorsFieldsBtn_Click(object sender, RoutedEventArgs e)
        {
            Distance_Textbox.Text = string.Empty;
            Time_Textbox.Text = string.Empty;
        }

     /*
     * METHOD		: UpdateCorridorsBtn_Click(object sender, RoutedEventArgs e)
     * DESCRIPTION	:   try to updates corridors data
     * PARAMETERS    :
     *                  - object   sender, as the sender of the object
     *                   - RoutedEventArgs    e, events
     *                   
     * RETURNS       :
     *                   - None
     */
        private void UpdateCorridorsBtn_Click(object sender, RoutedEventArgs e)
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

                    handler.UpdateCorridorToDatabase(destination, distance, timeField);
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
        /*
        * METHOD		:   LogFileBtn_Click(object sender, RoutedEventArgs e)
        * DESCRIPTION	:   try to manage the log data
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        *                   
        * RETURNS       :
        *                   - None
        */
        private void LogFileBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivateButton("LogFileBtn");
            HideAllElements();
            
            LogFileGrid.Visibility = Visibility.Visible;
            string logFile = Application.Current.Resources["logFile"].ToString();

            LogFileText.Text = File.ReadAllText(logFile);
        }







        /*
        * METHOD		: BackupBtn_Click
        * DESCRIPTION	:   try to save the data from the database 
        * PARAMETERS    :
        *                  - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        *                   
        * RETURNS       :
        *                   - None
        */
        private void BackupBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAllElements();
            ActivateButton("BackupBtn");
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "SQL Files | *.sql"; ;
            bool? res = sf.ShowDialog();

            if (res == true)
            {
                string filePath = sf.FileName.ToString();
                try
                {
                    handler.BackupDatabase(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Backup unsuccessful. Check logfile for details.", "Error");
                    // @TODO LOG HERE
                }
            }
        }





        // MANAGING BACKUP
        /*
        * METHOD		: GeneralConfigBtn_Click(object sender, RoutedEventArgs e)
        * DESCRIPTION	:   try to perform general configuration 
        * PARAMETERS    :
        *                  - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        *                   
        * RETURNS       :
        *                   - None
        */
        private void GeneralConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivateButton("GeneralConfigBtn");
            HideAllElements();
            GeneralConfigGrid.Visibility = Visibility.Visible;

            Dictionary<string, string> configs = new Dictionary<string, string>();
            MyQuery myQuery = new MyQuery();
            configs = myQuery.GetMarketplaceConfigs();
            myQuery.Close();

            ServerInput.Text = configs["marketplaceServer"];
            DatabaseInput.Text = configs["marketplaceDatabase"];
            PortInput.Text = configs["marketplacePort"];
            UsernameInput.Text = configs["marketplaceUsername"];
            PasswordInput.Text = configs["marketplacePassword"];

        }



        /*
        * METHOD		: ActivateButton
        * DESCRIPTION	: change the style of all buttons and enhance the one selected
        * PARAMETERS    :
        *                   - string    btn, as the button name to be activated
        * RETURNS       :
        *                   - None
        */
        private void ActivateButton(string btn)
        {
            GeneralConfigBtn.Background = Brushes.Transparent;
            GeneralConfigBtn.Foreground = Brushes.White;

            DatabaseBtn.Background = Brushes.Transparent;
            DatabaseBtn.Foreground = Brushes.White;

            BackupBtn.Background = Brushes.Transparent;
            BackupBtn.Foreground = Brushes.White;

            LogFileBtn.Background = Brushes.Transparent;
            LogFileBtn.Foreground = Brushes.White;

            if(btn == "GeneralConfigBtn")
            {
                GeneralConfigBtn.Background = Brushes.White;
                GeneralConfigBtn.Foreground = Brushes.Black;
            }
            else if (btn == "DatabaseBtn")
            {
                DatabaseBtn.Background = Brushes.White;
                DatabaseBtn.Foreground = Brushes.Black;
            }
            else if(btn == "BackupBtn")
            {
                BackupBtn.Background = Brushes.White;
                BackupBtn.Foreground = Brushes.Black;
            }
            else if(btn == "LogFileBtn")
            {
                LogFileBtn.Background = Brushes.White;
                LogFileBtn.Foreground = Brushes.Black;
            }
        }

        public void HideAllElements()
        {
            LogFileGrid.Visibility = Visibility.Hidden;
            GeneralConfigGrid.Visibility = Visibility.Hidden;

            // Databases subgrids
            RatesGrid.Visibility = Visibility.Hidden;
            CarriersGrid.Visibility = Visibility.Hidden;
            CorridorsGrid.Visibility = Visibility.Hidden;

            // Secondary Buttons
            RatesBtn.Visibility = Visibility.Hidden;
            CarriersBtn.Visibility = Visibility.Hidden;
            CorridorsBtn.Visibility = Visibility.Hidden;
        }

        private void MarketplaceDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            string server = ServerInput.Text;
            string database = DatabaseInput.Text;
            string port = PortInput.Text;
            string username = UsernameInput.Text;
            string password = PasswordInput.Text;
            try
            {
                handler.UpdateMarketplaceConfig(server, database, port, username, password);
                MessageBox.Show("Marketplace configs update.", "Success");
            }
            catch(Exception ex)
            {
                // @TODO log here!
            }
            

        }
    }
}
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


namespace Omnicorp.Admin
{
    /// <summary>
    /// Interaction logic for admin_panel.xaml
    /// </summary>
    /// 
    
    public partial class admin_panel : Window
    {

        public admin_panel()
        {
            InitializeComponent();
            QueryRates();
        }

        // Accessor fields
        string ftlRate;
        string ltlRate;
        string carrierName;
        decimal ftlRateCarriers;
        decimal ltlRateCarriers;
        decimal reefCharge;
        string depotCity;
        decimal ftlAval;
        decimal ltlAval;

        public string FltRate
        {
            get { return ftlRate; }
            set { ftlRate = value; }
        }

        public string LtlRate
        {
            get { return ltlRate; }
            set { ltlRate = value; }
        }

        public string CarrierName
        {
            get { return carrierName; }
            set { carrierName = value; }
        }

        public decimal FtlRateCarriers
        {
            get { return ftlRateCarriers;  }
            set { ftlRateCarriers = value;  }
        }

        public decimal LtlRateCarriers
        {
            get { return ltlRateCarriers; }
            set { ltlRateCarriers = value; }
        }

        public decimal ReefCharge
        {
            get { return reefCharge; }
            set { reefCharge = value; }
        }

        public string DepotCity
        {
            get { return depotCity; }
            set { depotCity = value; }
        }

        public decimal FtlAval
        {
            get { return ftlAval; }
            set { ftlAval = value; }
        }

        public decimal LtlAval
        {
            get { return ltlAval; }
            set { ltlAval = value; }
        }

        // Database button
        private void Database_Btn(object sender, RoutedEventArgs e)
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

        // Rates button
        private void Rates_Btn(object sender, RoutedEventArgs e)
        {
            HideBtns();
            RatesGrid_Click.Visibility = Visibility.Visible;
        }


        // Carriers button
        private void Carriers_Btn(object sender, RoutedEventArgs e)
        {
            HideBtns();
            CarriersGrid_Click.Visibility = Visibility.Visible;
            Carriers_Datagrid();
        }


        // Routes button
        private void Routes_Btn(object sender, RoutedEventArgs e)
        {
            HideBtns();
            RoutesGrid_Click.Visibility = Visibility.Visible;
            RoutesDatagrid();
        }


        // Hide database btns
        private void HideBtns()
        {
            RatesBtn.Visibility = Visibility.Hidden;
            CarriersBtn.Visibility = Visibility.Hidden;
            RoutesBtn.Visibility = Visibility.Hidden;
        }

        // Show Rates
        private void QueryRates()
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            
            
            // Exception habdler to open connection to MySQL database
            try
            {
                connection.Open();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
            string ftlRateQuery = "SELECT amount FROM rates; ";
            MySqlCommand cmd = new MySqlCommand(ftlRateQuery, connection);
            MySqlDataReader rdr = cmd.ExecuteReader();

            int i = 0;
            while (rdr.Read())
            {
                if (i == 0)
                {
                    FltRate = rdr.GetString(0).ToString();
                }

                else
                {
                    LtlRate = rdr.GetString(0).ToString();
                }

                i++;
                
            }
            Flt_Textbox.Text = FltRate;
            Ltl_Textbox.Text = LtlRate;
        }


        private void FLT_Update(object sender, RoutedEventArgs e)
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string fltAmount = Flt_Textbox.Text;
            string updateQuery = $"UPDATE rates SET amount = {fltAmount} WHERE id = 1;";

            MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            MessageBox.Show("FLT value has been updated.");
        }


        private void LTL_Update(object sender, RoutedEventArgs e)
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string ltlAmount = Ltl_Textbox.Text;
            string updateQuery = $"UPDATE rates SET amount = {ltlAmount} WHERE id = 2;";

            MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            MessageBox.Show("LTL value has been updated.");
        }


        // Carriers Data
        private void Carriers_Data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            if (rowSelected == null)
            {
                return;
            }

            CarrierName = (string)rowSelected[0];
            FtlRateCarriers = (decimal)rowSelected[1];
            LtlRateCarriers = (decimal)rowSelected[2];
            ReefCharge = (decimal)rowSelected[3];
            CarriersCity_Datagrid();
            CarrierDetails();
        }


        // Carrier City Data
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


        private void Carriers_Datagrid()
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = @"SELECT DISTINCT CompanyName AS 'Name', ftlRate AS 'FTLRate', ltlRate AS 'LTLRate', reefCharge AS 'reefCharge' FROM carriers";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();

            Carriers_Data.DataContext = dt;
        }


        // Carriers City
        private void CarriersCity_Datagrid()
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = $"SELECT depotCity, ftlAvailable, ltlAvailable FROM carriers WHERE companyName = \"{CarrierName}\" ";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();

            Carriers_City_Data.DataContext = dt;
        }
        
        // Fill text box with carrier details
        private void CarrierDetails()
        {
            CarrierDetailName.Content = CarrierName.ToString();
            ReeferCharge_Textbox.Text = ReefCharge.ToString();
            FTL_Textbox.Text = FtlRateCarriers.ToString();
            LTL_Textbox.Text = LtlRateCarriers.ToString();
        }

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
            Carriers_Datagrid();
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
        private void RoutesDatagrid()
        {
            string connectionString = @"server=127.0.0.1;database=omnicorp;uid=root;pwd=;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = $"SELECT destination, distance, time, west, east FROM routes;";
            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();

            Routes_Data.DataContext = dt;
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
            RoutesDatagrid();

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
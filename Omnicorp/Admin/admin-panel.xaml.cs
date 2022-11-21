using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Omnicorp.Admin
{
    /// <summary>
    /// Interaction logic for admin_panel.xaml
    /// </summary>
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
            MessageBox.Show($"{DepotCity} - {FtlAval} - {LtlAval}");
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

    }
}

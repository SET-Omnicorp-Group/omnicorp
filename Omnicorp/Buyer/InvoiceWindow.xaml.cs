using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Omnicorp.Buyer
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        private BuyerHandler handler;
        private string orderId;

        public InvoiceWindow(string orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            handler = new BuyerHandler();
            DisplayInvoiceInfo(orderId);
        }

        private void DisplayInvoiceInfo(string orderId)
        {
            string query = $"SELECT o.customer, o.origin, o.destination, o.jobType, o.vanType, o.quantity, i.amount, i.id, r.distance, r.totalHours " +
                            $"FROM orders o " +
                            $"INNER JOIN invoices i ON i.orderId = o.id " +
                            $"INNER JOIN routes r ON r.orderId = o.id " +
                            $"WHERE o.id = '{orderId}';";

            MyQuery myQuery = new MyQuery();
            MySqlDataReader rdr = myQuery.DataReader(query);
            rdr.Read();

            string customer = rdr.GetString(0);
            string origin = rdr.GetString(1);
            string destination = rdr.GetString(2);
            string jobType = rdr.GetString(3);
            string vanType = rdr.GetString(4);
            string quantity = rdr.GetString(5);
            decimal amount = rdr.GetDecimal(6);
            string invoiceId = rdr.GetString(7);
            string distance = rdr.GetString(8);
            decimal totalHours = rdr.GetDecimal(9);

            InvoiceNumberInfo.Content = invoiceId;
            OrderIdInfo.Content = orderId;
            ClientNameInfo.Content = customer;
            RouteInfo.Content = $"{origin} -> {destination}";
            JobDescriptionInfo.Content = $"{jobType} | {vanType}";
            TotalDistanceInfo.Content = distance;
            TotalHoursInfo.Content = String.Format("{0:0.00}", totalHours);
            TotalInfo.Content = String.Format("{0:0.00}", amount) + " CAD";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            handler.SaveInvoiceFile(orderId);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using ClassLibrary;
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

namespace Omnicorp.Buyer
{
    /// <summary>
    /// Interaction logic for buyer_panel.xaml
    /// </summary>
    public partial class BuyerPanel : Window
    {

        // Accessor fields 
        BuyerHandler handler;

        public string Client_Name { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Quantity { get; set; }
        public string JobType { get; set; }
        public string VanType { get; set; }


        public BuyerPanel()
        {
            InitializeComponent();
            handler = new BuyerHandler();
            HideDataGrids(); 
        }


        // Hide grids
        private void HideDataGrids()
        {
            OrdersGrid.Visibility = Visibility.Hidden;
            MarketplaceGrid.Visibility = Visibility.Hidden;
            AcceptContractBtn.Visibility = Visibility.Hidden;
        }


        // Show Radio button for orders 
        private void ShowRadioButtons()
        {
            ActiveOrders.Visibility = Visibility.Visible;
            OnRouteOrders.Visibility = Visibility.Visible;
            DeliveredOrders.Visibility = Visibility.Visible;
            CompletedOrders.Visibility = Visibility.Visible;
            AllOrders.Visibility = Visibility.Visible;
        }


        // Hide Radio button for orders 
        private void HideRadioButtons()
        {
            ActiveOrders.Visibility = Visibility.Hidden;
            OnRouteOrders.Visibility = Visibility.Hidden;
            DeliveredOrders.Visibility = Visibility.Hidden;
            CompletedOrders.Visibility = Visibility.Hidden;
            AllOrders.Visibility = Visibility.Hidden;
        }


        // Display client contracts datagrid
        private void MarketplaceBtn_Click(object sender, RoutedEventArgs e)
        {

            ContractsBtn.Background = Brushes.White;
            ContractsBtn.Foreground = Brushes.Black;

            ClientOrdersBtn.Background = Brushes.Transparent;
            ClientOrdersBtn.Foreground = Brushes.White;



            HideDataGrids();
            HideRadioButtons();
            MarketplaceGrid.Visibility = Visibility.Visible;
            AcceptContractBtn.Visibility=Visibility.Visible;
            MarketplaceGrid.DataContext = handler.GetContractsFromMarketplaceDatabase();
        }

        

        // Get value of selected rows
        private void MarketplaceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            if (rowSelected == null)
            {
                Client_Name = String.Empty;
                Origin = String.Empty;
                Destination = String.Empty;
                Quantity = 0;
                JobType = "";
                VanType = "";
                return;
            }

            Client_Name = rowSelected[0].ToString();
            Origin = rowSelected[1].ToString();
            Destination = rowSelected[2].ToString();
            Quantity = Convert.ToInt32(rowSelected[3]);
            JobType = rowSelected[4].ToString();
            VanType = rowSelected[5].ToString();
        }

        
        
        // Accept contracts button
        private void AcceptContractsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                handler.InsertContractsToOrderDatabase(Client_Name, JobType, Quantity, VanType, Origin, Destination, "Active");
                MessageBox.Show("Client contract has been saved to orders");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Cannot insert selected contract to the orders databases");
            }
        }



        // Orders button
        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {

            ClientOrdersBtn.Background = Brushes.White;
            ClientOrdersBtn.Foreground = Brushes.Black;


            ContractsBtn.Background = Brushes.Transparent;
            ContractsBtn.Foreground = Brushes.White;

            


            HideDataGrids();
            AllOrders.IsChecked = true;
            ShowRadioButtons();
            OrdersGrid.Visibility = Visibility.Visible;
        }


        // All contracts radio button
        private void AllOrders_Checked(object sender, RoutedEventArgs e)
        {
           OrdersGrid.DataContext = handler.GetOrdersFromDatabase();
        }


        // Active contracts radio button
        private void ActiveOrders_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase("Active");
        }


        // OnRoute contracts radio button
        private void OnRouteOrders_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase("On Route");
        }


        // Delivered contracts radio button
        private void DeliveredOrders_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase("Delivered");
        }


        // Completed contracts radio button
        private void CompletedOrders_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase("Completed");
        }

        private void OrdersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            if (rowSelected == null)
            {
                return;
            }

            string status = rowSelected[9].ToString();
            string orderId = rowSelected[0].ToString();

            if (status == "Delivered")
            {
                MessageBoxResult res = MessageBox.Show(
                    "Would you like to complete the order and generate an Invoice?", 
                    "Complete order?",
                    MessageBoxButton.YesNo
                );
                if(res == MessageBoxResult.Yes)
                {
                    handler.GenerateInvoice(orderId);
                    OrdersGrid.DataContext = handler.GetOrdersFromDatabase("Delivered");
                    InvoiceWindow invoice = new InvoiceWindow(orderId);
                    invoice.ShowDialog();
                }
            }

            else if (status == "Completed")
            {
                InvoiceWindow invoice = new InvoiceWindow(orderId);
                invoice.ShowDialog();
            }

           
        }
    }
}

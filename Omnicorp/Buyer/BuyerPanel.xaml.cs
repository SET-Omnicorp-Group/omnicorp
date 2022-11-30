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
            ClientContractGrid.Visibility = Visibility.Hidden;
            AcceptBtn.Visibility = Visibility.Hidden;
        }


        // Show Radio button for orders 
        private void ShowRadioButtons()
        {
            ActiveContracts.Visibility = Visibility.Visible;
            ProcessingContracts.Visibility = Visibility.Visible;
            CompletedContracts.Visibility = Visibility.Visible;
            AllContracts.Visibility = Visibility.Visible;
        }


        // Hide Radio button for orders 
        private void HideRadioButtons()
        {
            ActiveContracts.Visibility = Visibility.Hidden;
            ProcessingContracts.Visibility = Visibility.Hidden;
            CompletedContracts.Visibility = Visibility.Hidden;
            AllContracts.Visibility = Visibility.Hidden;
        }


        // Display client contracts datagrid
        private void ClientContractsBtn(object sender, RoutedEventArgs e)
        {
            HideDataGrids();
            HideRadioButtons();
            ClientContractGrid.Visibility = Visibility.Visible;
            AcceptBtn.Visibility=Visibility.Visible;
            ClientContractGrid.DataContext = handler.GetContractsFromDatabase();
        }

        

        // Get value of selected rows
        private void ClientContractGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        private void AcceptContractsBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                handler.InsertContractsToOrderDatabase(Client_Name, JobType, Quantity, VanType,
                                                   Origin, Destination, "Processing");
                MessageBox.Show("Client contract has been saved to orders");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Cannot insert selected contract to the orders databases");
            }
        }



        // Orders button
        private void OrderBtn(object sender, RoutedEventArgs e)
        {
            HideDataGrids();
            AllContracts.IsChecked = true;
            ShowRadioButtons();
            OrdersGrid.Visibility = Visibility.Visible;
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase();
        }



        // Order datagrid
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        // All contracts radio button
        private void AllContractsChecked(object sender, RoutedEventArgs e)
        {
           OrdersGrid.DataContext = handler.GetOrdersFromDatabase();
        }


        // Active contracts radio button
        private void ActiveContracts_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetActiveOrdersFromDatabase();
        }


        // Processing contracts radio button
        private void ProcessingContracts_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetProcessingOrdersFromDatabase();
        }

        
        // Completed contracts radio button
        private void CompletedContracts_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetCompletedOrdersFromDatabase();
        }
    }
}

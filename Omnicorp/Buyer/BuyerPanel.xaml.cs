/*
* FILE          :   BuyerPanel.xaml.cs
* PROJECT       :   SENG2020 - Omnicorp  project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to design the Buyer panel window
*/
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

    /*
    * CLASS NAME	:   BuyerPanel
    * DESCRIPTION	:   The purpose of this class is to perform the buyer user functionality.
    *
    * DATA MEMBERS  :
    *                   - Dictionary    activeGameSessions, as the sessions active on the current server
    *                   - string        Client_Name, client name
    *                   - string        Origin, origin of the order
    *                   - string        Destination, order destination
    *                   - int           Quantity, quantity of the order
    *                   - string        JobType, as FTL or LTL
    *                   - string        VanType, as the type of the van refrigerator or dry
    *
    *
    */
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

        /*
        * METHOD		:  BuyerPanel
        * DESCRIPTION	:   try to initilize the buyer functionality
        * PARAMETERS    :
        *                   - None
        * RETURNS       :
        *                   - None
        */
        public BuyerPanel()
        {
            InitializeComponent();
            handler = new BuyerHandler();
            HideDataGrids();
        }


        // Hide grids
        /*
        * METHOD		:  HideDataGrids
        * DESCRIPTION	:   try to visible the various button functionality
        * PARAMETERS    : None
        *
        * RETURNS       :
        *                   - None
        */
        private void HideDataGrids()
        {
            OrdersGrid.Visibility = Visibility.Hidden;
            MarketplaceGrid.Visibility = Visibility.Hidden;
        }


        // Show Radio button for orders

       /*
       * METHOD		:  ShowRadioButtons
       * DESCRIPTION	:   try to visible the radio button functionality
       * PARAMETERS    : None
       *
       * RETURNS       :
       *                   - None
       */

        private void ShowRadioButtons()
        {
            ActiveOrders.Visibility = Visibility.Visible;
            OnRouteOrders.Visibility = Visibility.Visible;
            DeliveredOrders.Visibility = Visibility.Visible;
            CompletedOrders.Visibility = Visibility.Visible;
            AllOrders.Visibility = Visibility.Visible;
        }


        // Hide Radio button for orders
      /*
      * METHOD		:  HideRadioButtons
      * DESCRIPTION	:   try to hide the radio button functionality
      * PARAMETERS    : None
      *
      * RETURNS       :
      *                   - None
      */
        private void HideRadioButtons()
        {
            ActiveOrders.Visibility = Visibility.Hidden;
            OnRouteOrders.Visibility = Visibility.Hidden;
            DeliveredOrders.Visibility = Visibility.Hidden;
            CompletedOrders.Visibility = Visibility.Hidden;
            AllOrders.Visibility = Visibility.Hidden;
        }


        /*
        * METHOD        :  MarketplaceBtn_Click
        * DESCRIPTION	:   try to show client contract display
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void MarketplaceBtn_Click(object sender, RoutedEventArgs e)
        {

            MarketplaceBtn.Background = Brushes.White;
            MarketplaceBtn.Foreground = Brushes.Black;

            OrdersBtn.Background = Brushes.Transparent;
            OrdersBtn.Foreground = Brushes.White;



            HideDataGrids();
            HideRadioButtons();
            MarketplaceGrid.Visibility = Visibility.Visible;
            MarketplaceGrid.DataContext = handler.GetContractsFromMarketplaceDatabase();
        }




        /*
        * METHOD        :   OrderBtn_Click
        * DESCRIPTION	:   try to show the order
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {

            OrdersBtn.Background = Brushes.White;
            OrdersBtn.Foreground = Brushes.Black;


            MarketplaceBtn.Background = Brushes.Transparent;
            MarketplaceBtn.Foreground = Brushes.White;




            HideDataGrids();
            AllOrders.IsChecked = true;
            ShowRadioButtons();
            OrdersGrid.Visibility = Visibility.Visible;
        }


        // All contracts radio button
        /*
        * METHOD		:  AllOrders_Checked
        * DESCRIPTION	:   try to get the all contract data
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void AllOrders_Checked(object sender, RoutedEventArgs e)
        {
           OrdersGrid.DataContext = handler.GetOrdersFromDatabase();
        }


        // Active contracts radio button
        /*
        * METHOD        :  ActiveOrders_Checked
        * DESCRIPTION	:   try to get the active contract data
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void ActiveOrders_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase("Active");
        }


        // Processing contracts radio button

        /*
        * METHOD        :  OnRouteOrders_Checked
        * DESCRIPTION	:   try to get the processing contract data
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void OnRouteOrders_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase("On Route");
        }


        /*
        * METHOD        :  DeliveredOrders_Checked
        * DESCRIPTION	:   try to get the delivered contract data
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void DeliveredOrders_Checked(object sender, RoutedEventArgs e)
        {
            OrdersGrid.DataContext = handler.GetOrdersFromDatabase("Delivered");
        }


        // Completed contracts radio button
        /*
        * METHOD        :  CompletedOrders_Checked
        * DESCRIPTION	:   try to get the completed contract data
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
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





        /*
       * METHOD         :  MarketplaceGrid_MouseDoubleClick
       * DESCRIPTION    :  try to accept the contract
       * PARAMETERS     :
       *                   - object   sender, as the sender of the object
       *                   - RoutedEventArgs    e, events
       * RETURNS        :
       *                   - None
       */
        private void MarketplaceGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

            string customer = rowSelected[0].ToString();
            string origin = rowSelected[1].ToString();
            string destination = rowSelected[2].ToString();
            int quantity = Convert.ToInt32(rowSelected[3]);
            string jobType = rowSelected[4].ToString();
            string vanType = rowSelected[5].ToString();

            try
            {
                handler.InsertContractsToOrderDatabase(customer, jobType, quantity, vanType, origin, destination, "Active");
                MessageBox.Show("Client contract has been saved to orders");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Cannot insert selected contract to the orders databases");
            }
        }
    }
}

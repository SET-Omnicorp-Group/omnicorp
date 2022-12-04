/*
* FILE          :   PlannerHandler.xaml.cs
* PROJECT       :   SENG2020 - Omnicorp  project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to design the plannerHandler window
*/
using Omnicorp.Buyer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace Omnicorp.Planner
{
    /// <summary>
    /// Interaction logic for PlannerPanel.xaml
    /// </summary>

    /*
    * CLASS NAME	:   PlannerPanel
    * DESCRIPTION	:   The purpose of this class is to perform the planner user functionality.
    * 
    * DATA MEMBERS  :   
    *                   
    *                   - string        SelectedOrderId, order Id
    *                   
    */
    public partial class PlannerPanel : Window
    {
        PlannerHandler handler;

        public string SelectedOrderId { get; set; }


        public PlannerPanel()
        {
            InitializeComponent();
            handler = new PlannerHandler();
            HideDataGrids();

        }


        // Left hand side planner button
        /*
        * METHOD		:  PlannerOrdersBtn_Click
        * DESCRIPTION	:   try to visible the planner left hand side button functionality
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void PlannerOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            PlannerOrdersBtn.Background = Brushes.White;
            PlannerOrdersBtn.Foreground = Brushes.Black;
            PlannerInvoicesBtn.Background = Brushes.Transparent;
            PlannerInvoicesBtn.Foreground = Brushes.White;


            PlannerOrdersBtn.Visibility = Visibility.Visible;

            PlannerInvoicesGrid.Visibility = Visibility.Hidden;
            PlannerOrdersGrid.Visibility = Visibility.Visible;
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Active");

            AllInvoicesRadio.Visibility = Visibility.Hidden;
            LastTwoWeeksRadio.Visibility = Visibility.Hidden;

            ActiveContractsRadio.IsChecked = true;
            ActiveContractsRadio.Visibility = Visibility.Visible;
            OnRouteContractsRadio.Visibility = Visibility.Visible;
            DeliveredContractsRadio.Visibility = Visibility.Visible;
            CompletedContractsRadio.Visibility = Visibility.Visible;
        }


        // Planner order grid

        /*
        * METHOD		:  PlannerOrdersGrid_SelectionChanged
        * DESCRIPTION	:   try to visible the planner order grid if order is active functionality
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        
        private void PlannerOrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;
            string orderStatus = string.Empty;

            if (rowSelected != null)
            {
                orderStatus = rowSelected[7].ToString();
            }

            if (orderStatus == "Active")
            {
                SelectedOrderId = rowSelected[0].ToString();
            }
            else
            {
                SelectedOrderId = string.Empty;
            }


        }
        */

        private void PlannerOrdersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;
            string orderStatus = string.Empty;
            string orderId = string.Empty;

            if (rowSelected != null)
            {
                orderStatus = rowSelected[7].ToString();
                orderId = rowSelected[0].ToString();
            }

            if (orderStatus == "Active")
            {
                CarrierSelection cs = new CarrierSelection(orderId);
                cs.ShowDialog();

                PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Active");
            }
            
        }


        // Planner invoice grid
        /*
        * METHOD		:  PlannerInvoicesBtn_Click
        * DESCRIPTION	:   try to show the invoice of the order
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void PlannerInvoicesBtn_Click(object sender, RoutedEventArgs e)
        {
            PlannerInvoicesBtn.Background = Brushes.White;
            PlannerInvoicesBtn.Foreground = Brushes.Black;
            PlannerOrdersBtn.Background = Brushes.Transparent;
            PlannerOrdersBtn.Foreground = Brushes.White;




            PlannerOrdersGrid.Visibility = Visibility.Hidden;
            PlannerInvoicesGrid.Visibility = Visibility.Visible;
            AllInvoicesRadio.Visibility = Visibility.Visible;
            AllInvoicesRadio.IsChecked = true;
            LastTwoWeeksRadio.Visibility = Visibility.Visible;


            ActiveContractsRadio.Visibility = Visibility.Hidden;
            OnRouteContractsRadio.Visibility = Visibility.Hidden;
            DeliveredContractsRadio.Visibility = Visibility.Hidden;
            CompletedContractsRadio.Visibility = Visibility.Hidden;
        }


        

        // Active contracts radio button

        /*
        * METHOD		:  ActiveContracts_Checked
        * DESCRIPTION	:   try to check the active order radio button
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void ActiveContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Active");
            SimulateDayBtn.Visibility = Visibility.Hidden;
        }


        // Processing contracts radio button

        /*
        * METHOD		:  OnRouteContracts_Checked
        * DESCRIPTION	:   try to check the OnRoute order radio button
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void OnRouteContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("On Route");
            SimulateDayBtn.Visibility = Visibility.Visible;
        }


        // Processing contracts radio button

        /*
        * METHOD		:  DeliveredContracts_Checked
        * DESCRIPTION	:   try to check the delivered order radio button
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void DeliveredContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Delivered");
            SimulateDayBtn.Visibility = Visibility.Hidden;
        }


        // Completed contracts radio button
        /*
        * METHOD		:  CompletedContracts_Checked
        * DESCRIPTION	:   try to check the completed order radio button
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void CompletedContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Completed");
            SimulateDayBtn.Visibility = Visibility.Hidden;
        }

        // Hide grids
        /*
        * METHOD		:  CompletedContracts_Checked
        * DESCRIPTION	:   try to hide the order data grid
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void HideDataGrids()
        {
            PlannerOrdersGrid.Visibility = Visibility.Hidden;
            PlannerInvoicesGrid.Visibility = Visibility.Hidden;
        }




        /*
        * METHOD		:  SimulateDayBtn_Click
        * DESCRIPTION	:   try to show the On route order data and perform simulate day functionlity
        * PARAMETERS    :
        *                   - object   sender, as the sender of the object
        *                   - RoutedEventArgs    e, events
        * RETURNS       :
        *                   - None
        */
        private void SimulateDayBtn_Click(object sender, RoutedEventArgs e)
        {
            handler.SimulateDay();
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("On Route");
        }

        private void AllInvoicesRadio_Checked(object sender, RoutedEventArgs e)
        {
            PlannerInvoicesGrid.DataContext = handler.GetInvoicesFromDatabase();
        }

        private void LastTwoWeekRadio_Checked(object sender, RoutedEventArgs e)
        {
            PlannerInvoicesGrid.DataContext = handler.GetLastWeeksInvoicesFromDatabase();
        }

        
    }
}

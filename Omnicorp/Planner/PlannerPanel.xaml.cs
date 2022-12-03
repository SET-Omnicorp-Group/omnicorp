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


            ActiveContractsRadio.IsChecked = true;
            ActiveContractsRadio.Visibility = Visibility.Visible;
            OnRouteContractsRadio.Visibility = Visibility.Visible;
            DeliveredContractsRadio.Visibility = Visibility.Visible;
            CompletedContractsRadio.Visibility = Visibility.Visible;
            AvailableCarriersBtn.Visibility = Visibility.Hidden;
        }


        // Planner order grid
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
                AvailableCarriersBtn.Visibility = Visibility.Visible;
            }
            else
            {
                SelectedOrderId = string.Empty;
                AvailableCarriersBtn.Visibility = Visibility.Hidden;
            }


        }

        // Planner invoice grid
        private void PlannerInvoicesBtn_Click(object sender, RoutedEventArgs e)
        {
            PlannerInvoicesBtn.Background = Brushes.White;
            PlannerInvoicesBtn.Foreground = Brushes.Black;
            PlannerOrdersBtn.Background = Brushes.Transparent;
            PlannerOrdersBtn.Foreground = Brushes.White;




            PlannerOrdersGrid.Visibility = Visibility.Hidden;
            PlannerInvoicesGrid.Visibility = Visibility.Visible;

            ActiveContractsRadio.Visibility = Visibility.Hidden;
            OnRouteContractsRadio.Visibility = Visibility.Hidden;
            DeliveredContractsRadio.Visibility = Visibility.Hidden;
            CompletedContractsRadio.Visibility = Visibility.Hidden;
            AvailableCarriersBtn.Visibility = Visibility.Hidden;
        }


        // Planner invoice grid
        private void PlannerInvoicesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Active contracts radio button
        private void ActiveContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Active");
            SimulateDayBtn.Visibility = Visibility.Hidden;
        }


        // Processing contracts radio button
        private void OnRouteContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("On Route");
            SimulateDayBtn.Visibility = Visibility.Visible;
        }


        // Processing contracts radio button
        private void DeliveredContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Delivered");
            SimulateDayBtn.Visibility = Visibility.Hidden;
        }


        // Completed contracts radio button
        private void CompletedContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Completed");
            SimulateDayBtn.Visibility = Visibility.Hidden;
        }

        // Hide grids
        private void HideDataGrids()
        {
            PlannerOrdersGrid.Visibility = Visibility.Hidden;
            PlannerInvoicesGrid.Visibility = Visibility.Hidden;
        }

        private void AvailableCarriersBtn_Click(object sender, RoutedEventArgs e)
        {
            CarrierSelection cs = new CarrierSelection(SelectedOrderId);
            cs.ShowDialog();

            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Active");
        }

        private void SimulateDayBtn_Click(object sender, RoutedEventArgs e)
        {
            handler.SimulateDay();
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("On Route");
        }
    }
}

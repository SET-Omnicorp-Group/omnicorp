using Omnicorp.Buyer;
using System;
using System.Collections.Generic;
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

        public PlannerPanel()
        {
            InitializeComponent();
            handler = new PlannerHandler();
            HideDataGrids();

        }


        // Left hand side planner button
        private void PlannerOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            PlannerInvoicesGrid.Visibility = Visibility.Hidden;
            PlannerOrdersGrid.Visibility = Visibility.Visible;
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Active");

            ActiveContracts.Visibility = Visibility.Visible;
            OnRouteContracts.Visibility = Visibility.Visible;
            CompletedContracts.Visibility = Visibility.Visible;
        }


        // Planner order grid
        private void PlannerOrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Planner invoice grid
        private void PlannerInvoicesBtn_Click(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.Visibility = Visibility.Hidden;
            PlannerInvoicesGrid.Visibility = Visibility.Visible;

            ActiveContracts.Visibility = Visibility.Hidden;
            OnRouteContracts.Visibility = Visibility.Hidden;
            CompletedContracts.Visibility = Visibility.Hidden;
        }


        // Planner invoice grid
        private void PlannerInvoicesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Active contracts radio button
        private void ActiveContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Active");
        }


        // Processing contracts radio button
        private void OnRouteContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("On Route");
        }


        // Completed contracts radio button
        private void CompletedContracts_Checked(object sender, RoutedEventArgs e)
        {
            PlannerOrdersGrid.DataContext = handler.GetOrdersFromDatabaseWhere("Completed");
        }

        // Hide grids
        private void HideDataGrids()
        {
            PlannerOrdersGrid.Visibility = Visibility.Hidden;
            PlannerInvoicesGrid.Visibility = Visibility.Hidden;
        }
    }
}

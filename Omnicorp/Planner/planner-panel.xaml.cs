using System;
using System.Collections.Generic;
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

namespace Omnicorp.Planner
{
    /// <summary>
    /// Interaction logic for planner_panel.xaml
    /// </summary>
    public partial class planner_panel : Window
    {
        public planner_panel()
        {
            InitializeComponent();
        }


        // Left hand side planner button
        private void PlannerOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            PlannersOrdersGrid.DataContext = PlannersOrdersGrid.DataContext;
        }


        // Planner order grid
        private void PlannerOrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Planner invoice grid
        private void PlannerInvoicesBtn_Click(object sender, RoutedEventArgs e)
        {

        }


        // Planner invoice grid
        private void PlannerInvoiceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

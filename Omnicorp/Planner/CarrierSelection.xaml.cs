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

namespace Omnicorp.Planner
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CarrierSelection : Window
    {
        private string orderId;
        private string carrierId;
        PlannerHandler handler;
        public CarrierSelection(string orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            this.handler = new PlannerHandler();

            AvailableCarrierGrid.DataContext = handler.GetAvailableCarriersForOrder(orderId);
        }

        private void AvailableCarrierGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;

            if (rowSelected != null)
            {
                carrierId = rowSelected[0].ToString();
                SelecCarrierBtn.IsEnabled = true;
            }
            else
            {
                SelecCarrierBtn.IsEnabled = false;
            }
        }


        private void SelecCarrierBtn_Click(object sender, RoutedEventArgs e)
        {
            handler.CreateNewRoute(orderId, carrierId);
            this.Close();
        }

        
    }
}

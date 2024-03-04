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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerQueuingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CQSWindow : Window
    {
        public CQSWindow()
        {
            InitializeComponent();
        }

        private void ExpressButton_Click(object sender, RoutedEventArgs e)
        {
            NumOfItemsScreen.Visibility = Visibility.Collapsed;
            POSTypeScreen.Visibility = Visibility.Visible;
        }

        private void NonExpressButton_Click(object sender, RoutedEventArgs e)
        {
            NumOfItemsScreen.Visibility = Visibility.Collapsed;
            POSTypeScreen.Visibility = Visibility.Visible;
        }

        private void SelfCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            POSTypeScreen.Visibility = Visibility.Collapsed;
            PaymentTypeScreen.Visibility = Visibility.Visible;
        }

        private void RegularCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            POSTypeScreen.Visibility = Visibility.Collapsed;
            PaymentTypeScreen.Visibility = Visibility.Visible;
        }

        private void CashButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentTypeScreen.Visibility = Visibility.Collapsed;
            ThankYouScreen.Visibility = Visibility.Visible;
        }

        private void CardCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentTypeScreen.Visibility = Visibility.Collapsed;
            ThankYouScreen.Visibility = Visibility.Visible;
        }
    }
}

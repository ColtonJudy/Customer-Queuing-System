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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerQueuingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CQSWindow : Window
    {
        PaymentType paymentChoice;
        CheckoutType checkoutChoice;
        bool isExpress;
        Store store;
        Simulation? simulation;

        public CQSWindow()
        {
            InitializeComponent();
            LoadFromConfig();

            store = new Store(Config.GetPOSsFromJSON());


            if (Config.GetRunSimulation())
            {
                simulation = new Simulation(store);
                simulation.Start();
            }
        }

        private void LoadFromConfig()
        {
            string[] storeInfo = Config.GetStoreInfo();
            this.Title = storeInfo[0];
            WelcomeText.Text = storeInfo[1];
        }

        private void ExpressButton_Click(object sender, RoutedEventArgs e)
        {
            isExpress = true;


            NumOfItemsScreen.Visibility = Visibility.Collapsed;
            POSTypeScreen.Visibility = Visibility.Visible;
        }

        private void NonExpressButton_Click(object sender, RoutedEventArgs e)
        {
            isExpress = false;


            NumOfItemsScreen.Visibility = Visibility.Collapsed;
            POSTypeScreen.Visibility = Visibility.Visible;
        }

        private void SelfCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            checkoutChoice = CheckoutType.SCO;

            POSTypeScreen.Visibility = Visibility.Collapsed;
            PaymentTypeScreen.Visibility = Visibility.Visible;
        }

        private void RegularCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if(isExpress)
            {
                checkoutChoice = CheckoutType.Express;
            }
            else
            {
                checkoutChoice = CheckoutType.Cashier;
            }

            POSTypeScreen.Visibility = Visibility.Collapsed;
            PaymentTypeScreen.Visibility = Visibility.Visible;
        }

        private void CashButton_Click(object sender, RoutedEventArgs e)
        {
            paymentChoice = PaymentType.Cash;

            MakeCustomer();

            PaymentTypeScreen.Visibility = Visibility.Collapsed;
            ThankYouScreen.Visibility = Visibility.Visible;
        }

        private void CardCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            paymentChoice = PaymentType.Card;

            MakeCustomer();

            PaymentTypeScreen.Visibility = Visibility.Collapsed;
            ThankYouScreen.Visibility = Visibility.Visible;
        }

        private async void MakeCustomer()
        {
            Customer customer = new Customer(paymentChoice, checkoutChoice, isExpress);

            string recommendation = CQS.AddCustomerToStore(customer, store);
            
            if(simulation != null)
            {
                simulation.Update(store);
            }

            RecommendedRegisterText.Text = recommendation;

            await Task.Delay(5000);

            ThankYouScreen.Visibility = Visibility.Collapsed;
            NumOfItemsScreen.Visibility = Visibility.Visible;
        }
    }
}

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

        PaymentType paymentChoice;
        CheckoutType checkoutChoice;
        bool isExpress;
        Store store;

        public CQSWindow()
        {
            InitializeComponent();
            CreateCQS();
        }

        private void CreateCQS()
        {
            POS SCO_POS = new POS(100, true, true, CheckoutType.SCO, false, CheckoutState.Open, 1);
            POS SCO_POS2 = new POS(101, true, true, CheckoutType.SCO, false, CheckoutState.Open, 1);
            POS cashier_POS = new POS(200, true, true, CheckoutType.Cashier, false, CheckoutState.Open, 3);
            POS cashier_POS2 = new POS(201, true, true, CheckoutType.Cashier, false, CheckoutState.Open, 3);
            POS cashier_POS3 = new POS(202, false, true, CheckoutType.Cashier, false, CheckoutState.Open, 3);
            POS express_POS = new POS(300, true, true, CheckoutType.Express, true, CheckoutState.Open, 3);
            POS express_POS2 = new POS(301, true, true, CheckoutType.Express, true, CheckoutState.Open, 3);

            List<POS> POS_List = new List<POS>();
            POS_List.Add(SCO_POS);
            POS_List.Add(SCO_POS2);
            POS_List.Add(cashier_POS);
            POS_List.Add(cashier_POS2);
            POS_List.Add(cashier_POS3);
            POS_List.Add(express_POS);
            POS_List.Add(express_POS2);

            store = new Store(POS_List);
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
            RecommendedRegisterText.Text = recommendation;

            await Task.Delay(5000);

            ThankYouScreen.Visibility = Visibility.Collapsed;
            NumOfItemsScreen.Visibility = Visibility.Visible;
        }
    }
}

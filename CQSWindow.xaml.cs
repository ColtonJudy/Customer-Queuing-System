using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for CQSWindow.xaml
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
                SimMenu.Visibility = Visibility.Visible;
                PopulateSimMenu();
            }
        }

        //loads the store title and welcome text from config
        private void LoadFromConfig()
        {
            string[] storeInfo = Config.GetStoreInfo();
            this.Title = storeInfo[0];
            WelcomeText.Text = storeInfo[1];
        }

        //populates the top menu for the simulation mode
        private void PopulateSimMenu()
        {
            PopulateRemoveCustomerMenuItem(store.SCO_POSList);
            PopulateRemoveCustomerMenuItem(store.CashierPOSList);
            PopulateCheckoutStateMenuItem(store.SCO_POSList);
            PopulateCheckoutStateMenuItem(store.CashierPOSList);
        }

        //populates the remove customer menu
        private void PopulateRemoveCustomerMenuItem(List<POS> posList)
        {
            //populating remove customer menu item
            for (int i = 0; i < posList.Count(); i++)
            {
                int index = i;
                MenuItem menuItem = new MenuItem();
                menuItem.Header = posList[index].ToString();
                menuItem.Click += new RoutedEventHandler(
                    (sendItem, args) =>
                    {
                        posList[index].DeleteCustomer();
                        simulation.Update(store);
                    }
                    );

                RemoveCustomerMenuItem.Items.Add(menuItem);
            }
        }

        //populates the checkout state menu
        private void PopulateCheckoutStateMenuItem(List<POS> posList)
        {
            //populate checkout state menu item
            for (int i = 0; i < posList.Count(); i++)
            {
                int index = i;

                MenuItem menuItem = new MenuItem();

                if (posList[index].CheckoutState == CheckoutState.Open)
                {
                    menuItem.Header = "Close " + posList[index].ToString();
                }
                else if (posList[index].CheckoutState == CheckoutState.Closed)
                {
                    menuItem.Header = "Open " + posList[index].ToString();
                }
                else
                {
                    menuItem.Header = "Undelay " + posList[index].ToString();
                }

                menuItem.Click += new RoutedEventHandler(
                    (sendItem, args) =>
                    {
                        if (posList[index].CheckoutState == CheckoutState.Open)
                        {
                            if (posList[index].CustomerCount() == 0)
                            {
                                posList[index].CheckoutState = CheckoutState.Closed;
                                menuItem.Header = "Open " + posList[index].ToString();
                            }
                            else
                            {
                                MessageBox.Show($"Error: Cannot close a POS with customers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            posList[index].CheckoutState = CheckoutState.Open;
                            menuItem.Header = "Close " + posList[index].ToString();
                        }

                        simulation.Update(store);
                    }
                    );

                CheckoutStateMenuItem.Items.Add(menuItem);
            }
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            CQS.PrintResultsToFile(store);
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

            AddCustomer();

            PaymentTypeScreen.Visibility = Visibility.Collapsed;
            ThankYouScreen.Visibility = Visibility.Visible;
        }

        private void CardCheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            paymentChoice = PaymentType.Card;

            AddCustomer();

            PaymentTypeScreen.Visibility = Visibility.Collapsed;
            ThankYouScreen.Visibility = Visibility.Visible;
        }

        //once the customer has finished checking out, make the customer object and add them to the store, checking for any recommendations.
        private async void AddCustomer()
        {
            Customer customer = new Customer(paymentChoice, checkoutChoice, isExpress);

            string recommendation = CQS.AddCustomerToStore(customer, store);
            
            if(simulation != null)
            {
                simulation.Update(store);
            }

            RecommendedRegisterText.Text = recommendation;

            await Task.Delay(2000);

            ThankYouScreen.Visibility = Visibility.Collapsed;
            NumOfItemsScreen.Visibility = Visibility.Visible;
        }
    }
}

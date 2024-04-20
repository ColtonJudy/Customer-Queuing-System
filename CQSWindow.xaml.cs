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
                SimMenu.Visibility = Visibility.Visible;
                PopulateSimMenu();
            }
        }

        private void LoadFromConfig()
        {
            string[] storeInfo = Config.GetStoreInfo();
            this.Title = storeInfo[0];
            WelcomeText.Text = storeInfo[1];
        }

        private void PopulateSimMenu()
        {
            //populating remove customer menu item
            for(int i = 0; i < store.SCO_POSList.Count(); i++)
            {
                int index = i;
                MenuItem menuItem = new MenuItem();
                menuItem.Header = store.SCO_POSList[index].ToString();
                menuItem.Click += new RoutedEventHandler(
                    (sendItem, args) =>
                        {
                            store.SCO_POSList[index].DeleteCustomer();
                            simulation.Update(store);
                        }
                    );

                RemoveCustomerMenuItem.Items.Add(menuItem);
            }
            for (int i = 0; i < store.CashierPOSList.Count(); i++)
            {
                int index = i;
                MenuItem menuItem = new MenuItem();
                menuItem.Header = store.CashierPOSList[index].ToString();
                menuItem.Click += new RoutedEventHandler(
                    (sendItem, args) =>
                        {
                            store.CashierPOSList[index].DeleteCustomer();
                            simulation.Update(store);
                        }
                    );

                RemoveCustomerMenuItem.Items.Add(menuItem);
            }

            //populate checkout state menu item
            for (int i = 0; i < store.SCO_POSList.Count(); i++)
            {
                int index = i;

                MenuItem menuItem = new MenuItem();

                if(store.SCO_POSList[index].CheckoutState == CheckoutState.Open)
                {
                    menuItem.Header = "Close " + store.SCO_POSList[index].ToString();
                }
                else if(store.SCO_POSList[index].CheckoutState == CheckoutState.Closed)
                {
                    menuItem.Header = "Open " + store.SCO_POSList[index].ToString();
                }
                else
                {
                    menuItem.Header = "Undelay " + store.SCO_POSList[index].ToString();
                }

                menuItem.Click += new RoutedEventHandler(
                    (sendItem, args) =>
                        {
                            if (store.SCO_POSList[index].CheckoutState == CheckoutState.Open)
                            {
                                if (store.SCO_POSList[index].CustomerCount() == 0)
                                {
                                    store.SCO_POSList[index].CheckoutState = CheckoutState.Closed;
                                    menuItem.Header = "Open " + store.SCO_POSList[index].ToString();
                                }
                                else
                                {
                                    MessageBox.Show($"Error: Cannot close a POS with customers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                store.SCO_POSList[index].CheckoutState = CheckoutState.Open;
                                menuItem.Header = "Close " + store.SCO_POSList[index].ToString();
                            }

                            simulation.Update(store);
                        }
                    );

                CheckoutStateMenuItem.Items.Add(menuItem);
            }
            for (int i = 0; i < store.CashierPOSList.Count(); i++)
            {
                int index = i;

                MenuItem menuItem = new MenuItem();

                if (store.CashierPOSList[index].CheckoutState == CheckoutState.Open)
                {
                    menuItem.Header = "Close " + store.CashierPOSList[index].ToString();
                }
                else if (store.CashierPOSList[index].CheckoutState == CheckoutState.Closed)
                {
                    menuItem.Header = "Open " + store.CashierPOSList[index].ToString();
                }
                else
                {
                    menuItem.Header = "Undelay " + store.CashierPOSList[index].ToString();
                }

                menuItem.Click += new RoutedEventHandler(
                    (sendItem, args) =>
                    {
                        if (store.CashierPOSList[index].CheckoutState == CheckoutState.Open)
                        {
                            if (store.CashierPOSList[index].CustomerCount() == 0)
                            {
                                store.CashierPOSList[index].CheckoutState = CheckoutState.Closed;
                                menuItem.Header = "Open " + store.CashierPOSList[index].ToString();
                            }
                            else
                            {
                                MessageBox.Show($"Error: Cannot close a POS with customers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            store.CashierPOSList[index].CheckoutState = CheckoutState.Open;
                            menuItem.Header = "Close " + store.CashierPOSList[index].ToString();
                        }

                        simulation.Update(store);
                    }
                    );

                CheckoutStateMenuItem.Items.Add(menuItem);
            }
        }

        private void RemoveCustomersMenuSubItem_Click(object sender, RoutedEventArgs e)
        {

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

            await Task.Delay(2000);

            ThankYouScreen.Visibility = Visibility.Collapsed;
            NumOfItemsScreen.Visibility = Visibility.Visible;
        }
    }
}

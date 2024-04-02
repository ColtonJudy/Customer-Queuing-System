using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomerQueuingSystem
{
    /// <summary>
    /// Interaction logic for StoreConfigurationWindow.xaml
    /// </summary>
    public partial class StoreConfigurationWindow : Window
    {
        //stores the POSs while the system is being configured
        List<POS> tempPOSList;

        private static readonly Regex textRegex = new Regex(@"^\d{0,4}$");

        public StoreConfigurationWindow()
        {
            InitializeComponent();
            tempPOSList = Config.GetPOSsFromJSON();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //set the checkout type box to the checkout type enum in POS.cs
            CheckoutTypeComboBox.ItemsSource = Enum.GetValues(typeof(CheckoutType)).Cast<CheckoutType>();
            CheckoutTypeComboBox.SelectedIndex = 1;
            UpdateListView();
        }

        private void UpdateListView()
        {
            CurrentRegistersListView.ItemsSource = tempPOSList;
        }

        //ensures that the text boxes only contain numeric values up to 4 digits
        private void PreviewInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            e.Handled = !textRegex.IsMatch(textBox.Text + e.Text);
        }

        private void ResetFields()
        {
            POSNumberTextBox.Clear();
            AcceptsCashCheckBox.IsChecked = true;
            AcceptsCardCheckBox.IsChecked = true;
            CheckoutTypeComboBox.SelectedIndex = 1;
            CustomerMaxTextBox.Clear();
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Visible;
            ResetFields();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Visible;
            DeleteRegisterButton.Visibility = Visibility.Visible;
            ResetFields();
        }

        private void SaveAndStartButton_Click(object sender, RoutedEventArgs e)
        {
            Config.SetPOSsInJSON(tempPOSList);

            //update json w/ changes
        }

        private void SaveRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if(POSNumberTextBox.Text == "" || CustomerMaxTextBox.Text == "")
            {
                ErrorLabel.Content = "All fields must have a value";
                return;
            }

            //create a POS from the entered values
            int posNumber = int.Parse(POSNumberTextBox.Text);
            bool acceptsCash = AcceptsCashCheckBox.IsChecked ?? true;
            bool acceptsCard = AcceptsCardCheckBox.IsChecked ?? true;
            CheckoutType checkoutType = (CheckoutType)CheckoutTypeComboBox.SelectedIndex;
            bool isExpress = CheckoutTypeComboBox.SelectedIndex == 2;
            int customerMax = int.Parse(CustomerMaxTextBox.Text);

            POS pos = new POS(posNumber, acceptsCash, acceptsCard, checkoutType, isExpress, CheckoutState.Open, customerMax);

            //if the POS already exists in the system, we update it. Otherwise, add a new POS
            int index = tempPOSList.FindIndex(p => p.POSNumber == pos.POSNumber);
            if (index >= 0)
            {
                tempPOSList[index] = pos;
            }
            else
            {
                tempPOSList.Add(pos);
            }

            CurrentRegistersListView.Items.Refresh();

            EditPanel.Visibility = Visibility.Collapsed;
            DeleteRegisterButton.Visibility = Visibility.Collapsed;

            ResetFields();
        }

        private void DeleteRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Collapsed;
            DeleteRegisterButton.Visibility = Visibility.Collapsed;
            ResetFields();
        }
    }
}

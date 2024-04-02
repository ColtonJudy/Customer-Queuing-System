using System;
using System.Collections.Generic;
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

            CurrentRegistersListView.ItemsSource = tempPOSList;
            
            //set the listview with POSs
        }

        //ensures that the text boxes only contain numeric values up to 4 digits
        private void PreviewInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            e.Handled = !textRegex.IsMatch(textBox.Text + e.Text);
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAndStartButton_Click(object sender, RoutedEventArgs e)
        {
            //update json w/ changes
        }
    }
}

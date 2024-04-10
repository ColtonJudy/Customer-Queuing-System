using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            ConfigureListView();
        }

        private void UploadLogoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files | *.jpg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                LogoPathTextbox.Text = openFileDialog.FileName;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (StoreNameTextbox.Text != "" && WelcomeTextTextbox.Text != "")
            {
                ConfigWindowPage1.Visibility = Visibility.Collapsed;
                ConfigWindowPage2.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorLabel1.Visibility = Visibility.Visible;
            }

        }

        private void ConfigureListView()
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
            DeleteRegisterButton.Visibility = Visibility.Collapsed;
            EditPanel.Visibility = Visibility.Visible;
            ResetFields();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedPOSIndex = CurrentRegistersListView.SelectedIndex;
            POS selectedPOS = tempPOSList[selectedPOSIndex];

            POSNumberTextBox.Text = selectedPOS.POSNumber.ToString();
            AcceptsCashCheckBox.IsChecked = selectedPOS.AcceptsCash;
            AcceptsCardCheckBox.IsChecked = selectedPOS.AcceptsCard;
            CheckoutTypeComboBox.SelectedIndex = (int) selectedPOS.CheckoutType;
            CustomerMaxTextBox.Text = selectedPOS.MaxCustomerCount.ToString();

            EditPanel.Visibility = Visibility.Visible;
            DeleteRegisterButton.Visibility = Visibility.Visible;
        }

        private void SaveAndStartButton_Click(object sender, RoutedEventArgs e)
        {

            //copy logo image into resource directory (This copies it into the .\bin\debug folder used at runtime)
            //TODO: figure out how to put image directly into the project "Images" directory
            if(LogoPathTextbox.Text != "")
            {
                try
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string imagesFolderPath = System.IO.Path.Combine(baseDirectory, "Images\\logo.png");

                    File.Copy(LogoPathTextbox.Text, imagesFolderPath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to upload logo: {ex.Message}", "Upload Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //copy the store name and welcome text to json
            Config.SetStoreInfo(StoreNameTextbox.Text, WelcomeTextTextbox.Text);

            //update json w/ changes
            Config.SetPOSsInJSON(tempPOSList);

            CQSWindow CQSWindow = new CQSWindow();
            CQSWindow.Show();
            Close();
        }

        private void SaveRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if(POSNumberTextBox.Text == "" || CustomerMaxTextBox.Text == "")
            {
                ErrorLabel2.Content = "All fields must have a value";
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
                tempPOSList.Sort((x, y) => x.POSNumber.CompareTo(y.POSNumber)); //sort the list so that the POS numbers are always in ascending order
            }

            CurrentRegistersListView.Items.Refresh();

            EditPanel.Visibility = Visibility.Collapsed;
            DeleteRegisterButton.Visibility = Visibility.Collapsed;

            ResetFields();
        }

        private void DeleteRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedPOSIndex = CurrentRegistersListView.SelectedIndex;
            POS selectedPOS = tempPOSList[selectedPOSIndex];
            tempPOSList.Remove(selectedPOS);

            CurrentRegistersListView.Items.Refresh();

            EditPanel.Visibility = Visibility.Collapsed;
            DeleteRegisterButton.Visibility = Visibility.Collapsed;
            ResetFields();
        }
    }
}

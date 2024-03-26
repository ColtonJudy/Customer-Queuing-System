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

namespace CustomerQueuingSystem
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void EditConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if(DontShowCheckbox.IsChecked.GetValueOrDefault())
            {
                Config.SetShowStartScreen(false);
            }

            StoreConfigurationWindow storeConfigurationWindow = new StoreConfigurationWindow();
            storeConfigurationWindow.Show();
            Close();
        }

        private void StartCQSButton_Click(object sender, RoutedEventArgs e)
        {
            if (DontShowCheckbox.IsChecked.GetValueOrDefault())
            {
                Config.SetShowStartScreen(false);
            }

            CQSWindow CQSWindow = new CQSWindow();
            CQSWindow.Show();
            Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerQueuingSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Config.GetShowStartScreen())
                StartupUri = new Uri("/CustomerQueuingSystem;component/StartWindow.xaml", UriKind.Relative);
            else
                StartupUri = new Uri("/CustomerQueuingSystem;component/CQSWindow.xaml", UriKind.Relative);
        }
    }
}

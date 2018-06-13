using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Pipes;

namespace ServerControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            IPCManager.Initialize();

            // Create main application window, starting minimized if specified
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        [STAThread]
        public static void Main()
        {
            OnPreApplication();

            var application = new App();
            application.InitializeComponent();
            application.Run();

            OnPostApplication();
        }
        private static void OnPreApplication()
        {
            try
            {
                Debugging.ServerControlLog.Initialize();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Unexpected Exception During Log Initialization", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private static void OnPostApplication()
        {

        }
    }
}

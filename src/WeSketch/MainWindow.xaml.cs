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

namespace WeSketch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Login _loginPage;
        Registration _registrationPage;
        WeSketchApp _weSketchAppPage;

        /// <summary>
        /// Initialize window components of login and registration pages
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            _loginPage = new Login();
            _loginPage.UserLoggedInEvent += UserLoggedInEvent;
            _loginPage.buttonRegister.Click += ButtonRegister_Click;

            _registrationPage = new Registration();
            _registrationPage.UserLoggedInEvent += UserLoggedInEvent;
            
            mainFrame.NavigationService.Navigate(_loginPage);
        }

        /// <summary>
        /// Handles the Closing event of the MainWindow control.  Cleans up any instanciated objects.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_loginPage != null)
            {
                _loginPage.Closing();
                _loginPage = null;
            }

            if(_registrationPage != null)
            {
                _registrationPage.Closing();
                _registrationPage = null;
            }

            if(_weSketchAppPage != null)
            {
                _weSketchAppPage.Closing();
                _weSketchAppPage = null;
            }

            Application.Current.Shutdown();
        }

        /// <summary>
        /// Navigates to the registration page.
        /// </summary>
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(_registrationPage);
        }

        /// <summary>
        /// Triggers when a user logs in.
        /// </summary>
        private void UserLoggedInEvent()
        {
            Dispatcher.Invoke(() =>
            {
                _weSketchAppPage = new WeSketchApp();
                mainFrame.NavigationService.Navigate(_weSketchAppPage);
            });
        }
    }
}

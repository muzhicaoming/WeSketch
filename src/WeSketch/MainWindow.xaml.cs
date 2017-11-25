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
        /// <summary>
        /// 
        /// </summary>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(_registrationPage);
        }

        /// <summary>
        /// 
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

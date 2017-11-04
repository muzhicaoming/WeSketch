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
        public MainWindow()
        {
            InitializeComponent();

            _loginPage = new Login();
            _loginPage.UserLoggedInEvent += UserLoggedInEvent;
            _loginPage.buttonRegister.Click += ButtonRegister_Click;

            _registrationPage = new Registration();
            _registrationPage.UserLoggedInEvent += UserLoggedInEvent;
            
            mainFrame.NavigationService.Navigate(_loginPage);
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

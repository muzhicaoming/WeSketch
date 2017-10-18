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
using WeSketchSharedDataModels;

namespace WeSketch
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        WeSketchRestRequests _rest = new WeSketchRestRequests();
        public Login()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Registration.xaml", UriKind.Relative));
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(userName.Text) &&
                !string.IsNullOrWhiteSpace(password.Password))
            {
                try
                {
                    Task.Run(() => AuthenticateUser());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private async void AuthenticateUser()
        {
            await _rest.Login(userName.Text, password.Password).ContinueWith(usr => UserLoggedIn(usr.Result));
        }

        private void UserLoggedIn(User user)
        {
            WeSketchClientData.Instance.User = user;
            NavigationService.Navigate(new Uri("WeSketchApp.xaml", UriKind.Relative));
        }
    }
}

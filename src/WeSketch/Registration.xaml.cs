using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public delegate void UserLoggedInEventHandler();
        public event UserLoggedInEventHandler UserLoggedInEvent;

        WeSketchRestRequests _rest = new WeSketchRestRequests();
        public Registration()
        {
            InitializeComponent();
        }

        private void regiserNew_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateInput())
            {
                Task.Run(() => CreateUser());
            }
        }

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(userName.Text) &&
                !string.IsNullOrWhiteSpace(email.Text) &&
                !string.IsNullOrWhiteSpace(password.Password) &&
                !string.IsNullOrWhiteSpace(passwordConfirm.Password) &&
                password.Password.Equals(passwordConfirm.Password);

        }

        private void popup_Closed(object sender, EventArgs e)
        {

        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void passwordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private async void CreateUser()
        {
            WeSketchRestRequests rest = new WeSketchRestRequests();
            string username = "";
            string pass = "";
            string userEmail = "";
            Dispatcher.Invoke(() =>
            {
                username = userName.Text;
                pass = password.Password;
                userEmail = email.Text;
            });
            
            User user = await rest.CreateUser(username, pass, userEmail);//.ContinueWith(usr => UserLoggedIn(usr.Result));
            UserLoggedIn(user);
        }

        private void UserLoggedIn(User user)
        {
            WeSketchClientData.Instance.User = user;
            UserLoggedInEvent.Invoke();
        }
    }
}

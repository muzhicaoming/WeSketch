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
        public delegate void UserLoggedInEventHandler();
        public event UserLoggedInEventHandler UserLoggedInEvent;

        WeSketchRestRequests _rest = new WeSketchRestRequests();
        public Login()
        {
            InitializeComponent();
        }

        public void Closing()
        {
            _rest.Dispose();
            _rest = null;
        }

        /// <summary>
        /// Event for pressing the login button, checks for blank or empty space, runs authentication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(userName.Text) &&
                !string.IsNullOrWhiteSpace(password.Password))
            {
                try
                {
                    string user = "";
                    string pass = "";
                    Dispatcher.Invoke(() =>
                    {
                        user = userName.Text;
                        pass = password.Password;
                    });

                    Task.Run(() => AuthenticateUser(user, pass));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        /// <summary>
        /// Authenticate user method, checks username and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public async void AuthenticateUser(string userName, string password)
        {
            try
            {
                User user = await _rest.Login(userName, password);
                UserLoggedIn(user);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// User logged in event, creates instance of user
        /// </summary>
        /// <param name="user"></param>
        private void UserLoggedIn(User user)
        {
            WeSketchClientData.Instance.User = user;
            UserLoggedInEvent.Invoke();
        }
    }
}

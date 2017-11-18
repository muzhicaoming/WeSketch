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
                string user = "";
                string eml = "";
                string pass = "";
                Dispatcher.Invoke(() =>
                {
                    user = userName.Text;
                    eml = email.Text;
                    pass = password.Password;
                });

                Task.Run(() => CreateUser(user, eml, pass));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public async void CreateUser(string userName, string email, string password)
        {
            WeSketchRestRequests rest = new WeSketchRestRequests();
            
            User user = await rest.CreateUser(userName, password, email);//.ContinueWith(usr => UserLoggedIn(usr.Result));
            UserLoggedIn(user);
        }

        private void UserLoggedIn(User user)
        {
            WeSketchClientData.Instance.User = user;
            UserLoggedInEvent.Invoke();
        }
    }
}

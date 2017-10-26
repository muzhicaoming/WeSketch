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

namespace WeSketch
{
    /// <summary>
    /// Interaction logic for InviteWindow.xaml
    /// </summary>
    public partial class InviteWindow : Window
    {
        public delegate void UserInvitedEventHandler(string user);
        public event UserInvitedEventHandler UserInvitedEvent;

        public InviteWindow()
        {
            InitializeComponent();
            buttonCancel.Click += ButtonCancel_Click;
            buttonInvite.Click += ButtonInvite_Click;
        }

        private void ButtonInvite_Click(object sender, RoutedEventArgs e)
        {
            UserInvitedEvent.Invoke(userName.Text);
            userName.Text = "";
            Hide();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            userName.Text = "";
            Hide();
        }
    }
}

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
        /// <summary>
        /// Triggers when a user is invited.
        /// </summary>
        /// <param name="user">The user.</param>
        public delegate void UserInvitedEventHandler(string user);
        public event UserInvitedEventHandler UserInvitedEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteWindow"/> class.
        /// </summary>
        public InviteWindow()
        {
            InitializeComponent();
            buttonCancel.Click += ButtonCancel_Click;
            buttonInvite.Click += ButtonInvite_Click;
        }

        /// <summary>
        /// Handles the Click event of the ButtonInvite control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonInvite_Click(object sender, RoutedEventArgs e)
        {
            UserInvitedEvent.Invoke(userName.Text);
            userName.Text = "";
            Hide();
        }

        /// <summary>
        /// Handles the Click event of the ButtonCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            userName.Text = "";
            Hide();
        }
    }
}

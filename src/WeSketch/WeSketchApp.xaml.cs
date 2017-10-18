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
    /// Interaction logic for WeSketchApp.xaml
    /// </summary>
    public partial class WeSketchApp : Page
    {
        private WeSketchRestRequests _rest = new WeSketchRestRequests();
        private WeSketchSignalrClient _client = new WeSketchSignalrClient();
        
        public WeSketchApp()
        {
            InitializeComponent();
            _client.UserAuthenticated(WeSketchClientData.Instance.User.UserID);
            _client.JoinBoardGroup(WeSketchClientData.Instance.User.Board.BoardID);
            _client.BoardInvitationReceivedEvent += BoardInvitationReceivedEvent;
        }

        private void BoardInvitationReceivedEvent(string user)
        {
            throw new NotImplementedException();
        }
    }
}

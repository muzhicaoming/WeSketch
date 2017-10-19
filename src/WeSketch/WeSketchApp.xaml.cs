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

            ik.StrokeCollected += Ik_StrokeCollected;
            ik.StrokeErased += Ik_StrokeErased;

            _client.UserAuthenticated(WeSketchClientData.Instance.User.UserID);
            _client.JoinBoardGroup(WeSketchClientData.Instance.User.Board.BoardID);
            _client.BoardInvitationReceivedEvent += BoardInvitationReceivedEvent;
            _client.BoardChangedEvent += _client_BoardChangedEvent;
            _client.StrokesReceivedEvent += _client_StrokesReceivedEvent;
        }

        private void Ik_StrokeErased(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ik_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _client_StrokesReceivedEvent(System.Windows.Ink.StrokeCollection strokes)
        {
            ik.Strokes.Add(strokes);
        }

        private void _client_BoardChangedEvent(Guid boardId)
        {
            WeSketchClientData.Instance.User.Board.BoardID = boardId;
            ik.Strokes.Clear();
            _client.RequestStrokes(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.User.Board.BoardID);
        }

        private void BoardInvitationReceivedEvent(string user, Guid boardId)
        {
            MessageBoxResult result = MessageBox.Show($"User {user} invited you to their board.  Would you like to join?", "Join board?", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                _client.LeaveBoardGroup(WeSketchClientData.Instance.User.Board.BoardID);
                _client.JoinBoardGroup(boardId);
            }
        }
    }
}

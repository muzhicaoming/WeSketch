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
        InviteWindow _inviteWindow = new InviteWindow();

        /// <summary>
        /// Initialize and set WeSketchApp client objects
        /// </summary>
        public WeSketchApp()
        {
            InitializeComponent();
            
            mainInkCanvas.StrokeCollected += StrokeCollected;
            mainInkCanvas.StrokeErasing += Ik_StrokeErasing;

            clearButton.Click += clearButton_Click;
            leaveButton.Click += leaveButton_Click;
            inviteButton.Click += InviteButton_Click;

            _client.UserAuthenticated(WeSketchClientData.Instance.User.UserID);
            _client.JoinBoardGroup(WeSketchClientData.Instance.User.Board.BoardID);
            _client.BoardInvitationReceivedEvent += BoardInvitationReceivedEvent;
            _client.BoardChangedEvent += _client_BoardChangedEvent;
            _client.StrokesReceivedEvent += _client_StrokesReceivedEvent;
            _client.StrokeRequestReceivedEvent += _client_StrokeRequestReceivedEvent;
            _client.StrokeClearEvent += _client_StrokeClearEvent;

            _inviteWindow.UserInvitedEvent += _inviteWindow_UserInvitedEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        private void _inviteWindow_UserInvitedEvent(string user)
        {
            _rest.InviteUserToBoard(WeSketchClientData.Instance.User.UserName, user, WeSketchClientData.Instance.User.Board.BoardID);
        }

        private void InviteButton_Click(object sender, RoutedEventArgs e)
        {
            if(_inviteWindow == null)
            {
                _inviteWindow = new InviteWindow();
            }
            _inviteWindow.Show();
        }

        /// <summary>
        /// Ink canvas has strokes cleared event
        /// </summary>
        private void _client_StrokeClearEvent()
        {
            this.mainInkCanvas.Strokes.Clear();
        }

        private void Ik_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _client_StrokeRequestReceivedEvent(string requestingUser)
        {
            // TODO: Send the board strokes to the requesting user.
            Dispatcher.Invoke(() =>
            {
                _client.SendStrokesToUser(requestingUser, mainInkCanvas.Strokes);
            });
        }

        private void StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            _client.SendStroke(WeSketchClientData.Instance.User.Board.BoardID, e.Stroke);
        }

        private void _client_StrokesReceivedEvent(System.Windows.Ink.StrokeCollection strokes)
        {
            if(strokes.Any())
            {
                Dispatcher.Invoke(() =>
                {
                    mainInkCanvas.Strokes.Add(strokes);
                });
            } //maininkcanvas
        }

        private void _client_BoardChangedEvent(Guid boardId)
        {
            WeSketchClientData.Instance.User.Board.BoardID = boardId;
            Dispatcher.Invoke(() =>
            {
                mainInkCanvas.Strokes.Clear();
            });
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

        /// <summary>
        /// Clear button is clicked by user, clears their board on inkcanvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainInkCanvas.Strokes.Clear();
            _client.StrokesClearedSend(WeSketchClientData.Instance.User.Board.BoardID);
            MessageBox.Show("Clear button pressed.");
        }

        /// <summary>
        /// User clicks leave button, leaves the current board session instance.
        /// </summary>
        private void leaveButton_Click(object sender, RoutedEventArgs e)
        {
            _client.LeaveBoardGroup(WeSketchClientData.Instance.User.Board.BoardID);
            MessageBox.Show("WeSketch leave button pressed.");
        }
    }
}

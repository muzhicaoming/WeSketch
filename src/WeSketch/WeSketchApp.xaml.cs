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
using System.Windows.Ink;
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
            mainInkCanvas.StrokeErasing += StrokeErasing;

            clearButton.Click += ClearButton_Click;
            leaveButton.Click += LeaveButton_Click;
            inviteButton.Click += InviteButton_Click;

            _client.UserAuthenticated(WeSketchClientData.Instance.User.UserID);
            _client.JoinBoardGroup(WeSketchClientData.Instance.User.Board.BoardID);
            _client.BoardInvitationReceivedEvent += BoardInvitationReceivedEvent;
            _client.BoardChangedEvent += BoardChangedEvent;
            _client.StrokesReceivedEvent += StrokesReceivedEvent;
            _client.StrokeRequestReceivedEvent += StrokeRequestReceivedEvent;
            _client.StrokeClearEvent += StrokeClearEvent;

            _inviteWindow.UserInvitedEvent += InviteWindow_UserInvitedEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        private void InviteWindow_UserInvitedEvent(string user)
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
        /// Method to change brush color from the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboColor.Items.Count > 0 && ((ComboBoxItem)comboColor.SelectedItem).Content != null)
            {
                /// Set brush color to selected color
                if (((ComboBoxItem)comboColor.SelectedItem).Content.ToString() == “Black”)
                
                    mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
                
                else if (((ComboBoxItem)comboColor.SelectedItem).Content.ToString() == “Blue”)
                
                    mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Blue;
                
                else if (((ComboBoxItem)comboColor.SelectedItem).Content.ToString() == “Green”)
                
                    mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Green;
                
                else if (((ComboBoxItem)comboColor.SelectedItem).Content.ToString() == “Red”)

                    mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
                
                else if (((ComboBoxItem)comboColor.SelectedItem).Content.ToString() == “Yellow”)
                
                    mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Yellow;
                
            }
        }

        /// <summary>
        /// Ink canvas has strokes cleared event
        /// </summary>
        private void StrokeClearEvent()
        {
            this.mainInkCanvas.Strokes.Clear();
        }

        private void StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void StrokeRequestReceivedEvent(string requestingUser)
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

        private void StrokesReceivedEvent(System.Windows.Ink.StrokeCollection strokes)
        {
            if(strokes.Any())
            {
                Dispatcher.Invoke(() =>
                {
                    mainInkCanvas.Strokes.Add(strokes);
                });
            } //maininkcanvas
        }

        private void BoardChangedEvent(Guid boardId)
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
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainInkCanvas.Strokes.Clear();
            _client.StrokesClearedSend(WeSketchClientData.Instance.User.Board.BoardID);
            MessageBox.Show("Clear button pressed.");
        }

        /// <summary>
        /// User clicks leave button, leaves the current board session instance.
        /// </summary>
        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            _client.LeaveBoardGroup(WeSketchClientData.Instance.User.Board.BoardID);
            MessageBox.Show("WeSketch leave button pressed.");
        }
    }
}

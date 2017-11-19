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

            WeSketchClientData.Instance.Color = "Black";
            userLabel.Content = WeSketchClientData.Instance.User.UserName;
            rbDraw.Checked += RadioButton_Checked;
            rbErase.Checked += RadioButton_Checked;

            mainInkCanvas.StrokeCollected += StrokeCollected;
            mainInkCanvas.StrokeErasing += StrokeErasing;

            clearButton.Click += ClearButton_Click;
            leaveButton.Click += LeaveButton_Click;
            inviteButton.Click += InviteButton_Click;

            _client.UserAuthenticated(WeSketchClientData.Instance.User.UserID);
            _client.JoinBoardGroup(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.Color, WeSketchClientData.Instance.User.Board.BoardID);
            _client.BoardInvitationReceivedEvent += BoardInvitationReceivedEvent;
            _client.BoardChangedEvent += BoardChangedEvent;
            _client.StrokesReceivedEvent += StrokesReceivedEvent;
            _client.StrokeRequestReceivedEvent += StrokeRequestReceivedEvent;
            _client.StrokeClearEvent += StrokeClearEvent;
            _client.StrokeErasedEvent += StrokeErasedEvent;
            _inviteWindow.UserInvitedEvent += InviteWindow_UserInvitedEvent;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                mainInkCanvas.EditingMode = (rbDraw.IsChecked ?? false) ? InkCanvasEditingMode.Ink : InkCanvasEditingMode.EraseByPoint;
            });
        }

        /// <summary>
        /// The given stroke was erased from the board.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        private void StrokeErasedEvent(Stroke stroke)
        {
            stroke.StylusPoints.ToList().ForEach(point =>
            {
                mainInkCanvas.Strokes.ToList().ForEach(boardStroke =>
                {
                    var points = boardStroke.StylusPoints.Where(pnt => pnt.X == point.X && pnt.Y == point.Y);
                    if(points.Any())
                    {
                        points.ToList().ForEach(pnt =>
                        {
                            boardStroke.StylusPoints.Remove(pnt);
                        });
                    }
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        private void InviteWindow_UserInvitedEvent(string user)
        {
            _rest.InviteUserToBoard(WeSketchClientData.Instance.User.UserName, user, WeSketchClientData.Instance.User.Board.BoardID);
        }

        /// <summary>
        /// Method for the button that invites a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                switch(((ComboBoxItem)comboColor.SelectedItem).Content.ToString())
                {
                    case "Black":
                        mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
                        WeSketchClientData.Instance.Color = "Black";
                        break;
                    case "Blue":
                        mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Blue;
                        WeSketchClientData.Instance.Color = "Blue";
                        break;
                    case "Green":
                        mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Green;
                        WeSketchClientData.Instance.Color = "Green";
                        break;
                    case "Red":
                        mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
                        WeSketchClientData.Instance.Color = "Red";
                        break;
                    case "Yellow":
                        mainInkCanvas.DefaultDrawingAttributes.Color = Colors.Yellow;
                        WeSketchClientData.Instance.Color = "Yelow";
                        break;
                }

                _client.ChangeUserColor(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.Color);
            }
        }

        /// <summary>
        /// Method to change the brush stroke size from the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboSize.Items.Count > 0 && ((ComboBoxItem)comboSize.SelectedItem).Content != null)
            {
                /// Sets the brush size.
                mainInkCanvas.DefaultDrawingAttributes.Width = Convert.ToDouble(((ComboBoxItem)comboSize.SelectedItem).Content);
                mainInkCanvas.DefaultDrawingAttributes.Height = Convert.ToDouble(((ComboBoxItem)comboSize.SelectedItem).Content);
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
            _client.SendStrokeToErase(e.Stroke, WeSketchClientData.Instance.User.Board.BoardID);
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

        /// <summary>
        /// Informs the client that the active board was changed.
        /// </summary>
        /// <param name="boardId">The board identifier.</param>
        private void BoardChangedEvent(Guid boardId)
        {
            WeSketchClientData.Instance.User.Board.BoardID = boardId;
            Dispatcher.Invoke(() =>
            {
                mainInkCanvas.Strokes.Clear();
            });
            _client.RequestStrokes(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.User.Board.BoardID);
        }

        /// <summary>
        /// Informs the client that an invitation to another board was received.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="boardId">The board identifier.</param>
        private void BoardInvitationReceivedEvent(string user, Guid boardId)
        {
            MessageBoxResult result = MessageBox.Show($"User {user} invited you to their board.  Would you like to join?", "Join board?", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                _client.LeaveBoardGroup(user, WeSketchClientData.Instance.User.Board.BoardID);
                _client.JoinBoardGroup(user, WeSketchClientData.Instance.Color, boardId);
            }
        }

        /// <summary>
        /// Clear button is clicked by user, clears their board on inkcanvas.
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainInkCanvas.Strokes.Clear();
            _client.StrokesClearedSend(WeSketchClientData.Instance.User.Board.BoardID);
        }

        /// <summary>
        /// User clicks leave button, leaves the current board session instance.
        /// </summary>
        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            _client.LeaveBoardGroup(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.User.Board.BoardID);
        }
    }
}

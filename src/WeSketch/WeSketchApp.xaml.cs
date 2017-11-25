﻿using System;
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
            _client.ConnectedUsersReceivedEvent += ConnectedUsersReceivedEvent;
            _client.ConnectedUsersRequestReceivedEvent += ConnectedUsersRequestReceivedEvent;
            _client.UserJoinedBoardEvent += UserJoinedBoardEvent;
            _client.UserLeftBoardEvent += UserLeftBoardEvent;
            LoadConnectedUsers();
        }

        /// <summary>
        /// Notifies the client that a user left the board.
        /// </summary>
        /// <param name="user">The user.</param>
        private void UserLeftBoardEvent(string user)
        {
            Dispatcher.Invoke(() =>
            {
                var connectedUser = WeSketchClientData.Instance.User.Board.ConnectedUsers.Where(usr => usr.UserName == user);
                if (connectedUser.Any())
                {
                    connectedUser.ToList().ForEach(usr =>
                    WeSketchClientData.Instance.User.Board.ConnectedUsers.Remove(usr));
                    lbConnectedUsers.UpdateLayout();
                }
            });
        }

        /// <summary>
        /// Notifies the client that a user connected to the board.
        /// </summary>
        /// <param name="user">The user.</param>
        private void UserJoinedBoardEvent(ConnectedUser user)
        {
            Dispatcher.Invoke(() =>
            {
                WeSketchClientData.Instance.User.Board.ConnectedUsers.Add(user);
                lbConnectedUsers.UpdateLayout();
            });
        }

        /// <summary>
        /// Notifies the client that a user requested that they be sent all of the connected users.
        /// </summary>
        /// <param name="user">The user.</param>
        private void ConnectedUsersRequestReceivedEvent(string user)
        {
            _client.SendConnectedUsersToUser(user, WeSketchClientData.Instance.User.Board.ConnectedUsers);
        }

        /// <summary>
        /// Notifies the client that they received all of the connected users from the board owner.
        /// </summary>
        /// <param name="connectedUsers">The connected users.</param>
        private void ConnectedUsersReceivedEvent(List<ConnectedUser> connectedUsers)
        {
            Dispatcher.Invoke(() =>
            {
                WeSketchClientData.Instance.User.Board.ConnectedUsers = connectedUsers;
                lbConnectedUsers.UpdateLayout();
            });
        }

        /// <summary>
        /// Handles the Checked event of the RadioButton control used for switching between drawing modes. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                mainInkCanvas.EditingMode = (rbDraw.IsChecked ?? false) ? InkCanvasEditingMode.Ink : InkCanvasEditingMode.EraseByStroke;
            });
        }

        /// <summary>
        /// The given stroke was erased from the board.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        private void StrokeErasedEvent(Guid id)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    var stroke = mainInkCanvas.Strokes.SingleOrDefault(strk => strk.ContainsPropertyData(id));
                    if (stroke != null)
                    {
                        mainInkCanvas.Strokes.Remove(stroke);
                    }
                }
                catch { }
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

        private void LoadConnectedUsers()
        {
            Dispatcher.Invoke(() =>
            {
                lbConnectedUsers.ItemsSource = WeSketchClientData.Instance.User.Board.ConnectedUsers;
            });
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
            Dispatcher.Invoke(() =>
            {
                this.mainInkCanvas.Strokes.Clear();
            });
        }

        /// <summary>
        /// Occurrs when a stroke is being erased.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="InkCanvasStrokeErasingEventArgs"/> instance containing the event data.</param>
        private void StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            _client.SendStrokeToErase(e.Stroke, WeSketchClientData.Instance.User.Board.BoardID);
        }

        /// <summary>
        /// Notifies the client that someone requested the boards strokes.
        /// </summary>
        /// <param name="requestingUser">The requesting user.</param>
        private void StrokeRequestReceivedEvent(string requestingUser)
        {
            Dispatcher.Invoke(() =>
            {
                _client.SendStrokesToUser(requestingUser, mainInkCanvas.Strokes);
            });
        }

        /// <summary>
        /// Notifies the client that strokes were collected from client user input.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="InkCanvasStrokeCollectedEventArgs"/> instance containing the event data.</param>
        private void StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            Guid id = Guid.NewGuid();
            e.Stroke.AddPropertyData(id, WeSketchClientData.Instance.User.UserName);
            _client.SendStroke(WeSketchClientData.Instance.User.Board.BoardID, e.Stroke);
        }

        /// <summary>
        /// Notifies the client that strokes were received from the signalr client.
        /// </summary>
        /// <param name="strokes">The strokes.</param>
        private void StrokesReceivedEvent(System.Windows.Ink.StrokeCollection strokes)
        {
            if(strokes.Any())
            {
                Dispatcher.Invoke(() =>
                {
                    mainInkCanvas.Strokes.Add(strokes);
                });
            }
        }

        /// <summary>
        /// Informs the client that the active board was changed.
        /// </summary>
        /// <param name="boardId">The board identifier.</param>
        private void BoardChangedEvent(Guid boardId)
        {
            Dispatcher.Invoke(() =>
            {
                WeSketchClientData.Instance.User.Board.BoardID = boardId;
                mainInkCanvas.Strokes.Clear();
                _client.RequestStrokes(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.User.Board.BoardID);
            });
        }

        /// <summary>
        /// Informs the client that an invitation to another board was received.
        /// </summary>
        /// <param name="user">The user that invited you to their board.</param>
        /// <param name="boardId">The board identifier that you are invited to join.</param>
        private void BoardInvitationReceivedEvent(string user, Guid boardId)
        {
            MessageBoxResult result = MessageBox.Show($"User {user} invited you to their board.  Would you like to join?", "Join board?", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                _client.LeaveBoardGroup(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.User.Board.BoardID);
                _client.JoinBoardGroup(WeSketchClientData.Instance.User.UserName, WeSketchClientData.Instance.Color, boardId);

                LoadConnectedUsers();
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

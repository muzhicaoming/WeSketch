using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Client;
using System.Configuration;

namespace WeSketch
{
    /// <summary>
    /// Communication portal for the client to send and receive messages from the hub running on WeSketchAPI.
    /// </summary>
    class WeSketchSignalrClient
    {

        public delegate void BoardInvitationReceivedEventHandler(string user);
        public event BoardInvitationReceivedEventHandler BoardInvitationReceivedEvent;
#if DEBUG
        private string _url = ConfigurationManager.AppSettings["debugUrl"];
#else
        private string _url = ConfigurationManager.AppSettings["url"];
#endif
        private HubConnection _hub;
        private IHubProxy _hubProxy;

        private Queue<Action> _queuedActions = new Queue<Action>();


        public delegate void BoardChangedEventHandler();
        public event BoardChangedEventHandler BoardChangedEvent;

        public WeSketchSignalrClient()
        {
            _hub = new HubConnection(_url);
            _hub.Received += Hub_Received;
            _hub.Reconnected += Hub_Reconnected;
            _hub.Reconnecting += Hub_Reconnecting;
            _hub.Closed += Hub_Closed;
            _hub.ConnectionSlow += Hub_ConnectionSlow;
            _hub.Error += Hub_Error;
            _hub.StateChanged += Hub_StateChanged;
            _hubProxy = _hub.CreateHubProxy("WeSketchAPIHub");

            _hubProxy.On("ReceiveInvitation", user => ReceiveInvitation(user));

            _hub.Start().Wait();
        }

        private void Hub_StateChanged(StateChange obj)
        {
        }

        private void Hub_Error(Exception obj)
        {
        }

        private void Hub_ConnectionSlow()
        {
        }

        private void Hub_Closed()
        {
        }

        private void Hub_Reconnecting()
        {
        }

        private void Hub_Reconnected()
        {
            lock (_queuedActions)
            {
                if (_queuedActions.Any())
                {
                    while (_queuedActions.Peek() != null && _hub.State == ConnectionState.Connected)
                    {
                        Action act = _queuedActions.Dequeue();
                        act.Invoke();
                    }
                }
            }
        }

        private void Hub_Received(string obj)
        {
        }

        public void ReceiveStrokes(string serializedStrokes)
        {
            if (_hub.State == ConnectionState.Connected)
            {
            }
            else
            {
                lock (_queuedActions)
                {
                    _queuedActions.Enqueue(new Action(() => ));
                }
            }
        }

        public void SendStrokes(Guid boardId)
        {
            if (_hub.State == ConnectionState.Connected)
            {
                
            }
            else
            {
                lock (_queuedActions)
                {
                    _queuedActions.Enqueue(new Action(() => ));
                }
            }
        }

        public void SendStrokesToUser(string userId)
        {
            if (_hub.State == ConnectionState.Connected)
            {
                
            }
            else
            {
                lock (_queuedActions)
                {
                    _queuedActions.Enqueue(new Action(() => ));
                }
            }
        }

        public void JoinBoardGroup(Guid boardId)
        {
            _hubProxy.Invoke<Task>("JoinBoardGroup", boardId);
        }

        public void LeaveBoardGroup(Guid boardId)
        {
            _hubProxy.Invoke<Task>("LeaveBoardGroup", boardId);
        }

        public void ReceiveInvitation(string user)
        {
            BoardInvitationReceivedEvent?.Invoke(user);
        }

        public void UserAuthenticated(Guid userId)
        {
            _hubProxy.Invoke("UserAuthenticated", userId);
        }

        public void Dispose()
        {
            _hub.Dispose();
            _hub = null;
        }
    }
}

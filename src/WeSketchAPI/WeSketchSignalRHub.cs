using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace WeSketchAPI
{
    /// <summary>
    /// SignalR hub for receiving requests from clients.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
    public class WeSketchSignalRHub : Hub
    {
        public Task JoinBoardGroup(Guid boardId)
        {   
            return Groups.Add(Context.ConnectionId, boardId.ToString());
        }
        public Task LeaveBoardGroup(Guid boardId)
        {
            return Groups.Remove(Context.ConnectionId, boardId.ToString());
        }
        public void SendInvitation(string fromUser, Guid toUser)
        {
            Clients.Group(toUser.ToString()).ReceiveInvitation(fromUser);
        }
        public void SendStrokesToGroup(Guid boardId, string serializedStrokes)
        {
            Clients.Group(boardId.ToString(), Context.ConnectionId).ReceiveStrokes(serializedStrokes);
        }
        public void RequestStrokes(string userId, Guid boardId)
        {

        }
        public void SendStrokesToUser(string serializedStrokes, string userId)
        {

        }
        public void UserAuthenticated(Guid userId)
        {
            Groups.Add(Context.ConnectionId, userId.ToString());
        }
    }
}
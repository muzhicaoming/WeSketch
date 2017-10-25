using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeSketchAPI
{
    /// <summary>
    /// SignalR hub for receiving requests from clients.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
    public class WeSketchSignalRHub : Hub
    {
        /// <summary>
        /// Joins the user to the board group.  This allows the userto receive strokes sent to the group.
        /// </summary>
        /// <param name="boardId">The board identifier.</param>
        /// <returns></returns>
        public void JoinBoardGroup(Guid boardId)
        {   
            Groups.Add(Context.ConnectionId, boardId.ToString());
        }

        /// <summary>
        /// Leaves the board group.  This will stop masseges to this group from being sent to the specified user.
        /// </summary>
        /// <param name="boardId">The board identifier.</param>
        /// <returns></returns>
        public void LeaveBoardGroup(Guid boardId)
        {
            Groups.Remove(Context.ConnectionId, boardId.ToString());
            using (var db = new WeSketchDataContext())
            {

            }
        }

        /// <summary>
        /// Sends the invitation to the specified user.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="toUser">To user.</param>
        public void SendInvitation(string fromUser, Guid toUser)
        {
            Clients.Group(toUser.ToString()).ReceiveInvitation(fromUser);
        }

        /// <summary>
        /// Sends the stroke to erase.
        /// </summary>
        /// <param name="seriailizedStroke">The seriailized stroke.</param>
        /// <param name="boardId">The board identifier.</param>
        public void SendStrokeToErase(string seriailizedStroke, Guid boardId)
        {
            Clients.Group(boardId.ToString(), Context.ConnectionId).ReceiveStrokeToErase(seriailizedStroke);
        }

        /// <summary>
        /// Sends the strokes to all in the group except to the calling user.
        /// </summary>
        /// <param name="boardId">The board identifier.</param>
        /// <param name="serializedStrokes">The serialized strokes.</param>
        public void SendStrokesToGroup(Guid boardId, string serializedStrokes)
        {
            Clients.Group(boardId.ToString(), Context.ConnectionId).ReceiveStrokes(serializedStrokes);
        }

        /// <summary>
        /// After a user joins a board they request strokes from a board.
        /// </summary>
        /// <param name="user">The user identifier.</param>
        /// <param name="boardId">The board identifier.</param>
        public void RequestStrokes(string user, Guid boardId)
        {
            using (var db = new WeSketchDataContext())
            {
                var board = db.UserBoards.Single(brd => brd.BoardID == boardId && brd.BoardOwner);
                var receiver = db.Users.Single(usr => usr.UserName == user);
                Clients.Group(receiver.UserID.ToString()).ReceiveStrokeRequest(user);
            }
        }

        /// <summary>
        /// Sends the strokes to the specieied user.
        /// </summary>
        /// <param name="serializedStrokes">The serialized strokes.</param>
        /// <param name="userId">The user identifier.</param>
        public void SendStrokesToUser(string user, string serializedStrokes)
        {
            using (var db = new WeSketchDataContext())
            {
                var receiver = db.Users.Single(usr => usr.UserName == user);
                Clients.Group(receiver.UserID.ToString()).ReceiveStrokes(serializedStrokes);
            }
        }

        /// <summary>
        /// Send message to all users connected to the board except the requester to clear the boards strokes.
        /// </summary>
        /// <param name="boardId">The board ID.</param>
        public void RequestClearBoardStrokes(Guid boardId)
        {
            Clients.Group(boardId.ToString(), Context.ConnectionId).StrokesClearedReceived();
        }

        /// <summary>
        /// User is confirmed as authenticated and then is placed in a gourp
        /// that is unique to the user.  This allows messages to be sent ot individual users.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void UserAuthenticated(Guid userId)
        {
            Groups.Add(Context.ConnectionId, userId.ToString());
        }
    }
}
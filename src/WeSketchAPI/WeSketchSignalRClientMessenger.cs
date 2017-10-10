using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeSketchAPI
{
    /// <summary>
    /// Abstracting SignalR client methods into a class to simplify calls to the client methods.
    /// </summary>
    public class WeSketchSignalRClientMessenger
    {
        /// <summary>
        /// Sends the strokes to the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void SendStrokes(string user)
        {

        }

        /// <summary>
        /// Sends the strokes to members of the specified board.
        /// </summary>
        /// <param name="boardId">The board identifier.</param>
        public void SendBoardStrokes(Guid boardId)
        {

        }

        /// <summary>
        /// Requests the strokes.
        /// </summary>
        /// <param name="userRequesting">The user requesting.</param>
        /// <param name="userSupplying">The user supplying.</param>
        public void RequestStrokes(string userRequesting, string userSupplying)
        {

        }
    }
}
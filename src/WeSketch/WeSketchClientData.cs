using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSketchSharedDataModels;

namespace WeSketch
{
    class WeSketchClientData
    {
        public delegate void BoardChangedEventHandler();
        public event BoardChangedEventHandler BoardChangedEvent;

        public User User { set; get; }

        public void AddConnectedUser(Guid userId, string userName)
        {
            if (!User.Board.ConnectedUsers.Keys.Any(k => k == userId))
            {
                User.Board.ConnectedUsers.Add(userId, userName);
            }
        }

        public void ChangeBoard(Guid boardId, bool owner)
        {
            User.Board.BoardID = boardId;
            User.Board.Owner = owner;
            BoardChangedEvent?.Invoke();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSketchSharedDataModels;

namespace WeSketch
{
    public class WeSketchClientData
    {
        public delegate void BoardChangedEventHandler();
        public static event BoardChangedEventHandler BoardChangedEvent;


        private static Lazy<WeSketchClientData> _instance = new Lazy<WeSketchClientData>(() => new WeSketchClientData());
        public static WeSketchClientData Instance => _instance.Value;
        private WeSketchClientData()
        {

        }

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

    public class WeSketchEventArgs
    {

    }
}

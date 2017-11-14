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
        public User User { set; get; }
        public string Color { set; get; }
        private static Lazy<WeSketchClientData> _instance = new Lazy<WeSketchClientData>(() => new WeSketchClientData());
        public static WeSketchClientData Instance => _instance.Value;
        private WeSketchClientData()
        {

        }
        public void AddConnectedUser(ConnectedUser user)
        {
            if (!User.Board.ConnectedUsers.Any(usr => usr.UserName == user.UserName))
            {
                User.Board.ConnectedUsers.Add(user);
            }
        }
    }
}

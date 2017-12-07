using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeSketchSharedDataModels;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;

namespace WeSketchAPI.Controllers
{
    /// <summary>
    /// WeSketch REST API calls.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        /// <summary>
        /// Logins the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpPost]
        public string Login(string user, string password)
        {
            var result = new Result();
            try
            {
                WeSketchSecurity security = new WeSketchSecurity();
                User existingUser = security.Login(user, password);
                WeSketchSharedDataModels.User newUserModel = new WeSketchSharedDataModels.User()
                {
                    UserID = existingUser.UserID,
                    UserName = existingUser.UserName,
                    Board = new Board()
                    {
                        BoardID = existingUser.UserBoard.BoardID,
                        Owner = existingUser.UserBoard.BoardOwner
                    }
                };

                result.ResultJSON = JsonConvert.SerializeObject(newUserModel);
            }
            catch(Exception e)
            {
                result.Error = true;
                result.ErrorMessage = e.Message;
            }
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpPost]
        public string CreateUser(string user, string password, string email)
        {
            var result = new Result();
            try
            {
                WeSketchSecurity security = new WeSketchSecurity();
                User newUser = security.CreateUser(user, password, email);
                WeSketchSharedDataModels.User newUserModel = new WeSketchSharedDataModels.User()
                {
                    UserID = newUser.UserID,
                    UserName = newUser.UserName,
                    Board = new Board()
                    {
                        BoardID = newUser.UserBoard.BoardID,
                        Owner = newUser.UserBoard.BoardOwner
                    }
                };

                result.ResultJSON = JsonConvert.SerializeObject(newUserModel);
            }
            catch (Exception e)
            {
                result.Error = true;
                result.ErrorMessage = e.Message;
            }
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Joins the board.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="boardId">The board identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public string JoinBoard(Guid userId, string userName, string hostUser)
        {
            var result = new Result();
            var board = new Board();
            try
            {
                using (var db = new WeSketchDataContext())
                {
                    var user = db.Users.Single(usr => usr.UserName == userName);
                    if (user.UserID == userId)
                    {
                        var host = db.Users.Single(usrHost => usrHost.UserName == hostUser);

                        user.UserBoard.BoardID = host.UserBoard.BoardID;
                        user.UserBoard.BoardOwner = false;
                        user.UserBoard.CanSketch = false;
                        db.SubmitChanges();

                        board.BoardID = host.UserBoard.BoardID;
                        board.Owner = false;
                        result.ResultJSON = JsonConvert.SerializeObject(board);
                    }
                    else
                    {
                        throw new Exception("Invalid user.");
                    }
                }
            }
            catch (Exception e)
            {
                result.Error = true;
                result.ErrorMessage = e.Message;
            }
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Invites the user to board.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="boardId">The board identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public string InviteUserToBoard(string fromUser, string toUser, Guid boardId)
        {
            var result = new Result();
            try
            {
                using (var db = new WeSketchDataContext())
                {
                    var context = GlobalHost.ConnectionManager.GetHubContext<WeSketchSignalRHub>();
                    var invitee = db.Users.SingleOrDefault(usr => usr.UserName == toUser);
                    if (invitee != null)
                    {
                        context.Clients.Group(invitee.UserID.ToString()).ReceiveInvitation(fromUser, boardId);
                    }
                    else
                    {
                        throw new Exception($"User {toUser} does not exist.  Please check spelling and try again.");
                    }
                }
            }
            catch (Exception e)
            {
                result.Error = true;
                result.ErrorMessage = e.Message;
            }
            return JsonConvert.SerializeObject(result);
        }
    }
}

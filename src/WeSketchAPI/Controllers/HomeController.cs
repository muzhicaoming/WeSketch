using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeSketchSharedDataModels;
using Newtonsoft.Json;

namespace WeSketchAPI.Controllers
{
    public class HomeController : Controller
    {
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

                result.ResultJSON = JsonConvert.SerializeObject(existingUser);
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
        public string JoinBoard(string user, string hostUser)
        {
            var result = new Result();
            try
            {
                // TODO: Update user board info on board record.
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
        public string InviteUserToBoard(string user, Guid boardId)
        {
            var result = new Result();
            try
            {
                // TODO: Add code to invite user to board.
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

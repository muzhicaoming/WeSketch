using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WeSketchSharedDataModels;

namespace WeSketch.Tests
{

    /// <summary>
    /// Class for testing possible WeSketchSignalrClient test cases
    /// </summary>
    [TestFixture]
    class WeSketchSignalrClientTests
    {

        /// <summary>
        /// Method to test valid username and changing color of username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="color"></param>
        /// </summary>
        [Test]
        [TestCase("", "")]
        [TestCase("", "somecolor")]
        [TestCase("someusername", "")]
        public void IsInvalidChangeUserColor(string username, string color)
        {
            WeSketchSignalrClient signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => signalr.ChangeUserColor(username, color));
            StringAssert.Contains("Error", ex.Message);
        }

        /// <summary>
        /// Method to test valid join board group event
        /// </summary>
        /// <param name="username"></param>
        /// <param name="color"></param>
        /// <param name="boardId"></param>
        [Test]
        [TestCase("", "", null)]
        [TestCase("someuser", "", null)]
        [TestCase("", "somecolor", null)]
        [TestCase("someuser", "somecolor", null)]
        public void IsInvalidJoinBoardGroup(string username, string color, Guid boardId)
        {
            WeSketchSignalrClient signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => signalr.JoinBoardGroup(username, color, boardId));
            StringAssert.Contains("Error", ex.Message);
        }

        /// <summary>
        /// Method to test valid kick user from board event
        /// </summary>
        /// <param name="user"></param>
        /// <param name="boardId"></param>
        [Test]
        [TestCase("", null)]
        [TestCase("someuser", null)]
        public void IsInvalidKickUserFromBoard(string user, Guid boardId)
        {
            WeSketchSignalrClient signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => signalr.KickUserFromBoard(user, boardId));
            StringAssert.Contains("Error", ex.Message);
        }

        /// <summary>
        /// Method to test valid leave board group event
        /// </summary>
        /// <param name="user"></param>
        /// <param name="boardId"></param>
        /// <param name="color"></param>
        [Test]
        [TestCase("", null, "")]
        [TestCase("someuser", null, "")]
        [TestCase("someuser", null, "somecolor")]
        [TestCase("", null, "somecolor")]
        public void IsInvalidLeaveBoardGroup(string user, Guid boardId, string color)
        {
            WeSketchSignalrClient signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => signalr.LeaveBoardGroup(user, boardId, color));
            StringAssert.Contains("Error", ex.Message);
        }

        /// <summary>
        /// Method to test valid requesting connected users
        /// </summary>
        /// <param name="user"></param>
        /// <param name="boardId"></param>
        [Test]
        [TestCase("", null)]
        [TestCase("someuser", null)]
        public void IsInvalidRequestConnectedUsers(string user, Guid boardId)
        {
            WeSketchSignalrClient signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => signalr.RequestConnectedUsers(user, boardId));
            StringAssert.Contains("Error", ex.Message);
        }

        /// <summary>
        /// Method to test valid authentication of user
        /// </summary>
        /// <param name="userId"></param>
        [Test]
        [TestCase(null)]
        public void IsInvalidUserAuthenticated(Guid userId)
        {
            WeSketchSignalrClient signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => signalr.UserAuthenticated(userId));
            StringAssert.Contains("Error", ex.Message);
        }
    }
}

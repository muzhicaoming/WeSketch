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
        public void IsInvalidChangeUserColor(string username, string color) {

            WeSketchSignalrClient _signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => _signalr.ChangeUserColor(username, color));
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
        public void IsInvalidJoinBoardGroup(string username, string color, Guid boardId) {

            WeSketchSignalrClient _signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => _signalr.JoinBoardGroup(username, color, boardId));
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

            WeSketchSignalrClient _signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => _signalr.KickUserFromBoard(user, boardId));
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

            WeSketchSignalrClient _signalr = new WeSketchSignalrClient();
            var ex = Assert.Catch<Exception>(() => _signalr.LeaveBoardGroup(user, boardId, color));
            StringAssert.Contains("Error", ex.Message);

        }
    }
}

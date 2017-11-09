using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WeSketchAPI;

namespace WeSketchAPI.Tests
{
    [TestFixture]
    class LoginTests
    {

        [Test]
        [TestCase("", "")]
        [TestCase("", "somepassword")]
        [TestCase("someusername", "")]
        public void IsInvalidAuthenticate_User(string username, string password)
        {
            Login lg = new Login();
            var ex = Assert.Catch<Exception>(() => lg.AuthenticateUser(username, password));
            StringAssert.Contains("Error", ex.Message);
        }
    }
}

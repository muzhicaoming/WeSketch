using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace WeSketch.Tests
{

    /// <summary>
    /// Testing login - authenticate user
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class LoginTests
    {
        /// <summary>
        /// Method to test authenticating a valid user with test user name and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        [Test]
        [TestCase("", "")]
        [TestCase("", "somepassword")]
        [TestCase("someusername", "")]
        public void IsInvalidAuthenticate_User(string userName, string password)
        {
            Login lg = new Login();
            lg.Dispatcher.Invoke(() =>
            {
                var ex = Assert.Catch<Exception>(() => lg.AuthenticateUser(userName, password));
                StringAssert.Contains("Error", ex.Message);
            });
        }
    }
}

using System;
using NUnit.Framework;
using WeSketchAPI;

namespace WeSketchAPI.Tests
{
    [TestFixture]
    public class WeSketchSecurityTests
    {
        /// <summary>
        /// Method to test authenticating an invalid user with test user name and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        [Test]
        [TestCase("", "")]
        [TestCase("", "somepassword")]
        [TestCase("someusername", "")]
        [TestCase("someusername", "somepassword")]
        public void IsInvalidLogin_Login_ReturnsError(string username, string password)
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            var ex = Assert.Catch<Exception>(() => Sec.Login(username, password));
            StringAssert.Contains("Invalid credentials", ex.Message);
        }

        /// <summary>
        /// Method to test registering a valid user with test user name and password.
        /// </summary>
        [Test]
        public void CreateValidUser_CreateUser_ReturnsUser()
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            User usr = Sec.CreateUser("testerSec", "TestPasswordSec", "testerSec@test.com");
            Assert.IsNotNull(usr);
        }

        /// <summary>
        /// Method to test authenticating a valid user with test user name and password.
        /// </summary>
        [Test]
        public void ValidLogin_Login_ReturnsUser()
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            User usr = Sec.Login("testerSec", "TestPasswordSec");
            Assert.IsNotNull(usr);
        }

        /// <summary>
        /// Method to test registering an invalid user with test user name, email and password.
        /// </summary>
        [Test]
        public void CreateExistingUser_CreateUser_ReturnsAlreadyExistsError()
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            var ex = Assert.Catch<Exception>(() => Sec.CreateUser("testerExists", "TestPasswordExists", "testerExists@test.com"));
            StringAssert.Contains("already exists.", ex.Message);
        }

        /// <summary>
        /// Method to test registering an invalid user with empty strings.
        /// </summary>
        public void CreateUserIncomplete_Login_ReturnsOutOfRange()
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            var ex = Assert.Catch<ArgumentOutOfRangeException>(() => Sec.CreateUser("", "", ""));
            StringAssert.Contains("System.ArgumentOutOfRangeException", ex.Message);
        }
    }
}

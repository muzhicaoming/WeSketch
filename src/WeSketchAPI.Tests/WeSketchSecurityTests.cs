using System;
using NUnit.Framework;
using WeSketchAPI;

namespace WeSketchAPI.Tests
{
    [TestFixture]
    public class WeSketchSecurityTests
    {
        [Test]
        public void CreateUser_Login_ReturnsUser()
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            User usr = Sec.CreateUser("tester", "TestPassword", "tester@test.com");
            Assert.IsNotNull(usr);
        }

        [Test]
        public void CreateExistingUser_Login_ReturnsError()
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            var ex = Assert.Catch<Exception>(() => Sec.CreateUser("tester", "TestPassword", "tester@test.com"));
            StringAssert.Contains("already exists.", ex.Message);
        }

        [Test]
        [TestCase("", "", "")]
        [TestCase("", "somepassword", "someemail")]
        [TestCase("", "", "someemail")]
        [TestCase("", "somepassword", "")]
        [TestCase("someusername", "", "")]
        [TestCase("someusername", "somepassword", "")]
        [TestCase("someusername", "", "someemail")]
        public void CreateUserIncomplete_Login_ReturnsError(string username, string password, string email)
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            User usr = Sec.CreateUser(username, password, email);
            Assert.IsNull(usr);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("", "somepassword")]
        [TestCase("someusername", "")]
        public void IsInvalidLogin_Login_ReturnsError(string username, string password)
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            var ex = Assert.Catch<Exception>(() => Sec.Login(username, password));
            StringAssert.Contains("Invalid credentials", ex.Message);
        }
    }
}

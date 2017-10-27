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
        public void IsInvalidLogin_Login_ReturnsError()
        {
            WeSketchSecurity Sec = new WeSketchSecurity();
            var ex = Assert.Catch<Exception>(() => Sec.Login("", ""));
            StringAssert.Contains("Invalid credentials", ex.Message);
        }
    }
}

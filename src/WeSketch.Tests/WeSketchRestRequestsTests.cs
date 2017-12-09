using System;
using NUnit.Framework;
using WeSketch;
using WeSketchSharedDataModels;
using System.Threading.Tasks;

namespace WeSketch.Tests
{
    [TestFixture]
    public class WeSketchRestRequestsTests
    {

        /// <summary>
        /// Method to test authenticating a valid user with test user name and password.
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// </summary>
        [Test]
        [TestCase("", "")]
        [TestCase("", "somepassword")]
        [TestCase("someusername", "")]
        public void IsInvalidLogin_RestLogin_ReturnsError(string username, string password)
        {
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            Assert.That(async () => await _rest.Login(username, password), Throws.Exception.TypeOf<Exception>().And.Message.Contains("Error"));
        }

        /// <summary>
        /// Method to test registering a valid user with test user name, email address and password.
        /// </summary>
        [Test]
        public async Task CreateUser_RestCreateUser_ReturnsUser()
        {
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            User usr = await _rest.CreateUser("tester", "TestPassword", "tester@test.com");
            Assert.IsNotNull(usr);
        }

        /// <summary>
        /// Method to test authenticating a valid user with test user name and password.
        /// </summary>
        [Test]
        public async Task IsValidLogin_RestLogin_ReturnsUser()
        {
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            User usr = await _rest.Login("tester", "TestPassword");
            Assert.IsNotNull(usr);
        }

        /// <summary>
        /// Method to test registering an invalid user with test user name and password.
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email address">The email address.</param>
        /// </summary>
        [Test]
        [TestCase("", "", "")]
        [TestCase("", "somepassword", "someemail")]
        [TestCase("", "", "someemail")]
        [TestCase("", "somepassword", "")]
        [TestCase("someusername", "", "")]
        [TestCase("someusername", "somepassword", "")]
        [TestCase("someusername", "", "someemail")]
        public void CreateUserIncomplete_RestCreateUser_ReturnsError(string username, string password, string email)
        {
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            Assert.That(async () => await _rest.CreateUser(username, password, email), Throws.Exception.TypeOf<Exception>().And.Message.Contains("Error"));
        }

        /// <summary>
        /// Method to test invite an invalid user
        /// </summary>
        [Test]
        public void InviteUserIncompleteData_RestInviteUserToBoard_ReturnsError()
        {
            Guid boardGuid = Guid.NewGuid();
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            var ex = Assert.Catch<Exception>(() => _rest.InviteUserToBoard("", "", boardGuid));
            StringAssert.Contains("Error", ex.Message);
        }
    }
}

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
        

        [Test]
        [TestCase("", "")]
        [TestCase("", "somepassword")]
        [TestCase("someusername", "")]
        public void IsInvalidLogin_RestLogin_ReturnsError(string username, string password)
        {
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            //var ex = Assert.Catch<Exception>(async() => await _rest.Login(username, password));
            //StringAssert.Contains("Error", ex.Message);
            Assert.That(async () => await _rest.Login(username, password), Throws.Exception.TypeOf<Exception>().And.Message.Contains("Error"));
        }

        [Test]
        public async Task CreateUser_RestCreateUser_ReturnsUser()
        {
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            User usr = await _rest.CreateUser("tester", "TestPassword", "tester@test.com");
            Assert.IsNotNull(usr);
        }

        [Test]
        public async Task IsValidLogin_RestLogin_ReturnsUser()
        {
            WeSketchRestRequests _rest = new WeSketchRestRequests();
            User usr = await _rest.Login("tester", "TestPassword");
            Assert.IsNotNull(usr);
        }

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
            //var ex = Assert.Catch<Exception>(async () => await _rest.CreateUser(username, password, email));
            //StringAssert.Contains("Error", ex.Message);
            Assert.That(async () => await _rest.CreateUser(username, password, email), Throws.Exception.TypeOf<Exception>().And.Message.Contains("Error"));
        }
    }
}

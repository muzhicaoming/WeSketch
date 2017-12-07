using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace WeSketchAPI.Tests
{
    [TestFixture]
    public class WeSketchSignalRHubTests
    {
        private WeSketchSignalRHub _hub;

        [TestMethod]
        public void TestMethod1()
        {
        }

        [SetUp]
        public void SetUpTests()
        {
            _hub = new WeSketchSignalRHub();
        }

        [Fact]
        public void HubsAreMockableViaDynamic()
        {
            bool sendCalled = false;
            var hub = new WeSketchSignalRHub();
            var mockClients = new Mock<I<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.broadcastMessage = new Action<string, string>((name, message) => {
                sendCalled = true;
            });
            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            hub.Send("TestUser", "TestMessage");
            Assert.True(sendCalled);
        }
    }
}

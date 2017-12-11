using System;
using WeSketchAPI;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using System.Dynamic;
using NUnit.Framework;

namespace WeSketchAPI.Tests
{
    [TestFixture]
    public class WeSketchSignalRHubTests
    {
        private WeSketchSignalRHub _hub;

        [SetUp]
        public void SetUpTests()
        {
            _hub = new WeSketchSignalRHub();
        }

        [Test]
        public void HubsAreMockableViaDynamic()
        {
            Guid boardGuid = new Guid("f990354c-f904-420c-a297-2cbb96d25a17");
            bool sendCalled = false;
            var hub = new WeSketchSignalRHub();
            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.broadcastMessage = new Action<string, string>((name, message) => {
                sendCalled = true;
            });
            mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            hub.SendInvitation("TesterExists", boardGuid);
            Assert.True(sendCalled);
        }
    }
}

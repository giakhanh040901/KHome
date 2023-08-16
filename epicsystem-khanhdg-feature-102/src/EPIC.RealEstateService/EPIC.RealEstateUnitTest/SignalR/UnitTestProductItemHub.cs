using EPIC.DataAccess.Base;
using EPIC.RealEstateDomain.SignalR.Hubs;
using EPIC.UnitTestsBase;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Dynamic;
using Xunit;

namespace EPIC.RealEstateUnitTest.SignalR
{
    public class UnitTestProductItemHub
    {
        [Fact]
        public void TestCount()
        {
            //IHost host = UnitTestBase.GetHost<EPIC.RealEstateAPI.Startup>();
            //var dbContext = host.GetService<EpicSchemaDbContext>();

            //bool sendCalled = false;
            //var hub = new ProductItemHub();
            //var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            //hub.Clients = mockClients.Object;
            //dynamic all = new ExpandoObject();
            //all.broadcastMessage = new Action<string, string>((name, message) => {
            //    sendCalled = true;
            //});
            //mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
            //hub.Send("TestUser", "TestMessage");
            //Assert.True(sendCalled);
        }
    }
}

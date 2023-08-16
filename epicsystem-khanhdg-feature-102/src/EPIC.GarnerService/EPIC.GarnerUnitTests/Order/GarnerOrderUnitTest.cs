using EPIC.DataAccess.Base;
using EPIC.GarnerRepositories;
using EPIC.UnitTestsBase;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace EPIC.GarnerUnitTests.Order
{
    public class GarnerOrderUnitTest : UnitTestBase
    {
        private void SeedData(EpicSchemaDbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        //[Fact]
        private void AddOrder()
        {
            IHost host = GetHost<EPIC.GarnerAPI.Startup>();
            var dbContext = host.GetService<EpicSchemaDbContext>();
        }
    }
}

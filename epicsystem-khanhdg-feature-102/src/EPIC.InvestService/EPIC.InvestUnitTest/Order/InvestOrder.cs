using EPIC.DataAccess.Base;
using EPIC.InvestAPI.Controllers;
using EPIC.InvestDomain.Interfaces;
using EPIC.UnitTestsBase;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Hangfire;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EPIC.InvestUnitTest.Order
{
    public class InvestOrder : UnitTestBase
    {
        private void SeedData(EpicSchemaDbContext dbContext)
        {
            dbContext.InvestProjects.Add(new InvestEntities.DataEntities.Project
            {
                Id = 1,
                GeneralContractorId = 1,
                HasTotalInvestmentSub = YesNo.NO,
                InvCode = "TEST1",
                InvName = "NAME1"
            });
            dbContext.SaveChanges();
        }

        [Theory]
        [InlineData(1808, 181)]
        public void Distribution(int tradingUserId, int tradingProviderId)
        {
            IHost mockRequestTrading = GetHost<InvestAPI.Startup>(CreateHttpContextTrading(tradingUserId, tradingProviderId));
            Task.Run(() => mockRequestTrading.Run());

            DistributionController controller = new(mockRequestTrading.GetService<ILogger<DistributionController>>(),
                mockRequestTrading.GetService<IDistributionServices>(), mockRequestTrading.GetService<IConfigContractCodeServices>());
            //controller.FindAllDistribution(1, 10, null, null, true, "N");
            var distributionFindById = controller.FindById(982);
            if (distributionFindById.Status != StatusCode.Success)
            {
                Assert.Fail(distributionFindById.Message);
            }
        }
    }
}

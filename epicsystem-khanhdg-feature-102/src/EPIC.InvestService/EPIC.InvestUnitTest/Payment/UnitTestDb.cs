using EPIC.DataAccess.Base;
using EPIC.UnitTestsBase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Xunit;

namespace EPIC.InvestUnitTest.Payment
{
    public class UnitTestDb : UnitTestBase
    {
        [Fact]
        public void MapTableDb()
        {
            IHost host = GetHost<EPIC.InvestAPI.Startup>();
            var dbContext = host.GetService<EpicSchemaDbContext>();

            var test = dbContext.InvOrders
                .Include(o => o.TradingProvider)
                .Include(o => o.CifCodes).ThenInclude(o => o.Investor)
                .Include(o => o.CifCodes).ThenInclude(o => o.BusinessCustomer)
                .Include(o => o.Department)
                .Include(o => o.Project)
                .Include(o => o.Distribution)
                .Include(o => o.Policy)
                .Include(o => o.PolicyDetail)
                .Include(o => o.BusinessCustomerBankAcc)
                .Include(o => o.InvestorIdentification)
                .Include(o => o.InvestorContactAddress)
                .Where(o => o.DepartmentId != null && o.InvestorBankAccId != null
                    && o.InvestorIdenId != null && o.ContractAddressId != null)
                .AsSplitQuery()
                .AsNoTracking() //no tracking
                .ToList();

            var test2 = dbContext.CifCodes
                .Include(c => c.Investor)
                .Include(c => c.BusinessCustomer)
                .ToList();

            var test3 = dbContext.Investors
                .Include(o => o.InvestorIdentifications)
                .Include(o => o.InvestorIdTemps)
                .Include(o => o.InvestorBankAccounts)
                .Include(o => o.InvestorBankAccountTemps)
                .Include(o => o.InvestorContactAddresses)
                .Include(o => o.InvestorContactAddressTemps)
                .ToList();

            var test4 = dbContext.BusinessCustomers
                .Include(b => b.BusinessCustomerBanks)
                .ToList();

            var test5 = dbContext.Sales
                .Include(s => s.Investor)
                .Include(s => s.BusinessCustomer)
                .AsNoTracking()
                .ToList();
        }
    }
}

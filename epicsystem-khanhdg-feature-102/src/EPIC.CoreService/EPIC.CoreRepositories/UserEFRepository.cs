using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Entities.Dto.User;
using EPIC.IdentityEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class UserEFRepository : BaseEFRepository<Users>
    {
        public UserEFRepository(DbContext dbContext, ILogger logger, string seqName = null) : base(dbContext, logger, Users.SEQ)
        {
        }
        public IQueryable<UserIsInvestorDto> GetInvestorAccount(FindBondInvestorAccountDto input)
        {
            var saleQuerry = from departmentSale in _epicSchemaDbContext.DepartmentSales
                             join sale in _epicSchemaDbContext.Sales on departmentSale.SaleId equals sale.SaleId
                             join saleTrading in _epicSchemaDbContext.SaleTradingProviders on departmentSale.SaleId equals saleTrading.SaleId
                             where departmentSale.TradingProviderId == saleTrading.TradingProviderId
                             && departmentSale.Deleted == YesNo.NO && (input.TradingProviderId == null || departmentSale.TradingProviderId == input.TradingProviderId)
                             && (saleTrading.Status == Status.ACTIVE)
                             && sale.Deleted == YesNo.NO
                             && saleTrading.Deleted == YesNo.NO
                             select sale;

            var listInvestorTradingId = from investorTrading in _epicSchemaDbContext.InvestorTradingProviders
                                        where investorTrading.TradingProviderId == input.TradingProviderId
                                        && investorTrading.Deleted == YesNo.NO
                                        select investorTrading.InvestorId;

            var listInvestorSaleId = from investorSale in _epicSchemaDbContext.InvestorSales
                                     join sale in _epicSchemaDbContext.SaleTradingProviders on investorSale.SaleId equals sale.SaleId
                                     where sale.Deleted == YesNo.NO && investorSale.Deleted == YesNo.NO
                                     && sale.TradingProviderId == input.TradingProviderId
                                     select investorSale.InvestorId;

            var investorParterIds = from investorTrading in _epicSchemaDbContext.InvestorTradingProviders
                                    join tradingPartner in _epicSchemaDbContext.TradingProviderPartners on investorTrading.TradingProviderId equals tradingPartner.TradingProviderId
                                    where investorTrading.Deleted == YesNo.NO && tradingPartner.Deleted == YesNo.NO
                                    && tradingPartner.PartnerId == input.PartnerId
                                    select investorTrading.InvestorId;


            var query = from user in _epicSchemaDbContext.Users
                        join investor in _epicSchemaDbContext.Investors on user.InvestorId equals investor.InvestorId
                        join cifCode in _epicSchemaDbContext.CifCodes on investor.InvestorId equals cifCode.InvestorId
                        from identification in _epicSchemaDbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                        .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()

                        where user.IsDeleted == YesNo.NO && user.UserType == CustomerTypes.INVESTOR
                        && (user.Status == UserStatus.ACTIVE || user.Status == UserStatus.DEACTIVE || user.Status == UserStatus.LOCKED)
                        && (input.Keyword == null || (investor.Phone.ToLower().Contains(input.Phone.ToLower())) || (user.UserName.ToLower().Contains(input.Keyword.ToLower())) || (cifCode.CifCode.ToLower().Contains(input.Keyword)))
                        && (input.Status == null || user.Status == input.Status)
                        && (input.CifCode == null || cifCode.CifCode.ToLower().Contains(input.CifCode.ToLower()))
                        && (input.Phone == null || investor.Phone.ToLower().Contains(input.Phone.ToLower()))
                        && (input.Fullname == null || identification.Fullname.ToLower().Contains(input.Fullname.ToLower()))
                        && (input.Email == null || investor.Email.ToLower().Contains(input.Email.ToLower()))
                        && (input.Sex == null || identification.Sex == input.Sex)
                        && (input.StartAge == null || (int)Math.Floor((DateTime.Now - identification.DateOfBirth).Value.TotalDays / 365.25) >= input.StartAge)
                        && (input.EndAge == null || (int)Math.Floor((DateTime.Now - identification.DateOfBirth).Value.TotalDays / 365.25) <= input.EndAge)
                        && (input.CustomerType == null
                            || (input.CustomerType == UserCustomerTypes.TAI_KHOAN_CHUA_XAC_MINH && (investor.Step == InvestorAppStep.BAT_DAU || investor.Step == InvestorAppStep.DA_DANG_KY))
                            || (input.CustomerType == UserCustomerTypes.SALE && saleQuerry.FirstOrDefault(e => e.Investor.InvestorId == investor.InvestorId) != null))
                        && (input.TradingProviderId == null || listInvestorTradingId.Contains(investor.InvestorId) || listInvestorSaleId.Contains(investor.InvestorId))
                        && (input.PartnerId == null || investorParterIds.Contains(investor.InvestorId))
                        select new UserIsInvestorDto
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            DisplayName = user.DisplayName,
                            Sex = identification.Sex,
                            Status = user.Status,
                            Email = investor.Email,
                            Name = identification.Fullname,
                            Phone = investor.Phone,
                            InvestorId = investor.InvestorId,
                            CifCode = cifCode.CifCode,
                            IdNo = identification.IdNo,
                            Source = investor.Source,
                            ReferralCodeSelf = investor.ReferralCodeSelf
                        };
           
            return query;
        }
    }
}

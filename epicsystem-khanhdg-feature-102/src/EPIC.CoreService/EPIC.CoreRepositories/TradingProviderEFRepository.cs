using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreRepositories
{
    public class TradingProviderEFRepository : BaseEFRepository<TradingProvider>
    {
        private readonly EpicSchemaDbContext _epicDbContext;
        public TradingProviderEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{TradingProvider.SEQ}")
        {
            _epicDbContext = dbContext;
        }

        public TradingProvider FindById(int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: tradingProviderId: {tradingProviderId}");
            return _dbSet.FirstOrDefault(t => t.TradingProviderId == tradingProviderId && t.Deleted == YesNo.NO);
        }

        public TradingProvider FindByTaxCode(int partnerId, string taxCode)
        {
            var result = from td in _dbSet
                         join bs in _epicSchemaDbContext.BusinessCustomers on td.BusinessCustomerId equals bs.BusinessCustomerId
                         join cc in _epicSchemaDbContext.TradingProviderPartners on td.TradingProviderId equals cc.TradingProviderId
                         where cc.PartnerId == partnerId && td.Deleted == YesNo.NO && cc.Deleted == YesNo.NO && bs.Deleted == YesNo.NO
                         && bs.TaxCode == taxCode
                         select td;
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Lấy thông tin đại lý
        /// </summary>
        public TradingProviderDto GetById(int tradingProviderId)
        {
            return (from tradingProvider in _dbSet
                    join business in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals business.BusinessCustomerId
                    where tradingProvider.Deleted == YesNo.NO && business.Deleted == YesNo.NO
                    && tradingProvider.TradingProviderId == tradingProviderId
                    select new TradingProviderDto
                    {
                        Name = business.Name,
                        AliasName = tradingProvider.AliasName,
                        Code = business.Code,
                        Email = business.Email,
                        BusinessCustomerId = business.BusinessCustomerId,
                        Phone = business.Phone,
                        TaxCode = business.TaxCode,
                        RepName = business.RepName,
                        ShortName = business.ShortName,
                        TradingProviderId = tradingProvider.TradingProviderId
                    }).FirstOrDefault();
        }


        /// <summary>
        /// Lấy danh sách đại lý theo đối tác
        /// </summary>
        public IQueryable<TradingProvider> GetAllByPartnerId(int partnerId)
        {
            var result = from trading in _dbSet
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers on trading.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                         join tradingParner in _epicSchemaDbContext.TradingProviderPartners on trading.TradingProviderId equals tradingParner.TradingProviderId
                         where tradingParner.PartnerId == partnerId && trading.Deleted == YesNo.NO && tradingParner.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                         select trading;
            return result;
        }

        /// <summary>
        /// Lấy đại lý mặc định
        /// </summary>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public TradingProvider FindByDefault(int isDefault)
        {
            var result = _dbSet.FirstOrDefault(t => t.Deleted == YesNo.NO && (isDefault == TradingProviderDefault.BOND && t.IsDefaultBond == YesNo.YES)
            && (isDefault == TradingProviderDefault.BOND && t.IsDefaultBond == YesNo.YES)
            && (isDefault == TradingProviderDefault.INVEST && t.IsDefaultInvest == YesNo.YES)
            && (isDefault == TradingProviderDefault.COMPANY_SHARES && t.IsDefaultCps == YesNo.YES)
            && (isDefault == TradingProviderDefault.GARNER && t.IsDefaultGarner == YesNo.YES));
            return result;
        }

        /// <summary>
        /// get data đại lý sơ cấp cho hợp đồng
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="BusinessCustomerBankAccId">id tài khoản thụ hưởng của đại lý trong bán theo kỳ hạn</param>
        public TradingProviderDataForContractDto GetTradingProviderForContract(int tradingProviderId, int businessCustomerBankAccId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: tradingProviderId: {tradingProviderId}");
            TradingProviderDataForContractDto tradingProviderDataForContractDto = new();
            tradingProviderDataForContractDto.TradingProvider = _dbSet.FirstOrDefault(t => t.TradingProviderId == tradingProviderId && t.Deleted == YesNo.NO);
            if (tradingProviderDataForContractDto.TradingProvider != null)
            {
                tradingProviderDataForContractDto.BusinessCustomerTrading = (from bs in _epicSchemaDbContext.BusinessCustomers
                                                                             join cc in _epicSchemaDbContext.CifCodes on bs.BusinessCustomerId equals cc.BusinessCustomerId
                                                                             where bs.BusinessCustomerId == tradingProviderDataForContractDto.TradingProvider.BusinessCustomerId && bs.Deleted == YesNo.NO
                                                                             select new BusinessCustomerDto
                                                                             {
                                                                                 BusinessCustomerId = bs.BusinessCustomerId,
                                                                                 Name = bs.Name,
                                                                                 Code = bs.Code,
                                                                                 ShortName = bs.ShortName,
                                                                                 Address = bs.Address,
                                                                                 TradingAddress = bs.TradingAddress,
                                                                                 Nation = bs.Nation,
                                                                                 Phone = bs.Phone,
                                                                                 Mobile = bs.Mobile,
                                                                                 Email = bs.Email,
                                                                                 TaxCode = bs.TaxCode,
                                                                                 LicenseDate = bs.LicenseDate,
                                                                                 LicenseIssuer = bs.LicenseIssuer,
                                                                                 Capital = bs.Capital,
                                                                                 RepName = bs.RepName,
                                                                                 RepPosition = bs.RepPosition,
                                                                                 DecisionNo = bs.DecisionNo,
                                                                                 DecisionDate = bs.DecisionDate,
                                                                                 NumberModified = bs.NumberModified,
                                                                                 DateModified = bs.DateModified,
                                                                                 Status = bs.Status,
                                                                                 IsCheck = bs.IsCheck,
                                                                                 Website = bs.Website,
                                                                                 Fanpage = bs.Fanpage,
                                                                                 BusinessRegistrationImg = bs.BusinessRegistrationImg,
                                                                                 Secret = bs.Secret,
                                                                                 AvatarImageUrl = bs.AvatarImageUrl,
                                                                                 StampImageUrl = bs.StampImageUrl,
                                                                                 ReferralCodeSelf = bs.ReferralCodeSelf,
                                                                                 AllowDuplicate = bs.AllowDuplicate,
                                                                                 CifCode = cc.CifCode,
                                                                             }).FirstOrDefault();
                tradingProviderDataForContractDto.TradingBank = (from businessBank in _epicSchemaDbContext.BusinessCustomerBanks
                                                                 join bank in _epicSchemaDbContext.CoreBanks on businessBank.BankId equals bank.BankId
                                                                 where businessBank.BusinessCustomerBankAccId == businessCustomerBankAccId && businessBank.Deleted == YesNo.NO
                                                                 select new BusinessCustomerBankDto
                                                                 {
                                                                     BusinessCustomerBankAccId = businessCustomerBankAccId,
                                                                     BusinessCustomerId = businessBank.BusinessCustomerId,
                                                                     BankAccName = businessBank.BankAccName,
                                                                     BankAccNo = businessBank.BankAccNo,
                                                                     BankCode = bank.BankCode,
                                                                     Logo = bank.Logo,
                                                                     BankId = businessBank.BankId,
                                                                     BankName = bank.BankName,
                                                                     FullBankName = bank.FullBankName,
                                                                     Status = businessBank.Status,
                                                                     IsDefault = businessBank.IsDefault
                                                                 }).FirstOrDefault();
            }

            return tradingProviderDataForContractDto;
        }
        public List<BusinessCustomerBankDto> FindBankByTradingInvest(int tradingProviderId)
        {
            var result = new List<BusinessCustomerBankDto>();
            var tradingBank = from businessCustomerBank in _epicSchemaDbContext.BusinessCustomerBanks
                              join tradingProvider in _epicDbContext.TradingProviders on businessCustomerBank.BusinessCustomerId equals tradingProvider.BusinessCustomerId
                              join coreBank in _epicDbContext.CoreBanks on businessCustomerBank.BankId equals coreBank.BankId into banks
                              from bank in banks.DefaultIfEmpty()
                              where tradingProvider.TradingProviderId == tradingProviderId && businessCustomerBank.Deleted == YesNo.NO && tradingProvider.Deleted == YesNo.NO
                              select new 
                              {
                                  businessCustomerBank,
                                  tradingProvider,
                                  bank
                              };
            foreach (var item in tradingBank)
            {
                var resultItem = new BusinessCustomerBankDto
                {
                    BusinessCustomerBankAccId = item.businessCustomerBank.BusinessCustomerBankAccId,
                    BusinessCustomerId = item.businessCustomerBank.BusinessCustomerId,
                    BankAccName = item.businessCustomerBank.BankAccName,
                    BankAccNo = item.businessCustomerBank.BankAccNo,
                    BankCode = item.bank.BankCode,
                    Logo = item.bank.Logo,
                    BankId = item.bank.BankId,
                    BankName = item.bank.BankName,
                    FullBankName = item.bank.FullBankName,
                    Status = item.businessCustomerBank.Status,
                    IsDefault = item.businessCustomerBank.IsDefault
                };
                result.Add(resultItem);
            }
            return result;
        }
        
        public TradingProvider ChangeStatus(int tradingProviderId, int status)
        {
            var tradingProvider = _dbSet.FirstOrDefault(o => o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO);
            if (tradingProvider != null)
            {
                tradingProvider.Status = status;
            }
            return tradingProvider;
        }
    }
}

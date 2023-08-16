using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreEntities.Dto.BusinessCustomer;
using EPIC.CoreSharedEntities.Dto.BusinessCustomer;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Utils;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace EPIC.CoreRepositories
{
    public class BusinessCustomerEFRepository : BaseEFRepository<BusinessCustomer>
    {
        public BusinessCustomerEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, TradingProvider.SEQ)
        {
        }

        public void Update(BusinessCustomer input)
        {
            _logger.LogInformation($"{nameof(BusinessCustomerEFRepository)}->{nameof(Update)}: input: {JsonSerializer.Serialize(input)}");
            var businessCustomerQuery = _dbSet.FirstOrDefault(b => b.BusinessCustomerId == input.BusinessCustomerId && b.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.CoreBusinessCustomerNotFound);

            businessCustomerQuery.Name = input.Name;
            businessCustomerQuery.Code = input.Code;
            businessCustomerQuery.ShortName = input.ShortName;
            businessCustomerQuery.Address = input.Address;
            businessCustomerQuery.TradingAddress = input.TradingAddress;
            businessCustomerQuery.Nation = input.Nation;
            businessCustomerQuery.Phone = input.Phone;
            businessCustomerQuery.Mobile = input.Mobile;
            businessCustomerQuery.Email = input.Email;
            businessCustomerQuery.TaxCode = input.TaxCode;
            businessCustomerQuery.LicenseDate = input.LicenseDate;
            businessCustomerQuery.LicenseIssuer = input.LicenseIssuer;
            businessCustomerQuery.Capital = input.Capital;
            businessCustomerQuery.RepName = input.RepName;
            businessCustomerQuery.RepPosition = input.RepPosition;
            businessCustomerQuery.DecisionNo = input.DecisionNo;
            businessCustomerQuery.DecisionDate = input.DecisionDate;
            businessCustomerQuery.NumberModified = input.NumberModified;
            businessCustomerQuery.DateModified = input.DateModified;
            businessCustomerQuery.Website = input.Website;
            businessCustomerQuery.Fanpage = input.Fanpage;
            businessCustomerQuery.BusinessRegistrationImg = input.BusinessRegistrationImg;
            businessCustomerQuery.Secret = input.Secret;
            businessCustomerQuery.AvatarImageUrl = input.AvatarImageUrl;
            businessCustomerQuery.StampImageUrl = input.StampImageUrl;
        }

        /// <summary>
        /// Tìm kiếm thông tin doanh nghiệp
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <returns></returns>
        public BusinessCustomerDto FindById(int businessCustomerId)
        {
            _logger.LogInformation($"{nameof(BusinessCustomerEFRepository)}->{nameof(FindById)}: businessCustomerId: {businessCustomerId}");

            var result = from bs in _dbSet
                         join cc in _epicSchemaDbContext.CifCodes on bs.BusinessCustomerId equals cc.BusinessCustomerId
                         where bs.BusinessCustomerId == businessCustomerId && bs.Deleted == YesNo.NO
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
                         };
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Xem thông tin của ngân hàng
        /// </summary>
        /// <param name="businessCustomerBankId"></param>
        /// <returns></returns>
        public BusinessCustomerBankDto FindBankById(int businessCustomerBankId)
        {
            return (from businessBank in _epicSchemaDbContext.BusinessCustomerBanks
                    join bank in _epicSchemaDbContext.CoreBanks on businessBank.BankId equals bank.BankId
                    where businessBank.BusinessCustomerBankAccId == businessCustomerBankId && businessBank.Deleted == YesNo.NO
                    select new BusinessCustomerBankDto
                    {
                        BusinessCustomerBankAccId = businessCustomerBankId,
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


        /// <summary>
        /// Lấy phân trang danh sách khách hàng doanh nghiệp tạm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<BusinessCustomerTemp> FindAllTemp(FilterBusinessCustomerTempDto input, int? tradingProviderId, int? partnerId)
        {
            var result = new PagingResult<BusinessCustomerTemp>();

            var businessCustomerTempQuery = _epicSchemaDbContext.BusinessCustomerTemps
                .Where(bt =>
                    bt.Deleted == YesNo.NO &&
                    bt.Status != BusinessCustomerStatus.HUY_DUYET &&
                    (input.Keyword == null || bt.TaxCode.ToLower().Contains(input.Keyword.ToLower())) &&
                    (input.Status == null || bt.Status == input.Status) &&
                    (input.Name == null || bt.Name.ToLower().Contains(input.Name.ToLower()) || bt.ShortName.ToLower().Contains(input.Name.ToLower())) &&
                    (input.Phone == null || bt.Phone.ToLower().Contains(input.Phone.ToLower())) &&
                    (input.Email == null || bt.Email.ToLower().Contains(input.Email.ToLower())) &&
                    (tradingProviderId == null || bt.TradingProviderId == tradingProviderId) &&
                    (partnerId == null || bt.PartnerId == partnerId))
                .AsQueryable();

            result.TotalItems = businessCustomerTempQuery.Count();
            businessCustomerTempQuery = businessCustomerTempQuery.OrderDynamic(input.Sort);

            if (input.PageSize != -1) businessCustomerTempQuery = businessCustomerTempQuery.Skip(input.Skip).Take(input.PageSize);

            result.Items = businessCustomerTempQuery.AsEnumerable();
            return result;
        }

        /// <summary>
        /// Lấy phân trang danh sách khách hàng doanh nghiệp thật
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public IQueryable<BusinessCustomerDto> GetAll(FilterBusinessCustomerDto input, int? tradingProviderId, int? partnerId)
        {
            var result = from businessCustomer in _epicSchemaDbContext.BusinessCustomers.AsNoTracking()
                         from businessCustomerTradings in _epicSchemaDbContext.BusinessCustomerTradings.Where(bt => businessCustomer.BusinessCustomerId == bt.BusinessCustomerId && bt.Deleted == YesNo.NO).DefaultIfEmpty()
                         from businessCustomerParters in _epicSchemaDbContext.BusinessCustomerPartners.Where(bp => businessCustomer.BusinessCustomerId == bp.BusinessCustomerId && bp.Deleted == YesNo.NO).DefaultIfEmpty()
                         where businessCustomer.Deleted == YesNo.NO
                               && (input.Name == null || businessCustomer.Name.ToLower().Contains(input.Name.ToLower())
                                   || businessCustomer.ShortName.ToLower().Contains(input.Name.ToLower()))
                               && (input.Keyword == null || businessCustomer.TaxCode.Contains(input.Keyword))
                               && (input.Email == null || businessCustomer.Email.ToLower().Contains(input.Email.ToLower()))
                               && (input.Phone == null || businessCustomer.Phone.ToLower().Contains(input.Phone.ToLower()))
                               && (input.IsCheck == null || businessCustomer.IsCheck == input.IsCheck)
                               && (tradingProviderId == null || businessCustomerTradings.TradingProviderId == tradingProviderId)
                               && (partnerId == null || businessCustomerParters.PartnerId == partnerId)
                         select new BusinessCustomerDto
                         {
                             Id = (int)businessCustomer.BusinessCustomerId,
                             BusinessCustomerId = businessCustomer.BusinessCustomerId,
                             Code = businessCustomer.Code,
                             Name = businessCustomer.Name,
                             ShortName = businessCustomer.ShortName,
                             Address = businessCustomer.Address,
                             TradingAddress = businessCustomer.TradingAddress,
                             Nation = businessCustomer.Nation,
                             Phone = businessCustomer.Phone,
                             Mobile = businessCustomer.Mobile,
                             Email = businessCustomer.Email,
                             TaxCode = businessCustomer.TaxCode,
                             LicenseDate = businessCustomer.LicenseDate,
                             LicenseIssuer = businessCustomer.LicenseIssuer,
                             Capital = businessCustomer.Capital,
                             RepName = businessCustomer.RepName,
                             RepPosition = businessCustomer.RepPosition,
                             DecisionDate = businessCustomer.DecisionDate,
                             DecisionNo = businessCustomer.DecisionNo,
                             NumberModified = businessCustomer.NumberModified,
                             StampImageUrl = businessCustomer.StampImageUrl,
                             Status = businessCustomer.Status,
                             DateModified = businessCustomer.DateModified,
                             IsCheck = businessCustomer.IsCheck,
                             Website = businessCustomer.Website,
                             Fanpage = businessCustomer.Fanpage,
                             BusinessRegistrationImg = businessCustomer.BusinessRegistrationImg,
                             Server = businessCustomer.Server,
                             Key = businessCustomer.Key,
                             Secret = businessCustomer.Secret,
                             AvatarImageUrl = businessCustomer.AvatarImageUrl,
                             ReferralCodeSelf = businessCustomer.ReferralCodeSelf,
                             AllowDuplicate = businessCustomer.AllowDuplicate,
                             RepIdNo = businessCustomer.RepIdNo,
                             RepIdDate = businessCustomer.RepIdDate,
                             RepSex = businessCustomer.RepSex,
                             RepAddress = businessCustomer.RepAddress,
                             RepBirthDate = businessCustomer.RepBirthDate,
                             RepIdIssuer = businessCustomer.RepIdIssuer,
                         };
            return result;
        }

        /// <summary>
        /// Lấy khdn theo mã giới thiệu (bất kể trạng thái)
        /// </summary>
        /// <param name="referralCodeSelf"></param>
        /// <returns></returns>
        public BusinessCustomer FindByReferralCodeSelf(string referralCodeSelf)
        {
            return _dbSet.FirstOrDefault(b => b.ReferralCodeSelf == referralCodeSelf && b.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Lấy danh sách ngân hàng theo businessCustomerId
        /// </summary>
        /// <param name="businessCustomerBankId"></param>
        /// <returns></returns>
        public IQueryable<BusinessCustomerBankDto> GetListBankByBusinessCustomerId(int businessCustomerId)
        {
            return (from businessBank in _epicSchemaDbContext.BusinessCustomerBanks
                    join bank in _epicSchemaDbContext.CoreBanks on businessBank.BankId equals bank.BankId
                    where businessBank.BusinessCustomerId == businessCustomerId && businessBank.Deleted == YesNo.NO
                    select new BusinessCustomerBankDto
                    {
                        BusinessCustomerBankAccId = businessBank.BusinessCustomerBankAccId,
                        BusinessCustomerId = businessCustomerId,
                        BankAccName = businessBank.BankAccName,
                        BankAccNo = businessBank.BankAccNo,
                        BankCode = bank.BankCode,
                        Logo = bank.Logo,
                        BankId = businessBank.BankId,
                        BankName = bank.BankName,
                        FullBankName = bank.FullBankName,
                        Status = businessBank.Status,
                        IsDefault = businessBank.IsDefault
                    }).OrderByDescending(b => b.IsDefault).ThenByDescending(x => x.BusinessCustomerBankAccId);
        }

        /// <summary>
        /// Lấy thông tin khách hàng doanh nghiệp cho hợp đồng
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <param name="businessCustomerBankAccId">Id tài khoản thụ hưởng của khách hàng</param>
        /// <returns></returns>
        public BusinessCustomerForContractDto GetBusinessCustomerForContract(int businessCustomerId, int businessCustomerBankAccId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: businessCustomerId: {businessCustomerId}");
            BusinessCustomerForContractDto businessCustomerForContractDto = new()
            {
                BusinessCustomer = (from bs in _dbSet
                                    join cc in _epicSchemaDbContext.CifCodes on bs.BusinessCustomerId equals cc.BusinessCustomerId
                                    where bs.BusinessCustomerId == businessCustomerId && bs.Deleted == YesNo.NO
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
                                        RepIdDate = bs.RepIdDate,
                                        RepIdNo = bs.RepIdNo,
                                        RepIdIssuer = bs.RepIdIssuer,
                                        RepSex = bs.RepSex,
                                        RepBirthDate = bs.RepBirthDate,
                                        RepAddress = bs.RepAddress,
                                    }).FirstOrDefault()
            };
            if (businessCustomerForContractDto.BusinessCustomer != null)
            {
                businessCustomerForContractDto.BusinessCustomerBank = (from businessBank in _epicSchemaDbContext.BusinessCustomerBanks
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
                                                                           IsDefault = businessBank.IsDefault,
                                                                           BankBranchName = businessBank.BankBranchName
                                                                       }).FirstOrDefault();
            }
            return businessCustomerForContractDto;
        }

        /// <summary>
        /// Kiểm tra khdn có thuộc đlsc ko
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public bool IsInTradingProvider(int businessCustomerId, int tradingProviderId)
        {
            bool businessInTrading = _epicSchemaDbContext.TradingProviders.Any(x => x.BusinessCustomerId == businessCustomerId && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO);
            bool saleBusinessInTrading = (from sale in _epicSchemaDbContext.Sales.Where(x => x.BusinessCustomerId == businessCustomerId && x.Deleted == YesNo.NO)
                                          from saleTrading in _epicSchemaDbContext.SaleTradingProviders.Where(x => x.TradingProviderId == tradingProviderId && x.SaleId == sale.SaleId && x.Deleted == YesNo.NO)
                                          select sale).FirstOrDefault() != null;
            return businessInTrading || saleBusinessInTrading;
        }
    }
}
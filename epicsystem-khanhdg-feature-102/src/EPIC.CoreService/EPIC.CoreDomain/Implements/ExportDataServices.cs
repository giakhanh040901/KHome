using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.ExportData;
using EPIC.CoreEntities.Dto.XptToken;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreDomain.Implements
{
    public class ExportDataServices : IExportDataServices
    {
        private readonly ILogger<TradingProviderConfigServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly XptTokenEFRepository _xptTokenEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;

        public ExportDataServices(
            EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<TradingProviderConfigServices> logger,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor;
            _xptTokenEFRepository = new XptTokenEFRepository(_dbContext, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
        }
        #region Token
        public void AddXvtToken(CreateXptTokenDto input)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            var tokenInsertResult = _dbContext.XptTokens.Add(new XptToken
            {
                Id = (int)_xptTokenEFRepository.NextKey(),
                Token = GenerateCodes.RandomXvtToken(),
                ExpiredDate = input.ExpiredDate,
                CreatedDate = DateTime.Now
            }).Entity;

            foreach (var item in input.DataTypes)
            {
                if (!XvtTokenDataTypes.DATA_TYPES.Contains(item))
                {
                    _xptTokenEFRepository.ThrowException(ErrorCode.BadRequest);
                }

                _dbContext.XptTokenDataTypes.Add(new XptTokenDataType
                {
                    Id = (int)_xptTokenEFRepository.NextKey($"{DbSchemas.EPIC}.{XptTokenDataType.SEQ}"),
                    TokenId = tokenInsertResult.Id,
                    DataType = item
                });
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public void UpdateXvtToken(UpdateXptTokenDto input)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            var token = _dbContext.XptTokens.FirstOrDefault(t => t.Id == input.Id)
                .ThrowIfNull(_dbContext, ErrorCode.CoreXvtTokenNotFound);

            token.ExpiredDate = input.ExpiredDate;

            var tokenDataTypes = _dbContext.XptTokenDataTypes.Where(t => t.TokenId == token.Id);

            var tokenRemove = tokenDataTypes.Where(t => !input.DataTypes.Contains(t.DataType));

            _dbContext.XptTokenDataTypes.RemoveRange(tokenRemove);

            foreach (var item in input.DataTypes)
            {
                if (!XvtTokenDataTypes.DATA_TYPES.Contains(item))
                {
                    _xptTokenEFRepository.ThrowException(ErrorCode.BadRequest);
                }
                if (tokenDataTypes.Select(p => p.DataType).Contains(item))
                {
                    _dbContext.XptTokenDataTypes.Add(new XptTokenDataType
                    {
                        Id = (int)_xptTokenEFRepository.NextKey($"{DbSchemas.EPIC}.{XptTokenDataType.SEQ}"),
                        TokenId = token.Id,
                        DataType = item
                    });
                }
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }
        #endregion

        #region ExportData
        public PagingResult<ExportDataInvestorDto> ExportDataInvestor(string token, FilterExportDataDto input)
        {
            //CheckToken(token, XvtTokenDataTypes.EP_INVESTOR);

            var result = from investor in _dbContext.Investors
                         join cifcode in _dbContext.CifCodes on investor.InvestorId equals cifcode.InvestorId
                         from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                                     .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                         let contactAddress = _dbContext.InvestorContactAddresses.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                                     .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.ContactAddressId).FirstOrDefault().ContactAddress
                         let bankAccount = _dbContext.InvestorBankAccounts.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                                     .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).FirstOrDefault()
                         from user in _dbContext.Users.Where(x => x.InvestorId == investor.InvestorId && x.UserType == UserTypes.INVESTOR && x.IsDeleted == YesNo.NO)//.DefaultIfEmpty()
                         where investor.Deleted == YesNo.NO
                         orderby investor.InvestorId descending
                         select new ExportDataInvestorDto
                         {
                             InvestorId = investor.InvestorId,
                             CifCode = cifcode.CifCode,
                             Fullname = identification.Fullname,
                             DateOfBirth = identification.DateOfBirth,
                             Sex = identification.Sex,
                             IdType = identification.IdType,
                             IdNo = identification.IdNo,
                             Nationality = identification.Nationality,
                             PlaceOfOrigin = identification.PlaceOfOrigin,
                             PlaceOfResidence = identification.PlaceOfResidence,
                             IdDate = identification.IdDate,
                             IdExpiredDate = identification.IdExpiredDate,
                             ContactAddress = contactAddress,
                             BankName = _dbContext.CoreBanks.FirstOrDefault(c => c.BankId == bankAccount.BankId).BankName,
                             UserName = user.UserName,
                             ReferralCode = investor.ReferralCode,
                             ReferralCodeSelf = investor.ReferralCodeSelf,
                             UserCreatedDate = user.CreatedDate,
                             UserDeactiveDate = user.DeletedDate,
                             UserCreatedBy = user.CreatedBy,
                             UserStatus = user.Status,

                         };
            return new PagingResult<ExportDataInvestorDto>
            {
                TotalItems = result.Count(),
                Items = result.Skip(input.Skip).Take(input.PageSize)
            };
        }

        public PagingResult<ExportDataSaleDto> ExportDataSale(string token, FilterExportDataDto input)
        {
            //CheckToken(token, XvtTokenDataTypes.EP_CORE_SALE);

            var result = from sale in _dbContext.Sales
                         join investor in _dbContext.Investors.Where(i => i.Deleted == YesNo.NO) on sale.InvestorId equals investor.InvestorId into investors
                         from investor in investors.DefaultIfEmpty()
                         from cifCodeInvestor in _dbContext.CifCodes.Where(c => c.InvestorId == investor.InvestorId && c.Deleted == YesNo.NO).DefaultIfEmpty()
                         join businessCustomer in _dbContext.BusinessCustomers.Where(i => i.Deleted == YesNo.NO) on sale.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         from cifCodeBusiness in _dbContext.CifCodes.Where(c => c.BusinessCustomerId == businessCustomer.BusinessCustomerId && c.Deleted == YesNo.NO).DefaultIfEmpty()
                         where sale.Deleted == YesNo.NO
                         select new ExportDataSaleDto
                         {
                             SaleId = sale.SaleId,
                             CifCode = cifCodeBusiness.CifCode ?? cifCodeInvestor.CifCode,
                             ReferralCodeSelf = (businessCustomer != null) ? businessCustomer.ReferralCodeSelf : investor.ReferralCodeSelf,
                             Fullname = businessCustomer.Name ??
                                        (_dbContext.InvestorIdentifications
                                        .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                                        .OrderByDescending(ii => ii.IsDefault)
                                        .ThenByDescending(ii => ii.Id)
                                        .Select(ii => ii.Fullname)
                                        .FirstOrDefault())
                         };

            return new PagingResult<ExportDataSaleDto>
            {
                TotalItems = result.Count(),
                Items = result.Skip(input.Skip).Take(input.PageSize)
            };
        }

        public PagingResult<ExportDataInvestOrderDto> ExportDataInvestOrder(string token, FilterExportDataDto input)
        {
            //CheckToken(token, XvtTokenDataTypes.EP_INV_ORDER);
            var result = _investOrderEFRepository.ExportDataInvestOrder(input);
            return result;
        }

        private void CheckToken(string token, string dataType)
        {
            var tokenQuery = _dbContext.XptTokens.FirstOrDefault(t => t.Token == token)
                .ThrowIfNull(_dbContext, ErrorCode.CoreXvtTokenNotFound);
            if (tokenQuery.ExpiredDate != null && tokenQuery.ExpiredDate > DateTime.Now)
            {
                _xptTokenEFRepository.ThrowException(ErrorCode.CoreXvtTokenIsExpired);
            }
            if (!_dbContext.XptTokenDataTypes.Any(x => x.TokenId == tokenQuery.Id && x.DataType == dataType))
            {
                _xptTokenEFRepository.ThrowException(ErrorCode.CoreXvtTokenIsNotPermissionExpired);
            }
        }
        #endregion
    }
}

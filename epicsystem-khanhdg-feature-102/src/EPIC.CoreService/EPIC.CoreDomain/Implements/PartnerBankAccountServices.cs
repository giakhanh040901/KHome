using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.PartnerBankAccount;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class PartnerBankAccountServices : IPartnerBankAccountServices
    {
        private readonly ILogger<PartnerBankAccountServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly PartnerBankAccountEFRepository _partnerBankAccountEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly PartnerEFRepository _partnerEFRepository;

        public PartnerBankAccountServices(
            ILogger<PartnerBankAccountServices> logger,
            EpicSchemaDbContext dbContext,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _partnerBankAccountEFRepository = new PartnerBankAccountEFRepository(_dbContext, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _partnerEFRepository = new PartnerEFRepository(_dbContext, _logger);
        }


        public PartnerBankAccount Add(CreatePartnerBankAccountDto input) 
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;
            var result = new PartnerBankAccount();

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if(userType != UserTypes.ROOT_EPIC)
            {
                return result;
            }
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, userType = {userType}");

            var checkPartner = _partnerEFRepository.EntityNoTracking.FirstOrDefault(o => ((partnerId == null) ? o.PartnerId == input.PartnerId : o.PartnerId == partnerId) && o.Deleted == YesNo.NO);
            if (checkPartner == null)
            {
                _partnerBankAccountEFRepository.ThrowException(ErrorCode.CorePartnerNotFound);
            }
            var insert = new PartnerBankAccount();
            insert.PartnerId = partnerId ?? input.PartnerId;
            insert.BankAccName = input.BankAccName;
            insert.BankAccNo = input.BankAccNo;
            insert.BankId = input.BankId;
            insert.CreatedBy = username;
            result = _partnerBankAccountEFRepository.Add(insert);
            _dbContext.SaveChanges();
            return result;
        }

        public PartnerBankAccount Update(UpdatePartnerBankAccountDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;
            var result = new PartnerBankAccount();

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType != UserTypes.ROOT_EPIC)
            {
                return result;
            }
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, userType = {userType}");

            var partnerBankAccount = _partnerBankAccountEFRepository.Entity.FirstOrDefault(o => o.Id == input.Id 
                                    && ((partnerId == null) ? o.PartnerId == input.PartnerId : o.PartnerId == partnerId) 
                                    && o.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.CorePartnerBankAccountNotFound);
            partnerBankAccount.BankAccName = input.BankAccName;
            partnerBankAccount.BankAccNo = input.BankAccNo;
            partnerBankAccount.BankId = input.BankId;
            partnerBankAccount.ModifiedBy = username;
            partnerBankAccount.ModifiedDate = DateTime.Now;
            result = _partnerBankAccountEFRepository.Update(partnerBankAccount);
            _dbContext.SaveChanges();
            return result;
        }

        public PartnerBankAccount FindById(int id, int? partnerId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType != UserTypes.ROOT_EPIC)
            {
                return null;
            }
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(FindById)}: id = {id}, username = {username}, userType = {userType}");
            var partnerBankAccount = _partnerBankAccountEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.CorePartnerBankAccountNotFound);
            partnerBankAccount.Deleted = YesNo.YES;
            return partnerBankAccount;
        }

        public void Delete(int id, int? partnerId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType != UserTypes.ROOT_EPIC)
            {
                return;
            }
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(Delete)}: id = {id}, username = {username}, userType = {userType}");
            var partnerBankAccount = _partnerBankAccountEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.CorePartnerBankAccountNotFound);
            partnerBankAccount.Deleted = YesNo.YES;
            partnerBankAccount.ModifiedDate = DateTime.Now;
            partnerBankAccount.ModifiedBy = username;
            _dbContext.SaveChanges();
        }
        
        public PartnerBankAccount ChangeStatus(int id, int? partnerId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType != UserTypes.ROOT_EPIC)
            {
                return null;
            }
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(ChangeStatus)}: id = {id},username = {username}, userType = {userType}, partnerId = {partnerId}");
            var partnerBankAccount = _partnerBankAccountEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.CorePartnerBankAccountNotFound);
            if (partnerBankAccount.Status == IntegerStatus.ACTIVE)
            {
                partnerBankAccount.Status = IntegerStatus.DEACTIVE;
            }
            else
            {
                partnerBankAccount.Status = IntegerStatus.ACTIVE;
            }
            partnerBankAccount.ModifiedDate = DateTime.Now;
            partnerBankAccount.ModifiedBy = username;
            _dbContext.SaveChanges();
            return partnerBankAccount;
        }

        public PagingResult<PartnerBankAccountDto> FindAll(FilterPartnerBankAccountDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;
            var result = new PagingResult<PartnerBankAccountDto>();

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType != UserTypes.ROOT_EPIC)
            {
                return result;
            }
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, userType = {userType}");

            result = _partnerBankAccountEFRepository.FindAll(new FilterPartnerBankAccountDto
            {
                Keyword = input.Keyword,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                PartnerId = partnerId ?? input.PartnerId
            });

            return result;
        }

        public PartnerBankAccount SetDefault(int id, int? partnerId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType != UserTypes.ROOT_EPIC)
            {
                return null;
            }
            _logger.LogInformation($"{nameof(PartnerBankAccountEFRepository)} -> {nameof(SetDefault)}: id = {id}, username = {username}, userType = {userType}, partnerId = {partnerId}");
            var partnerBankAccount = _partnerBankAccountEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.CorePartnerBankAccountNotFound);
            partnerBankAccount.IsDefault = YesNo.YES;
            partnerBankAccount.ModifiedDate = DateTime.Now;
            partnerBankAccount.ModifiedBy = username;
            var listPartnerBankAccount = _partnerBankAccountEFRepository.Entity.Where(o => o.Id != id && o.Deleted == YesNo.NO && o.PartnerId == partnerId);
            foreach (var item in listPartnerBankAccount)
            {
                item.IsDefault = YesNo.NO;
            }
            _dbContext.SaveChanges();
            return partnerBankAccount;
        }
    }
}

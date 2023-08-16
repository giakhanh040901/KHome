using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class PartnerMsbPrefixAccountServices : IPartnerMsbPrefixAccountServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PartnerMsbPrefixAccountServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly PartnerMsbPrefixAccountEFRepository _partnerMsbPrefixAccountEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly BankRepository _bankRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;

        public PartnerMsbPrefixAccountServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<PartnerMsbPrefixAccountServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _partnerMsbPrefixAccountEFRepository = new PartnerMsbPrefixAccountEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _bankRepository = new BankRepository(_connectionString, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PartnerMsbPrefixAccount Add(CreatePartnerMsbPrefixAccountDto input)
        {
            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            if (_partnerMsbPrefixAccountEFRepository.Entity.FirstOrDefault(e => e.PartnerBankAccountId == input.PartnerBankAccountId && e.Deleted == YesNo.NO) != null)
            {
                _partnerMsbPrefixAccountEFRepository.ThrowException(ErrorCode.CorePartnerMsbPrefixAccountIsExist);
            }

            var inputInsert = _mapper.Map<PartnerMsbPrefixAccount>(input);
            inputInsert.PartnerId = partnerId;
            inputInsert.CreatedBy = username;
            var result = _partnerMsbPrefixAccountEFRepository.Add(inputInsert);
            _dbContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, partnerId = {partnerId}, username = {username}");


            var partnerFind = _partnerMsbPrefixAccountEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.PartnerId == partnerId);
            if (partnerFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CorePartnerMsbPrefixAccountNotFound);
            }
            partnerFind.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<PartnerMsbPrefixAccountDto> FindAll(FilterPartnerMsbPrefixAccountDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            var resultPaging = new PagingResult<PartnerMsbPrefixAccountDto>();
            var find = _partnerMsbPrefixAccountEFRepository.FindAll(input, partnerId);
            var result = _mapper.Map<List<PartnerMsbPrefixAccountDto>>(find.Items);
            resultPaging.Items = result;
            foreach (var item in resultPaging.Items)
            {
                var businessBank = _businessCustomerBankEFRepository.FindById(item.PartnerBankAccountId);
                if (businessBank == null)
                {
                    continue;
                }
                var bank = _bankRepository.GetById(businessBank.BankId);
                businessBank.BankName = bank.BankName;
                var businessBanks = _mapper.Map<BusinessCustomerBankDto>(businessBank);
                item.BusinessCustomerBank = businessBanks;
            }
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartnerMsbPrefixAccount FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}, partnerId = {partnerId}");
            var partnerMsbFind = _partnerMsbPrefixAccountEFRepository.FindById(id, partnerId);
            if (partnerMsbFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CorePartnerMsbPrefixAccountNotFound);
            }

            return _mapper.Map<PartnerMsbPrefixAccount>(partnerMsbFind);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="partnerBankAccId"></param>
        /// <returns></returns>
        public PartnerMsbPrefixAccount FindByPartnerBankId(int partnerBankAccId)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindByPartnerBankId)}: partnerBankAccId = {partnerBankAccId}, partnerId = {partnerId}");
            var partnerMsbFind = _partnerMsbPrefixAccountEFRepository.EntityNoTracking.FirstOrDefault(b => b.PartnerId == partnerId && b.PartnerBankAccountId == partnerBankAccId);
            if (partnerMsbFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CorePartnerMsbPrefixAccountNotFound);
            }

            return _mapper.Map<PartnerMsbPrefixAccount>(partnerMsbFind);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="input"></param>
        public void Update(UpdatePartnerMsbPrefixAccountDto input)
        {
            //Lấy thông tin đối tác
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            var partnerMsbFind = _partnerMsbPrefixAccountEFRepository.FindById(input.Id, partnerId);
            if (partnerMsbFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CorePartnerMsbPrefixAccountNotFound);
            }

            partnerMsbFind.PartnerBankAccountId = input.PartnerBankAccountId;
            partnerMsbFind.PrefixMsb = input.PrefixMsb;
            partnerMsbFind.MId = input.MId;
            partnerMsbFind.TId = input.TId;
            partnerMsbFind.AccessCode = input.AccessCode;
            partnerMsbFind.ModifiedBy = username;
            partnerMsbFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}

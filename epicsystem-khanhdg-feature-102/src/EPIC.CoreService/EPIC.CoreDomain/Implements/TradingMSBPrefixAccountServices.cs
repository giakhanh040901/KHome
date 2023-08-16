using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.TradingMSBPrefixAccount;
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
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class TradingMsbPrefixAccountServices : ITradingMsbPrefixAccountServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<TradingMsbPrefixAccountServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingMSBPrefixAccountEFRepository _tradingMSBPrefixAccountEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly BankRepository _bankRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;

        public TradingMsbPrefixAccountServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<TradingMsbPrefixAccountServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _tradingMSBPrefixAccountEFRepository = new TradingMSBPrefixAccountEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _bankRepository = new BankRepository(_connectionString, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
        }
        public void Add(CreateTradingMsbPrefixAccountDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin đối tác
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            if (_tradingMSBPrefixAccountEFRepository.Entity.FirstOrDefault(e => e.TradingBankAccountId == input.TradingBankAccountId && e.Deleted == YesNo.NO) != null)
            {
                _tradingMSBPrefixAccountEFRepository.ThrowException(ErrorCode.CorePartnerMsbPrefixAccountIsExist);
            }

            var tradingMSBInsert = _tradingMSBPrefixAccountEFRepository.Add(_mapper.Map<TradingMsbPrefixAccount>(input), tradingProviderId, username);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: Id = {id}");

            //Lấy thông tin đối tác
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var productFind = _tradingMSBPrefixAccountEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId);
            if (productFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreTradingMSBPrefixAccountNotFound);
            }
            productFind.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }

        public PagingResult<TradingMsbPrefixAccountDto> FindAll(FilterTradingMsbPrefixAccountDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var resultPaging = new PagingResult<TradingMsbPrefixAccountDto>();
            var find = _tradingMSBPrefixAccountEFRepository.FindAll(input, tradingProviderId);
            var result = _mapper.Map<List<TradingMsbPrefixAccountDto>>(find.Items);
            resultPaging.Items = result;
            foreach(var item in resultPaging.Items )
            {
                var businessBank = _businessCustomerBankEFRepository.FindById(item.TradingBankAccountId);
                var bank = _bankRepository.GetById(businessBank.BankId);
                businessBank.BankName = bank.BankName;
                var businessBanks = _mapper.Map<BusinessCustomerBankDto>(businessBank);
                item.businessCustomerBank = businessBanks;
            }
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        public TradingMsbPrefixAccount FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var tradingMSBFind = _tradingMSBPrefixAccountEFRepository.FindById(id, tradingProviderId);
            if (tradingMSBFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreTradingMSBPrefixAccountNotFound);
            }

            return _mapper.Map<TradingMsbPrefixAccount>(tradingMSBFind);
        }

        public TradingMsbPrefixAccount FindByTradingBankId(int tradingBankAccId)
        {
            _logger.LogInformation($"{nameof(FindByTradingBankId)}: tradingBankAccId = {tradingBankAccId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var tradingMSBFind = _tradingMSBPrefixAccountEFRepository.EntityNoTracking.FirstOrDefault(b => b.TradingProviderId == tradingProviderId && b.TradingBankAccountId == tradingBankAccId);
            if (tradingMSBFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreTradingMSBPrefixAccountNotFound);
            }

            return _mapper.Map<TradingMsbPrefixAccount>(tradingMSBFind);
        }

        public void Update(UpdateTradingMsbPrefixAccountDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin đối tác
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var tradingMSBFind = _tradingMSBPrefixAccountEFRepository.FindById(input.Id, tradingProviderId);
            if (tradingMSBFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreTradingMSBPrefixAccountNotFound);
            }

            tradingMSBFind.TradingBankAccountId = input.TradingBankAccountId;
            tradingMSBFind.PrefixMsb = input.PrefixMsb;
            tradingMSBFind.MId = input.MId;
            tradingMSBFind.TId = input.TId;
            tradingMSBFind.AccessCode = input.AccessCode;
            tradingMSBFind.TIdWithoutOtp = input.TIdWithoutOtp;
            tradingMSBFind.ModifiedBy = username;
            tradingMSBFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}

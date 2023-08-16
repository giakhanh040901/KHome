using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ReceiveContractTemplate;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondDomain.Implements
{
    public class BondReceiveContractTemplateService : IBondReceiveContractTemplateService
    {
        private ILogger<BondReceiveContractTemplate> _logger;
        private IConfiguration _configuration;
        private string _connectionString;
        private BondReceiveContractTemplateRepository _receiveContractTemplateRepository;
        private BondSecondaryRepository _productBondSecondaryRepository;
        private BondOrderRepository _orderRepository;
        private BondSecondaryContractRepository _bondSecondaryContractRepository;
        private IHttpContextAccessor _httpContext;

        public BondReceiveContractTemplateService(ILogger<BondReceiveContractTemplate> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _receiveContractTemplateRepository = new BondReceiveContractTemplateRepository(_connectionString, _logger);
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }
        public BondReceiveContractTemplate Add(CreateReceiveContractTemplateDto input)
        {
            var tradingProvider = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var contractTemplate = new BondReceiveContractTemplate
            {
                Code = input.Code,
                Name = input.Name,
                TradingProviderId = tradingProvider,
                FileUrl = input.FileUrl,
                SecondaryId = input.SecondaryId,
                CreatedBy = username
            };
            return _receiveContractTemplateRepository.Add(contractTemplate);
        }

        public int Delete(int id)
        {
            return _receiveContractTemplateRepository.Delete(id);
        }


        public BondReceiveContractTemplate FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _receiveContractTemplateRepository.FindById(id, tradingProviderId);
            return result;
        }

        public int Update(UpdateReceiveContractTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractTemplate = _receiveContractTemplateRepository.FindById(input.Id, tradingProviderId);
            contractTemplate.Code = input.Code;
            contractTemplate.Name = input.Name;
            contractTemplate.FileUrl = input.FileUrl;
            contractTemplate.ModifiedBy = username;
            return _receiveContractTemplateRepository.Update(contractTemplate);
        }

        public int ChangeStatus(int id)
        {
            var contractTemplate = FindById(id);
            var status = ContractTemplateStatus.ACTIVE;
            if (contractTemplate.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _receiveContractTemplateRepository.UpdateStatus(id, status, username);
        }

        public BondReceiveContractTemplate FindBySecondaryId(int bondSecondaryId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _receiveContractTemplateRepository.FindAll(bondSecondaryId, tradingProviderId);
        }
    }
}

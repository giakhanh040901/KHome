using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.BusinessCustomer;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerShared;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.GarnerSharedDomain.Implements
{
    public class GarnerContractCodeServices : IGarnerContractCodeServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerContractCodeServices> _logger;
        private readonly GarnerContractTemplateEFRepository _contractTemplateEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        //private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;

        private readonly IConfiguration _configuration;

        public GarnerContractCodeServices(EpicSchemaDbContext dbContext,
            ILogger<GarnerContractCodeServices> logger,
            IMapper mapper,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _contractTemplateEFRepository = new GarnerContractTemplateEFRepository(_dbContext, _logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(_dbContext, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(_dbContext, _logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(_dbContext, _logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(_dbContext, _logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(_dbContext, _logger);
        }

        public string GenOrderContractCodeDefault(GenGarnerContractCodeDto input)
        {
            var contractTemplates = _contractTemplateEFRepository.Entity.Where(c => c.PolicyId == input.Order.PolicyId && c.TradingProviderId == input.Order.TradingProviderId && c.Status == Status.ACTIVE && c.Deleted == YesNo.NO)
                     .Select(c => c.ConfigContractId).Distinct();
            if (contractTemplates.Count() == 1)
            {
                string contractCode = GetContractCode(input.Order, input.Product, input.Policy, contractTemplates.First());
                return contractCode;
            }
            return null;
        }

        /// <summary>
        /// Get contract code cho hợp đồng
        /// </summary>
        /// <param name="order"></param>
        /// <param name="product"></param>
        /// <param name="policy"></param>
        /// <param name="configContractId"></param>
        /// <returns></returns>
        public string GetContractCode(GarnerOrder order, GarnerProduct product, GarnerPolicy policy, int configContractId)
        {
            InvestorDataForContractDto investor = new();
            BusinessCustomerForContractDto businessCustomer = new();
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (cifCode.InvestorId != null)
            {
                var investorId = cifCode.InvestorId;
                investor = _investorEFRepository.GetDataInvestorForContract(investorId ?? 0, order.InvestorIdenId ?? 0, order.InvestorBankAccId ?? 0, order.ContractAddressId ?? 0);
                businessCustomer = null;
            }
            else
            {
                var businessCustomerId = cifCode.BusinessCustomerId;
                businessCustomer = _businessCustomerEFRepository.GetBusinessCustomerForContract(businessCustomerId ?? 0, order.InvestorBankAccId ?? 0);
                investor = null;
            }
            string productType = ConfigContractCode.ProductTypes(product.ProductType);
            var contractCode = _garnerOrderEFRepository.GenContractCode(new OrderContractCodeDto
            {
                OrderId = order.Id,
                ConfigContractCodeId = configContractId,
                Now = DateTime.Now,
                PolicyName = policy.Name,
                PolicyCode = policy.Code,
                ProductType = productType,
                ProductCode = product.Code,
                ProductName = product.Name,
                BuyDate = order.BuyDate,
                PaymentFullDate = order.PaymentFullDate,
                InvestDate = order.InvestDate,
                ShortName = investor?.InvestorIdentification?.Fullname,
                ShortNameBusiness = businessCustomer?.BusinessCustomer?.ShortName
            });

            return contractCode;
        }
    }
}

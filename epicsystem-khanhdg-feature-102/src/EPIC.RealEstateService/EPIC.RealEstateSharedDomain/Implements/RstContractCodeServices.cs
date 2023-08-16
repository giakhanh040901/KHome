using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.BusinessCustomer;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.Dto.RstGenContractCode;
using EPIC.RealEstateRepositories;
using EPIC.RealEstateSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.RealEstateSharedDomain.Implements
{
    public class RstContractCodeServices : IRstContractCodeServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstContractCodeServices> _logger;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly RstConfigContractCodeDetailEFRepository _rstConfigContractCodeDetailEFRepository;
        private readonly RstConfigContractCodeEFRepository _rstConfigContractCodeEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstDistributionContractTemplateEFRepository _rstDistributionContractTemplateEFRepository;


        private readonly IConfiguration _configuration;

        public RstContractCodeServices(EpicSchemaDbContext dbContext,
            ILogger<RstContractCodeServices> logger,
            IMapper mapper,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(_dbContext, _logger);
            _rstConfigContractCodeEFRepository = new RstConfigContractCodeEFRepository(_dbContext, _logger);
            _rstConfigContractCodeDetailEFRepository = new RstConfigContractCodeDetailEFRepository(_dbContext, _logger);
            _rstOrderEFRepository = new RstOrderEFRepository(_dbContext, _logger);
            _rstDistributionContractTemplateEFRepository = new RstDistributionContractTemplateEFRepository(_dbContext, _logger);

        }
        public string GenOrderContractCodeDefault(RstGenContractCodeDefaultDto input)
        {
            var contractTemplates = _rstDistributionContractTemplateEFRepository.Entity.Where(c => c.DistributionPolicyId == input.DistributionPolicy.Id && c.PartnerId == input.DistributionPolicy.PartnerId && c.Status == Status.ACTIVE && c.Deleted == YesNo.NO)
                     .Select(c => c.ConfigContractCodeId).Distinct();
            if (contractTemplates.Count() == 1)
            {
                string contractCode = GetContractCode(new RstGenContractCodeDto()
                {
                    Order = input.Order,
                    ProductItem = input.ProductItem,
                    Project = input.Project,
                    DistributionPolicy = input.DistributionPolicy,
                    ConfigContractCodeId = contractTemplates.First()
                });
                return contractCode;
            }
            return null;
        }

        public string GetContractCode(RstGenContractCodeDto input)
        {
            InvestorDataForContractDto investor = new();
            BusinessCustomerForContractDto businessCustomer = new();

            var cifCode = _cifCodeEFRepository.FindByCifCode(input.Order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            if (cifCode.InvestorId != null)
            {
                var investorId = cifCode.InvestorId;
                investor = _investorEFRepository.GetDataInvestorForContract(investorId ?? 0, input.Order.InvestorIdenId ?? 0, 0, input.Order.ContractAddressId ?? 0);
                businessCustomer = null;
            }
            else
            {
                var businessCustomerId = cifCode.BusinessCustomerId;
                businessCustomer = _businessCustomerEFRepository.GetBusinessCustomerForContract(businessCustomerId ?? 0, 0);
                investor = null;
            }
            var contractCode = _rstOrderEFRepository.GenContractCode(new OrderContractCodeDto
            {
                OrderId = input.Order.Id,
                ConfigContractCodeId = input.ConfigContractCodeId,
                Now = DateTime.Now,
                PolicyCode = input.DistributionPolicy.Code,
                ProjectCode = input.Project.Code,
                ProductName = input.ProductItem.Name,
                RstProductItemCode = input.ProductItem.Code,
                BuyDate = input.Order.CreatedDate,
                ShortName = investor?.InvestorIdentification?.Fullname,
                ShortNameBusiness = businessCustomer?.BusinessCustomer?.ShortName
            });

            return contractCode;
        }
    }
}

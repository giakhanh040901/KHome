using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.BusinessCustomer;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestSharedDomain.Implements
{
    public class InvestContractCodeServices : IInvestContractCodeServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<InvestContractCodeServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly InvestContractTemplateEFRepository _contractTemplateEFRepository;


        public InvestContractCodeServices(EpicSchemaDbContext dbContext,
            ILogger<InvestContractCodeServices> logger,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _contractTemplateEFRepository = new InvestContractTemplateEFRepository(_dbContext, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
        }

        public string GenOrderContractCodeDefault(GenInvestContractCodeDto input)
        {
            if (input.Order != null && input.Order.RenewalsReferId != null)
            {
                var orderContractFile = _dbContext.InvestOrderContractFile.Where(o => o.OrderId == input.Order.Id && o.Deleted == YesNo.NO);
                if (orderContractFile.Any())
                {
                    var contractCodeGenFirst = orderContractFile.FirstOrDefault().ContractCodeGen;
                    string contractCodeGen = orderContractFile.All(r => r.ContractCodeGen == contractCodeGenFirst) ? contractCodeGenFirst : null;
                    return contractCodeGen;
                }
            }

            var contractTemplates = _contractTemplateEFRepository.Entity.Where(c => c.PolicyId == input.Order.PolicyId && c.TradingProviderId == input.Order.TradingProviderId && c.Status == Status.ACTIVE && c.Deleted == YesNo.NO)
                     .Select(c => c.ConfigContractId).Distinct();
            if (contractTemplates.Count() == 1)
            {
                string contractCode = GetContractCode(input.Order, input.Policy, contractTemplates.First());
                return contractCode;
            }
            return null;
        }

        public string GetContractCode(InvOrder order, Policy policy, int configContractId)
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
            var contractCode = _contractTemplateEFRepository.GenContractCode(new OrderContractCodeDto
            {
                OrderId = order.Id,
                ConfigContractCodeId = configContractId,
                Now = DateTime.Now,
                PolicyName = policy.Name,
                PolicyCode = policy.Code,
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

using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Issuer;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsIssuerService : ICpsIssuerService
    {
        private readonly ILogger<CpsIssuerService> _logger;
        private readonly IConfiguration _configuration;
        private string _connectionString;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly CpsIssuerRepository _cpsIssuerRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;


        public CpsIssuerService(ILogger<CpsIssuerService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _cpsIssuerRepository = new CpsIssuerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }
        public CpsIssuer Add(CreateCpsIssuerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var issuer = new CpsIssuer
            {
                BusinessCustomerId = input.BusinessCustomerId,
                BusinessTurnover = input.BusinessTurnover,
                BusinessProfit = input.BusinessProfit,
                ROA = input.ROA,
                ROE = input.ROE,
                Image = input.Image,
                PartnerId = partnerId,
                CreatedBy = username,
            };
            return _cpsIssuerRepository.Add(issuer);
        }

        public int Delete(int id)
        {
            return _cpsIssuerRepository.Delete(id);
        }

        public int Update(int issuerId, UpdateCpsIssuerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var issuer = new CpsIssuer
            {
                Id = issuerId,
                BusinessTurnover = input.BusinessTurnover,
                BusinessProfit = input.BusinessProfit,
                ROA = input.ROA,
                ROE = input.ROE,
                Image = input.Image,
                PartnerId = partnerId,
                ModifiedBy = username,
            };
            return _cpsIssuerRepository.Update(issuer);
        }
        public ViewCpsIssuerDto FindById(int id)
        {
            var issuerFind = _cpsIssuerRepository.FindIssuerById(id);

            var result = _mapper.Map<ViewCpsIssuerDto>(issuerFind);

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(issuerFind.BusinessCustomerId);
            if (businessCustomer != null)
            {
                result.Name = businessCustomer.Name;
                result.Code = businessCustomer.Code;
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankDefault(businessCustomer.BusinessCustomerId ?? 0, IsTemp.NO);
                if (businessCustomerBank != null)
                {
                    result.BusinessCustomer.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
                }
            }
            return result;
        }

        public PagingResult<ViewCpsIssuerDto> FindAll(CpsIssuerFilterDto input)
        {
            var issuerList = _cpsIssuerRepository.FindAll(CommonUtils.GetCurrentPartnerId(_httpContext), input.PageSize, input.PageNumber, input.Keyword, input.Status);
            var result = new PagingResult<ViewCpsIssuerDto>();
            var items = new List<ViewCpsIssuerDto>();
            result.TotalItems = issuerList.TotalItems;
            foreach (var issuerFind in issuerList.Items)
            {
                var cpsIssuer = _mapper.Map<ViewCpsIssuerDto>(issuerFind);
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(issuerFind.BusinessCustomerId);
                if (businessCustomer != null)
                {
                    cpsIssuer.Name = businessCustomer.Name;
                    cpsIssuer.Code = businessCustomer.Code;
                    cpsIssuer.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    cpsIssuer.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(cpsIssuer);
            }
            result.Items = items;
            return result;
        }
    }

}

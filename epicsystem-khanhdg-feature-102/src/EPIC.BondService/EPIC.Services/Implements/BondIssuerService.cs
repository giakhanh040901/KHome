using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Issuer;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondIssuerService : IBondIssuerService
    {
        private readonly ILogger<BondIssuerService> _logger;
        private readonly IConfiguration _configuration;
        private string _connectionString;
        private readonly BondIssuerRepository _issuerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BondIssuerService(ILogger<BondIssuerService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _issuerRepository = new BondIssuerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public BondIssuer Add(CreateIssuerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var issuer = new BondIssuer
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
            return _issuerRepository.Add(issuer);
        }

        public int Delete(int id)
        {
            return _issuerRepository.Delete(id);
        }

        public PagingResult<ViewIssuerDto> FindAll(int pageSize, int pageNumber,  bool isNoPaging, string keyword, string status)
        {
            if (isNoPaging)
            {
                pageSize = -1;
            }

            var issuerList = _issuerRepository.FindAll(CommonUtils.GetCurrentPartnerId(_httpContext), pageSize, pageNumber, keyword, status);
            var result = new PagingResult<ViewIssuerDto>();
            var items = new List<ViewIssuerDto>();
            result.TotalItems = issuerList.TotalItems;
            foreach (var issuerFind in issuerList.Items)
            {
                var bondIssuer = _mapper.Map<ViewIssuerDto>(issuerFind);
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(issuerFind.BusinessCustomerId);
                if (businessCustomer != null)
                {
                    bondIssuer.Name = businessCustomer.Name;
                    bondIssuer.Code = businessCustomer.Code;
                    bondIssuer.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    bondIssuer.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(bondIssuer);
            }
            result.Items = items;
            return result;
        }

        public ViewIssuerDto FindById(int id)
        {
            var issuerFind = _issuerRepository.FindIssuerById(id);

            var result = _mapper.Map<ViewIssuerDto>(issuerFind);

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

        public int Update(int issuerId, UpdateIssuerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var issuer = new BondIssuer
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
            return _issuerRepository.Update(issuer);
        }
    }
}

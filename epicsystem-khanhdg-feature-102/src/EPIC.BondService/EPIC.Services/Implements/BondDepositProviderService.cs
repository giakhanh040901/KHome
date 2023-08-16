using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.DepositProvider;
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
    public class BondDepositProviderService : IBondDepositProviderService
    {
        private readonly ILogger<BondDepositProviderService> _logger;
        private readonly IConfiguration _configuration;
        private string _connectionString;
        private readonly BondDepositProviderRepository _depositProviderRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BondDepositProviderService(ILogger<BondDepositProviderService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _depositProviderRepository = new BondDepositProviderRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public BondDepositProvider Add(CreateDepositProviderDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var depositProvider = new BondDepositProvider
            {
                BusinessCustomerId = input.BusinessCustomerId,
                PartnerId = partnerId,
                CreatedBy = username
            };
            return _depositProviderRepository.Add(depositProvider);
        }

        public PagingResult<DepositProviderDto> FindAll(int pageSize, int pageNumber, string keyword, string status)
        {
            var depositProviderList = _depositProviderRepository.FindAll(CommonUtils.GetCurrentPartnerId(_httpContext), pageSize, pageNumber, keyword, status);
            var result = new PagingResult<DepositProviderDto>();
            var items = new List<DepositProviderDto>();
            result.TotalItems = depositProviderList.TotalItems;
            foreach (var depositFind in depositProviderList.Items)
            {
                var bondDeposit = new DepositProviderDto()
                {
                    DepositProviderId = depositFind.DepositProviderId,
                    BusinessCustomerId = depositFind.BusinessCustomerId,
                };
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(depositFind.BusinessCustomerId ?? 0);
                if (businessCustomer != null)
                {
                    bondDeposit.Name = businessCustomer.Name;
                    bondDeposit.Code = businessCustomer.Code;
                    bondDeposit.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    bondDeposit.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(bondDeposit);
            }
            result.Items = items;
            return result;
        }

        public DepositProviderDto FindById(int id)
        {
            var deposit = _depositProviderRepository.FindDepositProviderById(id);
            var result = new DepositProviderDto()
            {
                DepositProviderId = deposit.DepositProviderId,
                BusinessCustomerId = deposit.BusinessCustomerId,
            };

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(deposit.BusinessCustomerId ?? 0);
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

                var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                result.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
            }
            return result;
        }

        public int Delete(int id)
        {
            return _depositProviderRepository.Delete(id);
        }
    }
}
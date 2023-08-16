using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Implements
{
    public class GeneralContractorServices : IGeneralContractorServices
    {
        private readonly ILogger<GeneralContractorServices> _logger;
        private readonly IConfiguration _configuration;
        private string _connectionString;
        private readonly GeneralContractorRepository _generalContractorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public GeneralContractorServices(ILogger<GeneralContractorServices> logger, IConfiguration configuration,
            DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _generalContractorRepository = new GeneralContractorRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public PagingResult<ViewGeneralContractorDto> FindAll(int pageSize, int pageNumber, string keyword, string status)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }    
            var invGeneralContractorList = _generalContractorRepository.FindAll(partnerId, pageSize, pageNumber, keyword, status);
            var result = new PagingResult<ViewGeneralContractorDto>();
            var items = new List<ViewGeneralContractorDto>();
            result.TotalItems = invGeneralContractorList.TotalItems;
            foreach (var invGeneralContractorFind in invGeneralContractorList.Items)
            {
                var invGeneralContractor = new ViewGeneralContractorDto()
                {
                    Id = invGeneralContractorFind.Id,
                    BusinessCustomerId = invGeneralContractorFind.BusinessCustomerId,
                    PartnerId = invGeneralContractorFind.PartnerId,
                    Status = invGeneralContractorFind.Status
                };

                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(invGeneralContractorFind.BusinessCustomerId);
                if (businessCustomer != null)
                {
                    invGeneralContractor.Name = businessCustomer.Name;
                    invGeneralContractor.Code = businessCustomer.Code;
                    invGeneralContractor.TaxCode = businessCustomer.TaxCode;
                    invGeneralContractor.RepName = businessCustomer.RepName;
                    invGeneralContractor.ShortName = businessCustomer.ShortName;
                    invGeneralContractor.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    invGeneralContractor.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }


                items.Add(invGeneralContractor);
            }
            result.Items = items;
            return result;
        }

        public ViewGeneralContractorDto FindById(int id)
        {
            var invGeneralContractorFind = _generalContractorRepository.FindById(id);

            if(invGeneralContractorFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đơn vị tổng thầu này "), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
            var result = new ViewGeneralContractorDto()
            {
                Id = invGeneralContractorFind.Id,
                BusinessCustomerId = invGeneralContractorFind.BusinessCustomerId,
                PartnerId = invGeneralContractorFind.PartnerId,
                Status = invGeneralContractorFind.Status
            };

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(invGeneralContractorFind.BusinessCustomerId);
            if (businessCustomer != null)
            {
                result.Name = businessCustomer.Name;
                result.Code = businessCustomer.Code;
                result.TaxCode = businessCustomer.TaxCode;
                result.RepName = businessCustomer.RepName;
                result.ShortName = businessCustomer.ShortName;
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankDefault(businessCustomer.BusinessCustomerId ?? 0, IsTemp.NO);
                if (businessCustomerBank != null)
                {
                    result.BusinessCustomer.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
                }
            }
            return result;
        }

        public GeneralContractor Add(CreateGeneralContractorDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var invGeneralContractor = new GeneralContractor
            {
                BusinessCustomerId = input.BusinessCustomerId,
                PartnerId = partnerId,
                CreatedBy = username,
            };
            return _generalContractorRepository.Add(invGeneralContractor);
        }

        public int Delete(int id)
        {
            return _generalContractorRepository.Delete(id, CommonUtils.GetCurrentPartnerId(_httpContext));
        }
    }
}

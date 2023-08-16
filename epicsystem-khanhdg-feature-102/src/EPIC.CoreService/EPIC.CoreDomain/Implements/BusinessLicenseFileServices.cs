using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessLicenseFile;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class BusinessLicenseFileServices : IBusinessLicenseFileServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BusinessLicenseFileRepository _businessLicenseFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BusinessLicenseFileServices(
            ILogger<BusinessLicenseFileServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _businessLicenseFileRepository = new BusinessLicenseFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public BusinessLicenseFile Add(CreateBusinessLicenseFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _businessLicenseFileRepository.Add(input, username);
        }

        public BusinessLicenseFile AddTemp(CreateBusinessLicenseFileTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _businessLicenseFileRepository.AddTemp(input, username);
        }

        public int Delete(int id)
        {
            return _businessLicenseFileRepository.Delete(id);
        }

        public BusinessLicenseFileDto FindById(int id)
        {
            return _mapper.Map<BusinessLicenseFileDto>( _businessLicenseFileRepository.FindById(id));
        }

        public List<BusinessLicenseFileDto> FindAll(int? businessCustomerId, int? businessCustomerTempId)
        {
            return _businessLicenseFileRepository.FindAll(businessCustomerId, businessCustomerTempId);
        }

        public int Update(UpdateBusinessLicenseFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _businessLicenseFileRepository.Update(input, username);
        }

        public int UpdateTemp(UpdateBusinessLicenseFileTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _businessLicenseFileRepository.UpdateTemp(input, username);
        }
    }
}

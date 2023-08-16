using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.PolicyFile;
using EPIC.CompanySharesRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsPolicyFileServices : ICpsPolicyFileServices
    {

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly CpsPolicyFileRepository _cpsPolicyFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public CpsPolicyFileServices(
            ILogger<CpsPolicyFileServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _cpsPolicyFileRepository = new CpsPolicyFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public CpsPolicyFile Add(CreateCpsPolicyFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var file = new CpsPolicyFile()
            {
                SecondaryId = input.SecondaryId,
                TradingProviderId = tradingProviderId,
                Name = input.Name,
                Url = input.Url,
                ExpirationDate = input.ExpirationDate,
                EffectiveDate = input.EffectiveDate,
                CreatedBy = username
            };
            return _cpsPolicyFileRepository.Add(file);
        }

        public int DeletePolicyFile(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _cpsPolicyFileRepository.DeletePolicyFile(id, tradingProviderId);
        }

        public PagingResult<CpsPolicyFile> FindAllPolicyFile(CpsPolicyFileFilterDto input)
        {
            return _cpsPolicyFileRepository.FindAllPolicyFile(input.SecondaryId, CommonUtils.GetCurrentTradingProviderId(_httpContext), input.PageSize, input.PageNumber, input.Keyword);
        }

        public CpsPolicyFile FindPolicyFileById(int id)
        {
            return _cpsPolicyFileRepository.FindPolicyFileById(id);
        }

        public int PolicyFileUpdate(int id, UpdateCpsPolicyFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _cpsPolicyFileRepository.PolicyFileUpdate(new CpsPolicyFile
            {
                Id = id,
                SecondaryId = input.SecondaryId,
                TradingProviderId = tradingProviderId,
                Name = input.Name,
                Url = input.Url,
                ExpirationDate = input.ExpirationDate,
                EffectiveDate = input.EffectiveDate,
                ModifiedBy = username
            });
        }
    }
}

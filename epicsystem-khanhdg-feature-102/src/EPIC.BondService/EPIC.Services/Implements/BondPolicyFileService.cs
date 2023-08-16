using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.PolicyFile;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EPIC.BondDomain.Implements
{
    public class BondPolicyFileService : IBondPolicyFileService
    {

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondPolicyFileRepository _policyFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BondPolicyFileService(
            ILogger<BondDistributionContractService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _policyFileRepository = new BondPolicyFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public BondPolicyFile Add(CreatePolicyFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var file = new BondPolicyFile()
            {
                SecondaryId = input.SecondaryId,
                TradingProviderId = tradingProviderId,
                Name = input.Name,
                Url = input.Url,
                ExpirationDate = input.ExpirationDate,
                EffectiveDate = input.EffectiveDate,
                CreatedBy = username
            };
            return _policyFileRepository.Add(file);
        }

        public int DeletePolicyFile(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _policyFileRepository.DeletePolicyFile(id, tradingProviderId);
        }

        public PagingResult<BondPolicyFile> FindAllPolicyFile(int bondSecondaryId, int pageSize, int pageNumber, string keyword)
        {
            return _policyFileRepository.FindAllPolicyFile(bondSecondaryId, CommonUtils.GetCurrentTradingProviderId(_httpContext), pageSize, pageNumber, keyword);
        }

        public BondPolicyFile FindPolicyFileById(int id)
        {
            return _policyFileRepository.FindPolicyFileById(id);
        }

        public int PolicyFileUpdate(int id, UpdatePolicyFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _policyFileRepository.PolicyFileUpdate(new BondPolicyFile
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

using AutoMapper;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistriPolicyFile;
using EPIC.InvestRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Implements
{
    public class DistriPolicyFileServices : IDistriPolicyFileServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DistriPolicyFileRepository _distriPolicyFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public DistriPolicyFileServices(
            ILogger<DistriPolicyFileServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _distriPolicyFileRepository = new DistriPolicyFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }
        public DistriPolicyFile Add(CreateDistriPolicyFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var file = new DistriPolicyFile()
            {
                DistributionId = input.DistributionId,
                TradingProviderId = tradingProviderId,
                Name = input.Name,
                Url = input.Url,
                ExpirationDate = input.ExpirationDate,
                EffectiveDate = input.EffectiveDate,
                CreatedBy = username
            };
            return _distriPolicyFileRepository.Add(file);
        }

        public int DeleteDistriPolicyFile(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _distriPolicyFileRepository.DeleteDistriPolicyFile(id, tradingProviderId);
        }

        public int DistriPolicyFileUpdate(int id, UpdateDistriPolicyFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _distriPolicyFileRepository.DistriPolicyFileUpdate(new DistriPolicyFile
            {
                Id = id,
                DistributionId = input.DistributionId,
                TradingProviderId = tradingProviderId,
                Name = input.Name,
                Url = input.Url,
                ExpirationDate = input.ExpirationDate,
                EffectiveDate = input.EffectiveDate,
                ModifiedBy = username
            });
        }

        public PagingResult<DistriPolicyFile> FindAllDistriPolicyFile(int distributionId, int pageSize, int pageNumber, string keyword)
        {
            return _distriPolicyFileRepository.FindAllDistriPolicyFile(distributionId, CommonUtils.GetCurrentTradingProviderId(_httpContext), pageSize, pageNumber, keyword);

        }

        public DistriPolicyFile FindDistriPolicyFileById(int id)
        {
            return _distriPolicyFileRepository.FindDistriPolicyFileById(id);
        }
    }
}

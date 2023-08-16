using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionFile;
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
using System.Transactions;

namespace EPIC.InvestDomain.Implements
{
    public class DistributionFileService : IDistributionFileService
    {
        private readonly ILogger<DistributionFileService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DistributionFileRepository _distributionFileRepository;
        private readonly IHttpContextAccessor _httpContext;

        public DistributionFileService(ILogger<DistributionFileService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _distributionFileRepository = new DistributionFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }


        public DistributionFile Add(CreateDistributionFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            DistributionFile distributionFile = new DistributionFile();

            using (TransactionScope scope = new TransactionScope())
            {
                distributionFile = _distributionFileRepository.Add(new DistributionFile
                {

                    DistributionId = input.DistributionId,
                    FileUrl = input.FileUrl,
                    Title = input.Title,
                    CreatedBy = username
                });
                scope.Complete();
            }

            _distributionFileRepository.CloseConnection();
            return distributionFile;
        }

        public void Update(UpdateDistributionFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            using (TransactionScope scope = new TransactionScope())
            {
                _distributionFileRepository.Update(new DistributionFile
                {
                    Id = input.Id,
                    DistributionId = input.DistributionId,
                    FileUrl = input.FileUrl,
                    Title = input.Title,
                    ModifiedBy = username,
                });
                scope.Complete();
            }

            _distributionFileRepository.CloseConnection();
        }

        public PagingResult<DistributionFileDto> FindAll(int distributionId, int pageSize, int pageNumber)
        {
            return _distributionFileRepository.FindAll(distributionId, pageSize, pageNumber);
        }

        public DistributionFile FindById(int id)
        {
            return _distributionFileRepository.FindById(id);
        }

        public int Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _distributionFileRepository.Delete(id);
        }


    }
}

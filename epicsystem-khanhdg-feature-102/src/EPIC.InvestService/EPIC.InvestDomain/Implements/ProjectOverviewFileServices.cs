using AutoMapper;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
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
    public class ProjectOverviewFileServices : IProjectOverviewFileServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ProjectOverviewFileRepository _projectOverviewFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ProjectOverviewFileServices(
            ILogger<ProjectOverviewFileServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _projectOverviewFileRepository = new ProjectOverviewFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public ProjectOverviewFile Add(CreateProjectOverviewFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var file = new ProjectOverviewFile()
            {
                DistributionId = input.DistributionId,
                Title = input.Title,
                Url = input.Url,
                CreatedBy = username
            };
            return _projectOverviewFileRepository.Add(file, tradingProviderId);
        }

        public int Delete(int id)
        {
            return _projectOverviewFileRepository.Delete(id);
        }

        public PagingResult<ProjectOverviewFile> FindAll(int distributionId, int pageSize, int pageNumber, int? status)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _projectOverviewFileRepository.FindAll(distributionId, pageSize, pageNumber, status, tradingProviderId);
        }

        public ProjectOverviewFile FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _projectOverviewFileRepository.FindById(id, tradingProviderId);
        }

        public int Update(UpdateProjectOverviewFileDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _projectOverviewFileRepository.Update(new ProjectOverviewFile
            {
                Id = input.Id,
                DistributionId = input.DistributionId,
                Title = input.Title,
                Url = input.Url,
                ModifiedBy = username
            }, tradingProviderId);
        }
    }
}

using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionNews;
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
    public class DistributionNewsServices : IDistributionNewsServices
    {
        private readonly ILogger<DistributionNewsServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DistributionNewsRepository _distributionNewsRepository;

        private readonly IHttpContextAccessor _httpContext;

        public DistributionNewsServices(ILogger<DistributionNewsServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _distributionNewsRepository = new DistributionNewsRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }
        public DistributionNews Add(CreateDistributionNewsDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionNews = new DistributionNews
            {
                DistributionId = input.DistributionId,
                TradingProviderId = tradingProviderId,
                ImgUrl = input.ImgUrl,
                Title = input.Title,
                Content = input.Content,
                CreatedBy = username
            };
            return _distributionNewsRepository.Add(distributionNews);
        }

        public PagingResult<ViewDistributionNewsDto> FindAll(int pageSize, int pageNumber, string status, int distributionId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _distributionNewsRepository.FindAll(pageSize, pageNumber, status, distributionId, tradingProviderId);
        }

        public int Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _distributionNewsRepository.Delete(id, tradingProviderId);
        }

        public ViewDistributionNewsDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _distributionNewsRepository.FindById(id, tradingProviderId);
            return result;
        }

        public int Update(UpdateDistributionNewsDto body)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _distributionNewsRepository.Update(new DistributionNews
            {
                Id = body.Id,
                TradingProviderId = tradingProviderId,
                ImgUrl = body.ImgUrl,
                Title = body.Title,
                Content = body.Content,
                ModifiedBy = username
            });
        }

        public int ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distribution = _distributionNewsRepository.FindById(id, tradingProviderId);
            var status = ContractTemplateStatus.ACTIVE;
            if (distribution.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _distributionNewsRepository.UpdateStatus(id, status, username);
        }
    }
}

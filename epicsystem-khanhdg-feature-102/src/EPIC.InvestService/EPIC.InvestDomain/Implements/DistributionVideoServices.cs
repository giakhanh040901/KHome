using AutoMapper;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionVideo;
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
    public class DistributionVideoServices : IDistributionVideoServices
    {
        private readonly ILogger<DistributionVideoServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DistributionVideoRepository _distributionVideoRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public DistributionVideoServices(ILogger<DistributionVideoServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _distributionVideoRepository = new DistributionVideoRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }
        public DistributionVideo Add(CreateDistributionVideoDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionVideo = new DistributionVideo
            {
                DistributionId = input.DistributionId,
                TradingProviderId = tradingProviderId,
                UrlVideo = input.UrlVideo,
                Title = input.Title,
                ModifiedBy = username
            };
            return _distributionVideoRepository.Add(distributionVideo , tradingProviderId);
        }

        public PagingResult<ViewDistributionVideoDto> FindAll(int pageSize, int pageNumber, string status, int distributionId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _distributionVideoRepository.FindAll(pageSize, pageNumber, status, distributionId, tradingProviderId);
        }

        public int Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _distributionVideoRepository.Delete(id, tradingProviderId);
        }

        public ViewDistributionVideoDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _distributionVideoRepository.FindById(id, tradingProviderId);
            return result;
        }

        public ViewDistributionVideoDto FindNewVideo(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionVideoFind = _distributionVideoRepository.FindById(id, tradingProviderId);
            return _mapper.Map<ViewDistributionVideoDto>(distributionVideoFind);
        }

        public int Update(UpdateDistributionVideoDto body)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _distributionVideoRepository.Update(new DistributionVideo
            {
                Id = body.Id,
                TradingProviderId = tradingProviderId,
                UrlVideo = body.UrlVideo,
                Title = body.Title,
                CreatedBy = username
            });
        }


        public int ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionVideo = _distributionVideoRepository.FindById(id, tradingProviderId);
            var status = ContractTemplateStatus.ACTIVE;
            if (distributionVideo.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _distributionVideoRepository.UpdateStatus(id, status, username);
        }

        public int ChangeFeature(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionVideo = _distributionVideoRepository.FindById(id, tradingProviderId);
            var feature = YesNo.YES;
            if (distributionVideo.Feature == YesNo.YES)
            {
                feature = YesNo.NO;
            }
            else
            {
                feature = YesNo.YES;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _distributionVideoRepository.UpdateFeature(id, feature, username);
        }
    }
}

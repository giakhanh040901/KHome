using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreProductNews;
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
    public class CoreProductNewsServices : ICoreProductNewsServices
    {
        private readonly ILogger<CoreProductNewsServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly CoreProductNewsRepositories _coreProductNewsRepositories;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;


        public CoreProductNewsServices(ILogger<CoreProductNewsServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _coreProductNewsRepositories = new CoreProductNewsRepositories(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public CoreProductNews Add(CreateCoreProductNewsDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var bondNewsServices = new CoreProductNews
            {
                TradingProviderId = tradingProviderId,
                ImgUrl = input.ImgUrl,
                Title = input.Title,
                Content = input.Content,
                Feature = input.Feature,
                Location = input.Location,
                CreatedBy = username
            };
            return _coreProductNewsRepositories.Add(bondNewsServices);
        }

        public PagingResult<ViewCoreProductNewsDto> FindAll(int pageSize, int pageNumber, string status , int? location)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _coreProductNewsRepositories.FindAll(pageSize, pageNumber, status, tradingProviderId, location);
        }

        public PagingResult<ViewCoreProductNewsDto> AppFindAll(int pageSize, int pageNumber, string status, int? location)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _coreProductNewsRepositories.FindAll(pageSize, pageNumber, status, tradingProviderId, location);
        }

        public int Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _coreProductNewsRepositories.Delete(id, tradingProviderId);
        }

        public ViewCoreProductNewsDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _coreProductNewsRepositories.FindById(id, tradingProviderId);
            return result;
        }

 
        public int Update(UpdateCoreProductNewsDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _coreProductNewsRepositories.Update(new CoreProductNews
            {
                Id = body.Id,
                TradingProviderId = tradingProviderId,
                ImgUrl = body.ImgUrl,
                Title = body.Title,
                Content = body.Content,
                Location =body.Location,
                ModifiedBy = username
            });
        }

        public int ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var BondNews = _coreProductNewsRepositories.FindById(id, tradingProviderId);
            var status = ContractTemplateStatus.ACTIVE;
            if (BondNews.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _coreProductNewsRepositories.UpdateStatus(id, status, username);
        }

        public int ChangeFeature(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var BondNews = _coreProductNewsRepositories.FindById(id, tradingProviderId);
            var feature = YesNo.YES;
            if (BondNews.Feature == YesNo.YES)
            {
                feature = YesNo.NO;
            }
            else
            {
                feature = YesNo.YES;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _coreProductNewsRepositories.UpdateFeature(id, feature, username);
        }
    }
}

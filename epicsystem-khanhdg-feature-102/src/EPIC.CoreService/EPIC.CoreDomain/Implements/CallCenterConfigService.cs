using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.CallCenterConfig;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace EPIC.CoreDomain.Implements
{
    public class CallCenterConfigService : ICallCenterConfigService
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly CallCenterConfigEFRepository _callCenterConfigEFRepository;

        public CallCenterConfigService(
            ILogger<CallCenterConfigService> logger,
            IHttpContextAccessor httpContext,
            EpicSchemaDbContext dbContext)
        {
            _logger = logger;
            _httpContext = httpContext;
            _dbContext = dbContext;
            _callCenterConfigEFRepository = new CallCenterConfigEFRepository(dbContext, logger);
        }

        public void UpdateConfig(UpdateCallCenterConfigDto input)
        {
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            var callCenterQuery = _dbContext.CallCenterConfigs.AsQueryable();
            if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC)
            {
                callCenterQuery = callCenterQuery.Where(c => c.TradingProviderId == null);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                callCenterQuery = callCenterQuery.Where(c => c.TradingProviderId == tradingProviderId);
            }
            else
            {
                return;
            }
            _logger.LogInformation($"{nameof(UpdateConfig)}: input = {JsonSerializer.Serialize(input)}, userType = {userType}, tradingProviderId = {tradingProviderId}");
            var removeList = callCenterQuery.Where(c => !input.Details.Select(d => d.UserId).Contains(c.UserId));
            _dbContext.CallCenterConfigs.RemoveRange(removeList);
            foreach (var config in input.Details)
            {
                if (!callCenterQuery.Any(c => c.UserId == config.UserId))
                {
                    _dbContext.CallCenterConfigs.Add(new CallCenterConfig
                    {
                        Id = (int)_callCenterConfigEFRepository.NextKey(),
                        UserId = config.UserId,
                        SortOrder = config.SortOrder,
                        TradingProviderId = tradingProviderId,
                    });
                }
                else
                {
                    var configFind = callCenterQuery.FirstOrDefault(c => c.UserId == config.UserId);
                    if (configFind != null)
                    {
                        configFind.SortOrder = config.SortOrder;
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        public PagingResult<CallCenterConfigDto> FindAllConfig(FilterCallCenterConfigDto input)
        {
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            var callCenterQuery = _dbContext.CallCenterConfigs
                .Include(c => c.TradingProvider).ThenInclude(c => c.BusinessCustomer)
                .AsQueryable();
            if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC)
            {
                callCenterQuery = callCenterQuery.Where(c => c.TradingProviderId == null);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                callCenterQuery = callCenterQuery.Where(c => c.TradingProviderId == tradingProviderId);
            }
            else
            {
                return null;
            }
            _logger.LogInformation($"{nameof(FindAllConfig)}: userType = {userType}, tradingProviderId = {tradingProviderId}");
            PagingResult<CallCenterConfigDto> result = new()
            {
                TotalItems = callCenterQuery.Count()
            };

            var queryResult = callCenterQuery.Select(c => new CallCenterConfigDto
            {
                Id = c.Id,
                TradingProviderId = c.TradingProviderId,
                TradingProvider = new TradingProviderCallCenterConfigDto
                {
                    BusinessCustomerName = c.TradingProvider.BusinessCustomer.Name,
                    AliasName = c.TradingProvider.AliasName
                },
                UserId = c.UserId,
                User = _dbContext.Users.Select(u => new UserCallCenterDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Fullname = u.DisplayName,
                }).FirstOrDefault(u => u.UserId == c.UserId),
                SortOrder = c.SortOrder,
            });
            queryResult = queryResult.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                queryResult = queryResult.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = queryResult;
            return result;
        }

        public IEnumerable<UserIdCallCenterDto> GetListUserIdCallCenter()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllConfig)}: investorId = {investorId}");
            List<int> tradingProviderIds = _dbContext.InvestorTradingProviders.Where(o => o.InvestorId == investorId)
                .Select(o => o.TradingProviderId)
                .ToList();
            var callCenterQuery = _dbContext.CallCenterConfigs.AsQueryable();
            if (tradingProviderIds.Count == 1) //lấy theo đại lý
            {
                callCenterQuery = callCenterQuery.Where(c => c.TradingProviderId == tradingProviderIds.First());
            }
            else //lấy root
            {
                callCenterQuery = callCenterQuery.Where(c => c.TradingProviderId == null);
            }
            return callCenterQuery.OrderBy(c => c.SortOrder)
                .Select(c => new UserIdCallCenterDto
                {
                    Id = c.Id,
                    TradingProviderId = c.TradingProviderId,
                    UserId = c.UserId,
                    SortOrder = c.SortOrder
                });
        }
    }
}

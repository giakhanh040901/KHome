using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.TradingFirstMessage;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.IdentityRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class TradingFirstMessageServices : ITradingFirstMessageServices
    {
        private readonly ILogger<TradingFirstMessageServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly TradingFirstMessageEFRepository _tradingFirstMsgEFRepository;

        public TradingFirstMessageServices(EpicSchemaDbContext dbContext,
            ILogger<TradingFirstMessageServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper, IRocketChatServices rocketChatServices)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _mapper = mapper;
            _tradingFirstMsgEFRepository = new TradingFirstMessageEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Lưu lời chào
        /// </summary>
        /// <param name="dto"></param>
        public void SaveMessage(SaveTradingFirstMessageDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name} dto={JsonSerializer.Serialize(dto)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                dto.TradingProviderId = tradingProviderId;
            }

            var tradingFirstMsg = _tradingFirstMsgEFRepository.FindByTrading(dto.TradingProviderId);

            if (tradingFirstMsg != null)
            {
                tradingFirstMsg.Message = dto.Message;
                _dbContext.SaveChanges();
            }
            else
            {
                _tradingFirstMsgEFRepository.Add(new TradingFirstMessage
                {
                    TradingProviderId = dto.TradingProviderId,
                    Message = dto.Message,
                });
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Tìm list lời chào
        /// </summary>
        /// <returns></returns>
        public List<ViewTradingFirstMessageDto> FindAll()
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (new string[] {UserTypes.ROOT_EPIC, UserTypes.EPIC}.Contains(usertype))
            {
                return _mapper.Map<List<ViewTradingFirstMessageDto>>(_tradingFirstMsgEFRepository.FindAll());
            }
            else if (new string[] {UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                var data = _tradingFirstMsgEFRepository.FindAll().Where(x => x.TradingProviderId == tradingProviderId).Take(1);
                return _mapper.Map<List<ViewTradingFirstMessageDto>>(data);
            }
            return null;
        }

        public ViewTradingFirstMessageDto FindByTrading(int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name} tradingProviderId={tradingProviderId}");

            return _mapper.Map<ViewTradingFirstMessageDto>(_tradingFirstMsgEFRepository.FindByTrading(tradingProviderId));
        }
    }
}

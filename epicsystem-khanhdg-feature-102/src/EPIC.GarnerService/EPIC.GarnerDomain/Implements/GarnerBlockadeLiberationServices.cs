using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerBlockadeLiberationServices : IGarnerBlockadeLiberationServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerBlockadeLiberationServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly GarnerBlockadeLiberationEFRepository _garnerBlockadeLiberationEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly IGarnerOrderServices _garnerOrderServices;


        public GarnerBlockadeLiberationServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerBlockadeLiberationServices> logger,
            IGarnerOrderServices garnerOrderServices,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _garnerBlockadeLiberationEFRepository = new GarnerBlockadeLiberationEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerOrderServices = garnerOrderServices;
        }
        public GarnerBlockadeLiberation Add(CreateGarnerBlockadeLiberationDto input)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationServices)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(input.OrderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            var insert = _garnerBlockadeLiberationEFRepository.Add(_mapper.Map<GarnerBlockadeLiberation>(input), username, tradingProviderId);
            var changeStatus = _garnerOrderEFRepository.ChangeStatus(input.OrderId, OrderStatus.PHONG_TOA, tradingProviderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Tìm kiến phong toả giải toả không phân trang
        /// </summary>
        /// <returns></returns>
        public List<GarnerBlockadeLiberation> FindAll()
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationServices)}->{nameof(FindAll)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var blockadeLiberations = _garnerBlockadeLiberationEFRepository.FindAll(tradingProviderId);
            return _mapper.Map<List<GarnerBlockadeLiberation>>(blockadeLiberations);
        }

        /// <summary>
        /// Tìm kiến phong toả giải toả có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerOrderMoreInfoDto> FindAllBlockadeLiberation(FilterGarnerOrderDto input, int[] status)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationServices)}->{nameof(FindAllBlockadeLiberation)}: input = {JsonSerializer.Serialize(input)}");
            var result = _garnerOrderServices.FindAllByPolicy(input,status);
            return result;
        }

        public GarnerBlockadeLiberationDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationServices)}->{nameof(FindById)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var blockadeLiberation = _garnerBlockadeLiberationEFRepository.FindById(id, tradingProviderId).ThrowIfNull<GarnerBlockadeLiberation>(_dbContext, ErrorCode.GarnerBlockadeLiberationNotFound);
            return _mapper.Map<GarnerBlockadeLiberationDto>(blockadeLiberation);
        }

        public GarnerBlockadeLiberation Update(UpdateGarnerBlockadeLiberationDto input)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationServices)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var orderFind = _garnerOrderEFRepository.FindById(input.OrderId, tradingProviderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            var blockadeLiberation = _garnerBlockadeLiberationEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull<GarnerBlockadeLiberation>(_dbContext, ErrorCode.GarnerBlockadeLiberationNotFound);
            var updateBlockadeLiberation = _garnerBlockadeLiberationEFRepository.Update(_mapper.Map<GarnerBlockadeLiberation>(input), username, tradingProviderId);
            
            if (updateBlockadeLiberation.Status == BlockadeLiberationStatus.GIAI_TOA)
            {
                var changeStatus = _garnerOrderEFRepository.ChangeStatus(input.OrderId, OrderStatus.DANG_DAU_TU, tradingProviderId).ThrowIfNull<GarnerOrder>(_dbContext, ErrorCode.GarnerOrderNotFound);
            }
            _dbContext.SaveChanges();
            return updateBlockadeLiberation;
        }
    }
}

using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtConfigContractCode;
using EPIC.EventEntites.Dto.EvtConfigContractCodeDetail;
using EPIC.EventEntites.Entites;
using EPIC.EventRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.EventDomain.Implements
{
    public class EvtConfigContractCodeServices : IEvtConfigContractCodeServices
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<EvtConfigContractCodeServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly EvtConfigContractCodeEFRepository _evtConfigContractCodeEFRepository;
        private readonly EvtConfigContractCodeDetailEFRepository _evtConfigContractCodeDetailEFRepository;

        public EvtConfigContractCodeServices(
            EpicSchemaDbContext dbContext,
            ILogger<EvtConfigContractCodeServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);

            _evtConfigContractCodeEFRepository = new EvtConfigContractCodeEFRepository(dbContext, logger);
            _evtConfigContractCodeDetailEFRepository = new EvtConfigContractCodeDetailEFRepository(dbContext, logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Thêm config mã hợp đồng (Config contract code)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EvtConfigContractCodeDto AddConfigContractCode(CreateEvtConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AddConfigContractCode)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, usernamme = {username}");

            var transaction = _dbContext.Database.BeginTransaction();
            //Add cấu hình contractCode
            var insert = new EvtConfigContractCode
            {
                Id = (int)_evtConfigContractCodeEFRepository.NextKey(),
                Name = input.Name,
                TradingProviderId = tradingProviderId,
                Description = input.Description,
                CreatedBy = username
            };
            _dbContext.Add(insert);

            _dbContext.SaveChanges();
            //Add Config detail
            foreach (var item in input.ConfigContractCodeDetails)
            {
                var insertConfigDetail = new EvtConfigContractCodeDetail
                {
                    Id = (int)_evtConfigContractCodeDetailEFRepository.NextKey(),
                    ConfigContractCodeId = insert.Id,
                    SortOrder = item.SortOrder,
                    Key = item.Key,
                    Value = item.Value
                };
                _evtConfigContractCodeDetailEFRepository.Entity.Add(insertConfigDetail);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return _mapper.Map<EvtConfigContractCodeDto>(insert);
        }

        /// <summary>
        /// Cập nhật config mã hợp đồng (Config contract code)
        /// </summary>
        /// <param name="input"></param>
        public void UpdateConfigContractCode(UpdateEvtConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCode)}: input = {JsonSerializer.Serialize(input)}; tradingProviderId = {tradingProviderId}, username = {username}");

            var configContractCode = _evtConfigContractCodeEFRepository.Entity.FirstOrDefault(c => c.Id == input.Id && c.TradingProviderId == tradingProviderId && c.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtConfigContractCodeNotFound);
            configContractCode.Name = input.Name;
            configContractCode.Description = input.Description;
            UpdateConfigContractCodeDetail(input.ConfigContractCodeDetails, input.Id);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật config contract code details
        /// </summary>
        /// <param name="input"></param>
        /// <param name="configContractCodeId"></param>
        public void UpdateConfigContractCodeDetail(List<CreateEvtConfigContractCodeDetailDto> input, int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCodeDetail)}: input = {JsonSerializer.Serialize(input)}, configContractCodeId = {configContractCodeId}, tradingProviderId = {tradingProviderId}");

            var configContractCode = _evtConfigContractCodeEFRepository.Entity.FirstOrDefault(c => c.Id == configContractCodeId && c.TradingProviderId == tradingProviderId && c.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.EvtConfigContractCodeNotFound);

            var getConfigDetailQuery = _evtConfigContractCodeDetailEFRepository.Entity.Where(c => c.ConfigContractCodeId == configContractCodeId);
            // Xóa detail
            var ids = input.Where(i => i.Id != 0).Select(i => i.Id);
            var removeDetail = getConfigDetailQuery.Where(d => !ids.Contains(d.Id));
            _evtConfigContractCodeDetailEFRepository.Entity.RemoveRange(removeDetail);

            foreach (var item in input)
            {
                if (item.Id == 0)
                {
                    if (item.Key != ConfigContractCode.FIX_TEXT)
                    {
                        item.Value = null;
                    }
                    var insertConfigDetail = new EvtConfigContractCodeDetail
                    {
                        Id = (int)_evtConfigContractCodeDetailEFRepository.NextKey(),
                        ConfigContractCodeId = configContractCodeId,
                        SortOrder = item.SortOrder,
                        Key = item.Key,
                        Value = item.Value
                    };
                    _evtConfigContractCodeDetailEFRepository.Entity.Add(insertConfigDetail);
                }
                else
                {
                    var configdetailUpdate = _evtConfigContractCodeDetailEFRepository.Entity.FirstOrDefault(e => e.Id == item.Id);
                    if (configdetailUpdate != null)
                    {
                        configdetailUpdate.Key = item.Key;
                        configdetailUpdate.SortOrder = item.SortOrder;
                        if (item.Key == ConfigContractCode.FIX_TEXT)
                        {
                            configdetailUpdate.Value = item.Value;
                        }
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xem danh sách config contract code
        /// </summary>
        /// <returns></returns>
        public PagingResult<EvtConfigContractCodeDto> GetAllConfigContractCode(FilterEvtConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(GetAllConfigContractCode)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var query = from configContractCodes in _dbContext.EvtConfigContractCodes.Include(configContractCode => configContractCode.ConfigContractCodeDetails)
                        where configContractCodes.TradingProviderId == tradingProviderId && configContractCodes.Deleted == YesNo.NO
                        && (input.Status == null || configContractCodes.Status == input.Status)
                        && (input.Keyword == null || configContractCodes.Name.ToLower().Contains(input.Keyword.ToLower()))
                        select new EvtConfigContractCodeDto
                        {
                            Id = configContractCodes.Id,
                            Name = configContractCodes.Name,
                            TradingProviderId = tradingProviderId,
                            Status = configContractCodes.Status,
                            Description = configContractCodes.Description,
                            CreatedDate = configContractCodes.CreatedDate,
                            CreatedBy = configContractCodes.CreatedBy,
                            CancelBy = configContractCodes.ModifiedBy,
                            CancelDate = configContractCodes.ModifiedDate,
                            ConfigContractCodeDetails = _mapper.Map<List<EvtConfigContractCodeDetailDto>>(configContractCodes.ConfigContractCodeDetails)
                        };
            var result = new PagingResult<EvtConfigContractCodeDto>();
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }

        public List<EvtConfigContractCodeDto> GetAllConfigContractCodeStatusActive()
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllConfigContractCode)}: tradingProviderId = {tradingProviderId}");

            var result = from configContractCodes in _dbContext.EvtConfigContractCodes.Include(configContractCode => configContractCode.ConfigContractCodeDetails)
                         where configContractCodes.TradingProviderId == tradingProviderId && configContractCodes.Deleted == YesNo.NO
                         && configContractCodes.Status == Status.ACTIVE
                         select new EvtConfigContractCodeDto
                         {
                             Id = configContractCodes.Id,
                             Name = configContractCodes.Name,
                             TradingProviderId = tradingProviderId,
                             Status = configContractCodes.Status,
                             Description = configContractCodes.Description,
                             CreatedDate = configContractCodes.CreatedDate,
                             CreatedBy = configContractCodes.CreatedBy,
                             ConfigContractCodeDetails = _mapper.Map<List<EvtConfigContractCodeDetailDto>>(configContractCodes.ConfigContractCodeDetails)
                         };
            return result.ToList();
        }

        /// <summary>
        /// Lấy config theo Id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <returns></returns>
        public EvtConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetConfigContractCodeById)}: configContractCodeId = {configContractCodeId}");

            var configContractCode = _evtConfigContractCodeEFRepository.Entity.FirstOrDefault(c => c.Id == configContractCodeId && c.TradingProviderId == tradingProviderId && c.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestConfigContractCodeNotFound);
            var result = _mapper.Map<EvtConfigContractCodeDto>(configContractCode);
            result.ConfigContractCodeDetails = _mapper.Map<List<EvtConfigContractCodeDetailDto>>(_evtConfigContractCodeDetailEFRepository.EntityNoTracking.Where(e => e.ConfigContractCodeId == configContractCodeId));
            return result;
        }

        /// <summary>
        /// Đổi trạng thái config theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        public void UpdateConfigContractCodeStatus(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCodeStatus)}: configContractCodeId = {configContractCodeId}");

            var configContractCode = _evtConfigContractCodeEFRepository.Entity.FirstOrDefault(c => c.Id == configContractCodeId && c.TradingProviderId == tradingProviderId && c.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtConfigContractCodeNotFound);
            configContractCode.Status = (configContractCode.Status == Status.ACTIVE) ? Status.INACTIVE : Status.ACTIVE;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        public void DeleteConfigContractCode(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(DeleteConfigContractCode)}: configContractCodeId = {configContractCodeId}");

            var configContractCode = _evtConfigContractCodeEFRepository.Entity.FirstOrDefault(c => c.Id == configContractCodeId && c.TradingProviderId == tradingProviderId && c.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.EvtConfigContractCodeNotFound);
            configContractCode.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }
    }
}

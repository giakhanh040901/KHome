using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestEntities.Dto.InvConfigContractCodeDetail;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.InvestDomain.Implements
{
    public class ConfigContractCodeServices : IConfigContractCodeServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<ConfigContractCodeServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DefErrorEFRepository _defErrorEFRepository;


        private readonly InvConfigContractCodeEFRepository _invConfigContractCodeEFRepository;
        private readonly InvConfigContractCodeDetailEFRepository _invConfigContractCodeDetailEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IInvestSharedServices _investSharedServices;

        public ConfigContractCodeServices(
            EpicSchemaDbContext dbContext,
            ILogger<ConfigContractCodeServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IInvestSharedServices investSharedServices)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
           
            _invConfigContractCodeEFRepository = new InvConfigContractCodeEFRepository(dbContext, logger);
            _invConfigContractCodeDetailEFRepository = new InvConfigContractCodeDetailEFRepository(dbContext, logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _investSharedServices = investSharedServices;
        }
        /// <summary>
        /// Thêm config mã hợp đồng (Config contract code)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public InvConfigContractCodeDto AddConfigContractCode(CreateConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AddConfigContractCode)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var transaction = _dbContext.Database.BeginTransaction();
            //Add cấu hình contractCode
            //var checkConfig = _invConfigContractCodeEFRepository.EntityNoTracking
            //    .Where(e => e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO && e.Status == Status.ACTIVE);
            var insertConfig = _mapper.Map<InvestConfigContractCode>(input);
            insertConfig.Status = Status.ACTIVE;

            var resultConfig = _invConfigContractCodeEFRepository.Add(insertConfig, tradingProviderId, username);

            _dbContext.SaveChanges();
            //Add Config detail
            foreach (var item in input.ConfigContractCodeDetails)
            {
                var insertConfigDetail = _mapper.Map<InvestConfigContractCodeDetail>(item);
                insertConfigDetail.ConfigContractCodeId = resultConfig.Id;
                var detailAdd = _invConfigContractCodeDetailEFRepository.Add(insertConfigDetail);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return _mapper.Map<InvConfigContractCodeDto>(resultConfig);
        }

        /// <summary>
        /// Cập nhật config mã hợp đồng (Config contract code)
        /// </summary>
        /// <param name="input"></param>
        public void UpdateConfigContractCode(UpdateConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCode)}: input = {JsonSerializer.Serialize(input)}; tradingProviderId = {tradingProviderId}");

            var configContractCode = _invConfigContractCodeEFRepository.FindById(input.Id, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestConfigContractCodeNotFound);
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
        public void UpdateConfigContractCodeDetail(List<InvestEntities.Dto.InvConfigContractCodeDetail.CreateConfigContractCodeDetailDto> input, int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCodeDetail)}: input = {JsonSerializer.Serialize(input)}, configContractCodeId = {configContractCodeId}");

            var configContractCode = _invConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestConfigContractCodeNotFound);

            var getConfigDetailQuery = _invConfigContractCodeDetailEFRepository.Entity.Where(c => c.ConfigContractCodeId == configContractCodeId);
            // Xóa detail
            var ids = input.Where(i => i.Id != 0).Select(i => i.Id);
            var removeDetail = getConfigDetailQuery.Where(d => !ids.Contains(d.Id)).ToList();
            _invConfigContractCodeDetailEFRepository.Entity.RemoveRange(removeDetail);

            foreach (var item in input)
            {
                if (item.Id == 0)
                {
                    if (item.Key != ConfigContractCode.FIX_TEXT)
                    {
                        item.Value = null;
                    }
                    var insertConfigDetail = _mapper.Map<InvestConfigContractCodeDetail>(item);
                    _invConfigContractCodeDetailEFRepository.Add(insertConfigDetail);
                }
                else
                {
                    var configdetailUpdate = _invConfigContractCodeDetailEFRepository.Entity.FirstOrDefault(e => e.Id == item.Id);
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
        /// <param name="customerType"></param>
        /// <returns></returns>
        public PagingResult<InvConfigContractCodeDto> GetAllConfigContractCode(FilterInvConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllConfigContractCode)}: input = {JsonSerializer.Serialize(input)} ,tradingProviderId = {tradingProviderId}");

            var configContractCodes = _invConfigContractCodeEFRepository.GetAll(input, tradingProviderId);
            var result = new PagingResult<InvConfigContractCodeDto>();
            result.Items = _mapper.Map<List<InvConfigContractCodeDto>>(configContractCodes.Items);
            result.TotalItems = configContractCodes.TotalItems;

            foreach (var item in result.Items)
            {
                item.ConfigContractCodeDetails = _mapper.Map<List<InvConfigContractCodeDetailDto>>(_invConfigContractCodeDetailEFRepository.GetAllByConfigId(item.Id).OrderBy(o => o.SortOrder));
            }
            return result;
        }

        public List<InvConfigContractCodeDto> GetAllConfigContractCodeStatusActive()
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllConfigContractCode)}: tradingProviderId = {tradingProviderId}");

            var configContractCodes = _invConfigContractCodeEFRepository.GetAllStatusActive(tradingProviderId);
            var result = _mapper.Map<List<InvConfigContractCodeDto>>(configContractCodes);
            foreach (var item in result)
            {
                item.ConfigContractCodeDetails = _mapper.Map<List<InvConfigContractCodeDetailDto>>(_invConfigContractCodeDetailEFRepository.GetAllByConfigId(item.Id));
            }
            return result;
        }

        /// <summary>
        /// Lấy config theo Id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <returns></returns>
        public InvConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _logger.LogInformation($"{nameof(GetConfigContractCodeById)}: configContractCodeId = {configContractCodeId}, tradingProviderId = {tradingProviderId}");

            var configContractCode = _invConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.InvestConfigContractCodeNotFound);
            var result = _mapper.Map<InvConfigContractCodeDto>(configContractCode);
            result.ConfigContractCodeDetails = _mapper.Map<List<InvConfigContractCodeDetailDto>>(_invConfigContractCodeDetailEFRepository.GetAllByConfigId(configContractCode.Id));
            return result;
        }

        /// <summary>
        /// Đổi trạng thái config theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        public void UpdateConfigContractCodeStatus(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCodeStatus)}: configContractCodeId = {configContractCodeId}, tradingProviderId = {tradingProviderId}");

            var configContractCode = _invConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.InvestConfigContractCodeNotFound);
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
            _logger.LogInformation($"{nameof(DeleteConfigContractCode)}: configContractCodeId = {configContractCodeId}, tradingProviderId = {tradingProviderId}");

            var configContractCode = _invConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.InvestConfigContractCodeNotFound);
            configContractCode.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }
    }
}

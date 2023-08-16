using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstConfigContractCode;
using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstConfigContractCodeServices : IRstConfigContractCodeServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstConfigContractCodeServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstConfigContractCodeEFRepository _rstConfigContractCodeEFRepository;
        private readonly RstConfigContractCodeDetailEFRepository _rstConfigContractCodeDetailEFRepository;

        public RstConfigContractCodeServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstConfigContractCodeServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstConfigContractCodeEFRepository = new RstConfigContractCodeEFRepository(dbContext, logger);
            _rstConfigContractCodeDetailEFRepository = new RstConfigContractCodeDetailEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm cấu trúc mã hợp đồng
        /// </summary>
        /// <param name="input"></param>
        public void AddConfigContractCode(CreateRstConfigContractCodeDto input)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(AddConfigContractCode)}: input = {input}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var transaction = _dbContext.Database.BeginTransaction();
            //Add cấu hình contractCode
            var insertConfig = _mapper.Map<RstConfigContractCode>(input);

            var resultConfig = _rstConfigContractCodeEFRepository.Add(insertConfig, username, partnerId, tradingProviderId);
            //Add Config detail
            foreach (var item in input.ConfigContractCodeDetails)
            {
                var insertConfigDetail = _mapper.Map<RstConfigContractCodeDetail>(item);
                insertConfigDetail.ConfigContractCodeId = resultConfig.Id;
                var detailAdd = _rstConfigContractCodeDetailEFRepository.Add(insertConfigDetail);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Cập nhật config mã hợp đồng (Config contract code)
        /// </summary>
        /// <param name="input"></param>
        public void UpdateConfigContractCode(UpdateRstConfigContractCodeDto input)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(UpdateConfigContractCode)}: input = {input}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var configContractCode = _rstConfigContractCodeEFRepository.FindById(input.Id, tradingProviderId, partnerId);
            if (configContractCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.RstConfigContractCodeNotFound);
            }
            configContractCode.Name = input.Name;
            configContractCode.Description = input.Description;
            configContractCode.ModifiedBy = username;
            configContractCode.ModifiedDate = DateTime.Now;
            UpdateConfigContractCodeDetail(input.ConfigContractCodeDetails, input.Id);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật config contract code details
        /// </summary>
        /// <param name="input"></param>
        /// <param name="configContractCodeId"></param>
        public void UpdateConfigContractCodeDetail(List<CreateRstConfigContractCodeDetailDto> input, int configContractCodeId)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(UpdateConfigContractCodeDetail)}: input = {input}, configContractCodeId = { configContractCodeId }");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var configContractCode = _rstConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId, partnerId)
                .ThrowIfNull(_dbContext, ErrorCode.RstConfigContractCodeNotFound);

            var getConfigDetailQuery = _rstConfigContractCodeDetailEFRepository.Entity.Where(c => c.ConfigContractCodeId == configContractCodeId);
            // Xóa detail
            var ids = input.Where(i => i.Id != 0).Select(i => i.Id);
            var removeDetail = getConfigDetailQuery.Where(d => !ids.Contains(d.Id)).ToList();
            _rstConfigContractCodeDetailEFRepository.Entity.RemoveRange(removeDetail);

            foreach (var item in input)
            {
                if (item.Id == 0)
                {
                    if (item.Key != ConfigContractCode.FIX_TEXT)
                    {
                        item.Value = null;
                    }
                    var insertConfigDetail = _mapper.Map<RstConfigContractCodeDetail>(item);
                    _rstConfigContractCodeDetailEFRepository.Add(insertConfigDetail);
                }
                else
                {
                    var configDetailUpdate = _rstConfigContractCodeDetailEFRepository.Entity.FirstOrDefault(e => e.Id == item.Id);
                    if (configDetailUpdate != null)
                    {
                        configDetailUpdate.Key = item.Key;
                        configDetailUpdate.SortOrder = item.SortOrder;
                        if (item.Key == ConfigContractCode.FIX_TEXT)
                        {
                            configDetailUpdate.Value = item.Value;
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
        public PagingResult<RstConfigContractCodeDto> GetAllConfigContractCode(FilterRstConfigContractCodeDto input)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(GetAllConfigContractCode)}: input = {input}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var configContractCodes = _rstConfigContractCodeEFRepository.GetAll(input, tradingProviderId, partnerId);
            var result = new PagingResult<RstConfigContractCodeDto>();
            result.Items = _mapper.Map<List<RstConfigContractCodeDto>>(configContractCodes.Items);
            result.TotalItems = configContractCodes.TotalItems;
            foreach (var item in result.Items)
            {
                item.ConfigContractCodeDetails = _mapper.Map<List<RstConfigContractCodeDetailDto>>(_rstConfigContractCodeDetailEFRepository.GetAllByConfigId(item.Id).OrderBy(c => c.SortOrder));
            }
            return result;
        }

        /// <summary>
        /// Xem danh sách config contract code (của đại lý và đối tác)cho mở bán
        /// </summary>
        /// <returns></returns>
        public PagingResult<RstConfigContractCodeDto> GetAllConfig(FilterRstConfigContractCodeDto input)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(GetAllConfig)}: input = {input}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var configContractCodes = _rstConfigContractCodeEFRepository.GetAllConfigContractCode(input, tradingProviderId, partnerId);
            var result = new PagingResult<RstConfigContractCodeDto>();
            result.Items = _mapper.Map<List<RstConfigContractCodeDto>>(configContractCodes.Items);
            result.TotalItems = configContractCodes.TotalItems;
            foreach (var item in result.Items)
            {
                item.ConfigContractCodeDetails = _mapper.Map<List<RstConfigContractCodeDetailDto>>(_rstConfigContractCodeDetailEFRepository.GetAllByConfigId(item.Id));
            }
            return result;
        }

        /// <summary>
        /// Tìm cấu trúc mã hợp đồng trạng thái Active
        /// </summary>
        /// <returns></returns>
        public List<RstConfigContractCodeDto> GetAllConfigContractCodeStatusActive()
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(GetAllConfigContractCodeStatusActive)}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);


            var configContractCodes = _rstConfigContractCodeEFRepository.GetAllStatusActive(tradingProviderId, partnerId);
            var result = _mapper.Map<List<RstConfigContractCodeDto>>(configContractCodes);
            foreach (var item in result)
            {
                item.ConfigContractCodeDetails = _mapper.Map<List<RstConfigContractCodeDetailDto>>(_rstConfigContractCodeDetailEFRepository.GetAllByConfigId(item.Id));
            }
            return result;
        }

        /// <summary>
        /// Lấy config theo Id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        public RstConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(GetConfigContractCodeById)}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);


            var configContractCode = _rstConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId, partnerId).ThrowIfNull<RstConfigContractCode>(_dbContext, ErrorCode.RstConfigContractCodeNotFound);
            var result = _mapper.Map<RstConfigContractCodeDto>(configContractCode);
            result.ConfigContractCodeDetails = _mapper.Map<List<RstConfigContractCodeDetailDto>>(_rstConfigContractCodeDetailEFRepository.GetAllByConfigId(configContractCode.Id));
            return result;
        }

        /// <summary>
        /// Đổi trạng thái config theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        public void UpdateConfigContractCodeStatus(int configContractCodeId)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(UpdateConfigContractCodeStatus)}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var configContractCode = _rstConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId, partnerId).ThrowIfNull<RstConfigContractCode>(_dbContext, ErrorCode.RstConfigContractCodeNotFound);
            configContractCode.Status = (configContractCode.Status == Status.ACTIVE) ? Status.INACTIVE : Status.ACTIVE;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        public void DeleteConfigContractCode(int configContractCodeId)
        {
            _logger.LogInformation($"{nameof(RstConfigContractCodeServices)}->{nameof(DeleteConfigContractCode)}");

            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var configContractCode = _rstConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId, partnerId).ThrowIfNull<RstConfigContractCode>(_dbContext, ErrorCode.RstConfigContractCodeNotFound);
            configContractCode.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }
    }
}

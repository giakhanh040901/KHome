using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerPolicyDetailTempServices : IGarnerPolicyDetailTempServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerPolicyDetailTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerPolicyTempEFRepository _garnerPolicyTempEFRepository;
        private readonly GarnerPolicyDetailTempEFRepository _garnerPolicyDetailTempEFRepository;

        public GarnerPolicyDetailTempServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerPolicyDetailTempServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _garnerPolicyTempEFRepository = new GarnerPolicyTempEFRepository(dbContext, logger);
            _garnerPolicyDetailTempEFRepository = new GarnerPolicyDetailTempEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm kỳ hạn mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerPolicyDetailTemp Add(CreateGarnerPolicyDetailTempDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyTemp = _garnerPolicyTempEFRepository.Entity.FirstOrDefault(p => p.Id == input.PolicyTempId && p.Deleted == YesNo.NO);
            if (policyTemp == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyTempNotFound);
            }
            var insert = _garnerPolicyDetailTempEFRepository.Add(_mapper.Map<GarnerPolicyDetailTemp>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Cập nhật kỳ hạn mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerPolicyDetailTemp Update(UpdateGarnerPolicyDetailTempDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetailTemp = _garnerPolicyDetailTempEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.Deleted == YesNo.NO);
            if (policyDetailTemp == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyDetailTempNotFound);
            }
            var updatePolicyDetailTemp = _garnerPolicyDetailTempEFRepository.Update(_mapper.Map<GarnerPolicyDetailTemp>(input), username);
            _dbContext.SaveChanges();
            return updatePolicyDetailTemp;
        }

        /// <summary>
        /// Xóa kỳ hạn mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="FaultException"></exception>
        public void Delete(int id)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetailTemp = _garnerPolicyDetailTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId);
            if (policyDetailTemp == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyDetailTempNotFound);
            }
            policyDetailTemp.Deleted = YesNo.YES;
            policyDetailTemp.ModifiedBy = username;
            policyDetailTemp.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kỳ hạn mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerPolicyDetailTempDto FindById(int id)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyTemp = _garnerPolicyDetailTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO);
            if (policyTemp == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyDetailTempNotFound);
            }
            return _mapper.Map<GarnerPolicyDetailTempDto>(policyTemp);
        }

        /// <summary>
        /// Tìm danh sách kỳ hạn theo id chính sách mẫu
        /// </summary>
        /// <param name="policyTempId"></param>
        /// <returns></returns>
        public List<GarnerPolicyDetailTempDto> FindAll(int policyTempId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: policyTempId = {policyTempId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetailTemps = _garnerPolicyDetailTempEFRepository.FindAll(policyTempId, tradingProviderId);
            return _mapper.Map<List<GarnerPolicyDetailTempDto>>(policyDetailTemps);
        }
    }
}

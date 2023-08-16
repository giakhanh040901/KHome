using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using EPIC.GarnerEntities.Dto.GarnerPolicyTemp;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerPolicyTempServices : IGarnerPolicyTempServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerPolicyTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerPolicyTempEFRepository _garnerPolicyTempEFRepository;
        private readonly GarnerPolicyDetailTempEFRepository _garnerPolicyDetailTempEFRepository;
        private readonly GarnerContractTemplateTempEFRepository _garnerContractTemplateTempEFRepository;

        public GarnerPolicyTempServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerPolicyTempServices> logger,
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
            _garnerContractTemplateTempEFRepository = new GarnerContractTemplateTempEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerPolicyTemp Add(CreateGarnerPolicyTempDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policy = _garnerPolicyTempEFRepository.Add(_mapper.Map<GarnerPolicyTemp>(input), username, tradingProviderId);

            if (input.InterestType == EPIC.Utils.InterestTypes.NGAY_CO_DINH)
            {
                if (input.RepeatFixedDate == null /*|| input.InterestPeriodType == null*/)
                {
                    throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }
            else if (input.InterestType == EPIC.Utils.InterestTypes.DINH_KY)
            {
                if (input.InterestPeriodQuantity == null || input.InterestPeriodType == null)
                {
                    throw new FaultException(new FaultReason($"Kỳ hạn {input.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            };

            //Thêm kỳ hạn mẫu
            foreach (var policyDetail in input.PolicyDetails)
            {
                policyDetail.PolicyTempId = policy.Id;
                _garnerPolicyDetailTempEFRepository.Add(_mapper.Map<GarnerPolicyDetailTemp>(policyDetail), username, tradingProviderId);
            }

            //Thêm mẫu hợp đồng mẫu
            foreach (var contractTemplateTemp in input.ContractTemplateTemps)
            {
                //contractTemplateTemp.PolicyTempId = policy.Id;
                _garnerContractTemplateTempEFRepository.Add(_mapper.Map<GarnerContractTemplateTemp>(contractTemplateTemp), username, tradingProviderId);
            }
            _dbContext.SaveChanges();
            return policy;
        }

        /// <summary>
        /// Cập nhật chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerPolicyTemp Update(UpdateGarnerPolicyTempDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var policyTemp = _garnerPolicyTempEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.Deleted == YesNo.NO);
            if (policyTemp == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyTempNotFound);
            }

            if (input.InterestType == EPIC.Utils.InterestTypes.NGAY_CO_DINH)
            {
                if (input.RepeatFixedDate == null /*|| input.InterestPeriodType == null*/)
                {
                    throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }
            else if (input.InterestType == EPIC.Utils.InterestTypes.DINH_KY)
            {
                if (input.InterestPeriodQuantity == null || input.InterestPeriodType == null)
                {
                    throw new FaultException(new FaultReason($"Kỳ hạn {input.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            };

            var updatePolicyTemp = _garnerPolicyTempEFRepository.Update(_mapper.Map<GarnerPolicyTemp>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return updatePolicyTemp;
        }

        /// <summary>
        /// Xóa chính sách mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="FaultException"></exception>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: Id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyTemp = _garnerPolicyTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId);
            if (policyTemp == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyTempNotFound);
            }
            policyTemp.Deleted = YesNo.YES;
            policyTemp.ModifiedBy = username;
            policyTemp.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm chính sách mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerPolicyTempDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyTemp = _garnerPolicyTempEFRepository.FindById(id, tradingProviderId);
            if (policyTemp == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyTempNotFound);
            }
            GarnerPolicyTempDto garnerPolicyTempDto = new();
            garnerPolicyTempDto = _mapper.Map<GarnerPolicyTempDto>(policyTemp);

            #region lấy danh sách kỳ hạn
            var policyDetailTemp = _garnerPolicyDetailTempEFRepository.FindAll(policyTemp.Id, tradingProviderId);
            garnerPolicyTempDto.PolicyDetails = _mapper.Map<List<GarnerPolicyDetailTempDto>>(policyDetailTemp);
            #endregion
            return garnerPolicyTempDto;
        }

        /// <summary>
        /// Danh sách chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerPolicyTempDto> FindAll(FilterPolicyTempDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {input}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new PagingResult<GarnerPolicyTempDto>();
            List<GarnerPolicyTempDto> garnerPolicyTempDtos = new ();
            
            var policyTemps = _garnerPolicyTempEFRepository.FindAll(input, tradingProviderId);
            foreach(var policyTemp in policyTemps.Items)
            {
                GarnerPolicyTempDto garnerPolicyTempDto = new();
                garnerPolicyTempDto = _mapper.Map<GarnerPolicyTempDto>(policyTemp);

                #region lấy danh sách kỳ hạn
                var policyDetailTemp = _garnerPolicyDetailTempEFRepository.FindAll(policyTemp.Id, tradingProviderId);
                garnerPolicyTempDto.PolicyDetails = _mapper.Map<List<GarnerPolicyDetailTempDto>>(policyDetailTemp);
                #endregion
                #region lấy danh sách kỳ hạn
                //var contractTemplateTemps = _garnerContractTemplateTempEFRepository.FindAll(policyTemp.Id, tradingProviderId);
                //garnerPolicyTempDto.ContractTemplateTemps = _mapper.Map<List<GarnerContractTemplateTempDto>>(contractTemplateTemps);
                #endregion
                garnerPolicyTempDtos.Add(garnerPolicyTempDto);
            }



            result.Items = garnerPolicyTempDtos;
            result.TotalItems = policyTemps.TotalItems;
            return result;
        }

        /// <summary>
        /// Danh sách chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<GarnerPolicyTempDto> FindAllNoPermission(string status)
        {
            _logger.LogInformation($"{nameof(FindAllNoPermission)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new List<GarnerPolicyTempDto>();
            List<GarnerPolicyTempDto> garnerPolicyTempDtos = new();

            var policyTemps = _garnerPolicyTempEFRepository.FindAllNoPermission(tradingProviderId);
            foreach (var policyTemp in policyTemps)
            {
                GarnerPolicyTempDto garnerPolicyTempDto = new();
                garnerPolicyTempDto = _mapper.Map<GarnerPolicyTempDto>(policyTemp);

                #region lấy danh sách kỳ hạn
                var policyDetailTemp = _garnerPolicyDetailTempEFRepository.FindAll(policyTemp.Id, tradingProviderId);
                garnerPolicyTempDto.PolicyDetails = _mapper.Map<List<GarnerPolicyDetailTempDto>>(policyDetailTemp);
                #endregion
                #region lấy danh sách kỳ hạn
                //var contractTemplateTemps = _garnerContractTemplateTempEFRepository.FindAll(policyTemp.Id, tradingProviderId);
                //garnerPolicyTempDto.ContractTemplateTemps = _mapper.Map<List<GarnerContractTemplateTempDto>>(contractTemplateTemps);
                #endregion
                garnerPolicyTempDtos.Add(garnerPolicyTempDto);
            }
            return garnerPolicyTempDtos;
        }

        /// <summary>
        /// Hủy kích hoạt chính sách mẫu
        /// </summary>
        /// <param name="policyTempId"></param>
        public void ChangeStatusPolicyTemp(int policyTempId)
        {
            _logger.LogInformation($"{nameof(ChangeStatusPolicyTemp)}: policyTempId = {policyTempId}");
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var policyTemp = _garnerPolicyTempEFRepository.Entity.FirstOrDefault(p => p.Id == policyTempId && p.TradingProviderId == tradingProviderId);
            if (policyTemp == null)
            {
                _garnerPolicyTempEFRepository.ThrowException(ErrorCode.GarnerPolicyTempNotFound);
            }

            if (policyTemp.Status == Status.ACTIVE)
            {
                policyTemp.Status = Status.INACTIVE;
            }
            else if (policyTemp.Status == Status.INACTIVE)
            {
                policyTemp.Status = Status.ACTIVE;
            }

            policyTemp.ModifiedDate = DateTime.Now;
            policyTemp.ModifiedBy = username;
            _dbContext.SaveChanges();
        }
    }
}

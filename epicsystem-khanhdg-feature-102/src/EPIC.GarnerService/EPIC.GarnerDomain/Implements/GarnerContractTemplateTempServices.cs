using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EntitiesBase.Dto;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
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
    public class GarnerContractTemplateTempServices : IGarnerContractTemplateTempServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerPolicyTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerPolicyTempEFRepository _garnerPolicyTempEFRepository;
        private readonly GarnerContractTemplateTempEFRepository _garnerContractTemplateTempEFRepository;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateEFRepository;

        public GarnerContractTemplateTempServices(EpicSchemaDbContext dbContext,
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
            _garnerPolicyTempEFRepository = new GarnerPolicyTempEFRepository(dbContext, logger);
            _garnerContractTemplateTempEFRepository = new GarnerContractTemplateTempEFRepository(dbContext, logger);
            _garnerContractTemplateEFRepository = new GarnerContractTemplateEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm chính mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerContractTemplateTemp Add(CreateGarnerContractTemplateTempDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var insert = _garnerContractTemplateTempEFRepository.Add(_mapper.Map<GarnerContractTemplateTemp>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return insert;
        }

        //// <summary>
        /// Cập nhật mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerContractTemplateTemp Update(UpdateGarnerContractTemplateTempDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemp = _garnerContractTemplateTempEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull<GarnerContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            var updateContractTepmlateTemp = _garnerContractTemplateTempEFRepository.Update(_mapper.Map<GarnerContractTemplateTemp>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return updateContractTepmlateTemp;
        }

        /// <summary>
        /// Xóa mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="FaultException"></exception>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: Id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemp = _garnerContractTemplateTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId).ThrowIfNull<GarnerContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            var contractTemplate = _garnerContractTemplateEFRepository.Entity.Any(c => c.ContractTemplateTempId == id && c.TradingProviderId == tradingProviderId && c.Deleted == YesNo.NO);
            if (contractTemplate)
            {
                _garnerContractTemplateTempEFRepository.ThrowException(ErrorCode.GarnerContractTemplateTempExist);
            }
            contractTemplateTemp.Deleted = YesNo.YES;
            contractTemplateTemp.ModifiedBy = username;
            contractTemplateTemp.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm mẫu hợp đồng mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerContractTemplateTempDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemp = _garnerContractTemplateTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO).ThrowIfNull<GarnerContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            return _mapper.Map<GarnerContractTemplateTempDto>(contractTemplateTemp);
        }

        /// <summary>
        /// Tìm danh sách mẫu hợp đồng mẫu theo id chính sách mẫu
        /// </summary>
        /// <param name="policyTempId"></param>
        /// <returns></returns>
        public List<GarnerContractTemplateTempDto> FindAll(int policyTempId)
        {
            _logger.LogInformation($"{nameof(FindAll)}: policyTempId = {policyTempId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemps = _garnerContractTemplateTempEFRepository.FindAll(tradingProviderId);
            return _mapper.Map<List<GarnerContractTemplateTempDto>>(contractTemplateTemps);
        }

        public PagingResult<GarnerContractTemplateTemp> FindAllContractTemplateTemp(FilterGarnerContractTemplateTempDto input)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext); 
            var resultPaging = new PagingResult<GarnerContractTemplateTemp>();

            var find = _garnerContractTemplateTempEFRepository.FindAllContractTemplateTemp(input, tradingProviderId);

            resultPaging.Items = find.Items.ToList();
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// Tìm danh sách mẫu hợp đồng mẫu không phân trang
        /// </summary>
        /// <param name="contractSource"></param>
        /// <returns></returns>
        public List<GarnerContractTemplateTempDto> GetAllContractTemplateTemp(int? contractSource)
        {
            _logger.LogInformation($"{nameof(GetAllContractTemplateTemp)}: contractSource = {contractSource}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemps = _garnerContractTemplateTempEFRepository.GetAllContractTemplateTemp(contractSource, tradingProviderId);
            return _mapper.Map<List<GarnerContractTemplateTempDto>>(contractTemplateTemps);
        }

        public GarnerContractTemplateTemp ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemp = _garnerContractTemplateTempEFRepository.FindById(id, tradingProviderId).ThrowIfNull<GarnerContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            var status = ContractTemplateStatus.ACTIVE;
            if (contractTemplateTemp.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var changeStatus = _garnerContractTemplateTempEFRepository.ChangeStatus(id, status, tradingProviderId);
            _dbContext.SaveChanges();
            return changeStatus;
        }
    }
}

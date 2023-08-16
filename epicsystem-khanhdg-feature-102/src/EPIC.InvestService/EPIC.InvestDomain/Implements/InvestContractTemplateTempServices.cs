using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.EntitiesBase.Dto;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using EPIC.InvestRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Implements
{
    public class InvestContractTemplateTempServices : IInvestContractTemplateTempServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PolicyTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly InvestContractTemplateTempEFRepository _investContractTemplateTempEFRepository;
        private readonly InvestContractTemplateEFRepository _investContractTemplateEFRepository;

        public InvestContractTemplateTempServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<PolicyTempServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _investContractTemplateTempEFRepository = new InvestContractTemplateTempEFRepository(dbContext, logger);
            _investContractTemplateEFRepository = new InvestContractTemplateEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm chính mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public InvestContractTemplateTempDto Add(CreateInvestContractTemplateTempDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var insert = _investContractTemplateTempEFRepository.Add(_mapper.Map<InvestContractTemplateTemp>(input), username, tradingProviderId);
            //var policyTemp = _garnerPolicyTempEFRepository.Entity.FirstOrDefault(p => p.Id == input.PolicyTempId && p.Deleted == YesNo.NO);
            //if (policyTemp == null)
            //{
            //    _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyTempNotFound);
            //}
            _dbContext.SaveChanges();
            return _mapper.Map<InvestContractTemplateTempDto>(insert);
        }

        //// <summary>
        /// Cập nhật mẫu hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public InvestContractTemplateTemp Update(UpdateInvestContractTemplateTempDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemp = _investContractTemplateTempEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull<InvestContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            var updateContractTemplateTemp = _investContractTemplateTempEFRepository.Update(_mapper.Map<InvestContractTemplateTemp>(input), username, tradingProviderId);
            _dbContext.SaveChanges();
            return updateContractTemplateTemp;
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
            var contractTemplateTemp = _investContractTemplateTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId).ThrowIfNull<InvestContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            var contractTemplate = _investContractTemplateEFRepository.Entity.Any(c => c.ContractTemplateTempId == id && c.TradingProviderId == tradingProviderId && c.Deleted == YesNo.NO);
            if (contractTemplate)
            {
                _investContractTemplateEFRepository.ThrowException(ErrorCode.GarnerContractTemplateTempExist);
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
        public InvestContractTemplateTempDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemp = _investContractTemplateTempEFRepository.Entity.FirstOrDefault(x => x.Id == id && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO).ThrowIfNull<InvestContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            return _mapper.Map<InvestContractTemplateTempDto>(contractTemplateTemp);
        }

        /// <summary>
        /// Lấy danh sách mẫu hồ sơ mẫu có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<InvestContractTemplateTemp> FindAllContractTemplateTemp(FilterInvestContractTemplateTempDto input)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var resultPaging = new PagingResult<InvestContractTemplateTemp>();

            var find = _investContractTemplateTempEFRepository.FindAllContractTemplateTemp(input, tradingProviderId);

            resultPaging.Items = find.Items;
            resultPaging.TotalItems = find.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// Tìm danh sách mẫu hợp đồng mẫu không phân trang
        /// </summary>
        /// <param name="contractSource"></param>
        /// <returns></returns>
        public List<InvestContractTemplateTempDto> GetAllContractTemplateTemp(int? contractSource)
        {
            _logger.LogInformation($"{nameof(GetAllContractTemplateTemp)}: contractSource = {contractSource}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemps = _investContractTemplateTempEFRepository.GetAllContractTemplateTemp(contractSource, tradingProviderId);
            return _mapper.Map<List<InvestContractTemplateTempDto>>(contractTemplateTemps);
        }

        public InvestContractTemplateTemp ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplateTemp = _investContractTemplateTempEFRepository.FindById(id, tradingProviderId).ThrowIfNull<InvestContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            var status = ContractTemplateStatus.ACTIVE;
            if (contractTemplateTemp.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var changeStatus = _investContractTemplateTempEFRepository.ChangeStatus(id, status, tradingProviderId);
            _dbContext.SaveChanges();
            return changeStatus;
        }
    }
}

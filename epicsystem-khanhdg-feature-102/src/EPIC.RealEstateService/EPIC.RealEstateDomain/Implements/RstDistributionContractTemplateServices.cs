using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using EPIC.Entities;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstDistributionContractTemplateServices : IRstDistributionContractTemplateServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstDistributionContractTemplateEFRepository _rstDistributionContractTemplateEFRepository;
        private readonly RstContractTemplateTempEFRepository _rstContractTemplateTempEFRepository;
        private readonly RstDistributionPolicyTempEFRepository _rstDistributionPolicyTempEFRepository;
        private readonly RstDistributionPolicyEFRepository _rstDistributionPolicyEFRepository;
        private readonly RstConfigContractCodeDetailEFRepository _rstConfigContractCodeDetailEFRepository;

        public RstDistributionContractTemplateServices(EpicSchemaDbContext dbContext, IMapper mapper,
             DatabaseOptions databaseOptions,
            ILogger<RstProjectServices> logger, 
            IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContext;
            _connectionString = databaseOptions.ConnectionString;
            _rstDistributionContractTemplateEFRepository = new RstDistributionContractTemplateEFRepository(dbContext, logger);
            _rstContractTemplateTempEFRepository = new RstContractTemplateTempEFRepository(dbContext, logger);
            _rstDistributionPolicyTempEFRepository = new RstDistributionPolicyTempEFRepository(dbContext, logger);
            _rstDistributionPolicyEFRepository = new RstDistributionPolicyEFRepository(dbContext, logger);
            _rstConfigContractCodeDetailEFRepository = new RstConfigContractCodeDetailEFRepository(dbContext, logger);
        }

        public RstDistributionContractTemplateDto Add(CreateRstDistributionContractTemplateDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            var distributionContractTemplateInsert = _rstDistributionContractTemplateEFRepository.Add(_mapper.Map<RstDistributionContractTemplate>(input),username, partnerId);
            _dbContext.SaveChanges();
            return _mapper.Map<RstDistributionContractTemplateDto>(distributionContractTemplateInsert);
        }

        public void Delete(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Delete)} : id = {id}, partnerId = {partnerId}, username = {username}");

            var distributionContractTemplate = _rstDistributionContractTemplateEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionContractTemplateNotFound);
            distributionContractTemplate.Deleted = YesNo.YES;
            distributionContractTemplate.ModifiedBy = username;
            distributionContractTemplate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        public PagingResult<RstDistributionContractTemplateDto> FindAll(FilterRstDistributionContractTemplateDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var tra = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            var result = new PagingResult<RstDistributionContractTemplateDto>();

            var listDistributionContractTemplate = _rstDistributionContractTemplateEFRepository.FindAll(input, input.DistributionId, partnerId);

            result.TotalItems = listDistributionContractTemplate.TotalItems;
            result.Items = _mapper.Map<List<RstDistributionContractTemplateDto>>(listDistributionContractTemplate.Items);
            foreach (var contractTemplate in result.Items)
            {
                var contractTemplateTemp = _rstContractTemplateTempEFRepository.FindById(contractTemplate.ContractTemplateTempId);
                if (contractTemplateTemp != null){
                    contractTemplate.ContractTemplateTempName = contractTemplateTemp.Name;
                    contractTemplate.ContractType = contractTemplateTemp.ContractType;
                }
                contractTemplate.DistributionPolicyName = _rstDistributionPolicyEFRepository.FindById(contractTemplate.DistributionPolicyId)?.Name;
                var configContractCodes = _rstConfigContractCodeDetailEFRepository.Entity.Where(e => e.ConfigContractCodeId == contractTemplate.ConfigContractCodeId);
                if (configContractCodes.Any())
                {
                    contractTemplate.ConfigContractCodes = _mapper.Map<List<RstConfigContractCodeDetailDto>>(configContractCodes).OrderBy(e => e.SortOrder).ToList();
                }
            }
            return result;
        }

        public RstDistributionContractTemplateDto FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(FindById)} : id = {id}, partnerId = {partnerId}, username = {username}");

            var distributionContractTemplate = _rstDistributionContractTemplateEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionContractTemplateNotFound);
            var contractTemplate = new RstDistributionContractTemplateDto();
            contractTemplate = _mapper.Map<RstDistributionContractTemplateDto>(distributionContractTemplate);
            contractTemplate.ContractTemplateTempName = _rstContractTemplateTempEFRepository.Entity.FirstOrDefault(e => e.Id == contractTemplate.ContractTemplateTempId)?.Name;
            contractTemplate.DistributionPolicyName = _rstDistributionPolicyEFRepository.FindById(contractTemplate.DistributionPolicyId)?.Name;
            var configContractCodes = _rstConfigContractCodeDetailEFRepository.Entity.Where(e => e.ConfigContractCodeId == contractTemplate.ConfigContractCodeId);
            if (configContractCodes.Any())
            {
                contractTemplate.ConfigContractCodes = _mapper.Map<List<RstConfigContractCodeDetailDto>>(configContractCodes).OrderBy(e => e.SortOrder).ToList();
            }
            return contractTemplate ;
        }

        public RstDistributionContractTemplateDto Update(UpdateRstDistributionContractTemplateDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            var distributionContractTemplate = _rstDistributionContractTemplateEFRepository.FindById(input.Id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionContractTemplateNotFound);
            _rstDistributionContractTemplateEFRepository.Update(_mapper.Map<RstDistributionContractTemplate>(input), username, partnerId);
            _dbContext.SaveChanges();
            return _mapper.Map<RstDistributionContractTemplateDto>(distributionContractTemplate);
        }

        public void ChangeStatus(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)} : id = {id}, partnerId = {partnerId}, username = {username}");

            var distributionContractTemplate = _rstDistributionContractTemplateEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionContractTemplateNotFound);

            if (distributionContractTemplate.Status == Status.ACTIVE)
            {
                distributionContractTemplate.Status = Status.INACTIVE;
            }
            else
            {
                distributionContractTemplate.Status = Status.ACTIVE;
            }
            distributionContractTemplate.ModifiedBy = username;
            distributionContractTemplate.ModifiedDate = DateTime.Now;

            _dbContext.SaveChanges();
        }
    }
}

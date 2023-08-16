using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOpenSellContractTemplateServices : IRstOpenSellContractTemplateServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstOpenSellContractTemplateEFRepository _rstOpenSellContractTemplateEFRepository;
        private readonly RstContractTemplateTempEFRepository _rstContractTemplateTempEFRepository;
        private readonly RstDistributionPolicyTempEFRepository _rstDistributionPolicyTempEFRepository;
        private readonly RstConfigContractCodeDetailEFRepository _rstConfigContractCodeDetailEFRepository;
        private readonly RstSellingPolicyEFRepository _rstSellingPolicyEFRepository;
        private readonly RstDistributionPolicyEFRepository _rstDistributionPolicyEFRepository;
        private readonly RstSellingPolicyTempEFRepository _rstSellingPolicyTempEFRepository;

        public RstOpenSellContractTemplateServices(EpicSchemaDbContext dbContext, IMapper mapper,
             DatabaseOptions databaseOptions,
            ILogger<RstProjectServices> logger,
            IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContext;
            _connectionString = databaseOptions.ConnectionString;
            _rstOpenSellContractTemplateEFRepository = new RstOpenSellContractTemplateEFRepository(dbContext, logger);
            _rstContractTemplateTempEFRepository = new RstContractTemplateTempEFRepository(dbContext, logger);
            _rstDistributionPolicyTempEFRepository = new RstDistributionPolicyTempEFRepository(dbContext, logger);
            _rstConfigContractCodeDetailEFRepository = new RstConfigContractCodeDetailEFRepository(dbContext, logger);
            _rstSellingPolicyEFRepository = new RstSellingPolicyEFRepository(dbContext, logger);
            _rstSellingPolicyTempEFRepository = new RstSellingPolicyTempEFRepository(dbContext, logger);
            _rstDistributionPolicyEFRepository = new RstDistributionPolicyEFRepository(dbContext, logger);
        }

        public RstOpenSellContractTemplate Add(CreateRstOpenSellContractTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");
            _rstContractTemplateTempEFRepository.FindById(input.ContractTemplateTempId).ThrowIfNull(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
            var inputInsert = _mapper.Map<RstOpenSellContractTemplate>(input);
            var contractTemplateTemp = _rstContractTemplateTempEFRepository.Entity.FirstOrDefault(e => e.Id == inputInsert.ContractTemplateTempId && e.Deleted == YesNo.NO);

            var openSellContracts = _rstOpenSellContractTemplateEFRepository.Entity.Where(o => o.OpenSellId == inputInsert.OpenSellId && o.Status == Status.ACTIVE);

            var query = from openSellContract in openSellContracts
                        join contractTemp in _dbContext.RstContractTemplateTemps on openSellContract.ContractTemplateTempId equals contractTemp.Id
                        where contractTemp.Status == Status.ACTIVE
                        && ((contractTemplateTemp.PartnerId != null && contractTemp.TradingProviderId != null) || ((contractTemplateTemp.TradingProviderId != null && contractTemp.PartnerId != null)))
                        select openSellContract;
            foreach (var item in query)
            {
                item.Status = Status.INACTIVE;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }

            inputInsert.TradingProviderId = tradingProviderId;
            inputInsert.CreatedBy = username;
            var openSellContractTemplateInsert = _rstOpenSellContractTemplateEFRepository.Add(inputInsert);
            _dbContext.SaveChanges();
            return openSellContractTemplateInsert;
        }

        public void Update(UpdateRstOpenSellContractTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");
            _rstContractTemplateTempEFRepository.FindById(input.ContractTemplateTempId).ThrowIfNull(_dbContext, ErrorCode.RstContractTemplateTempNotFound);

            var inputUpdate = _mapper.Map<RstOpenSellContractTemplate>(input);
            inputUpdate.TradingProviderId = tradingProviderId;
            inputUpdate.ModifiedBy = username;
            _rstOpenSellContractTemplateEFRepository.Update(inputUpdate);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Delete)} : id = {id}, tradingProviderId = {tradingProviderId}, username = {username}");

            var openSellContractTemplate = _rstOpenSellContractTemplateEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellContractTemplateNotFound);
            openSellContractTemplate.Deleted = YesNo.YES;
            openSellContractTemplate.ModifiedBy = username;
            openSellContractTemplate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        public PagingResult<RstOpenSellContractTemplateDto> FindAll(FilterRstOpenSellContractTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)} : input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var result = new PagingResult<RstOpenSellContractTemplateDto>();

            var openSellContractTemplateQuery = _rstOpenSellContractTemplateEFRepository.FindAll(input, tradingProviderId);

            result.TotalItems = openSellContractTemplateQuery.TotalItems;
            result.Items = _mapper.Map<List<RstOpenSellContractTemplateDto>>(openSellContractTemplateQuery.Items);
            foreach (var contractTemplate in result.Items)
            {
                var contractTemplateTempFind = _rstContractTemplateTempEFRepository.FindById(contractTemplate.ContractTemplateTempId).ThrowIfNull(_dbContext, ErrorCode.RstContractTemplateTempNotFound);
                contractTemplate.ContractSource = contractTemplateTempFind.TradingProviderId != null ? RstOpenSellContractTypes.TRADING_PROVIDER : RstOpenSellContractTypes.PARTNER;
                contractTemplate.ContractTemplateTempName = contractTemplateTempFind?.Name;
                var distributionPolicy = _rstDistributionPolicyEFRepository.FindById(contractTemplate.DistributionPolicyId);
                if (distributionPolicy != null )
                {
                    contractTemplate.DistributionPolicyName = distributionPolicy.Name;
                    contractTemplate.RstDistributionPolicy = _mapper.Map<RstDistributionPolicyDto>(distributionPolicy);
                }
                var configContractCodes = _rstConfigContractCodeDetailEFRepository.Entity.Where(e => e.ConfigContractCodeId == contractTemplate.ConfigContractCodeId);
                if (configContractCodes.Any())
                {
                    contractTemplate.ConfigContractCodes = _mapper.Map<List<RstConfigContractCodeDetailDto>>(configContractCodes).OrderBy(e => e.SortOrder).ToList();
                }
                if (contractTemplateTempFind != null)
                {
                    contractTemplate.RstContractTemplateTemp = _mapper.Map<RstContractTemplateTempDto>(contractTemplateTempFind);
                    contractTemplate.TradingProviderId = contractTemplateTempFind.TradingProviderId;
                    contractTemplate.PartnerId = contractTemplateTempFind.PartnerId;
                }
            }
            return result;
        }

        public RstOpenSellContractTemplateDto FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(FindById)} : id = {id}, tradingProviderId = {tradingProviderId}, username = {username}");
            var contractTemplate = new RstOpenSellContractTemplateDto();
            var distributionContractTemplate = _rstOpenSellContractTemplateEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellContractTemplateNotFound);
            contractTemplate = _mapper.Map<RstOpenSellContractTemplateDto>(distributionContractTemplate);
            var distributionPolicy = _rstDistributionPolicyEFRepository.FindById(contractTemplate.DistributionPolicyId);
            if (distributionPolicy != null)
            {
                contractTemplate.DistributionPolicyName = distributionPolicy.Name;
                contractTemplate.RstDistributionPolicy = _mapper.Map<RstDistributionPolicyDto>(distributionPolicy);
            }

            var configContractCodes = _rstConfigContractCodeDetailEFRepository.Entity.Where(e => e.ConfigContractCodeId == contractTemplate.ConfigContractCodeId);
            if (configContractCodes.Any())
            {
                contractTemplate.ConfigContractCodes = _mapper.Map<List<RstConfigContractCodeDetailDto>>(configContractCodes).OrderBy(e => e.SortOrder).ToList();
            }
            return contractTemplate;
        }

        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        public void ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)} : id = {id}, tradingProviderId = {tradingProviderId}, username = {username}");

            var openSellContractTemplate = _rstOpenSellContractTemplateEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellContractTemplateNotFound);

            if (openSellContractTemplate.Status == Status.ACTIVE)
            {
                openSellContractTemplate.Status = Status.INACTIVE;
            }
            else if (openSellContractTemplate.Status == Status.INACTIVE)
            {
                openSellContractTemplate.Status = Status.ACTIVE;

                var contractTemplateTemp = _rstContractTemplateTempEFRepository.Entity.FirstOrDefault(e => e.Id == openSellContractTemplate.ContractTemplateTempId && e.Deleted == YesNo.NO);

                var openSellContracts = _rstOpenSellContractTemplateEFRepository.Entity.Where(o => o.OpenSellId == openSellContractTemplate.OpenSellId && o.Status == Status.ACTIVE);

                var query = from openSellContract in openSellContracts
                            join contractTemp in _dbContext.RstContractTemplateTemps on openSellContract.ContractTemplateTempId equals contractTemp.Id
                            where contractTemp.Status == Status.ACTIVE
                            && ((contractTemplateTemp.PartnerId != null && contractTemp.TradingProviderId != null) || ((contractTemplateTemp.TradingProviderId != null && contractTemp.PartnerId != null)))
                            select openSellContract;

                foreach (var item in query)
                {
                    item.Status = Status.INACTIVE;
                    item.ModifiedBy = username;
                    item.ModifiedDate = DateTime.Now;
                }
               
            }
            openSellContractTemplate.ModifiedBy = username;
            openSellContractTemplate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}

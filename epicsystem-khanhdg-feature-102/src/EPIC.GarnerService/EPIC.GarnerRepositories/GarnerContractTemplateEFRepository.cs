using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerContractTemplateEFRepository : BaseEFRepository<GarnerContractTemplate>
    {
        public GarnerContractTemplateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerContractTemplate.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hợp đồng cho chính sách
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerContractTemplate Add(GarnerContractTemplate input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập nhật mẫu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerContractTemplate Update(GarnerContractTemplate input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            var contractTemplate = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (contractTemplate != null)
            {
                contractTemplate.DisplayType = input.DisplayType;
                contractTemplate.ContractSource = input.ContractSource;
                contractTemplate.ConfigContractId = input.ConfigContractId;
                contractTemplate.ContractTemplateTempId = input.ContractTemplateTempId;
                contractTemplate.PolicyId = input.PolicyId;
                contractTemplate.StartDate = input.StartDate;
                contractTemplate.ModifiedBy = username;
                contractTemplate.ModifiedDate = DateTime.Now;
            }
            return contractTemplate;
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerContractTemplate FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}, tradingproviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerContractTemplate> FindAll(int policyId, int? tradingProviderId = null, string contractTemplateType = null, int? contractType = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: policyId = {policyId}, tradingproviderId = {tradingProviderId}, contractTemplateType = {contractTemplateType}");

            PagingResult<GarnerPolicyTemp> result = new();
            var contractTemplates = _dbSet.Where(pt => pt.PolicyId == policyId && (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO && pt.Status == Status.ACTIVE).ToList();
            return contractTemplates;
        }

        public PagingResult<GarnerContractTemplateDto> FindAllContractTemplate(GarnerContractTemplateFilterDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplate)}: input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingProviderId}");

            //var contractTemplates = _dbSet.Where(pt => (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO).OrderByDescending(o => o.Id).ToList();
            var contractTemplates = (from contractTemplate in _dbSet
                                    join policy in _epicSchemaDbContext.GarnerPolicies on contractTemplate.PolicyId equals policy.Id
                                    join contractTemplateTemp in _epicSchemaDbContext.GarnerContractTemplateTemps on contractTemplate.ContractTemplateTempId equals contractTemplateTemp.Id
                                    where ((tradingProviderId == null || contractTemplate.TradingProviderId == tradingProviderId) && contractTemplate.Deleted == YesNo.NO && policy.DistributionId == input.DistributionId)
                                    && policy.Deleted == YesNo.NO && contractTemplateTemp.Deleted == YesNo.NO
                                    && (input.PolicyId == null || contractTemplate.PolicyId == input.PolicyId)
                                    && (input.ContractSource == null || contractTemplate.ContractSource == input.ContractSource)
                                    && (input.Status == null || contractTemplate.Status == input.Status)
                                    select new GarnerContractTemplateDto
                                    {
                                        Id = contractTemplate.Id,
                                        PolicyId = contractTemplate.PolicyId,
                                        ConfigContractId = contractTemplate.ConfigContractId,
                                        ContractSource = contractTemplate.ContractSource,
                                        ContractTemplateTempId = contractTemplate.ContractTemplateTempId,
                                        DisplayType = contractTemplate.DisplayType,
                                        StartDate = contractTemplate.StartDate,
                                        Status = contractTemplate.Status
                                    }).OrderByDescending(o => o.Id).ToList();


            if (input.PageSize != -1)
            {
                contractTemplates = contractTemplates.Skip(input.Skip).Take(input.PageSize).ToList();
            }
            return new PagingResult<GarnerContractTemplateDto>
            {
                Items = contractTemplates,
                TotalItems = contractTemplates.Count()
            };
        }

        public List<GarnerContractTemplateDto> FindAllContractTemplateActive(int distributionId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateActive)}: distributionId = {distributionId}, tradingproviderId = {tradingProviderId}");

            var contractTemplates = (from contractTemplate in _dbSet
                                     join policy in _epicSchemaDbContext.GarnerPolicies on contractTemplate.PolicyId equals policy.Id
                                     join contractTemplateTemp in _epicSchemaDbContext.GarnerContractTemplateTemps on contractTemplate.ContractTemplateTempId equals contractTemplateTemp.Id
                                     where ((tradingProviderId == null || contractTemplate.TradingProviderId == tradingProviderId) && contractTemplate.Deleted == YesNo.NO && policy.DistributionId == distributionId)
                                     && policy.Deleted == YesNo.NO && contractTemplateTemp.Deleted == YesNo.NO && contractTemplate.Status == Status.ACTIVE
                                     select new GarnerContractTemplateDto
                                     {
                                         Id = contractTemplate.Id,
                                         PolicyId = contractTemplate.PolicyId,
                                         ConfigContractId = contractTemplate.ConfigContractId,
                                         ContractSource = contractTemplate.ContractSource,
                                         ContractTemplateTempId = contractTemplate.ContractTemplateTempId,
                                         DisplayType = contractTemplate.DisplayType,
                                         StartDate = contractTemplate.StartDate,
                                         Status = contractTemplate.Status
                                     }).OrderByDescending(o => o.Id).ToList();
            return contractTemplates;
        }
        /// <summary>
        /// Tìm kiếm danh sách mẫu hợp đồng thông tin mẫu hợp đồng mẫu theo chính sách 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="type">Dành cho nhà đầu tư cá nhân hay chuyên nghiệp</param>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractType">1: Hợp đồng rút vốn; 2: Hợp đồng đầu tư</param>
        /// <param name="displayType">Hiển thị trước hay sau khi duyệt</param>
        /// <param name="contractSource">Online, offline</param>
        /// <returns></returns>
        public List<GarnerContractTemplateForOrderDto> FindAllForUpdateContractFile(int policyId, string type, int? tradingProviderId = null, int? contractType = null, string displayType = null, int? contractSource = null, string status = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: policyId = {policyId}, tradingproviderId = {tradingProviderId}, contractType = {contractType}, displayType = {displayType}, contractSource = {contractSource}, type = {type}");

            var contractTemplates = from ct in _dbSet
                                    join ctt in _epicSchemaDbContext.GarnerContractTemplateTemps
                                    on ct.ContractTemplateTempId equals ctt.Id
                                    where (ct.PolicyId == policyId && (tradingProviderId == null || ct.TradingProviderId == tradingProviderId) && ct.Deleted == YesNo.NO && (status == null || ct.Status == status)
                                    && (contractType == null || ctt.ContractType == contractType)
                                    && (contractSource == null || (ctt.ContractSource == contractSource || ctt.ContractSource == ContractSources.ALL))
                                    && (displayType == null || ct.DisplayType == displayType))
                                    select new GarnerContractTemplateForOrderDto
                                    {
                                        PolicyId = ct.PolicyId,
                                        ContractSource = ctt.ContractSource,
                                        TradingProviderId = ct.TradingProviderId,
                                        ContractTemplateTempId = ctt.Id,
                                        ContractTemplateUrl = type == SharedContractTemplateType.INVESTOR ?  ctt.FileInvestor : ctt.FileBusinessCustomer,
                                        ContractType = ctt.ContractType,
                                        DisplayType = ct.DisplayType,
                                        Name = ctt.Name,
                                        FileBusinessCustomer = ctt.FileBusinessCustomer,
                                        FileInvestor = ctt.FileInvestor,
                                        ConfigContractId = ct.ConfigContractId,
                                        StartDate = ct.StartDate,
                                        Id = ct.Id,
                                        Status = ct.Status,
                                        CreatedDate = ct.CreatedDate
                                    };
            return contractTemplates.ToList();
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng đặt lệnh theo chính sách 
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <param name="type">Dành cho nhà đầu tư cá nhân hay chuyên nghiệp</param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerContractTemplateForOrderDto FindByIdForUpdateContractFile(int contractTemplateId, string type, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: contractTemplateId = {contractTemplateId}, type = {type}, tradingproviderId = {tradingProviderId}");

            var contractTemplates = from ct in _dbSet
                                    join ctt in _epicSchemaDbContext.GarnerContractTemplateTemps
                                    on ct.ContractTemplateTempId equals ctt.Id
                                    where (ct.Id == contractTemplateId && (tradingProviderId == null || ct.TradingProviderId == tradingProviderId) && ct.Deleted == YesNo.NO)
                                    select new GarnerContractTemplateForOrderDto
                                    {
                                        PolicyId = ct.PolicyId,
                                        ContractSource = ctt.ContractSource,
                                        ContractTemplateUrl = type == SharedContractTemplateType.INVESTOR ? ctt.FileInvestor : ctt.FileBusinessCustomer,
                                        TradingProviderId = ct.TradingProviderId,
                                        ContractTemplateTempId = ctt.Id,
                                        ContractType = ctt.ContractType,
                                        DisplayType = ct.DisplayType,
                                        Name = ctt.Name,
                                        FileBusinessCustomer = ctt.FileBusinessCustomer,
                                        FileInvestor = ctt.FileInvestor,
                                        ConfigContractId = ct.ConfigContractId,
                                        StartDate = ct.StartDate,
                                        Id = ct.Id,
                                        Status = ct.Status,
                                    };
            return contractTemplates.FirstOrDefault();
        }
    }
}

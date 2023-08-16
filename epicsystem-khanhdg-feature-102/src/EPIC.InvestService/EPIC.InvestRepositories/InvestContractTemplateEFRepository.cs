
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplate;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.InvestRepositories
{
    public class InvestContractTemplateEFRepository : BaseEFRepository<InvestContractTemplate>
    {
        public InvestContractTemplateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestContractTemplate.SEQ}")
        {
        }


        /// <summary>
        /// Tìm kiếm mẫu hợp đồng theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<InvestContractTemplate> FindAll(int policyId, int? tradingProviderId = null, string contractTemplateType = null, int? contractType = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: policyId = {policyId}, tradingproviderId = {tradingProviderId}, contractTemplateType = {contractTemplateType}");

            var contractTemplates = _dbSet.Where(pt => pt.PolicyId == policyId && (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO && pt.Status == Status.ACTIVE).ToList();
            return contractTemplates;
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public InvestContractTemplate FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}, tradingproviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thêm hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public InvestContractTemplate Add(InvestContractTemplate input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập nhật mẫu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public InvestContractTemplate Update(InvestContractTemplate input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            var contractTemplate = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (contractTemplate != null)
            {
                contractTemplate.DisplayType = input.DisplayType;
                contractTemplate.ContractSource = input.ContractSource;
                contractTemplate.ConfigContractId = input.ConfigContractId;
                contractTemplate.ContractTemplateTempId = input.ContractTemplateTempId;
                contractTemplate.StartDate = input.StartDate;
                contractTemplate.ModifiedBy = username;
                contractTemplate.ModifiedDate = DateTime.Now;
            }
            return contractTemplate;
        }

        public PagingResult<ContractTemplateDto> FindAllContractTemplate(ContractTemplateTempFilterDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplate)}: input = {JsonSerializer.Serialize(input)}, tradingProvider = {tradingProviderId}");
            var contractTemplates = (from ct in _dbSet
                                    join pl in _epicSchemaDbContext.InvestPolicies on ct.PolicyId equals pl.Id
                                    join contractTemplateTemp in _epicSchemaDbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals contractTemplateTemp.Id
                                    where ((tradingProviderId == null || ct.TradingProviderId == tradingProviderId) && ct.Deleted == YesNo.NO && pl.DistributionId == input.DistributionId)
                                    && pl.Deleted == YesNo.NO && contractTemplateTemp.Deleted == YesNo.NO
                                    && (input.PolicyId == null || ct.PolicyId == input.PolicyId)
                                    && (input.ContractSource == null || ct.ContractSource == input.ContractSource)
                                    && (input.Status == null || ct.Status == input.Status)
                                    && (input.Keyword == null || contractTemplateTemp.Name.ToLower().Contains(input.Keyword.ToLower()))
                                    && (input.ContractType == null || contractTemplateTemp.ContractType == input.ContractType)
                                    select new ContractTemplateDto
                                    {
                                        Id = ct.Id,
                                        PolicyId = ct.PolicyId,
                                        ConfigContractId = ct.ConfigContractId,
                                        ContractSource = ct.ContractSource,
                                        ContractTemplateTempId = ct.ContractTemplateTempId,
                                        DisplayType = ct.DisplayType,
                                        StartDate = ct.StartDate,
                                        Status = ct.Status
                                    }).OrderByDescending(o => o.Id).ToList();
            var result = new PagingResult<ContractTemplateDto>();
            result.TotalItems = contractTemplates.Count();
            if (input.PageSize != -1)
            {
                contractTemplates = contractTemplates.Skip(input.Skip).Take(input.PageSize).ToList();
            }
            result.Items = contractTemplates;
            return result;
        }

        public List<ContractTemplateDto> FindAllContractTemplateActive(int distributionId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateActive)}: distributionId = {distributionId}, tradingProvider = {tradingProviderId}");
            var contractTemplates = (from ct in _dbSet
                                     join pl in _epicSchemaDbContext.InvestPolicies on ct.PolicyId equals pl.Id
                                     join contractTemplateTemp in _epicSchemaDbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals contractTemplateTemp.Id
                                     where ((tradingProviderId == null || ct.TradingProviderId == tradingProviderId) && ct.Deleted == YesNo.NO && pl.DistributionId == distributionId)
                                     && pl.Deleted == YesNo.NO && contractTemplateTemp.Deleted == YesNo.NO && ct.Status == Status.ACTIVE
                                     select new ContractTemplateDto
                                     {
                                         Id = ct.Id,
                                         PolicyId = ct.PolicyId,
                                         ConfigContractId = ct.ConfigContractId,
                                         ContractSource = ct.ContractSource,
                                         ContractTemplateTempId = ct.ContractTemplateTempId,
                                         DisplayType = ct.DisplayType,
                                         StartDate = ct.StartDate,
                                         Status = ct.Status
                                     }).OrderByDescending(o => o.Id).ToList();
            return contractTemplates;
        }

        /// <summary>
        /// Tìm kiếm danh sách mẫu hợp đồng thông tin mẫu hợp đồng mẫu theo chính sách 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="type">Dành cho nhà đầu tư cá nhân hay chuyên nghiệp</param>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractType">1: Hợp đồng rút vốn; 2: Hợp đồng đầu tư; 3: Hợp đồng tái tục</param>
        /// <param name="displayType">Hiển thị trước hay sau khi duyệt</param>
        /// <param name="contractSource">Online, offline (lấy loại tất cả)</param>
        /// <returns></returns>
        public List<ContractTemplateForUpdateContractDto> FindAllForUpdateContractFile(int policyId, string type, int? tradingProviderId = null, int? contractType = null, string displayType = null, int? contractSource = null, string status = null)
        {
            _logger.LogInformation($"{nameof(FindAllForUpdateContractFile)}: policyId = {policyId}, tradingproviderId = {tradingProviderId}, contractType = {contractType}, displayType = {displayType}, contractSource = {contractSource}, type = {type}");

            var contractTemplates = from ct in _dbSet
                                    join ctt in _epicSchemaDbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id into contracTempTemps
                                    from contractTemp in contracTempTemps.DefaultIfEmpty()
                                    where (ct.PolicyId == policyId && (tradingProviderId == null || ct.TradingProviderId == tradingProviderId) && ct.Deleted == YesNo.NO && (status == null || ct.Status == status)
                                    && (contractType == null || (ct.ContractType == null && contractTemp.ContractType == contractType) || (ct.ContractType != null && ct.ContractType == contractType))
                                    && (contractSource == null || (contractTemp.ContractSource == contractSource || contractTemp.ContractSource == ContractSources.ALL) || (ct.ContractSource == contractSource || ct.ContractSource == ContractSources.ALL))
                                    && (displayType == null || ct.DisplayType == displayType))
                                    select new ContractTemplateForUpdateContractDto
                                    {
                                        PolicyId = ct.PolicyId,
                                        ContractSource = ct.ContractSource,
                                        TradingProviderId = ct.TradingProviderId,
                                        ContractTemplateTempId = ct.ContractTemplateTempId,
                                        ContractTemplateUrl = type == SharedContractTemplateType.INVESTOR ? (ct.ContractTemplateTempId == null ? ct.FileUploadInvestorUrl : contractTemp.FileInvestor) : (ct.ContractTemplateTempId == null ? ct.FileUploadBusinessCustomerUrl : contractTemp.FileBusinessCustomer),
                                        ContractType = ct.ContractType ?? contractTemp.ContractType,
                                        DisplayType = ct.DisplayType,
                                        Name = ct.ContractTemplateTempId == null ? ct.FileUploadName : contractTemp.Name,
                                        FileBusinessCustomer = ct.ContractTemplateTempId == null ? ct.FileUploadBusinessCustomerUrl : contractTemp.FileBusinessCustomer,
                                        FileInvestor = ct.ContractTemplateTempId == null ? ct.FileUploadInvestorUrl : contractTemp.FileInvestor,
                                        ConfigContractId = ct.ConfigContractId,
                                        StartDate = ct.StartDate,
                                        Id = ct.Id,
                                        Status = ct.Status,
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
                                    join ctt in _epicSchemaDbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id into contracTempTemps
                                    from contractTemp in contracTempTemps.DefaultIfEmpty()
                                    where (ct.Id == contractTemplateId && (tradingProviderId == null || ct.TradingProviderId == tradingProviderId) && ct.Deleted == YesNo.NO)
                                    select new GarnerContractTemplateForOrderDto
                                    {
                                        PolicyId = ct.PolicyId,
                                        ContractSource = ct.ContractSource,
                                        ContractTemplateUrl = type == SharedContractTemplateType.INVESTOR ? (ct.ContractTemplateTempId == null ? ct.FileUploadInvestorUrl : contractTemp.FileInvestor) : (ct.ContractTemplateTempId == null ? ct.FileUploadBusinessCustomerUrl : contractTemp.FileBusinessCustomer),
                                        TradingProviderId = ct.TradingProviderId,
                                        ContractTemplateTempId = ct.ContractTemplateTempId,
                                        ContractType = ct.ContractType ?? contractTemp.ContractType,
                                        DisplayType = ct.DisplayType,
                                        Name = ct.ContractTemplateTempId == null ? ct.FileUploadName : contractTemp.Name,
                                        FileBusinessCustomer = ct.ContractTemplateTempId == null ? ct.FileUploadBusinessCustomerUrl : contractTemp.FileBusinessCustomer,
                                        FileInvestor = ct.ContractTemplateTempId == null ? ct.FileUploadInvestorUrl : contractTemp.FileInvestor,
                                        ConfigContractId = ct.ConfigContractId,
                                        StartDate = ct.StartDate,
                                        Id = ct.Id,
                                        Status = ct.Status,
                                    };
            return contractTemplates.FirstOrDefault();
        }

        //public int UpdateStatus(int id, string status, string modifiedBy)
        //{
        //    return _oracleHelper.ExecuteProcedureNonQuery(
        //            PROC_CHANGE_STATUS, new
        //            {
        //                pv_ID = id,
        //                pv_STATUS = status,
        //                SESSION_USERNAME = modifiedBy
        //            }, false);
        //}

        /// <summary>
        /// Sinh mã hợp đồng
        /// </summary>
        /// <returns></returns>
        public string GenContractCode(OrderContractCodeDto input)
        {
            List<ConfigContractCodeDto> configContractCodes = new();
            var configContractCodeDetails = _epicSchemaDbContext.InvestConfigContractCodeDetails.Where(d => d.ConfigContractCodeId == input.ConfigContractCodeId).OrderBy(o => o.SortOrder);

            string contractCode = null;
            foreach (var item in configContractCodeDetails)
            {
                string value = null;
                if (item.Key == ConfigContractCode.ORDER_ID)
                {
                    value = input.OrderId.ToString();
                }
                if (item.Key == ConfigContractCode.ORDER_ID_PREFIX_0)
                {
                    value = input.OrderId?.ToString().PadLeft(8, '0');
                }
                else if (item.Key == ConfigContractCode.PRODUCT_TYPE)
                {
                    value = input.ProductType.ToUnSign()?.ToUpper();
                }
                else if (item.Key == ConfigContractCode.PRODUCT_CODE)
                {
                    value = input.ProductCode.ToUnSign()?.ToUpper();
                }
                else if (item.Key == ConfigContractCode.PRODUCT_NAME)
                {
                    value = input.ProductName.ToUnSign()?.ToUpper();
                }
                else if (item.Key == ConfigContractCode.POLICY_NAME)
                {
                    value = input.PolicyName.ToUnSign()?.ToUpper();
                }
                else if (item.Key == ConfigContractCode.POLICY_CODE)
                {
                    value = input.PolicyCode.ToUnSign()?.ToUpper();
                }
                else if (item.Key == ConfigContractCode.SHORT_NAME)
                {
                    value = input.ShortName.FirstLetterEachWord().ToUnSign()?.ToUpper() ?? input.ShortNameBusiness.ToUnSign()?.ToUpper();
                }
                else if (item.Key == ConfigContractCode.FIX_TEXT)
                {
                    value = item.Value.ToUnSign()?.ToUpper();
                }
                else if (item.Key == ConfigContractCode.DATE_DD && input.Now != null)
                {
                    value = input.Now.Value.ToString("dd");
                }
                else if (item.Key == ConfigContractCode.DATE_MM && input.Now != null)
                {
                    value = input.Now.Value.ToString("MM");
                }
                else if (item.Key == ConfigContractCode.DATE_YY && input.Now != null)
                {
                    value = input.Now.Value.ToString("yy");
                }
                else if (item.Key == ConfigContractCode.DATE_YYYY && input.Now != null)
                {
                    value = input.Now.Value.ToString("yyyy");
                }
                else if (item.Key == ConfigContractCode.DATE_DD_MM_YYYY && input.Now != null)
                {
                    value = input.Now.Value.ToString("ddMMyyyy");
                }
                else if (item.Key == ConfigContractCode.BUY_DATE && input.BuyDate != null)
                {
                    value = input.BuyDate.Value.ToString("yyyyMMdd");
                }
                else if (item.Key == ConfigContractCode.PAYMENT_FULL_DATE && input.PaymentFullDate != null)
                {
                    value = input.PaymentFullDate.Value.ToString("yyyyMMdd");
                }
                else if (item.Key == ConfigContractCode.INVEST_DATE && input.InvestDate != null)
                {
                    value = input.InvestDate.Value.ToString("yyyyMMdd");
                }
                configContractCodes.Add(new ConfigContractCodeDto { Key = item.Key, Value = value });
            }
            contractCode = ConfigContractCode.GenContractCode(configContractCodes);
            return contractCode;
        }
    }
}

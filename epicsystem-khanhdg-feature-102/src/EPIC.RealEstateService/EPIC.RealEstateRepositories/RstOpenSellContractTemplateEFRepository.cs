using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.Utils.ConstantVariables.Contract;

namespace EPIC.RealEstateRepositories
{
    public class RstOpenSellContractTemplateEFRepository : BaseEFRepository<RstOpenSellContractTemplate>
    {
        public RstOpenSellContractTemplateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOpenSellContractTemplate.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        public RstOpenSellContractTemplate Add(RstOpenSellContractTemplate input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellContractTemplateEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập biểu mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        public RstOpenSellContractTemplate Update(RstOpenSellContractTemplate input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellContractTemplateEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var openSellContractTempalte = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == input.TradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOpenSellContractTemplateNotFound);
            openSellContractTempalte.ContractTemplateTempId = input.ContractTemplateTempId;
            openSellContractTempalte.ConfigContractCodeId = input.ConfigContractCodeId;
            openSellContractTempalte.DisplayType = input.DisplayType;
            openSellContractTempalte.EffectiveDate = input.EffectiveDate;
            openSellContractTempalte.ModifiedBy = input.ModifiedBy;
            openSellContractTempalte.ModifiedDate = DateTime.Now;
            return openSellContractTempalte;
        }

        /// <summary>
        /// Tìm kiếm biểu mẫu hợp đồng
        /// </summary>
        public RstOpenSellContractTemplate FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOpenSellContractTemplateEFRepository)}->{nameof(FindById)}: id = {id}");
            return _dbSet.FirstOrDefault(e => e.Id == id && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm danh sách biểu mẫu hợp đồng
        /// </summary>
        public PagingResult<RstOpenSellContractTemplate> FindAll(FilterRstOpenSellContractTemplateDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOpenSellContractTemplateEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var result = new PagingResult<RstOpenSellContractTemplate>();
            var contractTemplateQuery = from openSellContractTemplate in _dbSet
                                        join contractTemplateTemp in _epicSchemaDbContext.RstContractTemplateTemps on openSellContractTemplate.ContractTemplateTempId equals contractTemplateTemp.Id
                                        where openSellContractTemplate.OpenSellId == input.OpenSellId && openSellContractTemplate.Deleted == YesNo.NO
                                        && (tradingProviderId == null || openSellContractTemplate.TradingProviderId == tradingProviderId)
                                        && (input.ContractTemplateTempName == null || contractTemplateTemp.Name.ToLower().Contains(input.ContractTemplateTempName.ToLower()))
                                        && (input.Status == null || openSellContractTemplate.Status == input.Status)
                                        && (input.ContractType == null || contractTemplateTemp.ContractType == input.ContractType)
                                        select openSellContractTemplate;
            contractTemplateQuery = contractTemplateQuery.OrderByDescending(p => p.Id);
            result.TotalItems = contractTemplateQuery.Count();
            if (input.PageSize != -1)
            {
                contractTemplateQuery = contractTemplateQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = contractTemplateQuery.ToList();
            return result;
        }

        /// <summary>
        /// Tìm kiếm danh sách biểu mẫu hợp đồng
        /// </summary>
        public List<RstContractTemplateTempForOrderDto> FindAllContractTemplateTemp(int openSelllId, string customerType, int? tradingProviderId = null, int? contractType = null, 
                                                                string displayType = null, int? contractSource = null, string status = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionContractTemplateEFRepository)}->{nameof(FindAllContractTemplateTemp)}: openSelllId = {openSelllId}, customerType = {customerType}, tradingProviderId = {tradingProviderId}," +
                $" contractType {contractType}, displayType = {displayType}, contractSource = {contractSource}, status = {status}");

            var contractTemplates = from openSellContractTemplate in _dbSet
                                    join contractTemplateTemp in _epicSchemaDbContext.RstContractTemplateTemps
                                    on openSellContractTemplate.ContractTemplateTempId equals contractTemplateTemp.Id
                                    where (openSellContractTemplate.OpenSellId == openSelllId && (tradingProviderId == null || openSellContractTemplate.TradingProviderId == tradingProviderId) 
                                    && openSellContractTemplate.Deleted == YesNo.NO && (status == null || openSellContractTemplate.Status == status)
                                    && (contractType == null || contractTemplateTemp.ContractType == contractType)
                                    && (contractSource == null || (contractTemplateTemp.ContractSource == contractSource || contractTemplateTemp.ContractSource == ContractSources.ALL))
                                    && (displayType == null || openSellContractTemplate.DisplayType == displayType))
                                    select new RstContractTemplateTempForOrderDto
                                    {
                                        OpenSellId = openSellContractTemplate.OpenSellId,
                                        ContractSource = contractTemplateTemp.ContractSource,
                                        TradingProviderId = contractTemplateTemp.TradingProviderId,
                                        ContractTemplateTempId = contractTemplateTemp.Id,
                                        ContractTemplateUrl = customerType == SharedContractTemplateType.INVESTOR ? contractTemplateTemp.FileInvestor : contractTemplateTemp.FileBusinessCustomer,
                                        ContractType = contractTemplateTemp.ContractType,
                                        DisplayType = openSellContractTemplate.DisplayType,
                                        Name = contractTemplateTemp.Name,
                                        FileBusinessCustomer = contractTemplateTemp.FileBusinessCustomer,
                                        FileInvestor = contractTemplateTemp.FileInvestor,
                                        ConfigContractId = openSellContractTemplate.ConfigContractCodeId,
                                        Id = openSellContractTemplate.Id,
                                        Status = openSellContractTemplate.Status,
                                        CreatedDate = openSellContractTemplate.CreatedDate
                                    };
            return contractTemplates.ToList();
        }
    }
}

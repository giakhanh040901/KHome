using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerPolicyTemp;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.GarnerRepositories
{
    public class GarnerContractTemplateTempEFRepository : BaseEFRepository<GarnerContractTemplateTemp>
    {
        public GarnerContractTemplateTempEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerContractTemplateTemp.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerContractTemplateTemp Add(GarnerContractTemplateTemp input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            return _dbSet.Add(new GarnerContractTemplateTemp()
            {
                Id = (int)NextKey(),
                Name = input.Name,
                ContractType = input.ContractType,
                ContractSource = input.ContractSource,
                FileInvestor = input.FileInvestor,
                FileBusinessCustomer = input.FileBusinessCustomer,
                Description = input.Description,
                TradingProviderId = tradingProviderId,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerContractTemplateTemp Update(GarnerContractTemplateTemp input, string username, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            var contractTemplateTemp = _dbSet.FirstOrDefault(p => p.Id == input.Id && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) && p.Deleted == YesNo.NO);
            if (contractTemplateTemp != null)
            {
                contractTemplateTemp.Name = input.Name;
                contractTemplateTemp.ContractType = input.ContractType;
                contractTemplateTemp.ContractSource = input.ContractSource;
                contractTemplateTemp.FileInvestor = input.FileInvestor;
                contractTemplateTemp.FileBusinessCustomer = input.FileBusinessCustomer;
                contractTemplateTemp.Description = input.Description;
                contractTemplateTemp.ModifiedBy = username;
                contractTemplateTemp.ModifiedDate = DateTime.Now;
            }
            return contractTemplateTemp;
        }

        public GarnerContractTemplateTemp FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}, tradingproviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
        }

        public List<GarnerContractTemplateTemp> FindAll(int policyTempId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: policyTempId = {policyTempId}, tradingproviderId = {tradingProviderId}");

            var contractTemplateTemps = _dbSet.Where(pt => (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO).ToList();
            return contractTemplateTemps;
        }

        public PagingResult<GarnerContractTemplateTemp> FindAllContractTemplateTemp(FilterGarnerContractTemplateTempDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<GarnerContractTemplateTemp> result = new();
            IQueryable<GarnerContractTemplateTemp> contractTemplateTempQuery = _dbSet.Where(r => r.Deleted == YesNo.NO && (tradingProviderId == null || r.TradingProviderId == tradingProviderId)).OrderByDescending(o => o.Id);
            
            if (input.ContractSource == ContractSources.ONLINE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractSource == ContractSources.ONLINE);
            }
            else if (input.ContractSource == ContractSources.OFFLINE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractSource == ContractSources.OFFLINE);
            }

            if(input.Status == Status.ACTIVE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.Status == Status.ACTIVE);
            }
            else if (input.Status == Status.INACTIVE)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.Status == Status.INACTIVE);
            }

            if (input.ContractType == ContractTypes.DAT_LENH)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.DAT_LENH);
            }
            else if (input.ContractType == ContractTypes.RUT_TIEN)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.RUT_TIEN);
            }
            else if (input.ContractType == ContractTypes.TAI_TUC_GOC)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.TAI_TUC_GOC);
            }
            else if (input.ContractType == ContractTypes.RUT_TIEN_APP)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(c => c.ContractType == ContractTypes.RUT_TIEN_APP);
            }

            result.TotalItems = contractTemplateTempQuery.Count();

            if (input.PageSize != -1)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Skip(input.Skip).Take(input.PageSize);
            }

            if (input.Keyword != null)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Where(p => p.Name.Contains(input.Keyword));
            }

            result.Items = contractTemplateTempQuery.ToList();
            return result;
        }

        /// <summary>
        /// Tìm mẫu hợp đồng mẫu không phân trang
        /// </summary>
        /// <param name="contractSource"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerContractTemplateTemp> GetAllContractTemplateTemp(int? contractSource = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: contractSource = {contractSource}, tradingProviderId = {tradingProviderId}");

            var contractTemplateTemps = _dbSet.Where(pt => (contractSource == null || pt.ContractSource == contractSource) && (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO && pt.Status == Status.ACTIVE).OrderByDescending(o => o.Id).ToList();
            return contractTemplateTemps;
        }

        public GarnerContractTemplateTemp ChangeStatus(int id, string status, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, tradingProviderId = {tradingProviderId}");
            var contractTemplateTemp = _dbSet.FirstOrDefault(p => p.Id == id && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) && p.Deleted == YesNo.NO);
            if (contractTemplateTemp != null)
            {
                contractTemplateTemp.Status = status;
            }
            return contractTemplateTemp;
        }
    }
}

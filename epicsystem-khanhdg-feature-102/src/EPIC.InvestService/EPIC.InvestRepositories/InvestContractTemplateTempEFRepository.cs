using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestContractTemplateTempEFRepository : BaseEFRepository<InvestContractTemplateTemp>
    {
        public InvestContractTemplateTempEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestContractTemplateTemp.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public InvestContractTemplateTemp Add(InvestContractTemplateTemp input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            return _dbSet.Add(new InvestContractTemplateTemp()
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
        public InvestContractTemplateTemp Update(InvestContractTemplateTemp input, string username, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
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

        public InvestContractTemplateTemp FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}, tradingproviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
        }

        public PagingResult<InvestContractTemplateTemp> FindAllContractTemplateTemp(FilterInvestContractTemplateTempDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateTemp)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<InvestContractTemplateTemp> result = new();
            IQueryable<InvestContractTemplateTemp> contractTemplateTempQuery = _dbSet.Where(c => c.Deleted == YesNo.NO && (tradingProviderId == null || c.TradingProviderId == tradingProviderId)
            && (input.ContractSource == null || c.ContractSource == input.ContractSource)
            && (input.Status == null || c.Status == input.Status)
            && (input.ContractType == null || c.ContractType == input.ContractType)
            && (input.Keyword == null || c.Name.Contains(input.Keyword))
            );
            result.TotalItems = contractTemplateTempQuery.Count();
            contractTemplateTempQuery = contractTemplateTempQuery.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                contractTemplateTempQuery = contractTemplateTempQuery.Skip(input.Skip).Take(input.PageSize);
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
        public List<InvestContractTemplateTemp> GetAllContractTemplateTemp(int? contractSource = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GetAllContractTemplateTemp)}: contractSource = {contractSource}, tradingProviderId = {tradingProviderId}");

            var contractTemplateTemps = _dbSet.Where(pt => (contractSource == null || pt.ContractSource == contractSource) && (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO && pt.Status == Status.ACTIVE).OrderByDescending(o => o.Id).ToList();
            return contractTemplateTemps;
        }

        public InvestContractTemplateTemp ChangeStatus(int id, string status, int? tradingProviderId = null) 
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

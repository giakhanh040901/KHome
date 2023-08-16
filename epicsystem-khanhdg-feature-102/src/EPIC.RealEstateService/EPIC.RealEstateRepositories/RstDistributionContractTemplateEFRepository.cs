using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstDistributionContractTemplateEFRepository : BaseEFRepository<RstDistributionContractTemplate>
    {
        public RstDistributionContractTemplateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstDistributionContractTemplate.SEQ}")
        {
        }

        /// <summary>
        /// Thêm biểu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstDistributionContractTemplate Add(RstDistributionContractTemplate input, string username, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionContractTemplateEFRepository)}->{nameof(FindById)}: input = {JsonSerializer.Serialize(input)}, username = {username}, parnerId = {partnerId}");

            return _dbSet.Add(new RstDistributionContractTemplate()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                DistributionId = input.DistributionId,
                DistributionPolicyId = input.DistributionPolicyId,
                ConfigContractCodeId = input.ConfigContractCodeId,
                ContractTemplateTempId = input.ContractTemplateTempId,
                Description = input.Description,
                EffectiveDate = input.EffectiveDate,
                CreatedBy = username,
                CreatedDate = DateTime.Now,
            }).Entity;
        }

        /// <summary>
        /// Cập biểu mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        public RstDistributionContractTemplate Update(RstDistributionContractTemplate input, string username, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionContractTemplateEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var distributionContractTempalte = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstDistributionContractTemplateNotFound);

            distributionContractTempalte.ContractTemplateTempId = input.ContractTemplateTempId;
            distributionContractTempalte.ConfigContractCodeId = input.ConfigContractCodeId;
            distributionContractTempalte.DistributionPolicyId = input.DistributionPolicyId;
            distributionContractTempalte.Description = input.Description;
            distributionContractTempalte.EffectiveDate = input.EffectiveDate;
            distributionContractTempalte.ModifiedBy = username;
            distributionContractTempalte.ModifiedDate = DateTime.Now;
            return distributionContractTempalte;
        }

        /// <summary>
        /// Tìm kiếm biểu mẫu hợp đồng
        /// </summary>
        public RstDistributionContractTemplate FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionContractTemplateEFRepository)}->{nameof(FindById)}: id = {id}");

            return _dbSet.FirstOrDefault(e => e.Id == id && (partnerId == null || e.PartnerId == partnerId) && e.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm danh sách biểu mẫu hợp đồng
        /// </summary>
        public PagingResult<RstDistributionContractTemplate> FindAll(FilterRstDistributionContractTemplateDto input,int distributionId, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionContractTemplateEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, distributionId {distributionId}, partnerId = {partnerId}");

            PagingResult<RstDistributionContractTemplate> result = new();
            var contractTemplateQuery = _dbSet.Where(p => p.DistributionId == distributionId && p.Deleted == YesNo.NO && (partnerId == null || p.PartnerId == partnerId)
                                            && (input.Status == null || input.Status == p.Status));

            contractTemplateQuery = contractTemplateQuery.OrderByDescending(p => p.Id);
            result.TotalItems = contractTemplateQuery.Count();
            if (input.PageSize != -1)
            {
                contractTemplateQuery = contractTemplateQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = contractTemplateQuery.ToList();
            return result;
        }
    }
}

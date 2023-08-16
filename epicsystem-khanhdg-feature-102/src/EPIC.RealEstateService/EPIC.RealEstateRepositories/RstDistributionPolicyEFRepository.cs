using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstDistributionPolicyEFRepository : BaseEFRepository<RstDistributionPolicy>
    {
        public RstDistributionPolicyEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstDistributionPolicy.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chính sách
        /// </summary>
        /// <returns></returns>
        public RstDistributionPolicy Add(RstDistributionPolicy input)
        {
            _logger.LogInformation($"{nameof(RstDistributionPolicyEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập nhật chính sách
        /// </summary>
        /// <returns></returns>
        public RstDistributionPolicy Update(RstDistributionPolicy input)
        {
            _logger.LogInformation($"{nameof(RstDistributionPolicyEFRepository)}->{nameof(Update)}:input = {JsonSerializer.Serialize(input)}");

            var distributionPolicy = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.DistributionId == input.DistributionId && p.PartnerId == input.PartnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstProjectDistributionPolicyNotFound);

            distributionPolicy.Name = input.Name;
            distributionPolicy.PaymentType = input.PaymentType;
            distributionPolicy.DepositType = input.DepositType;
            distributionPolicy.DepositValue = input.DepositValue;
            distributionPolicy.LockType = input.LockType;
            distributionPolicy.LockValue = input.LockValue;
            distributionPolicy.Description = input.Description;
            distributionPolicy.ModifiedBy = input.ModifiedBy;
            distributionPolicy.ModifiedDate = DateTime.Now;
            return distributionPolicy;
        }

        /// <summary>
        /// Tìm kiếm chính sách
        /// </summary>
        public RstDistributionPolicy FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstDistributionPolicyEFRepository)}->{nameof(FindById)}: id = {id}");

            return _dbSet.FirstOrDefault(p => p.Id == id && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm danh sách chính sách phân phối
        /// </summary>
        public PagingResult<RstDistributionPolicy> FindAll(FilterRstDistributionPolicyDto input, int DistributionId, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionPolicyEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            PagingResult<RstDistributionPolicy> result = new();
            var policyQuery = _dbSet.Where(p => p.Deleted == YesNo.NO && p.DistributionId == DistributionId && (partnerId == null || p.PartnerId == partnerId)
                                            && (input.Keyword == null || p.Code.ToLower().Contains(input.Keyword.ToLower())
                                            || p.Name.ToLower().Contains(input.Keyword.ToLower()))
                                            && (input.Status == null || input.Status == p.Status));

            policyQuery = policyQuery.OrderByDescending(p => p.Id);
            result.TotalItems = policyQuery.Count();
            if (input.PageSize != -1)
            {
                policyQuery = policyQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = policyQuery.ToList();
            return result;
        }
    }
}

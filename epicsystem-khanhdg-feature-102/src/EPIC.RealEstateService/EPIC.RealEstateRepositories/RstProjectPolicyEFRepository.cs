using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
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
    public class RstProjectPolicyEFRepository : BaseEFRepository<RstProjectPolicy>
    {
        public RstProjectPolicyEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectPolicy.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectPolicy Add(RstProjectPolicy input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProjectPolicyEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            return _dbSet.Add(new RstProjectPolicy()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                ProjectId = input.ProjectId,
                Code = input.Code,
                Name = input.Name,
                Description = input.Description,
                PolicyType = input.PolicyType,
                ConversionValue = input.ConversionValue,
                CreatedDate = DateTime.Now,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstProjectPolicy Update(RstProjectPolicy input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProjectPolicyEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");


            var policy = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (policy != null)
            {
                policy.Code = input.Code;
                policy.Name = input.Name;
                policy.Description = input.Description;
                policy.PolicyType = input.PolicyType;
                policy.ConversionValue = input.ConversionValue;
                policy.Source = input.Source;
                policy.ModifiedBy = username;
                policy.ModifiedDate = DateTime.Now;
            }

            return policy;
        }

        /// <summary>
        /// Tìm kiếm chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectPolicy FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstProjectPolicyEFRepository)}->{nameof(FindById)}: id = {id}");

            return _dbSet.FirstOrDefault(p => p.Id == id && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstProjectPolicy ChangeStatus(int id, string status)
        {
            _logger.LogInformation($"{nameof(RstProjectPolicyEFRepository)}->{nameof(ChangeStatus)}: id = {id}, status = {status}");

            var policy = _dbSet.FirstOrDefault(p => p.Id == id && p.Deleted == YesNo.NO);

            if (policy != null)
            {
                policy.Status = status;
            }

            return policy;
        }

        /// <summary>
        /// Tìm kiếm danh sách chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstProjectPolicy> FindAll(FilterRstProjectPolicyDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectPolicyEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            PagingResult<RstProjectPolicy> result = new();
            IQueryable<RstProjectPolicy> policyQuery = _dbSet.Where(p => p.Deleted == YesNo.NO && (partnerId == null || p.PartnerId == partnerId)
                                                                   && (input.Status == null || p.Status == input.Status)
                                                                   && (input.Name == null || p.Name.Contains(input.Name))
                                                                   && (input.Code == null || p.Code.Contains(input.Code))
                                                                   && (p.ProjectId == input.ProjectId)
                                                                   && (input.PolicyType == null || p.PolicyType == input.PolicyType)
                                                                   && (input.Source == null || p.Source == input.Source)
                                                                   && (input.Keyword == null || (p.Name.Contains(input.Keyword) || p.Code.Contains(input.Keyword))));
            result.TotalItems = policyQuery.Count();
            policyQuery = policyQuery.OrderByDescending(p => p.Id);

            if (input.PageSize != -1)
            {
                policyQuery = policyQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = policyQuery;
            return result;
        }
    }
}

using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectUtilityMediaEFRepository : BaseEFRepository<RstProjectUtilityMedia>
    {
        public RstProjectUtilityMediaEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectUtilityMedia.SEQ}")
        {
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectUtilityMedia Add(RstProjectUtilityMedia input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityMediaEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, parnerId = {partnerId}");
            input.Id = (int)NextKey();
            input.PartnerId = partnerId;
            input.CreatedDate = DateTime.Now;
            input.CreatedBy = username;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Tìm kiếm theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectUtilityMedia FindById(int id, int partnerId,int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityMediaEFRepository)}->{nameof(FindById)}: id = {id}, parnerId = {partnerId}");
            var result = _dbSet.FirstOrDefault(e => e.Id == id && e.ProjectId == projectId && e.PartnerId == partnerId && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProjectUtilityMedia> GetAll(int partnerId, int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityMediaEFRepository)}->{nameof(GetAll)}: parnerId = {partnerId}");
            var result = _dbSet.Where(e => e.PartnerId == partnerId && e.ProjectId == projectId && e.Deleted == YesNo.NO).OrderByDescending(e => e.Id);
            return result.ToList();
        }
    }

}

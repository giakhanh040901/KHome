using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
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
    public class RstProjectUtilityExtendEFRepository : BaseEFRepository<RstProjectUtilityExtend>
    {
        public RstProjectUtilityExtendEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectUtilityExtend.SEQ}")
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectUtilityExtend Add(RstProjectUtilityExtend input, string username, int partnerId, int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityExtendEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, parnerId = {partnerId}");
            input.Id = (int)NextKey();
            input.PartnerId = partnerId;
            input.ProjectId = projectId;
            input.CreatedDate = DateTime.Now;
            input.CreatedBy = username;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Find by Id
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectUtilityExtend FindById(int id, int partnerId, int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityExtendEFRepository)}->{nameof(FindById)}: id = {id}, parnerId = {partnerId}");

            return _dbSet.FirstOrDefault(e => e.Id == id && e.ProjectId == projectId && e.PartnerId == partnerId && e.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProjectUtilityExtend> GetAll(int partnerId, int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityExtendEFRepository)}->{nameof(GetAll)}: parnerId = {partnerId}");

            var result = _dbSet.Where(e => e.PartnerId == partnerId && e.ProjectId == projectId && e.Deleted == YesNo.NO).OrderByDescending(e => e.Id);

            return result.ToList();
        }
    }
}

using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
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
    public class RstProjectUtilityEFRepository : BaseEFRepository<RstProjectUtility>
    {
        public RstProjectUtilityEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectUtility.SEQ}")
        {
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public void Add(RstProjectUtility input, string username, int partnerId, int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, parnerId = {partnerId}, projectId = {projectId}");

            input.Id = (int)NextKey();
            input.PartnerId = partnerId;
            input.CreatedDate = DateTime.Now;
            input.CreatedBy = username;
            _dbSet.Add(input);
        }

        /// <summary>
        /// Find by UtilityId
        /// </summary>
        /// <param name="utilityId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectUtility FindByUtilityId(int utilityId, int partnerId, int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectUtilityEFRepository)}->{nameof(FindByUtilityId)}:  utilityId = {utilityId}, parnerId = {partnerId}");

            var projectAdvantage = _dbSet.FirstOrDefault(e => e.UtilityId == utilityId && e.ProjectId == projectId&& e.PartnerId == partnerId && e.Deleted == YesNo.NO);
            return projectAdvantage;
        }
    }
}

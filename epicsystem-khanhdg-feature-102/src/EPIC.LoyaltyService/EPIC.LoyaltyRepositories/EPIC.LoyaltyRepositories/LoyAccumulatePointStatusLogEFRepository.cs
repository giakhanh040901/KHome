using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Security;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.LoyaltyRepositories
{
    public class LoyAccumulatePointStatusLogEFRepository : BaseEFRepository<LoyAccumulatePointStatusLog>
    {
        public LoyAccumulatePointStatusLogEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyAccumulatePointStatusLog.SEQ}")
        {
        }

        /// <summary>
        /// Ghi log trạng thái tích/tiêu điểm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public LoyAccumulatePointStatusLog Add(LoyAccumulatePointStatusLog input, string username)
        {
            _logger.LogInformation($"{nameof(LoyAccumulatePointStatusLog)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            input.Id = (int)NextKey();
            input.CreatedBy = username;
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;

            _dbSet.Add(input);

            return input;
        }

        /// <summary>
        /// LẤy list log theo accumulate id
        /// </summary>
        /// <param name="accumulatePointId"></param>
        /// <returns></returns>
        public List<LoyAccumulatePointStatusLog> FindByAccumulatePointId(int accumulatePointId)
        {
            _logger.LogInformation($"{nameof(FindByAccumulatePointId)}: accumulatePointId = {accumulatePointId}");

            var query = from his in _epicSchemaDbContext.LoyHisAccumulatePoints.AsNoTracking()
                        join log in _dbSet.AsNoTracking() on his.Id equals log.HisAccumulatePointId
                        where his.Id == accumulatePointId && his.Deleted == YesNo.NO && log.Deleted == YesNo.NO
                        orderby log.Id descending
                        select log;
            return query.ToList();
        }

    }
}

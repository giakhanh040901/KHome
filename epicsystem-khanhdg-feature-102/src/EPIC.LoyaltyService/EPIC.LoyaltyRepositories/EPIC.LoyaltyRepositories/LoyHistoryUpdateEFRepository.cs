using EPIC.DataAccess.Base;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace EPIC.LoyaltyRepositories
{
    public class LoyHistoryUpdateEFRepository : BaseEFRepository<LoyHistoryUpdate>
    {
        public LoyHistoryUpdateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyHistoryUpdate.SEQ}")
        {
        }

        /// <summary>
        /// Thêm lịch sử thao tác dữ liệu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        public void Add(LoyHistoryUpdate input, string username)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            input.Id = (int)NextKey();
            input.CreatedBy = username;
            input.CreatedDate = DateTime.Now;
            _dbSet.Add(input);
        }
    }
}

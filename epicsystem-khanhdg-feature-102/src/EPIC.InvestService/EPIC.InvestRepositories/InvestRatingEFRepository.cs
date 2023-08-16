using EPIC.DataAccess.Base;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestRatingEFRepository : BaseEFRepository<InvestRating>
    {
        public InvestRatingEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestRating.SEQ}")
        {
        }

        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public InvestRating Add(InvestRating input)
        {
            _logger.LogInformation($"{nameof(InvestRatingEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }
    }
}

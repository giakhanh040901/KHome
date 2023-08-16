using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
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
    public class RstRatingEFRepository : BaseEFRepository<RstRating>
    {
        public RstRatingEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstRating.SEQ}")
        {
        }

        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstRating Add(RstRating input)
        {
            _logger.LogInformation($"{nameof(RstRatingEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }

    }
}

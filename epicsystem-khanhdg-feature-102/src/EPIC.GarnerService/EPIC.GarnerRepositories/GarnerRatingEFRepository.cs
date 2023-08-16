using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerRatingEFRepository : BaseEFRepository<GarnerRating>
    {
        public GarnerRatingEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerRating.SEQ}")
        {
        }

        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerRating Add(GarnerRating input)
        {
            _logger.LogInformation($"{nameof(GarnerRating)}->{nameof(Add)}: input = {input}");
            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Tìm đánh giá theo OrderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<GarnerRating> FindByOrderId(long orderId)
        {
            _logger.LogInformation($"{nameof(GarnerRating)}->{nameof(FindByOrderId)}: orderId = {orderId}");
            var garnerRating = _dbSet.Where(r => r.OrderId == orderId).ToList();
            return garnerRating;
        }
    }
}

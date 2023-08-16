using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerInterestPaymentDetailEFRepository : BaseEFRepository<GarnerInterestPaymentDetail>
    {
        public GarnerInterestPaymentDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerInterestPaymentDetail.SEQ}")
        {
        }
        public GarnerInterestPaymentDetail Add(GarnerInterestPaymentDetail input)
        {
            _logger.LogInformation($"{nameof(GarnerInterestPaymentDetailEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (long)NextKey();
            return _dbSet.Add(input).Entity;
        }

        public IQueryable<GarnerInterestPaymentDetail> GetAllByInterestPaymentId(long interestPaymentId)
        {
            _logger.LogInformation($"{nameof(GarnerInterestPaymentDetailEFRepository)}->{nameof(GetAllByInterestPaymentId)}: interestPaymentId = {interestPaymentId}");
            return _dbSet.Where(d => d.InterestPaymentId == interestPaymentId);
        }
    }
}

using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectFavouriteEFRepository : BaseEFRepository<RstProjectFavourite>
    {
        public RstProjectFavouriteEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectFavourite.SEQ}")
        {
        }

        public RstProjectFavourite Add(RstProjectFavourite input, string username)
        {
            _logger.LogInformation($"{nameof(RstProjectFavouriteEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.CreatedBy = username;
            return _dbSet.Add(input).Entity;
        }
    }
}

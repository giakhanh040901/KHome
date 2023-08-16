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
    public class GarnerConfigContractCodeDetailEFRepository : BaseEFRepository<GarnerConfigContractCodeDetail>
    {
        public GarnerConfigContractCodeDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) 
            : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerConfigContractCodeDetail.SEQ}")
        {
        }

        public GarnerConfigContractCodeDetail Add(GarnerConfigContractCodeDetail entity)
        {
            entity.Id = (int)NextKey();
            return _dbSet.Add(entity).Entity;
        }

        public List<GarnerConfigContractCodeDetail> GetAllByConfigId(int configId)
        {
            var result = _dbSet.Where(e => e.ConfigContractCodeId == configId);
            return result.ToList();
        }
    }
}

using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvConfigContractCodeDetailEFRepository : BaseEFRepository<InvestConfigContractCodeDetail>
    {
        public InvConfigContractCodeDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger)
            : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestConfigContractCodeDetail.SEQ}")
        {
        }

        public InvestConfigContractCodeDetail Add(InvestConfigContractCodeDetail entity)
        {
            entity.Id = (int)NextKey();
            return _dbSet.Add(entity).Entity;
        }

        public List<InvestConfigContractCodeDetail> GetAllByConfigId(int configId)
        {
            var result = _dbSet.Where(e => e.ConfigContractCodeId == configId);
            return result.ToList();
        }
    }
}

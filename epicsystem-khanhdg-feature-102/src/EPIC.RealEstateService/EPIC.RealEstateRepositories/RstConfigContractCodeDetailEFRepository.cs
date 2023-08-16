using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstConfigContractCodeDetailEFRepository : BaseEFRepository<RstConfigContractCodeDetail>
    {
        public RstConfigContractCodeDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstConfigContractCodeDetail.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chi tiết cấu trúc mã hợp đồng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public RstConfigContractCodeDetail Add(RstConfigContractCodeDetail entity)
        {
            entity.Id = (int)NextKey();
            return _dbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Tìm danh sách chi tiết cấu trúc mã theo configContractCodeId
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public List<RstConfigContractCodeDetail> GetAllByConfigId(int configId)
        {
            var result = _dbSet.Where(e => e.ConfigContractCodeId == configId);
            return result.ToList();
        }
    }
}

using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class CifCodeEFRepository : BaseEFRepository<CifCodes>
    {
        private readonly EpicSchemaDbContext _cifCodeDbContext;

        public CifCodeEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, CifCodes.SEQ)
        {
            _cifCodeDbContext = dbContext;
        }

        /// <summary>
        /// Tìm kiếm thông tin cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        public CifCodes FindByCifCode(string cifCode)
        {
            return _dbSet.FirstOrDefault(c => c.CifCode == cifCode && c.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm kiếm sale Id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public CifCodes FindByInvestor(int investorId)
        {
            return _dbSet.FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tạo cif code cho investor
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="lenCifCode"></param>
        public void CreateCifCodeByInvestorId(int investorId, int lenCifCode = 10)
        {
            var id = (int)NextKey();

            _dbSet.Add(new CifCodes
            {
                CifId = id,
                InvestorId = investorId,
                CifCode = GenerateCifCode(lenCifCode),
            });
        }

        /// <summary>
        /// Sinh cif code
        /// </summary>
        /// <param name="lenCifCode"></param>
        /// <returns></returns>
        public string GenerateCifCode(int lenCifCode = 10)
        {
            string result = "";
            bool duplicate = true;
            do
            {
                result = CommonUtils.RandomNumber(lenCifCode);
                duplicate = _dbSet.Any(c => c.CifCode == result && c.Deleted == YesNo.NO);
            } while (duplicate);

            return result;
        }

    }
}

using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstOpenSellInterestEFRepository : BaseEFRepository<RstOpenSellInterest>
    {
        public RstOpenSellInterestEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectFavourite.SEQ}")
        {
        }

        /// <summary>
        /// Thêm data bảng quan tâm mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOpenSellInterest Add(RstOpenSellInterest input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellInterestEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập nhập data bảng quan tâm mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstOpenSellInterest Update(RstOpenSellInterest input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellInterestEFRepository)} -> {nameof(Update)}: {JsonSerializer.Serialize(input)}");
            var openSell = _dbSet.FirstOrDefault(x => x.OpenSellDetailId == input.OpenSellDetailId && x.InvestorId == input.InvestorId);
            if (openSell == null)
            {
                input.Id = (int)NextKey();
                input.InterestCount = DefaultValue.ONE;
                return _dbSet.Add(input).Entity;
            }
            else
            {
                openSell.InvestorId = input.InvestorId;
                openSell.OpenSellDetailId = input.OpenSellDetailId;
                openSell.InterestCount++;
                return openSell;
            }
        }
    }
}

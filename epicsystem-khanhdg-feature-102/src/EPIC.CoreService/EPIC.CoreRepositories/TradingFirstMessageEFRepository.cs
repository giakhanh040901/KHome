using DocumentFormat.OpenXml.Bibliography;
using EPIC.CoreEntities.DataEntities;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class TradingFirstMessageEFRepository : BaseEFRepository<TradingFirstMessage>
    {
        private readonly EpicSchemaDbContext _epicDbContext;

        public TradingFirstMessageEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, TradingFirstMessage.SEQ)
        {
            _epicDbContext = dbContext;
        }

        /// <summary>
        /// Get all tin nhắn
        /// </summary>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        public List<TradingFirstMessage> FindAll()
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
            return _dbSet.AsNoTracking().Where(x => x.Deleted == YesNo.NO).ToList();
        }

        /// <summary>
        /// Tìm theo đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public TradingFirstMessage FindByTrading(int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name} tradingProviderId={tradingProviderId}");
            return _dbSet.FirstOrDefault(x => x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thêm cấu hình tin nhắn
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TradingFirstMessage entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name} entity={JsonSerializer.Serialize(entity)}");

            entity.Id = (int)NextKey();
            entity.Deleted = YesNo.NO;

            _dbSet.Add(entity);
        }

        /// <summary>
        /// Lấy cấu hình tin nhắn theo phone của investor
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public TradingFirstMessage FindTradingProviderByInvestorPhone(string phone)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name} phone={phone}");

            var query = from inv in _epicDbContext.Investors.AsNoTracking()
                        join invTrading in _epicDbContext.InvestorTradingProviders.AsNoTracking() on inv.InvestorId equals invTrading.InvestorId
                        join trading in _epicDbContext.TradingProviders.AsNoTracking() on invTrading.TradingProviderId equals trading.TradingProviderId
                        join tradingMsg in _epicDbContext.TradingFirstMessages.AsNoTracking() on trading.TradingProviderId equals tradingMsg.TradingProviderId
                        where inv.Phone == phone && inv.Deleted == YesNo.NO && invTrading.Deleted == YesNo.NO && trading.Deleted == YesNo.NO && tradingMsg.Deleted == YesNo.NO
                        select tradingMsg;

            return query.FirstOrDefault();
        }
    }
}

using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class ApproveEFRepository : BaseEFRepository<CoreApprove>
    {
        private readonly EpicSchemaDbContext _cifCodeDbContext;

        public ApproveEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, CoreApprove.SEQ)
        {
            _cifCodeDbContext = dbContext;
        }

        /// <summary>
        /// Tạo yêu cầu trình duyệt
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        public CoreApprove CreateApproveRequest(CoreApprove entity, int? tradingProviderId = null, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(CoreApprove)}->{nameof(CreateApproveRequest)}: entity = {JsonSerializer.Serialize(entity)}, tradingProviderId = {tradingProviderId}, partnerId={partnerId}");

            entity.ApproveID = (int)NextKey();
            entity.TradingProviderId = tradingProviderId;
            entity.PartnerId = partnerId;
            entity.RequestDate = DateTime.Now;
            entity.Status = ApproveStatus.TRINH_DUYET;

            _dbSet.Add(entity);
            return entity;
        }

    }
}

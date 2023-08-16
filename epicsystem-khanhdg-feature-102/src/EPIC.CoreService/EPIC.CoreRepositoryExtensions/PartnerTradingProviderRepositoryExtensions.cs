using EPIC.DataAccess.Base;
using EPIC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreRepositoryExtensions
{
    public static class PartnerTradingProviderRepositoryExtensions
    {
        /// <summary>
        /// Kiểm tra mối quan hệ giữa partner và tradingProvider
        /// </summary>
        public static void CheckTradingRelationshipPartner(this EpicSchemaDbContext dbContext, int? partnerId, int? tradingProviderId)
        {
            if (partnerId == null || tradingProviderId == null)
            {
                return;
            }

            var defErrorEFRepository = new DefErrorEFRepository(dbContext);

            var relationship = dbContext.TradingProviderPartners
                .Any(tp => tp.TradingProviderId == tradingProviderId && tp.PartnerId == partnerId && tp.Deleted == YesNo.NO);
            if (!relationship)
            {
                defErrorEFRepository.ThrowException(ErrorCode.CoreTradingProviderPartnerRelationshipNotFound, tradingProviderId);
            }
        }

        /// <summary>
        /// Kiểm tra mối quan hệ giữa partner và tradingProvider
        /// </summary>
        public static void CheckTradingRelationshipPartner(this EpicSchemaDbContext dbContext, int? partnerId, IEnumerable<int> tradingProviderIds)
        {
            if (partnerId == null || tradingProviderIds == null || tradingProviderIds.Count() == 0)
            {
                return;
            }

            foreach (var tradingProviderId in tradingProviderIds)
            {
                dbContext.CheckTradingRelationshipPartner(partnerId, tradingProviderId);
            }
        }
    }
}

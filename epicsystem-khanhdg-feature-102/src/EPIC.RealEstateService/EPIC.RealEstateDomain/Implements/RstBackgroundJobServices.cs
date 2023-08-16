using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstBackgroundJobServices
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly EpicSchemaDbContext _dbContext;

        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderPaymentEFRepository _rstOrderPaymentEFRepository;

        private readonly IRstSignalRBroadcastServices _rstSignalRBroadcastServices;

        public RstBackgroundJobServices(
            EpicSchemaDbContext dbContext,
            ILogger<RstBackgroundJobServices> logger,
            DatabaseOptions databaseOptions,
            IRstSignalRBroadcastServices rstSignalRBroadcastServices)
        {
            _dbContext = dbContext;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstOrderPaymentEFRepository = new RstOrderPaymentEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);

            _rstSignalRBroadcastServices = rstSignalRBroadcastServices;
        }

        /// <summary>
        /// Update trạng thái căn hộ và sản phẩm mở bán khi hợp đồng hết thời hạn đặt cọc chưa đủ thanh toán
        /// </summary>
        [Queue(HangfireQueues.RealEstate)]
        [HangfireLogEverything]
        public async Task UpdateDepositExpire(int orderId)
        {
            // Tìm kiếm 
            var orderQuery = _rstOrderEFRepository.FindById(orderId);
            if (orderQuery == null)
            {
                return;
            }
            orderQuery.ModifiedDate = DateTime.Now;
            var sumPaymentDepositAmount = _rstOrderPaymentEFRepository.SumPaymentDepositAmount(orderId);
            if (orderQuery != null && orderQuery.Status < RstOrderStatus.CHO_DUYET_HOP_DONG_COC && orderQuery.DepositMoney > sumPaymentDepositAmount)
            {
                //Đổ trạng thái căn hộ và trạng thái mở bán về khởi tạo nếu chưa thanh toán đủ
                var productItemQuery = _rstProductItemEFRepository.Entity.FirstOrDefault(c => c.Id == orderQuery.ProductItemId && c.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);
                var openSellDetail = _rstOpenSellDetailEFRepository.Entity.FirstOrDefault(c => c.Id == orderQuery.OpenSellDetailId && c.Deleted == YesNo.NO)
                    .ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
                if (productItemQuery.Status == RstProductItemStatus.GIU_CHO && openSellDetail.Status == RstProductItemStatus.GIU_CHO)
                {
                    productItemQuery.Status = RstProductItemStatus.KHOI_TAO;
                    openSellDetail.Status = RstProductItemStatus.KHOI_TAO;
                    _dbContext.SaveChanges();
                    await _rstSignalRBroadcastServices.BroadcatProductItemHasPaymentOrder(orderQuery.ProductItemId, openSellDetail.OpenSellId);
                    await _rstSignalRBroadcastServices.BroadcastOpenSellDetailChangeStatus(openSellDetail.Id);
                }
            }
        }
    }
}

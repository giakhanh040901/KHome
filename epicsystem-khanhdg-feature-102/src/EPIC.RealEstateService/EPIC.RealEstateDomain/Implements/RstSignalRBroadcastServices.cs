using EPIC.DataAccess.Base;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateDomain.SignalR.Hubs;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.ConstantVariables.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstSignalRBroadcastServices : IRstSignalRBroadcastServices
    {
        private readonly ILogger _logger;
        private readonly IHubContext<ProductItemHub> _hub;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstDistributionProductItemEFRepository _rstDistributionProductItemEFRepository;

        public RstSignalRBroadcastServices(
            ILogger<RstSignalRBroadcastServices> logger,
            IHubContext<ProductItemHub> hub,
            EpicSchemaDbContext dbContext)
        {
            _logger = logger;
            _hub = hub;
            _dbContext = dbContext;
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstDistributionProductItemEFRepository = new RstDistributionProductItemEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Hợp đồng mới nhất trong bảng hàng của dự án
        /// </summary>
        public async Task BroadcastUpdateOrderNewInProject(int orderId)
        {
            var orderQuery = _rstOrderEFRepository.FindById(orderId);
            if (orderQuery == null) return;
            var productItem = _rstProductItemEFRepository.FindById(orderQuery.ProductItemId); 
            if (productItem == null) return;

            var result = _rstOrderEFRepository.InfoOrderNewInProject(orderId);

            List<Task> tasks = new()
            {
                _hub.Clients.Group(UserGroupNames.PrefixPartner + productItem.PartnerId)
                    .SendAsync(RstSignalRMethods.UpdateOrderNewInProject, result),

                _hub.Clients.Group(UserGroupNames.PrefixTradingProvider + orderQuery.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateOrderNewInProject, result)
            };
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Khi căn hộ có thanh toán trong trạng thái giữ chỗ 
        /// Sử dụng ở Phê duyệt/ Hủy thanh toán và Backgroundjob khi hết thời gian giữ chỗ
        /// </summary>"></param>
        /// <returns></returns>
        public async Task BroadcatProductItemHasPaymentOrder(int productItemId, int openSellId)
        {
            var openSellQuery = _dbContext.RstOpenSells.FirstOrDefault(s => s.Id == openSellId && s.Deleted == YesNo.NO);

            // Đếm Tổng số lượng căn hộ đang giữ chỗ có phát sinh thanh toán và đang trong thời gian giữ chỗ
            var productItemQuery = from openSell in _dbContext.RstOpenSells
                                              join openSellDetail in _dbContext.RstOpenSellDetails on openSell.Id equals openSellDetail.OpenSellId
                                              join distributionItem in _dbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionItem.Id
                                              join distribution in _dbContext.RstDistributions on distributionItem.DistributionId equals distribution.Id
                                              join productItem in _dbContext.RstProductItems on distributionItem.ProductItemId equals productItem.Id
                                              where openSell.Id == openSellId && openSell.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                                              && openSellDetail.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                              && distributionItem.Status == Status.ACTIVE && openSell.Status == RstDistributionStatus.DANG_BAN
                                              && openSell.StartDate.Date <= DateTime.Now.Date && openSell.IsShowApp == YesNo.YES && openSellDetail.IsShowApp == YesNo.YES
                                              && openSellDetail.IsLock == YesNo.NO && productItem.IsLock == YesNo.NO && distribution.Status == RstDistributionStatus.DANG_BAN
                                              && productItem.Status == RstProductItemStatus.GIU_CHO
                                              select productItem;
            var paymentOrderArisingCount = productItemQuery.Where(p => _dbContext.RstOrders.Any(o => productItemId == o.ProductItemId && o.Deleted == YesNo.NO && (o.ExpTimeDeposit == null || o.ExpTimeDeposit > DateTime.Now)
                                        && _dbContext.RstOrderPayments.Any(p => o.Id == p.OrderId && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN))).Count();

            var productItemCheck = _dbContext.RstOrders.Any(o => productItemId == o.ProductItemId && o.Deleted == YesNo.NO && (o.ExpTimeDeposit == null || o.ExpTimeDeposit > DateTime.Now)
                                        && _dbContext.RstOrderPayments.Any(p => o.Id == p.OrderId && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN));
            var dataPushProductItem = new
            {
                productItemId,
                HasPaymentOrder = (productItemCheck) ? YesNo.YES : null,
                PaymentOrderArisingCount = paymentOrderArisingCount,
                HoldCount = productItemQuery.Count() - paymentOrderArisingCount,
            };

            List<Task> tasks = new()
            {
                _hub.Clients.Group(UserGroupNames.PrefixInvestorTradingProvider + openSellQuery.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateProductItemHasPayment, dataPushProductItem)
            };
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Đổi trạng thái open sell detail
        /// </summary>
        public async Task BroadcastOpenSellDetailChangeStatus(int openSellDetailId)
        {
            var openSellInfo = (from openSellDetail in _rstOpenSellDetailEFRepository.EntityNoTracking
                                join openSell in _rstOpenSellEFRepository.EntityNoTracking on openSellDetail.OpenSellId equals openSell.Id
                                join distributionProductItem in _rstDistributionProductItemEFRepository.EntityNoTracking on openSellDetail.DistributionProductItemId equals distributionProductItem.Id
                                join productItem in _rstProductItemEFRepository.EntityNoTracking on distributionProductItem.ProductItemId equals productItem.Id
                                where openSellDetail.Id == openSellDetailId && openSellDetail.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO
                                select new
                                {
                                    openSellDetail.Id,
                                    openSellDetail.Status,
                                    ProductItemStatus = productItem.Status,
                                    openSell.TradingProviderId,
                                    productItem.PartnerId,
                                    openSell.ProjectId,
                                    distributionProductItem.ProductItemId,
                                    openSellDetail.OpenSellId
                                }).FirstOrDefault();
            if (openSellInfo == null)
            {
                return;
            }

            int productItemStatus = openSellInfo.ProductItemStatus;
            if (productItemStatus == RstProductItemStatus.KHOI_TAO)
            {
                productItemStatus = RstProductItemStatus.LOGIC_CHUA_MO_BAN;

                var checkStatus = (from distributionProductItem in _dbContext.RstDistributionProductItems
                                   join openSellDetail in _dbContext.RstOpenSellDetails on distributionProductItem.Id equals openSellDetail.DistributionProductItemId
                                   join openSell in _dbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                                   where distributionProductItem.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                                   && distributionProductItem.ProductItemId == openSellInfo.ProductItemId && openSell.Status != RstDistributionStatus.HUY_DUYET
                                   select openSellDetail).Any();
                if (checkStatus)
                {
                    productItemStatus = RstProductItemStatus.LOGIC_DANG_MO_BAN;
                }
            }
            var dataPushForPartnerProductItem = new
            {
                openSellInfo.ProductItemId,
                Status = productItemStatus
            };

            var dataPushProductItem = new
            {
                openSellInfo.ProductItemId,
                Status = openSellInfo.ProductItemStatus
            };
            List<Task> tasks = new()
            {
                _hub.Clients.Group(UserGroupNames.PrefixPartner + openSellInfo.PartnerId)
                    .SendAsync(RstSignalRMethods.UpdateProductItemStatus, dataPushForPartnerProductItem),
                _hub.Clients.Group(UserGroupNames.PrefixTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateProductItemStatus, dataPushProductItem),
                _hub.Clients.Group(UserGroupNames.PrefixInvestorTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateProductItemStatus, dataPushProductItem)
            };

            if (openSellInfo.Status == RstProductItemStatus.GIU_CHO)
            {
                AppCountRstProductItemSignalRDto countProductItemForPartner = new()
                {
                    ProjectId = openSellInfo.ProjectId,
                    OpenSellId = openSellInfo.OpenSellId,
                    HoldCount = CountStatusProductItemForPartner(openSellInfo.ProjectId, RstProductItemStatus.GIU_CHO),
                    OpenCount = CountStatusProductItemForPartner(openSellInfo.ProjectId, RstProductItemStatus.LOGIC_DANG_MO_BAN),
                };

                //đếm lại product item (đang mở bán, giữ chỗ), open sell detail (đang mở bán, giữ chỗ)
                AppCountRstProductItemSignalRDto countProductItemForTrading = new()
                {
                    ProjectId = openSellInfo.ProjectId,
                    OpenSellId = openSellInfo.OpenSellId,
                    HoldCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.GIU_CHO),
                    OpenCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.KHOI_TAO),
                };

                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixPartner + openSellInfo.PartnerId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForPartner));
                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForTrading));
                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixInvestorTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForTrading));
            }
            else if (openSellInfo.Status == RstProductItemStatus.KHOA_CAN) //đại lý khoá
            {
                //đếm lại open sell detail (khoá căn, )
                AppCountRstProductItemSignalRDto countProductItemForPartner = new()
                {
                    ProjectId = openSellInfo.ProjectId,
                    HoldCount = CountStatusProductItemForPartner(openSellInfo.ProjectId, RstProductItemStatus.GIU_CHO),
                    OpenCount = CountStatusProductItemForPartner(openSellInfo.ProjectId, RstProductItemStatus.LOGIC_DANG_MO_BAN),
                    LockCount = CountStatusProductItemForPartner(openSellInfo.ProjectId, RstProductItemStatus.KHOA_CAN),
                };

                //đếm lại product item (đang mở bán, giữ chỗ), open sell detail (đang mở bán, giữ chỗ)
                AppCountRstProductItemSignalRDto countProductItemForTrading = new()
                {
                    ProjectId = openSellInfo.ProjectId,
                    OpenSellId = openSellInfo.OpenSellId,
                    HoldCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.GIU_CHO),
                    OpenCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.KHOI_TAO),
                    LockCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.KHOA_CAN),
                };

                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixPartner + openSellInfo.PartnerId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForPartner));
                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForTrading));
                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixInvestorTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForTrading));
            }
            else if (openSellInfo.Status == RstProductItemStatus.DA_COC)
            {
                //đếm lại open sell detail (khoá căn, và giữ chỗ )
                AppCountRstProductItemSignalRDto countProductItemForPartner = new()
                {
                    ProjectId = openSellInfo.ProjectId,
                    DepositCount = CountStatusProductItemForPartner(openSellInfo.ProjectId, RstProductItemStatus.DA_COC),
                    LockCount = CountStatusProductItemForPartner(openSellInfo.ProjectId, RstProductItemStatus.KHOA_CAN),
                };

                AppCountRstProductItemSignalRDto countProductItemForTrading = new()
                {
                    ProjectId = openSellInfo.ProjectId,
                    OpenSellId = openSellInfo.OpenSellId,
                    DepositCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.DA_COC),
                    LockCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.KHOA_CAN),
                };

                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixPartner + openSellInfo.PartnerId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForPartner));
                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForTrading));
                tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixInvestorTradingProvider + openSellInfo.TradingProviderId)
                    .SendAsync(RstSignalRMethods.UpdateCountProductItem, countProductItemForTrading));
            }
            else if (openSellInfo.Status == RstProductItemStatus.DA_BAN)
            {
                //đếm lại product item (đã cọc, đã bán), đếm lại open sell detail (mở bán)
                int soldCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.DA_BAN);
                int depositCount = CountStatusProductItemForTrading(openSellInfo.Id, RstProductItemStatus.DA_COC);

                //tasks.Add(_hub.Clients.Group(UserGroupNames.PrefixInvestorTradingProvider + openSellInfo.TradingProviderId)
                //    .SendAsync(RstSignalRMethods.UpdateHoldCountProductItem, new AppSoldCountRstProductItemSignalRDto()
                //    {
                //        ProjectId = openSellInfo.ProjectId,
                //        ProductItemId = openSellInfo.ProductItemId,
                //        Status = RstProductItemStatus.DA_COC,
                //        SoldCount = soldCount,
                //        DepositCount = depositCount,
                //    }));
            }
            await Task.WhenAll(tasks);
        }

        #region Đếm số lượng căn hộ
        /// <summary>
        /// Đếm số lượng căn hộ của dự án theo trạng thái mà đại lý trong mở bán
        /// </summary>
        /// <param name="openSellDetailId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int CountStatusProductItemForTrading(int openSellDetailId, int status)
        {
            var productItemQuery = from openSellDetailCurrent in _dbContext.RstOpenSellDetails.AsNoTracking()
                                   join openSell in _dbContext.RstOpenSells.AsNoTracking() on openSellDetailCurrent.OpenSellId equals openSell.Id
                                   join openSellDetail in _dbContext.RstOpenSellDetails.AsNoTracking() on openSell.Id equals openSellDetail.OpenSellId
                                   join distributionItem in _dbContext.RstDistributionProductItems.AsNoTracking() on openSellDetail.DistributionProductItemId equals distributionItem.Id
                                   join productItem in _dbContext.RstProductItems.AsNoTracking() on distributionItem.ProductItemId equals productItem.Id
                                   where openSellDetailCurrent.Id == openSellDetailId && openSellDetailCurrent.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO
                                   && openSellDetail.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                   && productItem.Status == status && openSellDetail.Status == status && openSell.StartDate.Date <= DateTime.Now.Date
                                   select productItem;
            return productItemQuery.Count();
        }

        /// <summary>
        /// Đếm số lượng căn hộ theo trạng thái của dự án
        /// </summary>
        public int CountStatusProductItemForPartner(int projectId, int status)
        {
            // Nếu đếm count đang mở bán thì check xem căn đã được mở bán thì mới đếm
            var productItemCountByStatus = _dbContext.RstProductItems.AsNoTracking()
                .Count(p => p.ProjectId == projectId && p.Deleted == YesNo.NO
                && ((status == RstProductItemStatus.LOGIC_DANG_MO_BAN && p.Status == RstProductItemStatus.KHOI_TAO
                && (from distributionProductItem in _dbContext.RstDistributionProductItems
                    join openSellDetail in _dbContext.RstOpenSellDetails on distributionProductItem.Id equals openSellDetail.DistributionProductItemId
                    join openSell in _dbContext.RstOpenSells on openSellDetail.OpenSellId equals openSell.Id
                    where distributionProductItem.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                    && distributionProductItem.ProductItemId == p.Id && openSell.Status != RstDistributionStatus.HUY_DUYET
                    select openSellDetail).Any()) || status == p.Status));
            return productItemCountByStatus;
        }
        #endregion
    }
}

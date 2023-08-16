using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstCountServices : IRstCountServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstCountServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstProjectTypeEFRepository _rstProjectTypeEFRepository;
        private readonly RstProjectGuaranteeBankEFRepository _rstProjectGuaranteeBankRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly RstApproveEFRepository _rstApproveEFRepository;
        private readonly RstOwnerEFRepository _rstOwnerEFRepository;
        private readonly RstDistributionEFRepository _rstDistributionEFRepository;
        private readonly RstDistributionProductItemEFRepository _rstDistributionProductItemEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;

        public RstCountServices(EpicSchemaDbContext dbContext,
                 DatabaseOptions databaseOptions,
                 IMapper mapper,
                 ILogger<RstCountServices> logger,
                 IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProjectTypeEFRepository = new RstProjectTypeEFRepository(dbContext, logger);
            _rstProjectGuaranteeBankRepository = new RstProjectGuaranteeBankEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _rstApproveEFRepository = new RstApproveEFRepository(dbContext, logger);
            _rstOwnerEFRepository = new RstOwnerEFRepository(dbContext, logger);
            _rstDistributionEFRepository = new RstDistributionEFRepository(dbContext, logger);
            _rstDistributionProductItemEFRepository = new RstDistributionProductItemEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Đếm số lượng từng loại của sản phẩm trong dự án
        /// </summary>
        public RstCountProductItemSignalRDtoBase CountProductItemByPartner(int projectId)
        {
            var productItems = _dbContext.RstProductItems.Where(p => p.ProjectId == projectId && p.Deleted == YesNo.NO);
            return new RstCountProductItemSignalRDtoBase()
            {
                ProjectId = projectId,
                OpenCount = productItems.Where(p => p.Status == RstProductItemStatus.KHOI_TAO).Count(),
                HoldCount = productItems.Where(p => p.Status == RstProductItemStatus.GIU_CHO).Count(),
                LockCount = productItems.Where(p => p.Status == RstProductItemStatus.KHOA_CAN).Count(),
                DepositCount = productItems.Where(p => p.Status == RstProductItemStatus.DA_COC).Count(),
                SoldCount = productItems.Where(p => p.Status == RstProductItemStatus.DA_COC).Count(),
            };
        }

        /// <summary>
        /// Hiển thị trên App Đếm số lượng từng loại của sản phẩm của mở bán trong dự án theo đại lý
        /// </summary>
        public AppCountProductItemSignalRDto AppCountProductItemByTrading(int openSellId)
        {
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
                                   select new
                                   {
                                       productItem,
                                       openSell.StartDate
                                   };

            // Tổng số lượng căn hộ đang giữ chỗ có phát sinh thanh toán đang trong thời gian giữ chỗ
            var paymentArisingCount = productItemQuery.Where(p => p.productItem.Status == RstProductItemStatus.GIU_CHO
                                        && _dbContext.RstOrders.Any(o => p.productItem.Id == o.ProductItemId && o.Deleted == YesNo.NO && (o.ExpTimeDeposit == null || o.ExpTimeDeposit > DateTime.Now) 
                                            && _dbContext.RstOrderPayments.Any(p => o.Id == p.OrderId && p.Deleted == YesNo.NO && p.TranClassify == TranClassifies.THANH_TOAN && p.TranType == TranTypes.THU && p.Status == OrderPaymentStatus.DA_THANH_TOAN))
                                       ).Count();
            // Tổng số lượng căn hộ đang giữ chỗ chưa có phát sinh thanh toán
            var holdCount = productItemQuery.Where(p => p.productItem.Status == RstProductItemStatus.GIU_CHO).Count() - paymentArisingCount;

            return new AppCountProductItemSignalRDto()
            {
                OpenSellId = openSellId,
                OpenCount = productItemQuery.Where(p => p.productItem.Status == RstProductItemStatus.KHOI_TAO).Count(),
                HoldCount = holdCount,
                PaymentOrderArisingCount = paymentArisingCount,
                LockCount = productItemQuery.Where(p => p.productItem.Status == RstProductItemStatus.KHOA_CAN).Count(),
                DepositCount = productItemQuery.Where(p => p.productItem.Status == RstProductItemStatus.DA_COC).Count(),
                SoldCount = productItemQuery.Where(p => p.productItem.Status == RstProductItemStatus.DA_BAN).Count(),
            };
        }

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
    }
}

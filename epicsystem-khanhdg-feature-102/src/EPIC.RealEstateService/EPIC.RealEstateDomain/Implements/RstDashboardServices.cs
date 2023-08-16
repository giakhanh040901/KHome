using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Sale;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDashboard;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstDashboardServices : IRstDashboardServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<RstDashboardServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        public RstDashboardServices(
            EpicSchemaDbContext dbContext,
            ILogger<RstDashboardServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _configuration = configuration;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
        }

        public RstDashboardDto Dasboard(GetRstDashboardDto dto)
        {
            var result = new RstDashboardDto();
            List<int> tradingProviderIds = new List<int>();
            int? partnerId = null;
            // tạm thời chưa có đã bán nên lấy cả đã cọc và đã bán
            var listStatusProductItem = new List<int> { RstProductItemStatus.DA_COC, RstProductItemStatus.DA_BAN };

            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                var listTradingProviderInPartners = from trading in _dbContext.TradingProviders
                                                    join businessCustomer in _dbContext.BusinessCustomers on trading.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                                    join tradingParner in _dbContext.TradingProviderPartners on trading.TradingProviderId equals tradingParner.TradingProviderId
                                                    where tradingParner.PartnerId == partnerId && trading.Deleted == YesNo.NO && tradingParner.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                                    select trading.TradingProviderId;
                if (dto.TradingProviderId != null)
                {
                    tradingProviderIds.Add(dto.TradingProviderId ?? 0);
                }
                else
                {
                    tradingProviderIds.AddRange(listTradingProviderInPartners);
                }
            }
            else if (userType == UserTypes.ROOT_TRADING_PROVIDER || userType == UserTypes.TRADING_PROVIDER)
            {
                tradingProviderIds.Add(CommonUtils.GetCurrentTradingProviderId(_httpContext));
            }
            else if (userType != UserTypes.ROOT_EPIC || userType == UserTypes.EPIC)
            {
                return result;
            }

            // Biểu đồ bán hàng theo thời gian
            result.SellChartOverTime = BanHangTheoThoiGian(dto.ProjectId, dto.FirstDate, dto.EndDate, tradingProviderIds, listStatusProductItem);
            if (dto.TradingProviderId != null || tradingProviderIds.Count == 1)
            {
                // Dự án nổi bật
                result.DashboardOutstandingProject = OutstandingProjectByTrading(tradingProviderIds.FirstOrDefault(), listStatusProductItem);
                // Tổng quan
                result.DashboardOverView = TongQuanDaiLy(dto.ProjectId, tradingProviderIds.FirstOrDefault(), listStatusProductItem);
                // Biểu đồ theo từng loại hình và mức bán
                result.NumberOfApartmentsChart = NumberOfApartmentsChartTradingProvider(dto.ProjectId, dto.FirstDate, dto.EndDate, tradingProviderIds, listStatusProductItem);
                // Tỷ lệ bán hàng của các phòng ban trong đại lý
                result.ChartRatioSell = ChartRatioSellTradingProvider(dto.ProjectId, dto.Sort, tradingProviderIds, listStatusProductItem, dto.FirstDate, dto.EndDate);

            }
            else if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                // Dự án nổi bật
                result.DashboardOutstandingProject = OutstandingProjectByPartner(partnerId ?? 0, listStatusProductItem);
                result.DashboardOverView = TongQuanDoiTac(dto.ProjectId, partnerId ?? 0, listStatusProductItem);
                result.ChartRatioSell = ChartRatioSellPartner(dto.Sort, listStatusProductItem, dto.FirstDate, dto.EndDate, dto.ProjectId, partnerId ?? 0);
                result.NumberOfApartmentsChart = NumberOfApartmentsChartPartner(dto.ProjectId, dto.FirstDate, dto.EndDate, partnerId ?? 0, listStatusProductItem);
            }

            result.RstDashboardActions = DashboardGetNewAction(tradingProviderIds);
            return result;
        }

        /// <summary>
        /// Biểu đồ tỷ lệ bán hàng
        /// </summary>
        public List<ChartRatioSellDto> ChartRatioSellPartner(bool sort, List<int> listStatus, DateTime? startDate, DateTime? endDate, int? projectId, int partnerId)
        {

            var listCharRatioSell = from trading in _dbContext.TradingProviders
                                    join businessCustomer in _dbContext.BusinessCustomers on trading.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                    join tradingParner in _dbContext.TradingProviderPartners on trading.TradingProviderId equals tradingParner.TradingProviderId
                                    where tradingParner.PartnerId == partnerId && trading.Deleted == YesNo.NO && tradingParner.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                                    select new ChartRatioSellDto
                                    {
                                        Name = businessCustomer.Name,
                                        TotalSell = (from orders in _dbContext.RstOrders
                                                     join openSellDetail in _dbContext.RstOpenSellDetails on orders.OpenSellDetailId equals openSellDetail.Id
                                                     join productItem in _dbContext.RstProductItems on orders.ProductItemId equals productItem.Id
                                                     where openSellDetail.Deleted == YesNo.NO && listStatus.Contains(openSellDetail.Status)
                                                     && orders.Deleted == YesNo.NO && openSellDetail.Status == productItem.Status
                                                     && orders.TradingProviderId == trading.TradingProviderId && orders.DepositDate != null && productItem.Deleted == YesNo.NO
                                                     && (projectId == null || (productItem.ProjectId == projectId))
                                                     && (orders.Status == RstOrderStatus.DA_COC || orders.Status == RstOrderStatus.DA_BAN)
                                                     && (startDate == null || startDate.Value.Date <= orders.DepositDate.Value.Date)
                                                      && (endDate == null || endDate.Value.Date >= orders.DepositDate.Value.Date)
                                                     select openSellDetail).Count()
                                    };

            listCharRatioSell = sort ? listCharRatioSell.OrderBy(i => i.TotalSell) : listCharRatioSell.OrderByDescending(i => i.TotalSell);
            return listCharRatioSell.ToList();
        }

        /// <summary>
        /// Biểu đồ tỷ lệ bán hàng của các phòng ban trong đại lý
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="tradingProviderIds"></param>
        /// <param name="listStatus"></param>
        /// <returns></returns>
        public List<ChartRatioSellDto> ChartRatioSellTradingProvider(int? projectId, bool sort, List<int> tradingProviderIds, List<int> listStatus, DateTime? startDate, DateTime? endDate)
        {
            var listCharRatioSell = new List<ChartRatioSellDto>();

            var tradingProviderId = tradingProviderIds.FirstOrDefault();

            var departmentInTrading = _dbContext.Departments.Where(d => d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            foreach (var item in departmentInTrading)
            {
                listCharRatioSell.Add(new ChartRatioSellDto
                {
                    Name = item.DepartmentName,
                    DepartmentId = item.DepartmentId,
                    TotalSell = (from orders in _dbContext.RstOrders
                                 join openSellDetail in _dbContext.RstOpenSellDetails on orders.OpenSellDetailId equals openSellDetail.Id
                                 join productItem in _dbContext.RstProductItems on orders.ProductItemId equals productItem.Id
                                 where openSellDetail.Deleted == YesNo.NO && listStatus.Contains(openSellDetail.Status)
                                 && orders.DepartmentId == item.DepartmentId && orders.Deleted == YesNo.NO && openSellDetail.Status == productItem.Status
                                 && orders.TradingProviderId == tradingProviderId && orders.DepositDate != null && productItem.Deleted == YesNo.NO
                                 && (projectId == null ||(productItem.ProjectId == projectId))
                                 && (orders.Status == RstOrderStatus.DA_COC || orders.Status == RstOrderStatus.DA_BAN)
                                 && (startDate == null || startDate.Value.Date <= orders.DepositDate.Value.Date) 
                                 && (endDate == null || endDate.Value.Date >= orders.DepositDate.Value.Date)
                                 select openSellDetail).Count()
                });
            }

            if (sort)
            {
                listCharRatioSell = listCharRatioSell.OrderBy(i => i.TotalSell).ToList();
            }
            else
            {
                listCharRatioSell = listCharRatioSell.OrderByDescending(i => i.TotalSell).ToList();
            }
            return listCharRatioSell;
        }

        /// <summary>
        ///  du an noi bat
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<DashboardOutstandingProjectDto> OutstandingProjectByTrading(int tradingProviderId, List<int> listStatus)
        {
            var result = from openSell in _dbContext.RstOpenSells
                                   join project in _dbContext.RstProjects on openSell.ProjectId equals project.Id
                                   where openSell.Deleted == YesNo.NO && project.Deleted == YesNo.NO
                                   && openSell.Status == RstDistributionStatus.DANG_BAN && tradingProviderId == openSell.TradingProviderId
                                   && openSell.IsOutstanding == YesNo.YES
                                   select new DashboardOutstandingProjectDto
                                   {
                                       Name = project.Name,
                                       Address = project.Address,
                                       LandArea = project.LandArea,
                                       UrlImage = _dbContext.RstProjectMedias.FirstOrDefault(o => o.ProjectId == project.Id && o.Deleted == YesNo.NO && o.Location == RstMediaLocations.ANH_DAI_DIEN_DU_AN).UrlImage,
                                       TotalProductItem = _dbContext.RstOpenSellDetails.Count(o => openSell.Id == o.OpenSellId && o.Deleted == YesNo.NO),
                                       TotalProductItemSell = (from order in _dbContext.RstOrders
                                                              join openSellDetail in _dbContext.RstOpenSellDetails on order.OpenSellDetailId equals openSellDetail.Id
                                                              join productItem in _dbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                                              where openSellDetail.OpenSellId == openSell.Id && order.Status == RstOrderStatus.DA_COC
                                                              && listStatus.Contains(openSellDetail.Status) && openSellDetail.Status == productItem.Status
                                                              select order).Count()
                                   };

            return result.ToList();
        }

        public List<DashboardOutstandingProjectDto> OutstandingProjectByPartner(int partnerId, List<int> listStatus)
        {
            var result = from project in _dbContext.RstProjects
                         where project.PartnerId == partnerId && project.Deleted == YesNo.NO && project.Status == DistributionStatus.HOAT_DONG
                         && _dbContext.RstDistributions.Any(d => project.Id == d.ProjectId && d.Status == RstDistributionStatus.DANG_BAN && d.Deleted == YesNo.NO)
                         select new DashboardOutstandingProjectDto
                         {
                             Id = project.Id,
                             Name = project.Name,
                             Address = project.Address,
                             LandArea = project.LandArea,
                             UrlImage = _dbContext.RstProjectMedias.FirstOrDefault(o => o.ProjectId == project.Id && o.Deleted == YesNo.NO && o.Location == RstMediaLocations.ANH_DAI_DIEN_DU_AN).UrlImage,
                             TotalProductItem = _dbContext.RstProductItems.Count(o => project.Id == o.ProjectId && o.Deleted == YesNo.NO),
                             TotalProductItemSell = _dbContext.RstProductItems.Count(o => project.Id == o.ProjectId && listStatus.Contains(o.Status) && o.Deleted == YesNo.NO)
                         };
            return result.OrderByDescending(p => p.Id).Take(4).ToList();
        }

        // bieu đồ theo từng loại hình và mức bán
        public List<NumberOfApartmentsChartDto> NumberOfApartmentsChartTradingProvider(int? projectId, DateTime? startDate, DateTime? endDate, List<int> tradingProviderIds, List<int> listStatus)
        {
            var result = new List<NumberOfApartmentsChartDto>();

            var productItemSell = (from orders in _dbContext.RstOrders
                                   join openSellDetail in _dbContext.RstOpenSellDetails on orders.OpenSellDetailId equals openSellDetail.Id
                                   join productItem in _dbContext.RstProductItems on orders.ProductItemId equals productItem.Id
                                   where orders.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO 
                                         && (projectId == null || productItem.ProjectId == projectId)
                                         && (orders.Status == RstOrderStatus.DA_COC || orders.Status == RstOrderStatus.DA_BAN)
                                         && listStatus.Contains(openSellDetail.Status) && openSellDetail.Status == productItem.Status
                                         && tradingProviderIds.Contains(orders.TradingProviderId)
                                   select productItem);

            // lấy những căn chưa bán không join qua order
            var listProductItem = (from distribution in _dbContext.RstDistributions
                                   join distributionProductItem in _dbContext.RstDistributionProductItems on distribution.Id equals distributionProductItem.DistributionId
                                   join productItem in _dbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                                   where distribution.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO
                                         && productItem.Deleted == YesNo.NO && (projectId == null || distribution.ProjectId == projectId)
                                         && distribution.Status != RstDistributionStatus.HUY_DUYET
                                         && tradingProviderIds.Contains(distribution.TradingProviderId)
                                   select productItem);
            
            foreach (var item in RstClassifyType.All)
            {
                result.Add(new NumberOfApartmentsChartDto()
                {
                    ClassifyType = item,
                    TotalProductItem = listProductItem.Count(i => i.ClassifyType == item),
                    TotalProductItemSell = productItemSell.Count(i => i.ClassifyType == item)
                });
            }
            return result;
        }

        public List<NumberOfApartmentsChartDto> NumberOfApartmentsChartPartner(int? projectId, DateTime? startDate, DateTime? endDate, int partnerId, List<int> listStatus)
        {
            var result = new List<NumberOfApartmentsChartDto>();

            var productItemSell = (from order in _dbContext.RstOrders
                                   join productItem in _dbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                   join project in _dbContext.RstProjects on productItem.ProjectId equals project.Id
                                   where productItem.Deleted == YesNo.NO && order.Deleted == YesNo.NO
                                         && (startDate == null || (order.DepositDate.Value.Date >= startDate.Value.Date))
                                         && (projectId == null || project.Id == projectId)
                                         && (endDate == null || (order.DepositDate.Value.Date <= endDate.Value.Date))
                                         && listStatus.Contains(productItem.Status)
                                         && productItem.PartnerId == partnerId
                                   select productItem);

            // lấy những căn chưa bán không join qua order
            var listProductItem = (from productItem in _dbContext.RstProductItems
                                   join project in _dbContext.RstProjects on productItem.ProjectId equals project.Id
                                   where productItem.Deleted == YesNo.NO
                                   && productItem.PartnerId == partnerId && (projectId == null || project.Id == projectId)
                                   select productItem);

            foreach (var item in RstClassifyType.All)
            {
                result.Add(new NumberOfApartmentsChartDto()
                {
                    ClassifyType = item,
                    TotalProductItem = listProductItem.Count(i => i.ClassifyType == item),
                    TotalProductItemSell = productItemSell.Count(i => i.ClassifyType == item)
                });
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="tradingProviderIds"></param>
        /// <param name="listStatusProductItem"></param>
        /// <returns></returns>
        public List<SellChartOverTimeDto> BanHangTheoThoiGian(int? projectId, DateTime? startDate, DateTime? endDate, List<int> tradingProviderIds, List<int> listStatusProductItem)
        {
            var sellChartOverView = (from order in _dbContext.RstOrders
                                     join openSellDetail in _dbContext.RstOpenSellDetails on order.OpenSellDetailId equals openSellDetail.Id
                                     join productItem in _dbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                     where productItem.Deleted == YesNo.NO && listStatusProductItem.Contains(openSellDetail.Status) && openSellDetail.Status == productItem.Status
                                     && openSellDetail.Deleted == YesNo.NO && (projectId == null || (productItem.ProjectId == projectId))
                                     && order.DepositDate != null && (order.Status == RstOrderStatus.DA_COC || order.Status == RstOrderStatus.DA_BAN)
                                     && (startDate == null || startDate.Value.Date <= order.DepositDate.Value.Date)
                                     && (endDate == null || endDate.Value.Date >= order.DepositDate.Value.Date)
                                     && tradingProviderIds.Contains(order.TradingProviderId) && order.Deleted == YesNo.NO
                                     group productItem by new { order.DepositDate.Value.Date } into g
                                     select new { g.Key.Date, totalPrice = g.Sum(i => i.Price), total = g.Count() });

            var result = sellChartOverView.Select(x => new SellChartOverTimeDto
            {
                DepositDate = x.Date,
                Total = x.total,
                TotalPrice = x.totalPrice,
            });
            return result.OrderBy(i => i.DepositDate).ToList();
        }

        /// <summary>
        /// Tổng quan
        /// </summary>
        public DashboardOverViewDto TongQuanDaiLy(int? projectId, int tradingProviderId, List<int> listProductItemStatus)
        {

            var listProductItem = (from openSell in _dbContext.RstOpenSells
                                   join openSellDetail in _dbContext.RstOpenSellDetails on openSell.Id equals openSellDetail.OpenSellId
                                   join distributionProductItem in _dbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProductItem.Id
                                   join productItem in _dbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                                   join project in _dbContext.RstProjects on productItem.ProjectId equals project.Id
                                   where productItem.Deleted == YesNo.NO && openSell.TradingProviderId == tradingProviderId
                                   && project.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO && openSell.Deleted == YesNo.NO
                                   && (openSell.Status == RstDistributionStatus.DANG_BAN || openSell.Status == RstDistributionStatus.TAM_DUNG)
                                   && (projectId == null || project.Id == projectId)
                                   select new { productItem.Id, productItem.Status, productItem.ProjectId }).Distinct();

            var listProductItemSell = from order in _dbContext.RstOrders
                                      join productItem in _dbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                      join openSellDetail in _dbContext.RstOpenSellDetails on order.OpenSellDetailId equals openSellDetail.Id
                                      join project in _dbContext.RstProjects on productItem.ProjectId equals project.Id
                                      join cifCodes in _dbContext.CifCodes on order.CifCode equals cifCodes.CifCode
                                      join investor in _dbContext.Investors on cifCodes.InvestorId equals investor.InvestorId
                                      where order.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && project.Deleted == YesNo.NO 
                                      && order.TradingProviderId == tradingProviderId && investor.Deleted == YesNo.NO && (projectId == null || project.Id == projectId)
                                      && cifCodes.Deleted == YesNo.NO && (order.Status == RstOrderStatus.DA_COC || order.Status == RstOrderStatus.DA_BAN)
                                      && listProductItemStatus.Contains(productItem.Status) && productItem.Status == openSellDetail.Status
                                      select new { productItem, investor.InvestorId };

            var overView = new DashboardOverViewDto();
            overView.TotalProductItem = listProductItem.Count();
            overView.ProjectSell = listProductItem.Select(e => e.ProjectId).Distinct().Count();
            overView.TotalProductItemSell = listProductItemSell.Count();
            overView.Ratio = ((decimal)listProductItemSell.Count() / overView.TotalProductItem) * 100;

            overView.TotalCustomer = listProductItemSell.Select(o => o.InvestorId).Distinct().Count();
            overView.CustomerMaxsell = listProductItemSell.GroupBy(i => i.InvestorId).
                Select(i => new { investorId = i.Key, total = i.Count() }).OrderByDescending(i => i.total).FirstOrDefault()?.total ?? 0;
            return overView;
        }

        public DashboardOverViewDto TongQuanDoiTac(int? projectId, int partnerId, List<int> listStatus)
        {

            var listProductItem = from productItem in _dbContext.RstProductItems
                                  join project in _dbContext.RstProjects on productItem.ProjectId equals project.Id
                                  where productItem.Deleted == YesNo.NO && project.PartnerId == partnerId && project.Deleted == YesNo.NO 
                                  && (projectId == null || project.Id == projectId) && project.Status == DistributionStatus.HOAT_DONG
                                  select productItem;

            var listProductItemSell = from order in _dbContext.RstOrders
                                      join productItem in _dbContext.RstProductItems on order.ProductItemId equals productItem.Id
                                      join project in _dbContext.RstProjects on productItem.ProjectId equals project.Id
                                      join cifCodes in _dbContext.CifCodes on order.CifCode equals cifCodes.CifCode
                                      join investor in _dbContext.Investors on cifCodes.InvestorId equals investor.InvestorId
                                      where order.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && project.PartnerId == partnerId
                                      && project.Deleted == YesNo.NO && cifCodes.Deleted == YesNo.NO && project.Status == DistributionStatus.HOAT_DONG
                                      && listStatus.Contains(productItem.Status) && investor.Deleted == YesNo.NO
                                      && (projectId == null || project.Id == projectId) && (order.Status == RstOrderStatus.DA_COC || order.Status == RstOrderStatus.DA_BAN)
                                      select new { productItem, investor.InvestorId };

            var overview = new DashboardOverViewDto();

            overview.TotalProductItem = listProductItem.Count();
            overview.ProjectSell = listProductItem.Select(i => i.ProjectId).Distinct().Count();

            overview.TotalProductItemSell = listProductItemSell.Count();
            overview.Ratio = ((decimal)overview.TotalProductItemSell / overview.TotalProductItem) * 100;

            overview.TotalCustomer = listProductItemSell.Select(o => o.InvestorId).Distinct().Count();
            overview.CustomerMaxsell = listProductItemSell.GroupBy(i => i.InvestorId).
                Select(i => new { investorId = i.Key, TotalContract = i.Count() }).OrderByDescending(i => i.TotalContract).FirstOrDefault()?.TotalContract ?? 0;
            return overview;
        }

        public List<RstDashboardActionsDto> DashboardGetNewAction(List<int> tradingProviderIds)
        {
            var actionAddOrder = (from order in _dbContext.RstOrders.AsNoTracking().Where(x => tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId) && x.Deleted == YesNo.NO)
                                  from iden in _dbContext.InvestorIdentifications.AsNoTracking().Where(x => x.Id == order.InvestorIdenId && x.Deleted == YesNo.NO && x.IsDefault == YesNo.YES).DefaultIfEmpty()
                                  from investor in _dbContext.Investors.AsNoTracking().Where(x => x.InvestorId == iden.InvestorId && x.Deleted == YesNo.NO)
                                  orderby order.CreatedDate descending
                                  select new RstDashboardActionsDto
                                  {
                                      Action = RstDashboardActions.DAT_LENH_DAU_TU_MOI,
                                      Avatar = investor.AvatarImageUrl,
                                      Fullname = iden.Fullname,
                                      CreatedDate = order.CreatedDate,
                                      OrderId = order.Id,
                                  });

            var result = actionAddOrder.OrderByDescending(o => o.CreatedDate).Take(7);
            return result.ToList();
        }

        /// <summary>
        /// danh sách dự án theo tài khoản đăng nhập
        /// </summary>
        /// <returns></returns>
        public List<RstListProjectDashboardDto> GetListProject()
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            var result = new List<RstListProjectDashboardDto>();
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                result = _dbContext.RstProjects.Where(r => _dbContext.RstOpenSells.Any(o => o.TradingProviderId == tradingProviderId && r.Id == o.ProjectId && o.Deleted == YesNo.NO
                                && o.Status != RstDistributionStatus.HUY_DUYET) && r.Deleted == YesNo.NO)
                                .Select(x => new RstListProjectDashboardDto
                                {
                                    Name = x.Name,
                                    ProjectId = x.Id
                                }).ToList();
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                result = _dbContext.RstProjects.Where(r => r.PartnerId == partnerId && r.Deleted == YesNo.NO)
                            .Select(x => new RstListProjectDashboardDto
                            {
                                Name = x.Name,
                                ProjectId = x.Id
                            }).ToList();
            }
            return result;
        }
    }
}

using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.Dto.RstExportExcel;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.RealEstateRepositories
{
    public class RstExportExcelReportRepositories
    {
        private readonly ILogger _logger;
        private readonly EpicSchemaDbContext _dbContext;
        public RstExportExcelReportRepositories(DbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext as EpicSchemaDbContext;
            _logger = logger;
        }

        public IQueryable<RstProductProjectOverviewDto> ProductProjectOverviewTradingProvider(int? tradingProvider, DateTime? startDate, DateTime? endDate)
        {
            var result = from openSell in _dbContext.RstOpenSells
                         join openSellDetail in _dbContext.RstOpenSellDetails on openSell.Id equals openSellDetail.OpenSellId
                         join distributionProductItem in _dbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProductItem.Id
                         join productItem in _dbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                         join projectStructure in _dbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructure.Id
                         join project in _dbContext.RstProjects on projectStructure.ProjectId equals project.Id
                         join approve in _dbContext.RstApproves on project.Id equals approve.ReferId
                         where openSell.Deleted == YesNo.NO && openSell.TradingProviderId == tradingProvider && openSellDetail.Deleted == YesNo.NO
                         && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                         && projectStructure.Deleted == YesNo.NO && project.Status == DistributionStatus.HOAT_DONG
                         && approve.DataType == RstApproveDataTypes.RST_PROJECT && approve.Status == ApproveStatus.DUYET && approve.Deleted == YesNo.NO
                         && (startDate == null || (approve.ApproveDate.Value.Date >= startDate.Value.Date))
                         && (endDate == null || (approve.ApproveDate.Value.Date <= endDate.Value.Date))
                         group new { openSell, productItem.BuildingDensityId } by new
                         {
                             openSell.TradingProviderId,
                             productItem.ClassifyType,
                             productItem.Status,
                             project.Code,
                             productItem.BuildingDensityId,
                             projectStructure.BuildingDensityType,
                             projectStructure.Name
                         } into p
                         select new RstProductProjectOverviewDto()
                         {
                             Code = p.Key.Code,
                             BuildingDensityType = p.Key.BuildingDensityType,
                             ClassifyType = p.Key.ClassifyType,
                             Status = p.Key.Status,
                             BuildingDensityId = p.Key.BuildingDensityId,
                             BuildingDensityName = p.Key.Name
                         };
            return result.OrderByDescending(o => o.Code);
        }

        public IQueryable<RstProductProjectOverviewDto> ProductProjectOverviewPartner(int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from productItem in _dbContext.RstProductItems
                          join projectStructure in _dbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructure.Id
                          join project in _dbContext.RstProjects on projectStructure.ProjectId equals project.Id
                          join approve in _dbContext.RstApproves on project.Id equals approve.ReferId
                          where productItem.Deleted == YesNo.NO && projectStructure.Deleted == YesNo.NO && project.PartnerId == partnerId
                         && approve.DataType == RstApproveDataTypes.RST_PROJECT && approve.Status == ApproveStatus.DUYET && approve.Deleted == YesNo.NO
                         && (startDate == null || (approve.ApproveDate.Value.Date >= startDate.Value.Date))
                         && (endDate == null || (approve.ApproveDate.Value.Date <= endDate.Value.Date))
                          group new { productItem, projectStructure, project } by new
                          {
                              productItem.Id,
                              productItem.ClassifyType,
                              productItem.Status,
                              project.Code,
                              productItem.BuildingDensityId,
                              projectStructure.BuildingDensityType,
                              projectStructure.Name
                          } into p
                          select new RstProductProjectOverviewDto()
                          {
                              Code = p.Key.Code,
                              BuildingDensityType = p.Key.BuildingDensityType,
                              ClassifyType = p.Key.ClassifyType,
                              Status = p.Key.Status,
                              BuildingDensityId = p.Key.BuildingDensityId,
                              BuildingDensityName = p.Key.Name
                          });

            return result.OrderByDescending(o => o.Code);
        }

        public IQueryable<RstSyntheticMoneyProjectDto> SyntheticMoneyProjectTradingProvider(int? tradingProvider, DateTime? startDate, DateTime? endDate)
        {
            var result = from openSell in _dbContext.RstOpenSells
                         join openSellDetail in _dbContext.RstOpenSellDetails on openSell.Id equals openSellDetail.OpenSellId
                         join distributionProductItem in _dbContext.RstDistributionProductItems on openSellDetail.DistributionProductItemId equals distributionProductItem.Id
                         join productItem in _dbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                         join order in _dbContext.RstOrders on productItem.Id equals order.ProductItemId
                         join orderPayment in _dbContext.RstOrderPayments on order.Id equals orderPayment.OrderId
                         join projectStructure in _dbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructure.Id
                         join project in _dbContext.RstProjects on projectStructure.ProjectId equals project.Id
                         join approve in _dbContext.RstApproves on project.Id equals approve.ReferId
                         where openSell.Deleted == YesNo.NO && openSell.TradingProviderId == tradingProvider && openSellDetail.Deleted == YesNo.NO
                         && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                         && projectStructure.Deleted == YesNo.NO && project.Status == DistributionStatus.HOAT_DONG
                         && orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN && orderPayment.Deleted == YesNo.NO
                         && order.Status == OrderStatus.DANG_DAU_TU
                         && approve.DataType == RstApproveDataTypes.RST_PROJECT && approve.Status == ApproveStatus.DUYET && approve.Deleted == YesNo.NO
                         && (startDate == null || (approve.ApproveDate.Value.Date >= startDate.Value.Date))
                         && (endDate == null || (approve.ApproveDate.Value.Date <= endDate.Value.Date))
                         group new { openSell, productItem.BuildingDensityId, orderPayment } by new
                         {
                             openSell.TradingProviderId,
                             productItem.ClassifyType,
                             productItem.Status,
                             project.Code,
                             projectStructure.BuildingDensityType,
                             orderStatus = order.Status,
                         } into p
                         select new RstSyntheticMoneyProjectDto
                         {
                             Code = p.Key.Code,
                             BuildingDensityType = p.Key.BuildingDensityType,
                             ClassifyType = p.Key.ClassifyType,
                             Status = p.Key.Status,
                             OrderStatus = p.Key.orderStatus,
                             PaymentAmount = p.Sum(i => i.orderPayment.PaymentAmount),
                         };
            return result;
        }

        public IQueryable<RstSyntheticMoneyProjectDto> SyntheticMoneyProjectPartner(int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = from productItem in _dbContext.RstProductItems
                         join order in _dbContext.RstOrders on productItem.Id equals order.ProductItemId
                         join orderPayment in _dbContext.RstOrderPayments on order.Id equals orderPayment.OrderId
                         join projectStructure in _dbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructure.Id
                         join project in _dbContext.RstProjects on projectStructure.ProjectId equals project.Id
                         join approve in _dbContext.RstApproves on project.Id equals approve.ReferId
                         where productItem.Deleted == YesNo.NO && projectStructure.Deleted == YesNo.NO && project.PartnerId == partnerId
                        && approve.DataType == RstApproveDataTypes.RST_PROJECT && approve.Status == ApproveStatus.DUYET && approve.Deleted == YesNo.NO
                        && productItem.Status == RstProductItemStatus.DA_COC
                        && orderPayment.Status == OrderPaymentStatus.DA_THANH_TOAN && orderPayment.Deleted == YesNo.NO
                        && order.Status == OrderStatus.DANG_DAU_TU
                        && (startDate == null || (approve.ApproveDate.Value.Date >= startDate.Value.Date))
                        && (endDate == null || (approve.ApproveDate.Value.Date <= endDate.Value.Date))
                         group new { productItem, projectStructure, orderPayment } by new
                         {
                             productItem.Id,
                             productItem.ClassifyType,
                             productItem.Status,
                             project.Code,
                             projectStructure.BuildingDensityType,
                             orderStatus = order.Status,
                         } into p
                         select new RstSyntheticMoneyProjectDto
                         {
                             Code = p.Key.Code,
                             BuildingDensityType = p.Key.BuildingDensityType,
                             ClassifyType = p.Key.ClassifyType,
                             Status = p.Key.Status,
                             OrderStatus = p.Key.orderStatus,
                             PaymentAmount = p.Sum(i => i.orderPayment.PaymentAmount),
                         };
            return result;
        }

        /// <summary>
        /// tổng hợp các khoản giao dịch
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IQueryable<RstSyntheticTradingDto> SyntheticTrading(int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            var result = from orders in _dbContext.RstOrders
                         join cifcodes in _dbContext.CifCodes on orders.CifCode equals cifcodes.CifCode
                         join orderPayments in _dbContext.RstOrderPayments on orders.Id equals orderPayments.OrderId
                         join productItem in _dbContext.RstProductItems on orders.ProductItemId equals productItem.Id
                         join projectStructure in _dbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructure.Id
                         join project in _dbContext.RstProjects on projectStructure.ProjectId equals project.Id
                         where orders.Deleted == YesNo.NO
                         && (tradingProviderId == null || orders.TradingProviderId == tradingProviderId)
                         && cifcodes.Deleted == YesNo.NO
                         && orderPayments.Deleted == YesNo.NO && orderPayments.Status == OrderPaymentStatus.DA_THANH_TOAN
                         && productItem.Deleted == YesNo.NO
                         && projectStructure.Deleted == YesNo.NO
                         && project.Deleted == YesNo.NO
                         group new { orders, orderPayments } by new
                         {
                             orders.ContractCode,
                             orders.CreatedDate,
                             project.Code,
                             projectStructure.BuildingDensityType,
                             projectStructure.Name,
                             cifcodes.CifCode,
                             orders.SaleReferralCode,
                             orderPayments.PaymentType,
                             orderPayments.TranClassify,
                             productItem.Status,
                             productItem.CarpetArea,
                             productItem.BuiltUpArea,
                             productItem.Price,
                             productItem.ClassifyType,
                             orders.TradingProviderId,
                             orderPayments.PaymentAmount
                         } into p
                         select new RstSyntheticTradingDto
                         {
                             CreatedDate = p.Key.CreatedDate,
                             ContractCode = p.Key.ContractCode,
                             ProjectCode = p.Key.Code,
                             BuildingDensityType = p.Key.BuildingDensityType,
                             BuildingDensityName = p.Key.Name,
                             CifCode = p.Key.CifCode,
                             SaleReferralCode = p.Key.SaleReferralCode,
                             TranClassify = p.Key.TranClassify,
                             PaymentType = p.Key.PaymentType,
                             ClassifyType = p.Key.ClassifyType,
                             ProductItemStatus = p.Key.Status,
                             CarpetArea = p.Key.CarpetArea,
                             BuiltUpArea = p.Key.BuiltUpArea,
                             Price = p.Key.Price,
                             TradingProviderId = p.Key.TradingProviderId,
                             PaymentAmount = p.Sum( i => i.orderPayments.PaymentAmount)
                         };
            return result;
        }
    }
}
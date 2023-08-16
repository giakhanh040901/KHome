using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerOrderPayment;
using EPIC.GarnerEntities.Dto;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EPIC.Entities;
using EPIC.GarnerEntities.Dto.GarnerDashboard;
using EPIC.Utils.ConstantVariables.Core;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Garner;
using System.Globalization;
using EPIC.InvestEntities.DataEntities;

namespace EPIC.GarnerRepositories
{
    public class GarnerOrderPaymentEFRepository : BaseEFRepository<GarnerOrderPayment>
    {
        public GarnerOrderPaymentEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerOrderPayment.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mới thanh toán
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerOrderPayment Add(GarnerOrderPayment entity, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GarnerOrderPaymentEFRepository)} =>: {nameof(Add)}: input = {(entity != null ? JsonSerializer.Serialize(entity) : "")}; username: {username}; tradingProviderId: {tradingProviderId}");
            entity.Id = (long)NextKey();
            entity.TradingProviderId = tradingProviderId;
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            return _dbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerOrderPayment Update(GarnerOrderPayment input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GarnerOrderPaymentEFRepository)} => {nameof(Update)}: input: {JsonSerializer.Serialize(input)}; username: {username}");
            var orderPayment = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (orderPayment != null)
            {
                orderPayment.OrderId = input.OrderId;
                orderPayment.TranDate = input.TranDate;
                orderPayment.TranType = input.TranType;
                orderPayment.TranClassify = input.TranClassify;
                orderPayment.PaymentType = input.PaymentType;
                orderPayment.PaymentAmount = input.PaymentAmount;
                orderPayment.Description = input.Description;
                orderPayment.ModifiedBy = username;
                orderPayment.ModifiedDate = DateTime.Now;
            }
            return orderPayment;
        }

        /// <summary>
        /// Tìm thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerOrderPayment FindById(long id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerOrderPaymentEFRepository)} => {nameof(FindById)}: Id= {id}, tradingProviderId={tradingProviderId} ");

            var result =  _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) &&d.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Lấy danh sách thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public PagingResult<GarnerOrderPayment> FindAll(FilterOrderPaymentDto input)
        {
            _logger.LogInformation($"{nameof(GarnerOrderPaymentEFRepository)} -> {nameof(FindAll)}: input: {JsonSerializer.Serialize(input)};");

            PagingResult<GarnerOrderPayment> result = new();
            var orderPaymentQuery = _dbSet.Where(op => op.OrderId == input.OrderId && op.Deleted == YesNo.NO
                                            && (input.Status == 0 || input.Status == op.Status));
            orderPaymentQuery = orderPaymentQuery.OrderByDescending(o => o.Id);
            result.TotalItems = orderPaymentQuery.Count();

            if (input.PageSize != -1)
            {
                orderPaymentQuery = orderPaymentQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = orderPaymentQuery.ToList();
            return result;
        }

        /// <summary>
        /// Lấy danh sách thanh toán không phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<GarnerOrderPayment> GetAll(long orderId, int? status = null, int? tranType = null)
        {
            _logger.LogInformation($"{nameof(GarnerOrderPaymentEFRepository)} -> {nameof(GetAll)}: status: {status}; orderId: {orderId}");

            var orderPaymentQuery = _dbSet.Where(op => op.OrderId == orderId && op.Deleted == YesNo.NO
                                            && (status == null || op.Status == status)
                                            && (tranType == null || op.TranType == tranType));
            return orderPaymentQuery.ToList();
        }

        public decimal SumPaymentAmount(long orderId)
        {
            _logger.LogInformation($"{nameof(GarnerOrderPaymentEFRepository)} => {nameof(SumPaymentAmount)}: orderId: {orderId}");
            return _dbSet.Where(p => p.OrderId == orderId && p.TranClassify == TranClassifies.THANH_TOAN && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.TranType == TranTypes.THU && p.Deleted == YesNo.NO)
                .Sum(p => p.PaymentAmount);
        }

        public GarnerOrderPayment GetFirstPayment(long orderId)
        {
            _logger.LogInformation($"{nameof(GarnerOrderPaymentEFRepository)} -> {nameof(GetFirstPayment)}: orderId = {orderId}");

            List<int> orderPaymentStatus = new List<int>()
            {
                OrderPaymentStatus.NHAP,
                OrderPaymentStatus.DA_THANH_TOAN
            };
            return _dbSet.FirstOrDefault(op => op.OrderId == orderId && op.Deleted == YesNo.NO && op.TranType == TranTypes.THU
                                            && orderPaymentStatus.Contains(op.Status));
        }

        /// <summary>
        /// Tính doanh số theo chính sách (Tạm thời chỉ tính với Garner type = 1 (Ko kỳ hạn/Linh hoạt))
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<GarnerDashboardPolicyDto> CalculatePaymentByPolicy(List<int> tradingProviderIds, int? productId)
        {
            _logger.LogInformation($"{nameof(CalculatePaymentByPolicy)}: tradingProviderIds={JsonSerializer.Serialize(tradingProviderIds)}; productId={productId}");

            var result = (from policy in _epicSchemaDbContext.GarnerPolicies.AsNoTracking().Where(x => x.Deleted == YesNo.NO && (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId)))
                          from order in _epicSchemaDbContext.GarnerOrders.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.PolicyId == policy.Id && (productId == null || x.ProductId == productId) && (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId))).DefaultIfEmpty()
                          from payment in _epicSchemaDbContext.GarnerOrderPayments.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.OrderId == order.Id && x.TranType == TranTypes.THU && x.Status == OrderPaymentStatus.DA_THANH_TOAN).DefaultIfEmpty()
                          select new
                          {
                              garnerType = policy.GarnerType,
                              //periodQuantity = policy.InterestPeriodQuantity,
                              //periodType = policy.InterestPeriodType,
                              amount = payment.PaymentAmount,
                          })
                         .GroupBy(x => new
                         {
                             //x.periodType,
                             //x.periodQuantity,
                             x.garnerType
                         })
                         .Select(x => new GarnerDashboardPolicyDto
                         {
                             Name = x.Key.garnerType == PolicyGarnerTypes.LINH_HOAT ? "Không kỳ hạn" : "",
                             Value = x.Sum(s => s.amount),
                         })
                         .ToList();

            return result;
        }

        /// <summary>
        /// Tính tiền vào theo đại lý (Hiển thị từng phòng ban trên dashboard)
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<GarnerDashboardCashInByTrading> CalculateCashInByTrading(int tradingProviderId, DateTime startDate, DateTime endDate, int? productId)
        {
            var query = (
                        from departmentSale in (from ds in _epicSchemaDbContext.DepartmentSales.AsNoTracking().Where(x => x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO) select new DepartmentSale { DepartmentId = ds.DepartmentId }).Distinct()
                        from department in _epicSchemaDbContext.Departments.AsNoTracking().Where(x => x.DepartmentId == departmentSale.DepartmentId && x.Deleted == YesNo.NO)
                        from order in _epicSchemaDbContext.GarnerOrders.AsNoTracking().Where(x => x.DepartmentId == department.DepartmentId && x.Deleted == YesNo.NO && (productId == null || x.ProductId == productId) && x.PaymentFullDate != null && startDate.Date <= x.InvestDate.Value.Date && x.InvestDate.Value.Date <= endDate.Date).DefaultIfEmpty()
                        from orderPayment in _epicSchemaDbContext.GarnerOrderPayments.AsNoTracking().Where(x => x.OrderId == order.Id && x.TranType == TranTypes.THU && x.Status == OrderPaymentStatus.DA_THANH_TOAN && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        select new
                        {
                            depId = department.DepartmentId,
                            depName = department.DepartmentName,
                            amount = orderPayment.PaymentAmount
                        }
                        ).GroupBy(x => new
                        {
                            x.depId,
                            x.depName
                        })
                        .Select(x => new GarnerDashboardCashInByTrading
                        {
                            DepartmentId = x.Key.depId,
                            DepartmentName = x.Key.depName,
                            Amout = x.Sum(y => y.amount)
                        })
                        .OrderByDescending(x => x.Amout);

            return query.ToList();
        }

        /// <summary>
        /// Tính tiền vào theo partner (Hiển thị từng đại lý trên dashboard)
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<GarnerDashboardCashInByPartner> CalculateCashInByPartner(List<int> tradingProviderIds, DateTime startDate, DateTime endDate, int? productId)
        {
            var query = (
                        from trading in _epicSchemaDbContext.TradingProviders.AsNoTracking().Where(x => (tradingProviderIds.Count > 0 || tradingProviderIds.Contains(x.TradingProviderId)) && x.Deleted == YesNo.NO)
                        from bus in _epicSchemaDbContext.BusinessCustomers.AsNoTracking().Where(x => trading.BusinessCustomerId == x.BusinessCustomerId && x.Deleted == YesNo.NO)
                        from order in _epicSchemaDbContext.GarnerOrders.AsNoTracking().Where(x => x.TradingProviderId == trading.TradingProviderId && (productId == null || x.ProductId == productId) && x.PaymentFullDate != null && startDate.Date <= x.PaymentFullDate.Value.Date && x.PaymentFullDate.Value.Date <= endDate.Date && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        from orderPayment in _epicSchemaDbContext.GarnerOrderPayments.AsNoTracking().Where(x => x.OrderId == order.Id && x.TranType == TranTypes.THU && x.Status == OrderPaymentStatus.DA_THANH_TOAN && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        select new
                        {
                            tradingId = trading.TradingProviderId,
                            tradingName = bus.Name,
                            shortName = bus.ShortName,
                            amount = orderPayment.PaymentAmount
                        }
                        ).GroupBy(x => new
                        {
                            x.tradingId,
                            x.tradingName,
                            x.shortName,
                        })
                        .Select(x => new GarnerDashboardCashInByPartner
                        {
                            TradingProviderId = x.Key.tradingId,
                            TradingProviderName = x.Key.tradingName,
                            TradingProviderShortName = x.Key.shortName,
                            Amout = x.Sum(y => y.amount)
                        })
                        .OrderByDescending(x => x.Amout)
                        ;

            return query.ToList();
        }

        /// <summary>
        /// Tính thực chi theo tháng
        /// Tổng giá trị khoản tiền rút thành công của khách + lợi tức rút + lợi túc chi trả
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public decimal CalculateCashOutByMonth(List<int> tradingProviderIds, int month)
        {
            decimal result = 0;
            // Lấy giá trị được rút hoặc chi được lập sau khi duyệt ở bảng Thanh toán
            result = (from payment in _epicSchemaDbContext.GarnerOrderPayments.AsNoTracking()
                      join order in _epicSchemaDbContext.GarnerOrders.AsNoTracking() on payment.OrderId equals order.Id
                      where payment.Deleted == YesNo.NO && payment.Status == OrderPaymentStatus.DA_THANH_TOAN && payment.TranType == TranTypes.CHI
                      && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(payment.TradingProviderId))
                      && order.Deleted == YesNo.NO && (payment.ApproveDate != null && payment.ApproveDate.Value.Date.Month == month)
                      select payment).Sum(p => p.PaymentAmount);

            return result;
        }

    }
}

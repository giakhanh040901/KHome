using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerDashboard;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerWithdrawalEFRepository : BaseEFRepository<GarnerWithdrawal>
    {
        public GarnerWithdrawalEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerWithdrawal.SEQ}")
        {
        }

        public GarnerWithdrawal Add(GarnerWithdrawal input)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (long)NextKey();
            input.Status = WithdrawalStatus.YEU_CAU;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Lấy danh sách lệnh phải rút kèm số tiền rút
        /// </summary>
        /// <param name="cifCode"></param>
        /// <param name="policyId"></param>
        /// <param name="amount"></param>
        public List<GarnerOrderWithdrawalDto> CalOrderWithdraw(string cifCode, int policyId, decimal amount)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(CalOrderWithdraw)}: cifCode = {cifCode}, policyId = {policyId}, amount = {amount}");
            var policy = _epicSchemaDbContext.GarnerPolicies.FirstOrDefault(p => p.Id == policyId && p.Deleted == YesNo.NO)
                .ThrowIfNull<GarnerPolicy>(_epicSchemaDbContext, ErrorCode.GarnerPolicyNotFound);

            //tất cả lệnh đang đầu tư
            var orderQuery = _epicSchemaDbContext.GarnerOrders
                .Where(o => o.CifCode == cifCode && o.PolicyId == policyId && o.Status == OrderStatus.DANG_DAU_TU && o.Deleted == YesNo.NO);

            List<GarnerOrderWithdrawalDto> orderWithdraw = new(); //các lệnh rút
            List<GarnerOrder> orders = new();
            //sắp xếp thứ tự lệnh
            if (policy.OrderOfWithdrawal == GarnerOrderOfWithdrawals.MOI_NHAT_DEN_CU_NHAT)
            {
                orders = orderQuery.OrderByDescending(o => o.InvestDate).ToList();
            }
            else if (policy.OrderOfWithdrawal == GarnerOrderOfWithdrawals.CU_NHAT_DEN_MOI_NHAT)
            {
                orders = orderQuery.OrderBy(o => o.InvestDate).ToList();
            }
            else if (policy.OrderOfWithdrawal == GarnerOrderOfWithdrawals.GIA_TRI_GAN_NHAT_GIA_TRI_RUT)
            {

            }

            decimal amountRemain = amount;
            foreach (var order in orders)
            {
                if (order.TotalValue > amountRemain) //lớn hơn
                {
                    orderWithdraw.Add(new GarnerOrderWithdrawalDto
                    {
                        Order = order,
                        AmountMoney = amountRemain
                    });
                    amountRemain = 0;
                    break;
                }
                else if (order.TotalValue == amountRemain) //bằng
                {
                    orderWithdraw.Add(new GarnerOrderWithdrawalDto
                    {
                        Order = order,
                        AmountMoney = order.TotalValue
                    });
                    //order.TotalValue = 0;
                    //order.Status = OrderStatus.TAT_TOAN;
                    amountRemain = 0;
                    break;
                }
                else //lệnh có tiền nhỏ hơn tiền cần rút
                {
                    amountRemain -= order.TotalValue;
                    orderWithdraw.Add(new GarnerOrderWithdrawalDto
                    {
                        Order = order,
                        AmountMoney = order.TotalValue
                    });
                    //order.TotalValue = 0;
                    //order.Status = OrderStatus.TAT_TOAN;
                }

                if (amountRemain == 0) //đủ tiền cần rút
                {
                    break;
                }
            }

            return orderWithdraw;
        }

        /// <summary>
        /// Các lệnh rút
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <returns></returns>
        public List<GarnerOrderWithdrawalDto> GetOrderWithdrawal(long withdrawalId)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(GetOrderWithdrawal)}: withdrawalId = {withdrawalId}");
            var withdrawals = _dbSet.FirstOrDefault(w => w.Id == withdrawalId && w.Deleted == YesNo.NO)
                .ThrowIfNull<GarnerWithdrawal>(_epicSchemaDbContext, ErrorCode.GarnerOrderWithdrawalNotFound);

            var query = from orderWithdrawal in _epicSchemaDbContext.GarnerWithdrawalDetails
                        join order in _epicSchemaDbContext.GarnerOrders on orderWithdrawal.OrderId equals order.Id
                        where orderWithdrawal.WithdrawalId == withdrawalId && order.Deleted == YesNo.NO
                        select new GarnerOrderWithdrawalDto
                        {
                            Order = order,
                            AmountMoney = orderWithdrawal.AmountMoney
                        };
            return query.ToList();
        }

        /// <summary>
        /// Các lệnh rút theo orderId
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerOrderWithdrawalDto GetOrderWithdrawalDetail(long withdrawalId, long orderId)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(GetOrderWithdrawalDetail)}: withdrawalId = {withdrawalId}, orderId = {orderId}");
            var query = from orderWithdrawal in _epicSchemaDbContext.GarnerWithdrawalDetails
                        join order in _epicSchemaDbContext.GarnerOrders on orderWithdrawal.OrderId equals order.Id
                        where orderWithdrawal.WithdrawalId == withdrawalId && orderWithdrawal.OrderId == orderId && order.Deleted == YesNo.NO
                        select new GarnerOrderWithdrawalDto
                        {
                            Order = order,
                            AmountMoney = orderWithdrawal.AmountMoney,
                        };
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy thông tin rút vốn theo chính sách trong trạng thái yêu cầu chưa chi trả
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public GarnerWithdrawalByPolicyDto GetOrderWithdrawalByPolicyId(int investorId, int policyId, List<int> status)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(GetOrderWithdrawalByPolicyId)}: investorId = {investorId}, policyId = {policyId}, status = {status}");
            var result = new GarnerWithdrawalByPolicyDto();

            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                return null;
            }
            var withdrawalFind = _dbSet.FirstOrDefault(w => w.PolicyId == policyId && w.CifCode == cifCodeFind.CifCode && status.Contains(w.Status) && w.Deleted == YesNo.NO);
            if (withdrawalFind == null)
            {
                return null;
            }

            result.WithdrawalId = withdrawalFind.Id;
            result.AmountMoney = withdrawalFind.AmountMoney;
            result.Status = withdrawalFind.Status;
            result.CreatedDate = withdrawalFind.CreatedDate;
            result.WithdrawalDate = withdrawalFind.WithdrawalDate;

            result.WithdrawalDetails = (from orderWithdrawal in _epicSchemaDbContext.GarnerWithdrawalDetails
                                        join order in _epicSchemaDbContext.GarnerOrders on orderWithdrawal.OrderId equals order.Id
                                        where orderWithdrawal.WithdrawalId == withdrawalFind.Id && order.Deleted == YesNo.NO
                                        select new GarnerOrderWithdrawalDto
                                        {
                                            Order = order,
                                            AmountMoney = orderWithdrawal.AmountMoney,
                                            Tax = orderWithdrawal.Tax,
                                            ActuallyProfit = orderWithdrawal.ActuallyProfit,
                                            AmountReceived = orderWithdrawal.AmountReceived,
                                            DeductibleProfit = orderWithdrawal.DeductibleProfit,
                                            Profit = orderWithdrawal.Profit,
                                            WithdrawalFee = orderWithdrawal.WithdrawalFee,
                                        }).ToList();
            return result;
        }

        /// <summary>
        /// Danh sách rút vốn theo chính sách của nhà đầu tư
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="policyId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<GarnerWithdrawalByPolicyDto> GetListWithdrawalByPolicyId(int investorId, int policyId, List<int> status)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(GetListWithdrawalByPolicyId)}: investorId = {investorId}, policyId = {policyId}");
            var result = new List<GarnerWithdrawalByPolicyDto>();

            var cifCodeFind = _epicSchemaDbContext.CifCodes.FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO);
            if (cifCodeFind == null)
            {
                return null;
            }
            var withdrawalFind = _dbSet.Where(w => w.PolicyId == policyId && w.CifCode == cifCodeFind.CifCode && status.Contains(w.Status) && w.Deleted == YesNo.NO);

            foreach (var withdrawalItem in withdrawalFind)
            {
                var resultItem = new GarnerWithdrawalByPolicyDto();
                resultItem.WithdrawalId = withdrawalItem.Id;
                resultItem.AmountMoney = withdrawalItem.AmountMoney;
                resultItem.Status = withdrawalItem.Status;
                resultItem.CreatedDate = withdrawalItem.CreatedDate;
                resultItem.WithdrawalDate = withdrawalItem.WithdrawalDate;
                resultItem.ApproveDate = withdrawalItem.ApproveDate;
                resultItem.CancelDate = withdrawalItem.CancelDate;
                resultItem.CancelBy = withdrawalItem.CancelBy;
                resultItem.WithdrawalDetails = (from orderWithdrawal in _epicSchemaDbContext.GarnerWithdrawalDetails
                                                join order in _epicSchemaDbContext.GarnerOrders on orderWithdrawal.OrderId equals order.Id
                                                where orderWithdrawal.WithdrawalId == withdrawalItem.Id && order.Deleted == YesNo.NO
                                                select new GarnerOrderWithdrawalDto
                                                {
                                                    Order = order,
                                                    AmountMoney = orderWithdrawal.AmountMoney,
                                                    Tax = orderWithdrawal.Tax,
                                                    ActuallyProfit = orderWithdrawal.ActuallyProfit,
                                                    AmountReceived = orderWithdrawal.AmountReceived,
                                                    DeductibleProfit = orderWithdrawal.DeductibleProfit,
                                                    Profit = orderWithdrawal.Profit,
                                                    WithdrawalFee = orderWithdrawal.WithdrawalFee,
                                                }).ToList();
                result.Add(resultItem);
            }
            return result;
        }

        public GarnerWithdrawalByPolicyDto GetOrderWithdrawalById(long withdrawalId)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(GetOrderWithdrawalById)}: withdrawalId = {withdrawalId}");
            var result = new GarnerWithdrawalByPolicyDto();

            var withdrawalFind = _dbSet.FirstOrDefault(w => w.Id == withdrawalId && w.Deleted == YesNo.NO);
            if (withdrawalFind == null)
            {
                return null;
            }

            result.WithdrawalId = withdrawalFind.Id;
            result.AmountMoney = withdrawalFind.AmountMoney;
            result.Source = withdrawalFind.Source;
            result.Status = withdrawalFind.Status;
            result.CreatedDate = withdrawalFind.CreatedDate;
            result.ApproveDate = withdrawalFind.ApproveDate;
            result.WithdrawalDate = withdrawalFind.WithdrawalDate;

            result.WithdrawalDetails = (from orderWithdrawal in _epicSchemaDbContext.GarnerWithdrawalDetails
                                        join order in _epicSchemaDbContext.GarnerOrders on orderWithdrawal.OrderId equals order.Id
                                        where orderWithdrawal.WithdrawalId == withdrawalFind.Id && order.Deleted == YesNo.NO
                                        select new GarnerOrderWithdrawalDto
                                        {
                                            Order = order,
                                            AmountMoney = orderWithdrawal.AmountMoney,
                                            Tax = orderWithdrawal.Tax,
                                            ActuallyProfit = orderWithdrawal.ActuallyProfit,
                                            AmountReceived = orderWithdrawal.AmountReceived,
                                            DeductibleProfit = orderWithdrawal.DeductibleProfit,
                                            Profit = orderWithdrawal.Profit,
                                            WithdrawalFee = orderWithdrawal.WithdrawalFee,
                                        }).ToList();
            result.Tax = result.WithdrawalDetails?.Select(w => w.Tax).Sum() ?? 0;
            result.ActuallyProfit = result.WithdrawalDetails?.Select(w => w.ActuallyProfit).Sum() ?? 0;
            result.AmountReceived = result.WithdrawalDetails?.Select(w => w.AmountReceived).Sum() ?? 0;
            result.DeductibleProfit = result.WithdrawalDetails?.Select(w => w.DeductibleProfit).Sum() ?? 0;
            result.Profit = result.WithdrawalDetails?.Select(w => w.Profit).Sum() ?? 0;
            result.WithdrawalFee = result.WithdrawalDetails?.Select(w => w.WithdrawalFee).Sum() ?? 0;
            result.ContractCodes = result.WithdrawalDetails?.Select(w => w.Order.ContractCode).ToList();
            return result;
        }

        public GarnerWithdrawal FindById(long withdrawalId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(FindById)}: withdrawalId = {withdrawalId}, tradingProviderId = {tradingProviderId}");
            var result = _dbSet.FirstOrDefault(w => w.Id == withdrawalId && (tradingProviderId == null || w.TradingProviderId == tradingProviderId) && w.Deleted == YesNo.NO);
            return result;
        }

        public PagingResult<GarnerWithdrawal> FindAll(FilterGarnerWithdrawalDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(GetOrderWithdrawalById)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<GarnerWithdrawal> result = new();

            var garnerWithdrawalQuery = _dbSet.Where(w => w.Deleted == YesNo.NO
                && (tradingProviderId == null || w.TradingProviderId == tradingProviderId) 
                && (input.Status == null || input.Status.Contains(w.Status))
                && (input.WithdrawalDate == null || (w.WithdrawalDate.Date == input.WithdrawalDate.Value.Date))
                && (input.ApproveDate == null || (w.ApproveDate != null && w.ApproveDate.Value.Date == input.ApproveDate.Value.Date)));

            if (input.TradingProviderIds != null)
            {
                garnerWithdrawalQuery = garnerWithdrawalQuery.Where(e => input.TradingProviderIds.Contains(e.TradingProviderId));
            }
            else if (input.TradingProviderIds == null && tradingProviderId == null)
            {
                garnerWithdrawalQuery = garnerWithdrawalQuery.Where(e => (new List<int>()).Contains(e.TradingProviderId));
            }

            result.TotalItems = garnerWithdrawalQuery.Count();
            garnerWithdrawalQuery = garnerWithdrawalQuery.OrderByDescending(p => p.Id);
            result.Items = garnerWithdrawalQuery.OrderDynamic(input.Sort);
            return result;
        } 

        /// <summary>
        /// Lấy dòng tiền rút của hợp đồng
        /// </summary>
        public IQueryable<GarnerOrderCashFlowActualDto> WithdrawalByOrder(long orderId)
        {
            _logger.LogInformation($"{nameof(GarnerWithdrawalEFRepository)} -> {nameof(WithdrawalByOrder)}: orderId = {orderId}");

            var query = from orderWithdrawal in _epicSchemaDbContext.GarnerWithdrawalDetails
                        join withdrawal in _epicSchemaDbContext.GarnerWithdrawals on orderWithdrawal.WithdrawalId equals withdrawal.Id
                        join order in _epicSchemaDbContext.GarnerOrders on orderWithdrawal.OrderId equals order.Id
                        where orderWithdrawal.OrderId == orderId && order.Deleted == YesNo.NO && withdrawal.Deleted == YesNo.NO
                        select new GarnerOrderCashFlowActualDto
                        {
                            Id = orderWithdrawal.Id,
                            TotalValue = order.TotalValue,
                            InitTotalValue = order.InitTotalValue,
                            NumberOfDays = (withdrawal.WithdrawalDate.Date - order.InvestDate.Value.Date).Days,
                            Profit = orderWithdrawal.Profit,
                            AmountMoney = orderWithdrawal.AmountMoney,
                            Tax = orderWithdrawal.Tax,
                            AmountReceived = orderWithdrawal.AmountReceived,
                            DeductibleProfit = orderWithdrawal.DeductibleProfit,
                            PayDate = withdrawal.WithdrawalDate,
                            Status = withdrawal.Status
                        };
            return query.OrderBy(w => w.PayDate);
        }

        /// <summary>
        /// Tính tổng số tiền rút (DASHBOARD)
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public decimal CaculateCashOut(List<int> tradingProviderIds, DateTime? now = null)
        {
            _logger.LogInformation($"{nameof(CaculateCashOut)}: tradingProviderIds={JsonSerializer.Serialize(tradingProviderIds)}; now={now}");

            var result = _epicSchemaDbContext.GarnerWithdrawals.AsNoTracking()
                        .Where(x => (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId))
                                    && x.Deleted == YesNo.NO
                                    && new int[] { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.DUYET_KHONG_DI_TIEN }.Contains(x.Status)
                                    && (now == null || (x.ApproveDate != null && x.ApproveDate.Value.Date == now.Value.Date))
                        ).Sum(x => x.AmountMoney);

            return result;
        }

        /// <summary>
        /// Tính lợi tức rút (DASHBOARD)
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public decimal CaculateProfitOut(List<int> tradingProviderIds, DateTime? now = null)
        {
            _logger.LogInformation($"{nameof(CaculateProfitOut)}: tradingProviderIds={JsonSerializer.Serialize(tradingProviderIds)}; now={now}");

            var result = (from detail in _epicSchemaDbContext.GarnerWithdrawalDetails.AsNoTracking()
                          from withdrawal in _epicSchemaDbContext.GarnerWithdrawals.AsNoTracking().Where(
                              x => x.Id == detail.WithdrawalId && x.Deleted == YesNo.NO
                                     && (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId))
                                     && x.Deleted == YesNo.NO
                                     && new int[] { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.DUYET_KHONG_DI_TIEN }.Contains(x.Status)
                                     && (now == null || (x.ApproveDate != null && x.ApproveDate.Value.Date == now.Value.Date)))
                            select detail.ActuallyProfit
                          ).Sum();

            return result;
        }

        /// <summary>
        /// Tính tiền ra theo đại lý (Hiển thị từng phòng ban trên dashboard)
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<GarnerDashboardCashInByTrading> CalculateCashOutByTrading(int tradingProviderId, DateTime startDate, DateTime endDate, int? productId)
        {
            var query = (
                        from departmentSale in (from ds in _epicSchemaDbContext.DepartmentSales.AsNoTracking().Where(x => x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO) select new DepartmentSale { DepartmentId = ds.DepartmentId }).Distinct()
                        from department in _epicSchemaDbContext.Departments.AsNoTracking().Where(x => x.DepartmentId == departmentSale.DepartmentId && x.Deleted == YesNo.NO)
                        from order in _epicSchemaDbContext.GarnerOrders.AsNoTracking().Where(x => x.DepartmentId == department.DepartmentId && startDate.Date <= x.InvestDate.Value.Date && x.InvestDate.Value.Date <= endDate.Date && (productId == null || x.ProductId == productId) && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        from withdrawalDetail in _epicSchemaDbContext.GarnerWithdrawalDetails.AsNoTracking().Where(x => x.OrderId == order.Id).DefaultIfEmpty()
                        from withdrawal in _epicSchemaDbContext.GarnerWithdrawals.AsNoTracking().Where(x => x.Id == withdrawalDetail.WithdrawalId && x.ApproveDate != null && startDate.Date <= x.ApproveDate.Value.Date 
                            && x.ApproveDate.Value.Date <= endDate.Date && new int[] { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.DUYET_KHONG_DI_TIEN }.Contains(x.Status) && x.Deleted == YesNo.NO).DefaultIfEmpty()
                           
                        select new
                        {
                            depId = department.DepartmentId,
                            depName = department.DepartmentName,
                            amount = withdrawalDetail.AmountMoney
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
                        .OrderByDescending(x => x.Amout)
                        ;

            return query.ToList();
        }

        /// <summary>
        /// Tính tiền ra theo partner (Hiển thị từng đại lý trên dashboard)
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<GarnerDashboardCashInByPartner> CalculateCashOutByPartner(List<int> tradingProviderIds, DateTime startDate, DateTime endDate, int? productId)
        {
            var query = (
                       from trading in _epicSchemaDbContext.TradingProviders.AsNoTracking().Where(x => (tradingProviderIds.Count > 0 || tradingProviderIds.Contains(x.TradingProviderId)) && x.Deleted == YesNo.NO)
                       from bus in _epicSchemaDbContext.BusinessCustomers.AsNoTracking().Where(x => trading.BusinessCustomerId == x.BusinessCustomerId && x.Deleted == YesNo.NO)
                       from withdrawal in _epicSchemaDbContext.GarnerWithdrawals.AsNoTracking().Where(x => x.TradingProviderId == trading.TradingProviderId && x.ApproveDate != null && startDate.Date <= x.ApproveDate.Value.Date && x.ApproveDate.Value.Date <= endDate.Date && new int[] { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.DUYET_KHONG_DI_TIEN }.Contains(x.Status) && x.Deleted == YesNo.NO).DefaultIfEmpty()
                       from distribution in _epicSchemaDbContext.GarnerDistributions.AsNoTracking().Where(x => x.Id == withdrawal.DistributionId && (productId == null || x.ProductId == productId) && x.Deleted == YesNo.NO).DefaultIfEmpty()
                       select new
                       {
                           tradingId = trading.TradingProviderId,
                           tradingName = bus.Name,
                           amount = withdrawal.AmountMoney
                       }
                        ).GroupBy(x => new
                        {
                            x.tradingId,
                            x.tradingName
                        })
                        .Select(x => new GarnerDashboardCashInByPartner
                        {
                            TradingProviderId = x.Key.tradingId,
                            TradingProviderName = x.Key.tradingName,
                            Amout = x.Sum(y => y.amount)
                        })
                        .OrderByDescending(x => x.Amout)
                        ;

            return query.ToList();
        }

        /// <summary>
        /// Lấy danh sách hành động rút gần đây của người dùng | Dashboard
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <returns></returns>
        public List<GarnderDashboardActionsDto> DashboardGetNewAction(List<int> tradingProviderIds)
        {
            var query = (from withdrawal in _epicSchemaDbContext.GarnerWithdrawals.AsNoTracking().Where(x => tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId) && x.Deleted == YesNo.NO)
                         from withdrawlDetail in _epicSchemaDbContext.GarnerWithdrawalDetails.AsNoTracking().Where(x => x.WithdrawalId == withdrawal.Id)
                         from order in _epicSchemaDbContext.GarnerOrders.AsNoTracking().Where(x => x.Id == withdrawlDetail.OrderId && x.Deleted == YesNo.NO)
                         from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.Id == order.InvestorIdenId && x.Deleted == YesNo.NO && x.IsDefault == YesNo.YES).DefaultIfEmpty()
                         from investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == iden.InvestorId && x.Deleted == YesNo.NO)
                         where withdrawal.Status == WithdrawalStatus.YEU_CAU
                         orderby order.CreatedDate descending
                         select new GarnderDashboardActionsDto
                         {
                             Action = GarnerDashboardAction.TAO_YEU_CAU_RUT_TIEN,
                             Avatar = investor.AvatarImageUrl,
                             Fullname = iden.Fullname,
                             CreatedDate = order.CreatedDate,
                             WithdrawalId = withdrawal.Id,
                         }).Skip(0).Take(10);

            return query.ToList();

        }
    }
}

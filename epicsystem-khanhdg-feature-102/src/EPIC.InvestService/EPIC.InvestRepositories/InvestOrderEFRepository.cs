using EPIC.CoreEntities.Dto.ExportData;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.CoreRepositoryExtensions;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.SaleAppStatistical;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Dashboard;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.InvestRepositories
{
    public class InvestOrderEFRepository : BaseEFRepository<InvOrder>
    {
        public InvestOrderEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvOrder.SEQ}")
        {
        }

        public InvOrder FindById(long orderId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(InvestOrderEFRepository)}->{nameof(FindById)}: orderId = {orderId}");
            return _dbSet.FirstOrDefault(o => o.Id == orderId && (tradingProviderId == null || o.TradingProviderId == tradingProviderId) && o.Deleted == YesNo.NO);
        }

        public InvOrder OrderApprove(long orderId, int tradingProviderId, string modifiedBy)
        {
            _logger.LogInformation($"{nameof(InvestOrderEFRepository)}->{nameof(OrderApprove)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, modifiedBy = {modifiedBy}");

            //check order
            var orderFind = _dbSet.FirstOrDefault(o => o.TradingProviderId == tradingProviderId && o.Id == orderId && o.Deleted == YesNo.NO);
            if (orderFind == null)
            {
                ThrowException(ErrorCode.InvestOrderNotFound, orderId);
            }
            modifiedBy ??= _epicSchemaDbContext.GetUserByCifCode(orderFind.CifCode);

            //kiểm tra trạng thái và nguồn đặt online hoặc sale đặt
            if (!(orderFind.Status == OrderStatus.CHO_DUYET_HOP_DONG
                || ((orderFind.Source == SourceOrder.ONLINE || (orderFind.Source == SourceOrder.OFFLINE && orderFind.SaleOrderId != null))
                    && (orderFind.Status == OrderStatus.CHO_THANH_TOAN || orderFind.Status == OrderStatus.CHO_KY_HOP_DONG))
            ))
            {
                ThrowException(ErrorCode.InvestOrderCannotApprove, orderFind.Status);
            }
            // check nếu hợp đồng là offline và trạng thái giao nhận hợp đồng là null thì khi duyệt sẽ chuyển trạng thái giao nhận hợp đồng
            if (orderFind.DeliveryStatus == null || (orderFind.DeliveryStatus == DeliveryStatus.WAITING && orderFind.PendingDate == null))
            {
                orderFind.DeliveryStatus = DeliveryStatus.WAITING;
                orderFind.PendingDate = DateTime.Now;
                orderFind.PendingDateModifiedBy = modifiedBy;
            }
            //Tính tổng thanh toán được duyệt
            var listOrderPayment = _epicSchemaDbContext.InvestOrderPayments.Where(o => o.OrderId == orderId && o.Status == OrderPaymentStatus.DA_THANH_TOAN && o.TranType == TranTypes.THU && o.Deleted == YesNo.NO).ToList();
            var totalPaymentValue = listOrderPayment.Sum(p => p.PaymentAmnount);
            if (totalPaymentValue != orderFind.TotalValue) //check số tiền đã thanh toán có bằng total value
            {
                ThrowException(ErrorCode.InvestOrderApproveCheckTotalValue);
            }
            DateTime? maxTransDate = listOrderPayment.Max(p => p.TranDate);
            orderFind.PaymentFullDate = maxTransDate;
            orderFind.InvestDate = maxTransDate;
            orderFind.Status = OrderStatus.DANG_DAU_TU;
            orderFind.ApproveBy = modifiedBy;
            orderFind.ApproveDate = DateTime.Now;
            orderFind.ModifiedBy = modifiedBy;
            orderFind.ModifiedDate = DateTime.Now;
            orderFind.ActiveDate = DateTime.Now;
            return orderFind;
        }

        public PagingResult<InvestOrderInvestmentHistoryDto> FindAllInvestHistory(FilterInvestOrderDto input, int[] status, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(InvestOrderEFRepository)}->{nameof(FindAllInvestHistory)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            PagingResult<InvestOrderInvestmentHistoryDto> result = new();
            var query = _dbSet.Include(o => o.Project)
                                    .Include(o => o.CifCodes)
                                        .ThenInclude(c => c.Investor)
                                    .Include(o => o.CifCodes)
                                        .ThenInclude(c => c.BusinessCustomer)
                                    .Include(o => o.Policy)
                                    .Include(o => o.PolicyDetail)
                                    .Include(o => o.InvestorIdentification)
                                    .Where(o => o.Deleted == YesNo.NO
                                    && o.Status == InvestOrderStatus.TAT_TOAN
                                    && (input.BuyDate == null || o.BuyDate == input.BuyDate)
                                    && (input.DeliveryStatus == null || o.DeliveryStatus == input.DeliveryStatus)
                                    && (input.ReceivedDate == null || o.ReceivedDate.Value.Date == input.ReceivedDate)
                                    && (input.FinishedDate == null || o.FinishedDate.Value.Date == input.FinishedDate)
                                    && (input.CifCode == null || o.CifCode.Contains(input.CifCode))
                                    && (input.Status == null || o.Status == input.Status)
                                    && (input.Source == null || o.Source == input.Source)
                                    && (input.DistributionId == null || o.DistributionId == input.DistributionId)
                                    && (input.PolicyId == null || o.PolicyId == input.PolicyId)
                                    && (input.PolicyDetailId == null || o.PolicyDetailId == input.PolicyDetailId)
                                    && (input.SettlementDate == null || (o.SettlementDate != null && o.SettlementDate.Value.Date == input.SettlementDate.Value.Date))
                                    && (input.PendingDate == null || o.PendingDate.Value.Date == input.PendingDate.Value.Date)
                                    && (input.DeliveryDate == null || o.DeliveryDate.Value.Date == input.DeliveryDate.Value.Date)
                                    && ((input.ContractCode == null || o.ContractCode.Contains(input.ContractCode) || (_epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == o.Id && e.ContractCodeGen.Contains(input.ContractCode)).Any())))
                                    && ((tradingProviderId != null && tradingProviderId == o.TradingProviderId)
                                        || (input.TradingProviderIds != null && input.TradingProviderIds.Contains(o.TradingProviderId)))
                                    && (input.Keyword == null || o.Project.InvCode.Contains(input.Keyword) || o.Project.InvName.Contains(input.Keyword) || o.ContractCode.Contains(input.ContractCode) || o.Id.ToString() == input.Keyword)
                                    && (input.Phone == null || o.CifCodes.BusinessCustomer.Phone == input.Phone || o.CifCodes.Investor.Phone == input.Phone)
                                    && (input.IdNo == null || o.InvestorIdentification.IdNo.Contains(input.IdNo))
                                    && (input.Orderer == null
                                        || (input.Orderer == Orderer.QUAN_TRI_VIEN && o.Source == SourceOrder.OFFLINE && o.SaleOrderId == null)
                                        || (input.Orderer == Orderer.KHACH_HANG && o.Source == SourceOrder.ONLINE)
                                        || (input.Orderer == Orderer.TU_VAN_VIEN && o.Source == SourceOrder.OFFLINE && o.SaleOrderId != null))
                                    && (input.InvestHistoryStatus == null || (input.InvestHistoryStatus == InvestHistoryStatus.TAI_TUC_GOC && o.SettlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC)
                                        || (input.InvestHistoryStatus == InvestHistoryStatus.TAI_TUC_GOC_VA_LOI_TUC && o.SettlementMethod == SettlementMethod.TAI_TUC_GOC_VA_LOI_NHUAN)
                                        || (o.DueDate != null && o.SettlementDate != null && (o.SettlementMethod == null || o.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC)
                                            && ((input.InvestHistoryStatus == InvestHistoryStatus.TAT_TOAN_TRUOC_HAN && o.DueDate > o.SettlementDate)
                                            || (input.InvestHistoryStatus == InvestHistoryStatus.TAT_TOAN_DUNG_HAN && o.DueDate <= o.SettlementDate)))))
                                    .OrderByDescending(o => o.Id)
                                    .Select(order => new InvestOrderInvestmentHistoryDto
                                    {
                                        Id = order.Id,
                                        TradingProviderId = order.TradingProviderId,
                                        CifCode = order.CifCode,
                                        DepartmentId = order.DepartmentId,
                                        ProjectId = order.ProjectId,
                                        DistributionId = order.DistributionId,
                                        PolicyId = order.PolicyId,
                                        PolicyDetailId = order.PolicyDetailId,
                                        TotalValue = order.TotalValue,
                                        InitTotalValue = order.InitTotalValue,
                                        BuyDate = order.BuyDate,
                                        Source = order.Source,
                                        PaymentFullDate = order.PaymentFullDate,
                                        Status = order.Status,
                                        SettlementDate = order.SettlementDate,
                                        InvestDate = order.InvestDate,
                                        DueDate = order.DueDate,
                                        SettlementMethod = order.SettlementMethod,
                                        MethodInterest = order.MethodInterest,
                                        // Nếu mã hợp đồng trong contractFile trùng nhau thì lấy ra, nếu không thì lấy mã hợp đồng
                                        ContractCode = _epicSchemaDbContext.InvestOrderContractFile
                                                    .Where(o => o.OrderId == order.Id && o.Deleted == YesNo.NO)
                                                    .Select(o => o.ContractCodeGen)
                                                    .Distinct().Count() == 1 ? _epicSchemaDbContext.InvestOrderContractFile
                                                                               .Where(o => o.OrderId == order.Id && o.Deleted == YesNo.NO).FirstOrDefault().ContractCodeGen
                                                                             : order.ContractCode,
                                        OrderSource = order.Source == SourceOrder.ONLINE && order.SaleOrderId == null ? Orderer.KHACH_HANG
                                                                        : order.Source == SourceOrder.OFFLINE && order.SaleOrderId == null ? Orderer.QUAN_TRI_VIEN
                                                                            : order.Source == SourceOrder.OFFLINE && order.SaleOrderId != null ? Orderer.TU_VAN_VIEN
                                                                                : null,
                                        //Lấy ra để Sort theo tên khách hàng
                                        CustomerName = order.CifCodes.BusinessCustomer == null ? (order.InvestorIdentification.Fullname ?? order.CifCodes.Investor.Name) : order.CifCodes.BusinessCustomer.Name,
                                        CustomerType = order.CifCodes.BusinessCustomerId == null ? CustomerType.IS_INVETOR : CustomerType.IS_BUSINESSCUSTOMER,
                                        InvestHistoryStatus = (order.SettlementMethod == SettlementMethod.NHAN_LOI_NHUAN_VA_TAI_TUC_GOC) ? InvestHistoryStatus.TAI_TUC_GOC
                                                                : (order.SettlementMethod == SettlementMethod.TAI_TUC_GOC_VA_LOI_NHUAN) ? InvestHistoryStatus.TAI_TUC_GOC_VA_LOI_TUC
                                                                    : (order.DueDate != null && order.SettlementDate != null) ? (order.DueDate > order.SettlementDate) ? InvestHistoryStatus.TAT_TOAN_TRUOC_HAN : InvestHistoryStatus.TAT_TOAN_DUNG_HAN
                                                                        : null,
                                        InvCode = order.Project.InvCode,
                                        PolicyName = order.Policy.Name,
                                        Profit = order.PolicyDetail.Profit,
                                        PolicyDetail = new ViewPolicyDetailDto
                                        {
                                            Name = order.PolicyDetail.Name,
                                            Profit = order.PolicyDetail.Profit,
                                            PeriodQuantity = order.PolicyDetail.PeriodQuantity,
                                            PeriodType = order.PolicyDetail.PeriodType,
                                        }
                                    });
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        public InvOrder AddRenewalOrder(InvOrder entity)
        {
            entity.DeliveryCode = FuncVerifyCodeGenerate();
            entity.CreatedDate = DateTime.Now;
            entity.Status = OrderStatus.DANG_DAU_TU;
            entity.Deleted = YesNo.NO;
            return _dbSet.Add(entity).Entity;
        }

        public List<InvestDashboardActionsDto> DashboardGetNewAction(List<int> tradingProviderIds)
        {
            var actionAddOrder = (from order in _epicSchemaDbContext.InvOrders.AsNoTracking().Where(x => tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId) && x.Deleted == YesNo.NO)
                                  from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.Id == order.InvestorIdenId && x.Deleted == YesNo.NO && x.IsDefault == YesNo.YES).DefaultIfEmpty()
                                  from investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == iden.InvestorId && x.Deleted == YesNo.NO)
                                  where new int[] { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG }.Contains(order.Status)
                                  orderby order.CreatedDate descending
                                  select new InvestDashboardActionsDto
                                  {
                                      Action = InvestDashboardActions.DAT_LENH_DAU_TU_MOI,
                                      Avatar = investor.AvatarImageUrl,
                                      Fullname = iden.Fullname,
                                      CreatedDate = order.CreatedDate,
                                      OrderId = order.Id,
                                  });
            var actionRequestWithdrawal = (from order in _epicSchemaDbContext.InvOrders.AsNoTracking()
                                           join withdrawal in _epicSchemaDbContext.InvestWithdrawals.AsNoTracking() on order.Id equals withdrawal.OrderId
                                           from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.Id == order.InvestorIdenId && x.Deleted == YesNo.NO && x.IsDefault == YesNo.YES).DefaultIfEmpty()
                                           from investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == iden.InvestorId && x.Deleted == YesNo.NO)
                                           where withdrawal.Status == WithdrawalStatus.YEU_CAU
                                           && order.Deleted == YesNo.NO && tradingProviderIds.Contains(order.TradingProviderId)
                                           orderby withdrawal.WithdrawalDate descending
                                           select new InvestDashboardActionsDto
                                           {
                                               Action = InvestDashboardActions.TAO_YEU_CAU_RUT_TIEN,
                                               Avatar = investor.AvatarImageUrl,
                                               Fullname = iden.Fullname,
                                               CreatedDate = withdrawal.WithdrawalDate,
                                               OrderId = order.Id,
                                           });

            var actionRenewalRequest = (from order in _epicSchemaDbContext.InvOrders.AsNoTracking()
                                        join renewalRequest in _epicSchemaDbContext.InvestRenewalsRequests.AsNoTracking() on order.Id equals renewalRequest.OrderId
                                        join investApprove in _epicSchemaDbContext.InvestApproves.AsNoTracking() on renewalRequest.Id equals investApprove.ReferId
                                        from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.Id == order.InvestorIdenId && x.Deleted == YesNo.NO && x.IsDefault == YesNo.YES).DefaultIfEmpty()
                                        from investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == iden.InvestorId && x.Deleted == YesNo.NO)
                                        where renewalRequest.Status == InvestRenewalsRequestStatus.KHOI_TAO && investApprove.DataType == InvestApproveDataTypes.EP_INV_RENEWALS_REQUEST
                                        && order.Deleted == YesNo.NO && tradingProviderIds.Contains(order.TradingProviderId)
                                        orderby investApprove.RequestDate descending
                                        select new InvestDashboardActionsDto
                                        {
                                            Action = InvestDashboardActions.TAO_YEU_CAU_TAI_TUC,
                                            Avatar = investor.AvatarImageUrl,
                                            Fullname = iden.Fullname,
                                            CreatedDate = investApprove.RequestDate,
                                            OrderId = order.Id,
                                        });

            var actionDeliveryStatus = (from order in _epicSchemaDbContext.InvOrders.AsNoTracking()
                                        from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.Id == order.InvestorIdenId && x.Deleted == YesNo.NO && x.IsDefault == YesNo.YES).DefaultIfEmpty()
                                        from investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == iden.InvestorId && x.Deleted == YesNo.NO)
                                        where order.PendingDate != null && order.Source == SourceOrder.ONLINE && order.Deleted == YesNo.NO && tradingProviderIds.Contains(order.TradingProviderId)
                                        orderby order.PendingDate descending
                                        select new InvestDashboardActionsDto
                                        {
                                            Action = InvestDashboardActions.TAO_YEU_CAU_NHAN_HOP_DONG,
                                            Avatar = investor.AvatarImageUrl,
                                            Fullname = iden.Fullname,
                                            CreatedDate = order.PendingDate,
                                            OrderId = order.Id,
                                        });

            var result = actionAddOrder.AsEnumerable().Union(actionRequestWithdrawal).AsEnumerable().Union(actionRenewalRequest).AsEnumerable()
                            .Union(actionDeliveryStatus).AsEnumerable().OrderByDescending(o => o.CreatedDate).Take(7);
            return result.ToList();
        }

        /// <summary>
        /// Danh sách hợp đồng được gắn mã giới thiệu (ListSaleReferralCode) của Sale
        /// </summary>
        /// <returns></returns>
        public IQueryable<AppStatisticOrderBySale> AppGetAllStatisticOrderBySale(AppContractByListSaleFilterDto input)
        {
            _logger.LogInformation($"{nameof(InvestOrderEFRepository)}->{nameof(AppGetAllStatisticOrderBySale)}:input = {JsonSerializer.Serialize(input)}");
            // Lấy các thông tin hợp đồng của Sale bán được bao gồm cả bán hộ
            var result = from order in _dbSet
                         where order.Deleted == YesNo.NO && (input.Status == null || order.Status == input.Status)
                            && order.SaleReferralCode != null && (input.OrderListStatus == null || input.OrderListStatus.Contains(order.Status))
                            && (order.Status != OrderStatus.TAT_TOAN || (order.Status == OrderStatus.TAT_TOAN && order.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC))
                            && (input.DepartmentId == null || (order.DepartmentId == input.DepartmentId))
                            && (input.StartDate == null || (order.InvestDate != null && order.InvestDate.Value.Date >= input.StartDate.Value.Date))
                            && (input.EndDate == null || (order.InvestDate != null && order.InvestDate.Value.Date <= input.EndDate.Value.Date))
                            && ((order.TradingProviderId == input.TradingProviderId && input.ListSaleReferralCode.Contains(order.SaleReferralCode))
                                || (input.ListTradingIds != null && input.ListTradingIds.Contains(order.TradingProviderId) && input.ListSaleReferralCode.Contains(order.SaleReferralCodeSub)))
                         select new AppStatisticOrderBySale()
                         {
                             OrderId = order.Id,
                             Status = order.Status,
                             CifCode = order.CifCode,
                             InvestDate = order.InvestDate,
                             TotalValue = order.TotalValue,
                             InitTotalValue = order.InitTotalValue,
                             CreatedDate = order.BuyDate,
                             ProjectType = ProjectTypes.INVEST
                         };
            return result;
        }

        /// <summary>
        /// Danh sách hợp đồng được gắn mã Sale (SaleReferralCodeSub)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IQueryable<HopDongSaleAppDto> AppGetAllContractOrderBySale(AppSaleFilterContractDto input)
        {
            _logger.LogInformation($"{nameof(InvestOrderEFRepository)}->{nameof(AppGetAllContractOrderBySale)}:input = {JsonSerializer.Serialize(input)}");
            // Lấy các thông tin hợp đồng của Sale bán được bao gồm cả bán hộ
            var result = from order in _dbSet
                         join cifCode in _epicSchemaDbContext.CifCodes on order.CifCode equals cifCode.CifCode
                         join investor in _epicSchemaDbContext.Investors on cifCode.InvestorId equals investor.InvestorId into investors
                         from investor in investors.DefaultIfEmpty()
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers on cifCode.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         where cifCode.Deleted == YesNo.NO && order.Deleted == YesNo.NO
                            && (input.Status == null || order.Status == input.Status)
                            && (input.OrderListStatus == null || input.OrderListStatus.Contains(order.Status))
                            && (input.DepartmentId == null || (order.DepartmentId == input.DepartmentId))
                            && (input.StartDate == null || (order.PaymentFullDate != null && order.PaymentFullDate.Value.Date >= input.StartDate.Value.Date)
                                || (order.PaymentFullDate == null && order.CreatedDate != null && order.CreatedDate.Value.Date >= input.StartDate.Value.Date))
                            && (input.EndDate == null || (order.PaymentFullDate != null && order.PaymentFullDate.Value.Date <= input.EndDate.Value.Date)
                                || (order.PaymentFullDate == null && order.CreatedDate != null && order.CreatedDate.Value.Date <= input.EndDate.Value.Date))
                            && ((order.TradingProviderId == input.TradingProviderId && input.SaleReferralCode == order.SaleReferralCode)
                                || (input.ListTradingIds != null && input.ListTradingIds.Contains(order.TradingProviderId) && input.SaleReferralCode == order.SaleReferralCodeSub))
                         select new HopDongSaleAppDto()
                         {
                             OrderId = order.Id,
                             ContractCode = order.ContractCode,
                             OrderStatus = order.Status,
                             Status = order.Status,
                             CifCode = order.CifCode,
                             InvestDate = order.InvestDate,
                             TotalValue = order.TotalValue,
                             InitTotalValue = order.InitTotalValue,
                             BuyDate = order.BuyDate,
                             SettlementDate = order.SettlementDate,
                             PaymentFullDate = order.PaymentFullDate,
                             ProjectType = ProjectTypes.INVEST,
                             AvatarImageUrl = (businessCustomer != null) ? businessCustomer.AvatarImageUrl : investor.AvatarImageUrl,
                             ReferralCode = (businessCustomer != null) ? businessCustomer.ReferralCodeSelf : investor.ReferralCodeSelf,
                             CustomerName = (businessCustomer != null) ? businessCustomer.Name :
                                        (_epicSchemaDbContext.InvestorIdentifications
                                        .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                                        .OrderByDescending(ii => ii.IsDefault)
                                        .ThenByDescending(ii => ii.Id)
                                        .Select(ii => ii.Fullname)
                                        .FirstOrDefault())
                         };
            return result.Where(o => (input.Keyword == null || o.CustomerName.ToLower().Contains(input.Keyword.ToLower())
                            || o.ReferralCode.ToLower().Contains(input.Keyword.ToLower())
                            || o.ContractCode.ToLower().Contains(input.Keyword.ToLower())));
        }

        #region Function
        /// <summary>
        /// Random mã giao nhận hợp đồng
        /// </summary>
        /// <returns></returns>
        public string FuncVerifyCodeGenerate()
        {
            while (true)
            {
                var numberRandom = RandomNumberUtils.RandomNumber(10);
                var checkDeliveryCodeOrder = _dbSet.Where(o => o.DeliveryCode == numberRandom);
                if (!checkDeliveryCodeOrder.Any())
                {
                    return numberRandom;
                }
            }
        }

        public string ContractCode(long orderId)
        {
            return ContractCodes.INVEST + orderId.ToString().PadLeft(8, '0');
        }
        #endregion
        /// <summary>
        ///  Tìm thông tin hợp đồng mới sau tái tục
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public InvOrder FindNewOrderWhenRenewal(long orderId)
        {
            return _dbSet.FirstOrDefault(o => o.RenewalsReferId == orderId && o.Deleted == YesNo.NO);
        }

        public List<InvOrder> GetAllActiveOrderByCifCode(string cifcode)
        {
            _logger.LogInformation($"{nameof(GetAllActiveOrderByCifCode)}: cifcode = {cifcode}");

            var result = _dbSet.Where(e => e.Deleted == YesNo.NO && e.CifCode == cifcode && e.Status == OrderStatus.DANG_DAU_TU);
            return result.ToList();
        }

        public decimal SumValue(int distributionId, int? tradingProviderId = null)
        {
            decimal result = (_dbSet.Where(o => o.DistributionId == distributionId && (tradingProviderId == null || o.TradingProviderId == tradingProviderId)
                                    && (o.Status == OrderStatus.DANG_DAU_TU || o.Status == OrderStatus.CHO_DUYET_HOP_DONG) && o.Deleted == YesNo.NO)
                                    .Select(o => o.TotalValue).Sum());
            return result;
        }

        /// <summary>
        /// Lấy mã hợp đồng Order khi tất cả các file của hợp đồng đều chung 1 mã
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string GetContractCodeGen(long orderId)
        {
            var orderContractFile = _epicSchemaDbContext.InvestOrderContractFile.Where(o => o.OrderId == orderId && o.Deleted == YesNo.NO).Select(o => o.ContractCodeGen);
            if (orderContractFile.Any())
            {
                var contractCodeGen = orderContractFile.FirstOrDefault();
                return orderContractFile.All(r => r == contractCodeGen) ? contractCodeGen : null;
            }
            return null;
        }

        public PagingResult<InvOrder> FindAll(InvestOrderFilterDto input)
        {
            int[] policies = null;
            var result = new PagingResult<InvOrder>();

            //Lấy danh sách chính sách
            if (input.Policy != null)
            {
                string[] parts = input.Policy.Split(',');
                policies = parts.Select(int.Parse).ToArray();
            }
            var orderQuery = _epicSchemaDbContext.InvOrders
                .Where(order => (input.GroupStatus == null && InvestGroupStatusOrder.DEFAULT.Contains(order.Status))
                             || (input.GroupStatus == OrderGroupStatus.SO_LENH && InvestGroupStatusOrder.SO_LENH.Contains(order.Status))
                             || (input.GroupStatus == OrderGroupStatus.XU_LY_HOP_DONG && InvestGroupStatusOrder.XU_LY_HOP_DONG.Contains(order.Status))
                             || (input.GroupStatus == OrderGroupStatus.DANG_DAU_TU && InvestGroupStatusOrder.DANG_DAU_TU.Contains(order.Status))
                             || (input.GroupStatus == OrderGroupStatus.PHONG_TOA && InvestGroupStatusOrder.PHONG_TOA.Contains(order.Status)));

            var query = QueryOrder(orderQuery, input, policies);

            result.TotalItems = query.Count();

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }

        public PagingResult<InvOrder> FindAllDeliveryStatus(InvestOrderFilterDto input)
        {
            int[] policies = null;
            var result = new PagingResult<InvOrder>();

            //Lấy danh sách chính sách
            if (input.Policy != null)
            {
                string[] parts = input.Policy.Split(',');
                policies = parts.Select(int.Parse).ToArray();
            }
            var orderQuery = _epicSchemaDbContext.InvOrders
                .Where(order => (((order.Status == OrderStatus.DANG_DAU_TU
                            || order.Status == OrderStatus.PHONG_TOA
                            || order.Status == OrderStatus.GIAI_TOA
                            || order.Status == OrderStatus.TAT_TOAN)
                        || (input.GroupStatus == OrderGroupStatus.DANG_DAU_TU && InvestGroupStatusOrder.DANG_DAU_TU.Contains(order.Status))))
                        && order.DeliveryStatus != null);

            var query = QueryOrder(orderQuery, input, policies, true);

            result.TotalItems = query.Count();
            //Sort
            query = query.OrderDynamic(input.Sort);
            if (input.Sort != null && input.Sort.Contains("pendingDate-desc"))
            {
                query = query.OrderByDescending(o => o.PendingDate.HasValue).ThenByDescending(o => o.PendingDate);
            }
            //Phân trang
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }

        private IQueryable<InvOrder> QueryOrder(IQueryable<InvOrder> orderQuery, InvestOrderFilterDto input, int[] policies = null, bool? isDelivery = false)
        {
            var query = orderQuery.Include(o => o.Project)
                                    .Include(o => o.CifCodes)
                                        .ThenInclude(c => c.Investor)
                                    .Include(o => o.CifCodes)
                                        .ThenInclude(c => c.BusinessCustomer)
                                    .Include(o => o.Distribution)
                                    .Include(o => o.Policy)
                                    .Include(o => o.PolicyDetail)
                                    .Include(o => o.InvestorIdentification)
                                    .Include(o => o.InvestorContactAddress)
                                    .Where(o => o.Deleted == YesNo.NO
                                    && o.CifCodes != null
                                    && ((isDelivery == true && (input.Status == null || input.Status == o.Status)) || (isDelivery == false && ((input.Status == null) || o.Status == input.Status)))
                                    && (input.PendingDate == null || o.PendingDate.Value.Date == input.PendingDate.Value.Date)
                                    && (input.DeliveryDate == null || o.DeliveryDate.Value.Date == input.DeliveryDate.Value.Date)
                                    && (input.ReceivedDate == null || o.PendingDate.Value.Date == input.PendingDate.Value.Date)
                                    && (input.Policy == null || policies.Contains(o.Policy.Id))
                                    && (input.DistributionId == null || o.DistributionId == input.DistributionId)
                                    && ((input.ContractCode == null || o.ContractCode.Contains(input.ContractCode) || (_epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == o.Id && e.ContractCodeGen.Contains(input.ContractCode)).Any())))
                                    && (input.Source == null || o.Source == input.Source)
                                    && ((input.TradingProviderId != null && input.TradingProviderId == o.TradingProviderId)
                                        || (input.TradingProviderIds != null && input.TradingProviderIds.Contains(o.TradingProviderId)))
                                    && (input.DeliveryStatus == null || o.DeliveryStatus == input.DeliveryStatus)
                                    && (input.Keyword == null || o.Project.InvCode.Contains(input.Keyword) || o.Project.InvName.Contains(input.Keyword) || o.ContractCode.Contains(input.ContractCode) || o.Id.ToString() == input.Keyword)
                                    && (input.PolicyDetailId == null || o.PolicyDetailId == input.PolicyDetailId)
                                    && (input.TradingDate == null || input.TradingDate.Value.Date == o.BuyDate.Value.Date)
                                    && (input.InvestDate == null || input.InvestDate.Value.Date == o.InvestDate.Value.Date)
                                    && (input.CustomerName == null || o.InvestorIdentification.Fullname.ToLower().Contains(input.CustomerName.ToLower()) || (o.CifCodes.BusinessCustomer.Name.Contains(input.CustomerName.ToLower())))
                                    && (input.CifCode == null || o.CifCode.ToLower().Contains(input.CifCode.ToLower()))
                                    && (input.Phone == null || o.CifCodes.BusinessCustomer.Phone == input.Phone || o.CifCodes.Investor.Phone == input.Phone)
                                    && (input.ContractCodeGen == null || (_epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == o.Id && e.ContractCodeGen.Contains(input.ContractCodeGen)).Any()))
                                    && (input.Orderer == null
                                        || (input.Orderer == Orderer.QUAN_TRI_VIEN && o.Source == SourceOrder.OFFLINE && o.SaleOrderId == null)
                                        || (input.Orderer == Orderer.KHACH_HANG && o.Source == SourceOrder.ONLINE)
                                        || (input.Orderer == Orderer.TU_VAN_VIEN && o.Source == SourceOrder.OFFLINE && o.SaleOrderId != null))
                                    && o.TradingProvider != null && o.Project != null && o.Distribution != null & o.Policy != null && o.PolicyDetail != null)
                                    .OrderByDescending(o => o.InvestDate)
                                    .ThenByDescending(o => o.BuyDate)
                                    .Select(order => new InvOrder
                                    {
                                        #region Lấy thông tin các trường của order
                                        Id = order.Id,
                                        TradingProviderId = order.TradingProviderId,
                                        TradingProvider = order.TradingProvider,
                                        CifCode = order.CifCode,
                                        CifCodes = order.CifCodes,
                                        DepartmentId = order.DepartmentId,
                                        Department = order.Department,
                                        ProjectId = order.ProjectId,
                                        Project = order.Project,
                                        DistributionId = order.DistributionId,
                                        Distribution = order.Distribution,
                                        PolicyId = order.PolicyId,
                                        Policy = order.Policy,
                                        PolicyDetailId = order.PolicyDetailId,
                                        PolicyDetail = order.PolicyDetail,
                                        TotalValue = order.TotalValue,
                                        InitTotalValue = order.InitTotalValue,
                                        BuyDate = order.BuyDate,
                                        IsInterest = order.IsInterest,
                                        SaleReferralCode = order.SaleReferralCode,
                                        Source = order.Source,
                                        BusinessCustomerBankAccId = order.BusinessCustomerBankAccId,
                                        BusinessCustomerBankAcc = order.BusinessCustomerBankAcc,
                                        InvestorBankAccId = order.InvestorBankAccId,
                                        PaymentFullDate = order.PaymentFullDate,
                                        Status = order.Status,
                                        IpAddressCreated = order.IpAddressCreated,
                                        InvestorIdenId = order.InvestorIdenId,
                                        ContractAddressId = order.ContractAddressId,
                                        DeliveryCode = order.DeliveryCode,
                                        DeliveryStatus = order.DeliveryStatus,
                                        SettlementDate = order.SettlementDate,
                                        InvestDate = order.InvestDate,
                                        DueDate = order.DueDate,
                                        ActiveDate = order.ActiveDate,
                                        ApproveBy = order.ApproveBy,
                                        ApproveDate = order.ApproveDate,
                                        SettlementMethod = order.SettlementMethod,
                                        RenewalsPolicyDetailId = order.RenewalsPolicyDetailId,
                                        SaleOrderId = order.SaleOrderId,
                                        DepartmentIdSub = order.DepartmentIdSub,
                                        SaleReferralCodeSub = order.SaleReferralCodeSub,
                                        PendingDate = order.PendingDate,
                                        DeliveryDate = order.DeliveryDate,
                                        ReceivedDate = order.ReceivedDate,
                                        FinishedDate = order.FinishedDate,
                                        FinishedDateModifiedBy = order.FinishedDateModifiedBy,
                                        PendingDateModifiedBy = order.PendingDateModifiedBy,
                                        DeliveryDateModifiedBy = order.DeliveryDateModifiedBy,
                                        ReceivedDateModifiedBy = order.ReceivedDateModifiedBy,
                                        OrderSource = order.OrderSource,
                                        RenewalsReferId = order.RenewalsReferId,
                                        RenewalsReferIdOriginal = order.RenewalsReferIdOriginal,
                                        RenewalIndex = order.RenewalIndex,
                                        MethodInterest = order.MethodInterest,
                                        CreatedDate = order.CreatedDate,
                                        CreatedBy = order.CreatedBy,
                                        ModifiedDate = order.ModifiedDate,
                                        ModifiedBy = order.ModifiedBy,
                                        Deleted = order.Deleted,
                                        // Nếu mã hợp đồng trong contractFile trùng nhau thì lấy ra, nếu không thì lấy mã hợp đồng
                                        ContractCode = _epicSchemaDbContext.InvestOrderContractFile
                                                    .Where(o => o.OrderId == order.Id && o.Deleted == YesNo.NO)
                                                    .Select(o => o.ContractCodeGen)
                                                    .Distinct().Count() == 1 ? _epicSchemaDbContext.InvestOrderContractFile
                                                                               .Where(o => o.OrderId == order.Id && o.Deleted == YesNo.NO).FirstOrDefault().ContractCodeGen
                                                                             : order.ContractCode,
                                        #endregion
                                        //Lấy ra để Sort theo tên khách hàng
                                        CustomerName = order.CifCodes.BusinessCustomer == null 
                                        ? (order.CifCodes.Investor != null ? (order.InvestorIdentification != null ? order.InvestorIdentification.Fullname : order.CifCodes.Investor.Name) : null)
                                        : order.CifCodes.BusinessCustomer.Name,
                                    });

            query = query.OrderDynamic(input.Sort);
            return query;
        }

        /// <summary>
        /// Lấy danh sách hợp đồng để tính toán dự chi
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public IQueryable<InvestOrderDataForInvestPaymentDto> GetAllListOrderCalulationInterestPayment(FilterCalulationInterestPayment input, int? tradingProviderId, int? partnerId)
        {
            var query = (from order in _epicSchemaDbContext.InvOrders
                         join cifCode in _epicSchemaDbContext.CifCodes on order.CifCode equals cifCode.CifCode
                         let settlementMethod = _epicSchemaDbContext.InvestRenewalsRequests.Where(r => r.OrderId == order.Id && r.SettlementMethod != SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && r.Deleted == YesNo.NO
                                                    && (r.Status == InvestRenewalsRequestStatus.KHOI_TAO || r.Status == InvestRenewalsRequestStatus.DA_DUYET)).OrderByDescending(r => r.Id).FirstOrDefault().SettlementMethod

                         from investor in _epicSchemaDbContext.Investors.Where(i => i.InvestorId == cifCode.InvestorId && i.Deleted == YesNo.NO).DefaultIfEmpty()
                         from identification in _epicSchemaDbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                                     .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers on cifCode.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         where order.Status == OrderStatus.DANG_DAU_TU && order.InvestDate != null && order.Deleted == YesNo.NO && cifCode.Deleted == YesNo.NO
                         && (input.Phone == null || input.Phone.Contains(businessCustomer.Phone) || input.Phone.Contains(investor.Phone))
                         && ((input.ContractCode == null || input.ContractCode.ToLower().Contains(order.ContractCode.ToLower()))
                             || _epicSchemaDbContext.InvestOrderContractFile.Any(o => o.ContractCodeGen == input.ContractCode && o.OrderId == order.Id))
                         && (input.TaxCode == null || input.TaxCode.ToLower().Contains(businessCustomer.TaxCode.ToLower()))
                         && (input.CifCode == null || input.CifCode == order.CifCode)
                         && (input.ProjectId == null || input.ProjectId == order.ProjectId)
                         && (input.MethodInterest == null || input.MethodInterest == order.MethodInterest)
                         && (tradingProviderId == null || tradingProviderId == order.TradingProviderId)
                         && (input.TradingProviderChildIds == null || input.TradingProviderChildIds.Contains(order.TradingProviderId))
                         && (partnerId == null || _epicSchemaDbContext.InvestProjects.Any(project => project.PartnerId == partnerId && project.Deleted == YesNo.NO && project.Id == order.ProjectId))
                         && (input.SettlementMethod == null || (input.SettlementMethod == SettlementMethod.TAT_TOAN_KHONG_TAI_TUC && settlementMethod == null)
                                || input.SettlementMethod == settlementMethod)
                         select new InvestOrderDataForInvestPaymentDto
                         {
                             Id = order.Id,

                             TradingProviderId = order.TradingProviderId,
                             PolicyId = order.PolicyId,
                             PolicyDetailId = order.PolicyDetailId,
                             DistributionId = order.DistributionId,
                             CifCode = order.CifCode,
                             ContractCode = order.ContractCode,
                             InvestorIdenId = order.InvestorIdenId,
                             InvestDate = order.InvestDate,
                             DueDate = order.DueDate,
                             SettlementMethod = settlementMethod,
                             ProjectId = order.ProjectId,
                             InitTotalValue = order.InitTotalValue,
                             TotalValue = order.TotalValue,
                             CustomerName = identification.Fullname ?? businessCustomer.Name,
                             RenewalsPolicyDetailId = order.RenewalsPolicyDetailId,
                             Status = order.Status,

                         }).OrderBy(o => o.Id);
            return query;
        }

        public PagingResult<ExportDataInvestOrderDto> ExportDataInvestOrder(FilterExportDataDto input)
        {
            var result = _epicSchemaDbContext.InvOrders.Include(o => o.Project)
                                    .Include(o => o.CifCodes)
                                        .ThenInclude(c => c.Investor)
                                    .Include(o => o.CifCodes)
                                        .ThenInclude(c => c.BusinessCustomer)
                                    .Include(o => o.Distribution)
                                    .Include(o => o.Policy)
                                    .Include(o => o.PolicyDetail)
                                    .Include(o => o.InvestorIdentification)
                                    .Include(o => o.InvestorContactAddress)
                                    .Where(o => o.Deleted == YesNo.NO
                                    && o.CifCodes != null)
                                    .OrderByDescending(o => o.InvestDate)
                                    .ThenByDescending(o => o.BuyDate)
                                    .Select(order => new ExportDataInvestOrderDto
                                    {
                                        CifCode = order.CifCode,
                                        ContractCode = order.ContractCode,
                                        InvCode = order.Project.InvCode,
                                        InvName = order.Project.InvName,
                                        PolicyName = order.Policy.Name,
                                        PolicyCode = order.Policy.Code,
                                        PolicyDetailName = order.PolicyDetail.Name,
                                        BuyDate = order.BuyDate,
                                        InvestDate = order.InvestDate,
                                        DueDate = order.DueDate,
                                        TotalValue = order.TotalValue,
                                        InitTotalValue = order.InitTotalValue,
                                        SaleReferralCode = order.SaleReferralCodeSub ?? order.SaleReferralCode,
                                        CreatedBy = order.CreatedBy,
                                        ApproveBy = order.ApproveBy,
                                        Status = order.Status,
                                        //Lấy ra để Sort theo tên khách hàng
                                        CustomerName = order.CifCodes.BusinessCustomer == null ? (order.CifCodes.Investor.Name ?? order.InvestorIdentification.Fullname) : order.CifCodes.BusinessCustomer.Name,
                                    });
            return new PagingResult<ExportDataInvestOrderDto>
            {
                TotalItems = result.Count(),
                Items = result.Skip(input.Skip).Take(input.PageSize)
            };
        }
    }
}

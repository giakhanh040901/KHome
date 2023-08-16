using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerExportExcel;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.GarnerRepositories
{
    public class GarnerExportExcelReportRepositories
    {
        private readonly ILogger _logger;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        public GarnerExportExcelReportRepositories(DbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext as EpicSchemaDbContext;
            _logger = logger;
            _garnerOrderEFRepository = new GarnerOrderEFRepository(_dbContext, _logger);
        }

        public decimal SumValueCurrentInvestment(int? orderId, int? tradingProviderId, DateTime? tranDate = null)
        {
            var tongGiaTriDauTu = _dbContext.GarnerOrderPayments.Where(op => op.OrderId == orderId && op.TradingProviderId == tradingProviderId
                                                                && op.TranDate <= (tranDate ?? DateTime.MaxValue)
                                                                && op.TranType == TranTypes.THU
                                                                && op.Status == OrderPaymentStatus.DA_THANH_TOAN && op.Deleted == YesNo.NO)
                                                               .Select(op => op.PaymentAmount).Sum();

            var tongGiaTriRut = _dbContext.GarnerOrderPayments.Where(op => op.OrderId == orderId && op.TradingProviderId == tradingProviderId
                                                                && op.TranDate <= (tranDate ?? DateTime.MaxValue)
                                                                && op.TranType == TranTypes.CHI
                                                                && op.Status == OrderPaymentStatus.DA_THANH_TOAN && op.Deleted == YesNo.NO)
                                                               .Select(op => op.PaymentAmount).Sum();
            var result = tongGiaTriDauTu - tongGiaTriRut;
            return result;
        }

        /// <summary>
        /// Danh sách các khoản thực chi
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<GarnerListActualPayment> ListGarnerActualInvestment(int? tradingProviderId, int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from orders in _dbContext.GarnerOrders
                          join cifcodes in _dbContext.CifCodes on orders.CifCode equals cifcodes.CifCode
                          join policies in _dbContext.GarnerPolicies on orders.PolicyId equals policies.Id
                          join products in _dbContext.GarnerProducts on orders.ProductId equals products.Id
                          join interestPaymentDetails in _dbContext.GarnerInterestPaymentDetails on orders.Id equals interestPaymentDetails.OrderId
                          join interestPayments in _dbContext.GarnerInterestPayments on orders.DistributionId equals interestPayments.DistributionId
                          join withdrawDetails in _dbContext.GarnerWithdrawalDetails on orders.Id equals withdrawDetails.OrderId into orderWithdrawJoin
                          from withdrawDetails in orderWithdrawJoin.DefaultIfEmpty()
                          join withdraws in _dbContext.GarnerWithdrawals on withdrawDetails.WithdrawalId equals withdraws.Id
                          join tradingPartners in _dbContext.TradingProviderPartners on orders.TradingProviderId equals tradingPartners.TradingProviderId into tradingPartnerJoin
                          from tradingPartners in tradingPartnerJoin.DefaultIfEmpty()
                          where orders.Deleted == YesNo.NO
                                && cifcodes.Deleted == YesNo.NO
                                && policies.Deleted == YesNo.NO
                                && products.Deleted == YesNo.NO
                                && (startDate == null || (interestPayments.PayDate.Date >= startDate.Value.Date))
                                && (endDate == null || (interestPayments.PayDate.Date <= endDate.Value.Date))
                                && (orders.Status >= OrderStatus.CHO_DUYET_HOP_DONG)
                                && (orders.TradingProviderId == (tradingProviderId ?? interestPayments.TradingProviderId))
                                && (partnerId == null || tradingPartners.PartnerId == partnerId)
                          select new GarnerListActualPayment
                          {
                              PeriodIndex = (int)interestPayments.PeriodIndex,
                              WithdrawalAmount = withdraws.AmountMoney,
                              Paydate = interestPayments.PayDate,
                              TradingProviderId = orders.TradingProviderId,
                              OrderId = orders.Id,
                              CifCode = orders.CifCode,
                              PolicyId = orders.PolicyId,
                              BusinessCustomerId = cifcodes.BusinessCustomerId,
                              InvestorId = cifcodes.InvestorId,
                              ContractCode = orders.ContractCode,
                              InvestorBankAccId = orders.InvestorBankAccId,
                              InterestPaymentType = interestPayments.Status,
                              BusinessCustomerBankAccId = orders.BusinessCustomerBankAccId,
                              PolicyCode = policies.Code,
                              TotalValue = orders.TotalValue,
                          }).Distinct();
            return result.ToList();
        }

        /// <summary>
        /// chi rut von
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<GarnerListInvestment> ListPayWithdrawal(int? tradingProviderId, int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from orders in _dbContext.GarnerOrders
                          join cifcodes in _dbContext.CifCodes on orders.CifCode equals cifcodes.CifCode
                          join withdrawalDetail in _dbContext.GarnerWithdrawalDetails on orders.Id equals withdrawalDetail.OrderId
                          join withdrawal in _dbContext.GarnerWithdrawals on withdrawalDetail.WithdrawalId equals withdrawal.Id
                          join products in _dbContext.GarnerProducts on orders.ProductId equals products.Id
                          join tradingPartners in _dbContext.TradingProviderPartners on orders.TradingProviderId equals tradingPartners.TradingProviderId into tradingPartnerJoin
                          from tradingPartners in tradingPartnerJoin.DefaultIfEmpty()
                          where orders.Deleted == YesNo.NO && cifcodes.Deleted == YesNo.NO
                                && products.Deleted == YesNo.NO && orders.Status > OrderStatus.KHOI_TAO && withdrawal.Deleted == YesNo.NO
                                && (withdrawal.Status == WithdrawalStatus.DUYET_DI_TIEN || withdrawal.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                                && (orders.TradingProviderId == (tradingProviderId ?? orders.TradingProviderId))
                                && (partnerId == null || tradingPartners.PartnerId == partnerId)
                                && (startDate == null || withdrawal.ApproveDate.Value.Date >= startDate.Value.Date)
                                && (endDate == null || withdrawal.ApproveDate.Value.Date <= endDate.Value.Date)
                          select new GarnerListInvestment
                          {
                              TradingProviderId = orders.TradingProviderId,
                              OrderId = orders.Id,
                              CifCode = orders.CifCode,
                              BusinessCustomerId = cifcodes.BusinessCustomerId,
                              InvestorId = cifcodes.InvestorId,
                              ContractCode = orders.ContractCode,
                              TranDate = withdrawal.ApproveDate,
                              InvestorBankAccId = orders.InvestorBankAccId,
                              BusinessCustomerBankAccId = orders.BusinessCustomerBankAccId,
                              SaleReferralCode = orders.SaleReferralCode,
                              TranType = TranTypes.CHI,
                              ProductCode = products.Code,
                              PolicyId = orders.PolicyId,
                              Status = orders.Status,
                              InvestDate = orders.InvestDate,
                              TotalValue = orders.TotalValue,
                              SettlementDate = orders.SettlementDate,
                              ProfitRate = withdrawalDetail.ProfitRate,
                              Profit = withdrawalDetail.Profit,
                              PaymentAmount = withdrawalDetail.AmountMoney,
                              TranClassify = TranClassifies.RUT_VON,
                              Source = orders.Source,
                              SaleOrderId = orders.SaleOrderId,
                              InitTotalValue = orders.InitTotalValue,
                          }).Distinct().ToList();

            return result;
        }

        /// <summary>
        /// chi tra loi tuc
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<GarnerListInvestment> ListGarnerInterstPayment(int? tradingProviderId, int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from orders in _dbContext.GarnerOrders
                          join cifcodes in _dbContext.CifCodes on orders.CifCode equals cifcodes.CifCode
                          join interestPaymentDetail in _dbContext.GarnerInterestPaymentDetails on orders.Id equals interestPaymentDetail.OrderId
                          join interestPayment in _dbContext.GarnerInterestPayments on interestPaymentDetail.InterestPaymentId equals interestPayment.Id
                          join products in _dbContext.GarnerProducts on orders.ProductId equals products.Id
                          join tradingPartners in _dbContext.TradingProviderPartners.DefaultIfEmpty() on orders.TradingProviderId equals tradingPartners.TradingProviderId
                          where orders.Deleted == YesNo.NO && cifcodes.Deleted == YesNo.NO
                                && products.Deleted == YesNo.NO
                                && interestPayment.Deleted == YesNo.NO
                                && orders.Status >= OrderStatus.KHOI_TAO
                                && (orders.TradingProviderId == (tradingProviderId ?? orders.TradingProviderId))
                                && (partnerId == null || tradingPartners.PartnerId == partnerId)
                          select new GarnerListInvestment
                          {
                              TradingProviderId = orders.TradingProviderId,
                              OrderId = orders.Id,
                              CifCode = orders.CifCode,
                              BusinessCustomerId = cifcodes.BusinessCustomerId,
                              InvestorId = cifcodes.InvestorId,
                              ContractCode = orders.ContractCode,
                              TranDate = interestPayment.ApproveDate,
                              InvestorBankAccId = orders.InvestorBankAccId,
                              BusinessCustomerBankAccId = orders.BusinessCustomerBankAccId,
                              SaleReferralCode = orders.SaleReferralCode,
                              TranType = TranTypes.CHI,
                              PolicyId = orders.PolicyId,
                              TranClassify = TranClassifies.CHI_TRA_LOI_TUC,
                              ProductCode = products.Code,
                              Status = orders.Status,
                              Source = orders.Source,
                              SettlementDate = orders.SettlementDate,
                              InvestDate = orders.InvestDate,
                              TotalValue = orders.TotalValue,
                              ProfitRate = interestPaymentDetail.ProfitRate,
                              PaymentAmount = interestPaymentDetail.AmountReceived,
                              SaleOrderId = orders.SaleOrderId,
                              InitTotalValue = orders.InitTotalValue,
                          }).Distinct().ToList();

            return result.Where(i => (startDate == null || startDate <= i.TranDate?.Date) && (endDate == null || endDate >= i.TranDate?.Date)).ToList();
        }

        /// <summary>
        /// thu
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<GarnerListInvestment> ListInvestments(int? tradingProviderId, int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from orders in _dbContext.GarnerOrders
                          join cifcodes in _dbContext.CifCodes on orders.CifCode equals cifcodes.CifCode
                          join orderPayments in _dbContext.GarnerOrderPayments on orders.Id equals orderPayments.OrderId
                          join products in _dbContext.GarnerProducts on orders.ProductId equals products.Id
                          join tradingPartners in _dbContext.TradingProviderPartners.DefaultIfEmpty() on orders.TradingProviderId equals tradingPartners.TradingProviderId
                          where orders.Deleted == YesNo.NO && cifcodes.Deleted == YesNo.NO && orderPayments.Deleted == YesNo.NO
                                && products.Deleted == YesNo.NO
                                && (startDate == null || (orderPayments.TranDate != null && orderPayments.TranDate.Value.Date >= startDate.Value.Date))
                                && (endDate == null || (orderPayments.TranDate != null && orderPayments.TranDate.Value.Date <= endDate.Value.Date))
                                && orders.Status >= OrderStatus.KHOI_TAO 
                                && orderPayments.Status == OrderPaymentStatus.DA_THANH_TOAN
                                && orderPayments.TranType == TranTypes.THU
                                && (orders.TradingProviderId == (tradingProviderId ?? orderPayments.TradingProviderId))
                                && (partnerId == null || tradingPartners.PartnerId == partnerId)
                          select new GarnerListInvestment
                          {
                              TradingProviderId = orders.TradingProviderId,
                              OrderId = orderPayments.OrderId,
                              CifCode = orders.CifCode,
                              BusinessCustomerId = cifcodes.BusinessCustomerId,
                              InvestorId = cifcodes.InvestorId,
                              ContractCode = orders.ContractCode,
                              TranDate = orderPayments.TranDate,
                              InvestorBankAccId = orders.InvestorBankAccId,
                              BusinessCustomerBankAccId = orders.BusinessCustomerBankAccId,
                              SaleReferralCode = orders.SaleReferralCode,
                              TranType = TranTypes.THU,
                              ProductCode = products.Code,
                              PolicyId = orders.PolicyId,
                              Status = orders.Status,
                              SettlementDate = orders.SettlementDate,
                              InvestDate = orders.InvestDate,
                              TotalValue = orders.TotalValue,
                              InitTotalValue = orders.InitTotalValue,
                              PaymentAmount = orderPayments.PaymentAmount,
                              TranClassify = orderPayments.TranClassify,
                              Source = orders.Source,
                          }).Distinct().ToList();
            return result;
        }

        public List<GarnerListInvestment> ListOrderBySaleSub(int? tradingProviderId, int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from orders in _dbContext.GarnerOrders
                          join cifcodes in _dbContext.CifCodes on orders.CifCode equals cifcodes.CifCode
                          join orderPayments in _dbContext.GarnerOrderPayments on orders.Id equals orderPayments.OrderId
                          join products in _dbContext.GarnerProducts on orders.ProductId equals products.Id
                          join tradingPartners in _dbContext.TradingProviderPartners.DefaultIfEmpty() on orders.TradingProviderId equals tradingPartners.TradingProviderId
                          where orders.Deleted == YesNo.NO && cifcodes.Deleted == YesNo.NO
                                && products.Deleted == YesNo.NO
                                && (startDate == null || (orders.InvestDate != null && orders.InvestDate.Value.Date >= startDate.Value.Date))
                                && (endDate == null || (orders.InvestDate != null && orders.InvestDate.Value.Date <= endDate.Value.Date))
                                && orders.Status >= OrderStatus.CHO_DUYET_HOP_DONG
                                && orderPayments.TranType == TranTypes.THU
                                && (orders.TradingProviderId == (tradingProviderId ?? orders.TradingProviderId))
                                && (partnerId == null || tradingPartners.PartnerId == partnerId)
                          select new GarnerListInvestment
                          {
                              TradingProviderId = orders.TradingProviderId,
                              OrderId = orders.Id,
                              CifCode = orders.CifCode,
                              TranDate = orderPayments.TranDate,
                              TranType = orderPayments.TranType,
                              BusinessCustomerId = cifcodes.BusinessCustomerId,
                              InvestorId = cifcodes.InvestorId,
                              ContractCode = orders.ContractCode,
                              InvestorBankAccId = orders.InvestorBankAccId,
                              BusinessCustomerBankAccId = orders.BusinessCustomerBankAccId,
                              SaleReferralCode = orders.SaleReferralCode,
                              SaleReferralCodeSub = orders.SaleReferralCodeSub,
                              ProductCode = products.Code,
                              PolicyId = orders.PolicyId,
                              Status = orders.Status,
                              InvestDate = orders.InvestDate,
                              TotalValue = orders.TotalValue,
                              InitTotalValue = orders.InitTotalValue,
                              PaymentAmount = orderPayments.PaymentAmount,
                              Source = orders.Source,
                          }).Distinct().OrderBy(x => x.TranDate);
            return result.Where(i => i.TranDate != null && (startDate == null || startDate <= i.TranDate.Value.Date) && (endDate == null || endDate >= i.TranDate.Value.Date) && i.SaleReferralCodeSub != null).ToList();
        }
        public IEnumerable<GarnerListInvestmentPaymentDto> TotalInterestPayment(int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from orders in _dbContext.GarnerOrders
                          join interestPaymentDetail in _dbContext.GarnerInterestPaymentDetails on orders.Id equals interestPaymentDetail.OrderId
                          join interestPayment in _dbContext.GarnerInterestPayments on interestPaymentDetail.InterestPaymentId equals interestPayment.Id
                          where orders.Deleted == YesNo.NO && interestPayment.Deleted == YesNo.NO && orders.Status >= OrderStatus.KHOI_TAO
                          && (startDate == null || startDate <= interestPayment.PayDate) && (endDate == null || endDate >= interestPayment.PayDate)
                          && (tradingProviderId == null || interestPayment.TradingProviderId == tradingProviderId)
                          && (tradingProviderId == null || orders.TradingProviderId == tradingProviderId)
                          group interestPaymentDetail by new { interestPaymentDetail.AmountReceived, interestPayment.ApproveDate, orders.TradingProviderId } into w
                          select new GarnerListInvestmentPaymentDto
                          {
                              TradingProviderId = w.Key.TradingProviderId,
                              AmountMoney = w.Sum(o => o.AmountReceived),
                              PaymentDate = w.Key.ApproveDate
                          }).Distinct();
            return result;
        }

        public IEnumerable<GarnerListInvestmentPaymentDto> TotalWithdrawalPayment(int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            var result = (from orders in _dbContext.GarnerOrders
                          join withdrawalDetail in _dbContext.GarnerWithdrawalDetails on orders.Id equals withdrawalDetail.OrderId
                          join withdrawal in _dbContext.GarnerWithdrawals on withdrawalDetail.WithdrawalId equals withdrawal.Id
                          where orders.Deleted == YesNo.NO && withdrawal.Deleted == YesNo.NO && orders.Status >= OrderStatus.KHOI_TAO
                          && (startDate == null || (withdrawal.ApproveDate != null && startDate <= withdrawal.ApproveDate.Value.Date))
                          && (endDate == null || withdrawal.ApproveDate != null && endDate >= withdrawal.ApproveDate.Value.Date)
                          && (withdrawal.Status == WithdrawalStatus.DUYET_DI_TIEN || withdrawal.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN)
                          && (tradingProviderId == null || withdrawal.TradingProviderId == tradingProviderId)
                          && (tradingProviderId == null || orders.TradingProviderId == tradingProviderId)
                          group new { withdrawalDetail, withdrawal } by new { withdrawalDetail.AmountReceived, withdrawal.ApproveDate, orders.TradingProviderId } into w
                          select new GarnerListInvestmentPaymentDto
                          {
                              TradingProviderId = w.Key.TradingProviderId,
                              AmountMoney = w.Sum(o => o.withdrawalDetail.AmountReceived),
                              PaymentDate = w.Key.ApproveDate
                          }).Distinct();
            return result;
        }
    }
}

using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Withdrawal;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestWithdrawalEFRepository : BaseEFRepository<Withdrawal>
    {
        public InvestWithdrawalEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{Withdrawal.SEQ}")
        {
        }


        /// <summary>
        /// Duyệt không đi tiền
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="actuallyAmount"></param>
        /// <param name="approveIp"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Withdrawal ApproveWithdrawal(InvestWithdrawalApproveDto input)
        {
            _logger.LogInformation($"{nameof(InvestWithdrawalEFRepository)} => {nameof(ApproveWithdrawal)}: input = {JsonSerializer.Serialize(input)}");
            var withdrawal = _dbSet.FirstOrDefault(w => w.Id == input.Id && w.TradingProviderId == input.TradingProviderId && w.Deleted == YesNo.NO)
                .ThrowIfNull<Withdrawal>(_epicSchemaDbContext, ErrorCode.InvestWithdrawalNotFound);

            var orderFind = _epicSchemaDbContext.InvOrders.FirstOrDefault(o => o.Id == withdrawal.OrderId && o.TradingProviderId == input.TradingProviderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestOrderNotFound);
            if (orderFind.Status != OrderStatus.DANG_DAU_TU)
            {
                ThrowException(ErrorCode.InvestOrderNotInStatusActive);
            }
            if (orderFind.TotalValue - withdrawal.AmountMoney < 0)
            {
                ThrowException(ErrorCode.InvestWithdrawalApproveTooLarge);
            }    
            withdrawal.ApproveIp = input.ApproveIp;
            withdrawal.ActuallyAmount = input.ActuallyAmount;
            withdrawal.Profit = input.Profit;
            withdrawal.DeductibleProfit = input.DeductibleProfit;
            withdrawal.Tax= input.Tax;
            withdrawal.ActuallyProfit = input.ActuallyProfit;
            withdrawal.WithdrawalFee= input.WithdrawalFee;
            withdrawal.ApproveBy = input.Username;
            withdrawal.ApproveNote = input.ApproveNote;
            withdrawal.ApproveDate = DateTime.Now;
            return withdrawal;
        }

        /// <summary>
        /// Lấy tất cả yêu cầu rút vốn đã được duyệt theo id sổ lệnh
        /// </summary>
        /// <returns></returns>
        public List<Withdrawal> FindAll(long orderId)
        {
            return _dbSet.Where(w => w.OrderId == orderId && (w.Status == WithdrawalStatus.DUYET_KHONG_DI_TIEN || w.Status == WithdrawalStatus.DUYET_DI_TIEN)).ToList();
        }

        public Withdrawal FindById(long withdrawalId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(InvestWithdrawalEFRepository)} -> {nameof(FindById)}: withdrawalId = {withdrawalId}, tradingProviderId = {tradingProviderId}");
            var result = _dbSet.FirstOrDefault(w => w.Id == withdrawalId && (tradingProviderId == null || w.TradingProviderId == tradingProviderId) && w.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Dashboard tiền rút
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public decimal DashboardCaculateCashOut(List<int> tradingProviderIds, DateTime? now = null)
        {
            _logger.LogInformation($"{nameof(DashboardCaculateCashOut)}: tradingProviderIds={JsonSerializer.Serialize(tradingProviderIds)}; now={now}");

            var result = _epicSchemaDbContext.InvestWithdrawals.AsNoTracking() .Where(x => (tradingProviderIds.Count == 0 || tradingProviderIds.Contains(x.TradingProviderId ?? 0))
                            && new int[] { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.DUYET_KHONG_DI_TIEN }.Contains(x.Status ?? 0) && x.Deleted == YesNo.NO
                            && (now == null || (x.WithdrawalDate.Date == now.Value.Date))).Sum(x => x.ActuallyAmount ?? 0);
            return result;
        }

        public IQueryable<Withdrawal> FindAllWithdrawal(InvestWithdrawalFilterDto input)
        {
            var result = _dbSet.Include(w => w.Order).ThenInclude(o => o.CifCodes).ThenInclude(c => c.Investor)
                              .Include(w => w.Order).ThenInclude(o => o.CifCodes).ThenInclude(c => c.BusinessCustomer)
                              .Include(w => w.PolicyDetail)
                              .Where(w => w.Deleted == YesNo.NO
                              && w.CifCodes != null
                              && w.CifCodes.Investor != null
                              && (input.Status == null || w.Status == input.Status)
                              && (input.MethodInterest == null || w.Order.MethodInterest == input.MethodInterest)
                              && (input.RequestDate == null || w.CreatedDate.Value.Date == input.RequestDate.Value.Date)
                              && (input.ApproveDate == null || w.ApproveDate.Value.Date == input.ApproveDate.Value.Date)
                              && ((input.ContractCode == null || w.Order.ContractCode.Contains(input.ContractCode))
                                || (_epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == w.Order.Id && e.ContractCodeGen.Contains(input.ContractCode)).Any()))
                              && (input.Phone == null || w.Order.CifCodes.BusinessCustomer.Phone == input.Phone || w.Order.CifCodes.Investor.Phone == input.Phone)
                              && (input.CifCode == null || w.Order.CifCode.ToLower().Contains(input.CifCode.ToLower()))
                              && (input.Source == null || w.Source == input.Source)
                              && ((input.TradingProviderId != null && input.TradingProviderId == w.Order.TradingProviderId)
                                        || (input.TradingProviderIds != null && input.TradingProviderIds.Contains(w.Order.TradingProviderId)))
                              && (input.DistributionId == null || w.Order.DistributionId == input.DistributionId))
                              .OrderByDescending(w => w.Id)
                              .Select(withdrawal =>  withdrawal );
            return result;
        }
    }
}

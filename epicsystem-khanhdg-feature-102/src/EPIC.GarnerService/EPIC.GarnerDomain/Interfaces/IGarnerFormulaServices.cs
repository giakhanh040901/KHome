using EPIC.GarnerDomain.Implements;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.Dto.GarnerShared;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.GarnerEntities.Dto.GarnerOrder;

namespace EPIC.GarnerDomain.Interfaces
{
    /// <summary>
    /// Tính toán garner
    /// </summary>
    public interface IGarnerFormulaServices
    {
        /// <summary>
        /// Tính lợi nhuận của chính sách
        /// </summary>
        List<AppGarnerPolicyDetailDto> GetCashFlow(int policyId, decimal totalValue, DateTime now);
        /// <summary>
        /// Tính rút vốn theo id withdrawalId
        /// </summary>
        CalculateGarnerWithdrawalDto CalculateWithdrawal(long withdrawalId);
        /// <summary>
        /// Tính rút vốn theo danh sách orderWithdrawal
        /// </summary>
        CalculateGarnerWithdrawalDto CalculateWithdrawal(List<GarnerOrderWithdrawalDto> orderWithdrawal, DateTime? now = null);
        /// <summary>
        /// //xử lý tính tiền rút : lặp tính cho từng lệnh
        /// </summary>
        GarnerWithdrawalDetailDto CalculateWithdrawalDetail(GarnerOrderWithdrawalDto garnerOrderWithdrawalDto, DateTime? now = null);
        /// <summary>
        /// //xử lý tính tiền rút : lặp tính cho từng lệnh
        /// </summary>
        GarnerWithdrawalDetailDto CalculateWithdrawalDetail(long orderId, long withdrawalId);

        /// <summary>
        /// Tính lợi nhuận hiện tại của hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        CalculateProfitDto CaculateProfitNow(long orderId);

        /// <summary>
        /// Lấy danh sách lịch sử dòng tiền của Nhà đầu tư
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        List<TradingRecentlyDto> GarnerTradingRecently(int investorId);

        /// <summary>
        /// Lấy danh sách ngân hàng đặt lệnh theo cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        List<BankAccountInfoDto> FindListBankOfCifCode(string cifCode);

        /// <summary>
        /// Xem trước dòng tiền lợi nhuận chi trả
        /// </summary>
        List<GarnerOrderCashFlowExpectedDto> GetCashFlowOrder(long orderId);
    }
}

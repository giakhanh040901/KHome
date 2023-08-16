using DocumentFormat.OpenXml.Bibliography;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestSharedServices
    {
        /// <summary>
        /// Tính toán lợi tức, trái tức, chú ý trường hợp không truyền trading provider id
        /// </summary>
        /// <param name="project"></param>
        /// <param name="policy"></param>
        /// <param name="policyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="soTienDauTu"></param>
        /// <param name="khachCaNhan"></param>
        /// <returns></returns>
        ProfitAndInterestDto CalculateListInterest(Project project, Policy policy, PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan, DateTime? distributionCloseSellDate, int orderId = 0, bool isApp = false);

        /// <summary>
        /// Tính
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="policyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="soTienDauTu"></param>
        /// <param name="khachCaNhan"></param>
        /// <returns></returns>

        CalculateProfitDto CalculateProfit(Policy policy, PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan, DateTime? distributionCloseSellDate);

        /// <summary>
        /// Ngày làm việc tiếp theo
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        DateTime NextWorkDay(DateTime ngayDangXet, int tradingProviderId = 0);

        /// <summary>
        /// Tính ngày làm việc
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="soNgayLamViec"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        DateTime WorkDay(DateTime ngayDangXet, int soNgayLamViec, int tradingProviderId = 0);

        /// <summary>
        /// Tính ngày đáo hạn
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <returns></returns>
        DateTime CalculateDueDate(PolicyDetail policyDetail, DateTime ngayBatDauTinhLai, DateTime? distributionCloseSellDate);

        /// <summary>
        /// Lấy dòng tiền cho hợp đồng
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="paymentFullDate"></param>
        /// <param name="policyDetail"></param>
        /// <param name="policy"></param>
        /// <param name="distribution"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        CashFlowDto GetCashFlowContract(decimal totalValue, DateTime paymentFullDate, PolicyDetail policyDetail, Policy policy, Distribution distribution, Project project, int orderId = 0, bool isApp = false);

        /// <summary>
        /// Lấy dòng tiền
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        CashFlowDto GetCashFlow(decimal totalValue, int policyDetailId);

        /// <summary>
        /// Lợi tức tính đến hiện tại
        /// Nếu ngày hiện tại vượt quá ngày đáo hạn thì gán bằng ngày đáo hạn
        /// </summary>
        /// <param name="ngayDauTu"></param>
        /// <param name="profit"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        decimal ProfitNow(DateTime ngayDauTu, DateTime ngayDaoHan, decimal profit, decimal totalValue);
        RutVonDto RutVonInvest(InvOrder order, Policy policy, PolicyDetail policyDetail, decimal tongTienConDauTu, decimal soTienRut, DateTime ngayRut, bool khachCaNhan, DateTime? distributionCloseSellDate);
    }
}

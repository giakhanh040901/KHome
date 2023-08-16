using EPIC.BondEntities.DataEntities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondSharedService
    {
        /// <summary>
        /// Tính toán lợi tức, trái tức, chú ý trường hợp không truyền trading provider id
        /// </summary>
        /// <param name="bondPolicy"></param>
        /// <param name="bondPolicyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="soTienDauTu"></param>
        /// <param name="khachCaNhan"></param>
        /// <returns></returns>
        ProfitAndInterestDto CalculateListInterest(BondInfo bondInfo, BondPolicy bondPolicy, BondPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan);

        /// <summary>
        /// Tính trái tức lô theo số lượng, số lượng tùy ý có thể dùng để tính lợi tức cho hợp đồng phân phối
        /// </summary>
        /// <param name="bondInfo"></param>
        /// <param name="quantity"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        CouponDto CalculateCouponByQuantity(BondInfo bondInfo, long quantity, int partnerId);

        /// <summary>
        /// Tính lợi tức, chú ý trường hợp không truyền trading provider id
        /// </summary>
        /// <param name="bondPolicy"></param>
        /// <param name="bondPolicyDetail"></param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <param name="soTienDauTu"></param>
        /// <param name="khachCaNhan"></param>
        /// <returns></returns>
        CalculateProfitDto CalculateProfit(BondPolicy bondPolicy, BondPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, decimal soTienDauTu, bool khachCaNhan);

        /// <summary>
        /// Tính trái tức khi nhập số tiền (trường hợp trả định kỳ, cuối kỳ),
        /// ngày bắt đầu và ngày đáo hạn chứa ngày chốt quyền nào thì sẽ nhận được trái tức của ngày đó
        /// </summary>
        /// <param name="bondInfoId"></param>
        /// <param name="bondSecondaryId"></param>
        /// <param name="soTienDauTu"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="incomeTax">thuế, đã chia cho 100</param>
        /// <returns></returns>
        List<CouponInfoDto> GetListCoupon(int bondInfoId, int bondSecondaryId, decimal soTienDauTu, DateTime startDate, DateTime endDate, decimal incomeTax = 0, int tradingProviderId = 0);

        /// <summary>
        /// Tính ra ngày làm việc tiếp theo, chú ý trường hợp không truyền trading provider id
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <returns></returns>
        DateTime NextWorkDay(DateTime ngayDangXet, int tradingProviderId = 0, bool isClose = true);

        /// <summary>
        /// Lùi hoặc tiến lại đủ số ngày làm việc, tính cả ngày đang xét, chú ý trường hợp không truyền trading provider id
        /// </summary>
        /// <param name="ngayDangXet"></param>
        /// <param name="soNgayLamViec">dương thì đếm tiến, âm thì đếm lùi</param>
        /// <returns></returns>
        DateTime WorkDay(DateTime ngayDangXet, int soNgayLamViec, int tradingProviderId = 0);

        /// <summary>
        /// Tính ngày đáo hạn từ ngày bắt đầu tính lãi
        /// </summary>
        /// <param name="bondPolicyDetail">kỳ hạn</param>
        /// <param name="ngayBatDauTinhLai"></param>
        /// <returns></returns>
        DateTime CalculateDueDate(BondPolicyDetail bondPolicyDetail, DateTime ngayBatDauTinhLai, bool isClose = true);
        
        CashFlowDto GetCashFlowContract(decimal totalValue, DateTime paymentFullDate, BondPolicyDetail bondPolicyDetail, BondPolicy bondPolicy, BondSecondary bondSecondary, BondInfo bondInfo);

        /// <summary>
        /// Lấy thông tin dòng tiền
        /// </summary>
        /// <returns></returns>
        CashFlowDto GetCashFlow(decimal totalValue, int policyDetailId);

        /// <summary>
        /// Tính lợi tức đến thời điểm hiện tại
        /// Nếu sau ngày đáo hạn thì trả về 0
        /// </summary>
        /// <param name="ngayDauTu"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        decimal ProfitNow(DateTime ngayDauTu, DateTime ngayDaoHan, decimal profit, decimal totalValue);

        /// <summary>
        /// Tính số lượng và đơn giá trái phiếu của ngày đang xét
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="bondSecondaryId"></param>
        /// <param name="ngayTinh"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        QuantityAndUnitPriceDto CalculateQuantityAndUnitPrice(decimal totalValue, int bondSecondaryId, DateTime ngayTinh, int tradingProviderId);
    }
}

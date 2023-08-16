using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface IHisAccumulatePointServices
    {
        /// <summary>
        /// Thêm tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        Task Add(AddAccumulatePointDto dto);

        /// <summary>
        /// Tạo điểm từ file excel
        /// </summary>
        /// <param name="dto"></param>
        Task ImportExcel(ImportExceAccumulatePointlDto dto);

        /// <summary>
        /// Cập nhật thông tin tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        void Update(UpdateAccumulatePointDto dto);

        /// <summary>
        /// Cập nhật trạng thái yêu cầu đổi điểm (Exchanged point status)
        /// </summary>
        /// <param name="dto"></param>
        void UpdateExchangedPointStatus(UpdateHisAccumulateStatusDto dto, int exchangedPointStatus);

        /// <summary>
        /// Hủy lệnh tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        void Cancel(UpdateStatusCancelDto dto);

        /// <summary>
        /// Xoá tích điểm/tiêu điểm
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Tìm kiếm phân trang tích điểm/tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PagingResult<ViewFindAllHisAccumulatePointDto> FindAll(FindHisAccumulatePointDto dto);

        /// <summary>
        /// Tìm kiếm yêu cầu đổi điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PagingResult<ViewFindAllHisAccumulatePointDto> FindAll(FindRequestConsumePointDto dto);

        /// <summary>
        /// Tìm kiếm theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        PagingResult<ViewFindAllHisAccumulatePointDto> FindByInvestorId(int investorId, FindAccumulatePointByInvestorId dto);

        /// <summary>
        /// Get list voucher đang kích hoạt và chưa cấp phát cho ai
        /// </summary>
        /// <returns></returns>
        List<ViewListVoucherDto> FindFreeVoucher();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ViewHisAccumulatePointDto FindById(int id);

        /// <summary>
        /// Lấy list lý do tích điểm/tiêu điểm
        /// </summary>
        /// <param name="pointType"></param>
        /// <returns></returns>
        List<ViewListReasonsDto> GetAccumulateReason(int? pointType);

        /// <summary>
        /// App tiêu điểm
        /// </summary>
        /// <param name="dto"></param>
        void AppConsumePoint(AppConsumePointDto dto);

        /// <summary>
        /// App lấy lịch sử điểm thưởng
        /// </summary>
        /// <returns></returns>
        List<AppViewAccumulatePointHistoryDto> AppGetAccumulatePointHistory(int? tradingProviderId);
    }
}

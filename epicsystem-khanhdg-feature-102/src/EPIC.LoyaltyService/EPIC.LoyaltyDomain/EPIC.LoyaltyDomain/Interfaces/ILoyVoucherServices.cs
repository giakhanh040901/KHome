using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ContractData;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface ILoyVoucherServices
    {
        /// <summary>
        /// Thêm mới voucher
        /// </summary>
        /// <param name="dto"></param>
        public Task<ViewVoucherDto> Add(AddVoucherDto dto);

        /// <summary>
        /// Tìm kiếm voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewListVoucherDto> FindAll(FindAllVoucherDto dto);

        /// <summary>
        /// Lịch sử cấp phát
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PagingResult<VoucherConversionHistoryDto> FindAllVoucherConversionHistory(FilterVoucherConversionHistoryDto dto);

        /// <summary>
        /// Lấy danh sách voucher để đổi điểm Tab yêu cầu đổi điểm
        /// </summary>
        /// <returns></returns>
        List<ViewListVoucherDto> GetAllVoucherForConversionPoint(string keyword);

        /// <summary>
        /// Tạo voucher từ list excel
        /// </summary>
        /// <param name="dto"></param>
        public Task ImportExcelVoucher(ImportExcelVoucherDto dto);
        /// <summary>
        /// Update trạng thái voucher
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateStatus(UpdateVoucherStatusDto dto);

        /// <summary>
        /// Áp dụng voucher với khách
        /// </summary>
        /// <param name="dto"></param>
        public Task ApplyVoucherToInvestor(ApplyVoucherToInvestorDto dto);

        /// <summary>
        /// Tìm kiếm danh sách investor kèm thêm thông tin voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewInvestorVoucherDto> FindAllInvestorVoucher(FindAllInvestorForVoucherDto dto);
        /// <summary>
        /// Cập nhật voucher
        /// </summary>
        /// <param name="dto"></param>
        public Task Update(UpdateVoucherDto dto);

        /// <summary>
        /// Lấy voucher theo id
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public ViewVoucherDto FindById(int voucherId);

        /// <summary>
        /// Lấy voucher theo investorid
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewVoucherByInvestorDto> FindByInvestorPaging(FindVoucherByInvestorIdDto dto);

        /// <summary>
        /// Xoá mềm voucher theo voucher id (UPDATE TRẠNG THÁI)
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(int id);

        /// <summary>
        /// Xuất excel theo kq tìm kiếm khcn
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ExportResultDto ExportExcelInvestorVoucher(FindAllInvestorForVoucherDto dto);

        /// <summary>
        /// Update nổi bật
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateIsHot(UpdateIsHotDto dto);

        /// <summary>
        /// Update show app
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateIsShowApp(UpdateShowAppDto dto);

        /// <summary>
        /// Update dùng trong vòng quay may mắn
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateIsUseInLuckyDraw(UpdateIsUseInLuckyDrawDto dto);

        #region App
        /// <summary>
        /// Lấy danh sách 6 voucher nổi bật của đại lý
        /// </summary>
        List<AppViewVoucherByInvestorDto> AppFindVoucherIsHot(int tradingProviderId);

        /// <summary>
        /// App lấy voucher theo investor
        /// </summary>
        /// <returns></returns>
        public List<AppViewVoucherByInvestorDto> AppFindByInvestor(int? tradingProviderId, string useType);

        /// <summary>
        /// App lấy voucher hết hạn theo investor
        /// </summary>
        /// <returns></returns>
        public List<AppViewVoucherExpiredByInvestorDto> AppFindExpiredByInvestor();

        /// <summary>
        /// Các loại hình voucher của đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        List<string> AppGetVoucherUseTypeOfTrading(int tradingProviderId);
        #endregion
    }
}

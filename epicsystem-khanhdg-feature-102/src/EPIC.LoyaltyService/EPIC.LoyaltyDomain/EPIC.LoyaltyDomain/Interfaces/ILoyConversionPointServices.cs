using EPIC.DataAccess.Models;
using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.LoyaltyEntities.Dto.LoyPointInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface ILoyConversionPointServices
    {
        LoyConversionPointDto Add(AddLoyConversionPointDto input);

        /// <summary>
        /// Hủy chuyển đổi điểm
        /// </summary>
        /// <param name="input"></param>
        void ChangeStatusCancel(UpdateLoyConversionPointStatusDto input);

        /// <summary>
        /// Chuyển đổi trạng thái của yêu cầu chuyển đổi điểm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="conversionPointStatus"></param>
        Task ChangeStatusConversionPoint(UpdateLoyConversionPointStatusDto input, int conversionPointStatus);
        PagingResult<FindAllLoyConversionPointDto> FindAll(FilterLoyConversionPointDto dto);
        LoyConversionPointDto FindById(int id);
        void Update(UpdateLoyConversionPointDto input);

        #region App
        /// <summary>
        /// Tạo yêu cầu đổi voucher trên App
        /// </summary>
        Task<LoyConversionPointDto> AppConversionPointVoucher(AppCreateConversionPointDto input);

        /// <summary>
        /// Lấy danh sách Ưu đãi voucher có thể sử dụng
        /// </summary>
        /// <returns></returns>
        List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherCanUse(int tradingProviderId);


        /// <summary>
        /// Lấy danh sách Ưu đãi voucher có thể sử dụng lấy 4 voucher mới nhất
        /// </summary>
        /// <returns></returns>
        List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherNewCanUse(int tradingProviderId);

        /// <summary>
        /// Lấy danh sách Giao dịch voucher đã đổi
        /// </summary>
        /// <returns></returns>
        List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherIsConversionPoint(int tradingProviderId);


        /// <summary>
        /// Lấy danh sách Voucher đã hết hạn sử dụng
        /// </summary>
        /// <returns></returns>
        List<AppLoyConversionPointByInvestorDto> AppInvestorVoucherIsExpired(int tradingProviderId);

        /// <summary>
        /// Xem chi tiết voucher của nhà đầu tư
        /// </summary>
        /// <param name="conversionPointDetailId"></param>
        /// <returns></returns>
        AppLoyConversionPointByInvestorInfoDto AppInvestorVoucherInfo(int conversionPointDetailId);

        /// <summary>
        /// Lịch sử chuyển đổi điểm của nhà đầu tư trong đại lý
        /// </summary>
        List<AppHistoryConversionPointDto> AppHistoryConversionPoint(int tradingProviderId);
        #endregion
    }
}

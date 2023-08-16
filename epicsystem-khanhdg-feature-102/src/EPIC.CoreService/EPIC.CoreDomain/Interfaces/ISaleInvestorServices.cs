using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.SaleInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ISaleInvestorServices
    {
        /// <summary>
        /// Sale đăng ký investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResultAddInvestorDto RegisterInvestor(SaleRegisterInvestorDto dto);

        /// <summary>
        /// Sale Ekyc giấy tờ hộ investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EKYCOcrResultDto> UpdateEkycIdentification(EkycSaleInvestorDto input);
        /// <summary>
        /// Sale confirm quét giấy tờ và chỉnh sửa thông tin
        /// </summary>
        /// <param name="dto"></param>
        void ConfirmAndUpdateEkyc(SaleInvestorConfirmUpdateDto dto);
        /// <summary>
        /// Sale cập nhật ảnh đại diện hộ investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        string UploadAvatar(SaleInvestorUploadAvatarDto dto);
        /// <summary>
        /// Sale thêm địa chỉ hộ investor
        /// </summary>
        /// <param name="dto"></param>
        void AddContactAddress(CreateContactAddressDto dto);
        /// <summary>
        /// Sale thêm ngân hàng hộ investor
        /// </summary>
        /// <param name="dto"></param>
        Task AddBank(CreateBankDto dto);
        /// <summary>
        /// Sale up file nđt chuyên nghiệp hộ investor
        /// </summary>
        /// <param name="dto"></param>
        void UploadProfFile(SaleInvestorUploadProfFileDto dto);

        /// <summary>
        /// Lấy danh sách investor theo sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        StatisticInvestorBySaleDto GetListInvestorBySale(GetInvestorBySaleDto dto);

        /// <summary>
        /// Lọc investor cho sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        List<ViewInvestorsBySaleDto> FilterInvestor(FilterManagerInvestorDto dto);

        /// <summary>
        /// Sale xem thông tin chi tiết khách hàng
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        ViewInvestorInfoForSaleDto GetDetailInvestorInfo(int investorId);

        /// <summary>
        /// Xem chi tiết nhà đầu tư theo sale và doanh số trong đại lý
        /// </summary>
        InvestorInfoBySaleDto InvestorInfoBySale(int investorId, int tradingProviderId);
    }
}

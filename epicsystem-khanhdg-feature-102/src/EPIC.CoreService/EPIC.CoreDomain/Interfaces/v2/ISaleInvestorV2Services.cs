using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities.Dto.SaleInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces.v2
{
    public interface ISaleInvestorV2Services
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
        /// Lấy danh sách tất cả khách hàng của sale
        /// </summary>
        /// <returns></returns>
        IEnumerable<SaleInvestorInfoDto> GetAllListInvestor();
    }
}

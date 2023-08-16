//using EPIC.CoreEntities.Dto.Investor;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.IdentityEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IManagerInvestorServices
    {
        List<InvestorDto> FindAllList(string keyword);
        public InvestorTemporary CreateInvestorTemporary(CreateManagerInvestorEkycDto dto);
        public int CreateIdentificationTemporary(CreateIdentificationTemporaryDto dto);
        public void CreateApproveRequest(RequestApproveDto dto);

        public int CreateBankTemporary(CreateInvestorBankTempDto dto);

        public void CreateUser(CreateUserByInvestorDto dto);

        public Task Approve(ApproveManagerInvestorDto input);

        public void Check(ApproveManagerInvestorDto input);

        public void Cancel(CancelRequestManagerInvestorDto input);

        public Task<EKYCOcrResultDto> EkycOCRAsync(EkycManagerInvestorDto input);

        public int UpdateInvestor(UpdateManagerInvestorDto dto);

        public void UpdateInvestorCongTyChungKhoan(UpdateCongTyChungKhoanDto dto);

        public ViewManagerInvestorTemporaryDto FindById(int investorId, bool isTemp, OptionFindDto options);

        public PagingResult<ViewManagerInvestorTemporaryDto> GetListToRequest(FindListInvestorDto dto);

        public PagingResult<ViewManagerInvestorDto> GetListInvestor(FindListInvestorDto dto);

        public PagingResult<ViewUserDto> GetListUsers(FindUserByInvestorId dto);


        public PagingResult<InvestorBankAccount> GetBankPaging(int pageSize, int pageNumber, string keyword, int investorId, bool isTemp);

        public Task<EKYCFaceMatchResultDto> UploadFaceImageAsync(UploadFaceImageDto input);

        public void ChangeUserStatus(ChangeUserStatusDto dto);

        public string ResetUserPassword(ResetUserPasswordManagerInvestorDto dto);

        public string ResetPin(ResetPinDto dto);

        public void SetDefaultBank(SetDefaultBankDto dto);

        public void SetDefaultIdentification(SetDefaultIdentificationDto dto);
        PagingResult<InvestorContactAddress> GetListContactAddress(int? pageSize, int? pageNumber, string keyword, int investorId, bool isTemp);
        int AddContactAddress(CreateManagerInvestorContactAddressDto dto);
        void UpdateContactAddress(UpdateContactAddressDto dto);
        void SetDefaultContactAddress(SetDefaultManagerInvestorContactAddressDto dto);
        PagingResult<ViewManagerInvestorDto> FilterInvestor(FilterManagerInvestorDto dto);

        public PagingResult<InvestorTemporary> GetListRequestProf(int? pageSize, int? pageNumber, string keyword, int? status);
        public void ApproveProf(ApproveProfDto dto);
        void InvestorCheckProf();
        List<ViewManagerInvestorDto> GetIntroduceUsers(int investorId);
        int UploadAvatar(UploadAvatarDto dto);
        ViewInvestorUpdateHistoryDto GetDiff(int approveId);
        ViewManagerInvestorTemporaryDto FindByGroupId(int investorGroupId);
        void CancelRequestProf(CancelRequestInvestorProfDto dto);
        List<InvestorProfFile> GetListProfFile(int investorTempId);
        public List<SaleManagerInvestorDto> GetListSale(int investorId);
        public void AddSale(AddSaleManagerInvestorDto dto);
        public void SetDefaultSale(UpdateSaleIsDefaultDto dto);

        /// <summary>
        /// Tạo yêu cầu duyệt email
        /// </summary>
        /// <param name="dto"></param>
        void CreateRequestEmail(CreateRequestEmailDto dto);

        /// <summary>
        /// Tạo yêu cầu duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        void CreateRequestPhone(CreateRequestPhoneDto dto);



        /// <summary>
        /// Duyệt email
        /// </summary>
        /// <param name="dto"></param>
        void ApproveEmail(ApproveEmailDto dto);

        /// <summary>
        /// Duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        void ApprovePhone(ApprovePhoneDto dto);

        /// <summary>
        /// Huỷ duyệt email
        /// </summary>
        /// <param name="dto"></param>
        void CancelRequestEmail(CancelRequestNotInvestor dto);

        /// <summary>
        /// Huỷ duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        void CancelRequestPhone(CancelRequestNotInvestor dto);

        public List<InvestorStock> GetListStockNoPaging(FindInvestorStockDto dto);
        public int CreateInvestorStock(CreateInvestorStockDto dto);
        public void SetDefaultStock(SetDefaultInvestorStockDto dto);

        /// <summary>
        /// Lấy investor đăng ký trên app cho màn hình tài khoản chưa xác minh
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<InvestorNoEkycDto> FindAppNoEkycInvestor(FindInvestorNoEkycDto dto);

        Task UpdateAppInvestorIdentification(UpdateAppInvestorIdentificationDto dto);
        int DeleteBankAccount(DeleteBankAccountDto dto);

        /// <summary>
        /// Thay thế giấy tờ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int ReplaceIdentification(ReplaceIdentificationDto dto);

        /// <summary>
        /// Cập nhật liên kết ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateInvestorBankAcc(UpdateInvestorBankAccDto dto);

        /// <summary>
        /// Xóa Invesor các thông tin liên quan theo InvestorId hoặc phone
        /// </summary>
        void DeletedInvestor(int? investorId, string phone);
        PagingResult<ViewManagerInvestorTemporaryDto> GetListToRequests(FindListInvestorDto dto);
    }
}

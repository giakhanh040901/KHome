//using EPIC.CoreEntities.Dto.Investor;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IInvestorServices
    {
        List<InvestorDto> Find();
        void RegisterInventor(RegisterInvestorDto input);
        Task CreateVerificationCodeSendSmsAsync(InvestorEmailPhoneDto input);
        Task CreateVerificationCodeSendMailAsync(InvestorEmailPhoneDto input);
        bool CheckEmailExist(CheckEmailExistDto model);
        bool CheckPhoneExist(CheckPhoneExistDto model);
        void VerifyCode(VerificationCodeDto input);
        bool IsRegisteredOffline(CheckRegisteredOfflineDto input);
        Investor GetByEmailOrPhone(string phone);
        Investor GetByUsername(string username);
        Task ForgotPasswordAsync(ForgotPasswordDto input);
        VerifyOtpResultDto VerifyOTPResetPass(VerifyOTPDto input);
        void ResetPassword(ResetPasswordDto input);
        void ResetPassword(ResetPasswordManagerInvestorDto input);
        void ChangePassword(ChangePasswordDto input);
        bool ValidatePassword(string phone, string password, string fcmToken);
        void ChangePin(ChangePinDto input);
        int UpdateInvestor(int id, UpdateInvestorDto input);
        void AddBankAccount(BankAccountDto input);
        bool ValidatePin(ValidatePinDto input);
        Task<EKYCOcrResultDto> EkycOCRAsync(EKYCOcrDto input);
        void EkycConfirmInfo(EKYCConfirmInfoDto input);
        /// <summary>
        /// Nhận diện gương mặt. So khớp với ảnh upload lên
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EKYCFaceMatchResultDto> EkycFaceMatchAsync(EKYCFaceMatchDto input);

        /// <summary>
        /// Nhận diện gương mặt. So khớp với ảnh của giấy tờ mặc định
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EKYCFaceMatchResultDto> EkycFaceMatchLoggedInAsync(FaceRecognitionLoggedInDto input);
        PagingResult<InvestorDto> FindAll(int pageSize, int pageNumber, string keyword);
        Investor FindById(int id);
        int DeteleInvestor(int id);
        void ChangePasswordTemp(ChangePasswordTempDto input);
        int DeteleManagerInvestor(int id);
        int ActivateManagerInvestor(int investorId, bool isActive);
        List<IdDto> GetListIdInfo();
        List<TransactionAddessDto> GetListTransactionAddress();
        void AddTransactionAddess();
        void GenerateOtpMail();
        void GenerateOtpSms();
        List<ViewInvestorContactAddressDto> GetListContactAddress(string keyword);
        void AddContactAddress(CreateContactAddressDto dto);
        Task UpdateContactAddress(UpdateContactAddressDto dto);
        void SetDefaultContactAddress(SetDefaultContactAddressDto dto);
        void SetDefaultIdentification(int identificationId);
        List<InvestorIdentification> GetListIdentification(AppGetListIdentificationDto dto);
        InvestorIdentification GetDefaultIdentification();
        Task<EKYCOcrResultDto> AddIdentification(AddIdentificationDto input);
        void UploadProfFile(UploadProfFileDto dto);
        /// <summary>
        /// Upload file nhà đầu tư chuyên nghiệp dùng chung
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="investorId">investorId được upload</param>
        /// <param name="username">username của người thao tác upload</param>
        /// <param name="userId">userId của người thao tác upload</param>
        void UploadProfFileCommon(UploadProfFileDto dto, int investorId, string username, int userId);
        InvestorMyInfoDto GetMyInfo();
        List<InvestorBankAccount> GetListBank();
        void AddBank(CreateBankDto dto);
        public void SetBankDefault(int investorBankId);
        Task<string> SendVerifyEmail();
        void VerifyEmail(string verifyCode);
        void ConfirmIdentificationEkyc(ConfirmIdentificationEkycDto dto);
        public void UploadAvatar(AppUploadAvatarDto dto);
        void ScanReferralCodeFirstTime(ScanReferralCodeFirstTimeDto dto);
        List<SaleInfoByInvestorDto> GetListReferralCodeSale();
        void SetDefaultReferralCodeSale(decimal id);
        void ScanReferralCodeSale(ScanReferralCodeSaleDto dto);
        /// <summary>
        /// Check otp theo user id đang đăng nhập
        /// </summary>
        /// <param name="otp"></param>
        void CheckOtp(string otp);
        AppShortSalerInfoDto GetShortSalerInfoByReferralCodeSelf(string referralCode);
        bool isReferralCodeExist(string referralCode);
        void DeactiveMyUserAccount(AppDeactiveMyUserAccountDto dto);
        void DeleteBankAccount(AppDeleteBankDto dto);
        void GenerateOtpSmsByPhone(string phone);
        List<InvestorTodoDto> FindAllTodo();
        List<InvestorTodoDto> GetAllTodo();
        void UpdateStatusToDo(int toDoId);
        void UpdateStatusListToDo(List<int> toDoIds);
        List<int> FindAllTradingProviderIds();
    }
}

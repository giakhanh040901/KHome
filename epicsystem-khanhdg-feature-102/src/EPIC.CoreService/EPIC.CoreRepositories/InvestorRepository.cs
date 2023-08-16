using Dapper.Oracle;
using EPIC.CoreEntities.Dto.ExcelReport;
using EPIC.CoreSharedEntities.Dto.Investor;
//using EPIC.CoreEntities.Dto.Investor;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Auth;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;

namespace EPIC.CoreRepositories
{
    public class InvestorRepository
    {
        private OracleHelper _oracleHelper;
        private const string PROC_GET_ALL_INVESTORS = "EPIC.PKG_INVESTOR.PROC_GET_ALL_INVESTORS";
        private const string PROC_INVESTOR_FIND_ALL = "EPIC.PKG_INVESTOR.PROC_INVESTOR_FIND_ALL";
        private const string PROC_INVESTOR_FIND_BY_ID = "EPIC.PKG_INVESTOR.PROC_INVESTOR_FIND_BY_ID";
        private const string PROC_INVESTOR_CHECK_EMAIL_EXIST = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CK_EMAIL_EXIST";
        private const string PROC_INVESTOR_CHECK_PHONE_EXIST = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CK_PHONE_EXIST";
        private const string PROC_INVESTOR_IS_REGISTERED_OFFLINE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_IS_REGIS_OFFLINE";
        private const string PROC_INVESTOR_CREATE_VERIFICATION_CODE = "PKG_INVESTOR.PROC_INVESTOR_CREATE_VERIFY";
        private const string PROC_INVESTOR_VERIFY_CODE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VERIFY_CODE";
        private const string PROC_INVESTOR_REGISTER = "EPIC.PKG_INVESTOR.PROC_INVESTOR_REGISTER";
        private const string PROC_INVESTOR_VALIDATE_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VALID_PASSWORD";
        private const string PROC_INVESTOR_GET_BY_EMAIL_OR_PHONE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_GET_BY_MAILPHONE";
        private const string PROC_INVESTOR_GET_BY_USERNAME = "EPIC.PKG_INVESTOR.PROC_INVESTOR_GET_BY_USERNAME";
        private const string PROC_INVESTOR_FORGOT_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_FORGOT_PASSWORD";
        private const string PROC_INVESTOR_VERIFY_OTP_RESET_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VERIFY_RESET_P";
        private const string PROC_INVESTOR_GET_BY_USER_ID = "EPIC.PKG_INVESTOR.PROC_INVESTOR_GET_BY_USER_ID";
        private const string PROC_INVESTOR_RESET_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_RESET_PASSWORD";
        private const string PROC_INVESTOR_CHANGE_PIN = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CHANGE_PIN";
        private const string PROC_INVESTOR_VALIDATE_PIN = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VALIDATE_PIN";
        private const string PROC_INVESTOR_UPDATE_EKYC_ID = "EPIC.PKG_INVESTOR.PROC_INVESTOR_UPDATE_EKYC_ID";
        private const string PROC_INVESTOR_EKYC_CONFIRM_INFO = "EPIC.PKG_INVESTOR.PROC_INVES_EKYC_CONFIRM_INFO";
        private const string PROC_INVESTOR_EKYC_FINISH = "EPIC.PKG_INVESTOR.PROC_INVESTOR_EKYC_FINISH";
        private const string PROC_INVESTOR_UPDATE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_UPDATE";
        private const string PROC_INVESTOR_DELETE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_DELETE";
        private const string PROC_MANAGER_INVESTOR_ACTIVATE = "EPIC.PKG_MANAGER_INVESTOR.PROC_MANAGER_INVESTOR_ACTIVATE";
        private const string PROC_MANAGER_INVESTOR_RESET_PASSWORD = "EPIC.PKG_MANAGER_INVESTOR.PROC_MANAGER_RESET_PASSWORD";
        private const string PROC_MANAGER_INVESTOR_DELETE = "EPIC.PKG_MANAGER_INVESTOR.PROC_MANAGER_INVESTOR_DELETE";
        private const string PROC_FINAL_STEP = "EPIC.PKG_INVESTOR.PROC_FINAL_STEP";
        private const string PROC_GENERATE_OTP = "EPIC.PKG_INVESTOR.PROC_GENERATE_OTP";
        private const string PROC_PHONE_GENERATE_OTP = "EPIC.PKG_INVESTOR.PROC_PHONE_GENERATE_OTP";
        private const string PROC_ADD_IDEN = "EPIC.PKG_INVESTOR.PROC_ADD_IDEN";
        private const string PROC_CONFIRM_IDEN = "EPIC.PKG_INVESTOR.PROC_CONFIRM_IDEN";

        private const string PROC_INVESTOR_CHECK_IS_PROF = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CHECK_IS_PROF";
        private const string PROC_INVESTOR_GET_BY_VERIFY_EMAIL_CODE = "EPIC.PKG_INVESTOR.PROC_INV_GET_BY_VRF_EMAIL";

        private const string PROC_INVESTOR_CHECK_OTP = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CHECK_OTP";
        private const string PROC_APP_DEACTIVE_ACCOUNT = "EPIC.PKG_INVESTOR.PROC_APP_DEACTIVE_ACCOUNT";

        private const string PROC_GET_ID_WHEN_REGISTER = "EPIC.PKG_INVESTOR.PROC_GET_ID_WHEN_REGISTER";
        private const string PROC_FACE_MATCH = "EPIC.PKG_INVESTOR.PROC_FACE_MATCH";
        private const string PROC_APP_GET_CHAT_TRADING = "EPIC.PKG_INVESTOR.PROC_APP_GET_CHAT_TRADING";
        private const string PROC_GET_LIST_CUSTOMER_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_CUSTOMER";
        private const string PROC_GET_LIST_CUSTOMER_ROOT_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_CUSTOMER_ROOT";
        private const string PROC_GET_LIST_USER_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_USER";

        private const string PROC_GET_LIST_CUSTOMER_HVF_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_CUSTOMER_HVF";
        private const string PROC_GET_LIST_CUSTOMER_INFO_CHANGE_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_CUSTOMER_INFO_CHANGE";

        #region CONTACT ADDRESS
        private const string PROC_ADD_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR.PROC_ADD_CONTACT_ADDRESS";
        private const string PROC_UPDATE_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR.PROC_UPDATE_CONTACT_ADDRESS";
        private const string PROC_SET_DEFAULT_ADDRESS = "EPIC.PKG_INVESTOR.PROC_SET_DEFAULT_ADDRESS";
        private const string PROC_GET_ONE_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_GET";
        private const string PROC_GET_LIST_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_GET_PAGING";
        private const string PROC_CONTRACT_ADDRESS_GET = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_CONTRACT_ADDRESS_GET";
        private const string PROC_CONTRACT_ADDRESS_DEFAULT = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_CONTRACT_ADDRESS_DEFAULT";
        #endregion

        #region BANK
        private const string PROC_ADD_BANK = "EPIC.PKG_INVESTOR.PROC_ADD_BANK";
        private const string PROC_APP_DELETE_BANK_ACC = "EPIC.PKG_INVESTOR.PROC_APP_DELETE_BANK_ACC";

        #endregion

        #region VERIFY EMAIL
        private const string PROC_INSERT_VERIFY_EMAIL_CODE = "EPIC.PKG_INVESTOR.PROC_INSERT_VERIFY_EMAIL_CODE";
        private const string PROC_CHECK_VERIFY_EMAIL_CODE = "EPIC.PKG_INVESTOR.PROC_CHECK_VERIFY_EMAIL_CODE";
        #endregion

        #region Mã giới thiệu - Sale
        private const string PROC_REGIS_REFERRAL_CODE = "EPIC.PKG_INVESTOR.PROC_REGIS_REFERRAL_CODE";
        private const string PROC_SET_DEFAULT_REF_CODE = "EPIC.PKG_INVESTOR.PROC_SET_DEFAULT_REF_CODE";
        private const string PROC_SCAN_REFERRAL_CODE_SALE = "EPIC.PKG_INVESTOR.PROC_SCAN_REFERRAL_CODE_SALE";
        private const string PROC_GET_LIST_REF_CODE = "EPIC.PKG_INVESTOR.PROC_GET_LIST_REF_CODE";
        private const string PROC_IS_SALE = "EPIC.PKG_INVESTOR.PROC_IS_SALE";
        private const string PROC_IS_REFERRAL_CODE_EXIST = "EPIC.PKG_INVESTOR.PROC_IS_REFERRAL_CODE_EXIST";
        private const string PROC_GET_DEPARMENTS_BY_INV_ID = "EPIC.PKG_INVESTOR.PROC_GET_DEPARMENTS_BY_INV_ID";
        #endregion

        public Investor GetByUserId(int userId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_INVESTOR_GET_BY_USER_ID, new
            {
                pv_USER_ID = userId
            });
        }

        public InvestorRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public Investor GetByEmailOrPhone(string phone)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_INVESTOR_GET_BY_EMAIL_OR_PHONE, new
            {
                pv_EMAIL_OR_PHONE = phone,
            });
        }

        public Investor GetByUsername(string username)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_INVESTOR_GET_BY_USERNAME, new
            {
                pv_USERNAME = username,
            });
        }

        public OtpDto ForgotPassword(string emailOrPhone)
        {
            string OTP = null;
            DateTime exp = new();
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_EMAIL_OR_PHONE", emailOrPhone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_OTP_CODE", OTP, OracleMappingType.Varchar2, ParameterDirection.Output, 50);
            parameters.Add("pv_OTP_EXP", exp, OracleMappingType.Date, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_FORGOT_PASSWORD, parameters);

            OTP = parameters.Get<string>("pv_OTP_CODE");
            exp = parameters.Get<DateTime>("pv_OTP_EXP");
            return new OtpDto
            {
                Otp = OTP,
                Exp = exp
            };
        }

        /// <summary>
        /// Xác thực mã otp, kèm theo lưu reset password token
        /// </summary>
        /// <param name="emailOrPhone"></param>
        /// <param name="OTP"></param>
        /// <param name="resetPasswordToken"></param>
        /// <returns>1: valid, 0: invalid, -1: expire</returns>
        public int VerifyOTPResetPass(string emailOrPhone, string OTP, string resetPasswordToken)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_EMAIL_OR_PHONE", emailOrPhone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_OTP", OTP, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESET_PASSWORD_TOKEN", resetPasswordToken, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_VERIFY_OTP_RESET_PASSWORD, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (int)result;
        }

        public int ResetPassword(string emailOrPhone, string resetPasswordToken, string password)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_RESET_PASSWORD, new
            {
                pv_EMAIL_OR_PHONE = emailOrPhone,
                pv_RESET_PASSWORD_TOKEN = resetPasswordToken,
                pv_PASSWORD = CommonUtils.CreateMD5(password),
            });
        }

        /// <summary>
        /// Đã đăng ký offline trả về true, chưa tồn tại trả về false
        /// </summary>
        public bool IsRegisteredOffline(string email, string phone)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_EMAIL", email, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_PHONE", phone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_IS_REGISTERED_OFFLINE, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return result == TrueOrFalseNum.TRUE;
        }

        public int ChangePin(int investorId, string oldPin, string newPin)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_CHANGE_PIN, new
            {
                pv_INVESTOR_ID = investorId,
                pv_OLD_PIN = CommonUtils.CreateMD5(oldPin),
                pv_NEW_PIN = CommonUtils.CreateMD5(newPin),
            });
        }

        public bool ValidatePin(int investorId, string pin)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_INVESTOR_ID", investorId, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_PIN", CommonUtils.CreateMD5(pin), OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_VALIDATE_PIN, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return result == TrueOrFalseNum.TRUE;
        }

        public bool ValidatePassword(string emailOrPhone, string password)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_EMAIL_OR_PHONE", emailOrPhone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_PASSWORD", CommonUtils.CreateMD5(password), OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_VALIDATE_PASSWORD, parameters, false);

            result = parameters.Get<decimal>("pv_RESULT");
            return result == TrueOrFalseNum.TRUE;
        }

        /// <summary>
        /// Lưu thông tin cccd, cmnd, passport
        /// </summary>
        /// <returns></returns>
        public int UpdateEkycId(string phone, string name, DateTime? dateOfBirth, string sex, string idNo, DateTime? issueDate,
            DateTime? issueExpDate, string issuer, string placeOfOrigin, string placeOfResidence, string nationality, string idType,
            string frontImage, string backImage)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_UPDATE_EKYC_ID, new
            {
                pv_PHONE = phone,
                pv_NAME = name,
                pv_BIRTH_DATE = dateOfBirth,
                pv_SEX = sex,
                pv_ID_NO = idNo,
                pv_ID_DATE = issueDate,
                pv_ID_EXPIRED_DATE = issueExpDate,
                pv_ID_ISSUER = issuer,
                pv_PLACE_OF_ORIGIN = placeOfOrigin,
                pv_PLACE_OF_RESIDENCE = placeOfResidence,
                pv_NATIONALITY = nationality,
                pv_ID_TYPE = idType,
                pv_ID_FRONT_IMAGE_URL = frontImage,
                pv_ID_BACK_IMAGE_URL = backImage
            });
        }

        /// <summary>
        /// Lưu thông tin và sinh mã xác thực phục vụ cho việc 
        /// xác thực khi người dùng nhập mã.
        /// Nếu email, sđt đã tồn tại thì update mã xác thực
        /// </summary>
        /// <param name="input"></param>
        /// <param name="expirationTime">Thời gian hết hạn đơn vị giây</param>
        /// <returns>Mã xác thực</returns>
        public OtpDto CreateVerificationCode(InvestorEmailPhoneDto input)
        {
            string OTP = null;
            DateTime exp = new();
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_EMAIL", input.Email, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_PHONE", input.Phone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_OTP_CODE", OTP, OracleMappingType.Varchar2, ParameterDirection.Output, 50);
            parameters.Add("pv_OTP_EXP", exp, OracleMappingType.Date, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_CREATE_VERIFICATION_CODE, parameters);

            OTP = parameters.Get<string>("pv_OTP_CODE");
            exp = parameters.Get<DateTime>("pv_OTP_EXP");
            return new OtpDto
            {
                Otp = OTP,
                Exp = exp
            };
        }

        /// <summary>
        /// Kiểm tra mã xác thực, thời hạn mã xác thực
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="verificationCode"></param>
        /// <returns>1: valid, 0: invalid, -1: expire</returns>
        public int VerifyCode(string email, string phone, string verificationCode)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_EMAIL", email, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_PHONE", phone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_VERIFICATION_CODE", verificationCode, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_VERIFY_CODE, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (int)result;
        }

        public void EkycConfirmInfo(string phone, bool isConfirmed, string incorrectFields, string sex)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_EKYC_CONFIRM_INFO, new
            {
                pv_PHONE = phone,
                pv_IS_CONFIRMED = isConfirmed ? YesNo.YES : YesNo.NO,
                pv_EKYC_INCORRECT_FIELDS = incorrectFields,
                pv_SEX = sex
            });
        }

        /// <summary>
        /// Tạo tài khoản đăng nhập
        /// </summary>
        public void Register(RegisterInvestorDto input)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_REGISTER, new
            {
                pv_PHONE = input.Phone,
                pv_EMAIL = input.Email,
                pv_PASSWORD = CommonUtils.CreateMD5(input.Password),
                pv_REFERRAL_CODE = input.ReferralCode
            });
        }

        public bool CheckPhoneExist(string phone)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_PHONE", phone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Int32, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_CHECK_PHONE_EXIST, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return result == TrueOrFalseNum.TRUE;
        }

        public bool CheckEmailExist(string email)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_EMAIL", email, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Int32, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVESTOR_CHECK_EMAIL_EXIST, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return result == TrueOrFalseNum.TRUE;
        }

        public int FinishEKYC(string phone, string faceImageUrl)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_EKYC_FINISH, new
            {
                pv_PHONE = phone,
                pv_FACE_IMAGE_URL = faceImageUrl,
            });
        }

        public int Delete(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_DELETE, new
            {
                pv_INVESTOR_ID = id
            });
        }

        public List<Investor> GetAll()
        {
            IEnumerable<Investor> investors = _oracleHelper.ExecuteProcedure<Investor>(PROC_GET_ALL_INVESTORS, null);
            return investors.ToList();
        }

        public Investor FindById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_INVESTOR_FIND_BY_ID, new
            {
                pv_INVESTOR_ID = id
            });
        }

        public int Update(Investor entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_UPDATE, new
            {
                pv_INVESTOR_ID = entity.InvestorId,
                pv_NAME = entity.Name,
                pv_ADDRESS = entity.Address,
                pv_CONTACT_ADDRESS = entity.ContactAddress,
                pv_NATIONALITY = entity.Nationality,
            });
        }

        public PagingResult<InvestorDto> FindAll(int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvestorDto>(PROC_INVESTOR_FIND_ALL, new
            {
                pv_PAGE_SIZE = pageSize,
                pv_PAGE_NUMBER = pageNumber,
                pv_KEY_WORD = keyword
            });
            return result;
        }

        /// <summary>
        /// Manager Investor: Activate tài khoản bằng InvestorId
        /// </summary>
        public int ActivateManagerInvestor(int id, bool isActive, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_MANAGER_INVESTOR_ACTIVATE, new
            {
                pv_INVESTOR_ID = id,
                pv_IS_ACTIVE = isActive,
                pv_MODIFIED_BY = modifiedBy
            });
        }
        /// <summary>
        /// Manager Investor: ResetPassword bằng InvestorId
        /// </summary>
        public int ManagerResetPassword(string emailOrPhone, string password)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_MANAGER_INVESTOR_RESET_PASSWORD, new
            {
                pv_EMAIL_OR_PHONE = emailOrPhone,
                pv_PASSWORD = CommonUtils.CreateMD5(password),
            });
        }

        /// <summary>
        /// Manager Investor: Delete của USERS và EP_INVESTOR
        /// </summary>
        public int DeleteManagerInvestor(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_MANAGER_INVESTOR_DELETE, new
            {
                pv_INVESTOR_ID = id
            });
        }

        /// <summary>
        /// FINAL STEP => TẠO TEMPORARY TỪ BẢNG THẬT
        /// </summary>
        public void CreateTemporary(string phone)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_FINAL_STEP, new
            {
                pv_PHONE = phone,
            }, false);
        }

        public string GenerateOtp(int investorId)
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_INVESTOR_ID", investorId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_OTP", "", OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_GENERATE_OTP, parameters);
            return parameters.Get<string>("pv_OTP");
        }

        /// <summary>
        /// Generate OTP giao nhận hợp đồng theo Phone dựa theo cifcode (investor hay BusinessCustomer)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string GenerateOtpByPhone(string phone)
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_PHONE", phone, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("pv_OTP", "", OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_PHONE_GENERATE_OTP, parameters);

            return parameters.Get<string>("pv_OTP");
        }

        /// <summary>
        /// Tạo bank
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void AddBank(CreateBankDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_ADD_BANK, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_BANK_ID = dto.BankId,
                pv_BANK_ACCOUNT = dto.BankAccount,
                pv_OWNER_ACCOUNT = dto.OwnerAccount,
                pv_IS_DEFAULT = dto.IsDefault,
                SESSION_USERNAME = username,
            });
        }

        /// <summary>
        /// Tạo giấy tờ thật
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CreateIdentification(CreateIdentificationTemporaryDto dto, string username)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_ADD_IDEN, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_ID_TYPE = dto.IdType,
                pv_ID_NO = dto.IdNo,
                pv_FULLNAME = dto.Fullname,
                pv_DATE_OF_BIRTH = dto.DateOfBirth,
                pv_NATIONALITY = dto.Nationality,
                pv_PERSONAL_IDENTIFICATION = dto.PersonalIdentification,
                pv_ID_ISSUER = dto.IdIssuer,
                pv_ID_DATE = dto.IdDate,
                pv_ID_EXPIRED_DATE = dto.IdExpiredDate,
                pv_PLACE_OF_ORIGIN = dto.PlaceOfOrigin,
                pv_PLACE_OF_RESIDENCE = dto.PlaceOfResidence,
                pv_ID_FRONT_IMAGE_URL = dto.IdFrontImageUrl,
                pv_ID_BACK_IMAGE_URL = dto.IdBackImageUrl,
                pv_ID_EXTRA_IMAGE_URL = dto.IdExtraImageUrl,
                pv_FACE_IMAGE_URL = dto.FaceImageUrl,
                pv_FACE_VIDEO_URL = dto.FaceVideoUrl,
                pv_SEX = dto.Sex,
                SESSION_USERNAME = username
            }, false);

            return investorIdTemp;
        }

        #region contact address

        /// <summary>
        /// List contact address
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public List<InvestorContactAddress> GetListContactAddress(string keyword, int investorId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvestorContactAddress>(PROC_GET_LIST_CONTACT_ADDRESS, new
            {
                PAGE_SIZE = -1,
                PAGE_NUMBER = -1,
                KEYWORD = keyword,
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = 0,
            });

            return result.Items?.ToList();
        }

        /// <summary>
        /// Tạo contact address thật
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void CreateContactAddress(CreateContactAddressDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_ADD_CONTACT_ADDRESS, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_CONTACT_ADDRESS = dto.ContactAddress,
                pv_IS_DEFAULT = dto.IsDefault,
                pv_DETAIL_ADDRESS = dto.DetailAddress,
                pv_PROVINCE_CODE = dto.ProvinceCode,
                pv_DISTRICT_CODE = dto.DistrictCode,
                pv_WARD_CODE = dto.WardCode,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// Cập nhật contact address thật
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void UpdateContactAddress(UpdateContactAddressDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_CONTACT_ADDRESS, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_CONTACT_ADDRESS_ID = dto.ContactAddressId,
                pv_CONTACT_ADDRESS = dto.ContactAddress,
                pv_DETAIL_ADDRESS = dto.DetailAddress,
                pv_PROVINCE_CODE = dto.ProvinceCode,
                pv_DISTRICT_CODE = dto.DistrictCode,
                pv_WARD_CODE = dto.WardCode,
                pv_IS_DEFAULT = dto.IsDefault,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// Chọn địa chỉ mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void SetDefaultContactAddress(SetDefaultContactAddressDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SET_DEFAULT_ADDRESS, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_CONTACT_ADDRESS_ID = dto.ContactAddressId,
                SESSION_USERNAME = username
            });
        }

        public InvestorContactAddress GetContactAddress(int investorId, int contractAddressId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorContactAddress>(PROC_CONTRACT_ADDRESS_GET, new
            {
                pv_INVESTOR_ID = investorId,
                pv_CONTACT_ADDRESS_ID = contractAddressId
            });
        }

        public InvestorContactAddress GetContactAddressDefault(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorContactAddress>(PROC_CONTRACT_ADDRESS_DEFAULT, new
            {
                pv_INVESTOR_ID = investorId
            });
        }
        #endregion

        /// <summary>
        /// Kiểm tra xem nhà đầu tư có là nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="investorId"></param>
        public void InvestorCheckProf(int investorId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_CHECK_IS_PROF, new
            {
                pv_INVESTOR_ID = investorId,
            });
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public void OpenConnection()
        {
            _oracleHelper.OpenConnection();
        }

        /// <summary>
        /// Sinh mã xác thực email
        /// </summary>
        /// <param name="invesetorId"></param>
        /// <returns></returns>
        public string GenVerifyEmailCode(int invesetorId)
        {
            var code = _oracleHelper.ExecuteProcedureToFirst<string>(PROC_INSERT_VERIFY_EMAIL_CODE, new
            {
                pv_INVESTOR_ID = invesetorId
            });

            return code;
        }

        /// <summary>
        /// Verify mã xác thưc email
        /// </summary>
        /// <param name="verifyEmailCode"></param>
        public void VerifyEmail(string verifyEmailCode)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CHECK_VERIFY_EMAIL_CODE, new
            {
                pv_VERIFY_EMAIL_CODE = verifyEmailCode
            });
        }

        /// <summary>
        /// Xác nhận thông tin giấy tờ vừa ekyc
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="investorId"></param>
        public void ConfirmIdentificationEkyc(ConfirmIdentificationEkycDto dto, string incorrectFields, int investorId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CONFIRM_IDEN, new
            {
                pv_INVESTOR_ID = investorId,
                pv_IDENTIFICATION_ID = dto.IdentificationId,
                pv_IS_CONFIRMED = dto.IsConfirmed ? YesNo.YES : YesNo.NO,
                pv_EKYC_INCORRECT_FIELDS = incorrectFields,
                pv_SEX = dto.Sex
            });
        }

        /// <summary>
        /// Verify mã xác thưc email
        /// </summary>
        /// <param name="verifyEmailCode"></param>
        public Investor GetByVeryfiEmailCode(string verifyEmailCode)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_INVESTOR_GET_BY_VERIFY_EMAIL_CODE, new
            {
                pv_VERIFY_EMAIL_CODE = verifyEmailCode
            });
        }

        /// <summary>
        /// Investor ở app quét mã giới thiệu lần đầu
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="investorId"></param>
        public void ScanReferralCodeFirstTime(ScanReferralCodeFirstTimeDto dto, int investorId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_REGIS_REFERRAL_CODE, new
            {
                pv_INVESTOR_ID = investorId,
                pv_REFERRAL_CODE = dto.ReferralCode
            });
        }

        /// <summary>
        /// Quét mã giới thiệu của sale
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="investorId"></param>
        public void ScanReferralCodeSale(ScanReferralCodeSaleDto dto, int investorId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SCAN_REFERRAL_CODE_SALE, new
            {
                pv_INVESTOR_ID = investorId,
                pv_REFERRAL_CODE = dto.ReferralCode
            });
        }

        /// <summary>
        /// Chọn mã giới thiệu sale mặc định
        /// </summary>
        /// <param name="id"></param>
        public void SetDefaultReferralCodeSale(decimal id)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SET_DEFAULT_REF_CODE, new
            {
                pv_ID = id
            });
        }

        /// <summary>
        /// Lấy danh sách mã giới thiệu sale
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<SaleInfoByInvestorDto> GetListReferralCodeSaleByInvestorId(int investorId, int? tradingProviderId, string userType, bool appUse = false)
        {
            var rslt = _oracleHelper.ExecuteProcedure<SaleInfoByInvestorDto>(PROC_GET_LIST_REF_CODE, new
            {
                pv_INVESTOR_ID = investorId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_USER_TYPE = userType,
                pv_APP_USE = appUse ? YesNo.YES : YesNo.NO,
            });
            return rslt?.ToList();
        }

        /// <summary>
        /// Kiểm tra xem investor có phải sale ko
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public bool IsSaleById(int investorId)
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_INVESTOR_ID", investorId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_IS_SALE", "", OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_IS_SALE, parameters);

            var result = parameters.Get<string>("pv_IS_SALE");
            return result == YesNo.YES;
        }

        /// <summary>
        /// Kiểm tra mã giới thiệu có tồn tại ko
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public bool isReferralCodeExist(string referralCode)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<string>(PROC_IS_REFERRAL_CODE_EXIST, new
            {
                pv_REFERRAL_CODE = referralCode
            });

            return result == YesNo.YES;
        }

        /// <summary>
        /// Tự sát. Khóa tài khoản của chính mình
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        public void DeactiveMyUserAccount(AppDeactiveMyUserAccountDto dto, int userId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_DEACTIVE_ACCOUNT, new
            {
                pv_USER_ID = userId,
                pv_PIN_CODE = dto.PinCode
            });
        }

        public void HandleInvalidPin(IHttpContextAccessor httpContextAccessor, SysVarRepository sysVarRepository, UserRepository userRepository)
        {
            var varInvalidPin = sysVarRepository.GetVarByName("AUTH", "PIN_INVALID_COUNT");
            if (varInvalidPin == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy SysVar: GRNAME = {"AUTH"}, VARNAME = {"PIN_INVALID_COUNT"}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            int maxPinFail = int.Parse(varInvalidPin.VarValue);

            int? pinFailCount = httpContextAccessor.HttpContext.Session.GetInt32(SessionKeys.PIN_FAIL_COUNT);
            if (!pinFailCount.HasValue)
            {
                pinFailCount = 0;
                httpContextAccessor.HttpContext.Session.SetInt32(SessionKeys.PIN_FAIL_COUNT, 0);
            }

            pinFailCount++;
            httpContextAccessor.HttpContext.Session.SetInt32(SessionKeys.PIN_FAIL_COUNT, pinFailCount.Value);
            if (pinFailCount >= maxPinFail)
            {
                userRepository.Active(CommonUtils.GetCurrentUserId(httpContextAccessor), false, CommonUtils.GetCurrentUsername(httpContextAccessor));
                httpContextAccessor.HttpContext.Session.SetInt32(SessionKeys.PIN_FAIL_COUNT, 0);
            }
        }

        /// <summary>
        /// Lấy department theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<Department> GetListDepartmentByInvestorId(int investorId)
        {
            var result = _oracleHelper.ExecuteProcedure<Department>(PROC_GET_DEPARMENTS_BY_INV_ID, new
            {
                pv_INVESTOR_ID = investorId
            });

            return result?.ToList();
        }

        /// <summary>
        /// Xoá liên kết tk bank
        /// </summary>
        /// <param name="dto"></param>
        public void DeleteBankAccount(AppDeleteBankDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_DELETE_BANK_ACC, new
            {
                pv_ID = dto.Id,
            });
        }

        public void CheckOtp(int userId, string otp)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_INVESTOR_CHECK_OTP, new
            {
                pv_USER_ID = userId,
                pv_OTP = otp
            });
        }

        /// <summary>
        /// Lấy giấy tờ đầu tiên khi đăng ký
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public InvestorIdentification GetIdentificationFirstWhenRegister(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorIdentification>(PROC_GET_ID_WHEN_REGISTER, new
            {
                pv_INVESTOR_ID = investorId
            });
        }

        /// <summary>
        /// Nhận diện sau khi log in
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="faceImageUrl"></param>
        public void FaceMatchLoggedIn(int investorId, string faceImageUrl)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_FACE_MATCH, new
            {
                pv_INVESTOR_ID = investorId,
                pv_FACE_IMAGE_URL = faceImageUrl
            });
        }

        /// <summary>
        /// Lấy list dlsc theo investorId
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<TradingProvider> GetListTradingByInvestorId(int investorId)
        {
            return _oracleHelper.ExecuteProcedure<TradingProvider>(PROC_APP_GET_CHAT_TRADING, new
            {
                pv_INVESTOR_ID = investorId
            })?.ToList();
        }

        /// <summary>
        /// Lấy danh sách thông tin báo cáo excel khách hàng
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CustomerExcelReport> GetListCustomerExcelReport(string userType, int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            return _oracleHelper.ExecuteProcedure<CustomerExcelReport>(PROC_GET_LIST_CUSTOMER_EXCEL_REPORT, new
            {
                pv_USER_TYPE = userType,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_END_DATE = endDate,
                pv_START_DATE = startDate
            })?.ToList();
        }

        /// <summary>
        /// Lấy dữ liệu thông tin khách hàng root
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<CustomerRootExcel> GetListCustomerRootReport(int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            var result = _oracleHelper.ExecuteProcedure<CustomerRootExcel>(PROC_GET_LIST_CUSTOMER_ROOT_EXCEL_REPORT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate
            });
            return result;
        }

        /// <summary>
        /// Lấy dữ liệu thông tin user
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<UserReportDto> GetListUserReport(DateTime? startDate, DateTime? endDate)
        {
            var result = _oracleHelper.ExecuteProcedure<UserReportDto>(PROC_GET_LIST_USER_EXCEL_REPORT, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate
            }).ToList();
            return result;
        }

        /// <summary>
        /// Lấy danh sách khách hàng HVF
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CustomerHVFExcelReport> GetListCustomerHVFExcelReport(string userType, int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            return _oracleHelper.ExecuteProcedure<CustomerHVFExcelReport>(PROC_GET_LIST_CUSTOMER_HVF_EXCEL_REPORT, new
            {
                pv_USER_TYPE = userType,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_END_DATE = endDate,
                pv_START_DATE = startDate
            })?.ToList();
        }

        /// <summary>
        /// Lấy danh sách thay đổi thông tin khách hàng
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CustomerInfoChangeExcelReport> GetListCustomerInfoChangeExcelReport(int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            return _oracleHelper.ExecuteProcedure<CustomerInfoChangeExcelReport>(PROC_GET_LIST_CUSTOMER_INFO_CHANGE_EXCEL_REPORT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_END_DATE = endDate,
                pv_START_DATE = startDate
            })?.ToList();
        }
    }
}

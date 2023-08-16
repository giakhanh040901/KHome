using AutoMapper.Execution;
using Dapper.Oracle;
//using EPIC.CoreEntities.Dto.Investor;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.IdentityEntities.DataEntities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class ManagerInvestorRepository
    {
        private OracleHelper _oracleHelper;
        private readonly IHttpContextAccessor _httpContext;

        #region UPDATE
        private const string PROC_UPDATE_TEMPORARY = "EPIC.PKG_MANAGER_INVESTOR.PROC_UPDATE_TEMPORARY";
        private const string PROC_UPLOAD_FACE_IMAGE = "EPIC.PKG_MANAGER_INVESTOR.PROC_UPLOAD_FACE_IMAGE";
        private const string PROC_UPLOAD_PROF_FILE = "EPIC.PKG_MANAGER_INVESTOR.PROC_UPLOAD_PROF_FILE";
        private const string PROC_RESET_PIN = "EPIC.PKG_MANAGER_INVESTOR.PROC_RESET_PIN";
        private const string PROC_UPLOAD_AVATAR = "EPIC.PKG_MANAGER_INVESTOR.PROC_UPLOAD_AVATAR";
        private const string PROC_DELETE_BANK_ACC = "EPIC.PKG_MANAGER_INVESTOR.PROC_DELETE_BANK_ACC";
        private const string PROC_UPDATE_BANK = "EPIC.PKG_MANAGER_INVESTOR.PROC_UPDATE_BANK";
        #endregion

        #region SET DEFAULT
        private const string PROC_SET_IDEN_DEFAULT = "EPIC.PKG_MANAGER_INVESTOR.PROC_SET_IDEN_DEFAULT";
        private const string PROC_SET_BANK_DEFAULT = "EPIC.PKG_MANAGER_INVESTOR.PROC_SET_BANK_DEFAULT";
        private const string PROC_SET_STOCK_DEFAULT = "EPIC.PKG_MANAGER_INVESTOR.PROC_SET_STOCK_DEFAULT";
        #endregion

        #region PROC TAO
        private const string PROC_CREATE_INVESTOR_TEMPORARY = "EPIC.PKG_MANAGER_INVESTOR.PROC_INVESTOR_REGISTER";
        private const string PROC_CREATE_IDENTIFICATION_TEMPORARY = "EPIC.PKG_MANAGER_INVESTOR.PROC_ADD_IDEN_TEMP";
        private const string PROC_UPDATE_IDEN = "EPIC.PKG_MANAGER_INVESTOR.PROC_UPDATE_IDEN";
        private const string PROC_CREATE_BANK_TEMPORARY = "EPIC.PKG_MANAGER_INVESTOR.PROC_CREATE_BANK_TEMPORARY";
        private const string PROC_CREATE_USER_BY_INVESTOR = "EPIC.PKG_MANAGER_INVESTOR.PROC_CREATE_USER_BY_INVESTOR";
        private const string PROC_ADD_STOCK = "EPIC.PKG_MANAGER_INVESTOR.PROC_ADD_STOCK";
        #endregion

        #region PROC GET LIST
        private const string PROC_LIST_TO_REQUEST = "EPIC.PKG_MANAGER_INVESTOR.PROC_LIST_TO_REQUEST";
        private const string PROC_GET_LIST_IDENTIFICATION = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_IDEN_BY_INVESTOR";
        private const string PROC_GET_BANK_BY_INVESTOR = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_BANK_BY_INVESTOR";
        private const string PROC_APP_GET_BANK_BY_INVESTOR = "EPIC.PKG_MANAGER_INVESTOR.PROC_APP_GET_BANK_BY_INVESTOR";
        private const string PROC_INVESTOR_FIND_ALL_LIST = "EPIC.PKG_MANAGER_INVESTOR.PROC_INVESTOR_FIND_ALL_LIST";
        private const string PROC_GET_LIST_INVESTOR = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_LIST_INVESTOR";
        private const string PROC_GET_LIST_USERS = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_LIST_USERS";
        private const string PROC_GET_BANK_PAGING = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_BANK_PAGING";
        private const string PROC_FILTER = "EPIC.PKG_MANAGER_INVESTOR.PROC_FILTER";
        private const string PROC_GET_PROF_FILE = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_PROF_FILE";
        private const string PROC_GET_STOCK_BY_INVESTOR = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_STOCK_BY_INVESTOR";
        #endregion

        #region GET ONE
        private const string PROC_GET_INVESTOR = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_INVESTOR";
        private const string PROC_GET_DEFAULT_IDENTIFICATION = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_DEFAULT_IDEN";
        private const string PROC_GET_LIST_DEFAULT_IDEN = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_LIST_DEFAULT_IDEN";
        private const string PROC_GET_DEFAULT_BANK = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_DEFAULT_BANK";
        private const string PROC_GET_IDENTIFICATION_BY_ID = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_IDEN_BY_ID";
        private const string PROC_GET_INVESTOR_BY_TEMP = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_ACTUAL_BY_TEMP";
        private const string PROC_GET_INVESTOR_BY_REFERRAL_CODE = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_INV_REFERRAL_CODE";
        private const string PROC_GET_BANK_BY_ID = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_BANK_BY_ID";
        private const string PROC_GET_CONTACT_ADDRESS_BY_ID = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_ADDRESS_BY_ID";
        private const string PROC_GET_INTRODUCE_USER = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_INTRODUCE_USER";
        private const string PROC_GET_INVESTOR_BY_USER_ID = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_INVESTOR_BY_USER_ID";
        private const string PROC_GET_INVESTOR_BY_GROUP_ID = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_INVESTOR_BY_GROUP_ID";
        private const string PROC_GET_BY_REF_CODE_SELF = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_BY_REF_CODE_SELF";
        private const string PROC_GET_DEFAULT_STOCK = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_DEFAULT_STOCK";
        private const string PROC_GET_INVESTOR_BY_STOCK = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_INVESTOR_BY_STOCK";
        private const string PROC_GET_STOCK_BY_ID = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_STOCK_BY_ID";
        private const string PROC_IS_PROF = "EPIC.PKG_MANAGER_INVESTOR.PROC_IS_PROF";
        #endregion

        #region PROC DUYET
        private const string PROC_APPROVE_UPDATE = "EPIC.PKG_MANAGER_INVESTOR.PROC_APPROVE_UPDATE";
        private const string PROC_APPROVE_ADD = "EPIC.PKG_MANAGER_INVESTOR.PROC_APPROVE_ADD";
        private const string PROC_APPROVE_PROF = "EPIC.PKG_MANAGER_INVESTOR.PROC_APPROVE_PROF";
        private const string PROC_REQUEST_EMAIL = "EPIC.PKG_MANAGER_INVESTOR.PROC_REQUEST_EMAIL";
        private const string PROC_APPROVE_EMAIL = "EPIC.PKG_MANAGER_INVESTOR.PROC_APPROVE_EMAIL";
        private const string PROC_REQUEST_PHONE = "EPIC.PKG_MANAGER_INVESTOR.PROC_REQUEST_PHONE";
        private const string PROC_APPROVE_PHONE = "EPIC.PKG_MANAGER_INVESTOR.PROC_APPROVE_PHONE";
        private const string PROC_CANCEL_INVESTOR = "EPIC.PKG_MANAGER_INVESTOR.PROC_CANCEL_INVESTOR";
        #endregion

        #region XAC MINH
        private const string PROC_EPIC_CHECK = "EPIC.PKG_MANAGER_INVESTOR.PROC_EPIC_CHECK";
        #endregion

        #region THAO TAC VOI USER
        private const string PROC_RESET_PASSWORD = "EPIC.PKG_MANAGER_INVESTOR.PROC_RESET_PASSWORD";
        private const string PROC_CHANGE_USER_STATUS = "EPIC.PKG_MANAGER_INVESTOR.PROC_CHANGE_USER_STATUS";
        private const string PROC_CHECK_PHONE_EMAIL_TEMP = "EPIC.PKG_MANAGER_INVESTOR.PROC_CHECK_PHONE_EMAIL_TEMP";
        private const string PROC_CHECK_PHONE_EMAIL = "EPIC.PKG_MANAGER_INVESTOR.PROC_CHECK_PHONE_EMAIL";
        #endregion

        #region CONTACT ADDRESS
        private const string PROC_ADD_CONTACT_ADDRESS = "EPIC.PKG_MANAGER_INVESTOR.PROC_ADD_CONTACT_ADDRESS";
        private const string PROC_SET_DEFAULT_ADDRESS = "EPIC.PKG_MANAGER_INVESTOR.PROC_SET_CONTACT_ADD_DEFAULT";
        private const string PROC_GET_LIST_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_GET_PAGING";
        private const string PROC_GET_DEFAULT_ADDRESS = "EPIC.PKG_MANAGER_INVESTOR.PROC_GET_DEFAULT_ADDRESS";
        
        #endregion

        #region NHA DAU TU CHUYEN NGHIEP
        private const string PROC_LIST_REQUEST_PROF = "EPIC.PKG_MANAGER_INVESTOR.PROC_LIST_REQUEST_PROF";
        private const string PROC_CANCEL_PROF = "EPIC.PKG_MANAGER_INVESTOR.PROC_CANCEL_PROF";
        #endregion

        public ManagerInvestorRepository(string connectionString, ILogger logger, IHttpContextAccessor httpContext)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _httpContext = httpContext;
        }

        /// <summary>
        /// Tạo giấy tờ temp
        /// </summary>
        /// <param name="dto"></param>
        public int CreateIdentificationTemporary(CreateIdentificationTemporaryDto dto, string username)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_CREATE_IDENTIFICATION_TEMPORARY, new
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
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                pv_OLD_IDEN_ID = (int?)null,
                SESSION_USERNAME = username
            }, false);

            return investorIdTemp;
        }

        /// <summary>
        /// Thay thế giấy tờ
        /// </summary>
        /// <param name="dto"></param>
        public int ReplaceIdentification(ReplaceIdentificationDto dto, string username)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_UPDATE_IDEN, new
            {
                pv_IDENTIFICATION_ID = dto.IdentificationId,
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
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username
            }, false);

            return investorIdTemp;
        }

        /// <summary>
        /// Cập nhật liên kết tk ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int UpdateInvestorBankAcc(UpdateInvestorBankAccDto dto, string username)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_UPDATE_BANK, new
            {
                pv_INVESTOR_BANK_ACC_ID = dto.InvestorBankAccId,
                pv_INVESTOR_ID = dto.InvestorId,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_BANK_ID = 0,
                pv_BANK_ACCOUNT = dto.BankAccount,
                pv_OWNER_ACCOUNT = dto.OwnerAccount,
                pv_IS_DEFAULT = "",
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username
            }, false);

            return investorIdTemp;
        }

        /// <summary>
        /// Tạo investor temp
        /// </summary>
        /// <param name="input"></param>
        public InvestorTemporary CreateInvestorTemporary(CreateManagerInvestorEkycDto input, string username, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorTemporary>(PROC_CREATE_INVESTOR_TEMPORARY, new
            {
                pv_ID_TYPE = input.IdType,
                pv_ID_NO = input.IdNo,
                pv_FULLNAME = input.Fullname,
                pv_ADDRESS = input.Address,
                pv_DATE_OF_BIRTH = input.DateOfBirth,
                pv_SEX = input.Sex,
                pv_NATIONALITY = input.Nationality,
                pv_PERSONAL_IDENTIFICATION = input.PersonalIdentification,
                pv_ID_ISSUER = input.IdIssuer,
                pv_ID_DATE = input.IdDate,
                pv_ID_EXPIRED_DATE = input.IdExpiredDate,
                pv_PLACE_OF_ORIGIN = input.PlaceOfOrigin,
                pv_PLACE_OF_RESIDENCE = input.PlaceOfResidence,
                pv_ID_FRONT_IMAGE_URL = input.IdFrontImageUrl,
                pv_ID_BACK_IMAGE_URL = input.IdBackImageUrl,
                pv_ID_EXTRA_IMAGE_URL = input.IdExtraImageUrl,
                pv_FACE_IMAGE_URL = input.FaceImageUrl,
                pv_FACE_VIDEO_URL = input.FaceVideoUrl,
                pv_BANK_ID = input.BankId,
                pv_BANK_ACCOUNT = input.BankAccount,
                pv_OWNER_ACCOUNT = input.OwnerAccount,
                pv_EMAIL = input.Email,
                pv_PHONE = input.Phone,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SECURITY_COMPANY = input.SecurityCompany,
                pv_STOCK_TRADING_ACCOUNT = input.StockTradingAccount,
                pv_REPRESENTATIVE_PHONE = input.RepresentativePhone,
                pv_REPRESENTATIVE_EMAIL = input.RepresentativeEmail,
                pv_REFERRAL_CODE = input.ReferralCode,
                SESSION_USERNAME = username,
            });
        }

        /// <summary>
        /// Tạo Thông tin bank của nhà đầu tư
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public int CreateInvestorBankTemporary(CreateInvestorBankTempDto dto, string username)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_CREATE_BANK_TEMPORARY, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_BANK_ID = dto.BankId,
                pv_BANK_ACCOUNT = dto.BankAccount,
                pv_OWNER_ACCOUNT = dto.OwnerAccount,
                pv_IS_DEFAULT = dto.IsDefault,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username
            });

            return investorIdTemp;
        }

        /// <summary>
        /// Tạo user by investor
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void CreateUser(CreateUserByInvestorDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CREATE_USER_BY_INVESTOR, new
            {
                pv_USERNAME = dto.UserName,
                pv_PASSWORD = dto.Password,
                pv_INVESTOR_ID = dto.InvestorId
            });
        }

        /// <summary>
        /// Cập nhật investor
        /// </summary>
        /// <param name="dto"></param>
        public int UpdateInvestor(UpdateManagerInvestorDto dto)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var data = new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_NAME_EN = "",
                pv_SHORT_NAME = "",
                pv_BORI = dto.Bori,
                pv_DORF = dto.Dorf,
                pv_EDU_LEVEL = dto.EduLevel,
                pv_OCCUPATION = dto.Occupation,
                pv_ADDRESS = dto.Address,
                pv_CONTACT_ADDRESS = dto.ContactAddress,
                pv_PHONE = dto.Phone,
                pv_FAX = dto.Fax,
                pv_MOBILE = dto.Mobile,
                pv_EMAIL = dto.Email,
                pv_TAX_CODE = dto.TaxCode,
                pv_REFERRAL_CODE_SELF = dto.ReferralCodeSelf,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,

                // default identification
                pv_NAME = dto.DefaultIdentification.Fullname,
                pv_PLACE_OF_ORIGIN = dto.DefaultIdentification.PlaceOfOrigin,
                pv_PLACE_OF_RESIDENCE = dto.DefaultIdentification.PlaceOfResidence,
                pv_ID_NO = dto.DefaultIdentification.IdNo,
                pv_ID_DATE = dto.DefaultIdentification.IdDate,
                pv_ID_EXPIRED_DATE = dto.DefaultIdentification.IdExpiredDate,
                pv_ID_ISSUER = dto.DefaultIdentification.IdIssuer,
                pv_NATIONALITY = dto.DefaultIdentification.Nationality,
                pv_SEX = dto.DefaultIdentification.Sex,
                pv_BIRTH_DATE = dto.DefaultIdentification.DateOfBirth,

                //default bank
                pv_BANK_ID = dto.DefaultBank.BankId,
                pv_BANK_ACCOUNT = dto.DefaultBank.BankAccount,
                pv_OWNER_ACCOUNT = dto.DefaultBank.OwnerAccount,

                // cty chứng khoán
                pv_SECURITY_COMPANY = dto.SecurityCompany,
                pv_STOCK_TRADING_ACCOUNT = dto.StockTradingAccount,

                // người đại diện
                pv_REPRESENTATIVE_PHONE = dto.RepresentativePhone,
                pv_REPRESENTATIVE_EMAIL = dto.RepresentativeEmail,
                //temp
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username
            };

            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_UPDATE_TEMPORARY, data);

            return investorIdTemp;
        }

        /// <summary>
        /// Lấy ra danh sách Investor từ bảng Temp để trình duyệt hoặc cập nhật
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public PagingResult<InvestorTemporary> GetListToRequest(FindListInvestorDto dto, string usertype)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvestorTemporary>(PROC_LIST_TO_REQUEST, new
            {
                PAGE_SIZE = dto.PageSize,
                PAGE_NUMBER = dto.PageNumber,
                KEYWORD = dto.Keyword,
                pv_TRADING_PROVIDER_ID = dto.TradingProviderId,
                pv_STATUS = dto.Status,
                pv_CIF_CODE = dto.CifCode,
                pv_SEX = dto.Sex,
                pv_DATE_OF_BIRTH = dto.DateOfBirth,
                pv_FULLNAME = dto.Fullname,
                pv_NATIONALITY = dto.Nationality,
                pv_PHONE = dto.Phone,
                pv_EMAIL = dto.Email,
                pv_REPRESENTATIVE_PHONE = dto.RepresentativePhone,
                pv_REPRESENTATIVE_EMAIL = dto.RepresentativeEmail,
                pv_ID_NO = dto.IdNo,
                pv_USER_TYPE = usertype,
            });

            return result;
        }

        /// <summary>
        /// Lấy list investor thật
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public PagingResult<Investor> GetListInvestor(FindListInvestorDto dto, string usertype)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<Investor>(PROC_GET_LIST_INVESTOR, new
            {
                PAGE_SIZE = dto.PageSize,
                PAGE_NUMBER = dto.PageNumber,
                KEYWORD = dto.Keyword,
                pv_TRADING_PROVIDER_ID = dto.TradingProviderId,
                pv_STATUS = dto.Status,
                pv_CIF_CODE = dto.CifCode,
                pv_SEX = dto.Sex,
                pv_DATE_OF_BIRTH = dto.DateOfBirth,
                pv_FULLNAME = dto.Fullname,
                pv_NATIONALITY = dto.Nationality,
                pv_PHONE = dto.Phone,
                pv_EMAIL = dto.Email,
                pv_REPRESENTATIVE_PHONE = dto.RepresentativePhone,
                pv_REPRESENTATIVE_EMAIL = dto.RepresentativeEmail,
                pv_ID_NO = dto.IdNo,
                pv_IS_CHECK = dto.IsCheck,
                pv_USER_TYPE = usertype,
            });

            return result;
        }

        /// <summary>
        /// Filter khách hàng cá nhân theo điều kiện | Không dùng cho việc hiển thị ở màn hình danh sách khcn
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<Investor> FilterInvestor(FilterManagerInvestorDto dto)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<Investor>(PROC_FILTER, new
            {
                PAGE_SIZE = dto.PageSize,
                PAGE_NUMBER = dto.PageNumber,
                KEYWORD = dto.Keyword,
                pv_REPRESENTATIVE_PHONE = dto.RepresentativePhone,
                pv_REPRESENTATIVE_EMAIL = dto.RepresentativeEmail,
                pv_REQUIRE_KEYWORD = dto.RequireKeyword ? YesNo.YES : YesNo.NO
            });

            return result;
        }

        /// <summary>
        /// Get list user của investor
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public PagingResult<ViewUserDto> GetListUsers(int pageSize, int pageNumber, string keyword, int investorId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ViewUserDto>(PROC_GET_LIST_USERS, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEYWORD = keyword,
                pv_INVESTOR_ID = investorId,
            });
            return result;
        }

        /// <summary>
        /// Lấy ra danh sách identification cần duyệt (Trạng thái Nháp và Trình duyệt)
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="investorGroupId"></param>
        /// <returns></returns>
        public IEnumerable<InvestorIdentification> GetListIdentification(int investorId, int? investorGroupId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedure<InvestorIdentification>(PROC_GET_LIST_IDENTIFICATION, new
            {
                pv_INVESTOR_ID = investorId,
                pv_INVESTOR_GROUP_ID = investorGroupId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }

        /// <summary>
        /// Get ds ngân hàng phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public PagingResult<InvestorBankAccount> GetListBankPaging(int pageSize, int pageNumber, string keyword, int investorId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedurePaging<InvestorBankAccount>(PROC_GET_BANK_PAGING, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEYWORD = keyword,
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }

        /// <summary>
        /// Lấy investor thật dựa vào investor id temp
        /// </summary>
        /// <param name="investorIdTemp"></param>
        /// <returns></returns>
        public Investor GetActualInvestorByTemp(int investorIdTemp)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_GET_INVESTOR_BY_TEMP, new
            {
                pv_INVESTOR_ID_TEMP = investorIdTemp
            });
        }

        /// <summary>
        /// Kiểm tra xem có phải nhà đầu tư chuyên nghiệp ko
        /// 1: Không chuyên; 2: Đang chờ duyệt; 3: Đã là nđt chuyên nghiệp
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public int GetIsProf(int investorId)
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_INVESTOR_ID", investorId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_IS_PROF, parameters);
            int result = (int)parameters.Get<decimal>("pv_RESULT");
            return result;
        }

        /// <summary>
        /// Duyet them investor
        /// </summary>
        /// <param name="dto"></param>
        public Investor ApproveAddInvestor(ApproveManagerInvestorDto dto, string password)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_APPROVE_ADD, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_PASSWORD = password,
                SESSION_USERNAME = username,
            });
        }

        /// <summary>
        /// Duyệt cập nhật investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Investor ApproveUpdateInvestor(ApproveManagerInvestorDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_APPROVE_UPDATE, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                SESSION_USERNAME = username,
            });
        }

        /// <summary>
        /// Huỷ duyệt investor
        /// </summary>
        /// <param name="dto"></param>
        public void CancelRequestInvestor(CancelRequestManagerInvestorDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CANCEL_INVESTOR, new
            {
                pv_INVESTOR_TEMP_ID = dto.InvestorIdTemp,
                pv_EKYC_INCORRECT_FIELDS = string.Join(',', dto.IncorrectFields),
            }, false);
        }

        /// <summary>
        /// Lấy list bank by investor
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="investorGroupId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public IEnumerable<InvestorBankAccount> GetListBank(int investorId, int? investorGroupId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedure<InvestorBankAccount>(PROC_GET_BANK_BY_INVESTOR, new
            {
                pv_INVESTOR_ID = investorId,
                pv_INVESTOR_GROUP_ID = investorGroupId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }

        /// <summary>
        /// GET ONE INVESTOR từ bảng thật hoặc temp
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="investorGroupId"></param>
        /// <returns></returns>
        public InvestorTemporary FindById(int investorId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorTemporary>(PROC_GET_INVESTOR, new
            {
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }

        /// <summary>
        /// Lấy investor thật theo group id
        /// </summary>
        /// <param name="investorGroupId"></param>
        /// <returns></returns>
        public InvestorTemporary FindByInvestorGroupId(int investorGroupId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorTemporary>(PROC_GET_INVESTOR_BY_GROUP_ID, new
            {
                pv_INVESTOR_GROUP_ID = investorGroupId
            });
        }

        /// <summary>
        /// Lấy investor thật theo mã giới thiệu của investor đó
        /// </summary>
        /// <param name="referralCodeSelf"></param>
        /// <returns></returns>
        public InvestorTemporary FindByReferralCodeSelf(string referralCodeSelf, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorTemporary>(PROC_GET_BY_REF_CODE_SELF, new
            {
                pv_REFERRAL_CODE_SELF = referralCodeSelf,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        /// <summary>
        /// Upload ảnh mặt investor, cập nhật luôn Đã xác minh mặt với Giấy tờ tùy thân mặc định
        /// </summary>
        /// <param name="dto"></param>
        public int UploadFaceImage(UploadFaceImageDto dto, string faceImageUrl, bool isTemp)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_UPLOAD_FACE_IMAGE, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_FACE_IMAGE_URL = faceImageUrl,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_IS_TEMP = getIsTemp(isTemp),
                SESSION_USERNAME = username
            });
            return investorIdTemp;
        }

        /// <summary>
        /// Upload file nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="profFileUrl"></param>
        /// <param name="username"></param>
        public InvestorTemporary UploadProfFile(int investorId, string username, List<string> profFileUrl, List<string> profFileType, List<string> profFileName)
        {

            OracleDynamicParameters parameters = new();
            parameters.Add("pv_INVESTOR_ID", dbType: OracleMappingType.Int32, direction: ParameterDirection.Input, value: investorId);
            parameters.Add("pv_PROF_FILE_URL", collectionType: OracleMappingCollectionType.PLSQLAssociativeArray, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input, value: profFileUrl.ToArray());
            parameters.Add("pv_PROF_FILE_TYPE", collectionType: OracleMappingCollectionType.PLSQLAssociativeArray, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input, value: profFileType.ToArray());
            parameters.Add("pv_PROF_FILE_NAME", collectionType: OracleMappingCollectionType.PLSQLAssociativeArray, dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input, value: profFileName.ToArray());
            parameters.Add("SESSION_USERNAME", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Input, value: username);

            var result = _oracleHelper.ExecuteProcedureDynamicParamsToFirst<InvestorTemporary>(PROC_UPLOAD_PROF_FILE, parameters);

            return result;
        }

        /// <summary>
        /// Reset mật khẩu của user
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void ResetUserPassword(ResetUserPasswordManagerInvestorDto dto, string password, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_RESET_PASSWORD, new
            {
                pv_USER_ID = dto.UserId,
                pv_INVESTOR_ID = dto.InvestorId,
                pv_NEW_PASSWORD = password,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// Reset pin của investor
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="pin"></param>
        /// <param name="username"></param>
        public int ResetPin(ResetPinDto dto, string pin, string username)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_RESET_PIN, new
            {
                pv_USER_ID = dto.UserId,
                pv_NEW_PIN = pin,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// Active/deactive user
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void ChangeUserStatus(ChangeUserStatusDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CHANGE_USER_STATUS, new
            {
                pv_USER_ID = dto.UserId,
                pv_INVESTOR_ID = dto.InvestorId,
                pv_STATUS = dto.Status,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// EPIC xác minh
        /// </summary>
        /// <param name="investorId"></param>
        public void EpicCheck(int investorId, decimal approveId, int userCheckId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_EPIC_CHECK, new
            {
                pv_INVESTOR_ID = investorId,
                pv_APPROVE_ID = approveId,
                pv_USER_CHECK_ID = userCheckId,
            });
        }

        /// <summary>
        /// Chọn giấy tờ mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void SetDefaultIdentification(SetDefaultIdentificationDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SET_IDEN_DEFAULT, new
            {
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                pv_IDENTIFICATION_ID = dto.IdentificationId,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// Chon bank mac dinh
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void SetDefaultBank(SetDefaultBankDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SET_BANK_DEFAULT, new
            {
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                pv_INVESTOR_BANK_ID = dto.InvestorBankId,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// Lấy giấy tờ mặc định
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="isTemp">lây ở bảng thật hay bảng tạm, nếu true là bảng tạm</param>
        /// <returns></returns>
        public InvestorIdentification GetDefaultIdentification(int investorId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorIdentification>(PROC_GET_DEFAULT_IDENTIFICATION, new
            {
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }

        /// <summary>
        /// Lấy địa chỉ mặc định
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public InvestorContactAddress GetDefaultContactAddress(int investorId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorContactAddress>(PROC_GET_DEFAULT_ADDRESS, new
            {
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }

        /// <summary>
        /// Lấy danh sách giấy tờ mặc định theo list investor id
        /// </summary>
        /// <param name="investorIds"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public List<InvestorIdentification> GetListDefaultIdentification(IEnumerable<string> investorIds, bool isTemp)
        {

            var joinedInvestorIds = string.Join(',', investorIds);

            var data = _oracleHelper.ExecuteProcedure<InvestorIdentification>(PROC_GET_LIST_DEFAULT_IDEN, new
            {
                pv_INVESTOR_IDS = joinedInvestorIds,
                pv_IS_TEMP = getIsTemp(isTemp),
            });

            return data?.ToList();
        }

        #region contact address
        /// <summary>
        /// Lấy địa chỉ mặc định
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public InvestorContactAddress GetContactdAddressById(int contactAddressId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorContactAddress>(PROC_GET_CONTACT_ADDRESS_BY_ID, new
            {
                pv_ID = contactAddressId
            });
        }

        /// <summary>
        /// List contact address
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public PagingResult<InvestorContactAddress> GetListContactAddress(int? pageSize, int? pageNumber, string keyword, int investorId, bool isTemp)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<InvestorContactAddress>(PROC_GET_LIST_CONTACT_ADDRESS, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEYWORD = keyword,
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });

            return result;
        }

        /// <summary>
        /// List contact address no paging
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public IEnumerable<InvestorContactAddress> GetListContactAddressNoPaging(int investorId, bool isTemp)
        {
            string keyword = null;
            var result = _oracleHelper.ExecuteProcedurePaging<InvestorContactAddress>(PROC_GET_LIST_CONTACT_ADDRESS, new
            {
                PAGE_SIZE = -1,
                PAGE_NUMBER = 0,
                KEYWORD = keyword,
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });

            return result?.Items;
        }

        /// <summary>
        /// Tạo contact address thật
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public int CreateContactAddress(CreateManagerInvestorContactAddressDto dto, string username)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_ADD_CONTACT_ADDRESS, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_CONTACT_ADDRESS = dto.ContactAddress,
                pv_IS_DEFAULT = dto.IsDefault,
                pv_DETAIL_ADDRESS = dto.DetailAddress,
                pv_PROVINCE_CODE = dto.ProvinceCode,
                pv_DISTRICT_CODE = dto.DistrictCode,
                pv_WARD_CODE = dto.WardCode,
                pv_IS_TEMP = getIsTemp(dto.isTemp),
                SESSION_USERNAME = username
            });

            return investorIdTemp;
        }

        /// <summary>
        /// Cập nhật contact address thật
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void UpdateContactAddress(UpdateContactAddressDto dto, string username)
        {
            //_oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_CONTACT_ADDRESS, new
            //{
            //    pv_INVESTOR_ID = dto.InvestorId,
            //    pv_CONTACT_ADDRESS_ID = dto.ContactAddressId,
            //    pv_CONTACT_ADDRESS = dto.ContactAddress,
            //    SESSION_USERNAME = username
            //});
        }

        /// <summary>
        /// Chọn địa chỉ mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void SetDefaultContactAddress(SetDefaultManagerInvestorContactAddressDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SET_DEFAULT_ADDRESS, new
            {
                pv_CONTACT_ADDRESS_ID = dto.ContactAddressId,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username
            });
        }
        #endregion

        public List<InvestorDto> FindAllList(string keyword)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _oracleHelper.ExecuteProcedure<InvestorDto>(PROC_INVESTOR_FIND_ALL_LIST, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                KEYWORD = keyword
            }).ToList();
        }

        /// <summary>
        /// Check xem investor temp đã có email và phone chưa
        /// </summary>
        /// <param name="investorId"></param>
        public void CheckPhoneAndEmailTemp(int investorId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CHECK_PHONE_EMAIL_TEMP, new
            {
                pv_INVESTOR_ID = investorId,
            });
        }
        
        public IEnumerable<InvestorBankAccount> AppGetListBankByInvestor(int investorId)
        {
            return _oracleHelper.ExecuteProcedure<InvestorBankAccount>(PROC_APP_GET_BANK_BY_INVESTOR, new
            {
                pv_INVESTOR_ID = investorId,
            });
        }

        public InvestorBankAccount GetBankById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorBankAccount>(PROC_GET_BANK_BY_ID, new
            {
                pv_ID = id
            });
        }

        /// <summary>
        /// Lấy tài khoản thụ hưởng mặc định | investor thật
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public InvestorBankAccount GetDefaultBank(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorBankAccount>(PROC_GET_DEFAULT_BANK, new
            {
                pv_INVESTOR_ID = investorId,
            });
        }

        public InvestorIdentification GetIdentificationById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorIdentification>(PROC_GET_IDENTIFICATION_BY_ID, new
            {
                pv_ID = id
            });
        }


        /// <summary>
        /// Check xem investor đã có email và phone chưa
        /// </summary>
        /// <param name="investorId"></param>
        public void CheckPhoneAndEmail(int investorId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CHECK_PHONE_EMAIL, new
            {
                pv_INVESTOR_ID = investorId,
            });
        }

        /// <summary>
        /// Huỷ yêu cầu trình duyệt nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        public void CancelRequestProf(CancelRequestInvestorProfDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_CANCEL_PROF, new
            {
                pv_INVESTOR_TEMP_ID = dto.InvestorTempId
            });
        }

        /// <summary>
        /// Duyệt file nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void ApproveProf(ApproveProfDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APPROVE_PROF, new
            {
                pv_INVESTOR_ID_TEMP = dto.InvestorIdTemp,
                pv_INVESTOR_ID = dto.InvestorId,
                pv_PROF_START_DATE = dto.ProfStartDate,
                pv_PROF_DUE_DATE = dto.ProfDueDate,
                SESSION_USERNAME = username
            }, false);
        }

        /// <summary>
        /// Lấy thông tin người giới thiệu
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<Investor> GetIntroduceUsers(int investorId)
        {
            var data = _oracleHelper.ExecuteProcedure<Investor>(PROC_GET_INTRODUCE_USER, new
            {
                pv_INVESTOR_ID = investorId
            });
            return data?.ToList();
        }

        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="filePath"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int UploadAvatar(UploadAvatarDto dto, string filePath, string username)
        {
            return _oracleHelper.ExecuteProcedureToFirst<int>(PROC_UPLOAD_AVATAR, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_AVATAR_IMAGE_URL = filePath,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username,
            });
        }

        /// <summary>
        /// Lấy invesstor theo user id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Investor GetInvestorByUserId(int userid)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_GET_INVESTOR_BY_USER_ID, new
            {
                pv_USER_ID = userid,
            });
        } 
        
        
        /// <summary>
        /// Lấy invesstor theo referral Code
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public Investor GetInvestorByRefferalCode(string referralCode)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Investor>(PROC_GET_INVESTOR_BY_REFERRAL_CODE, new
            {
                pv_REFERRAL_CODE = referralCode
            });
        }

        /// <summary>
        /// Lấy list file nđt cn theo investorTempId
        /// </summary>
        /// <param name="investorTempId"></param>
        /// <returns></returns>
        public List<InvestorProfFile> GetProfFile(int investorTempId)
        {
            var result = _oracleHelper.ExecuteProcedure<InvestorProfFile>(PROC_GET_PROF_FILE, new
            {
                pv_INVESTOR_TEMP_ID = investorTempId
            });

            return result?.ToList();
        }

        /// <summary>
        /// Tạo yêu cầu duyệt email
        /// </summary>
        /// <param name="dto"></param>
        public InvestorTemporary CreateRequestEmail(CreateRequestEmailDto dto, string username)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorTemporary>(PROC_REQUEST_EMAIL, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_EMAIL = dto.Email,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username
            }, false);
        }

        /// <summary>
        /// Tạo yêu cầu duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        public InvestorTemporary CreateRequestPhone(CreateRequestPhoneDto dto, string username)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorTemporary>(PROC_REQUEST_PHONE, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_PHONE = dto.Phone,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username
            }, false);
        }

        /// <summary>
        /// Duyệt email
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void ApproveEmail(ApproveEmailDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APPROVE_EMAIL, new
            {
                pv_INVESTOR_ID_TEMP = dto.InvestorIdTemp,
                SESSION_USERNAME = username,
            });
        }

        /// <summary>
        /// Duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void ApprovePhone(ApprovePhoneDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APPROVE_PHONE, new
            {
                pv_INVESTOR_ID_TEMP = dto.InvestorIdTemp,
                SESSION_USERNAME = username,
            });
        }

        /// <summary>
        /// Xoá tài khoản ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public int DeleteBankAccount(DeleteBankAccountDto dto, string username)
        {
            return _oracleHelper.ExecuteProcedureToFirst<int>(PROC_DELETE_BANK_ACC, new
            {
                pv_INVESTOR_BANK_ACC_ID = dto.InvestorBankAccId,
                pv_INVESTOR_ID = dto.InvestorId,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username,
            });
        }

        #region công ty chứng khoán
        /// <summary>
        /// Lấy danh sách cty chứng khoán
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<InvestorStock> GetListStockNoPaging(FindInvestorStockDto dto)
        {
            var result = _oracleHelper.ExecuteProcedure<InvestorStock>(PROC_GET_STOCK_BY_INVESTOR, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
            });

            return result;
        }

        /// <summary>
        /// Tạo thêm cty chứng khoán
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CreateInvestorStock(CreateInvestorStockDto dto, string username)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_ADD_STOCK, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_INVESTOR_GROUP_ID = dto.InvestorGroupId,
                pv_SECURITY_COMPANY = dto.SecurityCompany,
                pv_STOCK_TRADING_ACCOUNT = dto.StockTradingAccount,
                pv_IS_DEFAULT = dto.IsDefault,
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                SESSION_USERNAME = username,
            });

            return investorIdTemp;
        }

        public InvestorStock GetInvestorByStockTradingAccount(int securityCompany, string stockTradingAccount)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorStock>(PROC_GET_INVESTOR_BY_STOCK, new
            {
                pv_SECURITY_COMPANY = securityCompany,
                pv_STOCK_TRADING_ACCOUNT = stockTradingAccount,
            });
        }

        /// <summary>
        /// Chọn địa chỉ mặc định
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void SetDefaultStock(SetDefaultInvestorStockDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SET_STOCK_DEFAULT, new
            {
                pv_IS_TEMP = getIsTemp(dto.IsTemp),
                pv_STOCK_ID = dto.Id,
                SESSION_USERNAME = username
            });
        }

        /// <summary>
        /// Lấy công ty chứng khoán mặc định
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public InvestorStock GetDefaultStock(int investorId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorStock>(PROC_GET_DEFAULT_STOCK, new
            {
                pv_INVESTOR_ID = investorId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }

        /// <summary>
        /// Lấy thông tin chứng khoán theo id chứng khoán
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public InvestorStock GetStockByStockId(int stockId, bool isTemp)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvestorStock>(PROC_GET_STOCK_BY_ID, new
            {
                pv_STOCK_ID = stockId,
                pv_IS_TEMP = getIsTemp(isTemp),
            });
        }
        #endregion

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        private int getIsTemp(bool isTemp)
        {
            return isTemp ? 1 : 0;
        }
    }
}

using Dapper.Oracle;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.DataAccess;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.SaleInvestor;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class SaleInvestorRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_APP_INVESTOR_REGISTER = "PKG_SALE_INVESTOR.PROC_APP_INVESTOR_REGISTER";
        private const string PROC_APP_UPDATE_EKYC_ID = "PKG_SALE_INVESTOR.PROC_APP_UPDATE_EKYC_ID";
        private const string PROC_APP_CONFIRM_UPDATE = "PKG_SALE_INVESTOR.PROC_APP_CONFIRM_UPDATE";
        private const string PROC_APP_UPLOAD_AVATAR = "PKG_SALE_INVESTOR.PROC_APP_UPLOAD_AVATAR";
        private const string PROC_APP_ADD_ADDRESS = "PKG_SALE_INVESTOR.PROC_APP_ADD_ADDRESS";
        private const string PROC_APP_ADD_BANK = "PKG_SALE_INVESTOR.PROC_APP_ADD_BANK";
        private const string PROC_GET_LIST_INVESTOR_BY_SALE = "PKG_SALE_INVESTOR.PROC_GET_LIST_INVESTOR_BY_SALE";
        private const string PROC_IS_INVESTOR_OF_SALE = "PKG_SALE_INVESTOR.PROC_IS_INVESTOR_OF_SALE";

        public SaleInvestorRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        /// <summary>
        /// Đăng ký investor
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="investorId"></param>
        /// <param name="password"></param>
        public ResultAddInvestorDto RegisterInvestor(SaleRegisterInvestorDto dto, int investorId, string password, string username)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ResultAddInvestorDto>(PROC_APP_INVESTOR_REGISTER, new
            {
                pv_PHONE = dto.Phone,
                pv_EMAIL = dto.Email,
                pv_PASSWORD = password,
                pv_INVESTOR_ID = investorId,
                pv_REFERRAL_CODE = dto.ReferralCode,
                SESSION_USERNAME = username
            });

            return result;
        }

        /// <summary>
        /// Thêm giấy tờ
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CreateIdentification(CreateIdentificationTemporaryDto dto)
        {
            var investorIdTemp = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_APP_UPDATE_EKYC_ID, new
            {
                pv_NAME = dto.Fullname,
                pv_BIRTH_DATE = dto.DateOfBirth,
                pv_SEX = dto.Sex,
                pv_ID_NO = dto.IdNo,
                pv_ID_DATE = dto.IdDate,
                pv_ID_EXPIRED_DATE = dto.IdExpiredDate,
                pv_ID_ISSUER = dto.IdIssuer,
                pv_PLACE_OF_ORIGIN = dto.PlaceOfOrigin,
                pv_PLACE_OF_RESIDENCE = dto.PlaceOfResidence,
                pv_NATIONALITY = dto.Nationality,
                pv_ID_TYPE = dto.IdType,
                pv_ID_FRONT_IMAGE_URL = dto.IdFrontImageUrl,
                pv_ID_BACK_IMAGE_URL = dto.IdBackImageUrl,
                pv_INVESTOR_ID = dto.InvestorId
            });

            return investorIdTemp;
        }

        /// <summary>
        /// Xác nhận thông tin chính xác và Cập nhật thông tin giấy tờ
        /// </summary>
        /// <param name="dto"></param>
        public void ConfirmAndUpdateEkyc(SaleInvestorConfirmUpdateDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_CONFIRM_UPDATE, new
            {
                pv_NAME = dto.Name,
                pv_BIRTH_DATE = dto.BirthDate,
                pv_SEX = dto.Sex,
                pv_ID_NO = dto.IdNo,
                pv_ID_DATE = dto.IdDate,
                pv_ID_EXPIRED_DATE = dto.IdExpiredDate,
                pv_ID_ISSUER = dto.IdIssuer,
                pv_PLACE_OF_ORIGIN = dto.PlaceOfOrigin,
                pv_PLACE_OF_RESIDENCE = dto.PlaceOfResidence,
                pv_NATIONALITY = dto.Nationality,
                pv_INVESTOR_ID = dto.InvestorId
            });
        }

        /// <summary>
        /// Upload avatar
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="avatarImageUrl"></param>
        public void UploadAvatar(int investorId, string avatarImageUrl)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_UPLOAD_AVATAR, new
            {
                pv_INVESTOR_ID = investorId,
                pv_AVATAR_IMAGE_URL = avatarImageUrl
            });
        }

        /// <summary>
        /// Thêm địa chỉ liên lạc
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void AddContactAddress(CreateContactAddressDto dto, string username)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_ADD_ADDRESS, new
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
        /// Thêm bank
        /// </summary>
        /// <param name="dto"></param>
        public void AddBank(CreateBankDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_ADD_BANK, new
            {
                pv_INVESTOR_ID = dto.InvestorId,
                pv_BANK_ID = dto.BankId,
                pv_BANK_ACCOUNT = dto.BankAccount,
                pv_OWNER_ACCOUNT = dto.OwnerAccount
            });
        }

        /// <summary>
        /// Lấy danh sách investor theo sale
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public List<ViewInvestorsBySaleDto> GetListInvestorBySale(GetInvestorBySaleDto dto, int saleId)
        {
            var data = _oracleHelper.ExecuteProcedure<ViewInvestorsBySaleDto>(PROC_GET_LIST_INVESTOR_BY_SALE, new
            {
                pv_SALE_ID = saleId,
                pv_STATUS = dto.Status,
            });

            return data?.ToList();
        }

        /// <summary>
        /// Check xem investor có phải của sale ko
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public bool IsInvestorOfSale(int investorId, int saleId)
        {
            string result = "N";
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_SALE_ID", saleId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_INVESTOR_ID", investorId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_IS_INVESTOR_OF_SALE, parameters);

            result = parameters.Get<string>("pv_RESULT");
            return result == "Y";
        }
    }
}

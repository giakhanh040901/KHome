using EPIC.BondRepositories;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.DigitalSign;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EPIC.CoreRepositories
{
    public class BusinessCustomerRepository : BaseRepository
    {
        #region Doanh nghiệp 
        private const string PROC_BUSINESS_CUSTOMER_TEMP_ADD = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUSTOMER_ADD";
        private const string PROC_BUSINESS_CUSTOMER_TEMP_UPDATE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUS_CUS_APPROVE_UPDATE";
        private const string PROC_BUSINESS_CUSTOMER_UPDATE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUSTOMER_UPDATE";
        private const string PROC_BUSINESS_CUSTOMER_TEMP_CANCEL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUS_CUS_PARTNER_CANCEL";
        private const string PROC_BUSINESS_CUSTOMER_TEMP_GET_ALL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_TEMP_GET_ALL";
        private const string PROC_BUSINESS_CUSTOMER_TEMP_GET = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_TEMP_GET";
        #endregion

        #region Quy trình duyệt doanh nghiệp
        private const string PROC_BUSINESS_CUSTOMER_APPROVE_ADD = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_APPROVE_ADD";
        private const string PROC_BUSINESS_CUSTOMER_APPROVE_UPDATE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_APPROVE_UPDATE";
        private const string PROC_BUSINESS_CUSTOMER_CHECK = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUSTOMER_CHECK";
        private const string PROC_BUSINESS_CUSTOMER_CANCEL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUSTOMER_CANCEL";
        private const string PROC_BUSINESS_CUSTOMER_TEMP_REQUEST = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_TEMP_REQUEST";
        #endregion

        #region Get
        private const string PROC_BUSINESS_CUS_CMS_GET_ALL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSNESS_CUS_CMS_GET_ALL";
        private const string PROC_BUSINESS_CUSTOMER_GET_ALL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUSTOMER_GET_ALL";
        private const string PROC_BUSINESS_CUSTOMER_GET_ALL_LIST = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_GET_ALL_LIST";
        private const string PROC_BUSINESS_CUSTOMER_GET = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUSTOMER_GET";
        private const string PROC_BUSINESS_CUS_GET = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_GET";
        private const string PROC_BUSINESS_CUS_DEL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUSTOMER_DEL";
        #endregion

        #region Bank
        private const string PROC_BUSINESS_CUSTOMER_BANK_GET_ALL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_BANK_GET_ALL";
        private const string PROC_BUSINESS_CUSTOMER_BANK_GET = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_BANK_GET";
        private const string PROC_BUSINESS_CUSTOMER_BANK_GET_DEFAULT = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BANK_GET_DEFAULT";
        private const string PROC_BUSINESS_CUSTOMER_BANK_GET_BY_BUSI_CUS_ID = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSI_CUS_BANK_GET_BY_CUS";
        private const string PROC_BUSINESS_CUSTOMER_BANK_ACTIVE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_BANK_ACTIVE";
        private const string PROC_BUSINESS_CUSTOMER_FIND_TAX_CODE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_B_C_FIND_TAX_CODE";
        private const string PROC_BUSINESS_CUSTOMER_BANK_GET_ALL_NO_PAGE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_BANK_GET_ALL_NO_PAGE";

        /// <summary>
        /// Khi ở bảng đã duyệt, Update tạo bản ghi ở bảng BankTemp
        /// </summary>
        private const string PROC_BUSINESS_CUS_BANK_UPDATE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_BANK_UPDATE";
        private const string PROC_BUSINESS_CUS_BANK_DEL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_BANK_DEL";
        /// <summary>
        /// Đặt làm ngân hàng mặc định
        /// </summary>
        private const string PROC_BANK_SET_DEFAULT = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BANK_SET_DEFAULT";
        private const string PROC_BANK_TEMP_SET_DEFAULT = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BANK_TEMP_SET_DEFAULT";

        #endregion

        #region Bank Temp
        /// <summary>
        /// Thêm mới ngân hàng - được lưu ở bảng tạm
        /// </summary>
        private const string PROC_BUSINESS_CUSTOMER_BANK_ADD = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_CUS_BANK_ADD";
        private const string PROC_BUS_CUS_BANK_UPDATE_TEMP = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUS_CUS_BANK_UPDATE_TEMP";
        private const string PROC_BUSINESS_BANK_REQUEST = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_BANK_REQUEST";
        private const string PROC_APPROVE_BANK_ADD = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_APPROVE_BANK_ADD";
        private const string PROC_APPROVE_BANK_UPDATE = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_APPROVE_BANK_UPDATE";
        private const string PROC_BUSINESS_BANK_CANCEL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUSINESS_BANK_CANCEL";

        private const string PROC_BANK_TEMP_GET_BY_BUS = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BANK_TEMP_GET_BY_BUS";
        private const string PROC_BUS_CUS_BANK_TEMP_GET = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUS_CUS_BANK_TEMP_GET";
        private const string PROC_BUS_CUS_BANK_TEMP_GET_ALL = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_BUS_CUS_BANK_TEMP_GET_ALL";
        #endregion

        #region Chữ ký số
        private const string PROC_UPDATE_DIGITAL_SIGN_TEMP = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_UPDATE_DIGITAL_SIGN_TEMP";
        private const string PROC_UPDATE_DIGITAL_SIGN = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_UPDATE_DIGITAL_SIGN";
        private const string PROC_GET_DIGITAL_SIGN = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_GET_DIGITAL_SIGN";
        private const string PROC_GET_DIGITAL_SIGN_TEMP = "EPIC.PKG_CORE_BUSINESS_CUSTOMER.PROC_GET_DIGITAL_SIGN_TEMP";
        #endregion
        public BusinessCustomerRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public BusinessCustomer FindBusinessCustomerByTaxCode(string taxCode)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomer>(PROC_BUSINESS_CUSTOMER_FIND_TAX_CODE, new
            {
                pv_TAX_CODE = taxCode,
            });
            return result;
        }

        public BusinessCustomerTemp Add(BusinessCustomerTemp entity)
        {
            _logger.LogInformation("Add BusinessCustomerTemp");
            return _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerTemp>(PROC_BUSINESS_CUSTOMER_TEMP_ADD, new
            {
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_SHORT_NAME = entity.ShortName,
                pv_ADDRESS = entity.Address,
                pv_TRADING_ADDRESS = entity.TradingAddress,
                pv_NATION = entity.Nation,
                pv_PHONE = entity.Phone,
                pv_MOBILE = entity.Mobile,
                pv_EMAIL = entity.Email,
                pv_TAX_CODE = entity.TaxCode,
                pv_BANK_ACC_NO = entity.BankAccNo,
                pv_BANK_ACC_NAME = entity.BankAccName,
                pv_BANK_ID = entity.BankId,
                pv_BANK_BRANCH_NAME = entity.BankBranchName,
                pv_LICENSE_DATE = entity.LicenseDate,
                pv_LICENSE_ISSUER = entity.LicenseIssuer,
                pv_CAPITAL = entity.Capital,
                pv_REP_NAME = entity.RepName,
                pv_REP_POSITION = entity.RepPosition,
                pv_DECISION_NO = entity.DecisionNo,
                pv_DECISION_DATE = entity.DecisionDate,
                pv_NUMBER_MODIFIED = entity.NumberModified,
                pv_DATE_MODIFIED = entity.DateModified,
                SESSION_USERNAME = entity.CreatedBy,
                pv_IS_CHECK = entity.IsCheck,
                pv_REP_ID_NO = entity.RepIdNo,
                pv_REP_ID_DATE = entity.RepIdDate,
                pv_REP_ID_ISSUER = entity.RepIdIssuer,
                pv_REP_ADDRESS = entity.RepAddress,
                pv_REP_SEX = entity.RepSex,
                pv_REP_BIRTH_DATE = entity.RepBirthDate,
                pv_BUSINESS_REGISTRATION_IMG = entity.BusinessRegistrationImg,
                pv_FANPAGE = entity.Fanpage,
                pv_WEBSITE = entity.Website,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_PARTNER_ID = entity.PartnerId,
                pv_AVATAR_IMAGE_URL = entity.AvatarImageUrl,
                pv_STAMP_IMAGE_URL = entity.StampImageUrl
            }, false);
        }

        public PagingResult<BusinessCustomerTemp> FindAll(int pageSize, int pageNumber, string keyword, string status, string name, string phone, string email, int? tradingProviderId, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BusinessCustomerTemp>(PROC_BUSINESS_CUSTOMER_TEMP_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_NAME = name,
                pv_PHONE = phone,
                pv_EMAIL = email,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
            });
            return result;
        }

        public BusinessCustomerTemp FindTempById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerTemp>(PROC_BUSINESS_CUSTOMER_TEMP_GET, new
            {
                pv_BUSINESS_CUSTOMER_TEMP_ID = id,
            });
            return result;
        }

        public BusinessCustomerTemp Update(BusinessCustomerTemp entity, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation("Update BusinessCustomerApprove");
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerTemp>(PROC_BUSINESS_CUSTOMER_TEMP_UPDATE, new
            {
                pv_BUSINESS_CUSTOMER_TEMP_ID = entity.BusinessCustomerTempId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_SHORT_NAME = entity.ShortName,
                pv_ADDRESS = entity.Address,
                pv_TRADING_ADDRESS = entity.TradingAddress,
                pv_NATION = entity.Nation,
                pv_PHONE = entity.Phone,
                pv_MOBILE = entity.Mobile,
                pv_EMAIL = entity.Email,
                pv_TAX_CODE = entity.TaxCode,
                pv_BANK_ACC_NO = entity.BankAccNo,
                pv_BANK_ACC_NAME = entity.BankAccName,
                pv_BANK_ID = entity.BankId,
                pv_BANK_BRANCH_NAME = entity.BankBranchName,
                pv_LICENSE_DATE = entity.LicenseDate,
                pv_LICENSE_ISSUER = entity.LicenseIssuer,
                pv_CAPITAL = entity.Capital,
                pv_REP_NAME = entity.RepName,
                pv_REP_POSITION = entity.RepPosition,
                pv_DECISION_NO = entity.DecisionNo,
                pv_DECISION_DATE = entity.DecisionDate,
                pv_NUMBER_MODIFIED = entity.NumberModified,
                pv_DATE_MODIFIED = entity.DateModified,
                SESSION_USERNAME = entity.ModifiedBy,
                pv_IS_CHECK = entity.IsCheck,
                pv_REP_ID_NO = entity.RepIdNo,
                pv_REP_ID_DATE = entity.RepIdDate,
                pv_REP_ID_ISSUER = entity.RepIdIssuer,
                pv_REP_ADDRESS = entity.RepAddress,
                pv_REP_SEX = entity.RepSex,
                pv_REP_BIRTH_DATE = entity.RepBirthDate,
                pv_BUSINESS_REGISTRATION_IMG = entity.BusinessRegistrationImg,
                pv_FANPAGE = entity.Fanpage,
                pv_WEBSITE = entity.Website,
                pv_SERVER = entity.Server,
                pv_KEY = entity.Key,
                pv_SECRET = entity.Secret,
                pv_AVATAR_IMAGE_URL = entity.AvatarImageUrl,
                pv_STAMP_IMAGE_URL = entity.StampImageUrl
            });
            return result;
        }
        public BusinessCustomerTemp BusinessCustomerUpdate(BusinessCustomerTemp entity, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation("Add BusinessCustomerApprove");
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerTemp>(PROC_BUSINESS_CUSTOMER_UPDATE, new
            {
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_BUSINESS_CUSTOMER_BANK_ID = entity.BusinessCustomerBankId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_SHORT_NAME = entity.ShortName,
                pv_ADDRESS = entity.Address,
                pv_TRADING_ADDRESS = entity.TradingAddress,
                pv_NATION = entity.Nation,
                pv_PHONE = entity.Phone,
                pv_MOBILE = entity.Mobile,
                pv_EMAIL = entity.Email,
                pv_TAX_CODE = entity.TaxCode,
                pv_BANK_ACC_NO = entity.BankAccNo,
                pv_BANK_ACC_NAME = entity.BankAccName,
                pv_BANK_ID = entity.BankId,
                pv_BANK_BRANCH_NAME = entity.BankBranchName,
                pv_LICENSE_DATE = entity.LicenseDate,
                pv_LICENSE_ISSUER = entity.LicenseIssuer,
                pv_CAPITAL = entity.Capital,
                pv_REP_NAME = entity.RepName,
                pv_REP_POSITION = entity.RepPosition,
                pv_DECISION_NO = entity.DecisionNo,
                pv_DECISION_DATE = entity.DecisionDate,
                pv_NUMBER_MODIFIED = entity.NumberModified,
                pv_DATE_MODIFIED = entity.DateModified,
                SESSION_USERNAME = entity.CreatedBy,
                pv_IS_CHECK = entity.IsCheck,
                pv_REP_ID_NO = entity.RepIdNo,
                pv_REP_ID_DATE = entity.RepIdDate,
                pv_REP_ID_ISSUER = entity.RepIdIssuer,
                pv_REP_ADDRESS = entity.RepAddress,
                pv_REP_SEX = entity.RepSex,
                pv_REP_BIRTH_DATE = entity.RepBirthDate,
                pv_BUSINESS_REGISTRATION_IMG = entity.BusinessRegistrationImg,
                pv_FANPAGE = entity.Fanpage,
                pv_WEBSITE = entity.Website,
                pv_SERVER = entity.Server,
                pv_KEY = entity.Key,
                pv_SECRET = entity.Secret,
                pv_AVATAR_IMAGE_URL = entity.AvatarImageUrl,
                pv_STAMP_IMAGE_URL = entity.StampImageUrl
            });
            return result;
        }

        public int BankTempSetDefault(BusinessCustomerBankTempDefault entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BANK_TEMP_SET_DEFAULT, new
            {
                pv_ID = entity.Id,
                pv_BUSINESS_CUSTOMER_TEMP_ID = entity.BusinessCustomerTempId
            });
        }

        public int BankSetDefault(BusinessCustomerBankDefault entity)
        {
            _logger.LogInformation("Update BusinessCustomerApprove");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BANK_SET_DEFAULT, new
            {
                pv_BUSINESS_CUSTOMER_BANK_ID = entity.BusinessCustomerBankAccId,
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId
            });
        }

        public CoreApprove AddApprove(int businessCustomerId)
        {

            return _oracleHelper.ExecuteProcedureToFirst<CoreApprove>(PROC_BUSINESS_CUSTOMER_APPROVE_ADD, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId
            });
        }

        public int UpdateApprove(int? businessCustomerId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_CUSTOMER_APPROVE_UPDATE, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId
            });
        }

        public int Check(int businessCustomerId)
        {

            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_CUSTOMER_CHECK, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId
            });
        }

        public int Cancel(int businessCustomerId)
        {

            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_CUSTOMER_CANCEL, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId
            });
        }

        public PagingResult<BusinessCustomerDto> FindAllBusinessCustomer(int pageSize, int pageNumber, string keyword, string name, string phone, string isCheck, string email, int? tradingProviderId, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BusinessCustomerDto>(PROC_BUSINESS_CUS_CMS_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_NAME = name,
                pv_PHONE = phone,
                pv_IS_CHECK = isCheck,
                pv_EMAIL = email,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
            });
            return result;
        }

        public PagingResult<BusinessCustomerDto> FindAllBusinessCustomerByTaxCode(int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BusinessCustomerDto>(PROC_BUSINESS_CUSTOMER_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public BusinessCustomer FindBusinessCustomerById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomer>(PROC_BUSINESS_CUSTOMER_GET, new
            {
                pv_BUSINESS_CUSTOMER_ID = id,
            });
            return result;
        }

        public PagingResult<BusinessCustomerBank> FindAllBusinessCusBank(int businessCustomerId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BusinessCustomerBank>(PROC_BUSINESS_CUSTOMER_BANK_GET_ALL, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public List<BusinessCustomerBank> FindAllBusinessCusBankNoPage(int businessCustomerId)
        {
            var result = _oracleHelper.ExecuteProcedure<BusinessCustomerBank>(PROC_BUSINESS_CUSTOMER_BANK_GET_ALL_NO_PAGE, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId
            }).ToList();
            return result;
        }

        public BusinessCustomerBank FindBusinessCusBankById(long id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerBank>(PROC_BUSINESS_CUSTOMER_BANK_GET, new
            {
                pv_BUSINESS_CUSTOMER_BANK_ID = id,
            });
            return result;
        }

        public BusinessCustomerBank FindBusinessCusBankDefault(int id, int isTemp)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerBank>(PROC_BUSINESS_CUSTOMER_BANK_GET_DEFAULT, new
            {
                pv_ID = id,
                pv_IS_TEMP = isTemp
            });
            return result;
        }
        public BusinessCustomerBank FindBusinessCusBankByBusiCusId(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerBank>(PROC_BUSINESS_CUSTOMER_BANK_GET_BY_BUSI_CUS_ID, new
            {
                pv_BUSINESS_CUSTOMER_ID = id,
            });
            return result;
        }

        public BusinessCustomerBankTemp BusinessCustomerBankAdd(BusinessCustomerBankTemp entity, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation("Add BusinessCustomerBankAdd");
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerBankTemp>(PROC_BUSINESS_CUSTOMER_BANK_ADD, new
            {
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_BANK_ACC_NO = entity.BankAccNo,
                pv_BANK_ACC_NAME = entity.BankAccName,
                pv_BANK_ID = entity.BankId,
                pv_BANK_BRANCH_NAME = entity.BankBranchName,
                pv_IS_DEFAULT = entity.IsDefault,
                pv_IS_TEMP = entity.IsTemp,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }


        public BusinessCustomerBankTemp BusinessCustomerBankUpdate(BusinessCustomerBankTemp entity, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation("Update BusinessCustomerBank");
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerBankTemp>(PROC_BUSINESS_CUS_BANK_UPDATE, new
            {
                pv_BUSINESS_CUS_BANK_ID = entity.Id,
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_BANK_ACC_NO = entity.BankAccNo,
                pv_BANK_ACC_NAME = entity.BankAccName,
                pv_BANK_ID = entity.BankId,
                pv_BANK_BRANCH_NAME = entity.BankBranchName,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int BusinessCustomerActive(int id, bool isActive)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_CUSTOMER_BANK_ACTIVE, new
            {
                pv_BUSINESS_CUSTOMER_BANK_ID = id,
                pv_STATUS = isActive,
            });
        }

        public int BusinessCustomerDelete(int id)
        {
            _logger.LogInformation($"Delete BussinessCustomer: {id}");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_CUS_DEL, new
            {
                pv_BUSINESS_CUSTOMER_ID = id
            });
            return result;
        }

        public int BusinessCustomerBankDelete(int id)
        {
            _logger.LogInformation($"Delete BussinessCustomer: {id}");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_CUS_BANK_DEL, new
            {
                pv_BUSINESS_CUSTOMER_BANK_ID = id
            });
            return result;
        }

        public int BusinessCustomerTempRequest(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BUSINESS_CUSTOMER_TEMP_REQUEST, new
            {
                pv_BUSINESS_CUSTOMER_TEMP_ID = id
            });
        }

        public int BusinessCustomerBankTempUpdate(BusinessCustomerBankTemp entity, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation("Update BusinessCustomerBank Temp");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BUS_CUS_BANK_UPDATE_TEMP, new
            {
                pv_ID = entity.Id,
                pv_BUSINESS_CUSTOMER_TEMP_ID = entity.BusinessCustomerTempId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_BANK_ACC_NO = entity.BankAccNo,
                pv_BANK_ACC_NAME = entity.BankAccName,
                pv_BANK_ID = entity.BankId,
                pv_BANK_BRANCH_NAME = entity.BankBranchName,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public BusinessCustomerBankTemp FindBusinessCustomerBankTempById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BusinessCustomerBankTemp>(PROC_BUS_CUS_BANK_TEMP_GET, new
            {
                pv_ID = id,
            });
            return result;
        }

        public IEnumerable<BusinessCustomerBankTemp> FindBankTempByBusinessCustomer(int? id)
        {
            var result = _oracleHelper.ExecuteProcedure<BusinessCustomerBankTemp>(PROC_BANK_TEMP_GET_BY_BUS, new
            {
                pv_BUSINESS_CUSTOMER_ID = id,
            });
            return result;
        }

        public int UpdateDigitalSignTemp(int? businessCustomerTempId, int? tradingProviderId, int? partnerId, DigitalSign input)
        {
            _logger.LogInformation("Update DigitalSign Temp");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_DIGITAL_SIGN_TEMP, new
            {
                pv_BUSINESS_CUSTOMER_TEMP_ID = businessCustomerTempId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_SERVER = input.Server,
                pv_KEY = input.Key,
                pv_SECRET = input.Secret,
                pv_STAMP_IMAGE_URL = input.StampImageUrl,
                SESSION_USERNAME = input.ModifiedBy,
            });
            return result;
        }
        public DigitalSign UpdateDigitalSign(int? businessCustomerId, int? tradingProviderId, int? partnerId, DigitalSign input)
        {
            _logger.LogInformation("Update DigitalSign");
            var result = _oracleHelper.ExecuteProcedureToFirst<DigitalSign>(PROC_UPDATE_DIGITAL_SIGN, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_SERVER = input.Server,
                pv_KEY = input.Key,
                pv_SECRET = input.Secret,
                pv_STAMP_IMAGE_URL = input.StampImageUrl,
                SESSION_USERNAME = input.ModifiedBy
            });
            return result;
        }
        /// <summary>
        /// Lấy dữ liẹu chữ ký số của khách hàng doanh nghiệp bảng tạm
        /// </summary>
        /// <param name="businesCustomerTempId"></param>
        /// <returns></returns>
        public DigitalSign GetDigitalSignTemp(int? businesCustomerTempId)
        {
            _logger.LogInformation("Get DigitalSignTemp");
            var result = _oracleHelper.ExecuteProcedureToFirst<DigitalSign>(PROC_GET_DIGITAL_SIGN_TEMP, new
            {
                pv_BUSINESS_CUSTOMER_TEMP_ID = businesCustomerTempId
            });
            return result;
        }

        /// <summary>
        /// Lấy dữ liệu chữ ký số của khách hàng doanh nghiệp bảng chính
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <returns></returns>
        public DigitalSign GetDigitalSign(int? businessCustomerId)
        {
            _logger.LogInformation("Get DigitalSign");
            var result = _oracleHelper.ExecuteProcedureToFirst<DigitalSign>(PROC_GET_DIGITAL_SIGN, new
            {
                pv_BUSINESS_CUSTOMER_ID = businessCustomerId,
            });
            return result;
        }
    }
}

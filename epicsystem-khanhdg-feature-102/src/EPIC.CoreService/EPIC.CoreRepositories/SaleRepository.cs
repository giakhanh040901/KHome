using EPIC.BondRepositories;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.ExportReport;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.SaleAppStatistical;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreRepositories
{
    public class SaleRepository : BaseRepository
    {
        private const string PROC_SALE_ADD_TEMP = "PKG_CORE_SALE.PROC_SALE_ADD_TEMP";
        private const string PROC_SALE_DELETE = "PKG_CORE_SALE.PROC_SALE_DELETE";
        private const string PROC_SALE_GET = "PKG_CORE_SALE.PROC_SALE_GET";
        private const string PROC_SALE_GET_ALL = "PKG_CORE_SALE.PROC_SALE_GET_ALL";
        private const string PROC_SALE_TEMP_GET_ALL = "PKG_CORE_SALE.PROC_SALE_TEMP_GET_ALL";
        private const string PROC_SALE_GET_ALL_MANAGER = "PKG_CORE_SALE.PROC_SALE_GET_ALL_MANAGER";
        private const string PROC_SALE_GET_BY_INVESTOR_ID = "PKG_CORE_SALE.PROC_SALE_GET_BY_INVESTOR_ID";
        private const string PROC_SALE_FIND_TEMP_BY_ID = "PKG_CORE_SALE.PROC_SALE_FIND_TEMP_BY_ID";
        private const string PROC_SALE_FIND_BY_ID = "PKG_CORE_SALE.PROC_SALE_FIND_BY_ID";
        private const string PROC_SALE_TEMP_REQUEST = "PKG_CORE_SALE.PROC_SALE_TEMP_REQUEST";
        private const string PROC_SALE_APPROVE = "PKG_CORE_SALE.PROC_SALE_APPROVE";
        private const string PROC_SALE_TEMP_UPDATE = "PKG_CORE_SALE.PROC_SALE_TEMP_UPDATE";
        private const string PROC_SALE_UPDATE = "PKG_CORE_SALE.PROC_SALE_UPDATE";
        private const string PROC_SALE_ACTIVE = "PKG_CORE_SALE.PROC_SALE_ACTIVE";
        private const string PROC_SALE_TEMP_DELETE = "PKG_CORE_SALE.PROC_SALE_TEMP_DELETE";
        private const string PROC_SALE_GET_BY_ID = "PKG_CORE_SALE.PROC_SALE_GET_BY_ID";
        private const string PROC_SALE_TEMP_UPDATE_CMS = "PKG_CORE_SALE.PROC_SALE_TEMP_UPDATE_CMS";
        private const string PROC_SALE_CANCEL = "PKG_CORE_SALE.PROC_SALE_CANCEL";
        private const string PROC_SALE_BY_REFERALL_CODE = "PKG_CORE_SALE.PROC_SALE_BY_REFERALL_CODE";
        private const string PROC_FIND_ALL_SALE_BUSINESS = "PKG_CORE_SALE.PROC_FIND_ALL_SALE_BUSINESS";
        private const string PROC_GET_ALL_BUSINESS_TRADING_SALE = "PKG_CORE_SALE.PROC_GET_ALL_BUS_TRADING_SALE";
        private const string PROC_FIND_ALL_TRADING_UP = "PKG_CORE_SALE.PROC_FIND_ALL_TRADING_UP";

        private const string PROC_APP_SALE_REGISTER = "PKG_CORE_SALE.PROC_APP_SALE_REGISTER";
        private const string PROC_APP_LIST_SALE_REGISTER = "PKG_CORE_SALE.PROC_APP_LIST_SALE_REGISTER";
        private const string PROC_APP_SALE_DIRECTION_SALE = "PKG_CORE_SALE.PROC_APP_SALE_DIRECTION_SALE";
        private const string PROC_APP_LIST_TRADING_PROVIDER = "PKG_CORE_SALE.PROC_APP_LIST_TRADING_PROVIDER";
        private const string PROC_APP_SALE_STATUS = "PKG_CORE_SALE.PROC_APP_SALE_STATUS";
        private const string PROC_APP_SALE_BY_ID = "PKG_CORE_SALE.PROC_APP_SALE_BY_ID";
        private const string PROC_APP_LIST_TRADING_BY_SALE = "PKG_CORE_SALE.PROC_APP_LIST_TRADING_BY_SALE";
        private const string PROC_SALE_REGISTER_GET_ALL = "PKG_CORE_SALE.PROC_SALE_REGISTER_GET_ALL";
        private const string PROC_SALE_TEMP_SIGN = "PKG_CORE_SALE.PROC_SALE_TEMP_SIGN";
        private const string PROC_SALE_GET_COLLAB_CONTRACT = "PKG_CORE_SALE.PROC_SALE_GET_COLLAB_CONTRACT";
        private const string PROC_APP_FIND_SALE_CHILD = "PKG_CORE_SALE.PROC_APP_FIND_SALE_CHILD";
        private const string PROC_APP_MANAGER_SALE_CHILD = "PKG_CORE_SALE.PROC_APP_MANAGER_SALE_CHILD";
        private const string PROC_SALE_REGISTER_GET_TRADING = "PKG_CORE_SALE.PROC_SALE_REGISTER_GET_TRADING";
        private const string PROC_SALE_GET_TRADING_IN_TEMP = "PKG_CORE_SALE.PROC_SALE_GET_TRADING_IN_TEMP";
        private const string PROC_APP_SALE_OVERVIEW = "PKG_CORE_SALE.PROC_APP_SALE_OVERVIEW";
        private const string PROC_APP_GET_REGISTER_BY_INVESTOR = "PKG_CORE_SALE.PROC_APP_REGISTER_BY_INVESTOR";
        private const string PROC_FIND_TEMP_BY_REGISTER_ID = "PKG_CORE_SALE.PROC_FIND_TEMP_BY_REGISTER_ID";
        private const string PROC_SALE_CANCEL_REGISTER = "PKG_CORE_SALE.PROC_SALE_CANCEL_REGISTER";
        private const string PROC_LIST_SALE_BY_PARENT_SALE = "PKG_CORE_SALE.PROC_LIST_SALE_BY_PARENT_SALE";
        private const string PROC_SALE_GET_DATA_REGISTER = "PKG_CORE_SALE.PROC_SALE_GET_DATA_REGISTER";
        private const string PROC_GET_SALE_INFO_BY_REFFERAL_CODE = "PKG_CORE_SALE.PROC_DEPART_SALE_N_NAME_SALE"; // lấy thông tin tên sale và phòng ban sale bằng refferal code
        private const string PROC_GET_BUSINESS_SALE_INFO_BY_REFFERAL_CODE = "PKG_CORE_SALE.PROC_DEPART_BUSI_SALE"; //lấy thông tin tên sale và phòn ban sale của sale business
        private const string PROC_GET_CAP_DUOI_SALE_BY_SALE_ID = "PKG_SALE_APP_STATISTICAL.FIND_SALE_CAP_DUOI_BY_SALE_ID"; //lấy thông tin của cấp dưới của sale bằng sale id
        private const string PROC_APP_SALE_DOANH_SO = "PKG_CORE_SALE.PROC_APP_SALE_DOANH_SO"; //lấy thông tin hợp đồng sale bằng sale id
        private const string PROC_APP_THONG_KE_KH = "PKG_CORE_SALE.PROC_APP_THONG_KE_KH";
        private const string PROC_APP_SALE_THONG_KE_HOP_DONG = "PKG_SALE_APP_STATISTICAL.FIND_HOP_DONG_SALE_APP";
        private const string PROC_SALE_INFO_BY_REFERALL_CODE = "PKG_CORE_SALE.PROC_SALE_INFO_BY_REFERALL_CODE";
        private const string PROC_SALE_FIND_CORE_SALE = "PKG_CORE_SALE.PROC_SALE_FIND_CORE_SALE";
        private const string PROC_SALE_UPDATE_AUTO_DIRECTIONAL = "PKG_CORE_SALE.PROC_SALE_UPDATE_AUTO_DIRECTIONAL";
        private const string PROC_SALE_CHECK_SALE_TYPE_NOT_COLLABORATOR = "PKG_CORE_SALE.PROC_SALE_CHECK_SALE_TYPE_NOT_COLLABORATOR";
        private const string PROC_GET_DEPARMENTS_BY_SALE_ID = "PKG_CORE_SALE.PROC_GET_DEPARMENTS_BY_SALE_ID";
        private const string PROC_SALE_TONG_DOANH_SO = "PKG_CORE_EXCEL_REPORT.PROC_SALE_DOANH_SO";
        private const string PROC_GET_SALER_LIST_EXCEL = "PKG_CORE_EXCEL_REPORT.PROC_LIST_SALE_EXCEL";

        private const string PROC_CHECK_SALE_INVESTOR_SOURCE = "PKG_CORE_SALE.PROC_CHECK_SALE_INVESTOR_SOURCE";
        public SaleRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public SaleDto FindById(int id, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleDto>(PROC_SALE_GET, new
            {
                pv_SALE_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public SaleTempDto AddSaleTemp(AddSaleDto entity, string createBy, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleTempDto>(PROC_SALE_ADD_TEMP, new
            {
                pv_INVESTOR_ID = entity.InvestorId,
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_EMPLOYEE_CODE = entity.EmployeeCode,
                pv_DEPARTMENT_ID = entity.DepartmentId,
                pv_SALE_TYPE = entity.SaleType,
                pv_SALE_PARENT_ID = entity.SaleParentId,
                pv_INVESTOR_BANK_ACC_ID = entity.InvestorBankAccId,
                pv_BUSINESS_BANK_ACC_ID = entity.BusinessCustomerBankAccId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_CREATED_BY = createBy
            });
        }

        public int Active(int id, string modifiedBy, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_ACTIVE, new
            {
                pv_SALE_ID = id,
                pv_MODIFIED_BY = modifiedBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public void SaleTempRequest(int saleTempId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_TEMP_REQUEST, new
            {
                pv_SALE_TEMP_ID = saleTempId
            });
        }

        public int ApproveSaler(int saleTempId)
        {
            var result =  _oracleHelper.ExecuteProcedureToFirst<int>(PROC_SALE_APPROVE, new
            {
                pv_SALE_TEMP_ID = saleTempId
            });
            return result;
        }

        public int Delete(int id, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_DELETE, new
            {
                pv_SALE_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public int DeleteSaleTemp(int id, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_TEMP_DELETE, new
            {
                pv_SALE_TEMP_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public SaleInvestorDto FindAllListManager(string referralCode, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<SaleInvestorDto>(PROC_SALE_GET_ALL_MANAGER, new
            {
                pv_REFERRAL_CODE = referralCode,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public SaleInvestorDto SaleGetInfoByReferralCode(string referralCode, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleInvestorDto>(PROC_SALE_INFO_BY_REFERALL_CODE, new
            {
                pv_REFERRAL_CODE = referralCode,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public PagingResult<ViewSaleDto> FindAll(int pageSize, int pageNumber, int? deparmentId, string keyword, int? saleType, string area, string status, int? tradingProviderId, string employeeCode, string phone, string email, string idNo, string investorName, string taxCode, int? isInvestor, string referralCode)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ViewSaleDto>(PROC_SALE_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                pv_DEPARTMENT_ID = deparmentId,
                KEY_WORD = keyword,
                pv_SALE_TYPE = saleType,
                pv_AREA = area,
                pv_STATUS = status,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_EMPLOYEE_CODE = employeeCode,
                pv_PHONE = phone,
                pv_EMAIL = email,
                pv_ID_NO = idNo,
                pv_INVESTOR_NAME = investorName,
                pv_TAX_CODE = taxCode,
                pv_IS_INVESTOR = isInvestor,
                pv_REFERRAL_CODE = referralCode
            });
            return result;
        }

        public PagingResult<SaleTempDto> FindAllSaleTemp(int? pageSize, int pageNumber, string keyword, int? saleType, int? tradingProviderId, string employeeCode, string phone, string email, string idNo, string investorName, int? status, int? source, string taxCode, int? isInvestor)
        {
            return _oracleHelper.ExecuteProcedurePaging<SaleTempDto>(PROC_SALE_TEMP_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_SALE_TYPE = saleType,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_EMPLOYEE_CODE = employeeCode,
                pv_PHONE = phone,
                pv_EMAIL = email,
                pv_ID_NO = idNo,
                pv_INVESTOR_NAME = investorName,
                pv_STATUS = status,
                pv_SOURCE = source,
                pv_TAX_CODE = taxCode,
                pv_IS_INVESTOR = isInvestor,
            });
        }

        public SaleTempDto FindSaleTemp(int saleTempId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleTempDto>(PROC_SALE_FIND_TEMP_BY_ID, new
            {
                pv_SALE_TEMP_ID = saleTempId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public ViewSaleDto FindSaleById(int saleId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<ViewSaleDto>(PROC_SALE_FIND_BY_ID, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public int UpdateSaleTemp(UpdateSaleTempDto input, int? tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_TEMP_UPDATE, new
            {
                pv_SALE_TEMP_ID = input.SaleTempId,
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_EMPLOYEE_CODE = input.EmployeeCode,
                pv_SALE_PARENT_ID = input.ParentId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_MODIFIED_BY = modifiedBy
            });
            return result;
        }

        /// <summary>
        /// Danh sách Sale chờ điều hướng, nếu không truyền lên đại lý thì lấy tất cả sale ở tất cả các đại lý mà sale quản lý điều hướng
        /// </summary>
        /// <param name="saleDirectionId"> Sale quản lý điều hướng</param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<AppListSaleRegisterDto> AppListSaleRegister(int saleDirectionId, int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<AppListSaleRegisterDto>(PROC_APP_LIST_SALE_REGISTER, new
            {
                pv_MANAGER_SALE_ID = saleDirectionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }).ToList();
            return result;
        }

        public SaleRegister AppSaleRegister(AppSaleRegisterDto input, int investorId, string ipAddress, string createdBy)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleRegister>(PROC_APP_SALE_REGISTER, new
            {
                pv_INVESTOR_ID = investorId,
                pv_INVESTOR_BANK_ACC_ID = input.BankAccId,
                pv_SALE_MANAGER_ID = input.SaleManagerId,
                pv_IP_ADDRESS = ipAddress,
                pv_CREATED_BY = createdBy
            });
        }

        public SaleTempDto DirectionSale(DirectionSaleDto input, int? saleManagerId, string createdBy)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleTempDto>(PROC_APP_SALE_DIRECTION_SALE, new
            {
                pv_SALE_REGISTER_ID = input.SaleRegisterId,
                pv_SALE_MANAGER_ID = saleManagerId,
                pv_TRADING_PROVIDER_ID = input.TradingProviderId,
                pv_SALE_TYPE = input.SaleType,
                pv_CREATED_BY = createdBy
            });
        }

        /// <summary>
        /// trả về sale theo investor id nếu investor này là sale
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public Sale FindSaleByInvestorId(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Sale>(PROC_SALE_GET_BY_INVESTOR_ID, new
            {
                pv_INVESTOR_ID = investorId
            });
        }

        public Sale SaleCheckSaleTypeNotCollaborator(int saleId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Sale>(PROC_SALE_CHECK_SALE_TYPE_NOT_COLLABORATOR, new
            {
                pv_SALE_ID = saleId
            });
        }

        public List<AppListTradingProviderDto> AppListTradingProvider(int managerSaleId)
        {
            var result = _oracleHelper.ExecuteProcedure<AppListTradingProviderDto>(PROC_APP_LIST_TRADING_PROVIDER, new
            {
                pv_MANAGER_SALE_ID = managerSaleId,
            }).ToList();
            return result;
        }

        /// <summary>
        /// Đổ ra thông tin từ các bảng liên quan của Sale: CORE_SALE, CORE_SALE_TRADING_PROVIDER, DEPARTMENT, DEPARTMENT_SALE...
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public SaleInfoDto SaleGetById(int saleId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleInfoDto>(PROC_SALE_GET_BY_ID, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        /// <summary>
        /// Lấy thông tin chi tiết Sale bằng tài khoản hiện hành
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public SaleInfoBySaleIdDto AppSaleInfo(int saleId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleInfoBySaleIdDto>(PROC_APP_SALE_BY_ID, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        /// <summary>
        /// Kiểm tra trạng thái tài khoản của Sale
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public AppCheckSaler AppCheckSaler(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppCheckSaler>(PROC_APP_SALE_STATUS, new
            {
                pv_INVESTOR_ID = investorId
            });
        }

        /// <summary>
        /// LấY danh sách Đại lý mà Sale đang tham gia
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public List<AppListTradingProviderDto> AppListTradingProviderBySale(int saleId)
        {
            var result = _oracleHelper.ExecuteProcedure<AppListTradingProviderDto>(PROC_APP_LIST_TRADING_BY_SALE, new
            {
                pv_SALE_ID = saleId,
            }).ToList();
            return result;
        }

        public int UpdateSaleTempCms(UpdateSaleTempCmsDto input, int? tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_TEMP_UPDATE_CMS, new
            {
                pv_SALE_TEMP_ID = input.SaleTempId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_EMPLOYEE_CODE = input.EmployeeCode,
                pv_SALE_TYPE = input.SaleType,
                pv_INVESTOR_BANK_ACC_ID = input.InvestorBankAccId,
                pv_BUSINESS_BANK_ACC_ID = input.BusinessCustomerBankAccId,
                pv_SALE_PARENT_ID = input.SaleParentId,
                pv_MODIFIED_BY = modifiedBy
            });
            return result;
        }

        public int CancelSale(int saleTempId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_CANCEL, new
            {
                pv_SALE_TEMP_ID = saleTempId
            });
        }

        public int Update(UpdateSaleDto input, int tradingProviderId, string modifiedBy)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_UPDATE, new
            {
                pv_SALE_ID = input.SaleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_DEPARTMENT_ID = input.DepartmentId,
                pv_EMPLOYEE_CODE = input.EmployeeCode,
                pv_SALE_TYPE = input.SaleType,
                pv_INVESTOR_BANK_ACC_ID = input.InvestorBankAccId,
                pv_BUSINESS_BANK_ACC_ID = input.BusinessCustomerBankAccId,
                pv_SALE_PARENT_ID = input.SaleParentId,
                pv_MODIFIED_BY = modifiedBy
            });
            return result;
        }

        public PagingResult<ViewSaleRegisterDto> FindAllSaleRegister(int? pageSize, int pageNumber, string keyword, int? status, string phone, string idNo, string investorName)
        {
            return _oracleHelper.ExecuteProcedurePaging<ViewSaleRegisterDto>(PROC_SALE_REGISTER_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_PHONE = phone,
                pv_ID_NO = idNo,
                pv_INVESTOR_NAME = investorName,
            });
        }

        public AppSaleByReferralCodeDto FindSaleByReferralCode(string referralCode, int? tradingProviderId, string phone = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppSaleByReferralCodeDto>(PROC_SALE_BY_REFERALL_CODE, new
            {
                pv_REFERRAL_CODE = referralCode,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PHONE = phone
            }).ThrowIfNull(ErrorCode.CoreSaleNotFound, $"Không tìm thấy sale có mã giới thiệu \"{referralCode}\"");
        }

        public List<AppListCollabContractDto> ListCollabContract(int investorId)
        {
            return _oracleHelper.ExecuteProcedure<AppListCollabContractDto>(PROC_SALE_GET_COLLAB_CONTRACT, new
            {
                pv_INVESTOR_ID = investorId
            }).ToList();
        }

        /// <summary>
        /// Ký hoặc huỷ ký, nếu tất cả không ký bảng ghi temp nào thì sẽ coi là đã huỷ
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="saleTempId"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        public AppSaleTempSignDto AppSaleTempSign(int investorId, int saleTempId, int isSign)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<AppSaleTempSignDto>(PROC_SALE_TEMP_SIGN, new
            {
                pv_INVESTOR_ID = investorId,
                pv_SALE_TEMP_ID = saleTempId,
                pv_IS_SIGN = isSign
            });
            return result;
        }

        public List<AppSaleManagerSaleDto> AppManagerSaleChild(int saleId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppSaleManagerSaleDto>(PROC_APP_MANAGER_SALE_CHILD, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }).ToList();
        }

        public List<DepartmentSaleDto> GetSaleByParentSale(int parentSaleId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<DepartmentSaleDto>(PROC_LIST_SALE_BY_PARENT_SALE, new
            {
                pv_PARENT_SALE_ID = parentSaleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }).ToList();
        }

        public AppSaleChildDto AppFindSaleChild(int saleId, int managerSaleId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppSaleChildDto>(PROC_APP_FIND_SALE_CHILD, new
            {
                pv_SALE_ID = saleId,
                pv_MANAGER_SALE_ID = managerSaleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        /// <summary>
        /// Khi sale Register đã điều hướng, xem sale thuộc DLSC nào
        /// </summary>
        /// <param name="saleRegisterId"></param>
        /// <returns></returns>
        public List<SaleRegisterDirectionToTradingProviderDto> ListTradingProviderBySaleRegister(int saleRegisterId)
        {
            return _oracleHelper.ExecuteProcedure<SaleRegisterDirectionToTradingProviderDto>(PROC_SALE_REGISTER_GET_TRADING, new
            {
                pv_SALE_REGISTER_ID = saleRegisterId
            }).ToList();
        }

        /// <summary>
        /// Lấy danh sách đại lý sơ cấp theo sale đang ở trạng thái tạm chờ ký
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<AppListTradingProviderDto> GetListTradingProviderBySaleInTemp(int investorId)
        {
            return _oracleHelper.ExecuteProcedure<AppListTradingProviderDto>(PROC_SALE_GET_TRADING_IN_TEMP, new
            {
                pv_INVESTOR_ID = investorId
            }).ToList();
        }

        /// <summary>
        /// Thống kê 4 ô trên app, lấy ra thông tin của sản phẩm INVEST
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="tradingProviderUpIds">đại lý trên của đại lý đang xét</param>
        /// <returns></returns>
        public AppSalerOverviewDto AppSalerOverview(int saleId, int tradingProviderId, List<int> tradingProviderUpIds = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppSalerOverviewDto>(PROC_APP_SALE_OVERVIEW, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TRADING_PROVIDER_UP_IDS = tradingProviderUpIds != null ? string.Join(',', tradingProviderUpIds) : null
            });
        }

        /// <summary>
        /// Lịch sử đăng ký của investor
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<CoreSaleRegisterDto> AppListRegisterByInvestor(int investorId)
        {
            return _oracleHelper.ExecuteProcedure<CoreSaleRegisterDto>(PROC_APP_GET_REGISTER_BY_INVESTOR, new
            {
                pv_INVESTOR_ID = investorId
            }).ToList();
        }

        /// <summary>
        /// Tìm kiếm bảng sale tạm theo trường sale register id
        /// </summary>
        /// <param name="saleRegisterId"></param>
        /// <returns></returns>
        public SaleTempDto FindTempByRegisterId(int saleRegisterId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleTempDto>(PROC_FIND_TEMP_BY_REGISTER_ID, new
            {
                pv_SALE_REGISTER_ID = saleRegisterId,
            });
        }

        /// <summary>
        /// Huỷ điều hướng sale, đổi trạng thái thành huỷ ở bảng đăng ký
        /// </summary>
        /// <param name="saleRegisterId"></param>
        public void CancelRegister(int saleRegisterId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_CANCEL_REGISTER, new
            {
                pv_SALE_REGISTER_ID = saleRegisterId,
            });
        }

        public SaleRegisterDto SaleGetDataRegister(int investorId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleRegisterDto>(PROC_SALE_GET_DATA_REGISTER, new
            {
                pv_INVESTOR_ID = investorId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        /// <summary>
        /// lấy thông tin tên sale và tên phòng ban của investor sale
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="SaleRefferalCode"></param>
        /// <returns></returns>
        public SaleInfoExcelDto GetSaleInfoByRefferalCode(int? tradingProviderId, string SaleRefferalCode)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleInfoExcelDto>(PROC_GET_SALE_INFO_BY_REFFERAL_CODE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SALE_REFERRAL_CODE = SaleRefferalCode,
            });
        }

        public SaleInfoExcelDto GetSaleInfoByRefferalCodeBusiness(int? tradingProviderId, string SaleRefferalCode)
        {
            return _oracleHelper.ExecuteProcedureToFirst<SaleInfoExcelDto>(PROC_GET_BUSINESS_SALE_INFO_BY_REFFERAL_CODE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SALE_REFERRAL_CODE = SaleRefferalCode,
            });
        }

        public List<SaleInfoAppDto> GetCapDuoiSaleBySaleId(int? tradingProviderId, int? saleId, string status)
        {
            var result = _oracleHelper.ExecuteProcedure<SaleInfoAppDto>(PROC_GET_CAP_DUOI_SALE_BY_SALE_ID, new
            {
                pv_STATUS = status,
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }).ToList();

            return result;
        }

        /// <summary>
        /// Thống kê dữ liệu hợp đồng cho nút hợp đồng sale app, productType = null: lấy tất cả, productType = 1 : lấy thông tin bond, productType = 2 : lấy thông tin invest , 
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="productType"></param>
        /// <param name="tradingProviderUpIds">đại lý cấp trên của đại lý đang xét</param>
        /// <returns></returns>
        public List<HopDongSaleAppDto> ThongKeHopDongAppSale(int? saleId, int? tradingProviderId, int? status, DateTime? startDate, DateTime? endDate, int? productType, List<int> tradingProviderUpIds = null)
        {
            var result = _oracleHelper.ExecuteProcedure<HopDongSaleAppDto>(PROC_APP_SALE_THONG_KE_HOP_DONG, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_PRODUCT_TYPE = productType,
                pv_TRADING_PROVIDER_UP_IDS = tradingProviderUpIds != null ? string.Join(',', tradingProviderUpIds) : null
            }).ToList();
            return result;
        }

        public AppSaleProceedDto ThongKeDoanhSo(AppSaleProceedFilterDto input, int saleId, List<int> tradingProviderUpIds = null, int? departmentId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppSaleProceedDto>(PROC_APP_SALE_DOANH_SO, new
            {
                pv_START_DATE = input.StartDate,
                pv_END_DATE = input.EndDate,
                pv_TRADING_PROVIDER_ID = input.TradingProviderId,
                pv_DEPARTMENT_ID = departmentId,
                pv_SAN_PHAM = input.Project,
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_UP_IDS = tradingProviderUpIds != null ? string.Join(',', tradingProviderUpIds) : null
            });
        }

        /// <summary>
        /// Created: CuongNX - 8/11/2022
        /// Purpose: Lấy thông tin doanh số của saler
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="project"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderUpIds"></param>
        /// <returns></returns>
        public AppSaleProceedDto ThongKeDoanhSoSale(int? tradingProviderId, int? saleId, List<int> tradingProviderUpIds = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppSaleProceedDto>(PROC_SALE_TONG_DOANH_SO, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_UP_IDS = tradingProviderUpIds != null ? string.Join(',', tradingProviderUpIds) : null
            });
        }

        public List<Investor> ThongKeKhachHangCuaSale(int saleId)
        {
            return _oracleHelper.ExecuteProcedure<Investor>(PROC_APP_THONG_KE_KH, new
            {
                pv_SALE_ID = saleId,
                pv_TRADING_PROVIDER_ID = 0
            }).ToList();
        }

        /// <summary>
        /// Liệt kê sale là doanh nghiệp của đại lý đang xét (1 bản ghi bussiness có thể có nhiều trading)
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="status">trạng thái của business customer ở đại lý đang xét</param>
        /// <returns></returns>
        public IEnumerable<SaleBusinessTradingDto> FindAllSaleBusinessTrading(int tradingProviderId, string status)
        {
            return _oracleHelper.ExecuteProcedure<SaleBusinessTradingDto>(PROC_FIND_ALL_SALE_BUSINESS, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status
            });
        }

        class SaleBusinessFromRootToDestLinkDto
        {
            public int TradingProviderId { get; set; }
            public List<int> Links { get; set; }
        }

        /// <summary>
        /// Tìm kiếm đường đi (khó) (có thể có nhiều đường đi từ gốc tới đích)
        /// </summary>
        /// <param name="tradingProviderIdCur"></param>
        /// <param name="saleIdDest"></param>
        /// <returns></returns>
        private SaleBusinessFromRootToDestLinkDto HandleFindAllListSaleBusinessFromRootToDest(SaleBusinessFromRootToDestLinkDto tradingProviderIdCur, int saleIdDest)
        {
            //đại lý hiện tại có chứa saler đang xét
            if (FindById(saleIdDest, tradingProviderIdCur.TradingProviderId) != null)
            {
                return tradingProviderIdCur;
            }

            var listTradingChild = FindAllSaleBusinessTrading(tradingProviderIdCur.TradingProviderId, SaleStatus.ACTIVE);
            foreach (var item in listTradingChild)
            {
                //var result = HandleFindAllListSaleBusinessFromRootToDest(item.TradingProviderId, saleIdDest);
                //if (result != null)
                //{
                //    return result;
                //}
            }
            return null;
        }

        /// <summary>
        /// danh sách doanh nghiệp + mã giới thiệu theo danh sách id đại lý
        /// </summary>
        /// <param name="tradingProviderIds"></param>
        /// <returns></returns>
        public List<SaleBusinessFromRootToDestDto> GetAllBusinessTradingSale(List<int> tradingProviderIds)
        {
            return _oracleHelper.ExecuteProcedure<SaleBusinessFromRootToDestDto>(PROC_GET_ALL_BUSINESS_TRADING_SALE, new
            {
                pv_TRADING_PROVIDER_IDS = string.Join('%', tradingProviderIds)
            }).ToList();
        }


        /// <summary>
        /// Danh sách saler là doanh nghiệp lần từ trading provider id gốc xuống đến sale id cần tìm (có thể có nhiều đường đi từ gốc tới đích)
        /// trả ra danh sách tính cả trading provider gốc
        /// </summary>
        public List<SaleBusinessFromRootToDestDto> FindAllListSaleBusinessFromRootToDest(int tradingProviderIdRoot, int saleIdDest)
        {
            List<int> businessTradingIds = new();

            //danh sách sale doanh nghiệp hoạt động
            var listTradingChild = FindAllSaleBusinessTrading(tradingProviderIdRoot, SaleStatus.ACTIVE);
            foreach (var item in listTradingChild)
            {
                if (FindById(saleIdDest, item.TradingProviderId) != null)
                {
                    businessTradingIds.Add(item.TradingProviderId);
                }
            }

            if (businessTradingIds.Count > 0) //nếu có đại lý con nào có saler đó thì thêm
            {
                businessTradingIds.Add(tradingProviderIdRoot); //tính cả đại lý bán hộ hiện tại
            }

            businessTradingIds = businessTradingIds.Distinct().ToList();
            var result = GetAllBusinessTradingSale(businessTradingIds);
            return result;
        }

        /// <summary>
        /// Lần ngược lên trên danh sách những đại lý là đại lý của đại lý đang xét (khi đại lý đang xét là saler doanh nghiệp)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusinessTradingDto> FindAllTradingUpFromSaleTrading(int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<BusinessTradingDto>(PROC_FIND_ALL_TRADING_UP, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        /// <summary>
        /// Lần ngược lên trên danh sách những đại lý là đại lý của đại lý đang xét (khi đại lý đang xét là saler doanh nghiệp)
        /// Lần 4 cấp
        /// </summary>
        /// <returns></returns>
        public List<BusinessTradingDto> FindAllTradingUpFromSaleTrading4Cap(int tradingProviderId)
        {
            var result = new List<BusinessTradingDto>();

            var tradingUp1s = FindAllTradingUpFromSaleTrading(tradingProviderId);
            foreach (var itemUp1 in tradingUp1s)
            {
                result.Add(itemUp1);
                var tradingUp2s = FindAllTradingUpFromSaleTrading(tradingProviderId);
                foreach (var itemUp2 in tradingUp2s)
                {
                    result.Add(itemUp2);
                    var tradingUp3s = FindAllTradingUpFromSaleTrading(tradingProviderId);
                    foreach (var itemUp3 in tradingUp3s)
                    {
                        result.Add(itemUp3);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy Id
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<int> GetListTradingProviderIdFrom4Level(int tradingProviderId)
        {
            return FindAllTradingUpFromSaleTrading4Cap(tradingProviderId).Select(r => r.TradingProviderId).Distinct().ToList();
        }

        /// <summary>
        /// Xem thông tin sale từ bảng EP_CORE_SALE
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public Sale FindCoreSale(int saleId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Sale>(PROC_SALE_FIND_CORE_SALE, new
            {
                pv_SALE_ID = saleId
            });
        }

        public int UpdateAutoDirectional(int saleId, string autoDirectional)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_SALE_UPDATE_AUTO_DIRECTIONAL, new
            {
                pv_SALE_ID = saleId,
                pv_AUTO_DIRECTIONAL = autoDirectional
            });
        }

        /// <summary>
        /// Lấy list department theo sale id
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public List<Department> GetListDepartmentBySaleId(int saleId)
        {
            return _oracleHelper.ExecuteProcedure<Department>(PROC_GET_DEPARMENTS_BY_SALE_ID, new
            {
                pv_SALE_ID = saleId
            })?.ToList();
        }

        /// <summary>
        /// Lấy danh sách saler cho báo cáo excel 
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<SalerListExcel> GetListSalerExcel(int tradingProviderId, DateTime? startDate, DateTime? endDate)
        {

            var result = _oracleHelper.ExecuteProcedure<SalerListExcel>(PROC_GET_SALER_LIST_EXCEL, new
            {
                pv_END_DATE = endDate,
                pv_START_DATE = startDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId

            }).ToList();
            return result;
        }
    }
}

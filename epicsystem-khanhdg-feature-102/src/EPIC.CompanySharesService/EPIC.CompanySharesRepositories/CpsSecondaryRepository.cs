using Dapper.Oracle;
using EPIC.DataAccess.Models;
using EPIC.DataAccess;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CpsSecondary;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.CompanySharesEntities.Dto.CpsApp;
using EPIC.CompanySharesEntities.Dto.Policy;

namespace EPIC.CompanySharesRepositories
{
    public class CpsSecondaryRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string schema = DbSchemas.EPIC_COMPANY_SHARES + ".";

        private const string PROC_ADD_SECONDARY = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_ADD";
        private const string PROC_ADD_POLICY = schema + "PKG_CPS_POLICY.PROC_CPS_POLICY_ADD";
        private const string PROC_ADD_POLICY_DE_RETURN = schema + "PKG_CPS_POLICY.PROC_CPS_POLICY_DE_ADD_RE";
        private const string PROC_ADD_POLICY_DETAIL = schema + "PKG_CPS_POLICY.PROC_CPS_POLICY_DETAIL_ADD";
        private const string PROC_UPDATE = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_UPDATE";
        private const string PROC_POLICY_CHANGE_STATUS = schema + "PKG_CPS_POLICY.PROC_POLICY_RE_STATUS";
        private const string PROC_POLICY_DETAIL_CHANGE_STATUS = schema + "PKG_CPS_POLICY.PROC_POLICY_DE_RE_STATUS";
        private const string PROC_GET_ALL = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_GET_ALL";
        private const string PROC_GET_ONE = schema + "PKG_CPS_SECONDARY.PROCT_CPS_SECONDARY_GET";
        private const string PROC_POLICY_UPDATE = schema + "PKG_CPS_POLICY.PROC_POLICY_UPDATE";
        private const string PROC_POLICY_DETAIL_UPDATE = schema + "PKG_CPS_POLICY.PROC_POLICY_DETAIL_UPDATE";
        private const string PROC_GET_ALL_LIST_POLICY = schema + "PKG_CPS_POLICY.PROC_GET_ALL_LIST_POLICY";
        private const string PROC_GET_ALL_LIST_POLICY_DETAIL = schema + "PKG_CPS_POLICY.PROC_GET_ALL_LIST_POLICY_DE";
        private const string PROC_SUPER_ADMIN_APPROVE = schema + "PKG_CPS_SECONDARY.PROC_SUPER_ADMIN_APPROVE";
        private const string PROC_TRADING_PROVIDER_APPROVE = schema + "PKG_CPS_SECONDARY.PROC_TRADING_PROVIDER_APPROVE";
        private const string PROC_TRADING_PROVIDER_SUBMIT = schema + "PKG_CPS_SECONDARY.PROC_TRADING_PROVIDER_SUBMIT";
        private const string PROC_IS_CLOSE = schema + "PKG_CPS_SECONDARY.PROC_IS_CLOSE";
        private const string PROC_IS_SHOW_APP = schema + "PKG_CPS_SECONDARY.PROC_IS_SHOW_APP";
        private const string PROC_POLICY_IS_SHOW_APP = schema + "PKG_CPS_POLICY.PROC_POLICY_IS_SHOW_APP";
        private const string PROC_POLICY_DETAIL_IS_SHOW_APP = schema + "PKG_CPS_POLICY.PROC_POLICY_DE_IS_SHOW_APP";
        private const string PROC_POLICY_DELETE = schema + "PKG_CPS_POLICY.PROC_POLICY_DELETE";
        private const string PROC_POLICY_DETAIL_DELETE = schema + "PKG_CPS_POLICY.PROC_POLICY_DETAIL_DELETE";
        private const string PROC_POLICY_GET = schema + "PKG_CPS_POLICY.PROCT_CPS_POLICY_GET";
        private const string PROCT_CPS_POLICY_DETAIL_GET_WITH_OUT_TRADING_PROVIDER_ID = schema + "PKG_CPS_POLICY.PROC_CPS_POLICY_DETAIL_GET_2";
        private const string PROC_POLICY_DETAIL_GET = schema + "PKG_CPS_POLICY.PROC_CPS_POLICY_DETAIL_GET";
        private const string PROC_CPS_SECONDARY_REQUEST = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_REQUEST";
        private const string PROC_CPS_SECONDARY_APPROVE = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_APPROVE";
        private const string PROC_CPS_SECONDARY_CHECK = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_CHECK";
        private const string PROC_CPS_SECONDARY_CANCEL = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_CANCEL";
        private const string PROC_SECONDARY_GET_BY_PRIMARY = schema + "PKG_CPS_SECONDARY.PROC_SECONDARY_GET_BY_PRIMARY";
        private const string PROC_GET_ALL_LIST_POLICY_ORDER = schema + "PKG_CPS_SECONDARY.PROC_GET_ALL_LIST_POLICY_ORDER";
        private const string PROC_CPS_SECONDARY_GET_ALL_ORDER = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_GET_ALL_OR";
        private const string PROC_CPS_SECONDARY_GET_BY_DATE = schema + "PKG_CPS_SECONDARY.PROC_CPS_SECONDARY_GET_DATE";
        private const string PROC_SECONDARY_SUM_QUANTITY_ORDER = schema + "PKG_CPS_SECONDARY.PROC_SECONDARY_SUM_QUANTITY_OR";
        private const string PROC_SECONDARY_GET_BY_PARTNER = schema + "PKG_CPS_SECONDARY.PROC_SECONDARY_GET_BY_PARTNER";
        private const string PROC_SECONDARY_UPDATE_OVERVIEW_CONTENT = schema + "PKG_CPS_SECONDARY.PROC_SECONDARY_UPDATE_OVERVIEW_CONTENT";
        private const string PROC_TRADING_BANK_ACC_ADD = schema + "PKG_CPS_SECONDARY_TRADING_BANK_ACC.PROC_TRADING_BANK_ACC_ADD";
        private const string PROC_GET_ALL_TRADING_BANK_ACC = schema + "PKG_CPS_SECONDARY_TRADING_BANK_ACC.PROC_GET_ALL_TRADING_BANK_ACC";
        private const string PROC_TRADING_BANK_ACC_DELETE = schema + "PKG_CPS_SECONDARY_TRADING_BANK_ACC.PROC_TRADING_BANK_ACC_DELETE";


        #region App
        private const string PROC_APP_SECONDARY_FIND_ALL = schema + "PKG_CPS_SECONDARY.PROC_APP_SECONDARY_GET_ALL";
        private const string PROC_APP_SECONDARY_GET_BY_INFO = schema + "PKG_CPS_SECONDARY.PROC_APP_SECONDARY_GET_BY_INFO";
        private const string PROC_APP_CPS_POLICY_GET_ALL = schema + "PKG_CPS_SECONDARY.PROC_APP_CPS_POLICY_GET_ALL";
        private const string PROC_APP_POLICY_DETAIL_FIND = schema + "PKG_CPS_SECONDARY.PROC_APP_POLICY_DETAIL_FIND";
        private const string PROC_POLICY_DETAIL_BY_PROFIT = schema + "PKG_CPS_SECONDARY.PROC_POLICY_DETAIL_BY_PROFIT";
        #endregion
        public CpsSecondaryRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public int CpsSecondaryRequest(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_SECONDARY_REQUEST, new
            {
                pv_CPS_SECONDARY_ID = id,
            });
            return result;
        }

        public int CpsSecondaryApprove(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_SECONDARY_APPROVE, new
            {
                pv_CPS_SECONDARY_ID = id
            });
            return result;
        }

        public int CpsSecondaryCheck(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_SECONDARY_CHECK, new
            {
                pv_CPS_SECONDARY_ID = id
            });
            return result;
        }

        public int CpsSecondaryCancel(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_SECONDARY_CANCEL, new
            {
                pv_CPS_SECONDARY_ID = id
            });
            return result;
        }

        public CpsSecondary Add(CpsSecondary entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsSecondary>(
                    PROC_ADD_SECONDARY, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BUSINESS_CUSTOMER_BANK_ID = entity.BusinessCustomerBankAccId,
                        pv_QUANTITY = entity.Quantity,
                        pv_OPEN_CELL_DATE = entity.OpenCellDate,
                        pv_CLOSE_CELL_DATE = entity.CloseCellDate,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public CpsPolicy AddPolicy(CpsPolicy entity)
        {

            return _oracleHelper.ExecuteProcedureToFirst<CpsPolicy>(
                    PROC_ADD_POLICY, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_SECONDARY_ID = entity.SecondaryId,
                        pv_CODE = entity.Code,
                        pv_NAME = entity.Name,
                        pv_TYPE = entity.Type,
                        pv_INVESTOR_TYPE = entity.InvestorType,
                        pv_INCOME_TAX = entity.IncomeTax,
                        pv_TRANSFER_TAX = entity.TransferTax,
                        pv_MIN_MONEY = entity.MinMoney,
                        pv_IS_TRANSFER = entity.IsTransfer,
                        pv_CLASSIFY = entity.Classify,
                        pv_START_DATE = entity.StartDate,
                        pv_END_DATE = entity.EndDate,
                        pv_DESCRIPTION = entity.Description,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public int AddPolicyDetail(CpsPolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_ADD_POLICY_DETAIL, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_POLICY_ID = entity.PolicyId,
                        pv_STT = entity.STT,
                        pv_SHORT_NAME = entity.ShortName,
                        pv_NAME = entity.Name,
                        pv_PERIOD_TYPE = entity.PeriodType,
                        pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                        pv_INTEREST_TYPE = entity.InterestType,
                        pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                        pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                        pv_PROFIT = entity.Profit,
                        pv_INTEREST_DAYS = entity.InterestDays,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public IEnumerable<CpsPolicy> GetAllPolicy(int cpsSecondaryId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<CpsPolicy>(PROC_GET_ALL_LIST_POLICY, new
            {
                pv_SECONDARY_ID = cpsSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status,
            });
        }

        public IEnumerable<CpsPolicy> GetAllPolicyOrder(int cpsSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<CpsPolicy>(PROC_GET_ALL_LIST_POLICY_ORDER, new
            {
                pv_SECONDARY_ID = cpsSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public IEnumerable<CpsPolicyDetail> GetAllPolicyDetail(int policyId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<CpsPolicyDetail>(PROC_GET_ALL_LIST_POLICY_DETAIL, new
            {
                pv_POLICY_ID = policyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status
            });
        }

        public PagingResult<CpsSecondary> FindAll(int pageSize, int pageNumber, string keyword, int? status, int? tradingProviderId, string isClose = null)
        {
            return _oracleHelper.ExecuteProcedurePaging<CpsSecondary>(PROC_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_IS_CLOSE = isClose
            });
        }

        public List<CpsSecondary> FindAllOrder(int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<CpsSecondary>(PROC_CPS_SECONDARY_GET_ALL_ORDER, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }).ToList();
            return result;
        }

        public CpsPolicy FindPolicyById(int id, int? tradingProviderId = null, string isShowApp = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsPolicy>(PROC_POLICY_GET, new
            {
                pv_POLICY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public CpsPolicyDetail FindPolicyDetailById(int id, int tradingProviderId, string isShowApp = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsPolicyDetail>(PROC_POLICY_DETAIL_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public CpsPolicyDetail FindPolicyDetailById(int id, bool isClose = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsPolicyDetail>(PROCT_CPS_POLICY_DETAIL_GET_WITH_OUT_TRADING_PROVIDER_ID, new
            {
                pv_ID = id
            }, isClose);
        }

        public CpsPolicyDetail GetPolicyDetailByIdWithoutTradingProviderId(int id, bool isClose = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsPolicyDetail>(PROCT_CPS_POLICY_DETAIL_GET_WITH_OUT_TRADING_PROVIDER_ID, new
            {
                pv_ID = id
            }, isClose);
        }

        public CpsSecondary FindSecondaryById(int id, int? tradingProviderId)
        {
            var rslt = _oracleHelper.ExecuteProcedureToFirst<CpsSecondary>(PROC_GET_ONE, new
            {
                pv_CPS_SECONDARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return rslt;
        }

        public List<CpsSecondary> FindSecondaryByDate(int? tradingProviderId, DateTime? startDate, DateTime? endDate, int? productCpsId)
        {
            var rslt = _oracleHelper.ExecuteProcedure<CpsSecondary>(PROC_CPS_SECONDARY_GET_BY_DATE, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_CPS_ID = productCpsId,
            }).ToList();
            return rslt;
        }

        public List<CpsSecondary> FindSecondaryByPartner(int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var rslt = _oracleHelper.ExecuteProcedure<CpsSecondary>(PROC_SECONDARY_GET_BY_PARTNER, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_PARTNER_ID = partnerId,
            }).ToList();
            return rslt;
        }
        public int Update(CpsSecondary entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_UPDATE, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_CPS_SECONDARY_ID = entity.Id,
                        pv_BUSINESS_CUSTOMER_BANK_ID = entity.BusinessCustomerBankAccId,
                        pv_QUANTITY = entity.Quantity,
                        pv_OPEN_CELL_DATE = entity.OpenCellDate,
                        pv_CLOSE_CELL_DATE = entity.CloseCellDate,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int IsClose(int cpsSecondaryId, string isClose, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_IS_CLOSE, new
            {
                pv_CPS_SECONDARY_ID = cpsSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_CLOSE = isClose
            });
        }

        public int IsShowApp(int cpsSecondaryId, string isShowApp, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_IS_SHOW_APP, new
            {
                pv_SECONDARY_ID = cpsSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public int PolicyIsShowApp(int policyId, string isShowApp, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_IS_SHOW_APP, new
            {
                pv_ID = policyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public int PolicyDetailIsShowApp(int policyDetailId, string isShowApp, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_DETAIL_IS_SHOW_APP, new
            {
                pv_ID = policyDetailId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public int SuperAdminApprove(int cpsSecondaryId, int status)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_SUPER_ADMIN_APPROVE, new
            {
                pv_CPS_SECONDARY_ID = cpsSecondaryId,
                pv_STATUS = status
            });
        }

        public int TradingProviderApprove(int cpsSecondaryId, int tradingProviderId, int status)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_PROVIDER_APPROVE, new
            {
                pv_CPS_SECONDARY_ID = cpsSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status
            });
        }

        public int TradingProviderSubmit(int cpsSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_PROVIDER_SUBMIT, new
            {
                pv_CPS_SECONDARY_ID = cpsSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int UpdatePolicy(CpsPolicy entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_UPDATE, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_POLICY_ID = entity.Id,
                        pv_CODE = entity.Code,
                        pv_NAME = entity.Name,
                        pv_TYPE = entity.Type,
                        pv_INVESTOR_TYPE = entity.InvestorType,
                        pv_INCOME_TAX = entity.IncomeTax,
                        pv_TRANSFER_TAX = entity.TransferTax,
                        pv_MIN_MONEY = entity.MinMoney,
                        pv_IS_TRANSFER = entity.IsTransfer,
                        pv_CLASSIFY = entity.Classify,
                        pv_STATUS = entity.Status,
                        pv_START_DATE = entity.StartDate,
                        pv_END_DATE = entity.EndDate,
                        pv_DESCRIPTION = entity.Description,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int UpdateStatusPolicy(CpsPolicy entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_CHANGE_STATUS, new
                    {
                        pv_POLICY_ID = entity.Id,
                        pv_STATUS = entity.Status,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int UpdateStatusPolicyDetail(CpsPolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_CHANGE_STATUS, new
                    {
                        pv_POLICY_DETAIL_ID = entity.Id,
                        pv_STATUS = entity.Status,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public int UpdatePolicyDetail(CpsPolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_UPDATE, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_ID = entity.Id,
                        pv_POLICY_ID = entity.PolicyId,
                        pv_STT = entity.STT,
                        pv_SHORT_NAME = entity.ShortName,
                        pv_NAME = entity.Name,
                        pv_PERIOD_TYPE = entity.PeriodType,
                        pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                        pv_INTEREST_TYPE = entity.InterestType,
                        pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                        pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                        pv_PROFIT = entity.Profit,
                        pv_INTEREST_DAYS = entity.InterestDays,
                        pv_STATUS = entity.Status,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int DeletePolicy(int policyId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_DELETE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ID = policyId,
            });
        }

        public int DeletePolicyDetail(int policyDetailId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_DETAIL_DELETE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ID = policyDetailId,
            });
        }

        public IEnumerable<CpsInfoSecondaryFindDto> FindAllProduct(string keyword, int? investorId)
        {
            return _oracleHelper.ExecuteProcedure<CpsInfoSecondaryFindDto>(PROC_APP_SECONDARY_FIND_ALL, new
            {
                KEY_WORD = keyword,
                pv_INVESTOR_ID = investorId
            });
        }

        public AppCpsInfoDto AppFindSecondaryById(int id, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppCpsInfoDto>(PROC_APP_SECONDARY_GET_BY_INFO, new
            {
                pv_CPS_SECONDARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<AppCpsPolicyDto> FindAllPolicy(int id, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppCpsPolicyDto>(PROC_APP_CPS_POLICY_GET_ALL, new
            {
                pv_CPS_SECONDARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<AppCpsPolicyDetailDto> FindAllPolicyDetail(int cpsPolicyId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppCpsPolicyDetailDto>(PROC_APP_POLICY_DETAIL_FIND, new
            {
                pv_CPS_POLICY_ID = cpsPolicyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public AppCpsPolicyDetailDto FindPolicyDetailMaxProfit(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppCpsPolicyDetailDto>(PROC_POLICY_DETAIL_BY_PROFIT, new
            {
                pv_CPS_SECONDARY_ID = id
            });
        }

        public int UpdatePolicyStatus(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_CHANGE_STATUS, new
                    {
                        pv_POLICY_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }
        public int UpdatePolicyDetailStatus(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_CHANGE_STATUS, new
                    {
                        pv_POLICY_DETAIL_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }

        public CpsSecondary FindSecondaryByPrimaryId(int cpsPrimaryId)
        {
            var rslt = _oracleHelper.ExecuteProcedureToFirst<CpsSecondary>(PROC_SECONDARY_GET_BY_PRIMARY, new
            {
                pv_CPS_PRIMARY_ID = cpsPrimaryId
            });
            return rslt;
        }

        /// <summary>
        /// ExportReport: TỔNG SỐ LƯỢNG CỦA LỆNH KHI ĐÃ ACTIVE
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="cpsPrimaryId"></param>
        /// <returns></returns>
        public decimal SumQuantity(int cpsSecondaryId, int classify)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_CLASSIFY", classify, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_CPS_SECONDARY_ID", cpsSecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SECONDARY_SUM_QUANTITY_ORDER, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }

        public int UpdateOverviewContent(UpdateCpsInfoOverviewContentDto input, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_SECONDARY_UPDATE_OVERVIEW_CONTENT, new
            {
                pv_CPS_SECONDARY_ID = input.SecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_OVERVIEW_IMAGE_URL = input.OverviewImageUrl,
                pv_CONTENT_TYPE = input.ContentType,
                pv_OVERVIEW_CONTENT = input.OverviewContent
            });
        }

        public int AddDistributionTradingBankAcc(SecondaryTradingBankAccount entity, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_TRADING_BANK_ACC_ADD, new
                    {
                        pv_SECONDARY_ID = entity.SecondaryId,
                        pv_BUSINESS_CUSTOMER_BACK_ACC_ID = entity.BusinessCustomerBankAccId,
                        SESSION_USERNAME = entity.CreatedBy,
                        pv_TRADING_PROVIDER_ID = tradingProviderId
                    }, false);
        }

        public List<SecondaryTradingBankAccount> GetAllTradingBankByDistribution(int distributionId)
        {
            var result = _oracleHelper.ExecuteProcedure<SecondaryTradingBankAccount>(PROC_GET_ALL_TRADING_BANK_ACC, new
            {
                pv_DISTRIBUTION_ID = distributionId
            }).ToList();
            return result;
        }

        public int DeletedDistributionTradingBankAcc(int id)
        {
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_BANK_ACC_DELETE, new
            {
                pv_ID = id
            });
            return rslt;
        }
    }
}

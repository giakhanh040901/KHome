using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Policy;
using EPIC.DataAccess;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesRepositories
{
    public class CpsPolicyRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_POLICY_ADD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_cps_POLICY.PROC_POLICY_ADD";
        private const string PROC_POLICY_UPDATE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_UPDATE";
        private const string PROC_POLICY_DELETE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DELETE";
        private const string PROC_POLICY_GET_ALL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_GET_ALL";
        private const string PROC_POLICY_GET = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_GET";
        private const string PROC_POLICY_GET_ALL_ORDER = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_GET_ALL_ORDER";

        private const string PROC_ADD_POLICY_DE_RETURN = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DE_ADD_RE";
        private const string PROC_POLICY_DETAIL_ADD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DETAIL_ADD";
        private const string PROC_POLICY_DETAIL_UPDATE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DETAIL_UPDATE";
        private const string PROC_POLICY_DETAIL_DELETE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DETAIL_DELETE";
        private const string PROC_POLICY_DETAIL_GET_ALL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DETAIL_GET_ALL";
        private const string PROC_POLICY_DETAIL_GET = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DETAIL_GET";
        private const string PROC_CHANGE_STATUS_POLICY = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_RE_STATUS";
        private const string PROC_CHANGE_STATUS_POLICY_DETAIL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DE_RE_STATUS";

        private const string PROC_POLICY_IS_SHOW_APP = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_IS_SHOW_APP";
        private const string PROC_POLICY_DETAIL_IS_SHOW_APP = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DETAIL_IS_SHOW_APP";

        #region App
        private const string PROC_POLICY_DETAIL_BY_PROFIT = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_POLICY_DETAIL_BY_PROFIT";
        private const string PROC_APP_POLICY_GET_ALL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_APP_POLICY_GET_ALL";
        private const string PROC_APP_POLICY_DETAIL_FIND = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_INV_POLICY.PROC_APP_POLICY_DETAIL_FIND";
        #endregion

        public CpsPolicyRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public Policy AddPolicy(Policy entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Policy>(
                    PROC_POLICY_ADD, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_SCONDARY_ID = entity.SecondaryId,
                        pv_CODE = entity.Code,
                        pv_NAME = entity.Name,
                        pv_TYPE = entity.Type,
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

        public int UpdatePolicy(Policy entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_UPDATE, new
                    {
                        pv_ID = entity.Id,
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_CODE = entity.Code,
                        pv_NAME = entity.Name,
                        pv_TYPE = entity.Type,
                        pv_INCOME_TAX = entity.IncomeTax,
                        pv_TRANSFER_TAX = entity.TransferTax,
                        pv_MIN_MONEY = entity.MinMoney,
                        pv_IS_TRANSFER = entity.IsTransfer,
                        pv_CLASSIFY = entity.Classify,
                        pv_START_DATE = entity.StartDate,
                        pv_END_DATE = entity.EndDate,
                        pv_DESCRIPTION = entity.Description,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int DeletePolicy(int id, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_DELETE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ID = id,
            });
        }

        public Policy FindPolicyById(int id, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Policy>(PROC_POLICY_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<Policy> GetAllPolicy(int secondaryId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<Policy>(PROC_POLICY_GET_ALL, new
            {
                pv_SECONDARY_ID = secondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status,
            });
        }

        public IEnumerable<Policy> GetAllPolicyOrder(int secondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<Policy>(PROC_POLICY_GET_ALL_ORDER, new
            {
                pv_SECONDARY_ID = secondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public int AddPolicyDetail(PolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_ADD, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_POLICY_ID = entity.PolicyId,
                        pv_STT = entity.STT,
                        pv_SHORT_NAME = entity.ShortName,
                        pv_NAME = entity.Name,
                        pv_PERIOD_TYPE = entity.PeriodType,
                        pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                        pv_PROFIT = entity.Profit,
                        pv_INTEREST_DAYS = entity.InterestDays,
                        pv_INTEREST_TYPE = entity.InterestType,
                        pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                        pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public PolicyDetail AddPolicyDetailReturnResult(PolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<PolicyDetail>(
                    PROC_ADD_POLICY_DE_RETURN, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_POLICY_ID = entity.PolicyId,
                        pv_STT = entity.STT,
                        pv_SHORT_NAME = entity.ShortName,
                        pv_NAME = entity.Name,
                        pv_PERIOD_TYPE = entity.PeriodType,
                        pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                        pv_PROFIT = entity.Profit,
                        pv_INTEREST_DAYS = entity.InterestDays,
                        pv_INTEREST_TYPE = entity.InterestType,
                        pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                        pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public int UpdatePolicyDetail(PolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_UPDATE, new
                    {
                        pv_ID = entity.Id,
                        pv_POLICY_ID = entity.PolicyId,
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_STT = entity.STT,
                        pv_SHORT_NAME = entity.ShortName,
                        pv_NAME = entity.Name,
                        pv_PERIOD_TYPE = entity.PeriodType,
                        pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                        pv_PROFIT = entity.Profit,
                        pv_INTEREST_DAYS = entity.InterestDays,
                        pv_INTEREST_TYPE = entity.InterestType,
                        pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                        pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int DeletePolicyDetail(int policyDetailId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_DETAIL_DELETE, new
            {
                pv_ID = policyDetailId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public IEnumerable<PolicyDetail> GetAllPolicyDetail(int policyId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<PolicyDetail>(PROC_POLICY_DETAIL_GET_ALL, new
            {
                pv_POLICY_ID = policyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status
            });
        }


        public PolicyDetail FindPolicyDetailById(int id, int? tradingProviderId = null, bool isClose = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<PolicyDetail>(PROC_POLICY_DETAIL_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }, isClose);
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

        public int UpdateStatusPolicy(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_CHANGE_STATUS_POLICY, new
                    {
                        pv_POLICY_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }

        public int UpdateStatusPolicyDetail(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_CHANGE_STATUS_POLICY_DETAIL, new
                    {
                        pv_POLICY_DETAIL_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }

        public AppCpsPolicyDetailDto FindPolicyDetailMaxProfit(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppCpsPolicyDetailDto>(PROC_POLICY_DETAIL_BY_PROFIT, new
            {
                pv_SECONDARY_ID = id
            });
        }

        public IEnumerable<AppCpsPolicyDto> FindAllProjectPolicy(int id, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppCpsPolicyDto>(PROC_APP_POLICY_GET_ALL, new
            {
                pv_SECONDARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<AppCpsPolicyDetailDto> FindAllProjectPolicyDetail(int policyId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppCpsPolicyDetailDto>(PROC_APP_POLICY_DETAIL_FIND, new
            {
                pv_POLICY_ID = policyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }
    }
}

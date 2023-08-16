using EPIC.CompanySharesEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesRepositories
{
    public class CpsInfoRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string ADD_COMPANY_SHARES_INFO_PROC = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_ADD";
        private static string UPDATE_COMPANY_SHARES_INFO_PROC = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_UPDATE";
        private static string GET_ALL_COMPANY_SHARES_INFO_PROC = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_FIND";
        private static string GET_COMPANY_SHARES_INFO_PROC = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_GET";
        private static string DELETE_COMPANY_SHARES_INFO_PROC = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_DELETE";
        private const string PROC_COMPANY_SHARES_INFO_REQUEST = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_REQUEST";
        private const string PROC_COMPANY_SHARES_INFO_APPROVE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_APPROVE";
        private const string PROC_COMPANY_SHARES_INFO_CHECK = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_CHECK";
        private const string PROC_COMPANY_SHARES_INFO_ACTIVE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_ACTIVE";
        private const string PROC_COMPANY_SHARES_INFO_CANCEL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_CANCEL";
        private const string PROC_COMPANY_SHARES_INFO_CLOSE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_INFO.PROC_CPS_INFO_CLOSE";

        public CpsInfoRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public int CompanySharesInfoRequest(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_COMPANY_SHARES_INFO_REQUEST, new
            {
                pv_CPS_INFO_ID = id,
            });
            return result;
        }

        public int CompanySharesInfoApprove(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_COMPANY_SHARES_INFO_APPROVE, new
            {
                pv_CPS_INFO_ID = id
            });
            return result;
        }

        public int CompanySharesInfoCheck(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_COMPANY_SHARES_INFO_CHECK, new
            {
                pv_CPS_INFO_ID = id
            });
            return result;
        }

        public int CompanySharesInfoCancel(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_COMPANY_SHARES_INFO_CANCEL, new
            {
                pv_CPS_INFO_ID = id
            });
            return result;
        }

        public int CompanySharesInfoClose(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_COMPANY_SHARES_INFO_CLOSE, new
            {
                pv_CPS_INFO_ID = id
            });
            return result;
        }

        public PagingResult<CpsInfo> FindAllCompanySharesInfo(int pageSize, int pageNumber, string keyword, string status, int partnerId, string isCheck, DateTime? issueDate, DateTime? dueDate)
        {
            var companySharesInfo = _oracleHelper.ExecuteProcedurePaging<CpsInfo>(GET_ALL_COMPANY_SHARES_INFO_PROC, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_PARTNER_ID = partnerId,
                pv_IS_CHECK = isCheck,
                pv_ISSUE_DATE = issueDate,
                pv_DUE_DATE = dueDate,
            });
            return companySharesInfo;
        }

        public CpsInfo FindById(int id)
        {
            CpsInfo companySharesInfo = _oracleHelper.ExecuteProcedureToFirst<CpsInfo>(GET_COMPANY_SHARES_INFO_PROC, new
            {
                pv_CPS_ID = id,
            });
            return companySharesInfo;
        }

        public CpsInfo Add(CpsInfo entity)
        {
            _logger.LogInformation("Add Company Shares Info");
            return _oracleHelper.ExecuteProcedureToFirst<CpsInfo>(ADD_COMPANY_SHARES_INFO_PROC, new
            {
                pv_ISSUER_ID = entity.IssuerId,
                pv_CPS_CODE = entity.CpsCode,
                pv_CPS_NAME = entity.CpsName,
                pv_DESCRIPTION = entity.Description,
                pv_CONTENT = entity.Content,
                pv_ISSUE_DATE = entity.IssueDate,
                pv_DUE_DATE = entity.DueDate,
                pv_PAR_VALUE = entity.ParValue,
                pv_PERIOD = entity.Period,
                pv_PERIOD_UNIT = entity.PeriodUnit,
                pv_INTEREST_RATE = entity.InterestRate,
                pv_INTEREST_PERIOD = entity.InterestPeriod,
                pv_INTEREST_PERIOD_UNIT = entity.InterestPeriodUnit,
                pv_INTEREST_RATE_TYPE = entity.InterestRateType,
                pv_IS_PAYMENT_GUARANTEE = entity.IsPaymentGuarantee,
                pv_IS_ALLOW_SBD = entity.IsAllowSbd,
                pv_ALLOW_SBD_DAY = entity.AllowSbdDay,
                pv_IS_COLLATERAL = entity.IsCollateral,
                pv_MAX_INVESTOR = entity.MaxInvestor,
                pv_NUMBER_CLOSE_PER = entity.NumberClosePer,
                pv_COUNT_TYPE = entity.CountType,
                pv_IS_LISTING = entity.IsListing,
                pv_QUANTITY = entity.Quantity,
                pv_FEE_RATE = entity.FeeRate,
                pv_IS_CHECK = entity.IsCheck,
                pv_POLICY_PAYMENT_CONTENT = entity.PolicyPaymentContent,
                pv_PARTNER_ID = entity.PartnerId,
                pv_ICON = entity.Icon,
                pv_TOTAL_INVESTMENT_DISPLAY = entity.TotalInvestment,
                pv_HAS_TOTAL_INVESTMENT_SUB = entity.HasTotalInvestmentSub,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }

        public int Update(CpsInfo entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_COMPANY_SHARES_INFO_PROC, new
            {
                pv_CPS_ID = entity.Id,
                pv_ISSUER_ID = entity.IssuerId,
                pv_CPS_CODE = entity.CpsCode,
                pv_CPS_NAME = entity.CpsName,
                pv_DESCRIPTION = entity.Description,
                pv_CONTENT = entity.Content,
                pv_ISSUE_DATE = entity.IssueDate,
                pv_DUE_DATE = entity.DueDate,
                pv_PAR_VALUE = entity.ParValue,
                pv_PERIOD = entity.Period,
                pv_PERIOD_UNIT = entity.PeriodUnit,
                pv_INTEREST_RATE = entity.InterestRate,
                pv_INTEREST_PERIOD = entity.InterestPeriod,
                pv_INTEREST_PERIOD_UNIT = entity.InterestPeriodUnit,
                pv_INTEREST_RATE_TYPE = entity.InterestRateType,
                pv_IS_PAYMENT_GUARANTEE = entity.IsPaymentGuarantee,
                pv_IS_ALLOW_SBD = entity.IsAllowSbd,
                pv_ALLOW_SBD_DAY = entity.AllowSbdDay,
                pv_IS_COLLATERAL = entity.IsCollateral,
                pv_MAX_INVESTOR = entity.MaxInvestor,
                pv_NUMBER_CLOSE_PER = entity.NumberClosePer,
                pv_COUNT_TYPE = entity.CountType,
                pv_IS_LISTING = entity.IsListing,
                pv_QUANTITY = entity.Quantity,
                pv_FEE_RATE = entity.FeeRate,
                pv_IS_CHECK = entity.IsCheck,
                pv_STATUS = entity.Status,
                pv_POLICY_PAYMENT_CONTENT = entity.PolicyPaymentContent,
                pv_PARTNER_ID = entity.PartnerId,
                pv_ICON = entity.Icon,
                pv_TOTAL_INVESTMENT_DISPLAY = entity.TotalInvestment,
                pv_HAS_TOTAL_INVESTMENT_SUB = entity.HasTotalInvestmentSub,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int Delete(int id, int partnerId)
        {
            _logger.LogInformation($"Delete Company Shares Info: {id}");
            return _oracleHelper.ExecuteProcedureNonQuery(DELETE_COMPANY_SHARES_INFO_PROC, new
            {
                pv_CPS_ID = id,
                pv_PARTNER_ID = partnerId
            });
        }
    }
}

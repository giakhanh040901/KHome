using DocumentFormat.OpenXml.Drawing.Charts;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestPolicyRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_POLICY_ADD = "PKG_INV_POLICY.PROC_POLICY_ADD";
        private const string PROC_POLICY_UPDATE = "PKG_INV_POLICY.PROC_POLICY_UPDATE";
        private const string PROC_POLICY_DELETE = "PKG_INV_POLICY.PROC_POLICY_DELETE";
        private const string PROC_POLICY_GET_ALL = "PKG_INV_POLICY.PROC_POLICY_GET_ALL";
        private const string PROC_POLICY_GET_ALL_PAGING_RESULT = "PKG_INV_POLICY.PROC_POLICY_GET_ALL_PAGING_RESULT";
        private const string PROC_POLICY_GET = "PKG_INV_POLICY.PROC_POLICY_GET";
        private const string PROC_POLICY_GET_ALL_ORDER = "PKG_INV_POLICY.PROC_POLICY_GET_ALL_ORDER";

        private const string PROC_ADD_POLICY_DE_RETURN = "PKG_INV_POLICY.PROC_POLICY_DE_ADD_RE";
        private const string PROC_POLICY_DETAIL_ADD = "PKG_INV_POLICY.PROC_POLICY_DETAIL_ADD";
        private const string PROC_POLICY_DETAIL_UPDATE = "PKG_INV_POLICY.PROC_POLICY_DETAIL_UPDATE";
        private const string PROC_POLICY_DETAIL_DELETE = "PKG_INV_POLICY.PROC_POLICY_DETAIL_DELETE";
        private const string PROC_POLICY_DETAIL_GET_ALL = "PKG_INV_POLICY.PROC_POLICY_DETAIL_GET_ALL";
        private const string PROC_POLICY_DETAIL_GET = "PKG_INV_POLICY.PROC_POLICY_DETAIL_GET";
        private const string PROC_CHANGE_STATUS_POLICY_DETAIL = "PKG_INV_POLICY.PROC_POLICY_DE_RE_STATUS";

        private const string PROC_POLICY_IS_SHOW_APP = "PKG_INV_POLICY.PROC_POLICY_IS_SHOW_APP";
        private const string PROC_POLICY_DETAIL_IS_SHOW_APP = "PKG_INV_POLICY.PROC_POLICY_DETAIL_IS_SHOW_APP";

        #region App
        private const string PROC_POLICY_DETAIL_BY_PROFIT = "PKG_INV_POLICY.PROC_POLICY_DETAIL_BY_PROFIT";
        private const string PROC_APP_POLICY_GET_ALL = "PKG_INV_POLICY.PROC_APP_POLICY_GET_ALL";
        private const string PROC_APP_POLICY_DETAIL_FIND = "PKG_INV_POLICY.PROC_APP_POLICY_DETAIL_FIND";
        #endregion

        public InvestPolicyRepository(string connectionString, ILogger logger)
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
                        pv_DISTRIBUTION_ID = entity.DistributionId,
                        pv_CODE = entity.Code,
                        pv_NAME = entity.Name,
                        pv_TYPE = entity.Type,
                        pv_INCOME_TAX = entity.IncomeTax,
                        pv_TRANSFER_TAX = entity.TransferTax,
                        pv_MIN_MONEY = entity.MinMoney,
                        pv_MAX_MONEY = entity.MaxMoney,
                        pv_IS_TRANSFER = entity.IsTransfer,
                        pv_CLASSIFY = entity.Classify,
                        pv_START_DATE = entity.StartDate,
                        pv_END_DATE = entity.EndDate,
                        pv_DESCRIPTION = entity.Description,
                        pv_MIN_WITHDRAW = entity.MinWithDraw,
                        pv_CALCULATE_TYPE = entity.CalculateType,
                        pv_IS_SHOW_APP = entity.IsShowApp,
                        pv_EXIT_FEE = entity.ExitFee,
                        pv_EXIT_FEE_TYPE = entity.ExitFeeType,
                        pv_POLICY_DISPLAY_ORDER = entity.PolicyDisplayOrder,
                        pv_RENEWALS_TYPE = entity.RenewalsType,
                        pv_REMIND_RENEWALS = entity.RemindRenewals,
                        pv_EXPIRATION_RENEWALS = entity.ExpirationRenewals,
                        pv_MAX_WITH_DRAW = entity.MaxWithDraw,
                        pv_MIN_TAKE_CONTRACT = entity.MinTakeContract,
                        pv_MIN_INVEST_DAY = entity.MinInvestDay,
                        pv_PROFIT_RATE_DEFAULT = entity.ProfitRateDefault,
                        pv_CAL_WITHDRAWAL_TYPE = entity.CalculateWithdrawType,
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
                        pv_MAX_MONEY = entity.MaxMoney,
                        pv_IS_TRANSFER = entity.IsTransfer,
                        pv_CLASSIFY = entity.Classify,
                        pv_START_DATE = entity.StartDate,
                        pv_END_DATE = entity.EndDate,
                        pv_DESCRIPTION = entity.Description,
                        pv_MIN_WITHDRAW = entity.MinWithDraw,
                        pv_CALCULATE_TYPE = entity.CalculateType,
                        pv_IS_SHOW_APP = entity.IsShowApp,
                        pv_EXIT_FEE = entity.ExitFee,
                        pv_EXIT_FEE_TYPE = entity.ExitFeeType,
                        pv_POLICY_DISPLAY_ORDER = entity.PolicyDisplayOrder,
                        pv_RENEWALS_TYPE = entity.RenewalsType,
                        pv_REMIND_RENEWALS = entity.RemindRenewals,
                        pv_EXPIRATION_RENEWALS = entity.ExpirationRenewals,
                        pv_MAX_WITH_DRAW = entity.MaxWithDraw,
                        pv_MIN_TAKE_CONTRACT = entity.MinTakeContract,
                        pv_MIN_INVEST_DAY = entity.MinInvestDay,
                        pv_PROFIT_RATE_DEFAULT = entity.ProfitRateDefault,
                        pv_CAL_WITHDRAWAL_TYPE = entity.CalculateWithdrawType,
                        SESSION_USERNAME = entity.ModifiedBy,
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

        public Policy FindPolicyById(int id, int? tradingProviderId = null, bool closeConnection = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Policy>(PROC_POLICY_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }, closeConnection);
        }

        public IEnumerable<Policy> GetAllPolicy(int distributionId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<Policy>(PROC_POLICY_GET_ALL, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status,
            });
        }

        public PagingResult<Policy> GetAllPolicyPagingResult(int tradingProviderId, InvestPolicyFilterDto input)
        {
            return _oracleHelper.ExecuteProcedurePaging<Policy>(PROC_POLICY_GET_ALL_PAGING_RESULT, new
            {
                PAGE_SIZE = input.PageSize,
                PAGE_NUMBER = input.PageNumber,
                KEYWORD = input.Keyword,
                pv_DISTRIBUTION_ID = input.DistributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = input.Status,
            });
        }

        public IEnumerable<Policy> GetAllPolicyOrder(int distributionId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<Policy>(PROC_POLICY_GET_ALL_ORDER, new
            {
                pv_DISTRIBUTION_ID = distributionId,
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
                        pv_FIXED_PAYMENT_DATE = entity.FixedPaymentDate,
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
                        pv_FIXED_PAYMENT_DATE = entity.FixedPaymentDate,
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
                        pv_FIXED_PAYMENT_DATE = entity.FixedPaymentDate,
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

        public IEnumerable<PolicyDetail> GetAllPolicyDetail(int bondPolicyId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<PolicyDetail>(PROC_POLICY_DETAIL_GET_ALL, new
            {
                pv_POLICY_ID = bondPolicyId,
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

        public AppInvestPolicyDetailDto FindPolicyDetailMaxProfit(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppInvestPolicyDetailDto>(PROC_POLICY_DETAIL_BY_PROFIT, new
            {
                pv_DISTRIBUTION_ID = id
            });
        }

        public IEnumerable<AppInvestPolicyDto> FindAllProjectPolicy(int id, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppInvestPolicyDto>(PROC_APP_POLICY_GET_ALL, new
            {
                pv_DISTRIBUTION_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<AppInvestPolicyDetailDto> FindAllProjectPolicyDetail(int policyId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppInvestPolicyDetailDto>(PROC_APP_POLICY_DETAIL_FIND, new
            {
                pv_POLICY_ID = policyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }
    }
}

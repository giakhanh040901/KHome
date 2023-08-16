using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.PolicyTemp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class PolicyTempRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private static string PROC_ADD_POLICY_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_ADD";
        private static string PROC_ADD_POLICY_DETAIL_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_DE_TEMP_ADD";
        private static string PROC_UPDATE_POLICY_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_UPDATE";
        private static string PROC_UPDATE_POLICY_DETAIL_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_DE_TEMP_UPDATE";
        private static string PROC_CHANGE_STATUS_POLICY_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_RE_STATUS";
        private static string PROC_CHANGE_STATUS_POLICY_DETAIL_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_DE_TEMP_RE_STATUS";
        private static string PROC_DELETE_POLICY_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_DELETE";
        private static string PROC_DELETE_POLICY_DETAIL_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_DE_TEMP_DELETE";
        private static string PROC_GET_POLICY_DETAIL_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_DE_TEMP_GET";
        private static string PROC_GET_POLICY_TEMP = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_GET";
        private static string PROC_GET_ALL = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_GET_ALL";
        private static string PROC_GET_BY_ID = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_FIND";
        private static string PROC_GET_POLICY_DETAIL_TEMP_BY_POLICY_TEMP = "PKG_INV_POLICY_TEMP.PROC_POL_DETAIL_TEMP_ALL";
        private static string PROC_GET_ALL_NO_PERMISSION = "PKG_INV_POLICY_TEMP.PROC_POLICY_TEMP_GET_ALL_NO_PERMISSION";

        public PolicyTempRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }
        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public PagingResult<ViewPolicyTemp> FindAllPolicyTemp(int pageSize, int pageNumber, string keyword, string status, decimal? classify, int? type, int? tradingProviderId = null)
        {
            var productBondPolicyTemp = _oracleHelper.ExecuteProcedurePaging<ViewPolicyTemp>(PROC_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_CLASSIFY = classify,
                pv_TYPE = type,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return productBondPolicyTemp;
        }


        /// <summary>
        /// Lấy Chính sách theo Id kèm theo kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ViewPolicyTemp> FindById(int id)
        {
            List<ViewPolicyTemp> productBondPolicyTempView = _oracleHelper.ExecuteProcedure<ViewPolicyTemp>(PROC_GET_BY_ID, new
            {
                pv_POLICY_TEMP_ID = id,
            }).ToList();
            return productBondPolicyTempView;
        }

        public PolicyTemp FindPolicyTempById(int id)
        {
            PolicyTemp productBondPolicyTemp = _oracleHelper.ExecuteProcedureToFirst<PolicyTemp>(PROC_GET_POLICY_TEMP, new
            {
                pv_POLICY_TEMP_ID = id,
            });
            return productBondPolicyTemp;
        }

        public PolicyDetailTemp FindPolicyDetailTempById(int id)
        {
            PolicyDetailTemp productBondPolicyDetailTemp = _oracleHelper.ExecuteProcedureToFirst<PolicyDetailTemp>(PROC_GET_POLICY_DETAIL_TEMP, new
            {
                pv_POLICY_DETAIL_TEMP_ID = id,
            });
            return productBondPolicyDetailTemp;
        }

        public List<PolicyDetailTemp> FindBondPolicyDetailTempByPolicyTempId(int id)
        {
            List<PolicyDetailTemp> productBondPolicyDetailTemp = _oracleHelper.ExecuteProcedure<PolicyDetailTemp>(PROC_GET_POLICY_DETAIL_TEMP_BY_POLICY_TEMP, new
            {
                pv_POLICY_TEMP_ID = id
            }).ToList();
            return productBondPolicyDetailTemp;
        }



        public PolicyTemp AddPolicyTemp(PolicyTemp entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<PolicyTemp>(
                    PROC_ADD_POLICY_TEMP, new
                    {
                        pv_CODE = entity.Code,
                        pv_NAME = entity.Name,
                        pv_TYPE = entity.Type,
                        pv_INCOME_TAX = entity.IncomeTax,
                        pv_MIN_MONEY = entity.MinMoney,
                        pv_MAX_MONEY = entity.MaxMoney,
                        pv_IS_TRANSFER = entity.IsTransfer,
                        pv_TRANSFER_TAX = entity.TransferTax,
                        pv_CLASSIFY = entity.Classify,
                        pv_PROFIT_RATE_DEFAULT = entity.ProfitRateDefault,
                        pv_CAL_WITHDRAWAL_TYPE = entity.CalculateWithdrawType,
                        pv_MIN_WITHDRAW = entity.MinWithdraw,
                        pv_CALCULATE_TYPE = entity.CalculateType,
                        pv_EXIT_FEE = entity.ExitFee,
                        pv_EXIT_FEE_TYPE = entity.ExitFeeType,
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_DESCRIPTION = entity.Description,
                        pv_POLICY_DISPLAY_ORDER = entity.PolicyDisplayOrder,
                        pv_RENEWALS_TYPE = entity.RenewalsType,
                        pv_REMIND_RENEWALS = entity.RemindRenewals,
                        pv_EXPIRATION_RENEWALS = entity.ExpirationRenewals,
                        pv_MAX_WITH_DRAW = entity.MaxWithDraw,
                        pv_MIN_TAKE_CONTRACT = entity.MinTakeContract,
                        pv_MIN_INVEST_DAY = entity.MinInvestDay,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }



        public PolicyDetailTemp AddPolicyDetailTemp(PolicyDetailTemp entity)
        {

            return _oracleHelper.ExecuteProcedureToFirst<PolicyDetailTemp>(
                    PROC_ADD_POLICY_DETAIL_TEMP, new
                    {
                        pv_POLICY_TEMP_ID = entity.PolicyTempId,
                        pv_NAME = entity.Name,
                        pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                        pv_PERIOD_TYPE = entity.PeriodType,
                        pv_SHORT_NAME = entity.ShortName,
                        pv_PROFIT = entity.Profit,
                        pv_INTEREST_DAYS = entity.InterestDays,
                        pv_STT = entity.STT,
                        pv_INTEREST_TYPE = entity.InterestType,
                        pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                        pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                        pv_FIXED_PAYMENT_DATE = entity.FixedPaymentDate,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public int UpdatePolicyTemp(PolicyTemp entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_POLICY_TEMP, new
            {
                pv_POLICY_TEMP_ID = entity.Id,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_TYPE = entity.Type,
                pv_INCOME_TAX = entity.IncomeTax,
                pv_MIN_MONEY = entity.MinMoney,
                pv_MAX_MONEY = entity.MaxMoney,
                pv_IS_TRANSFER = entity.IsTransfer,
                pv_TRANSFER_TAX = entity.TransferTax,
                pv_CLASSIFY = entity.Classify,
                pv_PROFIT_RATE_DEFAULT = entity.ProfitRateDefault,
                pv_CAL_WITHDRAWAL_TYPE = entity.CalculateWithdrawType,
                pv_MIN_WITHDRAW = entity.MinWithdraw,
                pv_CALCULATE_TYPE = entity.CalculateType,
                pv_EXIT_FEE = entity.ExitFee,
                pv_EXIT_FEE_TYPE = entity.ExitFeeType,
                pv_DESCRIPTION = entity.Description,
                pv_POLICY_DISPLAY_ORDER = entity.PolicyDisplayOrder,
                pv_RENEWALS_TYPE = entity.RenewalsType,
                pv_REMIND_RENEWALS = entity.RemindRenewals,
                pv_EXPIRATION_RENEWALS = entity.ExpirationRenewals,
                pv_MAX_WITH_DRAW = entity.MaxWithDraw,
                pv_MIN_TAKE_CONTRACT = entity.MinTakeContract,
                pv_MIN_INVEST_DAY = entity.MinInvestDay,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int UpdatePolicyDetailTemp(PolicyDetailTemp entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_UPDATE_POLICY_DETAIL_TEMP, new
            {
                pv_POLICY_DETAIL_TEMP_ID = entity.Id,
                pv_NAME = entity.Name,
                pv_PERIOD_QUANTITY = entity.PeriodQuantity,
                pv_PERIOD_TYPE = entity.PeriodType,
                pv_SHORT_NAME = entity.ShortName,
                pv_PROFIT = entity.Profit,
                pv_INTEREST_DAYS = entity.InterestDays,
                pv_STT = entity.STT,
                pv_INTEREST_TYPE = entity.InterestType,
                pv_INTEREST_PERIOD_QUANTITY = entity.InterestPeriodQuantity,
                pv_INTEREST_PERIOD_TYPE = entity.InterestPeriodType,
                pv_FIXED_PAYMENT_DATE = entity.FixedPaymentDate,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int DeletePolicyTemp(int id)
        {
            _logger.LogInformation("Delete Product Bond Policy Template - SQL: {}", PROC_DELETE_POLICY_TEMP);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DELETE_POLICY_TEMP, new
            {
                pv_POLICY_TEMP_ID = id
            });
            return result;
        }

        public int DeletePolicyDetailTemp(int id)
        {
            _logger.LogInformation("Delete Product Bond Policy Detail Template - SQL: {}", PROC_DELETE_POLICY_DETAIL_TEMP);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DELETE_POLICY_DETAIL_TEMP, new
            {
                pv_POLICY_DETAIL_TEMP_ID = id
            });
            return result;
        }

        public int UpdateStatusPolicyTemp(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_CHANGE_STATUS_POLICY_TEMP, new
                    {
                        pv_POLICY_TEMP_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }

        public int UpdateStatusPolicyDetailTemp(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_CHANGE_STATUS_POLICY_DETAIL_TEMP, new
                    {
                        pv_POLICY_DETAIL_TEMP_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }
        public PagingResult<ViewPolicyTemp> FindAllPolicyTempNoPermission(int? tradingProviderId = null, string status = null)
        {
            var productPolicyTemp = _oracleHelper.ExecuteProcedurePaging<ViewPolicyTemp>(PROC_GET_ALL_NO_PERMISSION, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status
            });
            return productPolicyTemp;
        }
    }
}

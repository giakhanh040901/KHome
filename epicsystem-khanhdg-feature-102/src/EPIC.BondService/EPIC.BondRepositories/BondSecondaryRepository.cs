using Dapper.Oracle;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ProductBond;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EPIC.BondRepositories
{
    public class BondSecondaryRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string PROC_ADD_SECONDARY = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_ADD";
        private const string PROC_ADD_POLICY = "PKG_PRODUCT_BOND_SECONDARY.PROC_PRODUCT_BOND_POLICY_ADD";
        private const string PROC_ADD_POLICY_DE_RETURN = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_POLICY_DE_ADD_RE";
        private const string PROC_ADD_POLICY_DETAIL = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_POLICY_DETAIL_ADD";
        private const string PROC_UPDATE = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_UPDATE";
        private const string PROC_POLICY_CHANGE_STATUS = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_CHANGE_STATUS";
        private const string PROC_POLICY_DETAIL_CHANGE_STATUS = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICYDETAIL_CHANGESTATUS";
        private const string PROC_GET_ALL = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_GET_ALL";
        private const string PROC_GET_ONE = "PKG_PRODUCT_BOND_SECONDARY.PROCT_BOND_SECONDARY_GET";
        private const string PROC_POLICY_UPDATE = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_UPDATE";
        private const string PROC_POLICY_DETAIL_UPDATE = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_DETAIL_UPDATE";
        private const string PROC_GET_ALL_LIST_POLICY = "PKG_PRODUCT_BOND_SECONDARY.PROC_GET_ALL_LIST_POLICY";
        private const string PROC_GET_ALL_LIST_POLICY_DETAIL = "PKG_PRODUCT_BOND_SECONDARY.PROC_GET_ALL_LIST_POLICY_DE";
        private const string PROC_SUPER_ADMIN_APPROVE = "PKG_PRODUCT_BOND_SECONDARY.PROC_SUPER_ADMIN_APPROVE";
        private const string PROC_TRADING_PROVIDER_APPROVE = "PKG_PRODUCT_BOND_SECONDARY.PROC_TRADING_PROVIDER_APPROVE";
        private const string PROC_TRADING_PROVIDER_SUBMIT = "PKG_PRODUCT_BOND_SECONDARY.PROC_TRADING_PROVIDER_SUBMIT";
        private const string PROC_IS_CLOSE = "PKG_PRODUCT_BOND_SECONDARY.PROC_IS_CLOSE";
        private const string PROC_IS_SHOW_APP = "PKG_PRODUCT_BOND_SECONDARY.PROC_IS_SHOW_APP";
        private const string PROC_POLICY_IS_SHOW_APP = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_IS_SHOW_APP";
        private const string PROC_POLICY_DETAIL_IS_SHOW_APP = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_DE_IS_SHOW_APP";
        private const string PROC_POLICY_DELETE = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_DELETE";
        private const string PROC_POLICY_DETAIL_DELETE = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_DETAIL_DELETE";
        private const string PROC_POLICY_GET = "PKG_PRODUCT_BOND_SECONDARY.PROCT_BOND_POLICY_GET";
        private const string PROCT_BOND_POLICY_DETAIL_GET_WITH_OUT_TRADING_PROVIDER_ID = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_POLICY_DETAIL_GET_2";
        private const string PROC_POLICY_DETAIL_GET = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_POLICY_DETAIL_GET";
        private const string PROC_BOND_SECONDARY_REQUEST = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_REQUEST";
        private const string PROC_BOND_SECONDARY_APPROVE = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_APPROVE";
        private const string PROC_BOND_SECONDARY_CHECK = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_CHECK";
        private const string PROC_BOND_SECONDARY_CANCEL = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_CANCEL";
        private const string PROC_SECONDARY_GET_BY_PRIMARY = "PKG_PRODUCT_BOND_SECONDARY.PROC_SECONDARY_GET_BY_PRIMARY";
        private const string PROC_GET_ALL_LIST_POLICY_ORDER = "PKG_PRODUCT_BOND_SECONDARY.PROC_GET_ALL_LIST_POLICY_ORDER";
        private const string PROC_BOND_SECONDARY_GET_ALL_ORDER = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_GET_ALL_OR";
        private const string PROC_BOND_SECONDARY_GET_BY_DATE = "PKG_PRODUCT_BOND_SECONDARY.PROC_BOND_SECONDARY_GET_DATE";
        private const string PROC_SECONDARY_SUM_QUANTITY_ORDER = "PKG_PRODUCT_BOND_SECONDARY.PROC_SECONDARY_SUM_QUANTITY_OR";
        private const string PROC_SECONDARY_GET_BY_PARTNER = "PKG_PRODUCT_BOND_SECONDARY.PROC_SECONDARY_GET_BY_PARTNER";
        private const string PROC_SECONDARY_UPDATE_OVERVIEW_CONTENT = "PKG_PRODUCT_BOND_SECONDARY.PROC_SECONDARY_UPDATE_OVERVIEW_CONTENT";

        #region App
        private const string PROC_APP_SECONDARY_FIND_ALL = "PKG_PRODUCT_BOND_SECONDARY.PROC_APP_SECONDARY_GET_ALL";
        private const string PROC_APP_SECONDARY_GET_BY_INFO = "PKG_PRODUCT_BOND_SECONDARY.PROC_APP_SECONDARY_GET_BY_INFO";
        private const string PROC_APP_BOND_POLICY_GET_ALL = "PKG_PRODUCT_BOND_SECONDARY.PROC_APP_BOND_POLICY_GET_ALL";
        private const string PROC_APP_POLICY_DETAIL_FIND = "PKG_PRODUCT_BOND_SECONDARY.PROC_APP_POLICY_DETAIL_FIND";
        private const string PROC_POLICY_DETAIL_BY_PROFIT = "PKG_PRODUCT_BOND_SECONDARY.PROC_POLICY_DETAIL_BY_PROFIT";
        #endregion
        public BondSecondaryRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public int BondSecondaryRequest(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_SECONDARY_REQUEST, new
            {
                pv_BOND_SECONDARY_ID = id,
            });
            return result;
        }

        public int BondSecondaryApprove(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_SECONDARY_APPROVE, new
            {
                pv_BOND_SECONDARY_ID = id
            });
            return result;
        }

        public int BondSecondaryCheck(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_SECONDARY_CHECK, new
            {
                pv_BOND_SECONDARY_ID = id
            });
            return result;
        }

        public int BondSecondaryCancel(int id)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_SECONDARY_CANCEL, new
            {
                pv_BOND_SECONDARY_ID = id
            });
            return result;
        }

        public BondSecondary Add(BondSecondary entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondSecondary>(
                    PROC_ADD_SECONDARY, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BOND_PRIMARY_ID = entity.PrimaryId,
                        pv_BUSINESS_CUSTOMER_BANK_ID = entity.BusinessCustomerBankAccId,
                        pv_QUANTITY = entity.Quantity,
                        pv_OPEN_CELL_DATE = entity.OpenSellDate,
                        pv_CLOSE_CELL_DATE = entity.CloseSellDate,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public BondPolicy AddPolicy(BondPolicy entity)
        {

            return _oracleHelper.ExecuteProcedureToFirst<BondPolicy>(
                    PROC_ADD_POLICY, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BOND_SECONDARY_ID = entity.SecondaryId,
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

        public BondPolicyDetail AddPolicyDetailReturnResult(BondPolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondPolicyDetail>(
                    PROC_ADD_POLICY_DE_RETURN, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BOND_POLICY_ID = entity.PolicyId,
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

        public int AddPolicyDetail(BondPolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_ADD_POLICY_DETAIL, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BOND_POLICY_ID = entity.PolicyId,
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

        public IEnumerable<BondPolicy> GetAllPolicy(int bondSecondaryId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<BondPolicy>(PROC_GET_ALL_LIST_POLICY, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status,
            });
        }

        public IEnumerable<BondPolicy> GetAllPolicyOrder(int bondSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<BondPolicy>(PROC_GET_ALL_LIST_POLICY_ORDER, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public IEnumerable<BondPolicyDetail> GetAllPolicyDetail(int bondPolicyId, int tradingProviderId, string status = null)
        {
            return _oracleHelper.ExecuteProcedure<BondPolicyDetail>(PROC_GET_ALL_LIST_POLICY_DETAIL, new
            {
                pv_BOND_POLICY_ID = bondPolicyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status
            });
        }

        public PagingResult<BondSecondary> FindAll(int pageSize, int pageNumber, string keyword, int? status, int? tradingProviderId, string isClose = null)
        {
            return _oracleHelper.ExecuteProcedurePaging<BondSecondary>(PROC_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_IS_CLOSE = isClose
            });
        }

        public List<BondSecondary> FindAllOrder(int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<BondSecondary>(PROC_BOND_SECONDARY_GET_ALL_ORDER, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }).ToList();
            return result;
        }

        public BondPolicy FindPolicyById(int id, int? tradingProviderId = null, string isShowApp = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondPolicy>(PROC_POLICY_GET, new
            {
                pv_BOND_POLICY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public BondPolicyDetail FindPolicyDetailById(int id, int tradingProviderId, string isShowApp = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondPolicyDetail>(PROC_POLICY_DETAIL_GET, new
            {
                pv_BOND_POLICY_DETAIL_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public BondPolicyDetail FindPolicyDetailById(int id, bool isClose = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondPolicyDetail>(PROCT_BOND_POLICY_DETAIL_GET_WITH_OUT_TRADING_PROVIDER_ID, new
            {
                pv_BOND_POLICY_DETAIL_ID = id
            }, isClose);
        }

        public BondPolicyDetail GetPolicyDetailByIdWithoutTradingProviderId(int id, bool isClose = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondPolicyDetail>(PROCT_BOND_POLICY_DETAIL_GET_WITH_OUT_TRADING_PROVIDER_ID, new
            {
                pv_BOND_POLICY_DETAIL_ID = id
            }, isClose);
        }

        public BondSecondary FindSecondaryById(int id, int? tradingProviderId)
        {
            var rslt = _oracleHelper.ExecuteProcedureToFirst<BondSecondary>(PROC_GET_ONE, new
            {
                pv_BOND_SECONDARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return rslt;
        }

        public List<BondSecondary> FindSecondaryByDate(int? tradingProviderId, DateTime? startDate, DateTime? endDate, int? productBondId)
        {
            var rslt = _oracleHelper.ExecuteProcedure<BondSecondary>(PROC_BOND_SECONDARY_GET_BY_DATE, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_BOND_ID = productBondId,
            }).ToList();
            return rslt;
        }

        public List<BondSecondary> FindSecondaryByPartner(int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var rslt = _oracleHelper.ExecuteProcedure<BondSecondary>(PROC_SECONDARY_GET_BY_PARTNER, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_PARTNER_ID = partnerId,
            }).ToList();
            return rslt;
        }
        public int Update(BondSecondary entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_UPDATE, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BOND_SECONDARY_ID = entity.Id,
                        pv_BUSINESS_CUSTOMER_BANK_ID = entity.BusinessCustomerBankAccId,
                        pv_QUANTITY = entity.Quantity,
                        pv_OPEN_CELL_DATE = entity.OpenSellDate,
                        pv_CLOSE_CELL_DATE = entity.CloseSellDate,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int IsClose(int bondSecondaryId, string isClose, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_IS_CLOSE, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_CLOSE = isClose
            });
        }

        public int IsShowApp(int bondSecondaryId, string isShowApp, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_IS_SHOW_APP, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public int PolicyIsShowApp(int policyId, string isShowApp, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_IS_SHOW_APP, new
            {
                pv_BOND_POLICY_ID = policyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public int PolicyDetailIsShowApp(int policyDetailId, string isShowApp, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_DETAIL_IS_SHOW_APP, new
            {
                pv_BOND_POLICY_DETAIL_ID = policyDetailId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_IS_SHOW_APP = isShowApp
            });
        }

        public int SuperAdminApprove(int bondSecondaryId, int status)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_SUPER_ADMIN_APPROVE, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_STATUS = status
            });
        }

        public int TradingProviderApprove(int bondSecondaryId, int tradingProviderId, int status)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_PROVIDER_APPROVE, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status
            });
        }

        public int TradingProviderSubmit(int bondSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_PROVIDER_SUBMIT, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int UpdatePolicy(BondPolicy entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_UPDATE, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BOND_POLICY_ID = entity.Id,
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

        public int UpdateStatusPolicy(BondPolicy entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_CHANGE_STATUS, new
                    {
                        pv_BOND_POLICY_ID = entity.Id,
                        pv_STATUS = entity.Status,
                        SESSION_USERNAME = entity.ModifiedBy
                    }, false);
        }

        public int UpdateStatusPolicyDetail(BondPolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_CHANGE_STATUS, new
                    {
                        pv_BOND_POLICY_DETAIL_ID = entity.Id,
                        pv_STATUS = entity.Status,
                        SESSION_USERNAME = entity.CreatedBy
                    }, false);
        }

        public int UpdatePolicyDetail(BondPolicyDetail entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_UPDATE, new
                    {
                        pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                        pv_BOND_POLICY_DETAIL_ID = entity.Id,
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
                pv_BOND_POLICY_ID = policyId,
            });
        }

        public int DeletePolicyDetail(int policyDetailId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_DETAIL_DELETE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_BOND_POLICY_DETAIL_ID = policyDetailId,
            });
        }

        public IEnumerable<BondInfoSecondaryFindDto> FindAllProduct(string keyword, int? investorId)
        {
            return _oracleHelper.ExecuteProcedure<BondInfoSecondaryFindDto>(PROC_APP_SECONDARY_FIND_ALL, new
            {
                KEY_WORD = keyword,
                pv_INVESTOR_ID = investorId
            });
        }

        public AppBondInfoDto AppFindSecondaryById(int id, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppBondInfoDto>(PROC_APP_SECONDARY_GET_BY_INFO, new
            {
                pv_BOND_SECONDARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<AppBondPolicyDto> FindAllProductPolicy(int id, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppBondPolicyDto>(PROC_APP_BOND_POLICY_GET_ALL, new
            {
                pv_BOND_SECONDARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public IEnumerable<AppBondPolicyDetailDto> FindAllProductPolicyDetail(int bondPolicyId, int? tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<AppBondPolicyDetailDto>(PROC_APP_POLICY_DETAIL_FIND, new
            {
                pv_BOND_POLICY_ID = bondPolicyId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public AppBondPolicyDetailDto FindPolicyDetailMaxProfit(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AppBondPolicyDetailDto>(PROC_POLICY_DETAIL_BY_PROFIT, new
            {
                pv_BOND_SECONDARY_ID = id
            });
        }

        public int UpdatePolicyStatus(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_CHANGE_STATUS, new
                    {
                        pv_BOND_POLICY_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }
        public int UpdatePolicyDetailStatus(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_POLICY_DETAIL_CHANGE_STATUS, new
                    {
                        pv_BOND_POLICY_DETAIL_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }

        public BondSecondary FindSecondaryByPrimaryId(int bondPrimaryId)
        {
            var rslt = _oracleHelper.ExecuteProcedureToFirst<BondSecondary>(PROC_SECONDARY_GET_BY_PRIMARY, new
            {
                pv_BOND_PRIMARY_ID = bondPrimaryId
            });
            return rslt;
        }

        /// <summary>
        /// ExportReport: TỔNG SỐ LƯỢNG CỦA LỆNH KHI ĐÃ ACTIVE
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="bondPrimaryId"></param>
        /// <returns></returns>
        public decimal SumQuantity(int bondSecondaryId, int classify)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_CLASSIFY", classify, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_BOND_SECONDARY_ID", bondSecondaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SECONDARY_SUM_QUANTITY_ORDER, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }

        public int UpdateOverviewContent(UpdateBondInfoOverviewContentDto input, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_SECONDARY_UPDATE_OVERVIEW_CONTENT, new
            {
                pv_BOND_SECONDARY_ID = input.SecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_OVERVIEW_IMAGE_URL = input.OverviewImageUrl,
                pv_CONTENT_TYPE = input.ContentType,
                pv_OVERVIEW_CONTENT = input.OverviewContent
            });
        }
    }
}

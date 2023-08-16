using Dapper.Oracle;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.InterestPayment;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondInterestPaymentRepository : BaseRepository
    {
        private static string PROC_INTEREST_PAYMENT_GET_ALL = "PKG_BOND_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_GET_ALL";
        private static string PROC_INTEREST_PAYMENT_GET = "PKG_BOND_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_GET";
        private static string PROC_CHANGE_STATUS_1_TO_2 = "PKG_BOND_INTEREST_PAYMENT.PROC_CHANGE_STATUS_1_TO_2";
        private static string PROC_INTEREST_PAYMENT_ADD = "PKG_BOND_INTEREST_PAYMENT.PROC_INTEREST_PAYMENT_ADD";
        private static string PROC_CHECK_PAYED_PERIOD = "PKG_BOND_INTEREST_PAYMENT.CHECK_PERIOD_IS_PAYED";
        private static string PROC_GET_LIST_INTEREST_PAYMENT = "PKG_BOND_INTEREST_PAYMENT.GET_LIST_INTEREST_PAY_BY_ORDER";
        public BondInterestPaymentRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public int InterestPaymentAdd(BondInterestPayment entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_INTEREST_PAYMENT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_PERIOD_INDEX = entity.PeriodIndex,
                pv_CIF_CODE = entity.CifCode,
                pv_AMOUNT_MONEY = entity.AmountMoney,
                pv_POLICY_DETAIL_ID = entity.PolicyDetailId,
                pv_PAY_DATE = entity.PayDate,
                pv_IS_LAST_PERIOD = entity.IsLastPeriod,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public PagingResult<BondInterestPayment> FindAll(int pageSize, int pageNumber, string keyword, int? status, string phone, string contractCode, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BondInterestPayment>(PROC_INTEREST_PAYMENT_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PHONE = phone,
                pv_CONTRACT_CODE = contractCode
            });
            return result;
        }

        public BondInterestPayment FindById(int id, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondInterestPayment>(PROC_INTEREST_PAYMENT_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        /// <summary>
        /// Chuyển status từ đã lập chưa chi trả sang đã chi trả
        /// Nếu là cối kỳ có tái tục Id : hợp đồng mới, PolicyDetailId: kỳ hạn mới sau khi tái tục
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public BondInterestPayment ChangeEstablishedWithOutPayingToPaidStatus(int? id, int? tradingProviderId, DateTime investDateNew, string approveIp, string username)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondInterestPayment>(PROC_CHANGE_STATUS_1_TO_2, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVEST_DATE_NEW = investDateNew,
                pv_APPROVE_IP = approveIp,
                SESSION_USERNAME = username
            });
            return result;
        }

        /// <summary>
        /// Lấy list interest payment by order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<BondInterestPayment> GetListInterestPaymentByOrderId(long? orderId, int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<BondInterestPayment>(PROC_GET_LIST_INTEREST_PAYMENT, new
            {
                pv_ORDER_ID = orderId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }).ToList();
            return result;
        }
        public string CheckPeriodIsPayed(int? orderId, int? periodIndex, bool isClose = true)
        {
            var result = "";
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_ORDER_ID ", orderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_PERIOD_INDEX", periodIndex, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Varchar2, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_CHECK_PAYED_PERIOD, parameters, isClose);

            result = parameters.Get<string>("pv_RESULT");
            return result;
        }
    }
}

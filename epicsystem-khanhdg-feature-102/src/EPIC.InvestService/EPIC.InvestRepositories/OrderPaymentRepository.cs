using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestOrderPaymentRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_ORDER_PAYMENT_ADD = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_ADD";
        private const string PROC_ORDER_PAYMENT_PAY_ADD = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_PAY_ADD";
        private const string PROC_ORDER_PAYMENT_UPDATE = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_UPDATE";
        private const string PROC_ORDER_PAYMENT_DELETE = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_DELETE";
        private const string PROC_ORDER_PAYMENT_GET_ALL = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_GET_ALL";
        private const string PROC_ORDER_PAYMENT_GET_LIST_SUCCESS = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_GET_LIST_SUCCESS";
        private const string PROC_ORDER_PAYMENT_GET = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_GET";
        private const string PROC_ORDER_PAYMENT_APPROVE = "PKG_INV_ORDER_PAYMENT.PROC_ORDER_PAYMENT_APPROVE";

        public InvestOrderPaymentRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public OrderPayment Add(OrderPayment entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<OrderPayment>(PROC_ORDER_PAYMENT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_TRADING_BANK_ACC_ID = entity.TradingBankAccId,
                pv_TRAN_DATE = entity.TranDate,
                pv_TRAN_TYPE = entity.TranType,
                pv_TRAN_CLASSIFY = entity.TranClassify,
                pv_PAYMENT_TYPE = entity.PaymentType,
                pv_PAYMENT_AMNOUNT = entity.PaymentAmnount,
                pv_DESCRIPTION = entity.Description,
                SESSION_USERNAME = entity.CreatedBy
            });
            return result;
        }

        public OrderPayment PaymentPayAdd(OrderPayment entity, bool isClose = true)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<OrderPayment>(PROC_ORDER_PAYMENT_PAY_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_TRADING_BANK_ACC_ID = entity.TradingBankAccId,
                pv_TRAN_DATE = entity.TranDate,
                pv_TRAN_TYPE = entity.TranType,
                pv_TRAN_CLASSIFY = entity.TranClassify,
                pv_PAYMENT_TYPE = entity.PaymentType,
                pv_PAYMENT_AMNOUNT = entity.PaymentAmnount,
                pv_STATUS = entity.Status,
                pv_APPROVE_DATE = entity.ApproveDate,
                pv_APPROVE_BY = entity.ApproveBy,
                pv_DESCRIPTION = entity.Description,
                SESSION_USERNAME = entity.CreatedBy
            }, isClose);
            return result;
        }

        public int Update(OrderPayment entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_PAYMENT_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TRAN_DATE = entity.TranDate,
                pv_TRAN_TYPE = entity.TranType,
                pv_TRAN_CLASSIFY = entity.TranClassify,
                pv_PAYMENT_TYPE = entity.PaymentType,
                pv_PAYMENT_AMNOUNT = entity.PaymentAmnount,
                pv_DESCRIPTION = entity.Description,
                SESSION_USERNAME = entity.ModifiedBy
            });
            return result;
        }

        public int Delete(int id, int tradingProviderId)
        {
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_PAYMENT_DELETE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return rslt;
        }

        public OrderPayment FindById(long id, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<OrderPayment>(PROC_ORDER_PAYMENT_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public PagingResult<OrderPayment> FindAll(int orderId, int pageSize, int pageNumber, string keyword, int? status)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<OrderPayment>(PROC_ORDER_PAYMENT_GET_ALL, new
            {
                pv_ORDER_ID = orderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status
            });
            return result;
        }

        public List<OrderPayment> GetListPaymentSuccess(int orderId)
        {
            var result = _oracleHelper.ExecuteProcedure<OrderPayment>(PROC_ORDER_PAYMENT_GET_LIST_SUCCESS, new
            {
                pv_ORDER_ID = orderId
            }).ToList();
            return result;
        }

        public int ApprovePayment(int id, int status, int tradingProviderId, string username)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_PAYMENT_APPROVE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_STATUS = status,
                SESSION_USERNAME = username
            });
        }
    }
}

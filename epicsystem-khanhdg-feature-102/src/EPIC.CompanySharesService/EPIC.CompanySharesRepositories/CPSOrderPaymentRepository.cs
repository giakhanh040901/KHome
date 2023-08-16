using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.OrderPayment;
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
    public class CpsOrderPaymentRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_ORDER_PAYMENT_ADD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER_PAYMENT.PROC_ORDER_PAYMENT_ADD";
        private const string PROC_ORDER_PAYMENT_UPDATE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER_PAYMENT.PROC_ORDER_PAYMENT_UPDATE";
        private const string PROC_ORDER_PAYMENT_DELETE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER_PAYMENT.PROC_ORDER_PAYMENT_DELETE";
        private const string PROC_ORDER_PAYMENT_GET_ALL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER_PAYMENT.PROC_ORDER_PAYMENT_GET_ALL";
        private const string PROC_ORDER_PAYMENT_GET = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER_PAYMENT.PROC_ORDER_PAYMENT_GET";
        private const string PROC_ORDER_PAYMENT_APPROVE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_ORDER_PAYMENT.PROC_O RDER_PAYMENT_APPROVE";

        public CpsOrderPaymentRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public CpsOrderPayment Add(CpsOrderPayment entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<CpsOrderPayment>(PROC_ORDER_PAYMENT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_TRAN_DATE = entity.TranDate,
                pv_TRAN_TYPE = entity.TranType,
                pv_TRAN_CLASSIFY = entity.TranClassify,
                pv_PAYMENT_TYPE = entity.PaymentType,
                pv_PAYMENT_AMNOUNT = entity.PaymentAmnount,
                pv_DESCRIPTION = entity.Description,
                CREATED_BY = entity.CreatedBy
            });
            return result;
        }


        public int Update(CpsOrderPayment entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_PAYMENT_UPDATE, new
            {
                pv_ORDER_PAYMENT_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
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

        public int Delete(int id)
        {
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_PAYMENT_DELETE, new
            {
                pv_ID = id
            });
            return rslt;
        }

        public OrderPaymentDto FindById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<OrderPaymentDto>(PROC_ORDER_PAYMENT_GET, new
            {
                pv_ID = id,
            });
            return result;
        }

        public PagingResult<CpsOrderPayment> FindAll(int orderId, int pageSize, int pageNumber, string keyword, int? status)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<CpsOrderPayment>(PROC_ORDER_PAYMENT_GET_ALL, new
            {
                pv_ORDER_ID = orderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status
            });
            return result;
        }

        public int ApprovePayment(int orderPaymentId, int status, int tradingProviderId, string username)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_PAYMENT_APPROVE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_PAYMENT_ID = orderPaymentId,
                pv_STATUS = status,
                SESSION_USERNAME = username
            });
        }
    }
}

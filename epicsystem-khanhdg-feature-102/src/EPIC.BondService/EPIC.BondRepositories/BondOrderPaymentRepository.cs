using Dapper.Oracle;
using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.OrderPayment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondOrderPaymentRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_ORDER_PAYMENT_ADD = "PKG_BOND_ORDER_PAYMENT.PROC_ORDER_PAYMENT_ADD";
        private const string PROC_ORDER_PAYMENT_UPDATE = "PKG_BOND_ORDER_PAYMENT.PROC_ORDER_PAYMENT_UPDATE";
        private const string PROC_ORDER_PAYMENT_DELETE = "PKG_BOND_ORDER_PAYMENT.PROC_ORDER_PAYMENT_DELETE";
        private const string PROC_ORDER_PAYMENT_GET_ALL = "PKG_BOND_ORDER_PAYMENT.PROC_ORDER_PAYMENT_GET_ALL";
        private const string PROC_ORDER_PAYMENT_GET = "PKG_BOND_ORDER_PAYMENT.PROC_ORDER_PAYMENT_GET";
        private const string PROC_ORDER_PAYMENT_APPROVE = "PKG_BOND_ORDER_PAYMENT.PROC_ORDER_PAYMENT_APPROVE";

        public BondOrderPaymentRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public BondOrderPayment Add(BondOrderPayment entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondOrderPayment>(PROC_ORDER_PAYMENT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_TRAN_DATE = entity.TranDate,
                pv_TRAN_TYPE = entity.TranType,
                pv_TRAN_CLASSIFY = entity.TranClassify,
                pv_PAYMENT_TYPE = entity.PaymentType,
                pv_PAYMENT_AMNOUNT = entity.PaymentAmount,
                pv_DESCRIPTION = entity.Description,
                CREATED_BY = entity.CreatedBy
            });
            return result;
        }


        public int Update(BondOrderPayment entity)
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
                pv_PAYMENT_AMNOUNT = entity.PaymentAmount,
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

        public PagingResult<BondOrderPayment> FindAll(int orderId, int pageSize, int pageNumber, string keyword, int? status)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BondOrderPayment>(PROC_ORDER_PAYMENT_GET_ALL, new
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

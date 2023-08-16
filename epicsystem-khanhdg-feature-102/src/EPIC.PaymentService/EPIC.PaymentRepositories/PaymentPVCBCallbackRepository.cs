
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.PaymentEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RabbitMQ.Client.Logging.RabbitMqClientEventSource;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using EPIC.PaymentEntities.Dto.Pvcb;

namespace EPIC.PaymentRepositories
{
    public class PaymentPvcbCallbackRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_PVCB_CALLBACK_ADD = "PKG_PAY_CALLBACK.PROC_PVCB_CALLBACK_ADD"; 
        private const string PROC_ADD_BOND_ORDER_PAYMENT = "PKG_PAY_CALLBACK.PROC_ADD_BOND_ORDER_PAYMENT";
        private const string PROC_ADD_INVEST_ORDER_PAYMENT = "PKG_PAY_CALLBACK.PROC_ADD_INVEST_ORDER_PAYMENT";
        private const string PROC_PVCB_PAY_CALLBACK_GET_ALL = "PKG_PAY_CALLBACK.PROC_PVCB_PAY_CALLBACK_GET_ALL";
        private const string PROC_PAY_CALLBACK_FIND_ORDER = "PKG_PAY_CALLBACK.PROC_PAY_CALLBACK_FIND_ORDER";

        public PaymentPvcbCallbackRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        /// <summary>
        /// Lưu lại callback từ PVCombank
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PvcbCallback Add(PvcbCallback entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<PvcbCallback>(PROC_PVCB_CALLBACK_ADD, new
            {
                pv_FtType = entity.FtType,
                pv_Amount = entity.Amount,
                pv_Balance = entity.Balance,
                pv_SenderBankId = entity.SenderBankId,
                pv_Description = entity.Description,
                pv_TranId = entity.TranId,
                pv_TranDate = entity.TranDate,
                pv_Currency = entity.Currency,
                pv_TranStatus = entity.TranStatus,
                pv_ConAmount = entity.ConAmount,
                pv_NumberOfBeneficiary = entity.NumberOfBeneficiary,
                pv_Account = entity.Account,
                pv_RequestIP = entity.RequestIP,
                pv_TOKEN = entity.Token,
            });
        }

        /// <summary>
        /// Truyền vào orderId
        /// Lấy ra thông tin Bank trong bảng EP_CORE_BUSINESS_CUSTOMER_BANK
        /// </summary>
        /// <returns></returns>
        public CreateBankOrderPaymentDto CreateBondPayment(CreateBankOrderPaymentDto input, decimal TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CreateBankOrderPaymentDto>(PROC_ADD_BOND_ORDER_PAYMENT, new
            {
                pv_OrderId = input.OrderId,
                pv_Amount = input.Amount,
                pv_Account = input.Account,
                pv_TranDate = input.TranDate,
                pv_Description = input.Description,
                pv_TradingProviderId = TradingProviderId
            });
        }

        public CreateBankOrderPaymentDto CreateInvestPayment(CreateBankOrderPaymentDto input, decimal TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CreateBankOrderPaymentDto>(PROC_ADD_INVEST_ORDER_PAYMENT, new
            {
                pv_OrderId = input.OrderId,
                pv_Amount = input.Amount,
                pv_Account = input.Account,
                pv_TranDate = input.TranDate,
                pv_Description = input.Description,
                pv_TradingProviderId = TradingProviderId
            });
        }

        public PagingResult<CallbackDataDto> FindAll(int pageSize, int pageNumber, string keyword, string status)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<CallbackDataDto>(PROC_PVCB_PAY_CALLBACK_GET_ALL, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status
            });
            return result;
        }

        public PvcbCallback FindOrderById(int orderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<PvcbCallback>(PROC_PAY_CALLBACK_FIND_ORDER, new
            {
                pv_OrderId = orderId
            });
            return result;
        }
    }
}

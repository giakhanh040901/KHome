using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.RenewalsRequest;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvRenewalsRequestRepository : BaseRepository
    {
        private const string PROC_INV_RENEWALS_REQUEST_ADD = "PKG_INV_RENEWALS_REQUEST.PROC_INV_RENEWALS_REQUEST_ADD";
        private const string PROC_REQUEST_APPROVE = "PKG_INV_RENEWALS_REQUEST.PROC_REQUEST_APPROVE";
        private const string PROC_CANCEL_APPROVE = "PKG_INV_RENEWALS_REQUEST.PROC_CANCEL_APPROVE";
        private const string PROC_GET_BY_ORDER_ID = "PKG_INV_RENEWALS_REQUEST.PROC_GET_BY_ORDER_ID";
        private const string PROC_GET_LIST_BY_ORDER_ID = "PKG_INV_RENEWALS_REQUEST.PROC_GET_LIST_BY_ORDER_ID";
        private const string PROC_GET_BY_ID = "PKG_INV_RENEWALS_REQUEST.PROC_GET_BY_ID";
        private const string PROC_APP_RENEWALS_REQUEST = "PKG_INV_RENEWALS_REQUEST.PROC_APP_RENEWALS_REQUEST";
        private const string PROC_GET_ALL_RENEW_REQUEST = "PKG_INV_RENEWALS_REQUEST.PROC_GET_ALL";

        public InvRenewalsRequestRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public InvRenewalsRequest CreateRenewalsRequestCms (CreateRenewalsRequestDto entity, int tradingProviderId, int userId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvRenewalsRequest>(PROC_INV_RENEWALS_REQUEST_ADD, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_USER_ID = userId,
                pv_ORDER_ID = entity.OrderId,
                pv_RENEWALS_POLICY_DETAIL_ID = entity.RenewarsPolicyDetailId,
                pv_SETTLEMENT_METHOD = entity.SettlementMethod,
                pv_REQUEST_NOTE = entity.RequestNote,
                pv_SUMMARY = entity.Summary
            });
        }

        public InvRenewalsRequest AppRenewalsRequest(AppCreateRenewalsRequestDto entity, int investorId, int userId, string requestNote, string summary)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvRenewalsRequest>(PROC_APP_RENEWALS_REQUEST, new
            {
                pv_INVESTOR_ID = investorId,
                pv_USER_ID = userId,
                pv_ORDER_ID = entity.OrderId,
                pv_RENEWALS_POLICY_DETAIL_ID = entity.RenewarsPolicyDetailId,
                pv_SETTLEMENT_METHOD = entity.SettlementMethod,
                pv_REQUEST_NOTE = requestNote,
                pv_SUMMARY = summary
            });
        }

        public int ApproveRequest (int id, bool closeConnection = true)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_REQUEST_APPROVE, new
            {
                pv_ID = id
            }, closeConnection);
        }

        public int CancelRequest(int id, string cancelNote)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_CANCEL_APPROVE, new
            {
                pv_ID = id,
                pv_CANCEL_NOTE = cancelNote
            });
        }

        /// <summary>
        /// Lấy yêu cầu tái tục rút vốn mới nhất có SettlementMethod = 2: vốn không lợi nhuận, 3: vốn có lợi nhuận
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public InvRenewalsRequest GetByOrderId(long orderId, int? tradingProviderId = null, bool isClose = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvRenewalsRequest>(PROC_GET_BY_ORDER_ID, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
            }, isClose);
        }

        public IEnumerable<InvRenewalsRequest> GetListByOrderId(long orderId)
        {
            return _oracleHelper.ExecuteProcedure<InvRenewalsRequest>(PROC_GET_LIST_BY_ORDER_ID, new
            {
                pv_ORDER_ID = orderId,
            });
        }

        public InvRenewalsRequest GetById(int id, bool closeConnection = true)
        {
            return _oracleHelper.ExecuteProcedureToFirst<InvRenewalsRequest>(PROC_GET_BY_ID, new
            {
                pv_ID = id,
            }, closeConnection);
        }

        public IEnumerable<OrderRenewalsRequestDto> GetAll(int? tradingProviderId, FilterRenewalsRequestDto input)
        {
            return _oracleHelper.ExecuteProcedure<OrderRenewalsRequestDto>(PROC_GET_ALL_RENEW_REQUEST, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TRADING_PROVIDER_CHILD_IDS = input.TradingProviderIds != null ? string.Join(',', input.TradingProviderIds) : null,
            });
        }
    }
}

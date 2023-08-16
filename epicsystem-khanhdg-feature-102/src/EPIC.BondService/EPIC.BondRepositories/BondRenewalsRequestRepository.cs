using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.RenewalsRequest;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondRenewalsRequestRepository : BaseRepository
    {
        private const string PROC_BOND_RENEWALS_REQUEST_ADD = "PKG_BOND_RENEWALS_REQUEST.PROC_BOND_RENEWALS_REQUEST_ADD";
        private const string PROC_REQUEST_APPROVE = "PKG_BOND_RENEWALS_REQUEST.PROC_REQUEST_APPROVE";
        private const string PROC_CANCEL_APPROVE = "PKG_BOND_RENEWALS_REQUEST.PROC_CANCEL_APPROVE";
        private const string PROC_GET_BY_ORDER_ID = "PKG_BOND_RENEWALS_REQUEST.PROC_GET_BY_ORDER_ID";
        private const string PROC_GET_BY_ID = "PKG_BOND_RENEWALS_REQUEST.PROC_GET_BY_ID";
        private const string PROC_APP_RENEWALS_REQUEST = "PKG_BOND_RENEWALS_REQUEST.PROC_APP_RENEWALS_REQUEST";

        public BondRenewalsRequestRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public BondRenewalsRequest AddRequest(CreateRenewalsRequestDto entity, int tradingProviderId, int userId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondRenewalsRequest>(PROC_BOND_RENEWALS_REQUEST_ADD, new
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

        public int ApproveRequest(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_REQUEST_APPROVE, new
            {
                pv_ID = id
            });
        }

        public int CancelRequest(int id, string cancelNote)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_CANCEL_APPROVE, new
            {
                pv_ID = id,
                pv_CANCEL_NOTE = cancelNote
            });
        }

        public BondRenewalsRequest GetByOrderId(int orderId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondRenewalsRequest>(PROC_GET_BY_ORDER_ID, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
            });
        }

        public BondRenewalsRequest GetById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondRenewalsRequest>(PROC_GET_BY_ID, new
            {
                pv_ID = id,
            });
        }

        public BondRenewalsRequest AppRenewalsRequest(AppCreateRenewalsRequestDto entity, int investorId, int userId, string requestNote, string summary)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondRenewalsRequest>(PROC_APP_RENEWALS_REQUEST, new
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
    }
 }

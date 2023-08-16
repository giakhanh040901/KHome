using Dapper.Oracle;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.Auth;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Withdrawal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class WithdrawalRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string PROC_SUM_WITHDRAWAL_IN_DISTRIBUTION = "PKG_INV_WITHDRAWAL.PROC_SUM_WITHDRAWAL_IN_DISTRIBUTION"; 
        private const string PROC_INV_WITHDRAWAL_ADD = "PKG_INV_WITHDRAWAL.PROC_INV_WITHDRAWAL_ADD";
        private const string PROC_INV_WITHDRAWAL_GET_LIST = "PKG_INV_WITHDRAWAL.PROC_INV_WITHDRAWAL_GET_LIST";
        private const string PROC_INV_WITHDRAWAL_GET_ALL = "PKG_INV_WITHDRAWAL.PROC_INV_WITHDRAWAL_GET_ALL";
        private const string PROC_WITHDRAWAL_APPROVE = "PKG_INV_WITHDRAWAL.PROC_WITHDRAWAL_APPROVE";
        private const string PROC_WITHDRAWAL_CANCEL = "PKG_INV_WITHDRAWAL.PROC_WITHDRAWAL_CANCEL";
        private const string PROC_APP_WITHDRAWAL_REQUEST = "PKG_INV_WITHDRAWAL.PROC_APP_WITHDRAWAL_REQUEST";
        private const string PROC_WITHDRAWAL_GET_BY_ID = "PKG_INV_WITHDRAWAL.PROC_WITHDRAWAL_GET_BY_ID";

        public WithdrawalRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public int WithdrawalAdd(Withdrawal entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_INV_WITHDRAWAL_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_AMOUNT_MONEY = entity.AmountMoney,
                pv_WITHDRAWAL_DATE = entity.WithdrawalDate,
                pv_REQUEST_IP = entity.RequestIp,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public List<Withdrawal> WithdrawalGetList(long orderId, int tradingProviderId, int? status = null)
        {
            return _oracleHelper.ExecuteProcedure<Withdrawal>(PROC_INV_WITHDRAWAL_GET_LIST, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                pv_STATUS = status,
            }).ToList();
        }

        public decimal WithdrawalOrderInDistribution(int distributionId, int? tradingProviderId, bool isClose = true)
        {
            decimal result = 0;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_DISTRIBUTION_ID", distributionId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_WITHDRAWAL_IN_DISTRIBUTION, parameters, isClose);

            result = parameters.Get<decimal>("pv_RESULT");
            return result;
        }

        public PagingResult<Withdrawal> FindAll(InvestWithdrawalFilterDto input, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<Withdrawal>(PROC_INV_WITHDRAWAL_GET_ALL, new
            {
                PAGE_SIZE = input.PageSize,
                PAGE_NUMBER = input.PageNumber,
                KEY_WORD = input.Keyword,
                pv_STATUS = input.Status,
                pv_REQUEST_DATE = input.RequestDate,
                pv_APPROVE_DATE = input.ApproveDate,
                pv_CONTRACT_CODE = input.ContractCode,
                pv_PHONE = input.Phone,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SOURCE = input.Source,
                pv_DISTRIBUTION_ID = input.DistributionId,
                pv_CIF_CODE = input.CifCode,
                pv_TRADING_PROVIDER_CHILD_IDS = input.TradingProviderIds != null ? string.Join(',', input.TradingProviderIds) : null,
            });
            return result;
        }

        public int WithdrawalApprove(long id, int tradingProviderId, decimal actuallyAmount, string approveIp, string userApprove)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_WITHDRAWAL_APPROVE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ID = id,
                pv_ACTUALLY_AMOUNT = actuallyAmount,
                pv_APPROVE_IP = approveIp,
                SESSION_USERNAME = userApprove,
            });
            return result;
        }

        public int WithdrawalCancel(long id, int tradingProviderId, string userCancel)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_WITHDRAWAL_CANCEL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ID = id,
                SESSION_USERNAME = userCancel,
            });
            return result;
        }

        public Withdrawal FindById(long withdrawalId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Withdrawal>(PROC_WITHDRAWAL_GET_BY_ID, new
            {
                pv_ID = withdrawalId
            });
        }

        public Withdrawal AppWithdrawalRequest(Withdrawal entity, int investorId, string otp)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Withdrawal>(PROC_APP_WITHDRAWAL_REQUEST, new
            {
                pv_INVESTOR_ID = investorId,
                pv_ORDER_ID = entity.OrderId,
                pv_AMOUNT_MONEY = entity.AmountMoney,
                pv_WITHDRAWAL_DATE = entity.WithdrawalDate,
                pv_REQUEST_IP = entity.RequestIp,
                pv_OTP = otp,
                SESSION_USERNAME = entity.ModifiedBy,
            });
        }
    }
}

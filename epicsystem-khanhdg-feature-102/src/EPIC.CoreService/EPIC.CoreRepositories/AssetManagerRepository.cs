using Dapper.Oracle;
using EPIC.DataAccess;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestSharedEntites.Dto.Order;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class AssetManagerRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_BOND_ORDER_SUM_VALUE = "PKG_BOND_ORDER.PROC_ORDER_SUM_VALUE";
        private const string PROC_INVEST_ORDER_SUM_VALUE = "PKG_INV_ORDER.PROC_ORDER_SUM_VALUE";
        private const string PROC_INVESTOR_TRADING_RECENTLY = "PKG_INVESTOR_ASSET_MANAGER.PROC_INVESTOR_TRADING_RECENTLY";
        private const string PROC_INVESTOR_TRADING_RECENTLY_INVEST = "PKG_INVESTOR_ASSET_MANAGER.PROC_INVESTOR_TRADING_RECENTLY_INVEST";

        public AssetManagerRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public decimal AppBondOrderInvestorQuantity(int investorId)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_INVESTOR_ID", investorId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_BOND_ORDER_SUM_VALUE, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }

        public decimal AppInvestOrderInvestorQuantity(int investorId)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_INVESTOR_ID", investorId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_INVEST_ORDER_SUM_VALUE, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }

        public IEnumerable<TradingRecentlyDto> TradingRecently (int investorId)
        {
            return _oracleHelper.ExecuteProcedure<TradingRecentlyDto>(PROC_INVESTOR_TRADING_RECENTLY, new
            {
                pv_INVESTOR_ID = investorId
            });
        }

        public IEnumerable<AppInvTransactionListDto> TradingRecentlyByOrder(int orderId)
        {
            return _oracleHelper.ExecuteProcedure<AppInvTransactionListDto>(PROC_INVESTOR_TRADING_RECENTLY_INVEST, new
            {
                pv_ORDER_ID = orderId
            });
        }
    }
}

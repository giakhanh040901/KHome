using EPIC.CompanySharesEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesRepositories
{
    public class CpsInfoTradingProviderRepository : BaseRepository
    {
        private const string PROC_CPS_TRADING_PROVIDER_ADD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_TRADING_PROVIDER.PROC_CPS_TRADING_PROVIDER_ADD";
        private const string PROC_CPS_TRADING_PROVIDER_GET = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_TRADING_PROVIDER.PROC_CPS_TRADING_PROVIDER_GET";
        private const string PROC_CPS_TRADING_PROVIDER_DELETED = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_TRADING_PROVIDER.PROC_CPS_TRADING_PROVIDER_DELETED";
        private const string PROC_CPS_TRADING_PROVIDER_UPDATE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_TRADING_PROVIDER.PROC_CPS_TRADING_PROVIDER_UPDATE";
        private const string PROC_CPS_TRADING_PROVIDER_GET_ALL = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_TRADING_PROVIDER.PROC_CPS_TRADING_PROVIDER_GET_ALL";

        public CpsInfoTradingProviderRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public CpsInfoTradingProvider FindById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsInfoTradingProvider>(PROC_CPS_TRADING_PROVIDER_GET, new
            {
                pv_ID = id,
            });
        }

        public List<CpsInfoTradingProvider> FindAll(int projectId, int? partnerId)
        {
            return _oracleHelper.ExecuteProcedure<CpsInfoTradingProvider>(PROC_CPS_TRADING_PROVIDER_GET_ALL, new
            {
                pv_CPS_INFO_ID = projectId,
                pv_PARTNER_ID = partnerId
            }).ToList();
        }

        public int Add(CpsInfoTradingProvider entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_TRADING_PROVIDER_ADD, new
            {
                pv_CPS_INFO_ID = entity.CpsInfoId,
                pv_PARTNER_ID = entity.PartnerId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_INVESTMENT_SUB = entity.TotalInvestmentSub,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }

        public int Update(CpsInfoTradingProvider entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_TRADING_PROVIDER_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_CPS_INFO_ID = entity.CpsInfoId,
                pv_PARTNER_ID = entity.PartnerId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_INVESTMENT_SUB = entity.TotalInvestmentSub,
                SESSION_USERNAME = entity.ModifiedBy,
            });
        }

        public int Delete(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_TRADING_PROVIDER_DELETED, new
            {
                pv_ID = id,
            });
        }
    }
}

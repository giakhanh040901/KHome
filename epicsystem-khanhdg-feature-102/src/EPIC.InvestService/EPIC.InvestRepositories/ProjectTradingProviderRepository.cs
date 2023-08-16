using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.InvestEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class ProjectTradingProviderRepository : BaseRepository
    {
        private const string PROC_PROJECT_TRADING_PROVIDER_ADD = "PKG_INV_PROJECT_TRADING_PROVIDER.PROC_PROJECT_TRADING_PROVIDER_ADD";
        private const string PROC_PROJECT_TRADING_PROVIDER_GET = "PKG_INV_PROJECT_TRADING_PROVIDER.PROC_PROJECT_TRADING_PROVIDER_GET";
        private const string PROC_PROJECT_TRADING_PROVIDER_DELETED = "PKG_INV_PROJECT_TRADING_PROVIDER.PROC_PROJECT_TRADING_PROVIDER_DELETED";
        private const string PROC_PROJECT_TRADING_PROVIDER_UPDATE = "PKG_INV_PROJECT_TRADING_PROVIDER.PROC_PROJECT_TRADING_PROVIDER_UPDATE";
        private const string PROC_PROJECT_TRADING_PROVIDER_GET_ALL = "PKG_INV_PROJECT_TRADING_PROVIDER.PROC_PROJECT_TRADING_PROVIDER_GET_ALL";

        public ProjectTradingProviderRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public ProjectTradingProvider FindById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<ProjectTradingProvider>(PROC_PROJECT_TRADING_PROVIDER_GET, new
            {
                pv_ID = id,
            });
        }

        public List<ProjectTradingProvider> FindAll(int projectId, int? partnerId)
        {
            return _oracleHelper.ExecuteProcedure<ProjectTradingProvider>(PROC_PROJECT_TRADING_PROVIDER_GET_ALL, new
            {
                pv_PROJECT_ID = projectId,
                pv_PARTNER_ID = partnerId
            }).ToList();
        }

        public int Add(ProjectTradingProvider entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_TRADING_PROVIDER_ADD, new
            {
                pv_PROJECT_ID = entity.ProjectId,
                pv_PARTNER_ID = entity.PartnerId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_INVESTMENT_SUB = entity.TotalInvestmentSub,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }

        public int Update(ProjectTradingProvider entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_TRADING_PROVIDER_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_PROJECT_ID = entity.ProjectId,
                pv_PARTNER_ID = entity.PartnerId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TOTAL_INVESTMENT_SUB = entity.TotalInvestmentSub,
                SESSION_USERNAME = entity.ModifiedBy,
            });
        }

        public int Delete(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_TRADING_PROVIDER_DELETED, new
            {
                pv_ID = id,
            });
        }
    }
}

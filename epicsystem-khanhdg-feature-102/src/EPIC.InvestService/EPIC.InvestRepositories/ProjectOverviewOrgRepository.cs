using EPIC.DataAccess.Models;
using EPIC.DataAccess;
using EPIC.InvestEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EPIC.InvestRepositories
{
    public class ProjectOverviewOrgRepository
    {
        private const string PROC_PROJECT_OVERVIEW_ORG_ADD = "PKG_INV_PROJECT_OVERVIEW_ORG.PROC_PROJECT_OVERVIEW_ORG_ADD";
        private const string PROC_PROJECT_OVERVIEW_ORG_GET = "PKG_INV_PROJECT_OVERVIEW_ORG.PROC_PROJECT_OVERVIEW_ORG_GET";
        private const string PROC_PROJECT_OVERVIEW_ORG_DELETE = "PKG_INV_PROJECT_OVERVIEW_ORG.PROC_PROJECT_OVERVIEW_ORG_DELETE";
        private const string PROC_PROJECT_OVERVIEW_ORG_UPDATE = "PKG_INV_PROJECT_OVERVIEW_ORG.PROC_PROJECT_OVERVIEW_ORG_UPDATE";
        private const string PROC_PROJECT_OVERVIEW_ORG_FIND = "PKG_INV_PROJECT_OVERVIEW_ORG.PROC_PROJECT_OVERVIEW_ORG_FIND";
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        public ProjectOverviewOrgRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public List<ProjectOverviewOrg> FindListOrg(int distributionId, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedure<ProjectOverviewOrg>(PROC_PROJECT_OVERVIEW_ORG_FIND, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }).ToList();
        }

        public ProjectOverviewOrg FindById(int id, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<ProjectOverviewOrg>(PROC_PROJECT_OVERVIEW_ORG_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public ProjectOverviewOrg Add(ProjectOverviewOrg entity, int tradingProviderId)
        {
            _logger.LogInformation("Add ProjectOverviewOrg");
            var result = _oracleHelper.ExecuteProcedureToFirst<ProjectOverviewOrg>(PROC_PROJECT_OVERVIEW_ORG_ADD, new
            {
                pv_DISTRIBUTION_ID = entity.DistributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_NAME = entity.Name,
                pv_ORG_CODE = entity.OrgCode,
                pv_ICON = entity.Icon,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int Update(ProjectOverviewOrg entity, int tradingProviderId)
        {
            _logger.LogInformation("Update ProjectOverviewOrg");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_OVERVIEW_ORG_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_DISTRIBUTION_ID = entity.DistributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_NAME = entity.Name,
                pv_ORG_CODE = entity.OrgCode,
                pv_ICON = entity.Icon,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation($"Delete ProjectOverviewOrg ");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_OVERVIEW_ORG_DELETE, new
            {
                pv_ID = id
            });
        }
    }
}

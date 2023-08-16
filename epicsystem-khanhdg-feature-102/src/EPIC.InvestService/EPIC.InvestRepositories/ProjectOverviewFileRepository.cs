using EPIC.DataAccess.Models;
using EPIC.DataAccess;
using EPIC.InvestEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using EPIC.DataAccess.Base;
using EPIC.BondEntities.DataEntities;

namespace EPIC.InvestRepositories
{
    public class ProjectOverviewFileRepository : BaseRepository
    {
        private const string PROC_PROJECT_OVERVIEW_FILE_ADD = "PKG_INV_PROJECT_OVERVIEW_FILE.PROC_PROJECT_OVERVIEW_FILE_ADD";
        private const string PROC_PROJECT_OVERVIEW_FILE_GET = "PKG_INV_PROJECT_OVERVIEW_FILE.PROC_PROJECT_OVERVIEW_FILE_GET";
        private const string PROC_PROJECT_OVERVIEW_FILE_DELETE = "PKG_INV_PROJECT_OVERVIEW_FILE.PROC_PROJECT_OVERVIEW_FILE_DELETE";
        private const string PROC_PROJECT_OVERVIEW_FILE_UPDATE = "PKG_INV_PROJECT_OVERVIEW_FILE.PROC_PROJECT_OVERVIEW_FILE_UPDATE";
        private const string PROC_PROJECT_OVERVIEW_FILE_GET_ALL = "PKG_INV_PROJECT_OVERVIEW_FILE.PROC_PROJECT_OVERVIEW_FILE_GET_ALL";
        private const string PROC_PROJECT_OVERVIEW_FILE_FIND = "PKG_INV_PROJECT_OVERVIEW_FILE.PROC_PROJECT_OVERVIEW_FILE_FIND";

        public ProjectOverviewFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        #region Repo file
        public PagingResult<ProjectOverviewFile> FindAll(int distributionId, int pageSize, int pageNumber, int? status, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ProjectOverviewFile>(PROC_PROJECT_OVERVIEW_FILE_GET_ALL, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                pv_STATUS = status
            });
            return result;
        }

        public List<ProjectOverviewFile> FindListFile(int distributionId, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedure<ProjectOverviewFile>(PROC_PROJECT_OVERVIEW_FILE_FIND, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }).ToList();
        }

        public ProjectOverviewFile FindById(int id, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<ProjectOverviewFile>(PROC_PROJECT_OVERVIEW_FILE_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public ProjectOverviewFile Add(ProjectOverviewFile entity, int tradingProviderId)
        {
            _logger.LogInformation("Add ProjectOverviewFile");
            var result = _oracleHelper.ExecuteProcedureToFirst<ProjectOverviewFile>(PROC_PROJECT_OVERVIEW_FILE_ADD, new
            {
                pv_DISTRIBUTION_ID = entity.DistributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int Update(ProjectOverviewFile entity, int tradingProviderId)
        {
            _logger.LogInformation("Update ProjectOverviewFile");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_OVERVIEW_FILE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_DISTRIBUTION_ID = entity.DistributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                pv_SORT_ORDER = entity.SortOrder,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation($"Delete ProjectOverviewfILE ");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_PROJECT_OVERVIEW_FILE_DELETE, new
            {
                pv_ID = id
            });
        }
        #endregion
    }
}

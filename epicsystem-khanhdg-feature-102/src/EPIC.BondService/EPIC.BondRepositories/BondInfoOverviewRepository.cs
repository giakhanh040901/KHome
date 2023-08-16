using EPIC.DataAccess.Models;
using EPIC.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.DataAccess.Base;
using Microsoft.Extensions.Logging;
using EPIC.BondEntities.DataEntities;

namespace EPIC.BondRepositories
{
    public class BondInfoOverviewRepository : BaseRepository
    {
        #region PROC File tổng quan
        private const string PROC_INFO_OVERVIEW_FILE_ADD = "PKG_BOND_INFO_OVERVIEW_FILE.PROC_INFO_OVERVIEW_FILE_ADD";
        private const string PROC_INFO_OVERVIEW_FILE_GET = "PKG_BOND_INFO_OVERVIEW_FILE.PROC_INFO_OVERVIEW_FILE_GET";
        private const string PROC_INFO_OVERVIEW_FILE_DELETE = "PKG_BOND_INFO_OVERVIEW_FILE.PROC_INFO_OVERVIEW_FILE_DELETE";
        private const string PROC_INFO_OVERVIEW_FILE_UPDATE = "PKG_BOND_INFO_OVERVIEW_FILE.PROC_INFO_OVERVIEW_FILE_UPDATE";
        private const string PROC_INFO_OVERVIEW_FILE_FIND = "PKG_BOND_INFO_OVERVIEW_FILE.PROC_INFO_OVERVIEW_FILE_FIND";
        #endregion

        #region PROC Org
        private const string PROC_INFO_OVERVIEW_ORG_ADD = "PKG_BOND_INFO_OVERVIEW_ORG.PROC_INFO_OVERVIEW_ORG_ADD";
        private const string PROC_INFO_OVERVIEW_ORG_GET = "PKG_BOND_INFO_OVERVIEW_ORG.PROC_INFO_OVERVIEW_ORG_GET";
        private const string PROC_INFO_OVERVIEW_ORG_DELETE = "PKG_BOND_INFO_OVERVIEW_ORG.PROC_INFO_OVERVIEW_ORG_DELETE";
        private const string PROC_INFO_OVERVIEW_ORG_UPDATE = "PKG_BOND_INFO_OVERVIEW_ORG.PROC_INFO_OVERVIEW_ORG_UPDATE";
        private const string PROC_INFO_OVERVIEW_ORG_FIND = "PKG_BOND_INFO_OVERVIEW_ORG.PROC_INFO_OVERVIEW_ORG_FIND";

        #endregion
        public BondInfoOverviewRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        #region Repo file
        public List<BondInfoOverviewFile> FindAllListFile(int bondSecondaryId, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedure<BondInfoOverviewFile>(PROC_INFO_OVERVIEW_FILE_FIND, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }).ToList();
        }

        public BondInfoOverviewFile FindFileById(int id, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondInfoOverviewFile>(PROC_INFO_OVERVIEW_FILE_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public BondInfoOverviewFile AddFile(BondInfoOverviewFile entity, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondInfoOverviewFile>(PROC_INFO_OVERVIEW_FILE_ADD, new
            {
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int UpdateFile(BondInfoOverviewFile entity, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_INFO_OVERVIEW_FILE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TITLE = entity.Title,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int DeleteFile(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INFO_OVERVIEW_FILE_DELETE, new
            {
                pv_ID = id
            });
        }
        #endregion

        #region Repo Org
        public List<BondInfoOverviewOrg> FindAllListOrg(int bondSecondaryId, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedure<BondInfoOverviewOrg>(PROC_INFO_OVERVIEW_ORG_FIND, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            }).ToList();
        }

        public BondInfoOverviewOrg FindOrgById(int id, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondInfoOverviewOrg>(PROC_INFO_OVERVIEW_ORG_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public BondInfoOverviewOrg AddOrg(BondInfoOverviewOrg entity, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondInfoOverviewOrg>(PROC_INFO_OVERVIEW_ORG_ADD, new
            {
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_NAME = entity.Name,
                pv_ORG_CODE = entity.OrgCode,
                pv_ICON = entity.Icon,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int UpdateOrg(BondInfoOverviewOrg entity, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_INFO_OVERVIEW_ORG_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_NAME = entity.Name,
                pv_ORG_CODE = entity.OrgCode,
                pv_ICON = entity.Icon,
                pv_URL = entity.Url,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int DeleteOrg(int id)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_INFO_OVERVIEW_ORG_DELETE, new
            {
                pv_ID = id
            });
        }
        #endregion
    }
}

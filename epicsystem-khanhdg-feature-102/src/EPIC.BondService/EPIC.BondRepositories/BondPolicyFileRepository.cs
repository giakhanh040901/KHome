using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondPolicyFileRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_POLICY_FILE_ADD = "PKG_POLICY_FILE.PROC_POLICY_FILE_ADD";
        private const string PROC_POLICY_FILE_GET = "PKG_POLICY_FILE.PROC_POLICY_FILE_GET";
        private const string PROC_POLICY_FILE_DELETE = "PKG_POLICY_FILE.PROC_POLICY_FILE_DELETE";
        private const string PROC_POLICY_FILE_UPDATE = "PKG_POLICY_FILE.PROC_POLICY_FILE_UPDATE";
        private const string PROC_POLICY_FILE_GET_ALL = "PKG_POLICY_FILE.PROC_POLICY_FILE_GET_ALL";

        public BondPolicyFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PagingResult<BondPolicyFile> FindAllPolicyFile(int bonSecondaryId, int? tradingProvider, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BondPolicyFile>(PROC_POLICY_FILE_GET_ALL, new
            {
                pv_BOND_SECONDARY_ID = bonSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProvider,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public BondPolicyFile FindPolicyFileById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondPolicyFile>(PROC_POLICY_FILE_GET, new
            {
                pv_POLICY_FILE_ID = id,
            });
            return result;
        }

        public BondPolicyFile Add(BondPolicyFile entity)
        {
            _logger.LogInformation("Add PolicyFile");
            var result = _oracleHelper.ExecuteProcedureToFirst<BondPolicyFile>(PROC_POLICY_FILE_ADD, new
            {
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                pv_EXPIRATION_DATE = entity.ExpirationDate,
                pv_EFFECTIVE_DATE = entity.EffectiveDate,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int PolicyFileUpdate(BondPolicyFile entity)
        {
            _logger.LogInformation("Update PolicylFile");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_FILE_UPDATE, new
            {
                pv_POLICY_FILE_ID = entity.Id,
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                pv_EXPIRATION_DATE = entity.ExpirationDate,
                pv_EFFECTIVE_DATE = entity.EffectiveDate,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int DeletePolicyFile(int id, int tradingProvider)
        {
            _logger.LogInformation($"Delete PolicyFile ");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_FILE_DELETE, new
            {
                pv_POLICY_FILE_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProvider
            });
        }
    }
}

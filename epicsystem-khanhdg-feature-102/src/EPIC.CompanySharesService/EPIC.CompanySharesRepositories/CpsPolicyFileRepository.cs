
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;

namespace EPIC.CompanySharesRepositories
{
    public class CpsPolicyFileRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private static string schemas = DbSchemas.EPIC_COMPANY_SHARES + ".";
        private string PROC_POLICY_FILE_ADD = schemas + "PKG_CPS_POLICY_FILE.PROC_POLICY_FILE_ADD";
        private string PROC_POLICY_FILE_GET = schemas + "PKG_CPS_POLICY_FILE.PROC_POLICY_FILE_GET";
        private string PROC_POLICY_FILE_DELETE = schemas + "PKG_CPS_POLICY_FILE.PROC_POLICY_FILE_DELETE";
        private string PROC_POLICY_FILE_UPDATE = schemas + "PKG_CPS_POLICY_FILE.PROC_POLICY_FILE_UPDATE";
        private string PROC_POLICY_FILE_GET_ALL = schemas + "PKG_CPS_POLICY_FILE.PROC_POLICY_FILE_GET_ALL";

        public CpsPolicyFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PagingResult<CpsPolicyFile> FindAllPolicyFile(int secondaryID, int? tradingProvider, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<CpsPolicyFile>(PROC_POLICY_FILE_GET_ALL, new
            {
                pv_SECONDARY_ID = secondaryID,
                pv_TRADING_PROVIDER_ID = tradingProvider,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public CpsPolicyFile FindPolicyFileById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<CpsPolicyFile>(PROC_POLICY_FILE_GET, new
            {
                pv_POLICY_FILE_ID = id,
            });
            return result;
        }

        public CpsPolicyFile Add(CpsPolicyFile entity)
        {
            _logger.LogInformation("Add PolicyFile");
            var result = _oracleHelper.ExecuteProcedureToFirst<CpsPolicyFile>(PROC_POLICY_FILE_ADD, new
            {
                pv_SECONDARY_ID = entity.SecondaryId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                pv_EXPIRATION_DATE = entity.ExpirationDate,
                pv_EFFECTIVE_DATE = entity.EffectiveDate,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int PolicyFileUpdate(CpsPolicyFile entity)
        {
            _logger.LogInformation("Update PolicylFile");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_POLICY_FILE_UPDATE, new
            {
                pv_POLICY_FILE_ID = entity.Id,
                pv_SECONDARY_ID = entity.SecondaryId,
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

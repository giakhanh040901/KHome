using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class DistriPolicyFileRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_DISTRI_POLICY_FILE_ADD = "PKG_INV_DISTRI_POLICY_FILE.PROC_DISTRI_POLICY_FILE_ADD";
        private const string PROC_DISTRI_POLICY_FILE_GET = "PKG_INV_DISTRI_POLICY_FILE.PROC_DISTRI_POLICY_FILE_GET";
        private const string PROC_DISTRI_POLICY_FILE_DELETE = "PKG_INV_DISTRI_POLICY_FILE.PROC_DISTRI_POLICY_FILE_DELETE";
        private const string PROC_DISTRI_POLICY_FILE_UPDATE = "PKG_INV_DISTRI_POLICY_FILE.PROC_DISTRI_POLICY_FILE_UPDATE";
        private const string PROC_DIS_POLICY_FILE_GET_ALL = "PKG_INV_DISTRI_POLICY_FILE.PROC_DIS_POLICY_FILE_GET_ALL";

        public DistriPolicyFileRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PagingResult<DistriPolicyFile> FindAllDistriPolicyFile(int distributionId, int? tradingProvider, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<DistriPolicyFile>(PROC_DIS_POLICY_FILE_GET_ALL, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TRADING_PROVIDER_ID = tradingProvider,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public DistriPolicyFile FindDistriPolicyFileById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<DistriPolicyFile>(PROC_DISTRI_POLICY_FILE_GET, new
            {
                pv_ID = id,
            });
            return result;
        }

        public DistriPolicyFile Add(DistriPolicyFile entity)
        {
            _logger.LogInformation("Add DistriPolicyFile");
            var result = _oracleHelper.ExecuteProcedureToFirst<DistriPolicyFile>(PROC_DISTRI_POLICY_FILE_ADD, new
            {
                pv_DISTRIBUTION_ID = entity.DistributionId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                pv_EXPIRATION_DATE = entity.ExpirationDate,
                pv_EFFECTIVE_DATE = entity.EffectiveDate,
                SESSION_USERNAME = entity.CreatedBy,
            });
            return result;
        }

        public int DistriPolicyFileUpdate(DistriPolicyFile entity)
        {
            _logger.LogInformation("Update DistriPolicylFile");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRI_POLICY_FILE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_DISTRIBUTION_ID = entity.DistributionId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_NAME = entity.Name,
                pv_URL = entity.Url,
                pv_EXPIRATION_DATE = entity.ExpirationDate,
                pv_EFFECTIVE_DATE = entity.EffectiveDate,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int DeleteDistriPolicyFile(int id, int tradingProvider)
        {
            _logger.LogInformation($"Delete DistriPolicyFile ");
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DISTRI_POLICY_FILE_DELETE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProvider
            });
        }
    }
}

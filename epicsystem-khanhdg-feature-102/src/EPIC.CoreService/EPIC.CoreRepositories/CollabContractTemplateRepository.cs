using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class CollabContractTemplateRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_COLLAB_CONTRACT_TEMP_ADD = "PKG_CORE_COLLAB_CONTRACT_TEMP.PROC_COLLAB_CONTRACT_TEMP_ADD";
        private const string PROC_COLLAB_CONTRACT_TEMP_DEL = "PKG_CORE_COLLAB_CONTRACT_TEMP.PROC_COLLAB_CONTRACT_TEMP_DEL";
        private const string PROC_COLLAB_CONTRACT_TEMP_GET = "PKG_CORE_COLLAB_CONTRACT_TEMP.PROC_COLLAB_CONTRACT_TEMP_GET";
        private const string PROC_COL_CONTRACT_TEMP_UPDATE = "PKG_CORE_COLLAB_CONTRACT_TEMP.PROC_COL_CONTRACT_TEMP_UPDATE";
        private const string PROC_COLLAB_CONTRACT_TEMP_ALL = "PKG_CORE_COLLAB_CONTRACT_TEMP.PROC_COLLAB_CONTRACT_TEMP_ALL";

        public CollabContractTemplateRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public CollabContractTemp Add(CollabContractTemp entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CollabContractTemp>(
                PROC_COLLAB_CONTRACT_TEMP_ADD, new
                {
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_TITLE = entity.Title,
                    pv_FILE_URL = entity.FileUrl,
                    pv_TYPE = entity.Type,
                    SESSION_USERNAME = entity.CreatedBy
                });
        }

        public PagingResult<ViewCollabContractTempDto> FindAll(int pageSize, int pageNumber, string status, string keyword, int TradingProviderId, string type)
        {
            return _oracleHelper.ExecuteProcedurePaging<ViewCollabContractTempDto>(PROC_COLLAB_CONTRACT_TEMP_ALL, new
            {
                pv_TRADING_PROVIDER_ID = TradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEYWORD = keyword,
                pv_STATUS = status,
                pv_TYPE = type,
            });
        }

        public ViewCollabContractTempDto FindById(int id, int? TradingProviderId)
        {

            var result = _oracleHelper.ExecuteProcedureToFirst<ViewCollabContractTempDto>(PROC_COLLAB_CONTRACT_TEMP_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
            });
            return result;
        }

        public int Update(CollabContractTemp entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_COL_CONTRACT_TEMP_UPDATE, new
                {
                    pv_ID = entity.Id,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_TITLE = entity.Title,
                    pv_FILE_URL = entity.FileUrl,
                    pv_TYPE = entity.Type,
                    SESSION_USERNAME = entity.ModifiedBy
                });
        }
        public int Delete(int id, int TradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_COLLAB_CONTRACT_TEMP_DEL, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = TradingProviderId,
            });
        }
    }
}

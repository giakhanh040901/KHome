using EPIC.DataAccess;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Sale;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class SaleCollabContractRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_COLLAB_CONTRACT_ADD = "PKG_CORE_SALE.PROC_COLLAB_CONTRACT_ADD";
        private const string PROC_COLLAB_CONTRACT_FIND = "PKG_CORE_SALE.PROC_COLLAB_CONTRACT_FIND";
        private const string PROC_COLLAB_CONTRACT_ALL = "PKG_CORE_SALE.PROC_COLLAB_CONTRACT_ALL";
        private const string PROC_COLLAB_CONTRACT_UPDATE = "PKG_CORE_SALE.PROC_COLLAB_CONTRACT_UPDATE";
        private const string PROC_COLLAB_CONTRACT_FIND_BY_ID = "PKG_CORE_SALE.PROC_COLLAB_CONTRACT_BY_ID";

        public SaleCollabContractRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public CollabContract Add(CollabContract entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<CollabContract>(PROC_COLLAB_CONTRACT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_SALE_ID = entity.SaleId,
                pv_COLLAB_CONTRACT_TEMP_ID = entity.CollabContractTempId,
                pv_FILE_TEMP_URL = entity.FileTempUrl,
                pv_FILE_SIGNATURE_URL = entity.FileSignatureUrl,
                pv_FILE_SCAN_URL = entity.FileScanUrl,
                pv_PAGE_SIGN = entity.PageSign,
                pv_IS_SIGN = entity.IsSign,
                SESSION_USERNAME = entity.CreatedBy
            });
            return result;
        }

        public int Update(CollabContract entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_COLLAB_CONTRACT_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_SALE_ID = entity.SaleId,
                pv_COLLAB_CONTRACT_TEMP_ID = entity.CollabContractTempId,
                pv_FILE_TEMP_URL = entity.FileTempUrl,
                pv_FILE_SIGNATURE_URL = entity.FileSignatureUrl,
                pv_FILE_SCAN_URL = entity.FileScanUrl,
                pv_PAGE_SIGN = entity.PageSign,
                pv_IS_SIGN = entity.IsSign,
                SESSION_USERNAME = entity.ModifiedBy
            });
            return result;
        }

        public List<AppCollabContractSignDto> FindAllList(int saleId, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<AppCollabContractSignDto>(PROC_COLLAB_CONTRACT_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SALE_ID = saleId,
            }).ToList();
            return result;
        }

        public CollabContract Find(int saleId, int tradingProviderId, int collabContractTemplateId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<CollabContract>(PROC_COLLAB_CONTRACT_FIND, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SALE_ID = saleId,
                pv_COLLAB_CONTRACT_TEMP_ID = collabContractTemplateId
            });
            return result;
        }

        public CollabContract FindById(int id, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<CollabContract>(PROC_COLLAB_CONTRACT_FIND_BY_ID, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
            return result;
        }
    }
}

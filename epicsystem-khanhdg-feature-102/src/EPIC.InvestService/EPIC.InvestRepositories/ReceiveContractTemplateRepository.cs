
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.InvestRepositories
{
    public class ReceiveContractTemplateRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private static string PROC_RECEIVE_TEMPLATE_ADD = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_RECEIVE_TEMPLATE_ADD";
        private static string PROC_RECEIVE_TEMPLATE_UPDATE = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_RECEIVE_TEMPLATE_UPDATE";
        private static string PROC_RECEIVE_TEMPLATE_DELETE = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_RECEIVE_TEMPLATE_DELETE";
        private static string PROC_RECEIVE_TEMPLATE_FIND = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_RECEIVE_TEMPLATE_FIND";
        private static string PROC_RECEIVE_TEMPLATE_GET = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_RECEIVE_TEMPLATE_GET";
        private static string PROC_RECEIVE_TEMPLATE_GET_DETECTIVE = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_RECEIVE_TEMPLATE_GET_DETECTIVE";
        private static string PROC_CHANGE_STATUS = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_CHANGE_STATUS";
        private static string PROC_RECEIVE_TEMPLATE_FIND_ALL = "PKG_INV_RECEIVE_CONTRACT_TEMP.PROC_RECEIVE_TEMPLATE_FIND_ALL";

        public ReceiveContractTemplateRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public ReceiveContractTemplate FindAll(int distributionId, int tradingProviderId)
        {
            _logger.LogInformation("FindAll ContractTemplate - SQL: {}", PROC_RECEIVE_TEMPLATE_FIND);
            return _oracleHelper.ExecuteProcedureToFirst<ReceiveContractTemplate>(PROC_RECEIVE_TEMPLATE_FIND, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public ReceiveContractTemplate FindById(int id, int tradingProviderId)
        {
            _logger.LogInformation("FindById receive ContractTemplate - SQL: {}", PROC_RECEIVE_TEMPLATE_GET);
            var result = _oracleHelper.ExecuteProcedureToFirst<ReceiveContractTemplate>(PROC_RECEIVE_TEMPLATE_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public ReceiveContractTemplate Add(ReceiveContractTemplate entity)
        {
            _logger.LogInformation("Add Receive ContractTemplate - SQL: {}", PROC_RECEIVE_TEMPLATE_ADD);
            return _oracleHelper.ExecuteProcedureToFirst<ReceiveContractTemplate>(
                PROC_RECEIVE_TEMPLATE_ADD,
                new
                {
                    pv_CODE = entity.Code,
                    pv_NAME = entity.Name,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_FILE_URL = entity.FileUrl,
                    pv_DISTRIBUTION_ID = entity.DistributionId,
                    SESSION_USERNAME = entity.CreatedBy,
                    pv_STATUS = entity.Status,
                }
             );
        }

        public int Update(ReceiveContractTemplate entity)
        {
            _logger.LogInformation("Update receive ContractTemplate - SQL: {}", PROC_RECEIVE_TEMPLATE_UPDATE);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_RECEIVE_TEMPLATE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_FILE_URL = entity.FileUrl,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation("Delete Receive ContractTemplate - SQL: {}", PROC_RECEIVE_TEMPLATE_DELETE);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_RECEIVE_TEMPLATE_DELETE, new
            {
                pv_ID = id
            });
            return result;
        }

        public int UpdateStatus(int id, string status, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                    PROC_CHANGE_STATUS, new
                    {
                        pv_ID = id,
                        pv_STATUS = status,
                        SESSION_USERNAME = modifiedBy
                    }, false);
        }

        /// <summary>
        /// Lấy danh sách hợp đồng theo distributionId
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<ReceiveContractTemplate> GetAllByDistributionId(int distributionId, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<ReceiveContractTemplate>(PROC_RECEIVE_TEMPLATE_FIND, new
            {
                pv_DISTRIBUTION_ID = distributionId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result.ToList();
        }

        public ReceiveContractTemplate FindDetectiveById(int id, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ReceiveContractTemplate>(PROC_RECEIVE_TEMPLATE_GET_DETECTIVE, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        
        public PagingResult<ReceiveContractTemplate> GetAll(int tradingProviderId, int pageSize, int pageNumber, string keyword, int distributionId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ReceiveContractTemplate>(PROC_RECEIVE_TEMPLATE_FIND_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_DISTRIBUTION_ID = distributionId,
            });
            return result;
        }
    }
}

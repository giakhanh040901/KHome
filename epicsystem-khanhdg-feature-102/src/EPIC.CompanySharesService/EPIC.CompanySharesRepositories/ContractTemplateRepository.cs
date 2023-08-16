
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;

namespace EPIC.CompanySharesRepositories
{
    public class ContractTemplateRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private static string PROC_CONTRACT_TEMPLATE_ADD = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_ADD";
        private static string PROC_CONTRACT_TEMPLATE_UPDATE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_UPDATE";
        private static string PROC_CONTRACT_TEMPLATE_DELETE = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_DELETE";
        private static string PROC_CONTRACT_TEMPLATE_FIND = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_FIND";
        private static string PROC_CONTRACT_TEMPLATE_GET = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_GET";
        private static string PROC_CHANGE_STATUS = DbSchemas.EPIC_COMPANY_SHARES + ".PKG_CPS_CONTRACT_TEMPLATE.PROC_CHANGE_STATUS";

        public ContractTemplateRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }


        public PagingResult<ContractTemplate> FindAll(int pageSize, int pageNumber, string keyword, int secondaryId, int tradingProviderId, string type)
        {
            _logger.LogInformation("FindAll ContractTemplate - SQL: {}", PROC_CONTRACT_TEMPLATE_FIND);
            var result = _oracleHelper.ExecuteProcedurePaging<ContractTemplate>(PROC_CONTRACT_TEMPLATE_FIND, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_SECONDARY_ID = secondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TYPE = type
            });
            return result;
        }

        public ContractTemplate FindById(int id, int tradingProviderId)
        {
            _logger.LogInformation("FindById ContractTemplate - SQL: {}", PROC_CONTRACT_TEMPLATE_GET);
            var result = _oracleHelper.ExecuteProcedureToFirst<ContractTemplate>(PROC_CONTRACT_TEMPLATE_GET, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public ContractTemplate Add(ContractTemplate entity)
        {
            _logger.LogInformation("Add ContractTemplate - SQL: {}", PROC_CONTRACT_TEMPLATE_ADD);
            return _oracleHelper.ExecuteProcedureToFirst<ContractTemplate>(
                PROC_CONTRACT_TEMPLATE_ADD,
                new
                {
                    pv_CODE = entity.Code,
                    pv_NAME = entity.Name,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_CONTRACT_TEMP_URL = entity.ContractTempUrl,
                    pv_SECONDARY_ID = entity.SecondaryId,
                    pv_TYPE = entity.Type,
                    pv_DISPLAY_TYPE = entity.DisplayType,
                    SESSION_USERNAME = entity.CreatedBy
                }
             );
        }

        public int Update(ContractTemplate entity)
        {
            _logger.LogInformation("Update ContractTemplate - SQL: {}", PROC_CONTRACT_TEMPLATE_UPDATE);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_TEMPLATE_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_CONTRACT_TEMP_URL = entity.ContractTempUrl,
                pv_TYPE = entity.Type,
                pv_DISPLAY_TYPE = entity.DisplayType,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation("Delete ContractTemplate - SQL: {}", PROC_CONTRACT_TEMPLATE_DELETE);
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_TEMPLATE_DELETE, new
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
    }
}

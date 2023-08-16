using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractTemplate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondContractTemplateRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private const string ADD_EP_CONTRACT_TEMPLATE_PROC = "PKG_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_ADD";
        private const string UPDATE_EP_CONTRACT_TEMPLATE_PROC = "PKG_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_UPDATE";
        private const string DELETE_EP_CONTRACT_TEMPLATE_PROC = "PKG_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_DELETE";
        private const string GET_ALL_EP_CONTRACT_TEMPLATE_PROC = "PKG_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_FIND";
        private const string GET_ALL_EP_CONTRACT_TEMPLATE_BY_ORDER_PROC = "PKG_CONTRACT_TEMPLATE.PROC_FIND_BY_TYPE";
        private const string GET_EP_CONTRACT_TEMPLATE_PROC = "PKG_CONTRACT_TEMPLATE.PROC_CONTRACT_TEMPLATE_GET";
        private const string PROC_CHANGE_STATUS = "PKG_CONTRACT_TEMPLATE.PROC_CHANGE_STATUS";

        public BondContractTemplateRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public PagingResult<BondContractTemplate> FindAll(int pageSize, int pageNumber, string keyword, int bondSecondaryId, int tradingProviderId, int? classify, string type)
        {
            _logger.LogInformation("FindAll ContractTemplate - SQL: {}", GET_ALL_EP_CONTRACT_TEMPLATE_PROC);
            var result = _oracleHelper.ExecuteProcedurePaging<BondContractTemplate>(GET_ALL_EP_CONTRACT_TEMPLATE_PROC, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_CLASSIFY = classify,
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_TYPE = type
            });
            return result;
        }


        public List<BondContractTemplate> Filter(Func<Predicate<BondContractTemplate>, bool> expression)
        {
            return new List<BondContractTemplate>();
        }

        public BondContractTemplate FindById(int id, int tradingProviderId)
        {
            _logger.LogInformation("FindById ContractTemplate - SQL: {}", GET_EP_CONTRACT_TEMPLATE_PROC);
            var result = _oracleHelper.ExecuteProcedureToFirst<BondContractTemplate>(GET_EP_CONTRACT_TEMPLATE_PROC, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public BondContractTemplate Add(BondContractTemplate entity)
        {
            _logger.LogInformation("Add ContractTemplate - SQL: {}", ADD_EP_CONTRACT_TEMPLATE_PROC);
            return _oracleHelper.ExecuteProcedureToFirst<BondContractTemplate>(
                ADD_EP_CONTRACT_TEMPLATE_PROC,
                new
                {
                    pv_CODE = entity.Code,
                    pv_NAME = entity.Name,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_CONTRACT_TYPE = entity.ContractType,
                    pv_CONTRACT_TEMP_URL = entity.ContractTempUrl,
                    pv_CLASSIFY = entity.Classify,
                    pv_BOND_SECONDARY_ID = entity.SecondaryId,
                    pv_TYPE = entity.Type,
                    SESSION_USERNAME = entity.CreatedBy
                }
             );
        }

        public int Update(BondContractTemplate entity)
        {
            _logger.LogInformation("Update ContractTemplate - SQL: {}", UPDATE_EP_CONTRACT_TEMPLATE_PROC);
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_EP_CONTRACT_TEMPLATE_PROC, new
            {
                pv_ID = entity.Id,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_CONTRACT_TYPE = entity.ContractType,
                pv_CONTRACT_TEMP_URL = entity.ContractTempUrl,
                pv_CLASSIFY = entity.Classify,
                pv_TYPE = entity.Type,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public int Delete(int id)
        {
            _logger.LogInformation("Delete ContractTemplate - SQL: {}", DELETE_EP_CONTRACT_TEMPLATE_PROC);
            var result = _oracleHelper.ExecuteProcedureNonQuery(DELETE_EP_CONTRACT_TEMPLATE_PROC, new
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

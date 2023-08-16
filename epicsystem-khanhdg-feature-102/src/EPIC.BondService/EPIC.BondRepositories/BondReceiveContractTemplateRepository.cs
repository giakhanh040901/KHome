using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EPIC.BondRepositories
{
    public class BondReceiveContractTemplateRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string ADD_EP_CONTRACT_TEMPLATE_PROC = "PKG_BOND_RECEIVE_CONTR_TEMP.PROC_RECEIVE_TEMPLATE_ADD";
        private static string UPDATE_EP_CONTRACT_TEMPLATE_PROC = "PKG_BOND_RECEIVE_CONTR_TEMP.PROC_RECEIVE_TEMPLATE_UPDATE";
        private static string DELETE_EP_CONTRACT_TEMPLATE_PROC = "PKG_BOND_RECEIVE_CONTR_TEMP.PROC_RECEIVE_TEMPLATE_DELETE";
        private static string GET_ALL_EP_CONTRACT_TEMPLATE_PROC = "PKG_BOND_RECEIVE_CONTR_TEMP.PROC_RECEIVE_TEMPLATE_FIND";
        private static string GET_EP_CONTRACT_TEMPLATE_PROC = "PKG_BOND_RECEIVE_CONTR_TEMP.PROC_RECEIVE_TEMPLATE_GET";
        private static string PROC_CHANGE_STATUS = "PKG_BOND_RECEIVE_CONTR_TEMP.PROC_CHANGE_STATUS";

        public BondReceiveContractTemplateRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public List<BondReceiveContractTemplate> GetAll()
        {
            throw new NotImplementedException();
        }

        public BondReceiveContractTemplate FindAll(int bondSecondaryId, int tradingProviderId)
        {
            _logger.LogInformation("FindAll ContractTemplate - SQL: {}", GET_ALL_EP_CONTRACT_TEMPLATE_PROC);
            return _oracleHelper.ExecuteProcedureToFirst<BondReceiveContractTemplate>(GET_ALL_EP_CONTRACT_TEMPLATE_PROC, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }


        public List<BondReceiveContractTemplate> Filter(Func<Predicate<BondReceiveContractTemplate>, bool> expression)
        {
            return new List<BondReceiveContractTemplate>();
        }

        public BondReceiveContractTemplate FindById(int id, int tradingProviderId)
        {
            _logger.LogInformation("FindById receive ContractTemplate - SQL: {}", GET_EP_CONTRACT_TEMPLATE_PROC);
            var result = _oracleHelper.ExecuteProcedureToFirst<BondReceiveContractTemplate>(GET_EP_CONTRACT_TEMPLATE_PROC, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
            return result;
        }

        public BondReceiveContractTemplate Add(BondReceiveContractTemplate entity)
        {
            _logger.LogInformation("Add Receive ContractTemplate - SQL: {}", ADD_EP_CONTRACT_TEMPLATE_PROC);
            return _oracleHelper.ExecuteProcedureToFirst<BondReceiveContractTemplate>(
                ADD_EP_CONTRACT_TEMPLATE_PROC,
                new
                {
                    pv_CODE = entity.Code,
                    pv_NAME = entity.Name,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_FILE_URL = entity.FileUrl,
                    pv_BOND_SECONDARY_ID = entity.SecondaryId,
                    SESSION_USERNAME = entity.CreatedBy
                }
             );
        }

        public int Update(BondReceiveContractTemplate entity)
        {
            _logger.LogInformation("Update receive ContractTemplate - SQL: {}", UPDATE_EP_CONTRACT_TEMPLATE_PROC);
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_EP_CONTRACT_TEMPLATE_PROC, new
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
            _logger.LogInformation("Delete Receive ContractTemplate - SQL: {}", DELETE_EP_CONTRACT_TEMPLATE_PROC);
            var result = _oracleHelper.ExecuteProcedureNonQuery(DELETE_EP_CONTRACT_TEMPLATE_PROC, new
            {
                pv_ID = id
            });
            return result;
        }

        public PagingResult<BondReceiveContractTemplate> FindAll(int pageSize, int pageNumber, string keyword)
        {
            throw new NotImplementedException();
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

        public BondReceiveContractTemplate FindById(int id)
        {
            throw new NotImplementedException();
        }

    }
}

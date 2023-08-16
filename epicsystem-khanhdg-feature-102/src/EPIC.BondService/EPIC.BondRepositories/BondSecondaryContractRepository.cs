using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.OrderPayment;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondSecondaryContractRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_SECONDARY_CONTRACT_ADD = "PKG_BOND_SECONDARY_CONTRACT.PROC_SECONDARY_CONTRACT_ADD";
        private const string PROC_SECONDARY_CONTRACT_FIND = "PKG_BOND_SECONDARY_CONTRACT.PROC_SECONDARY_CONTRACT_FIND";
        private const string PROC_SECONDARY_CONTRACT_ALL = "PKG_BOND_SECONDARY_CONTRACT.PROC_SECONDARY_CONTRACT_ALL";
        private const string PROC_SECONDARY_CONTRACT_UPDATE = "PKG_BOND_SECONDARY_CONTRACT.PROC_SECONDARY_CONTRACT_UPDATE";
        private const string PROC_SECONDARY_CONTRACT_FIND_BY_ID = "PKG_BOND_SECONDARY_CONTRACT.PROC_SECONDARY_CONTRACT_BY_ID";
        private const string PROC_SECONDARY_CONTRACT_UPDATE_BY_ORDER_ID = "PKG_BOND_SECONDARY_CONTRACT.PROC_SECONDARY_CONTRACT_BY_OR_ID";

        public BondSecondaryContractRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public BondSecondaryContract Add(BondSecondaryContract entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondSecondaryContract>(PROC_SECONDARY_CONTRACT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_CONTRACT_TEMP_ID = entity.ContractTempId,
                pv_FILE_TEMP_URL = entity.FileTempUrl,
                pv_FILE_SIGNATURE_URL = entity.FileSignatureUrl,
                pv_FILE_SCAN_URL = entity.FileScanUrl,
                pv_PAGE_SIGN = entity.PageSign,
                SESSION_USERNAME = entity.CreatedBy
            });
            return result;
        }

        public int Update(BondSecondaryContract entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SECONDARY_CONTRACT_UPDATE, new
            {
                pv_SECONDARY_CONTRACT_FILE_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_CONTRACT_TEMP_ID = entity.ContractTempId,
                pv_FILE_TEMP_URL = entity.FileTempUrl,
                pv_FILE_SIGNATURE_URL = entity.FileSignatureUrl,
                pv_FILE_SCAN_URL = entity.FileScanUrl,
                pv_IS_SIGN = entity.IsSign,
                pv_PAGE_SIGN = entity.PageSign,
                SESSION_USERNAME = entity.ModifiedBy
            });
            return result;
        }

        public List<BondSecondaryContract> FindAll(long orderId, int tradingProviderId, string isSign = null)
        {
            var result = _oracleHelper.ExecuteProcedure<BondSecondaryContract>(PROC_SECONDARY_CONTRACT_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                pv_IS_SIGN = isSign,
            }).ToList();
            return result;
        }

        public BondSecondaryContract Find(int orderId, int tradingProviderId, int contractTemplateId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondSecondaryContract>(PROC_SECONDARY_CONTRACT_FIND, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                pv_CONTRACT_TEMP_ID = contractTemplateId
            });
            return result;
        }

        public BondSecondaryContract FindById(int secondaryContractFileId, int? investorId = null, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondSecondaryContract>(PROC_SECONDARY_CONTRACT_FIND_BY_ID, new
            {
                pv_SECONDARY_CONTRACT_FILE_ID = secondaryContractFileId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVESTOR_ID = investorId,
            });
            return result;
        }

        public int UpdateIsSignByOrderId(int orderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_SECONDARY_CONTRACT_UPDATE_BY_ORDER_ID, new
            {
                pv_ORDER_ID = orderId,
            });
            return result;
        }
    }
}

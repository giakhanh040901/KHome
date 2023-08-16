using Dapper.Oracle;
using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.DistributionContractPayment;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondDistributionContractRepository 
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_CONTRACT_ADD = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_ADD";
        private const string PROC_CONTRACT_UPDATE = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_UPDATE";
        private const string PROC_CONTRACT_DELETE = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_DELETE";
        private const string PROC_CONTRACT_GET = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_GET";
        private const string PROC_CONTRACT_FIND_BY_ID = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_FIND_BY_ID";
        private const string PROC_CONTRACT_GET_BY_PRIMARY_ID = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_GET_PRIMARY_ID";
        private const string PROC_CONTRACT_GET_ALL = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_GET_ALL";
        private const string PROC_CONTRACT_DUYET = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_DUYET";

        private const string PROC_CONTRACT_PAYMENT_ADD = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_PAYMENT_ADD";
        private const string PROC_CONTRACT_PAYMENT_UPDATE = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_PAYMENT_UPDATE";
        private const string PROC_CONTRACT_PAYMENT_DELETE = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_PAYMENT_DELETE";
        private const string PROC_CONTRACT_PAYMENT_GET = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_PAYMENT_GET";
        private const string PROC_CONTRACT_PAYMENT_GET_ALL = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_PAYMENT_GET_ALL";
        private const string PROC_CONTRACT_PAYMENT_APPROVE = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_PAYMENT_APPROVE";
        private const string PROC_CONTRACT_PAYMENT_CANCEL = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_PAYMENT_CANCEL";

        private const string PROC_CONTRACT_FILE_ADD = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_FILE_ADD";
        private const string PROC_CONTRACT_FILE_GET = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_FILE_GET";
        private const string PROC_CONTRACT_FILE_GET_ALL = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_FILE_GET_ALL";
        private const string PROC_CONTRACT_FILE_APPROVE = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_FILE_APPROVE";
        private const string PROC_CONTRACT_FILE_CANCEL = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_FILE_CANCEL";
        private const string PROC_CONTRACT_FILE_DELETE = "PKG_DISTRIBUTION_CONTRACT.PROC_CONTRACT_FILE_DELETE";

        private const string PROC_SUM_QUANTITY = "PKG_DISTRIBUTION_CONTRACT.PROC_SUM_QUANTITY";
        private const string PROC_SUM_TOTAL_VALUE = "PKG_DISTRIBUTION_CONTRACT.PROC_SUM_TOTAL_VALUE";
        private const string PROC_APP_CONTRACT_FILE_FIND = "PKG_DISTRIBUTION_CONTRACT.PROC_APP_CONTRACT_FILE_FIND";

        public BondDistributionContractRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public BondDistributionContract Add(BondDistributionContract entity)
        {
            _logger.LogInformation("Add DistributionContract");
            var result = _oracleHelper.ExecuteProcedureToFirst<BondDistributionContract>(PROC_CONTRACT_ADD, new
            {
                pv_PARTNER_ID = entity.PartnerId,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_BOND_PRIMARY_ID = entity.BondPrimaryId,
                pv_QUANTITY = entity.Quantity,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_DATE_BUY = entity.DateBuy,
                SESSION_USERNAME = entity.CreatedBy
            });
            return result;
        } 

        public int Delete(int id, int partnerId)
        {
            _logger.LogInformation($"Delete DistributionContract ");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_DELETE, new
            {
                pv_PARTNER_ID = partnerId,
                pv_DISTRIBUTION_CONTRACT_ID = id
            });
            return rslt;
        }

        public int Duyet(int id)
        {
            _logger.LogInformation($"Duyet DistributionContract ");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_DUYET, new
            {
                pv_DISTRIBUTION_CONTRACT_ID = id
            });
            return rslt;
        }

        public PagingResult<DistributionContractDto> FindAllContract(int partnerId, int pageSize, int pageNumber, string keyword, int? status)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<DistributionContractDto>(PROC_CONTRACT_GET_ALL, new
            {
                pv_PARTNER_ID = partnerId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status
            });
            return result;
        }

        public BondDistributionContract FindById(int id, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondDistributionContract>(PROC_CONTRACT_GET, new
            {
                pv_PARTNER_ID = partnerId,
                pv_DISTRIBUTION_CONTRACT_ID = id,
            });
            return result;
        }

        public DistributionContractDto FindContractById(decimal id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<DistributionContractDto>(PROC_CONTRACT_FIND_BY_ID, new
            {
                pv_DISTRIBUTION_CONTRACT_ID = id,
            });
            return result;
        }

        public BondDistributionContract FindContractByPrimaryId(decimal id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondDistributionContract>(PROC_CONTRACT_GET_BY_PRIMARY_ID, new
            {
                pv_BOND_PRIMARY_ID = id,
            });
            return result;
        }

        public int Update(BondDistributionContract entity)
        {
            _logger.LogInformation("Update DistributionContract");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_UPDATE, new
            {
                pv_PARTNER_ID = entity.PartnerId,
                pv_DISTRIBUTION_CONTRACT_ID = entity.Id,
                pv_QUANTITY = entity.Quantity,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_DATE_BUY = entity.DateBuy,
                SESSION_USERNAME = entity.ModifiedBy
            });
            return result;
        }

        public BondDistributionContractPayment Add(BondDistributionContractPayment entity)
        {
            _logger.LogInformation("Add DistributionContract");
            var result = _oracleHelper.ExecuteProcedureToFirst<BondDistributionContractPayment>(PROC_CONTRACT_PAYMENT_ADD, new
            {
                pv_DISTRIBUTION_CONTRACT_ID = entity.DistributionContractId,
                pv_PAYMENT_TYPE = entity.PaymentType,
                pv_TRANSACTION_TYPE = entity.TransactionType,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_DESCRIPTION = entity.Description,
                SESSION_USERNAME = entity.CreatedBy,
                pv_TRADING_DATE = entity.TradingDate
            });
            return result;
        }

        public int DeleteContractPayment(int id)
        {
            _logger.LogInformation($"Delete Distribution Contract Payment ");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_PAYMENT_DELETE, new
            {
                pv_PAYMENT_ID = id
            });
            return rslt;
        }

        public PagingResult<DistributionContractPaymentDto> FindAllContractPayment(int contractId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<DistributionContractPaymentDto>(PROC_CONTRACT_PAYMENT_GET_ALL, new
            {
                pv_DISTRIBUTION_CONTRACT_ID = contractId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public int ContractPaymentApprove(int id, string approveBy)
        {
            _logger.LogInformation($"Duyet DistributionContract Payment ");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_PAYMENT_APPROVE, new
            {
                pv_PAYMENT_ID = id,
                SESSION_USERNAME = approveBy
            });
            return result;
        }

        public int ContractPaymentCancel(int id, string cancelBy)
        {
            _logger.LogInformation($"Duyet DistributionContract Payment ");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_PAYMENT_CANCEL, new
            {
                pv_PAYMENT_ID = id,
                SESSION_USERNAME = cancelBy
            });
            return result;
        }

        public BondDistributionContractPayment FindPaymentById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondDistributionContractPayment>(PROC_CONTRACT_PAYMENT_GET, new
            {
                pv_PAYMENT_ID = id,
            });
            return result;
        }

        public int UpdatePayment(BondDistributionContractPayment entity)
        {
            _logger.LogInformation("Update DistributionContract");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_PAYMENT_UPDATE, new
            {
                pv_PAYMENT_ID = entity.Id,
                pv_PAYMENT_TYPE = entity.TransactionType,
                pv_TRANSACTION_TYPE = entity.TransactionType,
                pv_TOTAL_VALUE = entity.TotalValue,
                pv_DESCRIPTION = entity.Description,
                SESSION_USERNAME = entity.ModifiedBy,
                pv_TRADING_DATE = entity.TradingDate
            });
            return result;
        }

        public DistributionContractFile Add(DistributionContractFile entity)
        {
            _logger.LogInformation("Add DistributionContractFile");
            var result = _oracleHelper.ExecuteProcedureToFirst<DistributionContractFile>(PROC_CONTRACT_FILE_ADD, new
            {
                pv_DISTRIBUTION_CONTRACT_ID = entity.DistributionContractId,
                pv_TITLE = entity.Title,
                pv_FILE_URL = entity.FileUrl,
                SESSION_USERNAME = entity.CreatedBy
            });
            return result;
        }

        public DistributionContractFile FindContractFileById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<DistributionContractFile>(PROC_CONTRACT_FILE_GET, new
            {
                pv_FILE_ID = id
            });
            return result;
        }

        public PagingResult<DistributionContractFile> FindAllContractFile( int contractId, int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<DistributionContractFile>(PROC_CONTRACT_FILE_GET_ALL, new
            {
                pv_DISTRIBUTION_CONTRACT_ID = contractId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return result;
        }

        public int ContractFileApprove(int id, string approveBy)
        {
            _logger.LogInformation($"Duyet DistributionContract File ");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_FILE_APPROVE, new
            {
                pv_FILE_ID = id,
                SESSION_USERNAME = approveBy
            });
            return result;
        }

        public int ContractFileCancel(int id, string cancelBy)
        {
            _logger.LogInformation($"Duyet DistributionContract File ");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_FILE_CANCEL, new
            {
                pv_FILE_ID = id,
                SESSION_USERNAME = cancelBy
            });
            return result;
        }

        public int DeleteContractFile(int id)
        {
            _logger.LogInformation($"Delete DistributionContract File ");
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_CONTRACT_FILE_DELETE, new
            {
                pv_FILE_ID = id
            });
            return result;
        }

        public decimal SumQuantity(int tradingProviderId, int bondPrimaryId)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_BOND_PRIMARY_ID", bondPrimaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_QUANTITY, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }

        public IEnumerable<DistributionContractFile> AppContractFileFind (int id, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<DistributionContractFile>(PROC_APP_CONTRACT_FILE_FIND, new
            {
                pv_BOND_PRIMARY_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
            return result;
        }
        /// <summary>
        /// ExportReport
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="bondPrimaryId"></param>
        /// <returns></returns>
        public decimal SumTotalValue(int tradingProviderId, int bondPrimaryId)
        {
            decimal result = TrueOrFalseNum.FALSE;
            OracleDynamicParameters parameters = new();
            parameters.Add("pv_TRADING_PROVIDER_ID", tradingProviderId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_BOND_PRIMARY_ID", bondPrimaryId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("pv_RESULT", result, OracleMappingType.Decimal, ParameterDirection.Output);
            _oracleHelper.ExecuteProcedureDynamicParams(PROC_SUM_TOTAL_VALUE, parameters);

            result = parameters.Get<decimal>("pv_RESULT");
            return (decimal)result;
        }
    }
}

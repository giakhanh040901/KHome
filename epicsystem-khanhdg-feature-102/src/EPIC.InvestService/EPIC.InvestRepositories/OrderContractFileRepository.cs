using EPIC.DataAccess;
using EPIC.InvestEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class OrderContractFileRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_ORDER_CONTRACT_ADD = "PKG_INV_ORDER_CONTRACT_FILE.PROC_ORDER_CONTRACT_ADD";
        private const string PROC_ORDER_CONTRACT_FIND = "PKG_INV_ORDER_CONTRACT_FILE.PROC_ORDER_CONTRACT_FIND";
        private const string PROC_ORDER_CONTRACT_ALL = "PKG_INV_ORDER_CONTRACT_FILE.PROC_ORDER_CONTRACT_ALL";
        private const string PROC_ORDER_CONTRACT_UPDATE = "PKG_INV_ORDER_CONTRACT_FILE.PROC_ORDER_CONTRACT_UPDATE";
        private const string PROC_ORDER_CONTRACT_FIND_BY_ID = "PKG_INV_ORDER_CONTRACT_FILE.PROC_ORDER_CONTRACT_BY_ID";
        private const string PROC_ORDER_CONTRACT_UPDATE_BY_ORDER_ID = "PKG_INV_ORDER_CONTRACT_FILE.PROC_ORDER_CONTRACT_BY_OR_ID";
        private const string PROC_ORDER_CONTRACT_DELETE_BY_ORDER_ID = "PKG_INV_ORDER_CONTRACT_FILE.PROC_DEL_CONTRACT_BY_OR_ID";
        private const string PROC_ORDER_CONTRACT_CODE = "PKG_INV_ORDER_CONTRACT_FILE.PROC_GET_ORDER_CONTRACT_CODE";

        public OrderContractFileRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        public OrderContractFile Add(OrderContractFile entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<OrderContractFile>(PROC_ORDER_CONTRACT_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_CONTRACT_TEMP_ID = entity.ContractTempId,
                pv_FILE_TEMP_URL = entity.FileTempUrl,
                pv_FILE_TEMP_PDF_URL = entity.FileTempPdfUrl,
                pv_FILE_SIGNATURE_URL = entity.FileSignatureUrl,
                pv_FILE_SCAN_URL = entity.FileScanUrl,
                pv_PAGE_SIGN = entity.PageSign,
                pv_WITHDRAWAL_ID = entity.WithdrawalId,
                pv_RENEWALS_ID = entity.RenewalsId,
                pv_TIMES = entity.Times,
                pv_CONTRACT_CODE_GEN = entity.ContractCodeGen,
                SESSION_USERNAME = entity.CreatedBy
            });
            return result;
        }

        public int Update(OrderContractFile entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_CONTRACT_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ORDER_ID = entity.OrderId,
                pv_CONTRACT_TEMP_ID = entity.ContractTempId,
                pv_FILE_TEMP_URL = entity.FileTempUrl,
                pv_FILE_TEMP_PDF_URL = entity.FileTempPdfUrl,
                pv_FILE_SIGNATURE_URL = entity.FileSignatureUrl,
                pv_FILE_SCAN_URL = entity.FileScanUrl,
                pv_IS_SIGN = entity.IsSign,
                pv_PAGE_SIGN = entity.PageSign,
                pv_WITHDRAWAL_ID = entity.WithdrawalId,
                pv_RENEWALS_ID = entity.RenewalsId,
                pv_TIMES = entity.Times,
                pv_CONTRACT_CODE_GEN = entity.ContractCodeGen,
                SESSION_USERNAME = entity.ModifiedBy
            });
            return result;
        }

        public List<OrderContractFile> FindAll(long orderId, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<OrderContractFile>(PROC_ORDER_CONTRACT_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
            }).ToList();
            return result;
        }

        public OrderContractFile Find(long orderId, int tradingProviderId, int contractTemplateId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<OrderContractFile>(PROC_ORDER_CONTRACT_FIND, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                pv_CONTRACT_TEMP_ID = contractTemplateId
            });
            return result;
        }

        /// <summary>
        /// Get danh sách hợp đồng theo id lệnh, id mẫu hợp đồng ( 1 mẫu rút tiền có thể có nhiều hợp đồng rút)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public List<OrderContractFile> FindAllByContractTemplate(int orderId, int tradingProviderId, int contractTemplateId)
        {
            var result = _oracleHelper.ExecuteProcedure<OrderContractFile>(PROC_ORDER_CONTRACT_FIND, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ORDER_ID = orderId,
                pv_CONTRACT_TEMP_ID = contractTemplateId
            });
            return result.ToList();
        }

        public OrderContractFile FindById(int id, int? investorId = null, int? tradingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<OrderContractFile>(PROC_ORDER_CONTRACT_FIND_BY_ID, new
            {
                pv_ID = id,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_INVESTOR_ID = investorId,
            });
            return result;
        }

        public int UpdateIsSignByOrderId(int orderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_CONTRACT_UPDATE_BY_ORDER_ID, new
            {
                pv_ORDER_ID = orderId,
            });
            return result;
        }

        public int DeleteByOrderId(int orderId)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_ORDER_CONTRACT_DELETE_BY_ORDER_ID, new
            {
                pv_ORDER_ID = orderId,
            });
            return result;
        }

        public List<OrderContractFile> FindByContractCodeGen(string contractCode, int? trangdingProviderId = null)
        {
            var result = _oracleHelper.ExecuteProcedure<OrderContractFile>(PROC_ORDER_CONTRACT_CODE, new
            {
                pv_CONTRACT_CODE = contractCode,
                pv_TRADING_PROVIDER_ID = trangdingProviderId
            });
            return result.ToList();
        }
    }
}

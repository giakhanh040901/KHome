using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.SharedEntities.Dto.OperationalInfo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.SharedRepositories
{
    public class SharedDataBCTRepository : BaseRepository
    {
        #region Báo cáo Bộ công thương
        private const string PROC_GET_SELLERS = "PKG_DATA_BCT.PROC_GET_SELLERS";
        private const string PROC_GET_TOTAL_PRODUCTS = "PKG_DATA_BCT.PROC_GET_TOTAL_PRODUCTS";
        private const string PROC_GET_TRANSACTIONS = "PKG_DATA_BCT.PROC_GET_TRANSACTIONS";
        private const string PROC_GET_SUCCESSFUL_ORDERS = "PKG_DATA_BCT.PROC_GET_SUCCESSFUL_ORDERS";
        private const string PROC_GET_FAILED_ORDERS = "PKG_DATA_BCT.PROC_GET_FAILED_ORDERS";
        private const string PROC_GET_TOTAL_TRANSACTION_VALUE = "PKG_DATA_BCT.PROC_GET_TOTAL_TRANSACTION_VALUE";
        private const string PROC_TRADING_PROVIDER_FIND = "PKG_DATA_BCT.PROC_TRADING_PROVIDER_FIND";
        private const string PROC_REPORT_TRADING_PROVIDER_FIND = "PKG_DATA_BCT.PROC_REPORT_TRADING_PROVIDER_FIND";
        private const string PROC_REMOVE_REPORT_TRADING = "PKG_DATA_BCT.PROC_REMOVE_REPORT_TRADING";
        private const string PROC_ADD_REPORT_TRADING = "PKG_DATA_BCT.PROC_ADD_REPORT_TRADING";
        #endregion
        public SharedDataBCTRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        // Báo báo Bộ công thương
        /// <summary>
        /// Tổng số đại lý
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetSellers(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation($"GetSellers: startDate = {startDate}, endDate = {endDate}");
            var result = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_GET_SELLERS, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
            });
            return result;
        }

        public int GetTotalProducts(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation($"Get Total Products: startDate = {startDate}, endDate = {endDate}");
            var result = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_GET_TOTAL_PRODUCTS, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
            });
            return result;
        }

        public int GetTransactions(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation($"Get Transactions: startDate = {startDate}, endDate = {endDate}");
            var result = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_GET_TRANSACTIONS, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
            });
            return result;
        }

        public int GetSuccessfulOrders(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation($"Get Successful Orders: startDate = {startDate}, endDate = {endDate}");
            var result = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_GET_SUCCESSFUL_ORDERS, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
            });
            return result;
        }

        public int GetFailedOrders(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation($"Get Failed Orders: startDate = {startDate}, endDate = {endDate}");
            var result = _oracleHelper.ExecuteProcedureToFirst<int>(PROC_GET_FAILED_ORDERS, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
            });
            return result;
        }

        public string GetTotalTransactionValue(DateTime? startDate, DateTime? endDate)
        {
            _logger.LogInformation($"Get Total Transaction Value: startDate = {startDate}, endDate = {endDate}");
            var result = _oracleHelper.ExecuteProcedureToFirst<string>(PROC_GET_TOTAL_TRANSACTION_VALUE, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
            });
            return result;
        }

        // CRUD Bảng EP_REPORT_TRADING_PROVIDER
        public PagingResult<TradingProviderDto> FindAllTrading()
        {
            var result = _oracleHelper.ExecuteProcedurePaging<TradingProviderDto>(PROC_TRADING_PROVIDER_FIND, new());
            return result;
        }

        public PagingResult<ReportTradingProviderDto> FindAllReportTrading()
        {
            var result = _oracleHelper.ExecuteProcedurePaging<ReportTradingProviderDto>(PROC_REPORT_TRADING_PROVIDER_FIND, new());
            return result;
        }

        public void RemoveReportTrading(int id)
        {
            _logger.LogInformation($"RemoveReportTrading: id = {id}");
            _oracleHelper.ExecuteProcedureToFirst<int>(PROC_REMOVE_REPORT_TRADING, new
            {
                pv_ID = id
            });
        }

        public void AddReportTrading(ReportTradingProviderDto input)
        {
            _logger.LogInformation($"AddReportTrading: input = {JsonSerializer.Serialize(input)}");
            _oracleHelper.ExecuteProcedureToFirst<ReportTradingProviderDto>(PROC_ADD_REPORT_TRADING, new
            {
                pv_ID = input.Id,
                pv_BUSINESS_CUSTOMER_ID = input.BusinessCustomerId,
                pv_STATUS = input.Status,
                pv_DELETED = input.Deleted,
                pv_CREATED_BY = input.CreatedBy,
                pv_CREATED_DATE = input.CreatedDate,
                pv_MODIFIED_BY = input.ModifiedBy,
                pv_MODIFIED_DATE = input.ModifiedDate,
            });
        }
    }
}

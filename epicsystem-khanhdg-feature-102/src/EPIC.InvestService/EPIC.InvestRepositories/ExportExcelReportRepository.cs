using DocumentFormat.OpenXml.VariantTypes;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Dashboard;
using EPIC.InvestEntities.Dto.ExportExcel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class ExportExcelReportRepository : BaseRepository
    {
        private const string PROC_GET_TOTAL_OPERATION_REPORT = "PKG_INV_EXCEL_REPORT.TOTAL_OPERATION_REPORT";
        private const string PROC_INVESTMENT_INVEST_ACCOUNT = "PKG_INV_EXCEL_REPORT.LIST_INVESTMENT_INVEST_ACCOUNT";
        private const string PROC_SALES_REPORT = "PKG_INV_EXCEL_REPORT.PROC_SALES_REPORT";
        private const string PROC_EXPORT_INVESTMENT_REPORT_HVF = "PKG_INV_EXCEL_REPORT.PROC_INVESTMENT_EXPORT_EXCEL_HVF";
        private const string PROC_ACTUAL_EXPEND = "PKG_INV_EXCEL_REPORT.ACTUAL_EXPEND";
        private const string PROC_EXPORT_INVESTMENT_REPORT = "PKG_INV_EXCEL_REPORT.PROC_INVESTMENT_EXPORT_EXCEL";
        private const string PROC_EXPORT_INVEST_CODE = "PKG_INV_EXCEL_REPORT.PROC_PROJECT_EXPORT_EXCEL";

        public ExportExcelReportRepository(string connectionString, ILogger logger)
        {
            _logger = logger;
            _oracleHelper = new OracleHelper(connectionString, logger);
        }

        /// <summary>
        /// lấy data báo cáo tổng hợp vận hành HVF
        /// </summary>
        /// <param name="dateOperation"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public TotalOperationReportDto GetTotalOperationReport(DateTime? dateOperation, int? tradingProviderId)
        {
            var data = _oracleHelper.ExecuteProcedureToFirst<TotalOperationReportDto>(PROC_GET_TOTAL_OPERATION_REPORT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_DATE = dateOperation
            });

            return data;
        }

        /// <summary>
        /// Lấy thông tin sao kê của tài khoản nhà đầu tư
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<InvestmentAccountStatistical> GetInvestmentAccountStatisticals(int? partnerId, int? tradingProviderId, DateTime? startDate, DateTime? endDate)
        {
            var result = _oracleHelper.ExecuteProcedure<InvestmentAccountStatistical>(PROC_INVESTMENT_INVEST_ACCOUNT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate
            }).ToList();
            return result;
        }

        /// <summary>
        /// lấy danh sách dữ liệu báo cáo tổng hợp các khoản đầu tư bán hộ
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<SaleInvestmentExcel> FindListSaleInvestment(int? tradingProviderId, DateTime? startDate, DateTime? endDate, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedure<SaleInvestmentExcel>(PROC_SALES_REPORT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_PARTNER_ID = partnerId
            }).ToList();
            return result;
        }

        /// <summary>
        /// Lấy list dữ liệu cho báo cáo tổng hợp các khoản đầu tư của VH HVF
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<InvestmentReport> FindExportInvestmentReportHVF(int? tradingProviderId, DateTime? startDate, DateTime? endDate, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedure<InvestmentReport>(PROC_EXPORT_INVESTMENT_REPORT_HVF, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_PARTNER_ID = partnerId
            }).ToList();
            return result;
        }

        /// <summary>
        /// lấy dữ liệu data của báo cáo thực chi
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<ActualExpendReport> ActualExpendReport(DateTime? startDate, DateTime? endDate, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedure<ActualExpendReport>(PROC_ACTUAL_EXPEND, new
            {
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            }).ToList();
            return result;
        }

        /// <summary>
        /// Lấy thông tin để lập báo cáo các khoản hợp đồng đầu tư
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<InvestmentReport> FindExportInvestmentReport(int? tradingProviderId, DateTime? startDate, DateTime? endDate, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedure<InvestmentReport>(PROC_EXPORT_INVESTMENT_REPORT, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
                pv_PARTNER_ID = partnerId
            }).ToList();
            return result;
        }

        /// <summary>
        /// Báo cáo tổng hợp các mã BĐS
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<InvestCodeDto> FindAllInvestCode(int? tradingProviderId, int? partnerId, DateTime? startDate, DateTime? endDate)
        {
            var result = _oracleHelper.ExecuteProcedure<InvestCodeDto>(PROC_EXPORT_INVEST_CODE, new
            {
                pv_PARTNER_ID = partnerId,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_START_DATE = startDate,
                pv_END_DATE = endDate,
            }).ToList();
            return result;
        }
    }
}

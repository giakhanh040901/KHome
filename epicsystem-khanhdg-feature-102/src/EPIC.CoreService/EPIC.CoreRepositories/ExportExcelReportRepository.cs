using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.DataAccess;
using EPIC.Entities.DataEntities;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class ExportExcelReportRepository
    {
        private OracleHelper _oracleHelper;
        private static string GET_CUSTOMER_BY_INFO = "PKG_CORE_EXCEL_REPORT.PROC_CUSTOMER_INFO";
        private static string GET_LIST_CUSTOMER_INFO_CHANGE_ROOT = "PKG_CORE_EXCEL_REPORT.PROC_LIST_CUSTOMER_INFO_CHANGE_ROOT";
        public ExportExcelReportRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
        }
        
        /// <summary>
        /// Báo cáo rà soát thông tin khách hàng
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<CustomerExcelReport> GetCustomerInfo(int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedure<CustomerExcelReport>(
                GET_CUSTOMER_BY_INFO,
                new
                {
                    pv_TRADING_PROVIDER_ID = tradingProviderId
                }
             ).ToList();
        }
        
        /// <summary>
        /// Báo cáo chỉnh sửa thông tin khách hàng root
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ExcelCustomerInfoChangeRoot> GetCustomerInfoChangeRoot(DateTime? startDate, DateTime? endDate)
        {
            return _oracleHelper.ExecuteProcedure<ExcelCustomerInfoChangeRoot>(
               GET_LIST_CUSTOMER_INFO_CHANGE_ROOT,
               new
               {
                   pv_START_DATE = startDate,
                   pv_END_DATE = endDate
               }
            ).ToList();
        }
    }
}

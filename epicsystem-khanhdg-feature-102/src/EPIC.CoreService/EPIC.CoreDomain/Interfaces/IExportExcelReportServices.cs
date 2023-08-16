using EPIC.CoreEntities.Dto.ExcelReport;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IExportExcelReportServices
    {
        /// <summary>
        /// Báo cáo excel khách hàng
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto CustomerListExcelReport(DateTime? startDate, DateTime? endDate);
        
        /// <summary>
        /// Báo cáo excel danh sách thông tin khách hàng root
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto CustomerRootListExcelReport(DateTime? startDate, DateTime? endDate);
        ExportResultDto ListCustomerHVF(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// báo cáo danh sách thông tin saler
        /// </summary>
        /// <returns></returns>
        ExportResultDto SalerListExcelReport(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// báo cáo doanh số thông tin tài khoản người dùng
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto UserExcelReport(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// báo cáo danh sách thay đổi thông tin khách hàng
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto CustomerInfoChangeReport(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Báo cáo danh sách chỉnh sửa thông tin khách hàng root
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto CustomerInfoChangeRootReport(DateTime? startDate, DateTime? endDate);
        ExportResultDto UserNoEkycExcelReport(DateTime? startDate, DateTime? endDate);
    }
}

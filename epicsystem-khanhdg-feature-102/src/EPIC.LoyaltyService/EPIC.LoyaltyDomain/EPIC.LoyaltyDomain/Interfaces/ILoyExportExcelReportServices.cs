using EPIC.Entities.Dto.ContractData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface ILoyExportExcelReportServices
    {
        /// <summary>
        /// Báo cáo danh sách yêu cầu đổi voucher
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto ReportConversionPoint(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Báo cáo xuất nhập tồn voucher
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto ReportVoucher(DateTime? startDate, DateTime? endDate);
    }
}

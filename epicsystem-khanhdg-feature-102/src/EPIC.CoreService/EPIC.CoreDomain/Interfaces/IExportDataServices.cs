using EPIC.CoreEntities.Dto.ExportData;
using EPIC.DataAccess.Models;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IExportDataServices
    {
        PagingResult<ExportDataInvestorDto> ExportDataInvestor(string token, FilterExportDataDto input);
        PagingResult<ExportDataInvestOrderDto> ExportDataInvestOrder(string token, FilterExportDataDto input);
        PagingResult<ExportDataSaleDto> ExportDataSale(string token, FilterExportDataDto input);
    }
}

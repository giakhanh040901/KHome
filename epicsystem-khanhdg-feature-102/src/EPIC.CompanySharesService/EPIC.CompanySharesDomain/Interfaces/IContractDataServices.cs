using EPIC.Entities.Dto.ContractData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface IContractDataServices
    {
        Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyDetailId, int BankAccId, int identificationId, int contractTemplateId, int? investorId);
        Task<ExportResultDto> ExportContractPdfTest(int tradingProviderId, int contractTemplateId);
        //Task<ExportResultDto> ExportContractReceive(int orderId, int distributionId, int tradingProviderId, int source);
        //Task<ExportResultDto> ExportContractReceivePdfTest(int tradingProviderId, int contractTemplateId);
        ExportResultDto ExportContractWordTest(int tradingProviderId, int contractTemplateId);
        ExportResultDto ExportFileContract(int id, string contractType);
        Task<SaveFileDto> SaveContract(int orderId, int contractTemplateId, string isSign, int? tradingProviderId = null);
        SaveFileDto SignContractPdf(int orderContractFileId, int contractTemplateId, int tradingProviderId);
    }
}

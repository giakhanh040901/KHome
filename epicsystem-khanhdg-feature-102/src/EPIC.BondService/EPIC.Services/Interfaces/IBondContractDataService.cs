using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.UploadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondContractDataService
    {
        public ExportResultDto ExportContract(int orderId, int contractTemplateId);
        /// <summary>
        /// Lưu hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public SaveFileDto SaveContract(int orderId, int contractTemplateId);
        public ExportResultDto ExportContractTemp(int orderId, int contractTemplateId, int orderContractFileId);
        public ExportResultDto ExportContractSignature(int orderId, int contractTemplateId, int orderContractFileId);
        public ExportResultDto ExportFileScanContract(int orderId, int contractTemplateId,int orderContractFileId);
        //ExportResultDto ExportContractApp(decimal totalValue, int policyDetailId, int BankAccId, int identificationId, int contractTemplateId);
        ExportResultDto ExportContractSignatureApp(int secondaryContractFileId);
        Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyDetailId, int BankAccId, int identificationId, int contractTemplateId, int? investorId);
        Task<SaveFileDto> SaveContractPdfNotSign(int orderId, int contractTemplateId, int policyDetailId);
        SaveFileDto SaveContractSignPdf(int secondaryContractFileId, int contractTemplateId, int tradingProviderId);
        Task<ExportResultDto> ExportContractReceive(int orderId, int bondSecondaryId, int tradingProviderId, int source);
        Task<ExportResultDto> ExportContractPdfTest(int tradingProviderId, int contractTemplateId);
        ExportResultDto ExportContractWordTest(int tradingProviderId, int contractTemplateId);
        Task<ExportResultDto> ExportContractReceivePdfTest(int tradingProviderId, int contractTemplateId);
    }
}

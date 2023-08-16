using EPIC.CoreEntities.Dto.CollabContract;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ISaleExportCollapContractServices
    {
        CollabContract CreateCollabContractFileScan(CreateSaleCollabContractDto input);
        Task<ExportResultDto> ExportContractApp(int collapContractTempId, int saleTempId);
        Task<ExportResultDto> ExportContractPdfTest(int tradingProviderId, int contractTemplateId);
        ExportResultDto ExportContractScan(int collabContractId);
        ExportResultDto ExportContractSignature(int collabContractId);
        ExportResultDto AppExportContractSignature(int collabContractId,int tradingProviderId);
        ExportResultDto ExportContractTemp(int collabContractId);
        ExportResultDto ExportContractWordTest(int tradingProviderId, int contractTemplateId);
        List<AppListCollabContractSignDto> FindAllFileSignatureForApp(int tradingProviderId);
        Task<SaveFileDto> SaveContractPdfNotSign(int collapContractTempId, int saleId, int tradingProviderId, int? investorId = null, bool? isSaleOnline = false);
        Task<SaveFileDto> SaveContractPdfSignApp(int collapContractTempId, int saleId, int tradingProviderId, int? investorId = null);
        int UpdateCollabContractFileScan(UpdateSaleCollabContractDto input);
        void UpdateContractFile(int saleId);
        Task<List<CollabContractDto>> UpdateContractFileApp(int saleId, int tradingProviderId);
        void UpdateContractFileSignPdf(int saleId);
    }
}

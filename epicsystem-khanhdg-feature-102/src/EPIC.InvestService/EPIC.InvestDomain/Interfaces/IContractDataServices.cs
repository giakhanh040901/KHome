using EPIC.Entities.Dto.ContractData;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IContractDataServices
    {
        Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyDetailId, int BankAccId, int identificationId, int contractTemplateId, List<ReplaceTextDto> replaceTexts, int? investorId = null);
        Task<ExportResultDto> ExportContractPdfTest(int contractTemplateId, string type);
        Task<ExportResultDto> ExportContractReceive(int orderId, int distributionId, int tradingProviderId, int source);
        Task<ExportResultDto> ExportContractReceivePdfTest(int tradingProviderId, int contractTemplateId);
        ExportResultDto ExportContractSignature(int id);
        ExportResultDto ExportContractSignatureApp(int id);
        ExportResultDto ExportContractTemp(int id);
        ExportResultDto ExportContractTempPdf(int id);
        ExportResultDto ExportContractWordTest(int contractTemplateId, string type);
        ExportResultDto ExportContractTempWordTest(int contractTemplateTempId, string type);
        Task<ExportResultDto> ExportContractTempPdfTest(int contractTemplateTempId, string type);
        ExportResultDto ExportFileScanContract(int id);
        List<ReplaceTextDto> GetDataContractFile(InvOrder order, int tradingProviderId, bool isSignature);
        List<ReplaceTextDto> GetDataContractFile(int orderId, int tradingProviderId, bool isSignature);
        List<ReplaceTextDto> GetDataContractFileApp(decimal totalValue, int policyDetailId, int bankAccId, int identificationId, int? investorId, string contractCode);
        List<ReplaceTextDto> GetDataRenewalsContractFile(PolicyDetail policyDetail);
        List<ReplaceTextDto> GetDataWithdrawalContractFile(InvOrder order, Policy policy, PolicyDetail policyDetail, decimal tongTienCondauTu, decimal soTienRut, DateTime ngayRut, bool isKhachCaNhan, DateTime? distributionCloseSellDate);

        //List<ReplaceTextDto> GetDataContractFile(InvOrder order, int tradingProviderId, bool isSignature);
        //SaveFileDto SaveContract(InvOrder order, int contractTemplateId);
        Task<SaveFileDto> SaveContractApp(InvOrder order, int contractTemplateId, int policyDetailId, List<ReplaceTextDto> replateTexts);
        Task<SaveFileDto> SaveContractApp(long orderId, int contractTemplateId, int policyDetailId, List<ReplaceTextDto> replaceTexts);
        SaveFileDto SignContractPdf(int orderContractFileId, int contractTemplateId, int tradingProviderId);

    }
}

using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerContractDataServices
    {
        /// <summary>
        /// Xem tạm hợp đồng app
        /// </summary>
        /// <param name="totalValue"></param>
        /// <param name="policyId">Id chính sách</param>
        /// <param name="BankAccId">Id tài khoản thụ hưởng</param>
        /// <param name="identificationId">Id thông tin giấy tờ</param>
        /// <param name="contractTemplateId">Id mẫu hợp đồng</param>
        /// <returns></returns>
        Task<ExportResultDto> ExportContractApp(decimal totalValue, int policyId, int BankAccId, int identificationId, int contractTemplateId);
        Task<ExportResultDto> ExportContractReceive(long orderId, int? tradingProviderId = null);
        Task<ExportResultDto> ExportContractReceivePdfTest(int tradingProviderId, int contractTemplateId);

        /// <summary>
        /// Xem trước hợp đồng rút tiền
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        Task<ExportResultDto> ExportContractWithDrawal(long orderId, int contractTemplateId, decimal amountMoney, int investorBankAccId);

        /// <summary>
        /// Tải file hợp đồng
        /// </summary>
        /// <param name="id">Id hợp đồng</param>
        /// <param name="contractType">Loại hợp đồng</param>
        /// <returns></returns>
        ExportResultDto ExportFileContract(int id, string contractType);
        //string GetContractCode(GarnerOrder order, GarnerProduct product, GarnerPolicy policy, int configContractId);
        List<ReplaceTextDto> GetDataContractFile(long orderId, int tradingProviderId, bool isSignature, int ? configContractId = null);
        List<ReplaceTextDto> GetDataContractFile(GarnerOrder order, int tradingProviderId, bool isSignature, int? configContractId = null);
        List<ReplaceTextDto> GetDataContractFileApp(decimal totalValue, GarnerPolicy policy, GarnerDistribution distribution, int bankAccId, int identificationId, int? investorId, string contractCode, int? contactAddressId);
        List<ReplaceTextDto> GetDataWithdrawalContractFile(long orderId, long? withdrawalId = null, decimal? amountMoney = null);
        List<ReplaceTextDto> GetDataWithdrawalContractFile(GarnerOrder order, long? withdrawalId = null, decimal? amountMoney = null);
        List<ReplaceTextDto> GetDataWithdrawalContractFileApp(int policyId, string cifCode, decimal amountMoney);

        /// <summary>
        /// Fill data hợp đồng
        /// </summary>
        /// <param name="contractTemplateType">Dành cho cá nhân hay chuyên nghiệp</param>
        /// <param name="contractTemplateId">Id mẫu hợp đồng</param>
        /// <param name="replaceTexts">data</param>
        /// <param name="tradingProviderId">Id đlsc</param>
        /// <returns></returns>
        Task<SaveFileDto> SaveContract(string contractTemplateType, int contractTemplateId, List<ReplaceTextDto> replaceTexts, int? tradingProviderId = null);

        /// <summary>
        /// Ký điện tử
        /// </summary>
        /// <param name="orderContractFileId">Id file hợp đồng</param>
        /// <param name="contractTemplateId">Id mẫu hợp đồng</param>
        /// <param name="tradingProviderId">Id đlsc</param>
        /// <returns></returns>
        SaveFileDto SignContractPdf(long orderContractFileId, int contractTemplateId, int tradingProviderId);
    }
}

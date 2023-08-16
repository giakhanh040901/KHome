using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerOrderContractFile;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerOrderServices
    {
        Task<GarnerOrderMoreInfoDto> Add(CreateGarnerOrderDto input);
        #region App Services
        /// <summary>
        /// Kiểm tra trước khi thêm hợp đồng
        /// </summary>
        void CheckOrder(AppCheckGarnerOrderDto input);

        /// <summary>
        /// Thêm hợp đồng cho Investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AppGarnerOrderDto> InvestorOrderAdd(AppCreateGarnerOrderDto input);

        /// <summary>
        /// Sale thêm hợp đồng BondOrder cho nhà đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AppGarnerOrderDto> SaleInvestorOrderAdd(AppSaleCreateGarnerOrderDto input);


        /// <summary>
        /// Kiểm tra mã giới thiệu của sale theo đại lý cúa chính sách
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyId);

        /// <summary>
        /// Lấy danh sách hợp đồng nhóm theo chính sách
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        List<AppGarnerOrderByPolicyDto> AppInvestorGetListOrder(int groupOrder);

        /// <summary>
        /// App xem chi tiết thông tin lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<AppGarnerOrderDetailDto> AppOrderDetail(long orderId);

        /// <summary>
        /// App xem màn lịch sử đầu tư
        /// </summary>
        /// <param name="groupOrder"></param>
        /// <returns></returns>
        List<AppGarnerOrderByPolicyDto> AppInvestorGetListOrderHistory(int groupOrder);

        /// <summary>
        /// App hủy duyệt trọng trạng thái chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        void AppOrderCancel(long orderId);
        #endregion

        PagingResult<GarnerHistoryUpdateDto> FindAllHistoryTable(FilterGarnerHistoryDto input);
        GarnerOrder Update(UpdateGarnerOrderDto input);
        Task<GarnerOrder> OrderApprove(long orderId);
        GarnerOrder OrderCancel(int id);
        Task<GarnerOrder> UpdateSource(long id);
        GarnerOrder ChangeDeliveryStatusDelivered(long orderId);
        GarnerOrder ChangeDeliveryStatusReceived(long orderId);
        GarnerOrder ChangeDeliveryStatusDone(long orderId);
        void Deleted(List<long> orderIds);
        GarnerOrder ProcessContract(long orderId);
        GarnerOrder ChangeDeliveryStatus(long orderId);
        GarnerOrderMoreInfoDto FindById(long orderId);
        /// <summary>
        /// Lấy danh sách chính sách của khách hàng theo cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        /// <returns></returns>
        List<GarnerPolicy> GetListPolicyByCifCode(string cifCode);

        /// <summary>
        /// Lấy danh sách ngân hàng đặt lệnh theo cifCode
        /// </summary>
        /// <param name="cifCode"></param>
        List<BankAccountInfoDto> FindListBankOfCifCode(string cifCode);
        /// <summary>
        /// Xem danh sách order
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<GarnerOrderMoreInfoDto> FindAll(FilterGarnerOrderDto input, int[] status, bool isGroupByCustomer = false, bool? isDelivaryStatus = null);

        PagingResult<GarnerOrderMoreInfoDto> FindAllByPolicy(FilterGarnerOrderDto input, int[] status);

        #region Cập nhật lại thông tin
        /// <summary>
        /// Update kỳ hạn cho sổ lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        GarnerOrder UpdatePolicyDetail(long orderId, int? policyDetailId);
        /// <summary>
        /// Update chính sách cho sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        GarnerOrder UpdatePolicy(long orderId, int policyId);
        /// <summary>
        /// Update số tiền đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        GarnerOrder UpdateTotalValue(long orderId, decimal? totalValue);
        /// <summary>
        /// Update mã giới thiệu
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        GarnerOrder UpdateReferralCode(long orderId, string referralCode);
        /// <summary>
        /// Update tài khoản ngân hangd
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="investorBankAccId"></param>
        /// <returns></returns>
        GarnerOrder UpdateInvestorBankAccount(long orderId, int? investorBankAccId);
        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerOrder UpdateInfoCustomer(UpdateGarnerInfoCustomerDto input);
        GarnerOrderCashFlowDto ProfitFuture(long orderId);
        /// <summary>
        /// Gửi lại email thông báo
        /// </summary>
        /// <param name="orderId"></param>
        Task ResendNotifyOrderApprove(long orderId);
        #endregion

        #region Giao nhận hợp đồng
        PhoneReceiveDto GetPhoneByDeliveryCodeHidenChar(string deliveryCode);
        void VerifyPhone(string deliveryCode, string phone, int tradingProviderId);
        void ChangeDeliveryStatusRecevired(string deliveryCode, string otp);
        ExportResultDto ImportFileTemplate();
        Task ImportExcelOrder(ImportExcelOrderDto dto);
        Task<GarnerOrderMoreInfoDto> AddOrderCommon(CreateGarnerOrderDto input, bool isImport = false);
        #endregion
    }
}

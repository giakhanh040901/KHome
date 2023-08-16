using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
using EPIC.Entities.Dto.Sale;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.OrderContractFile;
using EPIC.InvestEntities.Dto.OrderPayment;
using EPIC.InvestSharedEntites.Dto.Order;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IOrderServices
    {
        PagingResult<ViewOrderDto> FindAll(InvestOrderFilterDto input, int? groupStatus);
        PagingResult<ViewOrderDto> FindAllDeliveryStatus(InvestOrderFilterDto input, int? groupStatus);
        ViewOrderDto FindById(int id);
        ViewOrderDto Add(CreateOrderDto input);
        int Update(UpdateOrderDto input, int orderId);
        void Delete(List<int> ids);
        OrderPaymentDto OrderPaymentAdd(CreateOrderPaymentDto input);
        int UpdateOrderPayment(UpdateOrderPaymentDto input, int orderPaymentId);
        int DeleteOrderPayment(int id);
        int ApproveOrderPayment(int orderPaymentId, int status);
        OrderPaymentDto FindPaymentById(int id);
        PagingResult<OrderPayment> FindAllOrderPayment(int orderId, int pageSize, int pageNumber, string keyword, int? status);
        InvestOrderCashFlowDto GetProfitInfo(int orderId);
        int UpdateTotalValue(int id, decimal? totalValue);
        int UpdatePolicyDetail(int id, int? policyDetailId);
        int UpdateReferralCode(int id, string referralCode);
        Task<int> UpdateSource(int id);
        Task<int> OrderApprove(int id);
        int OrderCancel(int id);
        int UpdateInvestorBankAccount(int id, int? investorBankAccId);
        int UpdateInfoCustomer(int id, int? investorBankAccId, int? contractAddressId, int? investorIdenId);
        int ChangeDeliveryStatusDelivered(int orderId);
        int ChangeDeliveryStatusReceived(int orderId);
        int ChangeDeliveryStatusDone(int orderId);
        Task<int> ChangeDeliveryStatusReceviredApp(string deliveryCode);
        Task<decimal> ChangeDeliveryStatusRecevired(string deliveryCode, string otp);
        OrderContractFile AddOrderContractFile(CreateOrderContractFileDto input);
        void UpdateOrderContractFile(UpdateOrderContractFileDto input);
        void CancelRenewalRequestByOrderId(long orderId);
        AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyDetailId);
        InvestInterestCalculationDateDto CheckInvestmentDay(int policyDetailId, DateTime ngaybatdau);
        /// <summary>
        /// Lịch sử đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        PagingResult<InvestOrderInvestmentHistoryDto> FindAllInvestHistory(FilterInvestOrderDto input, int[] status);

        /// <summary>
        /// Thay đổi phương thức tất toán cuối kỳ,
        /// Đại lý thay đổi trên CMS
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int UpdateSettlementMethod(int orderId, SettlementMethodDto input);

        #region app
        /// <summary>
        /// Kiểm tra đặt lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void CheckOrder(AppCheckOrderDto input);

        /// <summary>
        /// Nhà đầu tư đặt lệnh qua OTP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AppOrderDto> OrderInvestorAdd(AppCreateOrderDto input);
        /// <summary>
        /// Sale đặt lệnh hộ investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AppOrderDto> SaleOrderInvestorAdd(AppSaleInvestorCreateOrderDto input);
        /// <summary>
        /// Thông tin màn sổ lệnh
        /// </summary>
        /// <returns></returns>
        List<AppInvestOrderInvestorDto> AppOrderGetAll(int groupStatus);

        /// <summary>
        /// Xem chi tiết sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<AppInvestOrderInvestorDetailDto> AppOrderInvestorDetail(int orderId);

        PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode);
        void VerifyPhone(string deliveryCode, string phone, int tradingProviderId);

        /// <summary>
        /// Yêu cầu nhận hợp đồng khi lệnh ở trạng thái đang đầu tư, deliveryStatus sẽ chuyển thành 1
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int AppRequestDeliveryStatus(int orderId);

        /// <summary>
        /// Saler thỏa mãn thông tin trước khi đặt lệnh thì sẽ trả ra thông tin
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        ViewCheckSaleBeforeAddOrderDto CheckSaleBeforeAddOrder(string referralCode, int distributionId);

        /// <summary>
        /// Cập nhật phương thức tất toán cuối kỳ
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int AppUpdateSettlementMethod(int orderId, SettlementMethodDto input);

        /// <summary>
        /// Hủy lệnh cho App đang trong trạng thái khởi tạo hoặc chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int AppCancelOrder(int orderId);

        /// <summary>
        /// App Xem thay đổi hợp đồng sau khi rút vốn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="soTienRut"></param>
        /// <returns></returns>
        RutVonDto AppViewThayDoiKhiRutVon(long orderId, decimal soTienRut);
        #endregion

        #region Lập chi, chi trả, rút vốn
        /// <summary>
        /// Lập danh sách chi trả khi đến hạn chi trả CMS
        /// </summary>
        /// <returns></returns>
        Task<PagingResult<DanhSachChiTraDto>> LapDanhSachChiTra(InterestPaymentFilterDto input, bool isLastPeriod);

        /// <summary>
        /// Kiểm tra thời gian đến hạn chi trả, đến hạn đáo hạn để gửi thông báo
        /// </summary>
        /// <returns></returns>
        Task<List<ThoiGianChiTraThucDto>> NoticePaymentDueDate();

        /// <summary>
        /// Kiểm tra thời gian đến hạn chi trả, đến hạn đáo hạn để gửi thông báo
        /// </summary>
        /// <returns></returns>
        Task NotifyPaymentDueDate();

        /// <summary>
        /// Danh sách thông tin rút vốn của hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        List<RutVonDto> TheoDoiRutTruocHan(long orderId);

        /// <summary>
        /// Xem thông tin rút vốn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="soTienRut"></param>
        /// <param name="ngayRut"></param>
        /// <returns></returns>
        RutVonDto RutVon(long orderId, decimal soTienRut, DateTime ngayRut);
        #endregion
        
        ThoiGianChiTraThucDto LayNgayChiTra( int? orderId);
        /// <summary>
        /// Lấy lịch sử chính sửa
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        PagingResult<HistoryUpdateDto> GetOrderHistoryUpdate(int pageNumber, int? pageSize, string keyword, int orderId);
        
        /// <summary>
        /// Update trường isSign theo orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int UpdateIsSignByOrderId(int orderId);

        /// <summary>
        /// Xử lý yêu cầu nhận hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int ProcessContract(int orderId);

        PagingResult<OrderRenewalsRequestDto> GetAllRenewalsRequest(FilterRenewalsRequestDto input);

        /// <summary>
        ///  Cập nhật lại dòng tiền
        /// </summary>
        void UpdateOrderCashFlow(int orderId);
        Task<List<ThoiGianChiTraThucDto>> LayDanhSachNgayChiTra(FilterCalulationInterestPayment input, bool isAllSystem = false);
    }
}

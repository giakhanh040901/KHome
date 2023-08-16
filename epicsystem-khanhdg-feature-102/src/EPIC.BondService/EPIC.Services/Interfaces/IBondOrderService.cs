using EPIC.BondEntities.Dto.AppOrder;
using EPIC.BondEntities.Dto.BondOrder;
using EPIC.BondEntities.Dto.SaleInvestor;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Bank;
using EPIC.EntitiesBase.Dto;
using EPIC.Entities.Dto.Bond;
using EPIC.Entities.Dto.BondSecondaryContract;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.OrderContractFile;
using EPIC.Entities.Dto.OrderPayment;
using EPIC.Entities.Dto.ProductBondSecondPrice;
using EPIC.Entities.Dto.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.BondEntities.DataEntities;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondOrderService
    {
        PagingResult<ViewOrderDto> FindAll(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? policyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer);
        PagingResult<ViewOrderDto> FindAllDeliveryStatus(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? policyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, DateTime? pendingDate, DateTime? deliveryDate, DateTime? receivedDate, DateTime? finishedDate, DateTime? date);
        ViewOrderDto FindById(int id);
        Task<BondOrder> Add(CreateOrderDto input);
        int Update(UpdateOrderDto input, int orderId);
        int UpdateSecondaryContractFileScan(UpdateBondSecondaryContractDto input);
        int Delete(int id);
        int UpdateTotalValue(int orderId, decimal? totalValue);
        int UpdatePolicyDetail(int orderId, int? policyDetailId);
        int UpdateReferralCode(int orderId, string referralCode);
        int UpdateSource(int orderId);

        /// <summary>
        /// Cập nhật phương thức tất toán cuối kỳ cho CMS
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int UpdateSettlementMethod(int orderId, SettlementMethodDto input);
        int OrderApprove(int orderId);
        int OrderCancel(int orderId);
        BondOrderPayment AddPayment(CreateOrderPaymentDto input);
        int Update(UpdateOrderPaymentDto input, int orderPaymentId);
        int DeleteOrderPayment(int id);
        int ApprovePayment(int orderPaymentId, int status);
        PagingResult<BondOrderPayment> FindAll(int orderId, int pageSize, int pageNumber, string keyword, int? status);
        OrderPaymentDto FindPaymentById(int id);
        /// <summary>
        /// Tính lợi tức
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        ProfitAndInterestDto GetProfitInfo(int orderId);
        ProductBondSecondPriceDto FindPriceByDate(int bondSecondaryId, DateTime priceDate);
        int UpdateInvestorBankAccount(int orderId, int? bankAccId);

        Task UpdateContractFile(int orderId);
        int ChangeDeliveryStatusDelivered(int orderId);
        int ChangeDeliveryStatusReceived(int orderId);
        int ChangeDeliveryStatusDone(int orderId);
        int ChangeDeliveryStatusRecevired(string deliveryCode);

        /// <summary>
        /// Tìm kiếm sale theo mã giới thiệu
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyDetailId);
        InterestCalculationDateDto CheckInvestmentDay(int policyDetailId, DateTime ngaybatdau);

        List<ThoiGianChiTraThucDto> LayDanhSachNgayChiTra(DanhSachChiTraFitlerDto input);
        PagingResult<DanhSachChiTraDto> LapDanhSachChiTra(DanhSachChiTraFitlerDto input);

        /// <summary>
        /// Update trường isSign theo orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int UpdateIsSignByOrderId(int orderId);
        #region app
        /// <summary>
        /// Nhà đầu tư tự đặt lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void CheckOrder(CheckOrderAppDto input);
        OrderAppDto InvestorAdd(CreateOrderAppDto input);

        /// <summary>
        /// sale đặt hộ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        OrderAppDto SaleAddInvestorOrder(SaleInvestorAddOrderDto input);

        /// <summary>
        /// Lấy thông tin danh sách tài khoản thụ hưởng của khách hàng
        /// </summary>
        /// <returns></returns>
        List<InvestorBankAccountDto> FindAllListBank();

        /// <summary>
        /// Lấy thông tin ngân hàng hỗ trợ và tìm kiếm theo tên viết tắt
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<BankSupportDto> GetListBankSupport(string keyword);

        /// <summary>
        /// Thông tin màn sổ lệnh
        /// </summary>
        /// <returns></returns>
        List<AppOrderInvestorDto> AppOrderGetAll(int groupStatus);

        /// <summary>
        /// Xem chi tiết sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        AppOrderInvestorDetailDto AppOrderInvestorDetail(int orderId);
        BondSecondaryContract CreateSecondaryContractFileScan(CreateBondSecondaryContractDto input);
        void UpdateContractFileSignPdf(int orderId);
        PhoneReceiveDto GetPhoneByDeliveryCode(string deliveryCode);
        decimal ChangeDeliveryStatusRecevired(string deliveryCode, string otp);
        void VerifyPhone(string deliveryCode, string phone, int tradingProviderId);

        /// <summary>
        /// Saler thỏa mãn thông tin trước khi đặt lệnh thì sẽ trả ra thông tin
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        ViewCheckSaleBeforeAddOrderDto CheckSaleBeforeAddOrder(string referralCode, int secondaryId);

        /// <summary>
        /// Cập nhật phương thức tất toán cuối kỳ cho App
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
        #endregion
    }
}

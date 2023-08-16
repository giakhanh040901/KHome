using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.RealEstateEntities.Dto.RstOrderCoOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstOrderServices
    {
        /// <summary>
        /// Thêm hợp đồng sổ lệnh cho CMS
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RstOrderMoreInfoDto> Add(CreateRstOrderDto input);

        /// <summary>
        /// Xóa hợp đồng ở trạng thái khởi tạo 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        RstOrder Deleted(int orderId);

        /// <summary>
        /// Tìm kiếm order phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <param name="status"></param>
        /// <param name="isGroupByCustomer"></param>
        /// <returns></returns>
        PagingResult<RstOrderMoreInfoDto> FindAll(FilterRstOrderDto input, int[] status, bool isGroupByCustomer = false);

        /// <summary>
        /// Tìm kiếm theo id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        RstOrderMoreInfoDto FindById(int orderId);

        /// <summary>
        /// Cập nhật hợp đồng sổ lệnh CMS
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RstOrder Update(UpdateRstOrderDto input);

        /// <summary>
        /// Cập nhật đồng sở hữu
        /// </summary>
        /// <param name="input"></param>
        void UpdateOrderCoOwner(UpdateListRstOrderCoOwnerDto input);

        /// <summary>
        /// Cập nhật hình thức thanh toán
        /// </summary>
        /// <param name="input"></param>
        void UpdatePaymentType(UpdateRstOrderPaymentTypeDto input);

        /// <summary>
        /// Thay đổi nguồn đặt lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        RstOrder ChangeSource(int orderId);
        /// <summary>
        /// Duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<RstOrder> OrderApprove(int orderId);
        /// <summary>
        /// Huỷ duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        RstOrder OrderCancel(int orderId);
        /// <summary>
        /// Gia hạn thêm thời gian
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="keepTime"></param>
        void ExtendedKeepTime(RstOrderExtendedKeepTimeDto input);
        
        #region App
        /// <summary>
        ///  Kiểm tra sản phẩm của mở bán trước khi đặt lệnh APP
        /// </summary>
        void CheckOpenSellDetailBeforeOrder(int openSellDetailId);

        /// <summary>
        /// Kiểm tra hợp đồng trước khi thanh toán
        /// </summary>
        void CheckOrder(AppCreateRstOrderCheckDto input);

        /// <summary>
        /// Thêm hợp đồng trên App
        /// </summary>
        Task<AppRstOrderDataSuccessDto> InvestorOrderAdd(AppCreateRstOrderDto input);

        /// <summary>
        /// Sale thêm hợp đồng BondOrder cho nhà đầu tư
        /// </summary>
        Task<AppRstOrderDataSuccessDto> SaleInvestorOrderAdd(AppSaleCreateRstOrderDto input);

        /// <summary>
        /// Quản lý danh sách hợp đồng
        /// </summary>
        Task<List<AppRstOrderDto>> AppGetAllOrder(AppRstOrderFilterDto input, int groupStatus);

        /// <summary>
        /// Xem chi tiết hợp đồng cho App
        /// </summary>
        Task<AppRstOrderDetailDto> AppOrderDetail(int orderId);

        /// <summary>
        /// Kiểm tra mã giới thiệu thuộc đại lý đang bán căn hộ
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="openSellDetailId"></param>
        /// <returns></returns>
        AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int openSellDetailId);

        /// <summary>
        /// Xóa hợp đồng từ trên App
        /// </summary>
        /// <param name="orderId"></param>
        void AppDeleteOrder(int orderId);
        /// <summary>
        /// Xoá đồng sở hữu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        void DeleteCoOwner(int id, int orderId);
        /// <summary>
        /// Thêm đồng sở hữu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RstOrderCoOwner AddCoOwner(CreateRstOrderCoOwnerDto input);

        /// <summary>
        /// Gia hạn hợp đồng trên App (gia hạn thời gian giữ chỗ)
        /// Khi hợp đồng hết thời gian giữ chỗ mà Căn hộ vẫn đang mở bán Status = 1
        /// </summary>
        void AppExtendedKeepTime(int orderId);

        #endregion
    }
}

using EPIC.CoreEntities.Dto.Sale;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreHistoryUpdate;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.SaleAppStatistical;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ISaleServices
    {
        SaleTempDto AddSaleTemp(AddSaleDto input);
        int Delete(int id);
        PagingResult<ViewSaleDto> FindAllSale(FilterSaleSto input);
        int Active(int id);
        SaleInvestorDto FindAllListManagerTrading(string referralCode);
        SaleTempDto FindSaleTemp(int id);
        GetDataSaleDto SaleFindById(int id);
        PagingResult<SaleTempDto> FindAllSaleTemp(FilterSaleTempDto input);
        int UpdateSaleTemp(UpdateSaleTempDto input);
        int UpdateSaleTempCms(UpdateSaleTempCmsDto input);
        int Update(UpdateSaleDto input);
        int DeleteSaleTemp(int id);
        void AddRequestSale(RequestSaleDto input);
        void ApproveSale(ApproveSaleDto input);
        void CancelSale(CancelSaleDto input);
        SaleInvestorDto FindAllListManager(string referralCode);

        Task<int> AppSaleRegister(AppSaleRegisterDto input);
        List<SaleRegisterWithTradingDto> AppListSaleRegister(int? tradingProviderId);
        Sale FindSaleByInvestorId(int investorId);
        PagingResult<CoreHistorySaleUpdateDto> FindAllHistorySale(int saleId, int pageSize, int pageNumber, string keyword);


        /// <summary>
        /// Điều hướng sale đến các đại lý,và vai trò là gì
        /// Quản lý sale điều hướng
        /// </summary>
        /// <param name="input"></param>
        Task ManagerDirectionSale(AppDirectionSaleDto input);

        // <summary>
        /// Điều hướng sale đến các đại lý,và vai trò là gì
        /// Root điều hướng sale
        /// </summary>
        /// <param name="input"></param>
        Task RootDirectionSale(AppDirectionSaleDto input);
        /// <summary>
        /// Danh sách Đại lý sơ cấp theo ManagerSale
        /// </summary>
        /// <returns></returns>
        List<AppListTradingProviderDto> AppListTradingProvider();

        /// <summary>
        /// Thông tin chi tiết của Sale
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        AppSaleInfoDto AppSaleInfo(int tradingProviderId);
        /// <summary>
        /// Kiểm tra xem có là Sale hay không
        /// </summary>
        /// <returns></returns>
        AppCheckSaler AppCheckSaler();

        /// <summary>
        /// Kiểm tra trạng thái Sale theo ĐLSC
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        AppSaleStatusByTrading AppSaleCheckStatusByTrading(int tradingProviderId);
        /// <summary>
        /// Lấy danh sách Đại lý sơ cấp theo Sale
        /// Sale đang đăng nhập
        /// </summary>
        /// <returns></returns>
        List<AppListTradingProviderDto> AppListTradingProviderBySale();

        /// <summary>
        /// Lấy danh sách đại lý sơ cấp mà sale quản lý đang thuộc
        /// dùng cho Root điều hướng saler
        /// không chặn typeSale
        /// </summary>
        /// <param name="managerSaleId"></param>
        /// <returns></returns>
        List<AppListTradingProviderDto> AppListTradingProviderByManagerSale(int managerSaleId);

        /// <summary>
        /// Danh sách đại lý sơ cấp của Sale bao gồm cả đại lý đang chờ ký duyệt và trạng thái của sale
        /// </summary>
        /// <returns></returns>
        List<AppListTradingProviderDto> AppListTradingProviderBySaleAndStatus();

        /// <summary>
        /// Danh sách các Sale điều hướng được xem bởi EPIC
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<ViewSaleRegisterDto> FindAllSaleRegister(FilterSaleRegisterDto input);

        /// <summary>
        /// Tìm kiếm Sale theo mã giới thiệu cho App
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        AppSaleByReferralCodeDto AppFindSaleByReferralCode(string referralCode, int tradingProviderId);

        /// <summary>
        /// Tìm kiếm sale theo mã giới thiệu
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        ViewSaleDto FindSaleByReferralCode(string referralCode, string phone);

        /// <summary>
        /// Lấy danh sách hợp đồng
        /// </summary>
        /// <returns></returns>
        List<AppListCollabContractDto> ListCollabContract();
        /// <summary>
        /// Ký vào hợp đồng hoặc huỷ ký
        /// </summary>
        /// <param name="saleTempId"></param>
        /// <param name="isAsign"></param>
        AppSaleTempSignDto AppSaleTempSign(int saleTempId, bool isAsign);

        /// <summary>
        /// Lấy danh sách các sale con
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        List<AppSaleManagerSaleDto> AppManagerSaleChild(int tradingProviderId);

        /// <summary>
        /// Xem thông tin sale con
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        AppSaleChildDto AppFindSaleChild(int saleId, int tradingProviderId);

        /// <summary>
        /// Tổng quan số liệu của Saler tại đại lý đấy
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="typeTime">Loại thời gian lọc biểu đồng W , M, Y : tuần tháng năm</param>
        /// <param name="filterNumberTime"> Số tuần số tháng số năm</param>
        /// <returns></returns>
        AppSalerOverviewDto AppSalerOverview(int tradingProviderId, string typeTime, int? filterNumberTime = null);

        /// <summary>
        /// Lịch sử đăng ký sale
        /// </summary>
        /// <returns></returns>
        List<AppHistoryRegisterDto> AppHistoryRegister();
        AppStatisticPersonnelDto StatisticPersonnelBySale(int? tradingProviderId, int? saleType, DateTime? startDate, DateTime? endDate, string keyword);

        /// <summary>
        /// Xem chi tiết thông tin nhân viên Sale trong đại lý
        /// </summary>
        AppPersonnelSaleInfoDto FindPersonnelSaleById(int saleId, int tradingProviderId);

        AppSaleProceedDto AppThongKeDoanhSo(AppSaleProceedFilterDto  input);

        /// <summary>
        /// Thống kê dữ liệu hợp đồng cho nút hợp đồng sale app, productType = null: lấy tất cả, productType = 1 : lấy thông tin bond, productType = 2 : lấy thông tin invest , 
        /// </summary>
        AppStatisticContractBySaleDto ThongKeHopDongSaleApp(AppContractOrderFilterDto input);

        AppSaleViewOrderDto SaleViewOrder(int orderId, int projectType);

        /// <summary>
        /// Thay đổi tự động điều hướng
        /// </summary>
        void ChangeAutoDirection();

    }
}

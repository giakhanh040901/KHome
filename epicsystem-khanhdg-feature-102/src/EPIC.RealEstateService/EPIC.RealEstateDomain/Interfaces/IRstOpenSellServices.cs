using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.RealEstateEntities.Dto.RstOpenSellDetail;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstOpenSellServices
    {
        #region Mở bán
        /// <summary>
        /// Thêm mở bán
        /// </summary>
        RstOpenSell Add(CreateRstOpenSellDto input);

        /// <summary>
        /// Cập nhật mở bán
        /// </summary>
        RstOpenSell Update(UpdateRstOpenSellDto input);

        /// <summary>
        /// Xóa mở bán
        /// </summary>
        void DeleteOpenSell(int id);

        /// <summary>
        /// Tạm dừng mở bán (Tạm dừng - Đang bán)
        /// </summary>
        void PauseOpenSell(int id);

        /// <summary>
        /// Bật tắt showApp của mở bán
        /// </summary>
        /// <param name="id"></param>
        void IsShowAppOpenSell(int id);

        /// <summary>
        /// Thêm nổi bật showApp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RstOpenSell ChangeOutstanding(int id);

        /// <summary>
        /// Dừng bán (dừng không được mở lại)
        /// </summary>
        /// <param name="id"></param>
        void StopOpenSell(int id);

        /// <summary>
        /// Danh sách mở bán của Đại lý
        /// </summary>
        PagingResult<RstOpenSellDto> FindAll(FilterRstOpenSellDto input);

        /// <summary>
        /// Danh sách ngân hàng có thể phân phối cho mở bán của đại lý
        /// </summary>
        List<BankAccountDtoForOpenSell> BankAccountCanDistributionOpenSell(int projectId, int? bankType);
        /// <summary>
        /// list sản phẩm bán hộ
        /// </summary>
        /// <returns></returns>
        IEnumerable<RstOpenSellByTradingDto> FindOpenSellBanHoByTrading();
        RstOpenSellDto FindById(int id);
        #endregion

        #region Sản phẩm mở bán
        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        /// <param name="input"></param>
        void AddOpenSellDetail(CreateRstOpenSellDetailDto input);

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="openSellDetailIds"></param>
        void DeleteOpenSellDetail(List<int> openSellDetailIds);

        /// <summary>
        /// Bật tắt showApp của sản phẩm mở bán
        /// </summary>
        /// <param name="id"></param>
        void IsShowAppOpenSellDetail(int id);

        /// <summary>
        /// Xem chi tiết căn hộ của mở bán
        /// </summary>
        /// <param name="openSellDetailId"></param>
        /// <returns></returns>
        RstProductItemByOpenSellDetailDto ProductItemByOpenSellDetail(int openSellDetailId);

        /// <summary>
        /// Xem danh sách sản phẩm mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<RstOpenSellDetailDto> FindAllOpenSellDetail(FilterRstOpenSellDetailDto input);

        /// <summary>
        /// Danh sách mở bán của đại lý có thể đặt lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<RstOpenSellDetailDto> GetAllOpenSellDetailForOrder(FilterRstOpenSellDetailForOrder input);

        /// <summary>
        /// Thông tin hợp đồng mới nhất được tạo trong mở bán
        /// </summary>
        InfoOrderNewInProjectDto InfoOrderNewInOpenSell(int openSellId);

        /// <summary>
        /// Ẩn hiển thị giá
        /// </summary>
        /// <param name="input"></param>
        void HideOpenSellDetail(RstOpenSellDetailHidePriceDto input);

        /// <summary>
        /// Mở hiển thị giá
        /// </summary>
        /// <param name="input"></param>
        void ShowOpenSellDetail(int openSellDetailId);

        /// <summary>
        /// Đại lý khóa sản phẩm mở bán (căn hộ và sản phẩm mở bán đang ở khởi tạo và không bị khóa ở phân phối
        /// </summary>
        /// <param name="input"></param>
        void LockOpenSellDetail(LockRstOpenSellDetailDto input);
        #endregion

        #region Phê duyệt
        void Request(RstRequestDto input);
        void Approve(RstApproveDto input);
        void Cancel(RstCancelDto input);
        #endregion

    }
}

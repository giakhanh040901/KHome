using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstDistributionProductItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstDistributionServices
    {
        #region Đối tác - Phân phối cho đại lý
        /// <summary>
        /// Đối tác - Thêm phân phối cho đại lý
        /// </summary>
        RstDistribution Add(CreateRstDistributionDto input);

        /// <summary>
        /// Cập nhật lại thông tin 
        /// </summary>
        void Update(UpdateRstDistributionDto input);

        /// <summary>
        /// Tạm dừng phân phối (Đang phân phối - Tạm dừng)
        /// </summary>
        /// <param name="id"></param>
        void PauseDistribution(int id);

        /// <summary>
        /// Xem danh sách phân phối sản phẩm
        /// </summary>
        PagingResult<RstDistributionDto> FindAll(FilterRstDistributionDto input);

        /// <summary>
        /// Xem chi tiet phan phoi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RstDistributionDto FindById(int id);

        /// <summary>
        /// Xóa phân phối
        /// </summary>
        void DeleteDistribution(int id);
        #endregion

        #region Sản phẩm của phân phối

        /// <summary>
        /// Xem chi tiết sản phẩm của phân phối
        /// </summary>
        RstDistributionProductItemDto FindDistributionItemById(int distributionProductItemId);

        /// <summary>
        /// Đối tác - Thêm căn cho phân phối sản phẩm
        /// </summary>
        void AddDistributionProductItem(CreateRstDistributionProductItemDto input);

        /// <summary>
        /// Xóa căn 
        /// </summary>
        /// <param name="input"></param>
        void DeleteDistributionProductItem(DeleteRstDistributionProductDto input);

        /// <summary>
        /// Danh sách căn của phân phối sản phẩm
        /// </summary>
        PagingResult<RstDistributionProductItemDto> FindAllItemByDistribution(int distributionId, FilterRstDistributionProductItemDto input);

        /// <summary>
        /// Đối tác khóa căn
        /// </summary>
        void LockDistributionProductItem(LockRstDistributionProductItemDto input);
        #endregion

        #region Đối tácPhê duyệt phân phối sản phẩm cho đại lý
        void Request(RstRequestDto input);
        void Approve(RstApproveDto input);
        void Cancel(RstCancelDto input);
        #endregion

        /// <summary>
        /// Danh sách sản phẩm được phân phối theo dự án cho đại lý
        /// </summary>
        List<RstDistributionProductItemDto> GetAllProductItemByTrading(FilterRstDistributionProductItemByTradingDto input);
        List<RstDistributionByTradingDto> GetAllByTrading();
    }
}

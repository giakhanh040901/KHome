using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.Sale;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProductItemServices
    {
        RstProductItem Add(CreateRstProductItemDto input);
        void Delete(int id);
        PagingResult<RstProductItemDto> FindAll(FilterRstProductItemDto input);
        RstProductItemDto FindById(int id);
        RstProductItem Update(UpdateRstProductItemDto input);

        /// <summary>
        /// Cập nhật trạng thái của căn hộ về khởi tạo
        /// </summary>
        Task ResetStatusProductItem(int productItemId);

        /// <summary>
        /// Lấy danh sách sản phẩm dự án có thể phân phối cho đại lý (Lọc những căn đã phân phối cho đại lý trước đó)
        /// </summary>
        List<RstProductItemDto> GetAllProductItemCanDistributionForTrading(FilterRstProductItemCanDistributionDto input);

        void UpdateProductItemDesignDiagramContent(UpdateRstProductItemDesignDiagramDto input);

        void UpdateProductItemMaterialContent(UpdateRstProductItemMaterialDto input);

        #region Tiện ích căn hộ
        //Tiện ích căn hộ
        PagingResult<RstSelectProductItemProjectUtilityDto> FindAllProjectUtility(FilterProductItemProjectUtilityDto input);
        PagingResult<RstProductItemUtilityDto> FindAllProjectUtilitySelected(FilterProductItemProjectUtilityDto input, int productItemId);
        void AddProductItemProjectUtility(CreateRstProductItemUtilityDto input);
        void ChangeStatusProductItemUtility(int id);
        #endregion

        #region Chính sách ưu đãi
        //Chính sách ưu đãi từ chủ đầu tư
        void AddProductItemProjectPolicy(CreateRstProductItemProjectPolicyDto input);
        PagingResult<RstProductItemProjectPolicyDto> FindAllProjectPolicy(FilterProductItemProjectPolicyDto input);
        void ChangeStatusProductItemPolicy(int id);
        #endregion

        #region App
        /// <summary>
        /// Lấy danh sách sản phẩm mở bán của đại lý theo dự án
        /// </summary>
        List<AppGetAllProductItemDto> AppGetAllProjectItem(AppFilterProductItemDto input);

        /// <summary>
        /// Các tham số chuẩn bị cho lọc sản phẩm dự án
        /// </summary>
        AppGetParamsFindProductItemDto AppGetParamsFindProductItem(int openSellId);

        /// <summary>
        /// Xem chi tiết của sản phẩm
        /// </summary>
        AppRstProductItemDetailDto AppProductItemDetail(int openSellDetailId);

        /// <summary>
        /// Thông tin hình ảnh của sản phẩm mở bán
        /// </summary>
        List<AppRstProductItemMediaDto> AppProductItemDetailMedia(int openSellDetailId);

        /// <summary>
        /// Tìm các sản phẩm mở bán tương tự trong cùng dự án
        /// </summary>
        AppRstProductItemSimilarDto OpenSellDetailSimilar(int openSellDetailId);
        void LockProductItem(RstProductItemLockingDto input);
        List<RstProductItem> ReplicateProductItem(CreateRstListProductItemReplicationDto input);
        RstProductItem ChangeStatus(int id, int status);
        void ImportExcelProductItem(ImportExcelProductItemDto dto);
        #endregion

        ExportResultDto ImportFileTemplate(int projectId);

        /// <summary>
        /// Thông tin hợp đồng mới nhất được tạo trong dự án
        /// </summary>
        InfoOrderNewInProjectDto InfoOrderNewInProject(int projectId);
    }
}

using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.ProductBond;
using EPIC.Entities.Dto.ProductBondSecondPrice;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondSecondaryService
    {
        BondSecondary Add(CreateProductBondSecondaryDto input);
        void Update(UpdateProductBondSecondaryDto input);
        ViewProductBondSecondaryDto FindById(int id);
        /// <summary>
        /// Lấy danh sách bán theo kỳ hạn, nếu là dlsc thì không cần truyền vào tradingProviderId,
        /// nếu là supper admin thì sẽ truyền vào tradingProviderId
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="isActivate">Khi lọc theo chính sách và kỳ hạn đang được hoạt động và đang cho đặt lệnh</param>
        /// <returns></returns>
        PagingResult<ViewProductBondSecondaryDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, bool isActivate, string isClose);
        List<ViewProductBondSecondaryDto> FindAllOrder();
        void TradingProviderSubmit(int bondSecondaryId);
        void TradingProviderApprove(int bondSecondaryId, int status);
        void SuperAdminApprove(int bondSecondaryId, int status);
        void IsClose(int bondSecondaryId, string isClose);
        void PolicyIsShowApp(int policyId, string isShowApp);
        void PolicyDetailIsShowApp(int policyDetailId, string isShowApp);
        void UpdatePolicy(int policyId, UpdateProductBondPolicyDto body);
        void UpdatePolicyDetail(int policyDetailId, UpdateProductBondPolicyDetailDto body);
        void DeletePolicy(int policyId);
        void DeletePolicyDetail(int policyDetailId);
        void IsShowApp(int bondSecondaryId, string isShowApp);
        ProductBondPolicyDto AddPolicy(CreateProductBondPolicySpecificDto body);
        List<ProductBondPolicyDto> AddPolicySecondary(int policytempId, List<int> bondsecondaryId);
        List<BondPolicyDetail> GetAllListPolicyDetailByPolicy(int policyId);
        void AddPolicyDetail(CreateProductBondPolicyDetailDto body);
        void ImportSecondPrice(IFormFile file, int bondSecondaryId);
        void DeleteSecondPrice(int BondSecondaryId);
        PagingResult<BondSecondPrice> FindAll(int pageSize, int pageNumber, int bondSecondaryId);
        void Request(RequestStatusDto input);
        void Approve(ApproveStatusDto input);
        void Check(CheckStatusDto input);
        void Cancel(CancelStatusDto input);
        void UpdatePrice(UpdateSecondaryPriceDto body);
        int ChangePolicyStatus(int id);
        int ChangePolicyDetailStatus(int id);

        #region app
        /// <summary>
        /// Danh sách sản phẩm trái phiếu cho khách hàng theo bán theo kỳ hạn
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="orderByInterestDesc"></param>
        /// <returns></returns>
        List<BondInfoSecondaryDto> FindAllBondSecondary(string keyword, bool orderByInterestDesc);

        /// <summary>
        /// Lấy thông tin Lô trái phiếu cho App từ bán theo kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AppBondInfoDto FindBondById(int id);

        /// <summary>
        /// Lấy thông tin chính sách đang được show App theo bán theo kỳ hạn
        /// </summary>
        /// <param name="bondSecondaryId"></param>
        /// <returns></returns>
        List<AppBondPolicyDto> FindAllListPolicy(int bondSecondaryId);

        /// <summary>
        /// Lấy thông tin kỳ hạn còn được show App theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        List<AppBondPolicyDetailDto> FindAllListPolicyDetail(int policyId, decimal totalValue);
        #endregion
    }
}

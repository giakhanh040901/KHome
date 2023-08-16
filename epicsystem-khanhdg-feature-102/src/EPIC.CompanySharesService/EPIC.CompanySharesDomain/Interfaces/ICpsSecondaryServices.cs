using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CpsApp;
using EPIC.CompanySharesEntities.Dto;
using EPIC.CompanySharesEntities.Dto.CpsSecondary;
using EPIC.CompanySharesEntities.Dto.CpsSecondPrice;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.CoreApprove;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.CompanySharesEntities.Dto.Policy;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface ICpsSecondaryServices
    {
        CpsSecondary Add(CreateCpsSecondaryDto input);
        void Update(UpdateCpsSecondaryDto input);
        ViewCpsSecondaryDto FindById(int id);
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
        PagingResult<ViewCpsSecondaryDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, bool isActivate, string isClose);
        List<ViewCpsSecondaryDto> FindAllOrder();
        void TradingProviderSubmit(int cpsSecondaryId);
        void TradingProviderApprove(int cpsSecondaryId, int status);
        void SuperAdminApprove(int cpsSecondaryId, int status);
        void IsClose(int cpsSecondaryId, string isClose);
        void PolicyIsShowApp(int policyId, string isShowApp);
        void PolicyDetailIsShowApp(int policyDetailId, string isShowApp);
        void UpdatePolicy(int policyId, UpdateCpsPolicyDto body);
        void UpdatePolicyDetail(int policyDetailId, UpdateCpsPolicyDetailDto body);
        void DeletePolicy(int policyId);
        void DeletePolicyDetail(int policyDetailId);
        void IsShowApp(int cpsSecondaryId, string isShowApp);
        CpsPolicyDto AddPolicy(CreateCpsPolicySpecificDto body);
        List<CpsPolicyDto> AddPolicySecondary(int policytempId, List<int> cpssecondaryId);
        List<CpsPolicyDetail> GetAllListPolicyDetailByPolicy(int policyId);
        void AddPolicyDetail(CreateCpsPolicyDetailDto body);
        //void ImportSecondPrice(IFormFile file, int cpsSecondaryId);
        //void DeleteSecondPrice(int CpsSecondaryId);
        //PagingResult<CpsSecondPrice> FindAll(int pageSize, int pageNumber, int cpsSecondaryId);
        //void Request(RequestStatusDto input);
        //void Approve(ApproveStatusDto input);
        //void Check(CheckStatusDto input);
        //void Cancel(CancelStatusDto input);
        //void UpdatePrice(UpdateSecondaryPriceDto body);
        int ChangePolicyStatus(int id);
        int ChangePolicyDetailStatus(int id);

        #region app
        /// <summary>
        /// Danh sách sản phẩm trái phiếu cho khách hàng theo bán theo kỳ hạn
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="orderByInterestDesc"></param>
        /// <returns></returns>
        List<CpsInfoSecondaryDto> FindAllCpsSecondary(string keyword, bool orderByInterestDesc);

        /// <summary>
        /// Lấy thông tin Lô trái phiếu cho App từ bán theo kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AppCpsInfoDto FindCpsById(int id);

        /// <summary>
        /// Lấy thông tin chính sách đang được show App theo bán theo kỳ hạn
        /// </summary>
        /// <param name="cpsSecondaryId"></param>
        /// <returns></returns>
        List<AppCpsPolicyDto> FindAllListPolicy(int cpsSecondaryId);

        /// <summary>
        /// Lấy thông tin kỳ hạn còn được show App theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        List<AppCpsPolicyDetailDto> FindAllListPolicyDetail(int policyId, decimal totalValue);
        #endregion
    }
}

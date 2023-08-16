using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.Order;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IDistributionServices
    {
        Distribution Add(CreateDistributionDto input);
        ViewPolicyDto AddPolicy(CreatePolicySpecificDto body);
        void AddPolicyDetail(CreatePolicyDetailDto body);
        void Approve(InvestApproveDto input);
        void Cancel(CancelStatusDto input);
        int ChangeStatusPolicy(int policyId);
        int ChangeStatusPolicyDetail(int id);
        void Check(CheckStatusDto input);
        void DeletePolicy(int policyId);
        void DeletePolicyDetail(int policyDetailId);
        PagingResult<ViewDistributionDto> FindAll(FilterInvestDistributionDto input);
        List<ViewDistributionDto> FindAllOrder(FilterInvestDistributionDto input);
        ViewDistributionDto FindById(int id);
        OverViewDistributionDto FindOverViewById(int id);
        ViewPolicyDto FindPolicyAndPolicyDetailByPolicyId(int policyId);
        PolicyDetail FindPolicyDetailById(int policyDetailId);
        List<PolicyDetail> GetAllListPolicyDetailByPolicy(int policyId);
        int IsClose(int distributionId);
        int IsShowApp(int bondSecondaryId);
        int PolicyDetailIsShowApp(int policyDetailId);
        int PolicyIsShowApp(int policyId);
        void Request(RequestStatusDto input);
        void Update(UpdateDistributionDto input);
        void UpdatePolicy(UpdatePolicyDto body);
        void UpdatePolicyDetail(UpdatePolicyDetailDto body);

        /// <summary>
        /// Thêm ngân hàng cho bán phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessCustomerBankAccId"></param>
        void UpdateBank(int id, int businessCustomerBankAccId);

        /// <summary>
        /// Update nội dung tổng quan của sản phẩm
        /// </summary>
        /// <param name="input"></param>
        void UpdateOverviewContent(UpdateDistributionOverviewContentDto input);

        PagingResult<ViewPolicyDto> GetAllPolicyByDistri(InvestPolicyFilterDto input);

        List<BusinessCustomerBankDto> FindBankByDistributionId(int distributionId, int type);
        IEnumerable<ViewDistributionByTradingDto> FindDistributionBanHoByTrading();

        #region
        /// <summary>
        /// Danh sách dự án đầu tư theo bán phân phối
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="orderByInterestDesc">null thì ko sắp xếp theo, Y: Giảm dần, N: tăng dần</param>
        /// <param name="isSaleView">True: sale xem danh sách bảng hàng</param>
        /// <returns></returns>
        List<ProjectDistributionDto> FindAllProjectDistribution(AppFilterProjectDistribution input);

        /// <summary>
        /// Lấy thông tin dự án cho App từ bán phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AppProjectDto FindProjectById(int id);

        /// <summary>
        /// Lấy thông tin chính sách đang được show App theo bán phân phối
        /// </summary>
        /// <param name="bondSecondaryId"></param>
        /// <returns></returns>
        List<AppInvestPolicyDto> FindAllListPolicy(int bondSecondaryId);

        /// <summary>
        /// Lấy thông tin kỳ hạn còn được show App theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        List<AppInvestPolicyDetailDto> FindAllListPolicyDetail(int policyId, decimal? totalValue);
        List<BusinessCustomerBankDto> FindBankByTradingInvest(int? distributionId = null, int? type = null);

        #endregion

    }
}

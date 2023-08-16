using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPIC.GarnerRepositories.GarnerDistributionEFRepository;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerProductPrice;
using Microsoft.AspNetCore.Http;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.Dto.GarnerHistory;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerDistributionServices
    {
        #region Phân phối sản phẩm
        /// <summary>
        /// Thêm phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerDistribution Add(CreateGarnerDistributionDto input);

        /// <summary>
        /// Cập nhật phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerDistribution Update(UpdateGarnerDistributionDto input);

        /// <summary>
        /// Lấy danh sách phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<GarnerDistributionDto> FindAll(FilterGarnerDistributionDto input);

        /// <summary>
        /// Thông tin chi tiết phân phối sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GarnerDistributionDto FindById(int id);

        /// <summary>
        /// thay đổi trạng thái IsClose của distribution
        /// </summary>
        /// <param name="distributionId"></param>
        void DistributionClose(int distributionId);

        /// <summary> 
        /// Lấy thông tin phân phối sản phẩm để đặt lệnh CMS
        /// </summary>
        /// <returns></returns>
        List<GarnerDistributionDto> FindAllDistribution(GarnerDistributionFilterDto input);

        /// <summary>
        /// Bật tắt show app
        /// </summary>
        /// <param name="distributionId"></param>
        void IsShowApp(int distributionId);

        /// <summary>
        /// Lấy danh sách ngân hàng thu chi của đại lý theo Phân phối sản phẩm
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<BusinessCustomerBankDto> FindBankByDistributionId(int distributionId, int type);


        /// <summary>
        /// EPIC, ROOT_EPIC Xét phân phối sản phẩm làm mặc định 
        /// </summary>
        void SetDefault(int distributionId);
        #endregion

        #region Duyệt
        /// <summary>
        /// Lấy thông tin tổng quan của dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GarnerProductOverviewDto FindProductOverview(int id);

        /// <summary>
        /// Yêu cầu duyệt phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        void Request(CreateGarnerRequestDto input);

        /// <summary>
        /// Phê duyệt
        /// </summary>
        /// <param name="input"></param>
        void Approve(GarnerApproveDto input);

        /// <summary>
        /// Hủy duyệt
        /// </summary>
        /// <param name="input"></param>
        void Cancel(GarnerCancelDto input);

        /// <summary>
        /// Kiểm tra
        /// </summary>
        /// <param name="input"></param>
        void Check(GarnerCheckDto input);

        /// <summary>
        /// Update tổng quan sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        void UpdateProductOverview(UpdateGarnerProductOverviewDto input);
        #endregion

        #region Chính sách bán theo kỳ hạn
        /// <summary>
        /// Thêm chính sách
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        GarnerPolicyMoreInfoDto AddPolicy(CreatePolicyDto body);
        /// <summary>
        /// Thêm kỳ hạn
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        GarnerPolicyDetailDto AddPolicyDetail(CreatePolicyDetailDto body);
        /// <summary>
        /// Cập nhật chính sách
        /// </summary>
        /// <param name="input"></param>
        void UpdatePolicy(UpdatePolicyDto input);
        /// <summary>
        /// Cập nhậy kỳ hạn
        /// </summary>
        /// <param name="input"></param>
        void UpdatePolicyDetail(UpdatePolicyDetailDto input);
        /// <summary>
        /// Xóa chính sách
        /// </summary>
        /// <param name="policyId"></param>
        void DeletePolicy(int policyId);
        /// <summary>
        /// Xóa kỳ hạn
        /// </summary>
        /// <param name="policyId"></param>
        void DeletePolicyDetail(int policyId);
        /// <summary>
        /// thay đổi trạng thái của chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        int UpdateStatusPolicy(int policyId);
        /// <summary>
        /// Thay đổi trạng thái của kỳ hạn
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        int UpdateStatusPolicyDetail(int policyDetailId);
        /// <summary>
        /// Lấy danh sách Policy theo distribution
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        List<GarnerPolicyMoreInfoDto> GetAllPolicy(int distributionId, GarnerDistributionFilterDto input);
        /// <summary>
        /// Lấy danh sách kỳ hạn theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        List<GarnerPolicyDetailDto> GetByPolicyId(int policyId);
        /// <summary>
        /// Lấy thông tin chính sach và danh sách kỳ hạn theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        GarnerPolicyMoreInfoDto FindPolicyAndPolicyDetailByPolicyId(int policyId);
        /// <summary>
        /// Lấy danh sách kỳ hạn theo chính sách (sửa thành phân trang)
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        List<GarnerPolicyDetailDto> FindPolicyDetailByPolicyId(int policyId);
        GarnerPolicyMoreInfoDto PolicyIsShowApp(int policyId);
        /// <summary>
        /// Lấy danh sách distribution không phân trang
        /// </summary>
        /// <returns></returns>
        List<GarnerDistributionDto> GetAllDistribution(GarnerDistributionFilterDto input);

        #endregion

        #region App
        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy cho App
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<AppGarnerDistributionDto> AppDistributionGetAll(string keyword, bool isSaleView);

        /// <summary>
        /// Bộ lọc bảng hàng
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="listTradingProvider"></param>
        /// <returns></returns>
        List<AppGarnerDistributionDto> AppFilterDistribution(string keyword, List<int> listTradingProviderIds = null);

        /// <summary>
        /// Lấy danh sách kỳ hạn theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        List<AppGarnerPolicyDetailDto> AppListPolicyDetail(int policyId, decimal totalValue);

        /// <summary>
        ///  App tổng quan sản phẩm tích lũy
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        AppDistributionOverviewDto DistributionOverview(int policyId);
        #endregion

        #region Config Contract Code
        void AddConfigContractCode(CreateConfigContractCodeDto input);
        /// <summary>
        /// Cập nhật config contract code
        /// </summary>
        /// <param name="input"></param>
        void UpdateConfigContractCode(UpdateConfigContractCodeDto input);

        PagingResult<GarnerConfigContractCodeDto> GetAllConfigContractCode(FilterConfigContractCodeDto input);
        List<GarnerConfigContractCodeDto> GetAllConfigContractCodeStatusActive();
        GarnerConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId);
        void UpdateConfigContractCodeStatus(int configContractCodeId);
        void DeleteConfigContractCode(int configContractCodeId);
        #endregion

        #region Bảng giá
        void ImportProductPrice(IFormFile file, int ditributionId);
        void DeleteProductPrice(int ditributionId);
        PagingResult<GarnerProductPrice> FindAll(FilterGarnerProductPriceDto input);
        void UpdateProductPrice(UpdateProductPriceDto input);
        #endregion

        #region File chính sách
        void AddDistributionPolicyFile(CreateGarnerProductOverviewFileDto input);
        void UpdateDistributionPolicyFile(CreateGarnerProductOverviewFileDto input);
        void DeleteDistributionPolicyFile(int id);
        PagingResult<GarnerProductOverviewFile> FindAllDistributionFile(FilterGarnerDistributionFileDto input);

        #endregion

        PagingResult<GarnerHistoryUpdate> FindAllDistributionHistory(FilterGarnerDistributionHistoryDto input);
        GarnerDistributionTradingBankAccount ChooseAutoAccount(int id);

        /// <summary>
        /// Lấy danh sách sản phẩm phân phối bán hộ
        /// </summary>
        /// <returns></returns>
        IEnumerable<GarnerDistributionByTradingDto> FindAllDistributionBanHoByTrading();
    }
}

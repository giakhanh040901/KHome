using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectServices
    {
        /// <summary>
        /// Thêm dự án
        /// </summary>
        RstProjectDto Add(RstCreateProjectDto input);

        /// <summary>
        /// Cập nhật dự án
        /// </summary>
        RstProjectDto Update(RstUpdateProjectDto input);

        /// <summary>
        /// Xóa dự án
        /// </summary>
        /// <param name="projectId"></param>
        void Delete(int projectId);

        /// <summary>
        /// Cập nhật mô tả
        /// </summary>
        void UpdateProjectOverviewContent(RstUpdateProjectOverviewContentDto input);

        /// <summary>
        /// Danh sách dự án
        /// </summary>
        PagingResult<ViewRstProjectDto> FindAll(FilterRstProjectDto input);

        /// <summary>
        /// Lấy danh sách dự án được phân phối cho đại lý
        /// </summary>
        /// <returns></returns>
        List<RstProjectDto> ProjectGetAll(List<int> tradingProviderIds);

        /// <summary>
        /// Xem chi tiết dự án
        /// </summary>
        RstProjectDto FindById(int id);

        void Request(RstRequestDto input);
        void Approve(RstApproveDto input);
        void Cancel(RstCancelDto input);

        /// <summary>
        /// App xem danh sách dự án
        /// </summary>
        /// <returns></returns>
        List<AppViewListProjectDto> AppFindProjects(AppFindProjectDto dto);
        /// <summary>
        /// Lấy các tham số để app tạo bộ lọc dự án
        /// </summary>
        /// <returns></returns>
        AppGetParamsFindProjectDto AppGetParamsFindProjects(bool isSaleView);
        /// <summary>
        /// App chi tiết dự án
        /// </summary>
        /// <returns></returns>
        AppDetailProjectDto AppGetDetailProject(int openSellId);
        /// <summary>
        /// App lấy hết media theo dự án
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        List<AppViewProjectMediaDto> AppGetAllMedia(int projectId);
    }
}

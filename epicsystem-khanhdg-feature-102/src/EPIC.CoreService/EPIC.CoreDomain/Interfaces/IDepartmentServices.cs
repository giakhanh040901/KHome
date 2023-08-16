using EPIC.CoreEntities.Dto.Sale;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Sale;
using System.Collections.Generic;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IDepartmentServices
    {
        List<DepartmentDto> FindAllList(int? parentId);
        DepartmentDto FindById(int departmentId);
        int MoveSale(MoveSaleDto input);
        int AssignManager(AssignManagerDto input);
        int AssignManager2(AssignManagerDto input);
        DepartmentDto Create(CreateDepartmentDto input);
        PagingResult<DepartmentSaleDto> FindAllListSale(SaleFilterDto input);
        DepartmentDto Update(UpdateDepartmentDto input);
        PagingResult<DepartmentDto> FindAll(int pageSize, int pageNumber, string keyword);
        void DeleteDepartment(int departmentId);
        void DeleteDepartmentManager(int managerType, int departmentId);
        void DepartmentChangeListSale(int departmentId, int newDepartmentId);

        /// <summary>
        /// Chuyển sale từ phòng ban này sang phòng ban khác
        /// </summary>
        /// <param name="input"></param>
        void UpdateListSaleToNewDepartment(UpdateListSaleToNewDepartment input);

        /// <summary>
        /// Xem danh sách sale của phòng ban đang xét
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="saleTypes"></param>
        /// <param name="departmentId"></param>
        /// <param name="isInvestor"></param>
        /// <returns></returns>
        PagingResult<DepartmentSaleDto> FindAllListSaleInDepartment(int pageSize, int pageNumber, string keyword, string saleTypes, int departmentId, bool? isInvestor);

        /// <summary>
        /// Thông tin các phòng ban con theo departmentId
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        List<ViewDepartmentDto> FindDepartmentChildById(int departmentId);

        /// <summary>
        /// Chuyển phòng ban đang xét đến phòng ban cha, kéo theo level của phòng ban cấp dưới
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="departmentParentId"></param>
        void UpdateLevelDepartment(int departmentId, int? departmentParentId);

        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        List<DepartmentDto> FindAllDepartmentNotDepartmentCurrent(int departmentId);
    }
}

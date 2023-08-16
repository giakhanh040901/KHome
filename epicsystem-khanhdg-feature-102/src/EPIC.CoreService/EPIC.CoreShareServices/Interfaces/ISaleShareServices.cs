using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Department;
using System.Collections.Generic;

namespace EPIC.CoreSharedServices.Interfaces
{
    public interface ISaleShareServices
    {
        /// <summary>
        /// Đệ quy tính số lượng Sale con và sale con mới trong tháng
        /// Sale là quản lý của phòng ban
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="departmentId"></param>
        /// <param name="isInvestor">nếu truyền vào true sẽ chỉ lấy investor, false sẽ chỉ lấy business, null sẽ lấy all</param>
        /// <param name="result"></param>
        /// <param name="resultMonth"></param>
        /// <param name="saleChildren"></param>
        void DeQuyDepartmentSaleChild(int tradingProviderId, int departmentId, bool? isInvestor, ref decimal result, ref decimal resultMonth, ref List<DepartmentSaleDto> saleChildren);

        /// <summary>
        /// Đệ quy thông tin phòng ban con của phòng ban đang xét
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="departmentId"></param>
        /// <param name="listDepartment"></param>
        void DeQuyDepartmentChild(int tradingProviderId, int departmentId, ref List<Department> listDepartment);

        /// <summary>
        /// Đệ quy tìm list phòng ban cấp trên
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="listDepartmentParent"></param>
        void DeQuyDepartmentParent(int tradingProviderId, int departmentId, ref List<Department> listDepartmentParent);
    }
}

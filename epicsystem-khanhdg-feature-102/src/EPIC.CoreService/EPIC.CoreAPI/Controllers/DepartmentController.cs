using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.Entities.Dto.Department;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/department")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentServices _departmentServices;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentServices departmentServices)
        {
            _logger = logger;
            _departmentServices = departmentServices;
        }

        /// <summary>
        /// Thêm phòng ban (có thể là phòng ban con), cho dlsc
        /// </summary>
        /// <returns></returns>
        //[PermissionFilter()]
        [HttpPost("create")]
        [PermissionFilter(Permissions.CorePhongBan_ThemMoi)]
        public APIResponse Create([FromBody] CreateDepartmentDto input)
        {
            try
            {
                var result = _departmentServices.Create(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin phòng ban
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.CorePhongBan_CapNhat)]
        public APIResponse Update([FromBody] UpdateDepartmentDto input)
        {
            try
            {
                var result = _departmentServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách các phòng ban con theo id cha
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("find-all-list")]
        [PermissionFilter(Permissions.CorePhongBan_DanhSach)]
        public APIResponse FindAllList(int? parentId)
        {
            try
            {
                var result = _departmentServices.FindAllList(parentId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy phòng ban theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _departmentServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sale trong phòng ban (saler có thể là người hoặc tổ chức),
        ///nếu truyền isInvestor = true sẽ chỉ lấy saler là con người
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-list-sale")]
        public APIResponse FindAllListSale([FromQuery] SaleFilterDto input)
        {
            try
            {
                var result = _departmentServices.FindAllListSale(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin sale trong phòng ban 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="saleTypes"></param>
        /// <param name="departmentId"></param>
        /// <param name="isInvestor"></param>
        /// <returns></returns>
        [HttpGet("find-all-list-sale-in-department")]
        public APIResponse FindAllListSaleInDepartment(int pageSize, int pageNumber, string keyword, string saleTypes, int departmentId, bool? isInvestor)
        {
            try
            {
                var result = _departmentServices.FindAllListSaleInDepartment(pageSize, pageNumber, keyword?.Trim(), saleTypes?.Trim(), departmentId, isInvestor);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm sale vào phòng ban, gán chức vụ
        /// </summary>
        /// <returns></returns>
        [HttpPost("move-sale")]
        public APIResponse MoveSale(MoveSaleDto input)
        {
            try
            {
                var result = _departmentServices.MoveSale(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gán sale là quản lý lên cây phòng ban, kiểm tra sale phải thuộc phòng ban
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("assign-manager")]
        [PermissionFilter(Permissions.CorePhongBan_ThemQuanLy)]
        public APIResponse AssignManager(AssignManagerDto input)
        {
            try
            {
                var result = _departmentServices.AssignManager(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gán sale là quản lý lên cây phòng ban (sale chỉ là con người không được là tổ chức), kiểm tra sale phải thuộc phòng ban
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("assign-manager-2")]
        [PermissionFilter(Permissions.CorePhongBan_ThemQuanLyDoanhNghiep)]
        public APIResponse AssignManager2(AssignManagerDto input)
        {
            try
            {
                var result = _departmentServices.AssignManager2(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách phòng ban chia trang hoặc không, tìm kiếm gần đúng tên phòng, địa chỉ theo keyword
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse GetAll(int? pageSize, int pageNumber, string keyword)
        {
            try
            {
                var result = _departmentServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        
        /// <summary>
        /// Xoá phòng ban nếu phòng ban trống
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{departmentId}")]
        public APIResponse Delete(int departmentId)
        {
            try
            {
                _departmentServices.DeleteDepartment(departmentId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa manager trong phòng ban
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpDelete("delete-department-manager")]
        public APIResponse DeleteDepartmentManager(int managerType, int departmentId)
        {
            try
            {
                _departmentServices.DeleteDepartmentManager(departmentId, managerType);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Chuyển danh sách sale sang phòng ban khác
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpPut("department-change-list-sale")]
        public APIResponse DepartmentChangeListSale(int managerType, int departmentId)
        {
            try
            {
                _departmentServices.DepartmentChangeListSale(departmentId, managerType);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update danh sách sale từ phòng ban này sang phòng ban khác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-sale-to-new-department")]
        public APIResponse UpdateListSaleToNewDepartment(UpdateListSaleToNewDepartment input)
        {
            try
            {
                _departmentServices.UpdateListSaleToNewDepartment(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách các phòng ban cấp dưới của phòng ban đang xét
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("find-department-child")]
        public APIResponse FindDepartmentChildById(int departmentId)
        {
            try
            {
                var result = _departmentServices.FindDepartmentChildById(departmentId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update cấp của phòng ban đang xét, kéo theo các phòng ban cấp dưới update theo
        /// nếu phòng ban cha null thì phòng ban đang xét lên cấp 1
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="departmentParentId"></param>
        /// <returns></returns>
        [HttpPut("update-level-department")]
        public APIResponse UpdateLevelDepartment(int departmentId, int? departmentParentId)
        {
            try
            {
                _departmentServices.UpdateLevelDepartment(departmentId, departmentParentId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách phòng ban trừ phòng ban đang xét và những phòng ban cấp dưới của invest
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("find-all-department-not-department-current")]
        public APIResponse FindAllDepartmentNotDepartmentCurrent(int departmentId)
        {
            try
            {
                var result = _departmentServices.FindAllDepartmentNotDepartmentCurrent(departmentId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

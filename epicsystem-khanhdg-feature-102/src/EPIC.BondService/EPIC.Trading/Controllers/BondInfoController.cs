using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/product-bond-info")]
    [ApiController]
    public class BondInfoController : BaseController
    {
        private IBondProductBondInfoService _productBondInfoService;
        public BondInfoController(ILogger<BondInfoController> logger, IBondProductBondInfoService productBondInfoService)
        {
            _logger = logger;
            _productBondInfoService = productBondInfoService;
        }

        [HttpGet]
        [Route("find")]
        [PermissionFilter(Permissions.BondMenuQLTP_LTP_DanhSach)]
        public APIResponse GetAllProductBondInfo(int pageNumber, int? pageSize, string keyword, string status, string isCheck, DateTime? issueDate, DateTime? dueDate)
        {
            try
            {
                var tradingProvider = _productBondInfoService.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status?.Trim(), isCheck?.Trim(), issueDate, dueDate);
                return new APIResponse(Utils.StatusCode.Success, tradingProvider, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.Bond_LTP_TTC_ChiTiet)]
        public APIResponse GetProductBondInfo(int id)
        {
            try
            {
                var result = _productBondInfoService.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo yêu cầu duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request")]
        [PermissionFilter(Permissions.BondMenuQLTP_LTP_TrinhDuyet)]
        public APIResponse AddRequest([FromBody] RequestStatusDto input)
        {
            try
            {
                _productBondInfoService.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt doanh nghiệp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approve")]
        [PermissionFilter(Permissions.BondQLPD_PDBTKH_PheDuyetOrHuy)]
        public APIResponse Approve(ApproveStatusDto input)
        {
            try
            {
                _productBondInfoService.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Epic xác minh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("check")]
        [PermissionFilter(Permissions.BondMenuQLTP_LTP_EpicXacMinh)]
        public APIResponse Check([FromBody] CheckStatusDto input)
        {
            try
            {
                _productBondInfoService.Check(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Xác minh thành công");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel")]
        [PermissionFilter(Permissions.BondQLPD_PDBTKH_PheDuyetOrHuy)]
        public APIResponse Cancel([FromBody] CancelStatusDto input)
        {
            try
            {
                _productBondInfoService.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đóng-Mở
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("close-open")]
        [PermissionFilter(Permissions.BondMenuQLTP_LTP_DongTraiPhieu)]
        public APIResponse CloseOpen(int id)
        {
            try
            {
                _productBondInfoService.CloseOpen(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.BondMenuQLTP_LTP_ThemMoi)]
        public APIResponse AddProductBondInfo([FromBody] CreateProductBondInfoDto body)
        {
            try
            {
                var result = _productBondInfoService.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.Bond_LTP_TTC_CapNhat)]
        public APIResponse UpdateProductBondInfo([FromBody] UpdateProductBondInfoDto body, int id)
        {
            try
            {
                var result = _productBondInfoService.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.BondMenuQLTP_LTP_Xoa)]
        public APIResponse DeleteProductBondInfo(int id)
        {
            try
            {
                var result = _productBondInfoService.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("coupon/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.BondMenuQLTP_LTP_TTCT)]
        public APIResponse GetCoupon(int id)
        {
            try
            {
                var result = _productBondInfoService.FindCouponById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

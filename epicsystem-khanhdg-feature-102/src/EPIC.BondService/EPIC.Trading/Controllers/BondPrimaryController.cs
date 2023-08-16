using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.ProductBondPrimary;
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
    [Route("api/bond/product-bond-primary")]
    [ApiController]
    public class BondPrimaryController : BaseController
    {
        private readonly IBondPrimaryService _productBondPrimaryServices;

        public BondPrimaryController(ILogger<BondPrimaryController> logger, IBondPrimaryService productBondPrimaryServices)
        {
            _logger = logger;
            _productBondPrimaryServices = productBondPrimaryServices;
        }

        [HttpGet]
        [Route("find")]
        [PermissionFilter(Permissions.BondMenuQLTP_PHSC_DanhSach)]
        public APIResponse GetAllProductBondPrimary(int pageNumber, int? pageSize, string keyword, string status)
        {
            try
            {
                var productBondPrimary = _productBondPrimaryServices.FindAllProductBondPrimary(pageSize ?? 100, pageNumber, keyword?.Trim(), status?.Trim());
                return new APIResponse(Utils.StatusCode.Success, productBondPrimary, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.Bond_PHSC_TTC_ChiTiet)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _productBondPrimaryServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.BondMenuQLTP_PHSC_ThemMoi)]
        public APIResponse AddProductBondPrimary([FromBody] CreateProductBondPrimaryDto body)
        {
            try
            {
                var result = _productBondPrimaryServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.Bond_PHSC_TTC_ChinhSua)]
        public APIResponse UpdateProductBondPrimary([FromBody] UpdateProductBondPrimaryDto body, int id)
        {
            try
            {
                var result = _productBondPrimaryServices.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.BondMenuQLTP_PHSC_Xoa)]
        public APIResponse DeleteProductBondPrimary(int id)
        {
            try
            {
                var result = _productBondPrimaryServices.Delete(id);
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
        [PermissionFilter(Permissions.BondMenuQLTP_PHSC_TrinhDuyet)]
        public APIResponse AddRequest([FromBody] RequestStatusDto input)
        {
            try
            {
                _productBondPrimaryServices.Request(input);
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
        [PermissionFilter(Permissions.BondMenuQLTP_PHSC_PheDuyetOrHuy)]
        public APIResponse Approve(ApproveStatusDto input)
        {
            try
            {
                _productBondPrimaryServices.Approve(input);
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
        
        public APIResponse Check([FromBody] CheckStatusDto input)
        {
            try
            {
                _productBondPrimaryServices.Check(input);
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
        [PermissionFilter(Permissions.BondMenuQLTP_PHSC_PheDuyetOrHuy)]
        public APIResponse Cancel([FromBody] CancelStatusDto input)
        {
            try
            {
                _productBondPrimaryServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        //[Route("active/{id}")]
        //[HttpPut]
        //public APIResponse ActiveBondPrimary([FromRoute] int id, bool isActive)
        //{
        //    try
        //    {
        //        var result = _productBondPrimaryServices.ActiveBondPrimary(id, isActive);
        //        return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        //[Route("duyet/{id}")]
        //[HttpPut]
        //public APIResponse DuyetBondPrimary([FromRoute] int id, string status)
        //{
        //    try
        //    {
        //        var result = _productBondPrimaryServices.DuyetBondPrimary(id, status);
        //        return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        /// <summary>
        /// Lấy thông tin phát hành sơ cấp đã duyệt hợp đồng phân phối theo đại lý sơ cấp hiện hành, để tạo bán theo kỳ hạn
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("find-by-trading")]
        public APIResponse GetAllByTradingProvider()
        {
            try
            {
                var productBondPrimary = _productBondPrimaryServices.GetAllByCurrentTradingProvider();
                return new APIResponse(Utils.StatusCode.Success, productBondPrimary, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin phát hành sơ cấp theo đại lý sơ cấp 
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("find-by-trading/{tradingProviderId}")]
        public APIResponse GetAllByTradingProvider(int tradingProviderId)
        {
            try
            {
                var productBondPrimary = _productBondPrimaryServices.GetAllByTradingProvider(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, productBondPrimary, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

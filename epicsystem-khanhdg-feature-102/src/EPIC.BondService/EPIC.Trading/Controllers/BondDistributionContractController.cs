using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.DistributionContractPayment;
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

namespace EPIC.BondAPI.Controllers
{
    /// <summary>
    /// Hợp đồng phân phối
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/distribution-contract")]
    [ApiController]
    public class BondDistributionContractController : BaseController
    {
        private readonly IBondDistributionContractService _distributionContractServices;

        public BondDistributionContractController(ILogger<IBondDistributionContractService> logger, IBondDistributionContractService distributionContractServices)
        {
            _logger = logger;
            _distributionContractServices = distributionContractServices;
        }

        /// <summary>
        /// Lấy danh sách hợp đồng phân phối
        /// </summary>
        [Route("find")]
        [HttpGet]
        [PermissionFilter(Permissions.BondMenuQLTP_HDPP_DanhSach)]
        public APIResponse GetAll(int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var result = _distributionContractServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin Hợp đồng phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.BondMenuQLTP_HDPP_TTCT)]
        public APIResponse Get(int id)
        {
            try
            {
                var result = _distributionContractServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tính trái tức cho hợp đồng phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("coupon/{id}")]
        [PermissionFilter(Permissions.BondMenuQLTP_HDPP_TTCT)]
        public APIResponse GetCoupon(int id)
        {
            try
            {
                var result = _distributionContractServices.FindCouponById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo Hợp đồng phân phối
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.BondMenuQLTP_HDPP_ThemMoi)]
        public APIResponse Add([FromBody] CreateDistributionContractDto body)
        {
            try
            {
                var result = _distributionContractServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật Hợp đồng phân phối
        /// </summary>
        /// <returns></returns>
        [Route("update/{contractId}")]
        [HttpPut]
        
        public APIResponse Update(int contractId, [FromBody] UpdateDistributionContractDto body)
        {
            try
            {
                var result = _distributionContractServices.Update(contractId, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa Hợp đồng phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.BondMenuQLTP_HDPP_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _distributionContractServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách thanh toán của hợp đồng phân phối
        /// </summary>
        /// <returns></returns>
        [Route("payment/find")]
        [HttpGet]
        [PermissionFilter(Permissions.Bond_HDPP_TTTT_DanhSach)]
        public APIResponse GetAllPayments(int contractId, int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _distributionContractServices.FindAllContractPayment(contractId, pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin Thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/{id}")]
        [HttpGet]
        public APIResponse GetContractPayment(int id)
        {
            try
            {
                var result = _distributionContractServices.FindPaymentById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo mới thanh toán
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("payment/add")]
        [HttpPost]
        [PermissionFilter(Permissions.Bond_HDPP_TTTT_ThemMoi)]
        public APIResponse AddContractPayment([FromBody] CreateDistributionContractPaymentDto body)
        {
            try
            {
                var result = _distributionContractServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <returns></returns>
        [Route("payment/update/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.Bond_HDPP_TTTT_CapNhat)]
        public APIResponse UpdateContractPayment(int id, [FromBody] UpdateDistributionContractPaymentDto body)
        {
            try
            {
                var result = _distributionContractServices.UpdateContractPayment(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.Bond_HDPP_TTTT_Xoa)]
        public APIResponse DeleteContractPayment(int id)
        {
            try
            {
                var result = _distributionContractServices.DeleteContractPayment(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/approve/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.Bond_HDPP_TTTT_PheDuyet)]
        public APIResponse DeleteContractPaymentApprove(int id)
        {
            try
            {
                var result = _distributionContractServices.ContractPaymentApprove(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("payment/cancel/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.Bond_HDPP_TTTT_HuyPheDuyet)]
        public APIResponse DeleteContractPaymentCancel(int id)
        {
            try
            {
                var result = _distributionContractServices.ContractPaymentCancel(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tải lên file hồ sơ hợp đồng
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("file/add")]
        [HttpPost]
        [PermissionFilter(Permissions.Bond_HDPP_DMHSKHK_ThemMoi)]
        public APIResponse AddDistributionContractFile([FromBody] CreateDistributionContractFileDto body)
        {
            try
            {
                var result = _distributionContractServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thông tin file hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("file/{id}")]
        [HttpGet]
        public APIResponse GetContractFile(int id)
        {
            try
            {
                var result = _distributionContractServices.FindContractFileById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thông tin file hợp đồng theo hợp đồng phân phối
        /// </summary>
        /// <returns></returns>
        [Route("file/find")]
        [HttpGet]
        [PermissionFilter(Permissions.Bond_HDPP_DMHSKHK_DanhSach)]
        public APIResponse GetAllContractFiles(int contractId, int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _distributionContractServices.FindAllContractFile(contractId, pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa file hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("file/delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.Bond_HDPP_TTTT_ThemMoi)]
        public APIResponse DeleteContractFile(int id)
        {
            try
            {
                var result = _distributionContractServices.DeleteContractFile(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt file hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("file/approve/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.Bond_HDPP_DMHSKHK_PheDuyet)]
        public APIResponse DeleteContractFileApprove(int id)
        {
            try
            {
                var result = _distributionContractServices.ContractFileApprove(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt file hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("file/cancel")]
        [PermissionFilter(Permissions.Bond_HDPP_DMHSKHK_HuyPheDuyet)]
        public APIResponse DistributionContractFileCancel([FromBody] CancelStatusDto input)
        {
            try
            {
                _distributionContractServices.ContractFileCancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

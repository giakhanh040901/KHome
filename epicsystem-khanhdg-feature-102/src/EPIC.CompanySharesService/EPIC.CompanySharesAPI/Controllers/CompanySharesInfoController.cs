using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.CpsInfo;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN })]
    [Route("api/company-shares/cps-info")]
    [ApiController]
    public class CompanySharesInfoController : BaseController
    {
        private ICpsInfoServices _cpsInfoServices;
        public CompanySharesInfoController(ILogger<CompanySharesInfoController> logger, ICpsInfoServices cpsInfoServices)
        {
            _logger = logger;
            _cpsInfoServices = cpsInfoServices;
        }

        [HttpGet]
        [Route("find")]
        //[PermissionFilter(Permissions.BondMenuQLTP_LTP_DanhSach)]
        public APIResponse GetAllCpsInfo(int pageNumber, int? pageSize, string keyword, string status, string isCheck, DateTime? issueDate, DateTime? dueDate)
        {
            try
            {
                var tradingProvider = _cpsInfoServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status?.Trim(), isCheck?.Trim(), issueDate, dueDate);
                return new APIResponse(Utils.StatusCode.Success, tradingProvider, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find/{id}")]
        [HttpGet]
        //[PermissionFilter(Permissions.Bond_LTP_TTC_ChiTiet)]
        public APIResponse GetCpsInfo(int id)
        {
            try
            {
                var result = _cpsInfoServices.FindById(id);
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
        //[PermissionFilter(Permissions.BondMenuQLTP_LTP_TrinhDuyet)]
        public APIResponse AddRequest([FromBody] RequestStatusDto input)
        {
            try
            {
                _cpsInfoServices.Request(input);
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
        //[PermissionFilter(Permissions.BondQLPD_PDBTKH_PheDuyetOrHuy)]
        public APIResponse Approve(ApproveStatusDto input)
        {
            try
            {
                _cpsInfoServices.Approve(input);
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
        //[PermissionFilter(Permissions.BondMenuQLTP_LTP_EpicXacMinh)]
        public APIResponse Check([FromBody] CheckStatusDto input)
        {
            try
            {
                _cpsInfoServices.Check(input);
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
        //[PermissionFilter(Permissions.BondQLPD_PDBTKH_PheDuyetOrHuy)]
        public APIResponse Cancel([FromBody] CancelStatusDto input)
        {
            try
            {
                _cpsInfoServices.Cancel(input);
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
        //[PermissionFilter(Permissions.BondMenuQLTP_LTP_DongTraiPhieu)]
        public APIResponse CloseOpen(int id)
        {
            try
            {
                _cpsInfoServices.CloseOpen(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("add")]
        [HttpPost]
        //[PermissionFilter(Permissions.BondMenuQLTP_LTP_ThemMoi)]
        public APIResponse AddCpsInfo([FromBody] CreateCpsInfoDto body)
        {
            try
            {
                var result = _cpsInfoServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update/{id}")]
        [HttpPut]
        //[PermissionFilter(Permissions.Bond_LTP_TTC_CapNhat)]
        public APIResponse UpdateCpsdInfo([FromBody] UpdateCpsInfoDto body, int id)
        {
            try
            {
                var result = _cpsInfoServices.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        //[PermissionFilter(Permissions.BondMenuQLTP_LTP_Xoa)]
        public APIResponse DeleteCpsdInfo(int id)
        {
            try
            {
                var result = _cpsInfoServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("dividend/{id}")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondMenuQLTP_LTP_TTCT)]
        public APIResponse GetDividend(int id)
        {
            try
            {
                var result = _cpsInfoServices.FindDividendById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

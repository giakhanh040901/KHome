using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.ReceiveContractTemplate;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    [Route("api/invest/receive-contract-template")]
    public class ReceiveContractTemplateController : BaseController
    {
        private IReceiveContractTemplateServices _receiveContractTemplateServices;

        public ReceiveContractTemplateController(ILogger<ReceiveContractTemplateController> logger, IReceiveContractTemplateServices receiveContractTemplateServices)
        {
            _logger = logger;
            _receiveContractTemplateServices = receiveContractTemplateServices;
        }

        [HttpGet]
        [Route("find/{id}")]
        public APIResponse GetContractTemplate(int id)
        {
            try
            {
                var result = _receiveContractTemplateServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find-by-distribution")]
        [PermissionFilter(Permissions.InvestPPDT_MauGiaoNhanHD_DanhSach)]
        public APIResponse GetByDistributionId(int distributionId)
        {
            try
            {
                var result = _receiveContractTemplateServices.FindByDistributionId(distributionId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        [Route("add")]
        [PermissionFilter(Permissions.InvestPPDT_MauGiaoNhanHD_ThemMoi)]
        public APIResponse AddContractTemplate([FromBody] CreateReceiveContractTemplateDto body)
        {
            try
            {
                var result = _receiveContractTemplateServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestPPDT_MauGiaoNhanHD_CapNhat)]
        public APIResponse UpdateContractTemplate([FromBody] UpdateReceiveContractTemplateDto body)
        {
            try
            {
                var result = _receiveContractTemplateServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }

        }

        [Route("change-status")]
        [HttpPut]
        [PermissionFilter(Permissions.InvestPPDT_MauGiaoNhanHD_KichHoat)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _receiveContractTemplateServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }

        }

        [Route("delete")]
        [HttpPost]
        [PermissionFilter(Permissions.InvestPPDT_MauGiaoNhanHD_Xoa)]
        public APIResponse UpdateContractTemplate(int id)
        {
            try
            {
                var result = _receiveContractTemplateServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }

        }

        /// <summary>
        /// Tìm danh sách hợp đồng có phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("find-all-by-distribution")]
        [PermissionFilter(Permissions.InvestPPDT_MauGiaoNhanHD_DanhSach)]
        public APIResponse GetAllBlockadeLiberation(int? pageSize, int pageNumber, string keyword, int distributionId)
        {
            try
            {
                var result = _receiveContractTemplateServices.GetAll(pageSize ?? 100, pageNumber, keyword?.Trim(), distributionId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

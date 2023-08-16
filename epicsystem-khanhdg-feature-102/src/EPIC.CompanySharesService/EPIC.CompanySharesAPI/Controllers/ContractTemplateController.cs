using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.ContractTemplate;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InveCompanySharesAPIstAPI.Controllers
{
    [Authorize]
    //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN })]
    [ApiController]
    [Route("api/company-shares/contract-template")]
    public class ContractTemplateController : BaseController
    {
        private readonly IContractTemplateServices _contractTemplateServices;

        public ContractTemplateController(ILogger<ContractTemplateController> logger, IContractTemplateServices contractTemplateServices)
        {
            _logger = logger;
            _contractTemplateServices = contractTemplateServices;
        }

        [HttpGet]
        [Route("find")]
        //[PermissionFilter(Permissions.InvestPPDT_MauHopDong_DanhSach)]

        public APIResponse GetAllContractTemplates(int pageNumber, int? pageSize, string keyword, int secondaryId, string type)
        {
            try
            {
                var result = _contractTemplateServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), secondaryId, type);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find/{id}")]
        public APIResponse GetContractTemplate(int id)
        {
            try
            {
                var result = _contractTemplateServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find-by-order")]
        //[PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach)]

        public APIResponse GetAllContractTemplatesByOrder(int pageNumber, int? pageSize, string keyword, int orderId)
        {
            try
            {
                var result = _contractTemplateServices.FindAllByOrderCheckDisplayType(pageSize ?? 100, pageNumber, keyword?.Trim(), orderId, null);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        [Route("add")]
        //[PermissionFilter(Permissions.InvestPPDT_MauHopDong_ThemMoi)]

        public APIResponse AddContractTemplate([FromBody] CreateContractTemplateDto body)
        {
            try
            {
                var result = _contractTemplateServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update")]
        [HttpPut]
        //[PermissionFilter(Permissions.InvestPPDT_MauHopDong_CapNhat)]

        public APIResponse UpdateContractTemplate([FromBody] UpdateContractTemplateDto body)
        {
            try
            {
                var result = _contractTemplateServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        //[PermissionFilter(Permissions.InvestPPDT_MauHopDong_Xoa)]

        public APIResponse DeleteContractTemplate(int id)
        {
            try
            {
                var result = _contractTemplateServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status")]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _contractTemplateServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

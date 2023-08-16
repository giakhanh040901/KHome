using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.ContractTemplate;
using EPIC.Utils;
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
    [ApiController]
    [Route("api/bond/contract-template")]
    public class BondContractTemplateController : BaseController
    {
        private IBondContractTemplateService _contractTemplateServices;

        public BondContractTemplateController(ILogger<BondContractTemplateController> logger, IBondContractTemplateService ContractTemplateService)
        {
            _logger = logger;
            _contractTemplateServices = ContractTemplateService;
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
        [Route("find")]
        public APIResponse GetAllContractTemplates(int pageNumber, int? pageSize, string keyword, int bondSecondaryId, int? classify, string type)
        {
            try
            {
                var result = _contractTemplateServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), bondSecondaryId, classify, type);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find-by-order")]
        public APIResponse GetAllContractTemplatesByOrder(int pageNumber, int? pageSize, string keyword, int orderId)
        {
            try
            {
                var result = _contractTemplateServices.FindAllByOrder(pageSize ?? 100, pageNumber, keyword?.Trim(), orderId, null);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        [Route("add")]
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

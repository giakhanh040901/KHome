using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.ReceiveContractTemplate;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    [Route("api/bond/receive-contract-template")]
    public class BondReceiveContractTemplateController : BaseController
    {
        private IBondReceiveContractTemplateService _receiveContractTemplateServices;

        public BondReceiveContractTemplateController(ILogger<BondReceiveContractTemplateController> logger, IBondReceiveContractTemplateService receiveContractTemplateServices)
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
        [Route("find-by-secondary")]
        public APIResponse GetBySecondaryId(int bondSecondaryId)
        {
            try
            {
                var result = _receiveContractTemplateServices.FindBySecondaryId(bondSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        [Route("add")]
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
    }
}

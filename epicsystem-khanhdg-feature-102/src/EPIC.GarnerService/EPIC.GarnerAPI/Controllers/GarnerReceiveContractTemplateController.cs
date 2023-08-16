using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/receive-contract-template")]
    [ApiController]
    public class GarnerReceiveContractTemplateController : BaseController
    {
        private readonly IGarnerReceiveContractTemplateServices _garnerReceiveContractTemplateServices;

        public GarnerReceiveContractTemplateController(ILogger<IGarnerReceiveContractTemplateServices> logger,
            IGarnerReceiveContractTemplateServices garnerReceiveContractTemplateServices)
        {
            _logger = logger;
            _garnerReceiveContractTemplateServices = garnerReceiveContractTemplateServices;
        }

        [HttpGet("find-by-id")]
        public APIResponse GetById(int? id)
        {
            try
            {
                var result = _garnerReceiveContractTemplateServices.FindById(id ?? 0);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-by-distribution")]
        public APIResponse GetByDistributionId([FromQuery] FilterGarnerReceiveContractTemplateDto input)
        {
            try
            {
                var result = _garnerReceiveContractTemplateServices.FindByDistributionId(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update")]
        public APIResponse Update(UpdateGarnerReceiveContractTempDto input)
        {
            try
            {
                _garnerReceiveContractTemplateServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("add")]
        public APIResponse Add(CreateGarnerReceiveContractTempDto input)
        {
            try
            {
                _garnerReceiveContractTemplateServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete("delete")]
        [PermissionFilter(Permissions.GarnerPPDT_MauGiaoNhanHD_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                _garnerReceiveContractTemplateServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("change-status")]
        [PermissionFilter(Permissions.GarnerPPDT_MauGiaoNhanHD_KichHoat)]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                _garnerReceiveContractTemplateServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}

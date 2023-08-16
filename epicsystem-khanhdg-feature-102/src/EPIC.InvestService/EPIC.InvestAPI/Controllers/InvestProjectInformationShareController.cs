using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.Utils.Controllers;
using EPIC.Utils.Filter;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.ProjectInformationShare;
using EPIC.WebAPIBase.FIlters;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/project-information-share")]
    [ApiController]
    public class InvestProjectInformationShareController : BaseController
    {
        private readonly IInvestProjectInformationShareServices _invProjectInformationShareServices;

        public InvestProjectInformationShareController(ILogger<InvestProjectInformationShareController> logger,
            IInvestProjectInformationShareServices invProjectInformationShareServices)
        {
            _logger = logger;
            _invProjectInformationShareServices = invProjectInformationShareServices;
        }

        /// <summary>
        /// Thêm chia sẻ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse AddProjectInformationShare([FromBody] CreateInvProjectInformationShareDto input)
        {
            try
            {
                var result = _invProjectInformationShareServices.AddProjectInformationShare(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update")]
        public APIResponse UpdateProjectInformationShare([FromBody] UpdateInvProjectInformationShareDto input)
        {
            try
            {
                _invProjectInformationShareServices.UpdateProjectInformationShare(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("change-status/{id}")]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                _invProjectInformationShareServices.ChangStatus(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _invProjectInformationShareServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _invProjectInformationShareServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-all")]
        public APIResponse FindAll([FromQuery] FilterInvProjectInformationShareDto input)
        {
            try
            {
                var result = _invProjectInformationShareServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.PolicyFile;
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
    [Route("api/bond/policy-file")]
    [ApiController]
    public class BondPolicyFileController : BaseController
    {
        private IBondPolicyFileService _policyFileServices;

        public BondPolicyFileController(ILogger<BondPolicyFileController> logger, IBondPolicyFileService policyFileServices)
        {
            _logger = logger;
            _policyFileServices = policyFileServices;
        }

        [Route("add")]
        [HttpPost]
        public APIResponse AddPolicyFile([FromBody] CreatePolicyFileDto body)
        {
            try
            {
                var result = _policyFileServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        [Route("fileAll/find")]
        [HttpGet]
        public APIResponse GetAllPolicyFiles(int bondSecondaryId, int pageSize, int pageNumber, string keyword)
        {
            try
            {
                var result = _policyFileServices.FindAllPolicyFile(bondSecondaryId, pageSize, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find/{id}")]
        [HttpGet]
        public APIResponse GetPolicyFileById(int id)
        {
            try
            {
                var result = _policyFileServices.FindPolicyFileById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public APIResponse DeletePolicyFile(int id )
        {
            try
            {
                var result = _policyFileServices.DeletePolicyFile( id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update/{id}")]
        [HttpPut]
        public APIResponse PolicyFileUpdate(int id, [FromBody] UpdatePolicyFileDto body)
        {
            try
            {
                var result = _policyFileServices.PolicyFileUpdate(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

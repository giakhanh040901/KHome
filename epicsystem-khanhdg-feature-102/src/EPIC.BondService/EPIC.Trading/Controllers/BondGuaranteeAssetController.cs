using EPIC.BondDomain.Interfaces;
using EPIC.Utils;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EPIC.Utils.Controllers;
using System;
using EPIC.Entities.Dto.GuaranteeAsset;
using EPIC.Entities.Dto.GuaranteeFile;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/guarantee-asset")]
    [ApiController]
    public class BondGuaranteeAssetController : BaseController
    {
        private readonly IBondGuaranteeAssetService _guaranteeAssetService;
        public BondGuaranteeAssetController(ILogger<BondGuaranteeAssetController> logger, IBondGuaranteeAssetService guaranteeAssetService)
        {
            _logger = logger;
            _guaranteeAssetService = guaranteeAssetService;
        }

        [Route("find/{id}")]
        [HttpGet]
        public APIResponse GetguaranteeAsset(int id)
        {
            try
            {
                var result = _guaranteeAssetService.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost] 
        [Route("add")]
        [PermissionFilter(Permissions.Bond_LTP_TSDB_Them)]
        public APIResponse Add([FromBody] CreateGuaranteeAssetDto body)
        {
            try
            {
                _guaranteeAssetService.Add(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.Bond_LTP_TSDB_Sua)]
        public APIResponse UpdateguaranteeAsset([FromBody] UpdateGuaranteeAssetDto body)
        {
            try
            {
                _guaranteeAssetService.Update(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.Bond_LTP_TSDB_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _guaranteeAssetService.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("file/find/{id}")]
        [HttpGet]
        public APIResponse GetguaranteeFile(int id)
        {
            try
            {
                var result = _guaranteeAssetService.FindByIdFile(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("file/update/{id}")]
        [HttpPut]
        public APIResponse UpdateGuaranteeFile(int id, [FromBody] UpdateGuaranteeFileDto body)
        {
            try
            {
                var result = _guaranteeAssetService.UpdateFile(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("file/delete/{id}")]
        [HttpDelete]
        public APIResponse DeleteFile(int id)
        {
            try
            {
                var result = _guaranteeAssetService.DeleteFile(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("findAll")]
        [HttpGet]
        [PermissionFilter(Permissions.Bond_LTP_TSDB_DanhSach)]
        public APIResponse FindAll(int productBondId, int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _guaranteeAssetService.FindAll(productBondId, pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

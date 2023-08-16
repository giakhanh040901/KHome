using EPIC.BondDomain.Interfaces;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Issuer;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;      

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/issuer")]
    [ApiController]
    public class BondIssuerController : BaseController
    {
        private IBondIssuerService _issuerServices;
        public BondIssuerController(ILogger<BondIssuerController> logger, IBondIssuerService issuerService)
        {
            _logger = logger;
            _issuerServices = issuerService;
        }

        [HttpGet("find")]
        [PermissionFilter(Permissions.BondCaiDat_TCPH_DanhSach)]
        public APIResponse GetAllIssuers(int pageNumber, int? pageSize, bool? isNoPaging, string keyword, string status)
        {
            try
            {
                var result = _issuerServices.FindAll(pageSize ?? 100, pageNumber, isNoPaging ?? true, keyword?.Trim(), status?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("{id}")]
        [PermissionFilter(Permissions.Bond_TCPH_ThongTinChiTiet)]
        public APIResponse GetIssuer(int id)
        {
            try
            {
                var result = _issuerServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("add")]
        [PermissionFilter(Permissions.BondCaiDat_TCPH_ThemMoi)]
        public APIResponse AddIssuer([FromBody]CreateIssuerDto body)
        {
            try
            {
                var result = _issuerServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.Bond_TCPH_Xoa)]
        public APIResponse DeleteIssuer(int id)
        {
            try
            {
                var result = _issuerServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("update/{id}")]
        [PermissionFilter(Permissions.Bond_TCPH_CapNhat)]
        public APIResponse UpdateIssuer(int id, [FromBody] UpdateIssuerDto body)
        {
            try
            {
                var result = _issuerServices.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

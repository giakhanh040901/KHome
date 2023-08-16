using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.JuridicalFile;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
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
    [Route("api/bond/juridical-file")]
    [ApiController]
    public class BondJuridicalFileController : BaseController
    {
        private IBondJuridicalFileService _juridicalFileServices;

        public BondJuridicalFileController(ILogger<BondJuridicalFileController> logger, IBondJuridicalFileService juridicalFileServices)
        {
            _logger = logger;
            _juridicalFileServices = juridicalFileServices;
        }

        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.Bond_LTP_HSPL_ThemMoi)]
        public APIResponse AddJuridicalFile([FromBody] CreateJuridicalFileDto body)
        {
            try
            {
                var result = _juridicalFileServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);

            }
        }

        [Route("file/find")]
        [HttpGet]
        [PermissionFilter(Permissions.Bond_LTP_HSPL_DanhSach)]
        public APIResponse GetAllJuridicalFiles(int productBondId, int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _juridicalFileServices.FindAllJuridicalFile(productBondId, pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find/{id}")]
        [HttpGet]
        public APIResponse GetJuridicalFileById(int id)
        {
            try
            {
                var result = _juridicalFileServices.FindJuridicalFileById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.Bond_LTP_HSPL_Xoa)]
        public APIResponse DeleteJuridicalFile(int id)
        {
            try
            {
                var result = _juridicalFileServices.DeleteJuridicalFile(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update/{id}")]
        [HttpPut]
        public APIResponse JuridicalFileUpdate(int id, [FromBody] UpdateJuridicalFileDto body)
        {
            try
            {
                var result = _juridicalFileServices.JuridicalFileUpdate(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

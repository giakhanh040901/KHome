using EPIC.BondDomain.Interfaces;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.DepositProvider;
using EPIC.Utils;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    [Route("api/bond/deposit-provider")]
    public class BondDepositProviderController : BaseController
    {
        private IBondDepositProviderService _depositProviderServices;

        public BondDepositProviderController(ILogger<BondDepositProviderController> logger, IBondDepositProviderService depositProviderService)
        {
            _logger = logger;
            _depositProviderServices = depositProviderService;
        }

        [HttpGet]
        [Route("find/{id}")]
        [PermissionFilter(Permissions.Bond_DLLK_ThongTinChiTiet)]
        public APIResponse GetDepositProvider(int id)
        {
            try
            {
                var result = _depositProviderServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find")]
        [PermissionFilter(Permissions.BondCaiDat_DLLK_DanhSach)]
        public APIResponse GetAllDepositProviders(int pageNumber, int? pageSize, string keyword, string status)
        {
            try
            {
                var result = _depositProviderServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success!");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /*[HttpPut("active-user/{depositProviderId}")]
        public APIResponse ActiveUser([FromRoute] int depositProviderId, bool isActive)
        {
            try
            {
                var result = _depositProviderServices.ActiveDepositProvider(depositProviderId, isActive);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }*/

        [HttpPost]
        [Route("add")]
        [PermissionFilter(Permissions.BondCaiDat_DLLK_ThemMoi)]
        public APIResponse AddDepositProvider([FromBody]CreateDepositProviderDto body)
        {
            try
            {
                var result = _depositProviderServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [PermissionFilter(Permissions.Bond_DLLK_Xoa)]
        public APIResponse DeleteDepositProvider(int id)
        {
            try
            {
                var result = _depositProviderServices.Delete(id);
                    return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}

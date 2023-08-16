using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityDomain.Implements;

namespace EPIC.Shared.Filter
{
    /// <summary>
    /// Một trong các quyền truyền vào mà tài khoản đang đăng nhập có là được, nếu không có thì trả về 401
    /// </summary>
    public class PermissionFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissions;
        private IHttpContextAccessor _httpContextAccessor;
        private IPermissionServices _permissionServices;

        public PermissionFilter(params string[] permissions)
        {
            _permissions = permissions;
        }

        private void GetServices(AuthorizationFilterContext context)
        {
            _httpContextAccessor = context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            _permissionServices = context.HttpContext.RequestServices.GetService(typeof(IPermissionServices)) as IPermissionServices;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            GetServices(context);
            //var permissions = _permissionServices.GetPermission();
            //bool isGrant = false;
            //foreach (var permission in _permissions)
            //{
            //    if (permissions.Contains(permission))
            //    {
            //        isGrant = true;
            //    }
            //}

            //if (!isGrant)
            //{
            //    context.Result = new UnauthorizedObjectResult(new { message = "Tài khoản không có quyền" });
            //}
        }
    }
}

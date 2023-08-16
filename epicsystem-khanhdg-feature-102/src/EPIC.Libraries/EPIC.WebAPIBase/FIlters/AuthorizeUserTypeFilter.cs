using EPIC.DataAccess.Base;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace EPIC.WebAPIBase.FIlters
{
    public class AuthorizeUserTypeFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] _userTypes;

        public AuthorizeUserTypeFilter(params string[] userTypes)
        {
            _userTypes = userTypes;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                 .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (hasAllowAnonymous)
                return;

            IHttpContextAccessor _httpContextAccessor = context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            EpicSchemaDbContext dbContext = context.HttpContext.RequestServices.GetService(typeof(EpicSchemaDbContext)) as EpicSchemaDbContext;
            string userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            if (!_userTypes.Contains(userType))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Loại tài khoản không có quyền truy cập" });
            }
        }
    }

    /// <summary>
    /// Filter những user là quản trị
    /// </summary>
    public class AuthorizeAdminUserTypeFilter : AuthorizeUserTypeFilter
    {
        public AuthorizeAdminUserTypeFilter() : base(UserTypes.ADMIN_TYPES)
        {
        }
    }
}

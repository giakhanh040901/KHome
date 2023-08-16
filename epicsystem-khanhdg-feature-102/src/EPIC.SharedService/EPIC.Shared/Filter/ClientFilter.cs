using EPIC.IdentityDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Shared.Filter
{
    public class ClientFilter : Attribute, IAuthorizationFilter
    {
        private IHttpContextAccessor _httpContextAccessor;
        private string _clientId;

        public ClientFilter(string clientId)
        {
            _clientId = clientId;
        }

        private void GetServices(AuthorizationFilterContext context)
        {
            _httpContextAccessor = context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            GetServices(context);
            string currentClientId = null;
            try
            {
                currentClientId = CommonUtils.GetCurrentClientId(_httpContextAccessor);
            }
            catch (Exception e)
            {
                context.Result = new UnauthorizedObjectResult(new { message = e.Message });
                return;
            }
            if (currentClientId != ClientIdentityServer.ClientSharedDataCustomer)
            {
                context.Result = new UnauthorizedObjectResult(new { message = $"ClientId = {currentClientId} không có quyền" });
            }
        }
    }
}

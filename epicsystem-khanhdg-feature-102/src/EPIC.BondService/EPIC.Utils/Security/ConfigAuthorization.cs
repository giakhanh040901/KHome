using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Security
{
    /// <summary>
    /// Cấu hình cho middleware Authorization về các policy,...
    /// </summary>
    public static class ConfigAuthorization
    {
        /// <summary>
        /// Các policy cho phân scope api
        /// </summary>
        /// <param name="options"></param>
        public static void AddScopeAPIPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy(AuthPolicies.LogicBusinessPolicy, builder =>
            {
                builder.RequireScope(ApiScopes.LogicBusiness); //nếu không có scope này sẽ trả về 403
            });

            options.AddPolicy(AuthPolicies.SharedDataCustomerPolicy, builder =>
            {
                builder.RequireScope(ApiScopes.SharedDataCustomer); //nếu không có scope này sẽ trả về 403
            });

            options.AddPolicy(AuthPolicies.SharedDataBoCongThuongPolicy, builder =>
            {
                builder.RequireScope(ApiScopes.SharedDataBoCongThuong); //nếu không có scope này sẽ trả về 403
            });
        }
    }
}

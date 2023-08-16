using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    /// <summary>
    /// Các policy cho authorization
    /// </summary>
    public static class AuthPolicies
    {
        public const string LogicBusinessPolicy = "Policy" + ApiScopes.LogicBusiness;
        public const string SharedDataCustomerPolicy = "Policy" + ApiScopes.SharedDataCustomer;
        public const string SharedDataBoCongThuongPolicy = "Policy" + ApiScopes.SharedDataBoCongThuong;
    }
}

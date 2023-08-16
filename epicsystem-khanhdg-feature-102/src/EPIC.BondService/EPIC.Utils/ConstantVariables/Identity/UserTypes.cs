using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Identity
{
    public class UserTypes
    {
        public const string SUPER_ADMIN = "S";
        public const string ROOT_EPIC = "RE";
        public const string EPIC = "E";
        public const string ROOT_PARTNER = "RP";
        public const string PARTNER = "P";
        public const string ROOT_TRADING_PROVIDER = "RT";
        public const string TRADING_PROVIDER = "T";
        public const string INVESTOR = "I";

        public static readonly string[] ADMIN_TYPES = new string[] 
        { 
            SUPER_ADMIN, ROOT_EPIC, EPIC, 
            ROOT_PARTNER, PARTNER,
            ROOT_TRADING_PROVIDER, TRADING_PROVIDER 
        };

        public static readonly string[] ROOT_ADMIN_TYPES = new string[]
        {
            SUPER_ADMIN, ROOT_EPIC, EPIC
        };

        public static readonly string[] TRADING_PROVIDER_TYPES = new string[]
        {
            ROOT_TRADING_PROVIDER, TRADING_PROVIDER
        };

        public static readonly string[] PARTNER_TYPES = new string[]
        {
            ROOT_PARTNER, PARTNER
        };
    }
}

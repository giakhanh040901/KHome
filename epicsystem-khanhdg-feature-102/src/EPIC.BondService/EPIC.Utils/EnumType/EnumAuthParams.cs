using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.EnumType
{
    public enum EnumAuthParams
    {
        [Description("UserName")]
        [EnumMember(Value = "SESSIONINFO_USERNAME")]
        UserName,
        [Description("UserId")]
        [EnumMember(Value = "SESSIONINFO_USERID")]
        UserId,
        [Description("UserType")]
        [EnumMember(Value = "SESSIONINFO_USERTYPE")]
        UserType,
        [Description("TradingProviderId")]
        [EnumMember(Value = "SESSIONINFO_TPID")]
        TradingProviderId,
        [Description("InvestorId")]
        [EnumMember(Value = "SESSIONINFO_INVESTORID")]
        InvestorId,
        [Description("PartnerId")]
        [EnumMember(Value = "SESSIONINFO_PARTNERID")]
        PartnerId
    }
}

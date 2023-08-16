using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.EnumType
{
    public enum EnumValidateType
    {
        [Description("Required")]
        [EnumMember(Value = "REQUIRED")]
        REQUIRED = 0,

        [Description("CallProcedure")]
        [EnumMember(Value = "PROCEDURE")]
        PROCEDURE = 1,

        [Description("IsEmail")]
        [EnumMember(Value = "EMAIL")]
        EMAIL = 2,

        [Description("ByRegx")]
        [EnumMember(Value = "REGX")]
        REGX = 3,

        [Description("ByCURL")]
        [EnumMember(Value = "CURL")]
        CURL = 4,
    }
}

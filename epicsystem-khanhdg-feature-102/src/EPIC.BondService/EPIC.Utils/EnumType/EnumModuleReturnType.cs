using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.EnumType
{
    public enum EnumModuleReturnType
    {
        [Description("Not Delete")]
        [EnumMember(Value = "L")]
        List,
        [Description("Not Delete")]
        [EnumMember(Value = "O")]
        Object
    }
}

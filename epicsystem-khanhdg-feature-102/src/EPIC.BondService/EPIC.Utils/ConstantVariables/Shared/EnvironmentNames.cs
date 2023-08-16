using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    public class EnvironmentNames
    {
        public const string Production = "Production";
        public const string Development = "Development";
        public const string DevTest = "DevTest";
        public const string DevelopmentWSL = "DevelopmentWSL";
        public const string Staging = "Staging";

        public static readonly string[] DevelopEnv = new string[]
        {
            Development,
            DevelopmentWSL,
            DevTest,
            Staging,
        };

        public static readonly string[] Develops = new string[]
        {
            Development,
            DevelopmentWSL,
            Staging, //thêm tạm
            DevTest
        };
    }
}

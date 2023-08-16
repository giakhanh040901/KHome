using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.EnumType
{
    /// <summary>
    /// Trạng thái của background job
    /// </summary>
    public static class EnumBackgroundJobState
    {
        public const string Processing = "Processing";
        public const string Deleted = "Deleted";
        public const string Awaiting = "Awaiting";
        public const string Failed = "Failed";
        public const string Succeeded = "Succeeded";  
    }
}

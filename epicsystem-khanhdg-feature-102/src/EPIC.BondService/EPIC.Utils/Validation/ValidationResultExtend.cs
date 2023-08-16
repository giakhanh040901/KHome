using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Validation
{
    public class ValidationResultExtend : ValidationResult
    {
        public int ErrorCode;

        public ValidationResultExtend(string errorMessage) : base(errorMessage)
        {
        }

        public ValidationResultExtend(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
        {
        }

        protected ValidationResultExtend(ValidationResult validationResult) : base(validationResult)
        {
        }

        /// <summary>
        /// Thêm error code
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public ValidationResultExtend(int errorCode, string errorMessage) : base(errorMessage)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Thêm error code
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        /// <param name="memberNames"></param>
        public ValidationResultExtend(int errorCode, string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
        {
            ErrorCode = errorCode;
        }
    }
}

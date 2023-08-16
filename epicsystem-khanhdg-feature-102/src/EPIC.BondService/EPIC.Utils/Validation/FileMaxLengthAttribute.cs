using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Validation
{
    public class FileMaxLengthAttribute : ValidationAttribute
    {
        public long MaxLength { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not IFormFile file)
            {
                throw new InvalidCastException("Field is not instance of IFormFile");
            }
            
            if (file.Length <= MaxLength)
            {
                return ValidationResult.Success;
            }

            var msg = $"File có size phải nhỏ hơn hoặc bằng {MaxLength/1024.0/1024.0} MB";
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                msg = ErrorMessage;
            }
            return new ValidationResult(msg);
        }
    }
}

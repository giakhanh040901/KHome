using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Validation
{
    public class FileExtentionAttribute : ValidationAttribute
    {
        public string[] AllowableExtentions { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is not IFormFile file)
            {
                throw new InvalidCastException("Field is not instance of IFormFile");
            }

            if (AllowableExtentions?.Contains(Path.GetExtension(file.FileName).ToLower()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"File chỉ được phép thuộc các định dạng sau: {string.Join(", ", AllowableExtentions ?? new string[] { "Không có định dạng nào được phép" })}.";
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                msg = ErrorMessage;
            }
            return new ValidationResult(msg);
        }
    }
}

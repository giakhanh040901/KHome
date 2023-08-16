using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Validation
{
    public class ListFormFileAttribute : ValidationAttribute
    {
        public string[] AllowableExtentions { get; set; }
        public long MaxLength { get; set; }

        public int MaxFileCount { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var listFormFile = new List<IFormFile>();

            try
            {
                listFormFile = value as List<IFormFile>;
            }
            catch (Exception)
            {
                return new ValidationResult("Field is not instance of IFormFile");
            }

            if (listFormFile == null)
            {
                return new ValidationResult(this.ErrorMessage ?? "Không được để trống");
            }

            if (MaxFileCount > 0 && listFormFile.Count > MaxFileCount)
            {
                var msg = $"Mỗi lần chỉ được upload tối đa là {MaxFileCount} file";
                return new ValidationResult(msg);
            }

            foreach (var formFile in listFormFile)
            {
                if (AllowableExtentions != null && !(AllowableExtentions?.Contains(Path.GetExtension(formFile.FileName).ToLower()) ?? false))
                {
                    var msg = $"File {formFile.FileName} chỉ được phép thuộc các định dạng sau: {string.Join(", ", AllowableExtentions ?? new string[] { "Không có định dạng nào được phép" })}.";
                    return new ValidationResult(msg);
                }

                if (MaxLength > 0 && formFile.Length > MaxLength)
                {
                    var msg = $"File {formFile.FileName} có size phải nhỏ hơn hoặc bằng {MaxLength / 1024.0 / 1024.0} MB";
                    return new ValidationResult(msg);
                }

            }

            return ValidationResult.Success;
        }
    }
}

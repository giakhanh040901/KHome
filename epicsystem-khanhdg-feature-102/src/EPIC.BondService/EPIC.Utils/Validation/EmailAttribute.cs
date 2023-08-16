using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Validation
{
    public class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string msg = "Email không hợp lệ";
            string vali = "Email không được bỏ trống";

            if (value == null)
            {
                return new ValidationResult(ErrorMessage ?? vali);
            }

            string email = value?.ToString();
            var trimmedEmail = email?.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return new ValidationResult(ErrorMessage ?? msg);
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address == trimmedEmail)
                {
                    return ValidationResult.Success;
                }
            }
            catch
            {
                return new ValidationResult(ErrorMessage ?? msg);
            }
            return new ValidationResult(ErrorMessage ?? msg);
        }

    }
}

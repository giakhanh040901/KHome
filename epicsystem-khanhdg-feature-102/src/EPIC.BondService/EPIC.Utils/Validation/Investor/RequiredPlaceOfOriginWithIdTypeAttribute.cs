using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.Validation.Investor
{
    public class RequiredPlaceOfOriginWithIdTypeAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var typeOfInstance = validationContext.ObjectInstance.GetType();
            var idType = typeOfInstance.GetProperty("IdType").GetValue(validationContext.ObjectInstance)?.ToString();

            if (idType?.ToLower() != IDTypes.PASSPORT.ToLower())
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = $"{validationContext.DisplayName} is required";
                }

                if (string.IsNullOrEmpty(value?.ToString().Trim()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;

        }
    }
}

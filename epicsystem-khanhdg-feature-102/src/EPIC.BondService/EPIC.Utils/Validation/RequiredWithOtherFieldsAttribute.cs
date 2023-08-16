using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EPIC.Utils.Validation
{
    /// <summary>
    /// Kiểm tra ràng buộc nếu các trường khác (OtherFields) đều null thì trường đang xét phải có dữ liệu
    /// </summary>
    public class RequiredWithOtherFieldsAttribute : RequiredAttribute
    {
        public string[] OtherFields { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = $"{validationContext.DisplayName} is required";
            }

            if (OtherFields == null || OtherFields.Count() == 0 || value != null)
            {
                return ValidationResult.Success;
            }    

            var typeOfInstance = validationContext.ObjectInstance.GetType();
            foreach (var field in OtherFields)
            {
                object otherValue = typeOfInstance.GetProperty(field).GetValue(validationContext.ObjectInstance);
                if (otherValue != null)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }

    /// <summary>
    /// Kiểm tra ràng buộc nếu các trường khác (OtherFields) có giá trị thì trường đang xét phải có dữ liệu
    /// </summary>
    public class RequiredWithFieldsAttribute : RequiredAttribute
    {
        public string[] OtherFields { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = $"{validationContext.DisplayName} is required";
            }

            if (OtherFields == null || OtherFields.Count() == 0)
            {
                return ValidationResult.Success;
            }

            var typeOfInstance = validationContext.ObjectInstance.GetType();
            foreach (var field in OtherFields)
            {
                object otherValue = typeOfInstance.GetProperty(field).GetValue(validationContext.ObjectInstance);
                if (otherValue != null && value != null || otherValue == null)
                {
                    return ValidationResult.Success;
                }
                else if (otherValue != null && value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }    
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}

using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class EKYCConfirmInfoDto
    {
        private string _phone;

        public bool IsConfirmed { get; set; }
        public List<string> IncorrectFields { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        [StringRange(AllowableValues = new string[] { null, Genders.FEMALE, Genders.MALE })]
        public string Sex { get; set; }
    }
}

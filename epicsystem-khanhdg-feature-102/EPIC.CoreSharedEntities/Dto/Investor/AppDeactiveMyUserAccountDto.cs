using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class AppDeactiveMyUserAccountDto
    {
        private string _pinCode; 

        [Required(AllowEmptyStrings = false)]
        public string PinCode { get => _pinCode; set => _pinCode = value?.Trim(); }
    }
}

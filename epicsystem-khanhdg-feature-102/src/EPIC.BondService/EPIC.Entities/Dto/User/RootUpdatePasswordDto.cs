using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class UpdatePasswordUserDto
    {
        public string NewPassword { get; set; }
    }
    public class RootUpdatePasswordDto : UpdatePasswordUserDto
    {
        public int UserId { get; set; }
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Chỉ được nhập trong các giá trị Y / N")]
        public string IsTempPassword { get; set; }
    }
}

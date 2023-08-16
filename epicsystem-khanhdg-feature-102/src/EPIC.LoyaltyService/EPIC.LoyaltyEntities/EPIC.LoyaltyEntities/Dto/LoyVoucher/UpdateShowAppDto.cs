using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucher
{
    public class UpdateShowAppDto
    {
        [Required(ErrorMessage = "ID voucher không được bỏ trống")]
        public int Id { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsShowApp { get; set; }
    }
}

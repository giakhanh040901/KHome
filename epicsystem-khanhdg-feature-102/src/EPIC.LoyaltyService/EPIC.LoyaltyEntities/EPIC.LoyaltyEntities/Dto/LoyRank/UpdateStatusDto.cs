using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyRank
{
    public class UpdateStatusDto
    {
        [Required(ErrorMessage = "Id không được bỏ trống")]
        public int Id { get; set; }

        [IntegerRange(AllowableValues = new int[] {LoyRankStatus.ACTIVE, LoyRankStatus.KHOI_TAO, LoyRankStatus.DEACTIVE})]
        public int Status { get; set; }
    }
}

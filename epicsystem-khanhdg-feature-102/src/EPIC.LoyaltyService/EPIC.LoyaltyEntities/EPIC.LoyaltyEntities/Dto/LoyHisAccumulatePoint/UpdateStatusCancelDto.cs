using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class UpdateStatusCancelDto
    {

        [Required(ErrorMessage = "Id không được bỏ trống")]
        public int Id { get; set; }

    }
}

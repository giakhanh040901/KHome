using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondPolicy
{
    public class UpdateProductPolicyDto : CreateProductPolicyDto
    {
        public int Id { get; set; }
        [StringRange(AllowableValues = new string[] { Utils.Status.ChoDuyet, Utils.Status.DaDuyet, Utils.Status.TuChoiDuyet }, ErrorMessage = "Vui lòng chọn Trạng thái")]
        public string Status { get; set; }
    }
}

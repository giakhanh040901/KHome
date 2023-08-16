using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicy
{
    public class UpdatePolicyDto : CreatePolicyDto
    {
        /// <summary>
        /// id chính sách nếu là 0 thì sẽ là thêm mới nếu khác 0 thì sẽ là cập nhật
        /// </summary>
        public int Id { get; set; }
    }
}

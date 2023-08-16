using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.XptToken
{
    public class CreateXptTokenDto
    {
        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Loại dữ liệu thao tác
        /// </summary>
        public List<string> DataTypes { get; set; }
    }
}

using DocumentFormat.OpenXml.Drawing.Diagrams;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class RstProductItemLockingDto : RstUpdateStatusLockDtoBase
    {
        /// <summary>
        /// Id sản phẩm dự án
        /// </summary>
        public int Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemExtend
{
    public class RstProductItemExtendDto
    {
        public int Id { get; set; }

        public int ProductItemId { get; set; }

        /// <summary>
        /// Tiêu đề thông tin
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tên icon
        /// </summary>
        public string IconName { get; set; }

        /// <summary> 
        /// Mô tả nội dung thông tin
        /// </summary>
        public string Description { get; set; }
    }
}

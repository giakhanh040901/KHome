using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class UpdateRstProductItemDesignDiagramDto
    {
        /// <summary>
        /// Id sản phẩm bán
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Loại nội dung sơ đồ thiết kế cho căn hộ : MARKDOWN, HTML
        /// </summary>
        [StringRange(AllowableValues = new string[] { ContentTypes.MARKDOWN, ContentTypes.HTML }, ErrorMessage = "Vui lòng chọn loại Type : MARKDOWN , HTML")]
        public string DesignDiagramContentType { get; set; }

        /// <summary>
        /// Nội dung sơ đồ thiết kế cho căn hộ 
        /// </summary>
        public string DesignDiagramContent { get; set; }
    }
}

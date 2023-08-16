using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class UpdateRstProductItemMaterialDto
    {
        /// <summary>
        /// Id sản phẩm bán
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Loại nội dung vật liệu thi công cho căn hộ : MARKDOWN, HTML
        /// </summary>
        [StringRange(AllowableValues = new string[] { ContentTypes.MARKDOWN, ContentTypes.HTML }, ErrorMessage = "Vui lòng chọn loại Type : MARKDOWN , HTML")]
        public string MaterialContentType { get; set; }

        /// <summary>
        /// Nội dung vật liệu thi công cho căn hộ
        /// </summary>
        public string MaterialContent { get; set; }
    }
}

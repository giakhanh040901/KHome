using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOwner
{
    public class UpdateRstOwnerDescriptionDto
    {
        /// <summary>
        /// Id chủ đầu tư
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Loại nội dung miêu tả chủ đầu tư : MARKDOWN, HTML
        /// </summary>
        [StringRange(AllowableValues = new string[] { ContentTypes.MARKDOWN, ContentTypes.HTML }, ErrorMessage = "Vui lòng chọn loại Type : MARKDOWN , HTML")]
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung miêu tả chủ đầu tư
        /// </summary>
        public string DescriptionContent { get; set; }
    }
}

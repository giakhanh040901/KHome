using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class UpdateEventOverviewContentDto
    {
        /// <summary>
        /// Id của sự kiện
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Loại nội dung tổng quan: MARKDOWN, HTML
        /// </summary>
        [StringRange(AllowableValues = new string[] { ContentTypes.MARKDOWN, ContentTypes.HTML }, ErrorMessage = "Vui lòng chọn loại Type : MARKDOWN , HTML")]
        public string ContentType { get; set; }

        /// <summary>
        /// Nội dung tổng quan
        /// </summary>
        public string OverviewContent { get; set; }
    }
}

using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class RstUpdateProjectOverviewContentDto
    {
        /// <summary>
        /// Id của dự án
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

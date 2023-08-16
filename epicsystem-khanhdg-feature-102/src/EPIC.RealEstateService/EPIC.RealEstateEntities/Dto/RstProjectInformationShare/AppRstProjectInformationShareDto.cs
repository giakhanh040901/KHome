using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectInformationShare
{
    public class AppRstProjectInformationShareDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }

        /// <summary> 
        /// trạng thái A:Active, D:Deactive
        /// </summary>
        public string Status { get; set; }

        #region Mô tả thông tin dự án
        /// <summary>
        /// Loại nội dung tổng quan: MARKDOWN, HTML
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Nội dung tổng quan
        /// </summary>
        [ColumnSnackCase(nameof(OverviewContent), TypeName = "CLOB")]
        public string OverviewContent { get; set; }
        #endregion

        public List<RstProjectInformationShareDetailDto> DocumentFiles { get; set; }
        public List<RstProjectInformationShareDetailDto> ImageFiles { get; set; }
    }
}

using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class UpdateGarnerProductOverviewDto
    {
        public int Id { get; set; }

        private string _contentType;
        /// <summary>
        /// Loại tổng quan
        /// </summary>
        [StringRange(AllowableValues = new string[] { ContentTypes.MARKDOWN, ContentTypes.HTML }, ErrorMessage = "Vui lòng chọn loại Type : MARKDOWN , HTML")]
        public string ContentType
        {
            get => _contentType;
            set => _contentType = value?.Trim();
        }

        private string _overviewImageUrl;
        /// <summary>
        /// Ảnh tổng quan của dự án
        /// </summary>
        public string OverviewImageUrl
        {
            get => _overviewImageUrl;
            set => _overviewImageUrl = value?.Trim();
        }

        private string _overviewContent;
        /// <summary>
        /// Nội dung tổng quan
        /// </summary>
        public string OverviewContent
        {
            get => _overviewContent;
            set => _overviewContent = value?.Trim();
        }

        /// <summary>
        /// Thêm các file tổng quan
        /// </summary>
        public List<CreateGarnerProductOverviewFileDto> ProductOverviewFiles { get; set; }

        /// <summary>
        /// Thêm các tổ chức liên quan
        /// </summary>
        public List<CreateGarnerProductOverviewOrgDto> ProductOverviewOrgs { get; set; }
    }
}

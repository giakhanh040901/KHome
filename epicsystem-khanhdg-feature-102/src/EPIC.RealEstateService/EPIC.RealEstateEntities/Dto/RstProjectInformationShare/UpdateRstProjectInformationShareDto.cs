using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectInformationShare
{
    public class UpdateRstProjectInformationShareDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }

        #region Mô tả thông tin dự án
        private string _contentType;
        /// <summary>
        /// Loại nội dung tổng quan: MARKDOWN, HTML
        /// </summary>
        public string ContentType
        {
            get => _contentType;
            set => _contentType = value?.Trim();
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
        #endregion
        /// <summary>
        /// Danh sách file tài liệu
        /// </summary>
        public List<UpdateRstProjectInfoShareDetailDto> DocumentFiles { get; set; }

        /// <summary>
        /// Danh sách file hình ảnh
        /// </summary>
        public List<UpdateRstProjectInfoShareDetailDto> ImageFiles { get; set; }
    }
}

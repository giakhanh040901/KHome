using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMedia
{
    public class CreateEvtEventMediaDto
    {
        public int EventId { get; set; }

        private string _groupTitle;
        /// <summary>
        /// Tên nhóm hình ảnh
        /// </summary>
        [StringLength(256, ErrorMessage = "Tên nhóm không được dài hơn 256 ký tự")]
        public string GroupTitle
        {
            get => _groupTitle;
            set => _groupTitle = value?.Trim();
        }

        private string _urlImage;
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        [StringLength(512, ErrorMessage = "Đường dẫn file ảnh không được dài hơn 512 ký tự")]
        public string UrlImage
        {
            get => _urlImage;
            set => _urlImage = value?.Trim();
        }

        /// <summary>
        /// Đường dẫn đến file
        /// </summary>
        private string _urlPath;
        [StringLength(512, ErrorMessage = "Đường dẫn điều hướng không được dài hơn 512 ký tự")]
        public string UrlPath
        {
            get => _urlPath;
            set => _urlPath = value?.Trim();
        }
    }
}

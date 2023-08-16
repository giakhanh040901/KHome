using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemMedia
{
    public class UpdateRstProductItemMediaDto
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }

        private string _groupTitle;
        [StringLength(256, ErrorMessage = "Tên nhóm không được dài hơn 256 ký tự")]
        public string GroupTitle
        {
            get => _groupTitle;
            set => _groupTitle = value?.Trim();
        }

        private string _urlImage;
        //[Required(ErrorMessage = "Đường dẫn file ảnh không được để trống")]
        [StringLength(512, ErrorMessage = "Đường dẫn file ảnh không được dài hơn 512 ký tự")]
        public string UrlImage
        {
            get => _urlImage;
            set => _urlImage = value?.Trim();
        }

        private string _urlPath;
        [StringLength(512, ErrorMessage = "Đường dẫn điều hướng không được dài hơn 512 ký tự")]
        public string UrlPath
        {
            get => _urlPath;
            set => _urlPath = value?.Trim();
        }
    }
}

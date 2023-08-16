using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.GuaranteeFile
{
    public class CreateGuaranteeFileDto
    {
        public int GuaranteeAssetId { get; set; }

        private string _title;
        [Required(ErrorMessage = "Tiêu đề file không được bỏ trống")]
        public string Title
        {
            get => _title;
            set => _title = value?.Trim();
        }

        private string _fileUrl;
        [Required(ErrorMessage = "Đường dẫn file không được bỏ trống")]
        public string FileUrl
        {
            get => _fileUrl;
            set => _fileUrl = value?.Trim();
        }
    }
}

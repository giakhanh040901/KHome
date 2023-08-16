using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.DistributionFile
{
    public class CreateDistributionFileDto
    {

        [Required(ErrorMessage = "Mã phân phối đầu tư không được bỏ trống")]
        public int? DistributionId { get; set; }

        private string _title;
        [StringLength(500, ErrorMessage = "Tiêu đề file không được dài hơn 200 ký tự")]
        public string Title
        {
            get => _title;
            set => _title = value?.Trim();
        }
        [Required(ErrorMessage = "Đầu vào file không được bỏ trống")]
        public string FileUrl { get; set; }
    }
}

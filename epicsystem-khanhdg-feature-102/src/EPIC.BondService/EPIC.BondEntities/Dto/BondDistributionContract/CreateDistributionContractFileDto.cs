using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DistributionContract
{
    public class CreateDistributionContractFileDto
    {
        [Required(ErrorMessage = "Hợp đồng phân phối không được bỏ trống")]
        public int DistributionContractId { get; set; }
        private string _title;
        [StringLength(512, ErrorMessage = "Tên file không được dài hơn 512 ký tự")]
        public string Title
        {
            get => _title;
            set => _title = value?.Trim();
        }
        public string FileUrl { get; set; }
    }
}

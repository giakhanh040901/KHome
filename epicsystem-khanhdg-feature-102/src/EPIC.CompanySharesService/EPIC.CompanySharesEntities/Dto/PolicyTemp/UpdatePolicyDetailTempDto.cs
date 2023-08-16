using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.PolicyTemp
{
    public class UpdatePolicyDetailTempDto
    {
        public int PolicyDetailTempId { get; set; }
        [Required(ErrorMessage = "Số thứ tự không được bỏ trống")]
        public int? Stt { get; set; }

        private string _shortName;
        [StringLength(50, ErrorMessage = "Tên viết tắt kỳ hạn không được dài qua 50 ký tự")]
        [Required(ErrorMessage = "Tên viết tắt kỳ hạn không được bỏ trống")]
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value?.Trim();
        }

        private string _name;
        [StringLength(256, ErrorMessage = "Tên kỳ hạn không được dài qua 256 ký tự")]
        [Required(ErrorMessage = "Tên  kỳ hạn không được bỏ trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
        public string InterestPeriodType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }
        public int? InterestType { get; set; }
    }
}

using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    public class CreateCpsPolicyDetailDto
    {
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Số thứ tự không được bỏ trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số kỳ trả phải lớn hơn 0")]
        public int? STT { get; set; }

        private string _name;
        [Required(ErrorMessage = "Tên kỳ hạn không được để trống")]
        [StringLength(256, ErrorMessage = "Tên kỳ hạn không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _shortName;
        [Required(ErrorMessage = "Tên viết tắt không được để trống")]
        [StringLength(256, ErrorMessage = "Tên viết tắt không được dài hơn 256 ký tự")]
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value?.Trim();
        }

        [Required(ErrorMessage = "Đơn vị đáo hạn không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { Utils.PeriodUnit.DAY,
            Utils.PeriodUnit.MONTH, Utils.PeriodUnit.YEAR },
        ErrorMessage = "Đơn vị đáo hạn không hợp lệ")]
        public string PeriodType { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Số kỳ đáo hạn phải lớn hơn 0")]
        [Required(ErrorMessage = "Số kỳ đầu tư không được bỏ trống")]
        public int? PeriodQuantity { get; set; }

        [Required(ErrorMessage = "Kiểu trả lãi không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { Utils.InterestTypes.DINH_KY, Utils.InterestTypes.CUOI_KY })]
        public int? InterestType { get; set; }

        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Số kỳ trả phải lớn hơn 0")]
        public int? InterestPeriodQuantity { get; set; }

        [StringRange(AllowableValues = new string[] { Utils.PeriodUnit.DAY,
            Utils.PeriodUnit.MONTH, Utils.PeriodUnit.YEAR },
            ErrorMessage = "Loại kỳ trả lãi không hợp lệ")]
        public string InterestPeriodType { get; set; }

        [Required(ErrorMessage = "Lợi nhuận không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Lợi nhuận phải lớn hơn 0")]
        public decimal? Profit { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Số ngày phải lớn hơn 0")]
        public int? InterestDays { get; set; }
    }
}

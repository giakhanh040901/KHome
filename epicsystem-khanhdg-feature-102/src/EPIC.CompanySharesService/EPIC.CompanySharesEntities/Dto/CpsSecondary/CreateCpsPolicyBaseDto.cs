using EPIC.Utils;
using EPIC.Utils.ConstantVariables.CompanyShares;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    public class CreateCpsPolicyBaseDto
    {
        public int SecondaryId { get; set; }

        private string _code;
        [Required(ErrorMessage = "Mã chính sách không được để trống")]
        [StringLength(100, ErrorMessage = "Mã chính sách không được dài hơn 100 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên chính sách không được để trống")]
        [StringLength(256, ErrorMessage = "Tên chính sách không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        [Required(ErrorMessage = "Kiểu chính sách sản phẩm không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { CpsPolicyType.FIX, CpsPolicyType.FLEXIBLE })]
        public int? Type { get; set; }

        [Required(ErrorMessage = "Loại nhà đầu tư không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { Utils.InvestorType.PROFESSIONAL, Utils.InvestorType.ALL })]
        public string InvestorType { get; set; }

        [Required(ErrorMessage = "Thuế lợi nhuận không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Thuế lợi nhuận phải lớn hơn 0")]
        public decimal? IncomeTax { get; set; }

        [Required(ErrorMessage = "Thuế chuyển nhượng không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Thuế chuyển nhượng phải lớn hơn 0")]
        public decimal? TransferTax { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Tiền đầu tư tối thiểu phải lớn hơn 0")]
        public decimal? MinMoney { get; set; }

        [Required(ErrorMessage = "Có cho phép chuyển nhượng không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsTransfer { get; set; }

        [Required(ErrorMessage = "Phân loại không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { CpsPolicyClassify.PRO, CpsPolicyClassify.PROA, CpsPolicyClassify.PNOTE })]
        public int? Classify { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
    }
}

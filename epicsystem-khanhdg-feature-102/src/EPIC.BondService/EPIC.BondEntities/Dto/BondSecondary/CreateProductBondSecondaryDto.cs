using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondSecondary
{
    public class CreateProductBondSecondaryDto
    {
        public int SecondaryId { get; set; }
        public int BondPrimaryId { get; set; }
        public int BusinessCustomerBankAccId { get; set; }

        //[Required(ErrorMessage = "Số lượng không được bỏ trống")]		
        //[Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        //public int? Quantity { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu bán không được bỏ trống")]
        public DateTime? OpenCellDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc bán không được bỏ trống")]
        public DateTime? CloseCellDate { get; set; }

        //[Required(ErrorMessage = "Danh sách chính sách không được bỏ trống")]	
        //[MinLength(1, ErrorMessage = "Danh sách chính sách không được bỏ trống")]
        public List<CreateProductBondPolicyDto> Policies { get; set; }
    }

    public class CreateProductBondPolicyDtoBase
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
        [IntegerRange(AllowableValues = new int[] { BondPolicyType.FIX, BondPolicyType.FLEXIBLE })]
        public int Type { get; set; }

        [Required(ErrorMessage = "Loại nhà đầu tư không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { Utils.InvestorType.PROFESSIONAL, Utils.InvestorType.ALL })]
        public string InvestorType { get; set; }

        [Required(ErrorMessage = "Thuế lợi nhuận không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Thuế lợi nhuận phải lớn hơn 0")]
        public decimal IncomeTax { get; set; }

        [Required(ErrorMessage = "Thuế chuyển nhượng không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Thuế chuyển nhượng phải lớn hơn 0")]
        public decimal TransferTax { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Tiền đầu tư tối thiểu phải lớn hơn 0")]
        public decimal? MinMoney { get; set; }

        [Required(ErrorMessage = "Có cho phép chuyển nhượng không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsTransfer { get; set; }

        [Required(ErrorMessage = "Phân loại không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { BondPolicyClassify.PRO, BondPolicyClassify.PROA, BondPolicyClassify.PNOTE })]
        public int Classify { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Dùng cho thêm mới bán theo kỳ hạn
    /// </summary>
    public class CreateProductBondPolicyDto : CreateProductBondPolicyDtoBase
    {
        [Required(ErrorMessage = "Danh sách kỳ hạn không được bỏ trống")]
        [MinLength(1, ErrorMessage = "Danh sách kỳ hạn không được bỏ trống")]
        public List<CreateProductBondPolicyDetailDto> Details { get; set; }
    }

    public class ProductBondPolicyDto
    {
        public int PolicyId { get; set; }
        public int TradingProviderId { get; set; }
        public int SecondaryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string InvestorType { get; set; }
        public decimal? IncomeTax { get; set; }
        public decimal? TransferTax { get; set; }
        public decimal? Classify { get; set; }
        public decimal? MinMoney { get; set; }
        public string IsTransfer { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsShowApp { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public List<ProductBondPolicyDetailDto> PolicyDetail { get; set; }

    }

    public class ProductBondPolicyDetailDto
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public int? STT { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string InterestPeriodType { get; set; }
        public int? InterestPeriodQuantity { get; set; }

        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }
        public string Status { get; set; }
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }
        public int? InterestType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public string IsShowApp { get; set; }

    }

    public class CreateProductBondPolicySpecificDto : CreateProductBondPolicyDtoBase
    {
        public int BondPolicyTempId { get; set; }
    }

    public class CreateProductBondPolicyDetailDto
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
        [StringRange(AllowableValues = new string[] { PeriodUnit.DAY,
            PeriodUnit.MONTH, PeriodUnit.YEAR },
        ErrorMessage = "Đơn vị đáo hạn không hợp lệ")]
        public string PeriodType { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Số kỳ đáo hạn phải lớn hơn 0")]
        [Required(ErrorMessage = "Số kỳ đầu tư không được bỏ trống")]
        public int PeriodQuantity { get; set; }

        [Required(ErrorMessage = "Kiểu trả lãi không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { InterestTypes.DINH_KY, InterestTypes.CUOI_KY })]
        public int InterestType { get; set; }

        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Số kỳ trả phải lớn hơn 0")]
        public int? InterestPeriodQuantity { get; set; }

        [StringRange(AllowableValues = new string[] { PeriodUnit.DAY,
            PeriodUnit.MONTH, PeriodUnit.YEAR },
            ErrorMessage = "Loại kỳ trả lãi không hợp lệ")]
        public string InterestPeriodType { get; set; }

        [Required(ErrorMessage = "Lợi nhuận không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Lợi nhuận phải lớn hơn 0")]
        public decimal Profit { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Số ngày phải lớn hơn 0")]
        public int? InterestDays { get; set; }
    }
}

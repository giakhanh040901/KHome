using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicy
{
    public class CreatePolicyDto
    {
        public int DistributionId { get; set; }

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

        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Thuế lợi nhuận phải lớn hơn 0")]
        public decimal IncomeTax { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Tiền đầu tư tối thiểu phải lớn hơn 0")]
        public decimal MinMoney { get; set; }

        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Tiền đầu tư tối đa phải lớn hơn 0")]
        public decimal? MaxMoney { get; set; }

        [Required(ErrorMessage = "Loại nhà đầu tư không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { EPIC.Utils.InvestorType.PROFESSIONAL, EPIC.Utils.InvestorType.ALL })]
        public string InvestorType { get; set; }

        [Required(ErrorMessage = "Số ngày tích lúy tối thiểu không được bỏ trống")]
        public int MinInvestDay { get; set; }

        [IntegerRange(AllowableValues = new int[] { GarnerPolicyClassify.MUA_BAN, GarnerPolicyClassify.HOP_TAC })]
        public int Classify { get; set; }

        [IntegerRange(AllowableValues = new int[] { CalculateTypes.NET, CalculateTypes.GROSS })]
        public int CalculateType { get; set; }

        [IntegerRange(AllowableValues = new int[] { PolicyGarnerTypes.LINH_HOAT, PolicyGarnerTypes.DINH_KY })]
        public int GarnerType { get; set; }

        [IntegerRange(AllowableValues = new int[] { PolicyGarnerTypes.LINH_HOAT, PolicyGarnerTypes.DINH_KY, PolicyGarnerTypes.NGAY_CO_DINH, PolicyGarnerTypes.NGAY_DAU_THANG, PolicyGarnerTypes.NGAY_CUOI_THANG })]
        public int? InterestType { get; set; }

        [StringRange(AllowableValues = new string[] { EPIC.Utils.PeriodType.NGAY, EPIC.Utils.PeriodType.THANG, EPIC.Utils.PeriodType.NAM })]
        public string InterestPeriodType { get; set; }
        public int? InterestPeriodQuantity { get; set; }

        [Range(1, 28, ErrorMessage = "Ngày chi trả cố định phải trong khoảng từ 1 đến 28")]
        public int? RepeatFixedDate { get; set; }
        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Số tiền rút tối thiểu phải lớn hơn 0")]
        public decimal MinWithdraw { get; set; }

        [Range(double.Epsilon, (double)decimal.MaxValue, ErrorMessage = "Số tiền rút tối đa phải lớn hơn 0")]
        public decimal? MaxWithdraw { get; set; }
        public decimal WithdrawFee { get; set; }
        public int OrderOfWithdrawal { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsTransferAssets { get; set; }
        public string IsShowApp { get; set; }
        public string IsDefault { get; set; }
        public string IsDefaultEpic { get; set; }
        public decimal TransferAssetsFee { get; set; }
        [IntegerRange(AllowableValues = new int[] { WithdrawFeeTypes.SO_TIEN, WithdrawFeeTypes.THEO_NAM })]
        public int WithdrawFeeType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public int? PolicyTempId { get; set; }
    }
}

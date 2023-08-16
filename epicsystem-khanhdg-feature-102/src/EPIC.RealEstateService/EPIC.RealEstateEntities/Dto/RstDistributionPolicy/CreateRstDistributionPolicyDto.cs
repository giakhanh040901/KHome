using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionPolicy
{
    public class CreateRstDistributionPolicyDto
    {
        /// <summary>
        /// Id bán phân phối
        /// </summary>
        [Required]
        public int DistributionId { get; set; }
        private string _code;
        /// <summary>
        /// Mã chính sách
        /// </summary>
        [Required(ErrorMessage = "Mã chính sách không được để trống")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        /// <summary>
        /// Tên chính sách
        /// </summary>
        [Required(ErrorMessage = "Tên chính sách không được để trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Loại thanh toán 1: Thanh toán thường, 2: Trả góp ngân hàng, 3: Trả trước<br/>
        /// <see cref="RstProjectDistributionPolicyPaymentTypes"/>
        /// </summary>
        [Required(ErrorMessage = "Loại thanh toán không được để trống")]
        [IntegerRange(AllowableValues = new int[] { RstProjectDistributionPolicyPaymentTypes.THANH_TOAN_THONG_THUONG, RstProjectDistributionPolicyPaymentTypes.TRA_GOP,
            RstProjectDistributionPolicyPaymentTypes.TRA_TRUOC },ErrorMessage = "Vui lòng loại thanh toán 1: Thanh toán thường, 2: Trả góp ngân hàng, 3: Trả trước")]
        public int PaymentType { get; set; }

        /// <summary>
        /// Loại hình đặt cọc: 1: Theo giá trị căn hộ, 2: Giá cố định<br/>
        /// <see cref="RstDistributionPolicyTypes"/>
        /// </summary>
        [Required(ErrorMessage = "Loại hình đặt cọc không được để trống")]
        [IntegerRange(AllowableValues = new int[] { RstDistributionPolicyTypes.GIA_CAN_HO, RstDistributionPolicyTypes.GIA_CO_DINH}, 
            ErrorMessage = "Vui lòng loại hình đặt cọc 1: Theo giá trị căn hộ, 2: Giá cố định")]
        public int DepositType { get; set; }

        /// <summary>
        /// Giá trị đặt cọc (%/VNĐ)
        /// </summary>
        [Required(ErrorMessage = "Giá trị đặt cọc không được để trống")]
        public decimal DepositValue { get; set; }

        /// <summary>
        /// Loại hình lock căn hộ 1: Theo giá trị căn hộ, 2: Giá cố định<br/>
        /// <see cref="RstDistributionPolicyTypes"/>
        /// </summary>
        [Required(ErrorMessage = "Giá trị lock căn không được để trống")]
        [IntegerRange(AllowableValues = new int[] { RstDistributionPolicyTypes.GIA_CAN_HO, RstDistributionPolicyTypes.GIA_CO_DINH },
            ErrorMessage = "Vui lòng giá trị lock cănc 1: Theo giá trị căn hộ, 2: Giá cố định")]
        public int LockType { get; set; }

        /// <summary>
        /// Giá trị lock căn hộ (%/VNĐ)
        /// </summary>

        [Required(ErrorMessage = "Giá trị lock căn hộ không được để trống")]
        public decimal LockValue { get; set; }

        private string _description;
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}

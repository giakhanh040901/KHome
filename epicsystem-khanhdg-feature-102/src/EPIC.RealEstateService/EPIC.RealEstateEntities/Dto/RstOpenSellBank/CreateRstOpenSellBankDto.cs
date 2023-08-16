using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellBank
{
    /// <summary>
    /// Thêm ngân hàng cho đại lý
    /// </summary>
    public class CreateRstOpenSellBankDto
    {
        public int? Id { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đối tác
        /// </summary>
        [RequiredWithOtherFields(ErrorMessage = "Ngân hàng của đối tác không được bỏ trống", OtherFields = new string[] { "TradingBankAccountId" })]
        public int? PartnerBankAccountId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đại lý
        /// </summary>
        [RequiredWithOtherFields(ErrorMessage = "Ngân hàng của đại lý không được bỏ trống", OtherFields = new string[] { "PartnerBankAccountId" })]
        public int? TradingBankAccountId { get; set; }
    }
}

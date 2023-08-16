using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class UpdateRstOrderDto
    {
        public int Id { get; set; }
        public int? InvestorIdenId { get; set; }

        /// <summary>
        /// Địa chỉ liên hệ, dùng cho nhà đầu tư cá nhân
        /// </summary>
        public int? ContractAddressId { get; set; }

        private string _saleReferralCode;
        public string SaleReferralCode
        {
            get => _saleReferralCode;
            set => _saleReferralCode = value?.Trim();
        }

        /// <summary>
        /// Hình thức thanh toán 1 Thanh toán thường, 2 Thanh toán Sớm 3: Trả góp ngân hàng
        /// </summary>
        public int? PaymentType { get; set; }

        /// <summary>
        /// Id sản phẩm mở bán
        /// </summary>
        public int OpenSellDetailId { get; set; }
        public List<UpdateRstOrderCoOwnerDto> RstOrderCoOwners { get; set; }
    }
}

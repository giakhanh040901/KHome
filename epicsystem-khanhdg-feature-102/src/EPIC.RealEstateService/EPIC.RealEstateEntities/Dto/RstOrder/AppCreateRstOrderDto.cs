using EPIC.RealEstateEntities.Dto.RstOrderCoOwner;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class AppCreateRstOrderDto
    {
        /// <summary>
        /// Id item giỏ hàng kho đặt lệnh từ App
        /// </summary>
        [RequiredWithOtherFields(ErrorMessage = "Item giỏ hàng không được bỏ trống", OtherFields = new string[] { "OpenSellDetailId" })]
        public int? CartId { get; set; }

        /// <summary>
        /// Id của mở bán nếu ko thông qua giỏ hàng
        /// </summary>
        [RequiredWithOtherFields(ErrorMessage = "Sản phẩm mở bán không được bỏ trống", OtherFields = new string[] { "CartId" })]
        public int? OpenSellDetailId { get; set; }

        /// <summary>
        /// Hình thức thanh toán 1 Thanh toán thường, 2 Thanh toán Sớm 3: Trả góp ngân hàng
        /// </summary>
        public int? PaymentType { get; set; } 

        /// <summary>
        /// Giấy tờ của nhà đầu tư
        /// </summary>
        public int? InvestorIdenId { get; set; }

        /// <summary>
        /// Địa chỉ nhận hợp đồng bản cứng
        /// </summary>
        public int? ContractAddressId { get; set; }

        /// <summary>
        /// Mã giới thiệu đặt lệnh
        /// </summary>
        private string _saleReferralCode;
        public string SaleReferralCode 
        { 
            get => _saleReferralCode; 
            set => _saleReferralCode = value?.Trim(); 
        }

        /// <summary>
        /// Danh sách ảnh mặt trước
        /// </summary>
        public List<IFormFile> IdFrontImages { get; set; }

        /// <summary>
        /// Danh sách ảnh mặt sau
        /// </summary>
        public List<IFormFile> IdBackImages { get; set; }

        /// <summary>
        /// Danh sách người đồng sở hữu để ở dạng json
        /// </summary>
        private string _orderCoOwners;
        public string OrderCoOwners 
        { 
            get => _orderCoOwners; 
            set => _orderCoOwners = value?.Trim(); 
        }
    }
}

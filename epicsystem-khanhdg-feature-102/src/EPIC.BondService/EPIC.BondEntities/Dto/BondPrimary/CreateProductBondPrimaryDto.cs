using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ProductBondPrimary
{
    public class CreateProductBondPrimaryDto 
    {
        [Required(ErrorMessage = "Lô trái phiếu không được bỏ trống")]
        public int ProductBondId { get; set; }

        public int PartnerId { get; set; }

        [Required(ErrorMessage = "Đại lý sơ cấp không được bỏ trống")]
        public int TradingProviderId { get; set; }

        public int BusinessCustomerBankAccId { get; set; }

        private string _code;
        [StringLength(50, ErrorMessage = "Mã gói trái phiếu không được dài hơn 50 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [StringLength(200, ErrorMessage = "Tên gói trái phiếu không được dài hơn 200 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        [Required(ErrorMessage = "Mã hợp đồng không được để trống")]
        private string _contractCode;
        public string ContractCode
        {
            get => _contractCode;
            set => _contractCode = value?.Trim();
        }

        [Required(ErrorMessage = "Ngày mở bán không được bỏ trống")]
        public DateTime? OpenSellDate { get; set; }

        public DateTime? CloseSellDate { get; set; }

        [Required(ErrorMessage = "Số lượng không được bỏ trống")]
        public long Quantity { get; set; }

        public long? MinMoney { get; set; }

        [Required(ErrorMessage = "Kiểu tính giá không được để trống")]
        public int PriceType { get; set; }

        public int? MaxInvestor { get; set; }

        private string _status;
        public string Status 
        { 
            get => _status; 
            set => _status = value?.Trim(); 
        }

        public string IsCheck { get; set; }

    }
}

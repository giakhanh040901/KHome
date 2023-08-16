using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellDetail
{
    /// <summary>
    /// Ẩn giá của sản phẩm mở bán
    /// </summary>
    public class RstOpenSellDetailHidePriceDto
    {
        /// <summary>
        /// Ip của sản phẩm mở bán
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Loại liên hệ
        /// </summary>
        public int ContractType { get; set; }

        /// <summary>
        /// Số điện thoại liên hệ
        /// </summary>
        [Required(ErrorMessage = "Số điện thoại liên hệ không được bỏ trống")]
        public string ContractPhone { get; set; }
    }
}

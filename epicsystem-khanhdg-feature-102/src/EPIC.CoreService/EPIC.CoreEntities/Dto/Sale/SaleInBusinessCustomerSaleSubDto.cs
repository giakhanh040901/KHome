using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    [Keyless]
    public class SaleInBusinessCustomerSaleSubDto
    {
        /// <summary>
        /// SaleId đang xét
        /// </summary>
        [Column("SALE_ID")]
        public int? SaleId { get; set; }

        /// <summary>
        /// Phòng ban mà sale đang xét đang thuộc
        /// </summary>
        [Column("DEPARTMENT_ID_SUB")]
        public int? DepartmentIdSub { get; set; }

        /// <summary>
        /// Id đại lý là kênh bán hộ
        /// </summary>
        [Column("TRADING_PROVIDER_ID_CHILD")]
        public int? TradingProviderIdChild { get; set; }

        /// <summary>
        /// Id sale của kênh bán hộ
        /// </summary>
        [Column("SALE_ID_IS_BUSINESS")]
        public int? SaleIdIsBusiness { get; set; }

        /// <summary>
        /// Mã giới thiệu của kênh bán hộ 
        /// </summary>
        [Column("REFERRAL_CODE")]
        public string ReferralCode { get; set; }

        /// <summary>
        /// Mã giới thiệu của kênh bán hộ 
        /// </summary>
        [Column("REFERRAL_CODE_SUB")]
        public string ReferralCodeSub { get; set; }

        /// <summary>
        /// Phòng ban mà kênh bán hộ đang nằm trong
        /// </summary>
        [Column("DEPARTMENT_ID")]
        public int? DepartmentId { get; set; }
    }
}

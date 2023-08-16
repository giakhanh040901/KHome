using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    [Keyless]
    public class AppListTradingProviderDto
    {
        [Column("TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [NotMapped]
        [Column("BUSINESS_CUSTOMER_ID")]
        public int BusinessCustomerId { get; set; }

        [NotMapped]
        //[Column("TRADING_PROVIDER_NAME")]
        public string TradingProviderName { get; set; }

        //[Column("TRADING_PROVIDER_ALIAS_NAME")]
        [NotMapped]
        public string TradingProviderAliasName { get; set; }

        //[Column("SHORT_NAME")]
        [NotMapped]
        public string ShortName { get; set; }

        //[Column("AVATAR_IMAGE_URL")]
        [NotMapped]
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Trạng thái Sale thuộc trong đại lý sơ cấp
        /// </summary>
        [Column("STATUS")]
        public string Status { get; set; }

        //[Column("REGISTER_DATE")]
        /// <summary>
        /// Ngày yêu cầu
        /// </summary>
        [NotMapped]
        public DateTime? RegisterDate { get; set; }

        //[Column("SIGN_DATE")]
        /// <summary>
        /// Ngày ký
        /// </summary>
        [NotMapped]
        public DateTime? SignDate { get; set; }

        /// <summary>
        /// Ngày khóa
        /// </summary>
        [Column("DEACTIVE_DATE")]
        public DateTime? DeactiveDate { get; set; }
    }
}

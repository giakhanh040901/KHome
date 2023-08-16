using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondSecondaryContract : IFullAudited
    {
        [Column(Name = "SECONDARY_CONTRACT_FILE_ID")]
        public int Id { get; set; }

        [Column(Name = "TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column(Name = "ORDER_ID")]
        public int OrderId { get; set; }

        [Column(Name = "CONTRACT_TEMP_ID")]
        public int ContractTempId { get; set; }

        [Column(Name = "FILE_TEMP_URL")]
        public string FileTempUrl { get; set; }

        [Column(Name = "FILE_SIGNATURE_URL")]
        public string FileSignatureUrl { get; set; }

        [Column(Name = "FILE_SCAN_URL")]
        public string FileScanUrl { get; set; }

        [Column(Name = "IS_SIGN")]
        public string IsSign { get; set; }

        [Column(Name = "PAGE_SIGN")]
        public int PageSign { get; set; }

        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }

        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }

        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }
    }
}

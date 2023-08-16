using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondOrderContractFile : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column(Name = "ORDER_ID")]
        public int OrderId { get; set; }

        [Column(Name = "CONTRACT_TEMP_ID")]
        public int ContractTempId { get; set; }

        [Column(Name = "FILE_URL")]
        public string FileUrl { get; set; }

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

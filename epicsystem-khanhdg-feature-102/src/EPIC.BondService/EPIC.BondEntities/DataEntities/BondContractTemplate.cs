using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondContractTemplate : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "BOND_SECONDARY_ID")]
        public int SecondaryId { get; set; }

        [Column(Name = "TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column(Name = "CODE")]
        public string Code { get; set; }
        [Column(Name = "NAME")]
        public string Name { get; set; }
        [Column(Name = "CONTRACT_TYPE")]
        public int ContractType { get; set; }  
        [Column(Name = "CONTRACT_TEMP_URL")]
        public string ContractTempUrl{ get; set; }
        
        [Column(Name = "CLASSIFY")]
        public int Classify { get; set; }
        [Column(Name = "STATUS")]
        public string Status { get; set; }
        public string Type { get; set; }

        
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

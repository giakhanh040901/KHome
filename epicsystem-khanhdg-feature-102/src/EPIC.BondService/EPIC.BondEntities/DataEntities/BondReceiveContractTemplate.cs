using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondReceiveContractTemplate : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column(Name = "SECONDARY_ID")]
        public int SecondaryId { get; set; }

        [Column(Name = "CODE")]
        public string Code { get; set; }
        [Column(Name = "NAME")]
        public string Name { get; set; }
        [Column(Name = "FILE_URL")]
        public string FileUrl { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }
        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }
        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }
        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }

        [Required]
        [Column(Name = "STATUS")]
        public string Status { get; set; }
    }
}

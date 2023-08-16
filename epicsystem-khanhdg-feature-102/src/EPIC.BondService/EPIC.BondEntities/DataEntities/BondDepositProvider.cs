using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondDepositProvider : IFullAudited
    {
        [Column(Name = "DEPOSIT_PROVIDER_ID")]
        public int Id { get; set; }

        [Column(Name = "BUSINESS_CUSTOMER_ID")]
        public int BusinessCustomerId { get; set; }

        [Column(Name = "PARTNER_ID")]
        public int PartnerId { get; set; }

        [Column(Name = "STATUS")]
        public int Status { get; set; }

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
    }
}

using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondGuaranteeAsset : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "BOND_ID")]
        public int BondId { get; set; }

        [Column(Name = "CODE")]
        public string Code { get; set; }

        [Column(Name = "ASSET_VALUE")]
        public decimal AssetValue { get; set; }

        [Column(Name = "DESCRIPTION_ASSET")]
        public string DescriptionAsset { get; set; }

        [Column(Name = "STATUS")]
        public int Status { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }

        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }

        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }
    }
}

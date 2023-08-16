using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondJuridicalFile : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "PRODUCT_BOND_ID")]
        public int BondId { get; set; }

        [Column(Name = "NAME")]
        public string Name { get; set; }

        [Column(Name = "URL")]
        public string Url { get; set; }

        [Column(Name = "STATUS")]
        public int Status { get; set; }

        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }

        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }

    }
}

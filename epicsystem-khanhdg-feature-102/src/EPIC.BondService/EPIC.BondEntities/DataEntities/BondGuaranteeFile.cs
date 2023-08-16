using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondGuaranteeFile
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "GUARANTEE_ASSET_ID")]
        public int GuaranteeAssetId { get; set; }

        [Column(Name = "TITLE")]
        public string Title { get; set; }

        [Column(Name = "FILE_URL")]
        public string FileUrl { get; set; }

        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }
    }
}

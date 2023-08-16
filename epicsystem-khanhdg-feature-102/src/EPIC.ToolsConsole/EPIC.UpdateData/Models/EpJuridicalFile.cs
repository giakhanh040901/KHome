using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpJuridicalFile
    {
        public decimal JuridicalFileId { get; set; }
        public decimal? ProductBondId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public bool? Stastus { get; set; }
    }
}

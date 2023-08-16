using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpBondOrderContractFile
    {
        public decimal OrderContractFileId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? ContractTempId { get; set; }
        public string FileUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }
}

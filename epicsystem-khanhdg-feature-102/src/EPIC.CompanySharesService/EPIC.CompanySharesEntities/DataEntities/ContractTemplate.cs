using EPIC.Entities;
using System;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class ContractTemplate : IFullAudited
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContractTempUrl { get; set; }
        public int TradingProviderId { get; set; }
        public int SecondaryId { get; set; }
        public string Type { get; set; }
        public string DisplayType { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; }
    }
}

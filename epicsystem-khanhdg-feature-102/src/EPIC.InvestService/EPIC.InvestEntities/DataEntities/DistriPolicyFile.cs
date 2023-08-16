﻿using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    public class DistriPolicyFile : IFullAudited
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        public int TradingProviderId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}

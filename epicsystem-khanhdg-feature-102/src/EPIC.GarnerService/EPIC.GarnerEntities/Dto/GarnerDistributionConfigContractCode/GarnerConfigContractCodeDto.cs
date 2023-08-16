using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode
{
    public class GarnerConfigContractCodeDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public List<GarnerConfigContractCodeDetailDto> ConfigContractCodeDetails { get; set; }
    }
}

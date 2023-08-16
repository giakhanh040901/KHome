using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp
{
    public class CreateGarnerReceiveContractTempDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public int DistributionId { get; set; }
    }
}

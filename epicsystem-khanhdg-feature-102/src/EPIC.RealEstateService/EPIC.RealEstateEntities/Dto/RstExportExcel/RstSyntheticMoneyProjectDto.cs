using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstExportExcel
{
    public class RstSyntheticMoneyProjectDto
    {
        public string Code { get; set; }
        public int? BuildingDensityType { get; set; }
        public int ClassifyType { get; set; }
        public int Status { get; set; }
        public int OrderStatus { get; set; }
        public decimal PaymentAmount { get; set; }

    }
}

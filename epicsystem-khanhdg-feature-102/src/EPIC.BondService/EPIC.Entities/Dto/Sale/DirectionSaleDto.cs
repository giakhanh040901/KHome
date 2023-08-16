using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class DirectionSaleDto
    {
        public int SaleRegisterId { get; set; }
        public int TradingProviderId { get; set; }

        [IntegerRange(AllowableValues = new int[] { SaleTypes.EMPLOYEE, SaleTypes.COLLABORATOR, SaleTypes.SALE_REPRESENTATIVE })]
        public int SaleType { get; set; }
    }
}

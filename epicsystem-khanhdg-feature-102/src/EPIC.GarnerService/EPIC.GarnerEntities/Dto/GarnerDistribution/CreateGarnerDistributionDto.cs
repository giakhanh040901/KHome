using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class CreateGarnerDistributionDto
    {
        public int ProductId { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        [MinLength(1, ErrorMessage = "Vui lòng chọn ngân hàng thụ hưởng")]
        public List<int> TradingBankAccountCollects { get; set; }
        public List<int> TradingBankAccountPays { get; set; }
    }
}

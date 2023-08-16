using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class CoreSaleRegisterDto
    {
        public int Id { get; set; }
        public int InvestorId { get; set; }
        public int InvestorBankAccId { get; set; }
        public int SaleManagerId { get; set; }
        public int Status { get; set; }
        public string IpAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? DirectionDate { get; set; }
    }
}

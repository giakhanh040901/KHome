using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class AppHistoryRegisterDto
    {
        public int Status { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? DirectionDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public string SaleManagerReferralCode { get; set; }
        public string SaleManagerName { get; set; }
        public string SaleManagerAvatar { get; set; }
        public string SaleManagerPhone { get; set; }
        public string SaleManagerEmail { get; set; }
    }
}

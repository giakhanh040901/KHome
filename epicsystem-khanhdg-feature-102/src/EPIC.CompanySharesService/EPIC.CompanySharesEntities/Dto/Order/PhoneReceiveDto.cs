using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.Order
{
    public class PhoneReceiveDto
    {
        public int TradingProviderId { get; set; }
        public string Phone { get; set; }
        public int DeliveryStatus { get; set; }
    }
}

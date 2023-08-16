using EPIC.BondEntities.DataEntities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Bond;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Order;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.RealEstateEntities.Dto.RstOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentSharedEntities.Dto
{
    public class ViewMsbCollectionPaymentDto : MsbNotificationDto
    {
        public GarnerOrderDto GarnerOrder { get; set; }
        public BondOrder BondOrder { get; set; }
        public ViewOrderDto InvestOrder { get; set; }
        public RstOrderDto RstOrder { get; set; }
        public EvtOrderDto EvtOrder { get; set; }
        public DateTime? TransferDate { get; set; }
        public string GenContractCode { get; set; }
    }
}

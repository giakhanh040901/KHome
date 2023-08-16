using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerSharedEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class GarnerOrderCifCodeDto : GarnerOrderDto
    {
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public GarnerPolicyMoreInfoDto Policy { get; set; }
        public GarnerPolicyDetailDto PolicyDetail { get; set; }
        public List<GarnerOrderDto> GarnerOrders { get; set; }
        public GarnerProductDto Product { get; set; }
    }
}

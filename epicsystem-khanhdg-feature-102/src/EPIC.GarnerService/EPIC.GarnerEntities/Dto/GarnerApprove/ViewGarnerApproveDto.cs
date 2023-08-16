using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.User;
using EPIC.EntitiesBase.Dto.ModuleApprove;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerApprove
{
    public class ViewGarnerApproveDto: ModuleDataApproveBaseDto
    {
        public UserDto UserRequest { get; set; }
        public UserDto UserApprove { get; set; }

        /// <summary>
        /// To chuc phat hanh khi xem GarnerProduct
        /// </summary>
        public BusinessCustomerDto CpsIssuer { get; set; }
    }
}

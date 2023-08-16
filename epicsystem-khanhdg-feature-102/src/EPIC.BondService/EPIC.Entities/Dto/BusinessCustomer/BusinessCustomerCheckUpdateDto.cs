using EPIC.Entities.Dto.User;
using System;

namespace EPIC.Entities.Dto.BusinessCustomer
{
    public class BusinessCustomerCheckUpdateDto
    {
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public BusinessCustomerTempDto BusinessCustomerTemp { get; set; }
        public UserDto UserRequest { get; set; }
        public DateTime? RequestDate { get; set; }
    }
}

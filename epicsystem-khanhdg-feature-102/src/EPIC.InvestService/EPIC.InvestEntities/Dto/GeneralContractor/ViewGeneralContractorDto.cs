using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.GeneralContractor
{
    public class ViewGeneralContractorDto
    {
        public int Id { get; set; }
        public int BusinessCustomerId { get; set; }
        public int PartnerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TaxCode { get; set; }
        public string RepName { get; set; }
        public string ShortName { get; set; }
        public int Status { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}

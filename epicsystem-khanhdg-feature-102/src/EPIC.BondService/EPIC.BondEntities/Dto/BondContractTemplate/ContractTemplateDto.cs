using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractTemplate
{
    public class ContractTemplateDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ContractTypeId { get; set; }
        public string ContractTempType { get; set; }
        public string ContractTempUrl { get; set; }
        public string ContractTempContent { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ContractTypeName{ get; set; }
        public string Status{ get; set; }
    }
}

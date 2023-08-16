using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail
{
    public class RstConfigContractCodeDetailDto
    {
        public int Id { get; set; }
        public int ConfigContractCodeId { get; set; }
        public int SortOrder { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

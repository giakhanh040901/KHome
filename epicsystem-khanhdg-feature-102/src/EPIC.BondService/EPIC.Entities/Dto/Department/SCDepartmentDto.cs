using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Department
{
    public class SCDepartmentDto
    {
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public string Area { get; set; }
        public int? DepartmentLevel { get; set; }
    }
}

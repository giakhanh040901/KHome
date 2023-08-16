using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Department
{
    public class UpdateListSaleToNewDepartment
    {
        public int DepartmentId { get; set; }
        public int NewDepartmentId { get; set; }
        public List<int> Sales { get; set; }
    }
}

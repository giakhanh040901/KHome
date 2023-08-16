using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectType
{
    public class UpdateProjectTypeDto : CreateProjectTypeDto
    {
        public int? Id { get; set; }
    }
}

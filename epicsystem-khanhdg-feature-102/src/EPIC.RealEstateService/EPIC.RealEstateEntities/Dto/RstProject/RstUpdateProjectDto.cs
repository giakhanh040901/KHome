using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    /// <summary>
    /// Cập nhận dự án
    /// </summary>
    public class RstUpdateProjectDto : RstCreateProjectDto
    {
        public int Id { get; set; }
    }
}

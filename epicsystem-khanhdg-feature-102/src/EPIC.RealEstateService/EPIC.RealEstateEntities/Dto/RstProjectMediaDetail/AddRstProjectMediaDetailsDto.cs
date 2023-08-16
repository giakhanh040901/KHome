using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMediaDetail
{
    /// <summary>
    /// thêm mới list project media detail vào project media
    /// </summary>
    public class AddRstProjectMediaDetailsDto
    {
        public int ProjectMediaId { get; set; }
        public List<CreateRstProjectMediaDetailDto> RstProjectMediaDetails { get; set; }
    }
}

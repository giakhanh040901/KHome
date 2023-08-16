using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMedia
{
    public class RstProjectMediaDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string GroupTitle { get; set; }
        public List<RstProjectMediaDetailDto> RstProjectMediaDetail { get; set; }
    }
}

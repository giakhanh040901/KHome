using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMediaDetail
{
    public class RstProjectMediaDetailDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public int ProjectMediaId { get; set; }
        public string GroupTitle { get; set; }
        public string UrlImage { get; set; }
        public string MediaType { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public int SortOrder { get; set; }
    }
}

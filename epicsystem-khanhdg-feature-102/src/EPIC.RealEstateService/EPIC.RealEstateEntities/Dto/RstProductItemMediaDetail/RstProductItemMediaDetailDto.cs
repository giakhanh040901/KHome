using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail
{
    public class RstProductItemMediaDetailDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public int ProductItemMediaId { get; set; }
        public string GroupTitle { get; set; }
        public string UrlImage { get; set; }
        public string MediaType { get; set; }
        public string Status { get; set; }
        public int SortOrder { get; set; }
    }
}

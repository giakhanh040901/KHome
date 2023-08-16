using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstRating
{
    public class ViewRstRatingDto
    {
        public string FaceImageUrl { get; set; }
        public string Name { get; set; }
        public string Feedback { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

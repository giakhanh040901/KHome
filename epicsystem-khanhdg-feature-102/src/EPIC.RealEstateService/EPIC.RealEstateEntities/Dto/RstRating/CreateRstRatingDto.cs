using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstRating
{
    public class CreateRstRatingDto
    {
        public int Rate { get; set; }
        public int ProductExperience { get; set; }
        public string Feedback { get; set; }
    }
}

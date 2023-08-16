using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestRating
{
    public class CreateInvestRatingDto
    {
        public int Rate { get; set; }
        public int ProductExperience { get; set; }
        public string Feedback { get; set; }
    }
}

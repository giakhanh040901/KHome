using System.Collections.Generic;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor
{
    public class CreateLuckyProgramInvestorDto
    {
        public int LuckyProgramId { get; set; }
        public List<int> InvestorIds { get; set; }
    }
}

using EPIC.BondEntities.DataEntities;
using EPIC.Entities.Dto.CoreApprove;

namespace EPIC.BondEntities.Dto.RenewalsRequest
{
    public class ViewRenewalsRequestDto : ViewCoreApproveDto
    {
        public int? SettlementMethod { get; set; }
        public BondPolicyDetail PolicyDetail { get; set; }
        public DataEntities.BondPolicy Policy { get; set; }
        public DataEntities.BondInfo ProductBondInfo { get; set; }
    }
}

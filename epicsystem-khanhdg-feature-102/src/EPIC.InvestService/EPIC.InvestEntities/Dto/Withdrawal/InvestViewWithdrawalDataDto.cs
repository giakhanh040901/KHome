using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.Utils.DataUtils;
using System.Collections.Generic;

namespace EPIC.InvestEntities.Dto.Withdrawal
{
    /// <summary>
    /// Xem dữ liệu khi rút vốn
    /// </summary>
    public class InvestViewWithdrawalDataDto : RutVonDto
    {
        public List<ReplaceTextDto> DataContractFileWithdrawal { get; set; }
    }
}

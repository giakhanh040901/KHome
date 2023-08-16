using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class SetDefaultInvestorStockDto
    {
        public int Id { get; set; }
        public int InvestorId { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsDefault { get; set; }
        public bool IsTemp { get; set; }
    }
}

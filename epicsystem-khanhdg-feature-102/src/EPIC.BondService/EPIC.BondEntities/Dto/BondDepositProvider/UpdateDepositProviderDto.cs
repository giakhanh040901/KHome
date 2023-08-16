using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DepositProvider
{
    public class UpdateDepositProviderDto : CreateDepositProviderDto
    {
        public int DepositProviderId { get; set; }
    }
}

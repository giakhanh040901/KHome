using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DepositProvider
{
    public class CreateDepositProviderDto
    {
        public int BusinessCustomerId { get; set; }
    }
}

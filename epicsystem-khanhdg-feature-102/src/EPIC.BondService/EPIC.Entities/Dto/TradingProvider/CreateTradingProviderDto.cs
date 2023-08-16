using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.TradingProvider
{
    public class CreateTradingProviderDto
    {
        public int BusinessCustomerId { get; set; }
        public string AliasName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.Calendar
{
    public class Test
    {
        [Column("TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column("ALIAS_NAME")]
        public string AliasName { get; set; }
    }
}

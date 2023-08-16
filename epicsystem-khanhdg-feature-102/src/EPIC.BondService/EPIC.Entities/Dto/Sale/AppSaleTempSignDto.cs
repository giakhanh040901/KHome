﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSaleTempSignDto
    {
        [Range(1, int.MaxValue)]
        public int SaleId { get; set; }

        [Range(1, int.MaxValue)]
        public int TradingProviderId { get; set; }

        [Range(1, int.MaxValue)]
        public int InvestorId { get; set; }
    }
}

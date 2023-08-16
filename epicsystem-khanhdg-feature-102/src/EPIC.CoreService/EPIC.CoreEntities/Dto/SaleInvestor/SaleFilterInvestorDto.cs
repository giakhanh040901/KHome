using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class SaleFilterInvestorDto
    {
        private string _keyword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [FromQuery(Name = "keyword")]
        public string Keyword { get => _keyword ; set => _keyword = value?.Trim(); }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedEntities.Dto.InvestorTelesale
{
    public class FilterInvestorTelesaleDto
    {
        private string _idNo;
        [FromQuery(Name = "idNo")]
        public string IdNo
        {
            get => _idNo;
            set => _idNo = value?.Trim();
        }

        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }
        
        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }
    }
}

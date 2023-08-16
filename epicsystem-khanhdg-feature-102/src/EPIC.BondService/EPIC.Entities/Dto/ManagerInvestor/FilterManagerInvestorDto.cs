using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class FilterManagerInvestorDto : PagingRequestBaseDto
    {
        //[FromQuery(Name = "idNo")]
        //public string IdNo { get; set; }

        //[FromQuery(Name = "cifCode")]
        //public string CifCode { get; set; }

        [FromQuery(Name = "requireKeyword")]
        public bool RequireKeyword { get; set; }

        [FromQuery(Name = "representativePhone")]
        public string RepresentativePhone { get; set; }

        [FromQuery(Name = "representativeEmail")]
        public string RepresentativeEmail { get; set; }
    }
}

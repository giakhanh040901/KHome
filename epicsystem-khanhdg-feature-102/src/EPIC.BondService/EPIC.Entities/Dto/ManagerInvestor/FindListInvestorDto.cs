using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class FindListInvestorDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public string Status { get; set; }

        [FromQuery(Name = "cifCode")]
        public string CifCode { get; set; }

        [FromQuery(Name = "dateOfBirth")]
        public string DateOfBirth { get; set; }

        [FromQuery(Name = "sex")]
        public string Sex { get; set; }

        [FromQuery(Name = "fullname")]
        public string Fullname { get; set; }

        [FromQuery(Name = "nationality")]
        public string Nationality { get; set; }

        [FromQuery(Name = "phone")]
        public string Phone { get; set; }

        [FromQuery(Name = "email")]
        public string Email { get; set; }
        [FromQuery(Name = "representativePhone")]
        public string RepresentativePhone { get; set; }

        [FromQuery(Name = "representativeEmail")]
        public string RepresentativeEmail { get; set; }

        /// <summary>
        /// Truyền bởi dấu phẩy
        /// </summary>
        [FromQuery(Name = "tradingProviderId")]
        public string TradingProviderId { get; set; }

        [FromQuery(Name = "idNo")]
        public string IdNo { get; set; }

        [FromQuery(Name = "isCheck")]
        public string IsCheck { get; set; }

        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
    }
}

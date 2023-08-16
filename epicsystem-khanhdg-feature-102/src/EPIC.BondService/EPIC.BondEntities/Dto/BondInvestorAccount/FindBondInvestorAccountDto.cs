using DocumentFormat.OpenXml.Drawing.Diagrams;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BondInvestorAccount
{
    public class FindBondInvestorAccountDto : PagingRequestBaseDto
    {
        private string _status { get; set; }
        private string _fullname { get; set; }
        private string _phone { get; set; }
        private string _cifCode { get; set; }
        private string _email { get; set; }
        private string _sex { get; set; }

        [FromQuery(Name = "status")]
        public string Status { get => _status; set => _status = value?.Trim(); }

        [FromQuery(Name = "fullname")]
        public string Fullname { get => _fullname; set => _fullname = value?.Trim(); }

        [FromQuery(Name = "phone")]
        public string Phone { get => _phone; set => _phone = value?.Trim(); }

        [FromQuery(Name = "cifCode")]
        public string CifCode { get => _cifCode; set => _cifCode = value?.Trim(); }

        [FromQuery(Name = "email")]
        public string Email { get => _email; set => _email = value?.Trim(); }

        [FromQuery(Name = "sex")]
        public string Sex { get => _sex; set => _sex = value?.Trim(); }

        [FromQuery(Name = "startAge")]
        public int? StartAge { get; set; }

        [FromQuery(Name = "endAge")]
        public int? EndAge { get; set; }

        /// <summary>
        /// Đối tượng gửi null, 1: Khách hàng, 2: Tài khoản chưa xác minh, 3: Sale
        /// </summary>
        [FromQuery(Name = "customerType")]
        public int? CustomerType { get; set; }
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
        [FromQuery(Name = "partnerId")]
        public int? PartnerId { get; set; }
    }
}

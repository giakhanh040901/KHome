using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreApprove
{
    public class GetApproveListDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        [FromQuery(Name = "userApproveId")]
        public int? UserApproveId { get; set; }

        [FromQuery(Name = "userRequestId")]
        public int? UserRequestId { get; set; }

        [FromQuery(Name = "dataType")]
        public int? DataType { get; set; }

        [FromQuery(Name = "actionType")]
        public int? ActionType { get; set; }

        [FromQuery(Name = "summary")]
        public string Summary { get; set; }

        [FromQuery(Name = "requestDate")]
        public DateTime? RequestDate { get; set; }
        [FromQuery(Name = "approveDate")]
        public DateTime? ApproveDate { get; set; }

        [FromQuery(Name = "referId")]
        public int? ReferId { get; set; }

        [FromQuery(Name = "referIdTemp")]
        public int? ReferIdTemp { get; set; }

        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
    }
}

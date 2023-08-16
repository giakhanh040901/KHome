using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.EntitiesBase.Dto.ModuleApprove
{
    public class ModuleFilterApproveBaseDto : PagingRequestBaseDto
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

        private string _summary;

        [FromQuery(Name = "summary")]
        public string Summary
        {
            get => _summary;
            set => _summary = value?.Trim();
        }

        [FromQuery(Name = "requestDate")]
        public DateTime? RequestDate { get; set; }

        [FromQuery(Name = "approveDate")]
        public DateTime? ApproveDate { get; set; }
    }
}

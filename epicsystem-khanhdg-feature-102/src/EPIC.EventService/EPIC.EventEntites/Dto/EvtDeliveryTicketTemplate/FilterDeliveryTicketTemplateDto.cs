using EPIC.EntitiesBase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtDeliveryTicketTemplate
{
    public class FilterDeliveryTicketTemplateDto : PagingRequestBaseDto
    {
        /// <summary>
        /// ID sự kiện
        /// </summary>
        public int EventId { get; set; }
    }
}

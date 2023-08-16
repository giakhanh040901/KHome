using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtDeliveryTicketTemplate
{
    public class EvtDeliveryTicketTemplateDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id su kien
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// ten
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// url
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// trang thai
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// nguoi tao
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// ngay tao
        /// </summary>
        public DateTime? CreatedDate { get; set; }
    }
}

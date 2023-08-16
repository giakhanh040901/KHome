using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtDeliveryTicketTemplate
{
    public class CreateDeliveryTicketTemplateDto
    {
        /// <summary>
        /// Id event
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Tên mẫu
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// File url
        /// </summary>
        private string _fileUrl;
        public string FileUrl
        {
            get => _fileUrl;
            set => _fileUrl = value?.Trim();
        }
    }
}

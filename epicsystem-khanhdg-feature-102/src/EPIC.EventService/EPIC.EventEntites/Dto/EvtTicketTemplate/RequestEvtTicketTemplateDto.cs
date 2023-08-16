using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtTicketTemplate
{
    public class CreateEvtTicketTemplateDto
    {
        /// <summary>
        /// id event
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// tên
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// url
        /// </summary>
        private string _fileUrl;
        public string FileUrl
        {
            get => _fileUrl;
            set => _fileUrl = value?.Trim();
        }
    }

    public class UpdateEvtTicketTemplateDto : CreateEvtTicketTemplateDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
    }
}

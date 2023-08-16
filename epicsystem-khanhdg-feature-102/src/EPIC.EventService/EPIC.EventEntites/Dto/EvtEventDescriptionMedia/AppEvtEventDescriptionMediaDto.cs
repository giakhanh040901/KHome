using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventDescriptionMedia
{
    public class AppEvtEventDescriptionMediaDto
    {
        /// <summary>
        /// Id ảnh
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id sự kiện
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string UrlImage { get; set; }
    }
}

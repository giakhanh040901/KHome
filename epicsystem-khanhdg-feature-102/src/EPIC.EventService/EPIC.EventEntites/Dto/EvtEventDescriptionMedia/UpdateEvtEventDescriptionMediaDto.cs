using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventDescriptionMedia
{
    public class UpdateEvtEventDescriptionMediaDto
    {
        /// <summary>
        /// Id ảnh
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        private string _urlImgae;
        public string UrlImage
        {
            get => _urlImgae;
            set => _urlImgae = value?.Trim();
        }

    }
}

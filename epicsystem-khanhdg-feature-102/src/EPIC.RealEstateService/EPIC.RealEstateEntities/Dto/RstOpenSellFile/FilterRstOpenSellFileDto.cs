using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellFile
{
    public class FilterRstOpenSellFileDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Id mở bán
        /// </summary>
        [FromQuery(Name = "openSellId")]
        public int? OpenSellId { get; set; }

        /// <summary>
        /// 1: Tài liệu phân phối, 2: Chính sách ưu đãi, 3:Chương trình bán hàng
        /// </summary>
        [FromQuery(Name = "openSellFileType")]
        public int? OpenSellFileType { get; set; }
        /// <summary>
        /// A: Kích hoạt, D: Khoá
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Tên file
        /// </summary>
        public string Name { get; set; }
    }
}

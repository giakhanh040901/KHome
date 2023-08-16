using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate
{
    public class FilterRstOpenSellContractTemplateDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Id mở bán
        /// </summary>
        [FromQuery(Name = "openSellId")]
        public int OpenSellId { get; set; }

        /// <summary>
        /// Tên hợp đồng
        /// </summary>
        [FromQuery(Name = "contractTemplateTempName")]
        public string ContractTemplateTempName { get; set; }

        /// <summary>
        /// A: Kích hoạt, D: Khoá
        /// </summary>
        [FromQuery(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Kieu hop dong (1: HD ban BDS, 2: HD mua BDS, 3 HD thanh ly, 4: Hợp đồng đặt cọc, 5. Hợp đồng thỏa thuận đặt cọc, 6. Hợp đồng đảm bảo, 7. Hợp đồng lock căn, 8. Hợp đồng Hủy lock căn)
        /// </summary>
        [FromQuery(Name = "contractType")]
        public int? ContractType { get; set; }
    }
}

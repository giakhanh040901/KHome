using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstProjectFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppProjectFilesDto
    {
        /// <summary>
        /// Hồ sơ pháp lý dự án
        /// </summary>
        public List<AppViewProjectFileDto> ProjectFiles { get; set; }
        /// <summary>
        /// Tài liệu phân phối
        /// </summary>
        public List<AppViewOpenSellFileDto> DistributionFiles { get; set; }
        /// <summary>
        /// Chương trình bán hàng
        /// </summary>
        public List<AppViewOpenSellFileDto> OpenSellFiles { get; set; }
        /// <summary>
        /// Tài liệu bán hàng của của dự án
        /// </summary>
        public List<AppViewProjectFileDto> SellingDocumentFiles { get; set; }
    }
}

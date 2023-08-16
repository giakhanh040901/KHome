using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProducItemDesignDiagramFile
{
    public class RstProductItemDesignDiagramFileDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id căn hộ
        /// </summary>
        public int ProductItemId { get; set; }
        /// <summary>
        /// Tên file
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn file
        /// </summary>
        public string FileUrl { get; set; }
    }
}

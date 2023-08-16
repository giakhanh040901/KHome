using EPIC.RealEstateEntities.Dto.RstProductItemMaterialFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProducItemDesignDiagramFile
{
    public class CreateRstProductItemDesignDiagramFileDto
    {
        /// <summary>
        /// Id căn hộ
        /// </summary>
        public int ProductItemId { get; set; }
        /// <summary>
        /// Danh sách File vật liệu
        /// </summary>
        public List<RstMaterialFileDto> Files { get; set; }
    }
    public class RstDesignDiagramFileDto
    {
        /// <summary>
        /// Tên file
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Đường dẫn file
        /// </summary>
        private string _fileUrl;
        public string FileUrl
        {
            get => _fileUrl;
            set => _fileUrl = value?.Trim();
        }
    }
}

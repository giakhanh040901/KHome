using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectFile
{
    public class AppViewProjectFileDto
    {
        /// <summary>
        /// Id hồ sơ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên hồ sơ pháp lý
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn file hồ sơ pháp lý
        /// </summary>
        public string Url { get; set; }
    }
}

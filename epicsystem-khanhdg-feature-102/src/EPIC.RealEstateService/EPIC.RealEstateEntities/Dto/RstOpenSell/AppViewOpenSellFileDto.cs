using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSell
{
    public class AppViewOpenSellFileDto
    {
        /// <summary>
        /// Id file
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên file
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn file 
        /// </summary>
        public string Url { get; set; }
    }
}

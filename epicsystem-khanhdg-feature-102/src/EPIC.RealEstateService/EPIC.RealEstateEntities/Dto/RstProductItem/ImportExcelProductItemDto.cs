using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class ImportExcelProductItemDto
    {
        /// <summary>
        /// File excel
        /// </summary>
        public IFormFile File { get; set; }
        public int ProjectId { get; set; }
    }
}

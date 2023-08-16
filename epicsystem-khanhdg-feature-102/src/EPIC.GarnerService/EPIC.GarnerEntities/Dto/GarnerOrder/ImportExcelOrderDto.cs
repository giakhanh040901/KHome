using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class ImportExcelOrderDto
    {
        /// <summary>
        /// File excel
        /// </summary>
        public IFormFile File { get; set; }
    }
}

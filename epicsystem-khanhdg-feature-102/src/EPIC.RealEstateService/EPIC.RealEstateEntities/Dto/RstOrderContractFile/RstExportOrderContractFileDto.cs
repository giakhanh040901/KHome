using EPIC.FillContractData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderContractFile
{
    public class RstExportOrderContractFileDto : ExportOrderContractFileDto
    {
        /// <summary>
        /// Id của orderContractFile
        /// </summary>
        public int Id { get; set; }
    }
}

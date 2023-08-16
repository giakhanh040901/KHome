using EPIC.FillContractData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderContractFile
{
    public class RstExportContracDto : ExportContractInputDtoBase
    {
        /// <summary>
        /// Id căn hộ
        /// </summary>
        public int ProductItemId { get; set; }
        /// <summary>
        /// Id project
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Id chi tiết mở bán
        /// </summary>
        public int OpenSellDetailId { get; set; }
    }
}

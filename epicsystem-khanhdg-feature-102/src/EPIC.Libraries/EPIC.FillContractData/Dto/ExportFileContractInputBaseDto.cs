using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.FillContractData.Dto
{
    /// <summary>
    /// Input cho hàm xuất file ExportContract
    /// </summary>
    public class ExportFileContractInputBaseDto
    {
        /// <summary>
        /// Id contract template
        /// </summary>
        public int Id { get; set; }
        //Loại hợp đồng
        public string ContractType { get; set; }
    }
}

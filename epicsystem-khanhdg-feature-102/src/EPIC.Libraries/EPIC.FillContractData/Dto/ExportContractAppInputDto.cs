using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.FillContractData.Dto
{
    /// <summary>
    /// input class cho hàm xuất file word tạm trên app
    /// </summary>
    public class ExportContractInputDtoBase : ContractFileInputDtoBase
    {
        /// <summary>
        /// Id mẫu hợp đồng
        /// </summary>
        public int ContractTemplateTempId { get; set; }
    }
}

using EPIC.Utils.DataUtils;
using System.Collections.Generic;

namespace EPIC.FillContractData.Dto
{
    /// <summary>
    /// Input cho hàm SaveContract
    /// </summary>
    public class SaveContractInputBaseDto
    {
        /// <summary>
        /// Loại hợp đồng (1: HĐ MUA BĐS, 2: HĐ BÁN BĐS, 3: HĐ THANH LÝ, 4: HĐ ĐẶT CỌC)
        /// </summary>
        public string ContractType { get; set; }
        /// <summary>
        /// Id mẫu hợp đồng
        /// </summary>
        public int ContractTemplateId { get; set; }
        /// <summary>
        /// Id đại lys
        /// </summary>
        public int? TradingProviderId { get; set; }
        /// <summary>
        /// Replace text fill data vào hợp đồng
        /// </summary>
        public List<ReplaceTextDto> ReplaceTexts { get; set; }
    }
}

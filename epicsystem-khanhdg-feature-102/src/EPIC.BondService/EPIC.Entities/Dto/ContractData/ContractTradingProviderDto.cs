using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractData
{
    /// <summary>
    /// Thông tin đại lý cho hợp đồng
    /// </summary>
    public class ContractTradingProviderDto
    {
        /// <summary>
        /// Tên đại lý sơ cấp
        /// </summary>
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// Nơi cấp mã số thuế
        /// </summary>
        public string LicenseIssuer { get; set; }

        /// <summary>
        /// Ngày cấp mã số thuế
        /// </summary>
        public DateTime? LicenseDate { get; set; }

        /// <summary>
        /// Địa chỉ dlsc
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Người đại diện
        /// </summary>
        public string RepName { get; set; }

        /// <summary>
        /// Chức vụ người đại diện
        /// </summary>
        public string RepPosition { get; set; }

        /// <summary>
        /// Số văn bản ủy quyền
        /// </summary>
        public string DecisionNo { get; set; }
    }
}

using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    /// <summary>
    /// Ngân hàng đảm bảo của dự án
    /// </summary>
    public class RstProjectGuaranteeBankDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string FullBankName { get; set; }
    }
}

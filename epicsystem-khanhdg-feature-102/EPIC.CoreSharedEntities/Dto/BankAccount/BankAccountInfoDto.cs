using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.BankAccount
{
    public class BankAccountInfoDto
    {
        /// <summary>
        /// Id ngân hàng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Chủ tài khoản
        /// </summary>
        public string OwnerAccount { get; set; }

        /// <summary>
        /// Có là mặc định Y/N
        /// </summary>
        public string IsDefault { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.FillContractData.Dto
{
    public class IdInfoInvestorDto
    {
        /// <summary>
        /// Id tài khoản thụ hưởng cá nhân của nhà đầu tư
        /// </summary>
        public int BankAccId { get; set; }
        /// <summary>
        /// Id giấy tờ cá nhân
        /// </summary>
        public int IdentificationId { get; set; }
    }
}

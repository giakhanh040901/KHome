using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    /// <summary>
    /// Sinh mô tả thông báo cho nhà đầu tư
    /// </summary>
    public class InvestorToDoDetails
    {
        /// <summary>
        /// Sinh mô tả thông báo cho nhà đầu tư theo loại
        /// </summary>
        /// <returns></returns>
        public static string Details(int type, int countPaymentDueDate)
        {
            return type switch
            {
                InvestorToDoTypes.INVEST_DEN_HAN => $"Nhà đầu tư đang có {countPaymentDueDate} hợp đồng sắp đến hạn",
                _ => string.Empty
            };
        }
    }
}

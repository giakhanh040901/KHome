using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSaleRegisterDto
    {
        /// <summary>
        /// Id quản lý
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "SaleManagerId phải lớn hơn 1")]
        public int SaleManagerId { get; set; }

        /// <summary>
        /// Tài khoản thụ hưởng
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "BankAccId phải lớn hơn 1")]
        public int BankAccId { get; set; }
    }
}

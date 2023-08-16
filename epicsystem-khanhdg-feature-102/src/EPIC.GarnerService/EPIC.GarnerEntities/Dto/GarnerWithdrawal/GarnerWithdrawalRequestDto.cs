using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class GarnerWithdrawalRequestDto
    {
        private string _cifCode;

        [Required]
        [FromQuery(Name = "cifCode")]
        public string CifCode 
        { 
            get => _cifCode;
            set => _cifCode = value?.Trim(); 
        }

        [FromQuery(Name = "policyId")]
        public int PolicyId { get; set; }

        [FromQuery(Name = "amount")]
        public decimal Amount { get; set; }

        [FromQuery(Name = "withdrawDate")]
        public DateTime WithdrawDate { get; set; }

        /// <summary>
        /// Id ngân hàng thụ hưởng nhận
        /// </summary>
        public int BankAccountId { get; set; }
    }
}

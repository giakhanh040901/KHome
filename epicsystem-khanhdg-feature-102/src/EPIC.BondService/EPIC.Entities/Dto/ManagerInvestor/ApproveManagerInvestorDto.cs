using EPIC.Utils;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ApproveManagerInvestorDto
    {
        /// <summary>
        /// Investor Id của bảng Investor Temp
        /// </summary>
        public int InvestorId { get; set; }
        public string Notice { get; set; }
    }
}

using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    [Keyless]
    public class AppSaleByReferralCodeDto
    {
        [ColumnSnackCase(nameof(SaleId))]
        public int SaleId { get; set; }

        [ColumnSnackCase(nameof(ReferralCode))]
        public string ReferralCode { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int? InvestorId { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }

        [ColumnSnackCase(nameof(Fullname))]
        public string Fullname { get; set; }

        [ColumnSnackCase(nameof(Phone))]
        public string Phone { get; set; }

        [ColumnSnackCase(nameof(Email))]
        public string Email { get; set; }

        [ColumnSnackCase(nameof(AvatarImageUrl))]
        public string AvatarImageUrl { get; set; }

        [NotMapped]
        [ColumnSnackCase(nameof(DepartmentName))]
        public string DepartmentName { get; set; }
    }
}

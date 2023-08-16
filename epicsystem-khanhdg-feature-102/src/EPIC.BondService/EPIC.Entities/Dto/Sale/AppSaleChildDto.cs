using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSaleChildDto
    {
        public int SaleId { get;set;}
        public string FullName { get; set; }
        public string ReferralCode { get; set; }
        public int? SaleType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AvatarImageUrl { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

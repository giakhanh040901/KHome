using EPIC.CompanySharesEntities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    public class ViewCpsSecondaryDto : DataEntities.CpsSecondary
    {
        public decimal? SoLuongCoPhanNamGiu { get; set; }
        public decimal? HanMucToiDa { get; set; }
        public decimal? SoLuongConLai { get; set; }
        //public ProductBondPrimaryDto ProductBondPrimary { get; set; }
        public DataEntities.CpsInfo CpsInfo { get; set; }
        public List<ViewCpsPolicyDto> Policies { get; set; }
        public BusinessCustomerBankDto businessCustomerBank { get; set; }
        //Danh sách ngân hàng của trading 
        public List<BusinessCustomerBankDto> ListBusinessCustomerBanks { get; set; }
        public string CpsName { get; set; }
    }

    public class ViewCpsPolicyDto : CpsPolicy
    {
        public int FakeId { get; set; }
        public List<ViewCpsPolicyDetailDto> Details { get; set; }
    }

    public class ViewCpsPolicyDetailDto : CpsPolicyDetail
    {
        public int FakeId { get; set; }
    }
}

using EPIC.BondEntities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ProductBondPrimary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondSecondary
{
    public class ViewProductBondSecondaryDto : DataEntities.BondSecondary
    {
        public decimal? SoLuongTraiPhieuNamGiu { get; set; }
        public decimal? HanMucToiDa { get; set; }
        public decimal? SoLuongConLai { get; set; }
        public ProductBondPrimaryDto ProductBondPrimary { get; set; }
        public DataEntities.BondInfo ProductBondInfo { get; set; }
        public List<ViewProductBondPolicyDto> Policies { get; set; }
        public BusinessCustomerBankDto businessCustomerBank { get; set; }
        //Danh sách ngân hàng của trading 
        public List<BusinessCustomerBankDto> ListBusinessCustomerBanks { get; set; }
        public string BondName { get; set; }
    }

    public class ViewProductBondPolicyDto : DataEntities.BondPolicy
    {
        public int FakeId { get; set; }
        public List<ViewProductBondPolicyDetailDto> Details { get; set; }
    }

    public class ViewProductBondPolicyDetailDto : BondPolicyDetail
    {
        public int FakeId { get; set; }
    }
}

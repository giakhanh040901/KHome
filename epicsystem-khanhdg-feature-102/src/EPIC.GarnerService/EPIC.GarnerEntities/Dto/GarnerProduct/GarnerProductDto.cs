using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;

namespace EPIC.GarnerEntities.Dto.GarnerProduct
{
    public class GarnerProductDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public int ProductType { get; set; }
        public string Icon { get; set; }
        public int IssuerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? MaxInvestor { get; set; }
        public int CountType { get; set; }
        public int? MinInvestDay { get; set; }
        public string GuaranteeOrganization { get; set; }
        public string IsPaymentGurantee { get; set; }
        public string IsCollateral { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string IsCheck { get; set; }
        public int Status { get; set; }

        #region tích luỹ cổ phần
        public int? CpsIssuerId { get; set; }
        public int? CpsDepositProviderId { get; set; }
        public int? CpsPeriod { get; set; }
        public string CpsPeriodUnit { get; set; }
        public decimal? CpsInterestRate { get; set; }
        public int? CpsInterestRateType { get; set; }
        public int? CpsInterestPeriod { get; set; }
        public string CpsInterestPeriodUnit { get; set; }
        public int? CpsNumberClosePer { get; set; }
        public decimal? CpsParValue { get; set; }
        public long? CpsQuantity { get; set; }
        public string CpsIsListing { get; set; }
        public string CpsIsAllowSBD { get; set; }
        public BusinessCustomerDto CpsIssuer { get; set; }
        public BusinessCustomerDto CpsDepositProvider { get; set; }
        #endregion

        #region tích luỹ invest
        public int? InvOwnerId { get; set; }
        public int? InvGeneralContractorId { get; set; }
        public decimal? InvTotalInvestmentDisplay { get; set; }
        public decimal? InvTotalInvestment { get; set; }
        public string InvArea { get; set; }
        public string InvLocationDescription { get; set; }
        public string InvLatitude { get; set; }
        public string InvLongitude { get; set; }
        public List<int> InvProductTypes{ get; set; }

        public BusinessCustomerDto InvOwner { get; set; }
        public BusinessCustomerDto InvGeneralContractor { get; set; }
        #endregion
        public List<GarnerProductTradingProviderDto> ProductTradingProvider { get; set; }
        public List<GarnerHistoryUpdate> HistoryUpdate { get; set; }
    }
}

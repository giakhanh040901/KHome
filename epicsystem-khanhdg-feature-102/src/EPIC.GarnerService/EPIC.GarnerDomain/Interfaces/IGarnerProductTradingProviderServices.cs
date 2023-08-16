using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerProductTradingProviderServices
    {
        void Add(CreateGarnerProductTradingProviderDto input);
        List<GarnerProductTradingProviderDto> FindAllByProduct(int productId);
        GarnerProductTradingProviderDto FindById(int id);
        void Update(UpdateGarnerProductTradingProviderDto input);
        List<BusinessCustomerBankDto> FindBankByTrading(int? distributionId = null, int? type = null);
    }
}

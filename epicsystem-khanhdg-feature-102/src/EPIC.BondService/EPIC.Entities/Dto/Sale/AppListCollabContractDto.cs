using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppListCollabContractDto
    {
        public int SaleTempId { get; set; }
        public string TradingProviderName { get; set; }
        public int TradingProviderId { get; set; }
        public List<AppCollabContractDto> CollabContracts { get; set; }
    }

    public class AppCollabContractDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}

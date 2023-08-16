using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreCollabContractTemp
{
    public class ViewCollabContractTempDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string FileUrl { get; set; }
        public string Status { get; set; }
    }
}

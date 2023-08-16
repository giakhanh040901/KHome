using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class TradingBankAccountOfDistributionDto
    {
        public long OrderId { get; set; }
        public int DistributionId { get; set; }
        public decimal TotalValue { get; set; }
        private string _contractCode;
        public string ContractCode
        {
            get => _contractCode;
            set => _contractCode = value?.Trim();
        }

        private string _tId;
        public string TId
        {
            get => _tId;
            set => _tId = value?.Trim();
        }
        private string _mId;
        public string MId
        {
            get => _mId;
            set => _mId = value?.Trim();
        }
    }
}

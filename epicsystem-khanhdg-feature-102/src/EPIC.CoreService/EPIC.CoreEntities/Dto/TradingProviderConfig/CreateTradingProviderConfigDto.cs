using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.TradingProviderConfig
{
    public class CreateTradingProviderConfigDto
    {
        private string _key;
        public string Key 
        { 
            get => _key; 
            set => _key = value?.Trim(); 
        }

        private string _value;
        public string Value
        {
            get => _value; 
            set => _value = value?.Trim(); 
        }
    }
}

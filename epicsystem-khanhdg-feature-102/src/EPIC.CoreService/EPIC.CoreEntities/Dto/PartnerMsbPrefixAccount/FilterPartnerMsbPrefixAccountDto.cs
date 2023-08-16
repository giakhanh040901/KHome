using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount
{
    public class FilterPartnerMsbPrefixAccountDto : PagingRequestBaseDto
    {
        private string _prefixMsb;
        [FromQuery(Name = "prefixMSB")]
        public string PrefixMsb
        {
            get => _prefixMsb;
            set => _prefixMsb = value?.Trim();
        }
    }
}

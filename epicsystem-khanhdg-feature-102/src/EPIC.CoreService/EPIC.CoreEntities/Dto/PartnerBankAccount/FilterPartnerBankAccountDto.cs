using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.PartnerBankAccount
{
    public class FilterPartnerBankAccountDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "partnerId")]
        public int PartnerId { get; set; }
    }
}

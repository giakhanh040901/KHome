using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicy
{
    /// <summary>
    /// Bộ lọc chính sách 
    /// </summary>
    public class FilterPolicyDto : PagingRequestBaseDto
    {
        private string _name;
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _code;
        [FromQuery(Name = "code")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }
    }
}

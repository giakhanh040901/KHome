using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.GarnerEntities.Dto.GarnerPolicyTemp
{
    /// <summary>
    /// Bộ lọc chính sách mẫu
    /// </summary>
    public class FilterPolicyTempDto : PagingRequestBaseDto
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
        private string _status;
        [FromQuery(Name = "status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }
    }
}

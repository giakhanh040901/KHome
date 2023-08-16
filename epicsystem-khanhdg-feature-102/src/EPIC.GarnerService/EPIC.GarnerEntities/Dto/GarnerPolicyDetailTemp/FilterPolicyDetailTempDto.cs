using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp
{
    /// <summary>
    /// Bộ lọc chính sách mẫu
    /// </summary>
    public class FilterPolicyDetailTempDto : PagingRequestBaseDto
    {
        private string _name;
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

    }
}

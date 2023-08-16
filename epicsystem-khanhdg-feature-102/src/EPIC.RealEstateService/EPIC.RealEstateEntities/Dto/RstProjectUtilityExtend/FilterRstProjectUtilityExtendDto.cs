using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using Microsoft.AspNetCore.Mvc;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend
{
    public class FilterRstProjectUtilityExtendDto : FilterRstProjectUtilityDto
    {
        private string _status;
        [FromQuery(Name = "Status")]
        public string Status
        {
            get => _status;
            set => _status = value?.Trim();
        }
    }
}

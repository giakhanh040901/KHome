using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreEntities.Dto.CallCenterConfig
{
    public class UpdateCallCenterConfigDto
    {
        public IEnumerable<CallCenterConfigDetailDto> Details { get; set; } = Enumerable.Empty<CallCenterConfigDetailDto>();
    }

    public class CallCenterConfigDetailDto
    {
        public int UserId { get; set; }
        public int SortOrder { get; set; }
    }
}

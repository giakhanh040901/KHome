using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class AppDeparmentDto
    {
		public int DepartmentId { get; set; }
		public int TradingProviderId { get; set; }
		public string DepartmentName { get; set; }
		public string DepartmentAddress { get; set; }
		public string Area { get; set; }
		public int? ParentId { get; set; }
		public int? DepartmentLevel { get; set; }
		public int? ManagerId { get; set; }
	}
}

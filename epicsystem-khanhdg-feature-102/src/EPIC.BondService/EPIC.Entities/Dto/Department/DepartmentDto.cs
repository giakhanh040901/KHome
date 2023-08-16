using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;

namespace EPIC.Entities.Dto.Department
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public int TradingProviderId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public int? ParentId { get; set; }
        public int? DepartmentLevel { get; set; }
        public int? ManagerId { get; set; }
        /// <summary>
        /// Id saler là quản lý con người
        /// </summary>
        public int? ManagerId2 { get; set; }

        /// <summary>
        /// Có phòng ban con hay không?
        /// </summary>
        public bool HasDepartmentChild { get; set; }
        public InvestorDto Manager { get; set; }
        public InvestorDto Manager2 { get; set; }
        public BusinessCustomerDto ManagerBusinessCustomer { get; set; }
    }
}

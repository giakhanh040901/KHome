namespace EPIC.Entities.Dto.Department
{
    public class UpdateDepartmentDto : BaseDepartmentDto
    {
        public int DepartmentId { get; set; }
        public int? DepartmentParentId { get; set; }
    }
}

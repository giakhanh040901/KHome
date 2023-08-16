namespace EPIC.Entities.Dto.Department
{
    public class CreateDepartmentDto : BaseDepartmentDto
    {
        public int? ParentId { get; set; }
    }
}

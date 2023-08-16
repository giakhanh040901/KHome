using EPIC.Utils.Validation;

namespace EPIC.CompanySharesEntities.Dto.Policy
{
    public class UpdatePolicyDto : CreatePolicyDto
    {
        public int CPSPolicyId { get; set; }
        [StringRange(AllowableValues = new string[] { Utils.Status.ChoDuyet, Utils.Status.DaDuyet, Utils.Status.TuChoiDuyet }, ErrorMessage = "Vui lòng chọn Trạng thái")]
        public string Status { get; set; }
    }
}

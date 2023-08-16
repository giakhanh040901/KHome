using EPIC.FillContractData.Dto;

namespace EPIC.RealEstateEntities.Dto.RstOrderContractFile
{
    public class RstReplaceTextContractFileDto
    {
        public int ProductItemId { get; set; }
        public int ProjectId { get; set; }
        public int OrderSellingPolicyId { get; set; }
        /// <summary>
        ///Có bao gồm Chi phí bảo trì hay không?
        /// </summary>
        public bool? IsHaveMaintenanceCost { get; set; }
    }
}

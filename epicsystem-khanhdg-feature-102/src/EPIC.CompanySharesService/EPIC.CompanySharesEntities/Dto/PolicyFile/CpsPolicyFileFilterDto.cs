using Microsoft.AspNetCore.Mvc;

namespace EPIC.CompanySharesEntities.Dto.PolicyFile
{
    public class CpsPolicyFileFilterDto
    {
        [FromQuery(Name = "secondaryId")]
        public int SecondaryId { get; set; }
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }

        private string _keyword;
        [FromQuery(Name = "keyword")]
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }
    }
}

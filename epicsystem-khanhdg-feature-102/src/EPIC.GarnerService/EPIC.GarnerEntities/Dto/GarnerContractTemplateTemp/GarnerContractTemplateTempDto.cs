using System;

namespace EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp
{
    public class GarnerContractTemplateTempDto
    {
        public int Id { get; set; }

        [Obsolete("bỏ")]
        public string Code { get; set; }
        public string Name { get; set; }

        [Obsolete("bỏ")]
        public string ContractTempUrl { get; set; }

        [Obsolete("bỏ")]
        public int PolicyTempId { get; set; }

        [Obsolete("bỏ")]
        public string Type { get; set; }
        public int ContractType { get; set; }

        [Obsolete("bỏ")]
        public string DisplayType { get; set; }
        public string Status { get; set; }
        public int ContractSource { get; set; }
        public string FileInvestor { get; set; }
        public string FileBusinessCustomer { get; set; }
        public string Description { get; set; }
    }
}

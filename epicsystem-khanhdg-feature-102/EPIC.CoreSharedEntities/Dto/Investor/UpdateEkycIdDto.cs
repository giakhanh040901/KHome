using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class UpdateEkycIdDto
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string IdNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? IssueExpDate { get; set; }
        public string Issuer { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string PlaceOfResidence { get; set; }
        public string Nationality { get; set; }
        public string IdType { get; set; }
        public string FrontImage { get; set; }
        public string BackImage { get; set; }
    }
}

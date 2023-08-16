using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class SCInvestorIdentificationDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdType { get; set; }
        public string IdNo { get; set; }
        public string Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string PersonalIdentification { get; set; }
        public string IdIssuer { get; set; }
        public DateTime? IdDate { get; set; }
        public DateTime? IdExpiredDate { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string PlaceOfResidence { get; set; }
        public string Sex { get; set; }
        public string ContractAddress { get; set; }
    }
}

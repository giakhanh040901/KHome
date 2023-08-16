using EPIC.CoreSharedEntities.Dto.Investor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderCoOwner
{
    public class RstOrderCoOwnerDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? InvestorIdenId { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdType { get; set; }
        public string IdFrontImageUrl { get; set; }
        public string IdBackImageUrl { get; set; }
        public string IdNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfOrigin { get; set; }
    }
}

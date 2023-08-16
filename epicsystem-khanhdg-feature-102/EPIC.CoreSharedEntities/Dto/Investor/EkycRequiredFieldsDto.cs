using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class EkycRequiredFieldsDto
    {
        public string Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string IdNo { get; set; }
        public DateTime? IdDate { get; set; }
        public DateTime? IdExpiredDate { get; set; }
        public string IdIssuer { get; set; }
        public string PlaceOfOrigin { get; set; }
        public string PlaceOfResidence { get; set; }
        public string IdFrontImageUrl { get; set; }
    }
}

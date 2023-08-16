using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class ConfirmIdentificationEkycDto
    {
        public int IdentificationId { get; set; }
        public bool IsConfirmed { get; set; }
        public List<string> IncorrectFields { get; set; }

        [StringRange(AllowableValues = new string[] { null, Genders.FEMALE, Genders.MALE })]
        public string Sex { get; set; }
    }
}

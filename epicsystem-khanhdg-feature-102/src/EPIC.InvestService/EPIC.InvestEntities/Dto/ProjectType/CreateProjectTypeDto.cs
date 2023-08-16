using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectType
{
    public class CreateProjectTypeDto
    {
        [IntegerRange(AllowableValues = new int[] { InvestProjectTypes.NHA_RIENG, InvestProjectTypes.CAN_HO_CHUNG_CU, InvestProjectTypes.NHA_PHO_BIET_THU_DU_AN, InvestProjectTypes.DAT_NEN_DU_AN, InvestProjectTypes.BIET_THU_NGHI_DUONG, InvestProjectTypes.CONDOTEL, InvestProjectTypes.SHOPHOUSE, InvestProjectTypes.OFFICETEL}, ErrorMessage = "Vui lòng chọn 1 trong các loại hình dự án sau")]
        public int? Type { get; set; }
    }
}

using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Core;

namespace EPIC.CoreEntities.Dto.InvestorRegistorLog
{
    public class CreateInvestorRegisterLogDto
    {
        public string Phone { get; set; }

        [Required(ErrorMessage = "Loại tài khoản log không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] 
        { InvestorRegisterLogTypes.RegisterNow, InvestorRegisterLogTypes.OtpSent, 
            InvestorRegisterLogTypes.SuccessfulOtp, InvestorRegisterLogTypes.SuccessfulIdentification, 
            InvestorRegisterLogTypes.StartEkyc, InvestorRegisterLogTypes.SuccessfulEkyc, InvestorRegisterLogTypes.SuccessfulBank, 
            InvestorRegisterLogTypes.CompleteRegistration },
            ErrorMessage = "Vui lòng chọn 1 trong các loại trạng thái sổ đỏ sau")]
        public int Type { get; set; }
    }
}

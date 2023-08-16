using EPIC.Entities.Dto.Investor;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class UploadAvatarDto : AppUploadAvatarDto
    {
        public int InvestorId { get; set; }

        public bool IsTemp { get; set; }
    }
}

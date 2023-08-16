using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Auth
{
    public class OtpDto
    {
        public string Otp { get; set; }
        public DateTime Exp { get; set; }
    }
}

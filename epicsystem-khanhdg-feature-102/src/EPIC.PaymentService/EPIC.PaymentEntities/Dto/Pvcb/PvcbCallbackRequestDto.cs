using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.Pvcb
{
    public class PvcbCallbackRequestDto
    {
        public string Data { get; set; }
        public string Signature { get; set; }
    }
}

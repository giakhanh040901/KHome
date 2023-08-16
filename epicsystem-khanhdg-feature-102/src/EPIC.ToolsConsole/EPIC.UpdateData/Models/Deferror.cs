using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class Deferror
    {
        public decimal ErrCode { get; set; }
        public string ErrName { get; set; }
        public string ErrMessage { get; set; }
    }
}

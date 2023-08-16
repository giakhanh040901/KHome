using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class Defcode
    {
        public string Cdtype { get; set; }
        public string Cdname { get; set; }
        public string Cdvalue { get; set; }
        public string Cdvaluename { get; set; }
        public decimal? Cdposition { get; set; }
        public string Cddesc { get; set; }
    }
}

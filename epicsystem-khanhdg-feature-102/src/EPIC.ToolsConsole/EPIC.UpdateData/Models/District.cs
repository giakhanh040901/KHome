﻿using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class District
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string CodeName { get; set; }
        public string ProvinceCode { get; set; }
        public decimal? AdministrativeUnitId { get; set; }
    }
}

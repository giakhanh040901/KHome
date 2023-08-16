using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ProductBondPrimary
{
    public class UpdateProductBondPrimaryDto : CreateProductBondPrimaryDto
    {
        public int BondPrimaryId { get; set; }

        //[StringRange(AllowableValues = new string[] { StatusPrimary.CHO_DUYET, StatusPrimary.HOAT_DONG, StatusPrimary.NGUNG_HOAT_DONG, StatusPrimary.NHAP })]
        //public string Status { get; set; }
    }
}

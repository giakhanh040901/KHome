using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.BondEntities.Dto.AppOrder
{
    public class DanhSachChiTraFitlerDto
    {
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }

        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }

        [FromQuery(Name = "contractCode")]
        public string ContractCode { get; set; }

        [FromQuery(Name = "taxCode")]
        public string TaxCode { get; set; }

        [FromQuery(Name = "phone")]
        public string Phone { get; set; }

        [FromQuery(Name = "ngayChiTra")]
        public DateTime? NgayChiTra { get; set; }
    }
}

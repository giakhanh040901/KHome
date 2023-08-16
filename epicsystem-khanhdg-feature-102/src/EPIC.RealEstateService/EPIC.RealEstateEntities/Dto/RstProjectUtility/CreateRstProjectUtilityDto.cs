using EPIC.Utils.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtility
{
    public class CreateRstProjectUtilityDto
    {
        public int ProjectId { get; set; }
        public List<ViewCreateRstProjectUtilityDto> ProjectUtilites { get; set; }
    }
    public class ViewCreateRstProjectUtilityDto 
    {
        /// <summary>
        /// Id tiện ích
        /// </summary>
        public int UtilityId { get; set; }
        /// <summary>
        /// Nổi bật
        /// </summary>
        public string IsHighlight { get; set; }
    }

}

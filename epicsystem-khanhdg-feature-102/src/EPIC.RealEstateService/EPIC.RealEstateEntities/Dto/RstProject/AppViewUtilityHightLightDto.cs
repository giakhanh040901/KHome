using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppViewUtilityDto
    {
        /// <summary>
        /// Tên tiện ích
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Loại tiện ích 1 Nội khu 2 ngoại khu
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Tiện ích nooit bật Y/N
        /// </summary>
        public string IsHighlight { get; set; }
    }
}

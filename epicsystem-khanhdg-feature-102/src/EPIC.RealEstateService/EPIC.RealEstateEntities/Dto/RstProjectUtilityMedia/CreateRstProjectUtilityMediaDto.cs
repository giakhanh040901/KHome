using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia
{
    public class CreateRstProjectUtilityMediaDto
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class AppDeleteBankDto
    {
        [Range(1, int.MaxValue)]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}

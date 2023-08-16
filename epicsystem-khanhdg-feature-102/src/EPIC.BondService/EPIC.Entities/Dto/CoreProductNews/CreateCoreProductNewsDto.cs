using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreProductNews
{
    public class CreateCoreProductNewsDto
    {
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Feature { get; set; }
        public int Location { get; set; }
    }
}

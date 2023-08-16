using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.JuridicalFile
{
    public class JuridicalFileDto
    {
        public int JuridicalFileId { get; set; }
        public int ProductBondId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}

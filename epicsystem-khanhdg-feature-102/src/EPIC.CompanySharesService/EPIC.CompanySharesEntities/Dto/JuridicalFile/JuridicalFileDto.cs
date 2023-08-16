using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.JuridicalFile
{
    public class JuridicalFileDto
    {
        public int JuridicalFileId { get; set; }
        public int CpsId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}

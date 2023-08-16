using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectJuridicalFile
{
    public class CreateProjectJuridicalFileDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}

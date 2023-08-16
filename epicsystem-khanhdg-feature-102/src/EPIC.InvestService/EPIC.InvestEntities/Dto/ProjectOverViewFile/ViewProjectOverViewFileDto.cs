using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectOverViewFile
{
    public class ViewProjectOverViewFileDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int SortOrder { get; set; }
    }
}

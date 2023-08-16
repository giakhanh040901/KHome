using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectOverViewFile
{
    public class CreateProjectOverviewFileDto
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        private string _title;
        public string Title 
        { 
            get => _title;
            set => _title = value?.Trim(); 
        }

        private string _url;
        public string Url 
        { 
            get => _url; 
            set => _url = value?.Trim(); 
        }
        public int SortOrder { get; set; }
    }
}

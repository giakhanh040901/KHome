using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectOverviewOrg
{
    public class CreateProjectOverviewOrgDto
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _orgCode;
        public string OrgCode
        {
            get => _orgCode;
            set => _orgCode = value?.Trim();
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => _icon = value?.Trim();
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => _url = value?.Trim();
        }
    }
}

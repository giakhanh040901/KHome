using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    public class CreateCpsSecondaryOverviewOrgDto
    {
        public int Id { get; set; }
        public int SecondaryId { get; set; }

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

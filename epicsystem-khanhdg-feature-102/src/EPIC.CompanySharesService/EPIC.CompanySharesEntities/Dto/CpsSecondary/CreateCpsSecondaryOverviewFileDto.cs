using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    public class CreateCpsSecondaryOverviewFileDto
    {
        public int Id { get; set; }
        public int SecondaryId { get; set; }
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
    }
}

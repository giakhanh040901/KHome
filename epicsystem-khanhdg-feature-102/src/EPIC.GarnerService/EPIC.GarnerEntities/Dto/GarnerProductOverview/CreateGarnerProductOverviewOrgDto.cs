using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class CreateGarnerProductOverviewOrgDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Id phân phối sản phẩm không được bỏ trống")]
        public int DistributionId { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _code;
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _role;
        public string Role
        {
            get => _role;
            set => _role = value?.Trim();
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

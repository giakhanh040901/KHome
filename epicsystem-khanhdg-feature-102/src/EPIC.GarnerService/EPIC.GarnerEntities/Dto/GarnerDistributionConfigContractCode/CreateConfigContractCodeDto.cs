using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode
{
    public class CreateConfigContractCodeDto
    {
        private string _name;
        [MaxLength(128)]
        [Required(ErrorMessage = "Tên không được bỏ trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }

        public List<CreateConfigContractCodeDetailDto> ConfigContractCodeDetails { get; set; }
    }
}

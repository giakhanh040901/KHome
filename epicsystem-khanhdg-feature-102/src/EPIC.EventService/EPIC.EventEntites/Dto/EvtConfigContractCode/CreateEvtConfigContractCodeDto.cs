using EPIC.EventEntites.Dto.EvtConfigContractCodeDetail;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtConfigContractCode
{
    public class CreateEvtConfigContractCodeDto
    {
        private string _name;
        [MaxLength(128)]
        [Required(ErrorMessage = "Tên không được bỏ trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Mô tả
        /// </summary>
        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }

        public List<CreateEvtConfigContractCodeDetailDto> ConfigContractCodeDetails { get; set; }
    }
}

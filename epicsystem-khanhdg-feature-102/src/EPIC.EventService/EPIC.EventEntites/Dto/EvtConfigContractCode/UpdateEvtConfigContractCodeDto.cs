using EPIC.EventEntites.Dto.EvtConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtConfigContractCode
{
    public class UpdateEvtConfigContractCodeDto
    {
        // Tên cấu trúc
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

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

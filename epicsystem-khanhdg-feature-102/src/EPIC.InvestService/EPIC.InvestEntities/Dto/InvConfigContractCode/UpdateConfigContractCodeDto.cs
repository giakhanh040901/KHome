using EPIC.InvestEntities.Dto.InvConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvConfigContractCode
{
    public class UpdateConfigContractCodeDto
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        public int Id { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
        public List<CreateConfigContractCodeDetailDto> ConfigContractCodeDetails { get; set; }
    }
}

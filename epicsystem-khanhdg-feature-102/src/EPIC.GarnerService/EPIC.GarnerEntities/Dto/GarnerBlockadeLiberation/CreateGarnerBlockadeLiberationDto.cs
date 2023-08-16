using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation
{
    public class CreateGarnerBlockadeLiberationDto
    {
        [IntegerRange(AllowableValues = new int[] { BlockadeLiberationTypes.OTHER, BlockadeLiberationTypes.PLEDGE, BlockadeLiberationTypes.ADVANCE_CAPITAL })]
        public int Type { get; set; }

        private string _blockadeDescription;
        [StringLength(256, ErrorMessage = "Ghi chú phong toả không được dài hơn 256 ký tự")]
        public string BlockadeDescription 
        { 
            get => _blockadeDescription; 
            set => _blockadeDescription = value?.Trim(); 
        }

        public DateTime? BlockadeDate { get; set; }
        public long OrderId { get; set; }
    }
}

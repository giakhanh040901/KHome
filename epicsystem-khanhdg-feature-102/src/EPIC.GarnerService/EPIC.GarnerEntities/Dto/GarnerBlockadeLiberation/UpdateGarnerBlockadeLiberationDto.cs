using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation
{
    public class UpdateGarnerBlockadeLiberationDto : CreateGarnerBlockadeLiberationDto
    {
        public int Id { get; set; }

        private string _liberationDescription;
        [StringLength(256, ErrorMessage = "Ghi chú giải toả không được dài hơn 256 ký tự")]
        public string LiberationDescription 
        { 
            get => _liberationDescription; 
            set => _liberationDescription =value?.Trim(); 
        }
        public DateTime? LiberationDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class UpdateGarnerDistributionDto : CreateGarnerDistributionDto
    {
        public int Id { get; set; }

        private string _image;
        /// <summary>
        /// Hình ảnh của phân phối
        /// </summary>
        public string Image 
        { 
            get => _image; 
            set => _image = value?.Trim(); 
        }
    }
}

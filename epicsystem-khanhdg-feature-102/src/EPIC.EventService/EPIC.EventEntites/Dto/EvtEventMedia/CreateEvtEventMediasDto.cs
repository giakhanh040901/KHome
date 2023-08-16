using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEventMedia
{
    public class CreateEvtEventMediasDto
    {
        private string _location;
        public string Location
        {
            get => _location;
            set => _location = value?.Trim();
        }

        public IEnumerable<CreateEvtEventMediaDto> EventMedias { get; set; }
    }
}

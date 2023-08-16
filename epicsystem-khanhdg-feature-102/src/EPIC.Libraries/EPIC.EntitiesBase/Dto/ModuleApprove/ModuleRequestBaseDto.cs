using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Dto.ModuleApprove
{
    public class ModuleRequestBaseDto
    {
        public int Id { get; set; }
        public int? UserApproveId { get; set; }

        private string _requestNote;
        public string RequestNote 
        { 
            get => _requestNote;
            set => _requestNote = value?.Trim(); 
        }

        private string _summary;
        public string Summary 
        { 
            get => _summary;
            set => _summary = value?.Trim();
        }
    }
}

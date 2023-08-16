using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Dto.ModuleApprove
{
    public class ModuleApproveBaseDto
    {
        public int Id { get; set; }

        private string _approveNote;
        public string ApproveNote 
        { 
            get => _approveNote; 
            set => _approveNote = value?.Trim(); 
        }
    }
}

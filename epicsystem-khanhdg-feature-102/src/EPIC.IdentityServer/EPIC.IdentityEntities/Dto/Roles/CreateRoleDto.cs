using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.Roles
{
    public class CreateRoleDto
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
    }
}

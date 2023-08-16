using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class UpdateUserDto : CreateUserDtoBase
    {
        public int UserId { get; set; }
    }
}

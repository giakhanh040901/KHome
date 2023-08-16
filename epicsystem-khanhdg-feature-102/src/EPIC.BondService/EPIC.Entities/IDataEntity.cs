using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities
{
    public interface IFullAudited : ICreatedBy, IModifiedBy, ISoftDelted
    {
    }

    public interface ISoftDelted
    {
        public string Deleted { get; set; }
    }

    public interface ICreatedBy
    {
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public interface IModifiedBy
    {
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}

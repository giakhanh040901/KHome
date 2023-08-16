using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IProvinceServices
    {
        public List<Province> GetListProvince(string keyword);

        public List<District> GetListDistrict(string keyword, string provinceCode);

        public List<Ward> GetListWard(string keyword, string districtCode);
    }
}

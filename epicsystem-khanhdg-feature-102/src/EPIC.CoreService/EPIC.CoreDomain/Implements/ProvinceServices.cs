using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class ProvinceServices : IProvinceServices
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;
        private readonly ProvinceRepository _provinceRepository;

        public ProvinceServices(ILogger<ProvinceServices> logger, DatabaseOptions databaseOptions)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _provinceRepository = new ProvinceRepository(_connectionString, _logger);
        }

        public List<District> GetListDistrict(string keyword, string provinceCode)
        {
            return _provinceRepository.GetListDistrict(keyword, provinceCode);
        }

        public List<Province> GetListProvince(string keyword)
        {
            return _provinceRepository.GetListProvince(keyword);
        }

        public List<Ward> GetListWard(string keyword, string districtCode)
        {
            return _provinceRepository.GetListWard(keyword, districtCode);
        }
    }
}

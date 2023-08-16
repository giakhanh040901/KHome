using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessLicenseFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IBusinessLicenseFileServices
    {
        BusinessLicenseFile Add(CreateBusinessLicenseFileDto input);
        BusinessLicenseFile AddTemp(CreateBusinessLicenseFileTempDto input);
        int Delete(int id);
        BusinessLicenseFileDto FindById(int id);
        List<BusinessLicenseFileDto> FindAll(int? businessCustomerId, int? businessCustomerTempId);
        int Update(UpdateBusinessLicenseFileDto input);
        int UpdateTemp(UpdateBusinessLicenseFileTempDto input);
    }
}

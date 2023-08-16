using EPIC.Entities.Dto.ContractData;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ISignPdfServices
    {
        ExportResultDto SignPdf(SignPdfDto dto);
    }
}

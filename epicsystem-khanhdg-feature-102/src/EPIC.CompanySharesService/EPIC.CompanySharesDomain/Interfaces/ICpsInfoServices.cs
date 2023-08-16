using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CpsInfo;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.CoreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface ICpsInfoServices
    {
        #region cms
        PagingResult<CpsInfoDto> FindAll(int pageSize, int pageNumber, string keyword, string status, string isCheck, DateTime? issueDate, DateTime? dueDate);
        CpsInfoDto FindById(int id);
        CpsInfo Add(CreateCpsInfoDto input);
        int Update(int id, UpdateCpsInfoDto input);
        int Delete(int id);
        DividendCpsDto FindDividendById(int id);
        void Request(RequestStatusDto input);
        void Approve(ApproveStatusDto input);
        void Check(CheckStatusDto input);
        void Cancel(CancelStatusDto input);
        void CloseOpen(int id);
        #endregion
    }
}

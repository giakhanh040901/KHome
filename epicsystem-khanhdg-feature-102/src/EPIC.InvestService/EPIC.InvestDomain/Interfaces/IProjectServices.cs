using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IProjectServices
    {
        PagingResult<ProjectDto> FindAll(FilterInvestProjectDto input);
        ProjectDto FindById(int id);
        ProjectDto Add(CreateProjectDto input);
        void Update(UpdateProjectDto input);
        int Delete(int id);
        void Request(CreateInvestRequestDto input);
        void Approve(InvestApproveDto input);
        void Check(InvestCheckDto input);
        void Cancel(InvestCancelDto input);
        void CloseOpen(int id);
        PagingResult<ProjectDto> FindAllTradingProvider(int pageSize, int pageNumber, string keyword, int? status);
    }
}

using EPIC.CoreEntities.Dto.InvestorSearch;
using EPIC.DataAccess.Models;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IInvestorSearchService
    {
        PagingResult<object> Search(FilterInvestorSearchDto input);
        void AddHistorySearch(InvestorSearchHistoryCreateDto input);
        void DeleteHistorySearch();
        PagingResult<object> HistorySearch(FilterInvestorSearchDto input);
    }
}

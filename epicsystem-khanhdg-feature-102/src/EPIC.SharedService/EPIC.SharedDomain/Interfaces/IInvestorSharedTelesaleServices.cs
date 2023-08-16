using EPIC.SharedEntities.Dto.InvestorTelesale;

namespace EPIC.SharedDomain.Interfaces
{
    public interface IInvestorSharedTelesaleServices
    {
        /// <summary>
        /// Tìm danh sách hợp đồng đang đầu tư của invest + garner theo số giấy tờ của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        InvestorTelesaleDto FindActiveInfo(FilterInvestorTelesaleDto input);

        /// <summary>
        /// Thông tin các khoản đầu tư của khách hàng tìm theo thông tin giấy tờ
        /// </summary>
        /// <param name="idNo"></param>
        /// <returns></returns>
        object FindInvestInfo(string idNo);
    }
}

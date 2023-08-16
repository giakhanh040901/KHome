using EPIC.InvestEntities.Dto.InvestRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestRatingServices
    {
        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        void Add(CreateInvestRatingDto input);
        /// <summary>
        /// Tìm hợp đồng mới nhất để đánh giá
        /// </summary>
        /// <returns></returns>
        InvestRatingDto FindLastOrder();
    }
}

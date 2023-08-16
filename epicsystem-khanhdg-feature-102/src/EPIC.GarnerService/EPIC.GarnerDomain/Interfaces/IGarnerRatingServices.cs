using EPIC.GarnerEntities.Dto;
using EPIC.GarnerEntities.Dto.GarnerRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerRatingServices
    {
        /// <summary>
        /// Tìm hợp đồng mới nhất để đánh giá
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        GarnerRatingDto FindLastOrder();

        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        void Add(CreateGarnerRatingDto input);
    }
}

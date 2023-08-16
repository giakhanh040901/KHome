using EPIC.RealEstateEntities.Dto.RstCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstCartServices
    {
        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        /// <param name="input"></param>
        void Add(CreateRstCartDto input);

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Xem danh sách trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        List<AppRstCartDto> GetAllCart();
    }
}

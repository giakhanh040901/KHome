using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerOrderPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerOrderPaymentServices
    {
        /// <summary>
        /// Thêm thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerOrderPaymentDto Add(CreateGarnerOrderPaymentDto input);

        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerOrderPayment Update(UpdateGarnerOrderPaymentDto input);

        /// <summary>
        /// Xóa thanh toán
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Xem chi tiết thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GarnerOrderPaymentDto FindById(int id);

        /// <summary>
        /// Danh sách thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        PagingResult<GarnerOrderPaymentDto> FindAll(FilterOrderPaymentDto input);

        /// <summary>
        /// Phê duyệt / Hủy duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        Task ApprovePayment(long id, int status);
        GarnerOrderPayment AddOrderPaymentCommon(CreateGarnerOrderPaymentDto input);
    }
}

using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerProductServices
    {
        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        /// <param name="input"></param>
        void Add(CreateGarnerProductDto input);

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        /// <param name="input"></param>
        void Update(UpdateGarnerProductDto input);

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id, string summary);

        /// <summary>
        /// Xem danh sách có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<GarnerProductDto> FindAll(FilterGarnerProductDto input);

        /// <summary>
        /// lấy danh sách tổ chức phát hành không trùng nhau
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<BusinessCustomerDto> DistinctIssuer(FilterGarnerProductDto input);

        /// <summary>
        /// Xem theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GarnerProductDto FindById(int id);

        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy cho đại lý
        /// </summary>
        /// <returns></returns>
        List<GarnerProductByTradingProviderDto> GetListProductByTradingProvider();

        /// <summary>
        /// thay đổi trạng thái status đóng và hoạt động 
        /// </summary>
        /// <param name="productId"></param>
        void ChangeStatus(int productId);

        /// <summary>
        /// Tìm lịch sử update sản phẩm phân phối đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<GarnerHistoryUpdate> FindHistoryUpdateById(int id);

        #region Quy trình duyệt
        /// <summary>
        /// Trình duyệt
        /// </summary>
        /// <param name="input"></param>
        void Request(CreateGarnerRequestDto input);

        /// <summary>
        /// Duyệt
        /// </summary>
        /// <param name="input"></param>
        void Approve(GarnerApproveDto input);

        /// <summary>
        /// Hủy duyệt
        /// </summary>
        /// <param name="input"></param>
        void Cancel(GarnerCancelDto input);

        /// <summary>
        /// Kiểm tra
        /// </summary>
        /// <param name="input"></param>
        void Check(GarnerCheckDto input);

        /// <summary>
        /// tìm kiếm list file theo productId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        List<GarnerProductFileDto> FindAllListByProduct(int productId, int? documentType);

        /// <summary>
        /// update ProductFile
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerProductFile UpdateProductFile(CreateGarnerProductFileDto input);

        /// <summary>
        /// thêm mới ProductFile
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerProductFile AddProductFile(int productId, CreateGarnerProductFileDto input);

        /// <summary>
        /// xóa file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GarnerProductFile DeletedProductFile(int id);
        #endregion
    }
}

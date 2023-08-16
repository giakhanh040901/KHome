using EPIC.CoreEntities.Dto.BusinessCustomer;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.DigitalSign;
using EPIC.Entities.Dto.ManagerInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IBusinessCustomerServices
    {
        BusinessCustomerTempDto FindById(int id);
        PagingResult<BusinessCustomerTemp> FindAll(FilterBusinessCustomerTempDto input);
        BusinessCustomerTemp Add(CreateBusinessCustomerTempDto input);
        BusinessCustomerTemp Update(int id, UpdateBusinessCustomerTempDto input);
        void Request(RequestBusinessCustomerDto input);

        int Approve(ApproveBusinessCustomerDto input);

        void Check(CheckBusinessCustomerDto input);

        void Cancel(CancelBusinessCustomerDto input);

        BusinessCustomerTemp BusinessCustomerUpdate(CreateBusinessCustomerTempDto input);
        BusinessCustomer FindBusinessCustomerByTaxCode(string taxCode);
        BusinessCustomerDto FindBusinessCustomerById(int id);

        /// <summary>
        /// Tìm kiếm thông tin ngân hàng theo TaxCode
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        PagingResult<BusinessCustomerDto> FindAllBusinessCustomerByTaxCode(int pageSize, int pageNumber, string keyword);
        PagingResult<BusinessCustomerDto> FindAllBusinessCustomer(FilterBusinessCustomerDto input);

        BusinessCustomerBank FindBusinessCusBankById(int id);
        PagingResult<BusinessCustomerBank> FindAllBusinessCusBank(int businessCustomerId, int pageSize, int pageNumber, string keyword);
        BusinessCustomerBankTemp BusinessCustomerBankAdd(CreateBusinessCustomerBankTempDto input);
        BusinessCustomerBankTemp BusinessCustomerBankUpdate(int id, UpdateBusinessCustomerBankTempDto input);

        /// <summary>
        /// Kích hoạt, hủy kích hoạt ngân hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        int ActiveBusinessCustomerBank(int id, bool isActive);

        int BusinessCustomerDelete(int id);
        int BusinessCustomerBankDelete(int id);

        /// <summary>
        /// Set ngân hàng mặc định
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        int BankSetDefault(BusinessCustomerBankDefault input);

        int BankTempSetDefault(BusinessCustomerBankTempDefault input);
        /// <summary>
        /// Cập nhật thông tin ngân hàng ở bảng Tạm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int BusinessCustomerBankTempUpdate(int id, UpdateBusinessCustomerBankTempDto input);

        /// <summary>
        /// Lấy id ngân hàng doanh nghiệp từ bảng tạm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BusinessCustomerBankTemp FindBusinessCustomerBankTempById(int id);

        /// <summary>
        /// Lấy danh sách ngân hàng tạm theo doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<BusinessCustomerBankTempDto> FindBankTempByBusinessCustomer(int id);

        /// <summary>
        /// So sánh sự thay đổi của bảng thật và Temp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BusinessCustomerCheckUpdateDto BusinessCustomerCheckUpdate(int id);
        
        /// <summary>
        /// Cập nhập dữ liệu chữ ký số của khách hàng doanh nghiệp bảng tạm
        /// </summary>
        /// <param name="businessCustomerTempId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        int UpdateDigitalSignTemp(int? businessCustomerTempId, DigitalSignDto input);


        /// <summary>
        /// Lấy dữ liệu thông tin chữ ký số của khách hàng doanh nghiệp bảng tạm
        /// </summary>
        /// <param name="businessCustomerTempId"></param>
        /// <returns></returns>
        DigitalSignDto GetDigitalSignTemp(int? businessCustomerTempId);


        /// <summary>
        /// Lấy dữ liệu thông tin chữ ký số của khách hàng doanh nghiệp bảng chính
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <returns></returns>
        DigitalSignDto GetDigitalSign(int? businessCustomerId);

        /// <summary>
        /// Cập nhập dữ liệu khách hàng doanh nghiệp ở bảng chính
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        DigitalSign UpdateDigitalSign(int? businessCustomerId, DigitalSignDto input);
    }
}

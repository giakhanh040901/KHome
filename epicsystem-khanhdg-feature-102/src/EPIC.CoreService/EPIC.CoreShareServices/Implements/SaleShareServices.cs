using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Department;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedServices.Implements
{
    public class SaleShareServices : ISaleShareServices
    {
        private readonly ILogger<SaleShareServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly SaleRepository _saleRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DepartmentRepository _departmentRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly IMapper _mapper;

        public SaleShareServices(
            ILogger<SaleShareServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _mapper = mapper;
        }

        public void DeQuyDepartmentSaleChild(int tradingProviderId, int departmentId, bool? isInvestor, ref decimal result, ref decimal resultMonth, ref List<DepartmentSaleDto> saleChildren)
        {
            //Danh sách Sale trong phòng ban đấy
            var departmentSale = _departmentRepository.FindAllListSale(-1, 0, null, null, departmentId, tradingProviderId, isInvestor).Items.ToList();

            saleChildren.AddRange(departmentSale);

            //Cộng tổng số Sale
            result += departmentSale.Count();

            //Danh sách số sale mới trong tháng
            var saleNewMonth = from sale1 in departmentSale where sale1.CreatedDate.ToString("yyyy MMMM") == DateTime.Now.ToString("yyyy MMMM") select sale1;
            resultMonth += saleNewMonth.Count();

            ///Đệ quy với các phòng ban cấp dưới
            var departmentChild = _departmentRepository.FindAllListChild(departmentId, tradingProviderId);
            foreach (var item in departmentChild)
            {
                DeQuyDepartmentSaleChild(tradingProviderId, item.DepartmentId, isInvestor, ref result, ref resultMonth, ref saleChildren);
            }
        }

        //Đệ quy lấy thông tin phòng ban cấp dưới
        public void DeQuyDepartmentChild(int tradingProviderId, int departmentId, ref List<Department> listDepartment)
        {
            ///Đệ quy với các phòng ban cấp dưới
            var departmentChild = _departmentRepository.FindAllListChild(departmentId, tradingProviderId);
            listDepartment.AddRange(departmentChild);
            foreach (var item in departmentChild)
            {
                DeQuyDepartmentChild(tradingProviderId, item.DepartmentId, ref listDepartment);
            }
        }

        /// <summary>
        /// Đệ quy tìm list phòng ban cấp trên
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="parentId"></param>
        /// <param name="listDepartmentParent"></param>
        public void DeQuyDepartmentParent(int tradingProviderId, int departmentId, ref List<Department> listDepartmentParent)
        {
            var departmentParent = _departmentRepository.FindById(departmentId, tradingProviderId);
            listDepartmentParent.Add(departmentParent);

            ///Đệ quy tìm list phòng ban cấp trên
            if (departmentParent != null && departmentParent.ParentId != null)
            {
                DeQuyDepartmentParent(tradingProviderId, departmentParent.ParentId ?? 0, ref listDepartmentParent);
            }
        }
    }
}

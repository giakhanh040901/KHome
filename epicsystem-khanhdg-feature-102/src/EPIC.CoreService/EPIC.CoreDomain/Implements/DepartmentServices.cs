using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.CoreRepositories;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Sale;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace EPIC.CoreDomain.Implements
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly ISaleShareServices _saleShareServices;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly SaleRepository _saleRepository;

        public DepartmentServices(
            ILogger<DepartmentServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            ISaleShareServices saleShareServices,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _mapper = mapper;
            _saleShareServices = saleShareServices;
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
        }

        public DepartmentDto Create(CreateDepartmentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var department = _departmentRepository.Add(new Department
            {
                DepartmentName = input.DepartmentName,
                DepartmentAddress = input.DepartmentAddress,
                ParentId = input.ParentId,
                CreatedBy = username,
                TradingProviderId = tradingProviderId
            });
            return _mapper.Map<DepartmentDto>(department);
        }

        public int MoveSale(MoveSaleDto input)
        {
            if (input.SaleType == SaleTypes.COLLABORATOR && input.SaleParentId == null)
            {
                throw new FaultException(new FaultReason($"Phải chọn saler quản lý cho saler là cộng tác viên"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _departmentRepository.MoveSale(input, username, tradingProviderId);
        }

        public int AssignManager(AssignManagerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _departmentRepository.AssignManager(input, username, tradingProviderId);
        }

        public int AssignManager2(AssignManagerDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _departmentRepository.AssignManager2(input, username, tradingProviderId);
        }

        public List<DepartmentDto> FindAllList(int? parentId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var list = _departmentRepository.FindAllListChild(parentId, tradingProviderId);
            var result = new List<DepartmentDto>();
            foreach (var item in list)
            {
                var department = _mapper.Map<DepartmentDto>(item);

                var listDepartmentChild = _departmentRepository.FindAllListChild(item.DepartmentId, tradingProviderId);
                if (listDepartmentChild.Count() == 0)
                {
                    department.HasDepartmentChild = false;
                }
                else department.HasDepartmentChild = true;

                var saleFind = _saleRepository.AppSaleInfo(item.ManagerId ?? 0, tradingProviderId);
                if (saleFind != null)
                {
                    var info = GetSaleInfoDetail(saleFind.InvestorId, saleFind.BusinessCustomerId);
                    department.Manager = info.Investor;
                    department.ManagerBusinessCustomer = info.BusinessCustomer;
                }

                if (department.ManagerId2 != null)
                {
                    var saleFind2 = _saleRepository.AppSaleInfo(department.ManagerId2 ?? 0, tradingProviderId);
                    if (saleFind2 != null)
                    {
                        var info = GetSaleInfoDetail(saleFind2.InvestorId, null);
                        department.Manager2 = info.Investor;
                    }
                }
                result.Add(department);
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin dựa trên id
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="businessCustomerId"></param>
        /// <returns></returns>
        private SaleInfoDetailDto GetSaleInfoDetail(int? investorId, int? businessCustomerId)
        {
            SaleInfoDetailDto result = new();
            if (investorId != null)
            {
                var investorFind = _investorRepository.FindById(investorId ?? 0);
                if (investorFind != null)
                {
                    var investor = _mapper.Map<InvestorDto>(investorFind);
                    result.Investor = investor;
                    var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(investorId ?? 0);

                    if (investorIdenDefaultFind != null)
                    {
                        result.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                    }
                }
            }

            if (businessCustomerId != null)
            {
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(businessCustomerId ?? 0);
                if (businessCustomer != null)
                {
                    result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                }
            }
            return result;
        }

        public DepartmentDto FindById(int departmentId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var departmentFind = _departmentRepository.FindById(departmentId, tradingProviderId);
            var result = new DepartmentDto();
            result.DepartmentId = departmentFind.DepartmentId;
            result.TradingProviderId = departmentFind.TradingProviderId;
            result.DepartmentName = departmentFind.DepartmentName;
            result.DepartmentAddress = departmentFind.DepartmentAddress;
            result.ParentId = departmentFind.ParentId;
            result.DepartmentLevel = departmentFind.DepartmentLevel;
            result.ManagerId = departmentFind.ManagerId;
            result.ManagerId2 = departmentFind.ManagerId2;

            var saleFind = _saleRepository.AppSaleInfo(departmentFind.ManagerId ?? 0, tradingProviderId);
            if (saleFind != null)
            {
                var info = GetSaleInfoDetail(saleFind.InvestorId, saleFind.BusinessCustomerId);
                result.Manager = info.Investor;
                result.ManagerBusinessCustomer = info.BusinessCustomer;
            }

            if (departmentFind.ManagerId2 != null)
            {
                var saleFind2 = _saleRepository.AppSaleInfo(departmentFind.ManagerId2 ?? 0, tradingProviderId);
                if (saleFind2 != null)
                {
                    var info = GetSaleInfoDetail(saleFind2.InvestorId, null);
                    result.Manager2 = info.Investor;
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách phòng ban con theo departmentId
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public List<ViewDepartmentDto> FindDepartmentChildById(int departmentId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            List<Department> departmentList = new();
            _saleShareServices.DeQuyDepartmentChild(tradingProviderId, departmentId, ref departmentList);
            return _mapper.Map<List<ViewDepartmentDto>>(departmentList);
        }

        /// <summary>
        /// Xem danh sách sale của phòng ban
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="saleTypes"></param>
        /// <param name="departmentId"></param>
        /// <param name="isInvestor"></param>
        /// <returns></returns>
        public PagingResult<DepartmentSaleDto> FindAllListSaleInDepartment(int pageSize, int pageNumber, string keyword, string saleTypes, int departmentId, bool? isInvestor)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var departmentSaleList = _departmentRepository.FindAllListSale(pageSize, pageNumber, keyword, saleTypes, departmentId, tradingProviderId, isInvestor);

            var result = new PagingResult<DepartmentSaleDto>();
            var items = new List<DepartmentSaleDto>();

            foreach (var saleItem in departmentSaleList.Items)
            {
                var sale = _mapper.Map<DepartmentSaleDto>(saleItem);
                if (saleItem.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(saleItem.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        sale.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleItem.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            sale.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                    }
                }

                else if (saleItem.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleItem.BusinessCustomerId ?? 0);
                    if (businessCustomer != null)
                    {
                        sale.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    }
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    sale.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(sale);
            }
            result.Items = items;
            result.TotalItems = items.Count;
            return result;
        }

        public PagingResult<DepartmentSaleDto> FindAllListSale(SaleFilterDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            decimal tongSoSale = 0;
            decimal tongSoSaleThang = 0;
            var departmentSaleList = new List<DepartmentSaleDto>();

            _saleShareServices.DeQuyDepartmentSaleChild(tradingProviderId, input.DepartmentId, input.IsInvestor, ref tongSoSale, ref tongSoSaleThang, ref departmentSaleList);

            var result = new PagingResult<DepartmentSaleDto>();

            //TotalItems
            result.TotalItems = departmentSaleList.Count();

            var items = new List<DepartmentSaleDto>();
            if (input.PageSize != -1)
            {
                departmentSaleList = departmentSaleList.Skip(input.Skip).Take(input.PageSize).ToList();
            }
            foreach (var saleItem in departmentSaleList)
            {
                var sale = _mapper.Map<DepartmentSaleDto>(saleItem);
                if (saleItem.InvestorId != null)
                {
                    var investorFind = _investorRepository.FindById(saleItem.InvestorId ?? 0);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        sale.Investor = investor;
                        sale.ReferralCode = investorFind.ReferralCodeSelf;
                        var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleItem.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            sale.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }
                        sale.ReferralCode = investorFind.ReferralCodeSelf;
                    }
                }

                else if (saleItem.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleItem.BusinessCustomerId ?? 0);
                    if (businessCustomer != null)
                    {
                        sale.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                        sale.ReferralCode = businessCustomer.ReferralCodeSelf;
                    }
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    sale.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(sale);
            }
            //Tìm theo loại Sale
            if (input.SaleType != null)
            {
                items = items.Where(e => e.SaleType == input.SaleType).ToList();
            }
            //Tìm theo trạng thái
            if (input.Status != null)
            {
                items = items.Where(e => e.Status == input.Status).ToList();
            }
            //Tìm theo Mã nhân viên
            if (!string.IsNullOrEmpty(input.EmployeeCode))
            {
                items = items.Where(e => e.EmployeeCode != null && e.EmployeeCode.Contains(input.EmployeeCode)).ToList();
            }
            //Tìm theo họ tên
            if (!string.IsNullOrEmpty(input.FullName))
            {
                items = items.Where(e => e.Investor != null && e.Investor.InvestorIdentification != null && e.Investor.InvestorIdentification.Fullname.ToLower().Contains(input.FullName.ToLower()) || e.BusinessCustomer != null && e.BusinessCustomer.Name.ToLower().Contains(input.FullName.ToLower())).ToList();
            }
            //Tìm theo số điện thoại
            if (!string.IsNullOrEmpty(input.Phone))
            {
                items = items.Where(e => e.Investor != null && e.Investor.Phone.Contains(input.Phone) || e.BusinessCustomer != null && e.BusinessCustomer.Phone.Contains(input.Phone)).ToList();
            }
            //Tìm theo Loại khách hàng
            if (input.IsInvestor == true)
            {
                items = items.Where(e => e.Investor != null).ToList();
            }
            //Tìm theo CCCD/CMND
            if (!string.IsNullOrEmpty(input.IdNo))
            {
                items = items.Where(e => e.Investor != null && e.Investor.InvestorIdentification != null && e.Investor.InvestorIdentification.IdNo.Contains(input.IdNo)).ToList();
            }
            //Tìm theo mã tư vấn viên
            if (!string.IsNullOrEmpty(input.ReferralCode))
            {
                items = items.Where(e => e.ReferralCode != null && e.ReferralCode.Contains(input.ReferralCode)).ToList();
            }
            //Tìm theo mã số thuế
            if (!string.IsNullOrEmpty(input.TaxCode))
            {
                items = items.Where(e => e.Investor != null && e.Investor.TaxCode.Contains(input.TaxCode) || e.BusinessCustomer != null && e.BusinessCustomer.TaxCode.Contains(input.TaxCode)).ToList();
            }
            result.Items = items;
            return result;
        }

        public DepartmentDto Update(UpdateDepartmentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var departmentFind = _departmentRepository.FindById(input.DepartmentId, tradingProviderId);
            if (departmentFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin phòng ban của đại lý"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }  
            //Nếu có cập nhật phòng ban cha
            if (departmentFind.ParentId != input.DepartmentParentId)
            {
                var listDepartment = new List<Department>();

                //Đệ quy lấy thông tin của các phòng ban cấp dưới
                _saleShareServices.DeQuyDepartmentChild(tradingProviderId, input.DepartmentId, ref listDepartment);

                var departmentParentFind = _departmentRepository.FindById(input.DepartmentParentId ?? 0, tradingProviderId);

                //Xét cấp level của phòng ban cấp trên 
                int departmentLevel = 0;
                if (departmentParentFind != null && departmentParentFind.DepartmentLevel != null)
                {
                    departmentLevel = departmentParentFind.DepartmentLevel.Value;
                }

                //Cập nhật phòng ban hiện tại
                _departmentRepository.UpdateLevelDepartment(tradingProviderId, input.DepartmentId, input.DepartmentParentId, departmentLevel + 1, username);
                listDepartment = listDepartment.Where(r => r.DepartmentId != input.DepartmentId).ToList();
                //Cập nhật cấp độ của các phòng ban cấp dưới
                foreach (var departmentItem in listDepartment)
                {
                    _departmentRepository.UpdateLevelDepartment(tradingProviderId, departmentItem.DepartmentId, departmentItem.ParentId, departmentItem.DepartmentLevel.Value + 1, username);
                }
            }
            
            var department = _departmentRepository.Update(new Department
            {
                DepartmentId = input.DepartmentId,
                DepartmentName = input.DepartmentName,
                DepartmentAddress = input.DepartmentAddress,
                ModifiedBy = username,
                TradingProviderId = tradingProviderId
            });
            return _mapper.Map<DepartmentDto>(department);
        }

        public PagingResult<DepartmentDto> FindAll(int pageSize, int pageNumber, string keyword)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var departmentList = _departmentRepository.FindAll(pageSize, pageNumber, keyword, tradingProviderId);
            return departmentList;
        }

        /// <summary>
        /// Lấy danh sách phòng ban trừ phòng ban đang xét và những phòng ban cấp dưới của phòng ban đang xét
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public List<DepartmentDto> FindAllDepartmentNotDepartmentCurrent(int departmentId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            //Lấy toàn bộ danh sách phòng ban có trong đại lý
            var departmentList = _departmentRepository.FindAll(-1, 0, null, tradingProviderId).Items.ToList();

            //Đệ quy lấy danh sách phòng ban cấp dưới của phòng ban đang xét
            List<Department> listDepartmentChild = new();
            _saleShareServices.DeQuyDepartmentChild(tradingProviderId, departmentId, ref listDepartmentChild);

            var listDepartmentIdChild = listDepartmentChild.Select(r => r.DepartmentId).ToList();
            listDepartmentIdChild.Add(departmentId);

            // Lọc danh sách phòng ban trừ phòng ban đang xét và những phòng ban cấp dưới của phòng ban đang xét
            departmentList = departmentList.Where(r => !listDepartmentIdChild.Contains(r.DepartmentId)).ToList();
            return departmentList;
        }

        public void DeleteDepartment(int departmentId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _departmentRepository.Delete(departmentId, tradingProviderId);
        }

        public void DeleteDepartmentManager(int departmentId, int managerType)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _departmentRepository.DeleteDepartmentManager(departmentId, tradingProviderId, managerType, username);
        }

        public void DepartmentChangeListSale(int departmentId, int newDepartmentId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _departmentRepository.DepartmentChangeListSale(departmentId, newDepartmentId, username, tradingProviderId);
        }

        public void UpdateListSaleToNewDepartment(UpdateListSaleToNewDepartment input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _departmentRepository.UpdateListSaleToNewDepartment(tradingProviderId, input, username);
        }

        public void UpdateLevelDepartment(int departmentId, int? departmentParentId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var listDepartment = new List<Department>();

            //Đệ quy lấy thông tin của các phòng ban cấp dưới
            _saleShareServices.DeQuyDepartmentChild(tradingProviderId, departmentId, ref listDepartment);

            var departmentParentFind = _departmentRepository.FindById(departmentParentId ?? 0, tradingProviderId);

            //Xét cấp level của phòng ban cấp trên
            int departmentLevel = 0;
            if (departmentParentFind != null && departmentParentFind.DepartmentLevel != null)
            {
                departmentLevel = departmentParentFind.DepartmentLevel.Value;
            }

            //Cập nhật phòng ban hiện tại
            _departmentRepository.UpdateLevelDepartment(tradingProviderId, departmentId, departmentParentId, departmentLevel + 1, username);
            listDepartment = listDepartment.Where(r => r.DepartmentId != departmentId).ToList();
            //Cập nhật cấp độ của các phòng ban cấp dưới
            foreach (var departmentItem in listDepartment)
            {
                _departmentRepository.UpdateLevelDepartment(tradingProviderId, departmentItem.DepartmentId, departmentItem.ParentId, departmentItem.DepartmentLevel.Value + 1, username);
            }
        }
    }
}

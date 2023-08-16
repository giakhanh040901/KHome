using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.ExcelReport;
using EPIC.CoreRepositories;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.CoreDomain.Implements
{
    public class ExportExcelReportService : IExportExcelReportServices
    {
        private readonly ILogger<ExportExcelReportService> _logger;
        private string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly BankRepository _bankRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly ExportExcelReportRepository _exportExcelReportRepository;
        private readonly UserRepository _userRepository;
        private readonly SaleRepository _saleRepository;
        private readonly ISaleShareServices _saleShareServices;
        private readonly IManagerInvestorServices _managerInvestorServices;

        public ExportExcelReportService(ILogger<ExportExcelReportService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext, IMapper mapper,
            ISaleShareServices saleShareServices,
            IManagerInvestorServices managerInvestorServices,
            EpicSchemaDbContext dbContext)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _mapper = mapper;
            _dbContext = dbContext;
            _exportExcelReportRepository = new ExportExcelReportRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _userRepository = new UserRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _saleShareServices = saleShareServices;
            _managerInvestorServices = managerInvestorServices;
        }

        /// <summary>
        /// báo cáo danh sách saler
        /// </summary>
        /// <returns></returns>
        public ExportResultDto SalerListExcelReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var salersFind = _saleRepository.GetListSalerExcel(tradingProviderId, startDate, endDate);
            var listSaleReport = new List<SalerListExcelReportDto>();
            foreach (var item in salersFind)
            {
                var salerExcel = new SalerListExcelReportDto();
                var listDepartment = new List<Department>();

                //Đệ quy lấy danh sách phòng ban node cha của phòng ban hiện tại đang xét
                _saleShareServices.DeQuyDepartmentParent(tradingProviderId, item.DepartmentId ?? 0, ref listDepartment);
                var saleManagerTrucTiep = _saleRepository.SaleGetById(item.SaleParentId, tradingProviderId);

                salerExcel.SaleManagerName = saleManagerTrucTiep?.Fullname;
                salerExcel.SaleManagerReferralCode = saleManagerTrucTiep?.ReferralCode;

                var departmentSaleInfo = _departmentRepository.FindById(item.DepartmentId ?? 0, tradingProviderId);

                if (listDepartment != null && listDepartment.Count > 0 && listDepartment[0] != null)
                {
                    foreach (var department in listDepartment)
                    {
                        if (department?.DepartmentLevel == 4)
                        {
                            var saleManagerLv1 = _saleRepository.SaleGetById(department.ManagerId ?? 0, tradingProviderId);
                            salerExcel.ManagerNameLv1 = saleManagerLv1?.Fullname;
                            salerExcel.ManagerCodeLv1 = saleManagerLv1?.ReferralCode;
                        }
                        else if (department?.DepartmentLevel == 3)
                        {
                            var saleManagerLv2 = _saleRepository.SaleGetById(department.ManagerId ?? 0, tradingProviderId);
                            salerExcel.ManagerNameLv2 = saleManagerLv2?.Fullname;
                            salerExcel.ManagerCodeLv2 = saleManagerLv2?.ReferralCode;
                        }
                        else if (department.DepartmentLevel == 2)
                        {
                            var saleManagerLv3 = _saleRepository.SaleGetById(department.ManagerId ?? 0, tradingProviderId);
                            salerExcel.ManagerNameLv3 = saleManagerLv3?.Fullname;
                            salerExcel.ManagerCodeLv3 = saleManagerLv3?.ReferralCode;
                        }
                        else if (department.DepartmentLevel == 1)
                        {
                            var saleManagerLv4 = _saleRepository.SaleGetById(department.ManagerId ?? 0, tradingProviderId);
                            salerExcel.ManagerNameLv4 = saleManagerLv4?.Fullname;
                            salerExcel.ManagerCodeLv4 = saleManagerLv4?.ReferralCode;
                        }
                    }
                }

                salerExcel.Status = ExcelDataUtils.StatusActive(item?.Status);
                salerExcel.DeactiveDate = item?.DeactiveDate;
                salerExcel.Source = ExcelDataUtils.SourceDisplay(item?.Source);
                salerExcel.DepartmentName = item.DepartmentName;
                salerExcel.EmployeeCode = item.EmployeeCode;
                salerExcel.SalerCode = item.RefferalCodeSelf;
                salerExcel.SaleType = ExcelDataUtils.SaleTypeDisplay(item?.SaleType);
                salerExcel.SaleManagerCode = item.SaleManagerCode;
                salerExcel.ApproveContractDate = item.CreatedDate?.Date;

                var listTradings = _saleRepository.FindAllTradingUpFromSaleTrading4Cap(tradingProviderId);
                var listTradingIds = listTradings.Select(t => t.TradingProviderId).Distinct().ToList();

                try
                {
                    var doanhSoSale = _saleRepository.ThongKeDoanhSoSale(tradingProviderId, item.SaleId, listTradingIds);
                    salerExcel.DoanhSo = doanhSoSale.TotalValueMoney;
                }
                catch
                {
                }

                if (item.InvestorId != null && item.BusinessCustomerId == null)
                {
                    //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)

                    var investorIdentification = _managerInvestorRepository
                        .GetDefaultIdentification(item.InvestorId ?? 0, false); //lấy thông tin của khách hàng cá nhân thông qua giấy tờ mặc định

                    var investor = _managerInvestorRepository.FindById(item.InvestorId ?? 0, false);

                    salerExcel.SaleName = investorIdentification?.Fullname;
                    salerExcel.Email = investor?.Email;
                    salerExcel.Gender = ExcelDataUtils.GenderDisplay(investorIdentification?.Sex);
                    salerExcel.IdNo = investorIdentification?.IdNo;
                    salerExcel.ContractAddress = investorIdentification?.PlaceOfResidence;
                    salerExcel.IdType = investorIdentification?.IdType;
                    salerExcel.Sdt = investor?.Phone;

                }
                else if (item.InvestorId == null && item.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(item.BusinessCustomerId ?? 0);
                    salerExcel.SaleName = businessCustomer?.Name;
                    salerExcel.Email = businessCustomer?.Email;
                    salerExcel.IdNo = businessCustomer?.TaxCode;
                    salerExcel.IdType = TaxCode.MA_SO_THUE;
                }

                if (item.InvestorManagerId != null && item.BusinessManagerId == null)
                {
                    var investorIdentification = _managerInvestorRepository
                        .GetDefaultIdentification(item.InvestorManagerId ?? 0, false); //lấy thông tin của khách hàng cá nhân thông qua giấy tờ mặc định

                    var investor = _managerInvestorRepository.FindById(item.InvestorId ?? 0, false);
                    salerExcel.SaleManagerCode = investor?.ReferralCodeSelf;
                    salerExcel.SaleManagerName = investorIdentification?.Fullname;

                }
                else if (item.InvestorManagerId == null && item.BusinessManagerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(item?.BusinessManagerId ?? 0);
                    salerExcel.SaleManagerName = businessCustomer?.Name;
                    salerExcel.SaleManagerCode = businessCustomer?.ReferralCodeSelf;
                }
                listSaleReport.Add(salerExcel);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Core");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã NV";
            worksheet.Cell(currentRow, 3).Value = "Mã Saler";
            worksheet.Cell(currentRow, 4).Value = "Họ tên";
            worksheet.Cell(currentRow, 5).Value = "Mã số giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 7).Value = "Giới tính";
            worksheet.Cell(currentRow, 8).Value = "Số điện thoại";
            worksheet.Cell(currentRow, 9).Value = "Email";
            worksheet.Cell(currentRow, 10).Value = "Vai trò";
            worksheet.Cell(currentRow, 11).Value = "Ngày duyệt hợp đồng";
            worksheet.Cell(currentRow, 12).Value = "Nguồn đăng kí";
            worksheet.Cell(currentRow, 13).Value = "Trạng thái";
            worksheet.Cell(currentRow, 14).Value = "Nghỉ việc ngày";
            worksheet.Cell(currentRow, 15).Value = "Phòng ban";
            worksheet.Cell(currentRow, 16).Value = "Người quản lý trực tiếp";
            worksheet.Cell(currentRow, 17).Value = "Mã quản lý trực tiếp";
            worksheet.Cell(currentRow, 18).Value = "Tên quản lý cấp 1";
            worksheet.Cell(currentRow, 19).Value = "Mã quản lý cấp 1";
            worksheet.Cell(currentRow, 20).Value = "Tên quản lý cấp 2";
            worksheet.Cell(currentRow, 21).Value = "Mã quản lý cấp 2";
            worksheet.Cell(currentRow, 22).Value = "Tên quản lý cấp 3";
            worksheet.Cell(currentRow, 23).Value = "Mã quản lý cấp 3";
            worksheet.Cell(currentRow, 24).Value = "Tên quản lý cấp 4";
            worksheet.Cell(currentRow, 25).Value = "Mã quản lý cấp 4";
            worksheet.Cell(currentRow, 26).Value = "Doanh số bán hàng";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 20;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 60;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;
            worksheet.Column("S").Width = 40;
            worksheet.Column("T").Width = 40;
            worksheet.Column("U").Width = 40;
            worksheet.Column("V").Width = 40;
            worksheet.Column("W").Width = 40;
            worksheet.Column("X").Width = 40;
            worksheet.Column("Y").Width = 40;
            worksheet.Column("Z").Style.NumberFormat.Format = "0";
            worksheet.Column("AA").Width = 40;

            foreach (var item in listSaleReport)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item?.EmployeeCode;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.SalerCode;
                worksheet.Cell(currentRow, 4).Value = item?.SaleName;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 6).Value = item?.IdType;
                worksheet.Cell(currentRow, 7).Value = item?.Gender;
                worksheet.Cell(currentRow, 8).Value = "'" + item?.Sdt;
                worksheet.Cell(currentRow, 9).Value = item?.Email;
                worksheet.Cell(currentRow, 10).Value = item?.SaleType;
                worksheet.Cell(currentRow, 11).Value = item?.ApproveContractDate?.Date;
                worksheet.Cell(currentRow, 12).Value = item?.Source;
                worksheet.Cell(currentRow, 13).Value = item.Status;
                worksheet.Cell(currentRow, 14).Value = item?.DeactiveDate?.Date;
                worksheet.Cell(currentRow, 15).Value = item?.DepartmentName;
                worksheet.Cell(currentRow, 16).Value = item?.SaleManagerName;
                worksheet.Cell(currentRow, 17).Value = "'" + item?.SaleManagerReferralCode;
                worksheet.Cell(currentRow, 18).Value = "'" + item?.ManagerNameLv1;
                worksheet.Cell(currentRow, 19).Value = "'" + item?.ManagerCodeLv1;
                worksheet.Cell(currentRow, 20).Value = "'" + item?.ManagerNameLv2;
                worksheet.Cell(currentRow, 21).Value = "'" + item?.ManagerCodeLv2;
                worksheet.Cell(currentRow, 22).Value = "'" + item?.ManagerNameLv3;
                worksheet.Cell(currentRow, 23).Value = "'" + item?.ManagerCodeLv3;
                worksheet.Cell(currentRow, 24).Value = "'" + item?.ManagerNameLv4;
                worksheet.Cell(currentRow, 25).Value = "'" + item?.ManagerCodeLv4;
                worksheet.Cell(currentRow, 26).Value = item?.DoanhSo;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// báo cáo danh sách khách hàng
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto CustomerListExcelReport(DateTime? startDate, DateTime? endDate)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var customerListExcelReport = new List<CustomerListExcelReportDto>();
            var result = new ExportResultDto();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var listCustomerReportFind = _investorRepository.GetListCustomerExcelReport(userType, tradingProviderId, startDate, endDate);

            foreach (var dataItem in listCustomerReportFind)
            {
                var customerExcelReport = new CustomerListExcelReportDto();
                var investorIndentification = _managerInvestorRepository.GetDefaultIdentification(dataItem?.InvestorId ?? 0, false);
                var invBankFind = _investorBankAccountRepository.GetByInvestorId(dataItem?.InvestorId ?? 0);
                var bankInfo = _bankRepository.GetById(invBankFind?.BankId ?? 0);

                customerExcelReport.CifCode = dataItem?.CifCode;
                customerExcelReport.CreatedDate = dataItem?.CreatedDate;
                customerExcelReport.Gender = ExcelDataUtils.GenderDisplay(investorIndentification?.Sex);

                customerExcelReport.CustomerName = investorIndentification?.Fullname;
                customerExcelReport.DateOfBirth = investorIndentification?.DateOfBirth?.Date;
                customerExcelReport.IdNo = investorIndentification?.IdNo;
                customerExcelReport.IdType = investorIndentification?.IdType;
                customerExcelReport.BankAccount = invBankFind?.BankAccount;
                customerExcelReport.BankName = bankInfo?.BankName;
                customerExcelReport.IdDate = investorIndentification?.IdDate?.Date;
                customerExcelReport.IdExpiredDate = investorIndentification?.IdExpiredDate?.Date;
                customerExcelReport.PlaceOfOrigin = investorIndentification?.PlaceOfOrigin;
                customerExcelReport.PlaceOfResidence = investorIndentification?.PlaceOfResidence;
                customerExcelReport.Status = ExcelDataUtils.IsConfirmDisplay(investorIndentification?.EkycInfoIsConfirmed);

                customerExcelReport.IsProf = ExcelDataUtils.YesNoDisplay(dataItem.IsProf);
                customerExcelReport.Source = ExcelDataUtils.SourceDisplay(dataItem.Source);
                customerExcelReport.SecurityCompany = ExcelDataUtils.SecurityCompanyDisplay(dataItem.SecurityCompany);

                customerListExcelReport.Add(customerExcelReport);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Core");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày tạo";
            worksheet.Cell(currentRow, 3).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Họ và tên";
            worksheet.Cell(currentRow, 5).Value = "Giới tính";
            worksheet.Cell(currentRow, 6).Value = "Ngày sinh";
            worksheet.Cell(currentRow, 7).Value = "CMT/CCCD/HC";
            worksheet.Cell(currentRow, 8).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 9).Value = "Ngày cấp giấy tờ";
            worksheet.Cell(currentRow, 10).Value = "Ngày hết hạn";
            worksheet.Cell(currentRow, 11).Value = "Nguyên quán";
            worksheet.Cell(currentRow, 12).Value = "Địa chỉ liên hệ";
            worksheet.Cell(currentRow, 13).Value = "Số tài khoản";
            worksheet.Cell(currentRow, 14).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 15).Value = "Nguồn tạo";
            worksheet.Cell(currentRow, 16).Value = "Trạng thái xác minh";
            worksheet.Cell(currentRow, 17).Value = "Thông tin NĐTCN";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 20;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 60;
            worksheet.Column("L").Width = 60;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;

            foreach (var item in customerListExcelReport)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.CreatedDate?.Date;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.CifCode;
                worksheet.Cell(currentRow, 4).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 5).Value = item?.Gender;
                worksheet.Cell(currentRow, 6).Value = item?.DateOfBirth?.Date;
                worksheet.Cell(currentRow, 7).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 8).Value = item.IdType;
                worksheet.Cell(currentRow, 9).Value = item.IdDate?.Date;
                worksheet.Cell(currentRow, 10).Value = item.IdExpiredDate?.Date;
                worksheet.Cell(currentRow, 11).Value = item?.PlaceOfOrigin;
                worksheet.Cell(currentRow, 12).Value = item?.PlaceOfResidence;
                worksheet.Cell(currentRow, 13).Value = "'" + item?.BankAccount;
                worksheet.Cell(currentRow, 14).Value = item.BankName;
                worksheet.Cell(currentRow, 15).Value = item?.Source;
                worksheet.Cell(currentRow, 16).Value = item?.Status;
                worksheet.Cell(currentRow, 17).Value = item?.IsProf;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// báo cáo danh sách thông tin khách hàng root
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto CustomerRootListExcelReport(DateTime? startDate, DateTime? endDate)
        {
            var listCustomerRootExcelReport = new List<CustomerRootListDto>();
            var result = new ExportResultDto();
            var listCustomers = from investor in _dbContext.Investors
                                join cifcode in _dbContext.CifCodes on investor.InvestorId equals cifcode.InvestorId
                                from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                                  .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                                let contactAddress = _dbContext.InvestorContactAddresses.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                                            .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.ContactAddressId).FirstOrDefault().ContactAddress
                                let bankAccount = (from bankAccount in _dbContext.InvestorBankAccounts
                                                   join coreBank in _dbContext.CoreBanks on bankAccount.BankId equals coreBank.BankId
                                                   where bankAccount.InvestorId == investor.InvestorId && bankAccount.Deleted == YesNo.NO
                                                   orderby bankAccount.IsDefault descending
                                                   select new { bankAccount.BankAccount, coreBank.BankName }).FirstOrDefault()
                                where investor.Deleted == YesNo.NO && investor.Step >= InvestorAppStep.DA_ADD_BANK
                                && (investor.Status == InvestorStatus.ACTIVE || investor.Status == InvestorStatus.DEACTIVE)
                                && (startDate == null || (investor.CreatedDate != null && startDate.Value.Date <= investor.CreatedDate.Value.Date))
                                && (endDate == null || (investor.CreatedDate != null && endDate.Value.Date >= investor.CreatedDate.Value.Date))
                                orderby investor.CreatedDate descending
                                select new CustomerRootListDto
                                {
                                    Phone = investor.Phone,
                                    CifCode = cifcode.CifCode,
                                    CreatedDate = investor.CreatedDate,
                                    DateOfBirth = identification.DateOfBirth,
                                    Gender = ExcelDataUtils.GenderDisplay(identification.Sex),
                                    IdType = identification.IdType,
                                    IdNo = identification.IdNo,
                                    PlaceOfOrigin = identification.PlaceOfOrigin,
                                    PlaceOfResidence = identification.PlaceOfResidence,
                                    ReferralCode = investor.ReferralCode,
                                    IssueDate = identification.IdDate,
                                    IdExpireDate = identification.IdExpiredDate,
                                    ContractAddress = contactAddress,
                                    CustomerName = identification.Fullname,
                                    Status = ExcelDataUtils.IsConfirmDisplay(identification.EkycInfoIsConfirmed),
                                    Source = ExcelDataUtils.SourceDisplay(investor.Source),
                                    EpicIsConfirm = ExcelDataUtils.YesNoDisplay(investor.IsCheck),
                                    CreatedBy = investor.CreatedBy,
                                    IsProf = ExcelDataUtils.YesNoDisplay(investor.IsProf),
                                    BankAccNo = bankAccount.BankAccount,
                                    CustomerBankName = bankAccount.BankName
                                };

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Core");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày tạo";
            worksheet.Cell(currentRow, 3).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Họ và tên";
            worksheet.Cell(currentRow, 5).Value = "Giới tính";
            worksheet.Cell(currentRow, 6).Value = "Ngày sinh";
            worksheet.Cell(currentRow, 7).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 8).Value = "Ngày cấp giấy tờ";
            worksheet.Cell(currentRow, 9).Value = "Ngày hết hạn";
            worksheet.Cell(currentRow, 10).Value = "Nguyên quán";
            worksheet.Cell(currentRow, 11).Value = "Địa chỉ liên hệ";
            worksheet.Cell(currentRow, 12).Value = "Số tài khoản";
            worksheet.Cell(currentRow, 13).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 14).Value = "Nguồn tạo";
            worksheet.Cell(currentRow, 15).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 16).Value = "Trạng thái xác minh";
            worksheet.Cell(currentRow, 17).Value = "Xác minh EPIC";
            worksheet.Cell(currentRow, 18).Value = "Thông tin NĐTCN";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;

            foreach (var item in listCustomers)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.CreatedDate?.Date;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.CifCode;
                worksheet.Cell(currentRow, 4).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 5).Value = item?.Gender;
                worksheet.Cell(currentRow, 6).Value = item?.DateOfBirth;
                worksheet.Cell(currentRow, 7).Value = "'" + item?.IdType;
                worksheet.Cell(currentRow, 8).Value = item.IssueDate;
                worksheet.Cell(currentRow, 9).Value = item.IdExpireDate;
                worksheet.Cell(currentRow, 10).Value = "'" + item.PlaceOfOrigin;
                worksheet.Cell(currentRow, 11).Value = "'" + item.ContractAddress;
                worksheet.Cell(currentRow, 12).Value = "'" + item.BankAccNo;
                worksheet.Cell(currentRow, 13).Value = "'" + item.CustomerBankName;
                worksheet.Cell(currentRow, 14).Value = "'" + item.Source;
                worksheet.Cell(currentRow, 15).Value = "'" + item.ReferralCode;
                worksheet.Cell(currentRow, 16).Value = "'" + item.Status;
                worksheet.Cell(currentRow, 17).Value = "'" + item.EpicIsConfirm;
                worksheet.Cell(currentRow, 18).Value = "'" + item.IsProf;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo tài khoản người dùng
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto UserExcelReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var listUserReport = _investorRepository.GetListUserReport(null, null);

            listUserReport.AddRange(_investorEFRepository.FindUserNoEkyc(null));
            listUserReport.OrderBy(u => u.CreatedDate);

            if (startDate != null && endDate == null)
            {
                listUserReport = listUserReport.Where(l => l.CreatedDate?.Date >= startDate?.Date).ToList();
            }
            else if (startDate == null && endDate != null)
            {
                listUserReport = listUserReport.Where(l => l.CreatedDate?.Date <= endDate?.Date).ToList();
            }
            else if (startDate != null && endDate != null)
            {
                listUserReport = listUserReport.Where(l => l.CreatedDate?.Date <= endDate?.Date && l.CreatedDate?.Date >= startDate?.Date).ToList();
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Core");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "User";
            worksheet.Cell(currentRow, 3).Value = "Ngày tạo";
            worksheet.Cell(currentRow, 4).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 5).Value = "eKYC";
            worksheet.Cell(currentRow, 6).Value = "Ngày xác minh";
            worksheet.Cell(currentRow, 7).Value = "Trạng thái";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 50;

            foreach (var item in listUserReport)
            {
                currentRow++;
                stt = ++stt;

                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item?.Username;
                worksheet.Cell(currentRow, 3).Value = item?.CreatedDate;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.ReferralCode;
                worksheet.Cell(currentRow, 5).Value = item.Step >= 4 ? IsConfirm.DA_XAC_MINH : IsConfirm.CHUA_XAC_MINH;
                worksheet.Cell(currentRow, 6).Value = item.FinalStepDate;
                worksheet.Cell(currentRow, 7).Value = ExcelDataUtils.UserStatusDisplay(item.Status);
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo danh sách khách hàng HVF
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto ListCustomerHVF(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var customerHVFListExcelReport = new List<CustomerHVFListExcelReportDto>();
            var listCustomerHVFReportFind = _investorRepository.GetListCustomerHVFExcelReport(userType, tradingProviderId, startDate, endDate);
            foreach (var dataItem in listCustomerHVFReportFind)
            {
                var customerHVFExcelReport = new CustomerHVFListExcelReportDto();

                // Thông tin chung
                var investorIndentification = _managerInvestorRepository.GetDefaultIdentification(dataItem?.InvestorId ?? 0, false);
                var investorFind = _managerInvestorRepository.FindById(dataItem?.InvestorId ?? 0, false);
                // Bank
                var invBankFind = _investorBankAccountRepository.GetByInvestorId((int)dataItem?.InvestorId);
                customerHVFExcelReport.BankAcc = invBankFind?.BankAccount;
                customerHVFExcelReport.BankAccHolder = invBankFind?.OwnerAccount;
                customerHVFExcelReport.BankBranch = invBankFind?.BankBranch;

                var coreBankFind = _bankRepository.GetById(invBankFind?.BankId ?? 0);
                customerHVFExcelReport.BankName = coreBankFind?.FullBankName;

                //Sale
                var saleInfo = _saleRepository.GetSaleInfoByRefferalCode(tradingProviderId, dataItem.ReferralCode);
                var saleInfoBusiness = _saleRepository.GetSaleInfoByRefferalCodeBusiness(tradingProviderId, dataItem.ReferralCode);

                if (saleInfo != null && saleInfoBusiness == null)
                {
                    customerHVFExcelReport.Presenter = saleInfo.Fullname;
                    customerHVFExcelReport.Department = saleInfo.DepartmentName;
                }
                else if (saleInfoBusiness != null && saleInfo == null)
                {
                    customerHVFExcelReport.Presenter = saleInfoBusiness.Name;
                    customerHVFExcelReport.Department = saleInfoBusiness.DepartmentName;
                }

                customerHVFExcelReport.Phone = investorFind.Phone;
                customerHVFExcelReport.Email = investorFind.Email;
                customerHVFExcelReport.CustomerName = investorIndentification?.Fullname;
                customerHVFExcelReport.IdNo = investorIndentification?.IdNo;
                customerHVFExcelReport.IdType = investorIndentification?.IdType;
                customerHVFExcelReport.PlaceOfOrigin = investorIndentification?.PlaceOfOrigin;
                customerHVFExcelReport.CifCode = dataItem?.CifCode;
                customerHVFExcelReport.Gender = ExcelDataUtils.GenderDisplay(investorIndentification?.Sex);
                customerHVFExcelReport.ReferralCode = dataItem.ReferralCode;
                customerHVFExcelReport.InvestmentValue = dataItem?.InvestmentValue.ToString();

                customerHVFListExcelReport.Add(customerHVFExcelReport);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Core");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Tên khách";
            worksheet.Cell(currentRow, 3).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 4).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 5).Value = "Địa chỉ";
            worksheet.Cell(currentRow, 6).Value = "Số điện thoại";
            worksheet.Cell(currentRow, 7).Value = "Email";
            worksheet.Cell(currentRow, 8).Value = "Mã KH";
            worksheet.Cell(currentRow, 9).Value = "Giới tính";
            worksheet.Cell(currentRow, 10).Value = "Tài khoản ngân hàng";
            worksheet.Cell(currentRow, 11).Value = "Tên chủ TK";
            worksheet.Cell(currentRow, 12).Value = "Tên ngân hàng";
            worksheet.Cell(currentRow, 13).Value = "Chi nhánh";
            worksheet.Cell(currentRow, 14).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 15).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 16).Value = "Phòng ban";
            worksheet.Cell(currentRow, 17).Value = "Giá trị đầu tư";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;
            worksheet.Column("S").Width = 40;
            worksheet.Column("T").Width = 40;

            foreach (var item in customerHVFListExcelReport)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 4).Value = item?.IdType;
                worksheet.Cell(currentRow, 5).Value = item?.PlaceOfOrigin;
                worksheet.Cell(currentRow, 6).Value = "'" + item.Phone;
                worksheet.Cell(currentRow, 7).Value = "'" + item.Email;
                worksheet.Cell(currentRow, 8).Value = "'" + item?.CifCode;
                worksheet.Cell(currentRow, 9).Value = item?.Gender;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAcc;
                worksheet.Cell(currentRow, 11).Value = item?.BankAccHolder;
                worksheet.Cell(currentRow, 12).Value = item?.BankName;
                worksheet.Cell(currentRow, 13).Value = item?.BankBranch;
                worksheet.Cell(currentRow, 14).Value = "'" + item?.ReferralCode;
                worksheet.Cell(currentRow, 15).Value = item?.Presenter;
                worksheet.Cell(currentRow, 16).Value = item?.Department;
                worksheet.Cell(currentRow, 17).Value = item?.InvestmentValue;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Lấy danh sách thay đổi thông tin khách hàng
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto CustomerInfoChangeReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var listCustomerInfoChangeExcelReport = new List<CustomerInfoChangeReportDto>();
            var listCustomerReportFind = _investorRepository.GetListCustomerInfoChangeExcelReport(tradingProviderId, startDate, endDate);

            foreach (var dataItem in listCustomerReportFind)
            {
                if (InvestorInfo.InfoUpdate.Contains(dataItem?.FieldName))
                {
                    var customerExcelReport = new CustomerInfoChangeReportDto();

                    customerExcelReport.CifCode = dataItem?.CifCode;
                    customerExcelReport.CustomerName = dataItem?.FullName;
                    customerExcelReport.IdNo = dataItem?.IdNo;
                    customerExcelReport.ChangeType = ExcelDataUtils.FieldNameDisplay(dataItem?.FieldName);
                    customerExcelReport.OldValue = ExcelDataUtils.FieldNameDataDisplay(dataItem?.OldValue);
                    customerExcelReport.NewValue = ExcelDataUtils.FieldNameDataDisplay(dataItem?.NewValue);
                    customerExcelReport.ModifiedDate = dataItem?.RequestDate;
                    customerExcelReport.ApproveDate = dataItem?.ApproveDate;
                    customerExcelReport.Description = dataItem?.ApproveNote;
                    customerExcelReport.Status = ExcelDataUtils.ApproveStatusDisplay(dataItem?.Status);
                    if (dataItem?.IsCheck == YesNo.YES) // Epic duyệt
                    {
                        customerExcelReport.Status = ExcelDataUtils.ApproveStatusDisplay(ApproveStatus.EPIC_DUYET);
                    }
                    // Lấy thông tin người duyệt
                    var userApprove = _userRepository.FindById(dataItem?.UserApproveId ?? 0);
                    customerExcelReport.ApproveBy = userApprove?.DisplayName;

                    listCustomerInfoChangeExcelReport.Add(customerExcelReport);
                }

            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Core");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 3).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 4).Value = "ĐKKD/CCCD/CMT/HC";
            worksheet.Cell(currentRow, 5).Value = "Loại thay đổi";
            worksheet.Cell(currentRow, 6).Value = "Thông tin trước thay đổi";
            worksheet.Cell(currentRow, 7).Value = "Thông tin sau thay đổi";
            worksheet.Cell(currentRow, 8).Value = "Thời gian sửa";
            worksheet.Cell(currentRow, 9).Value = "Trạng thái";
            worksheet.Cell(currentRow, 10).Value = "Người duyệt";
            worksheet.Cell(currentRow, 11).Value = "Thời gian duyệt";
            worksheet.Cell(currentRow, 12).Value = "Ghi chú";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;

            foreach (var item in listCustomerInfoChangeExcelReport)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item?.CifCode;
                worksheet.Cell(currentRow, 3).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 5).Value = item?.ChangeType;
                worksheet.Cell(currentRow, 6).Value = item?.OldValue;
                worksheet.Cell(currentRow, 7).Value = item?.NewValue;
                worksheet.Cell(currentRow, 8).Value = "'" + item?.ModifiedDate;
                worksheet.Cell(currentRow, 9).Value = item?.Status;
                worksheet.Cell(currentRow, 10).Value = item?.ApproveBy;
                worksheet.Cell(currentRow, 11).Value = "'" + item?.ApproveDate;
                worksheet.Cell(currentRow, 12).Value = "'" + item?.Description;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo chỉnh sửa thông tin khách hàng root
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto CustomerInfoChangeRootReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var listCustomerInfoChangeRootExcelReport = new List<ExcelCustomerChangeInfoRootDto>();
            var listCustomerInfoChangeRootFind = _exportExcelReportRepository.GetCustomerInfoChangeRoot(startDate, endDate);

            foreach (var dataItem in listCustomerInfoChangeRootFind)
            {

                var customerChangeInfoRoot = new ExcelCustomerChangeInfoRootDto();

                customerChangeInfoRoot.ApproveDate = dataItem.ApproveDate;
                customerChangeInfoRoot.CifCode = dataItem.CifCode;

                if (dataItem.InvestorId != null && dataItem.BusinessCustomerId == null)
                {
                    var investorIdentification = _managerInvestorRepository
                        .GetDefaultIdentification(dataItem?.InvestorId ?? 0, false); //lấy thông tin của khách hàng cá nhân thông qua giấy tờ mặc định
                    customerChangeInfoRoot.CustomerName = investorIdentification?.Fullname;
                }
                else if (dataItem.InvestorId == null && dataItem.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(dataItem?.BusinessCustomerId ?? 0);
                    customerChangeInfoRoot.CustomerName = businessCustomer?.Name;
                }

                customerChangeInfoRoot.EditBy = dataItem.EditBy;
                customerChangeInfoRoot.EditDate = dataItem.EditDate;
                customerChangeInfoRoot.Status = ExcelDataUtils.ApproveStatusDisplay(dataItem?.Status);
                customerChangeInfoRoot.ApproveNote = dataItem.ApproveNote;
                customerChangeInfoRoot.CancelNote = dataItem.CancelNote;
                customerChangeInfoRoot.ApproveBy = dataItem.ApproveUser;
                customerChangeInfoRoot.CustomerType = dataItem.CustomerType;

                var customerInfoChangeRootFind = listCustomerInfoChangeRootExcelReport.LastOrDefault(e => e.CifCode == customerChangeInfoRoot.CifCode);
                customerChangeInfoRoot.EditTimes = customerInfoChangeRootFind == null ? 1 : customerInfoChangeRootFind.EditTimes + 1;

                listCustomerInfoChangeRootExcelReport.Add(customerChangeInfoRoot);
            }

            listCustomerInfoChangeRootExcelReport = listCustomerInfoChangeRootExcelReport.OrderBy(l => l.EditDate).ThenBy(l => l.EditDate).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("CORE");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày chỉnh sửa";
            worksheet.Cell(currentRow, 3).Value = "Ngày duyệt";
            worksheet.Cell(currentRow, 4).Value = "Mã khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Họ và tên";
            worksheet.Cell(currentRow, 6).Value = "Lần chỉnh sửa";
            worksheet.Cell(currentRow, 7).Value = "Người chỉnh sửa";
            worksheet.Cell(currentRow, 8).Value = "Người duyệt";
            worksheet.Cell(currentRow, 9).Value = "Trạng thái";
            worksheet.Cell(currentRow, 10).Value = "Ghi chú chỉnh sửa";
            worksheet.Cell(currentRow, 11).Value = "Ghi chú hủy duyệt";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("J").Width = 40;
            worksheet.Column("K").Width = 40;
            worksheet.Column("L").Width = 40;
            worksheet.Column("M").Width = 40;
            worksheet.Column("N").Width = 40;
            worksheet.Column("O").Width = 40;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 40;
            worksheet.Column("R").Width = 40;

            foreach (var item in listCustomerInfoChangeRootExcelReport)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.EditDate;
                worksheet.Cell(currentRow, 3).Value = item.ApproveDate;
                worksheet.Cell(currentRow, 4).Value = "'" + item.CifCode;
                worksheet.Cell(currentRow, 5).Value = item.CustomerName;
                worksheet.Cell(currentRow, 6).Value = item.EditTimes;
                worksheet.Cell(currentRow, 7).Value = item.EditBy;
                worksheet.Cell(currentRow, 8).Value = item.ApproveBy;
                worksheet.Cell(currentRow, 9).Value = item.Status;
                worksheet.Cell(currentRow, 10).Value = item.ApproveNote;
                worksheet.Cell(currentRow, 11).Value = item.CancelNote;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo khách hàng không xác minh
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto UserNoEkycExcelReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var listUserReport = _investorEFRepository.FindUserNoEkyc(null);
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Core");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "User";
            worksheet.Cell(currentRow, 3).Value = "Ngày tạo";
            worksheet.Cell(currentRow, 4).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 5).Value = "eKYC";
            worksheet.Cell(currentRow, 6).Value = "Trạng thái";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 50;

            foreach (var item in listUserReport)
            {
                currentRow++;
                stt = ++stt;

                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item?.Username;
                worksheet.Cell(currentRow, 3).Value = item?.CreatedDate;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.ReferralCode;
                worksheet.Cell(currentRow, 5).Value = item.Step >= 4 ? IsConfirm.DA_XAC_MINH : IsConfirm.CHUA_XAC_MINH;
                worksheet.Cell(currentRow, 6).Value = item.Status;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }
    }
}
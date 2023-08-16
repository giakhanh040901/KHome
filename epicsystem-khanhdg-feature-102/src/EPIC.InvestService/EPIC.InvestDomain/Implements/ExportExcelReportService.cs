using AutoMapper;
using ClosedXML.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.ExportReport;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ExportExcel;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.InvestDomain.Implements
{
    public class ExportExcelReportService : IExportExcelReportService
    {
        private readonly ILogger<ExportExcelReportService> _logger;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly BankRepository _bankRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly SaleRepository _saleRepository;
        private readonly InvestInterestPaymentRepository _investInterestPaymentRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly InvestOrderRepository _orderRepository;
        private readonly InvestInterestPaymentRepository _interestPaymentRepository;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly EPIC.InvestRepositories.ExportExcelReportRepository _exportExcelReportRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvRenewalsRequestRepository _renewalsRequestRepository;
        private readonly WithdrawalRepository _withdrawalSettlementRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly IInterestPaymentServices _interestPaymentService;
        private readonly SaleEFRepository _saleEFRepository;

        public ExportExcelReportService(ILogger<ExportExcelReportService> logger, IConfiguration configuration,
            DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper,
            IInvestSharedServices investSharedServices, IOrderServices orderServices, EpicSchemaDbContext dbContext,
            IInterestPaymentServices interestPaymentService
            )
        {
            _dbContext = dbContext;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _mapper = mapper;
            _orderServices = orderServices;
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, _logger);
            _withdrawalSettlementRepository = new WithdrawalRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investSharedServices = investSharedServices;
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _investInterestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _interestPaymentRepository = new InvestInterestPaymentRepository(_connectionString, _logger);
            _exportExcelReportRepository = new EPIC.InvestRepositories.ExportExcelReportRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _renewalsRequestRepository = new InvRenewalsRequestRepository(_connectionString, _logger);
            _interestPaymentService = interestPaymentService;
            _saleEFRepository = new SaleEFRepository(_dbContext, logger);
        }

        /// <summary>
        /// Export file excel trong có thông tin của các mã invest bất động sản<br/>
        /// sum_total_value = Sum(EP_INV_ORDER.TOTAL_VALUE)
        /// TotalAmountRemain = (EP_INV_PROJECT.TOTAL_INVESTMENT - sum_total_value)
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// </summary>
        /// <returns></returns>
        public ExportResultDto FindAllInvestCode(DateTime? startDate, DateTime? endDate)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var listInvestCode = new List<InvestCodeDto>();
            var result = new ExportResultDto();
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER) ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var investCodeFind = _exportExcelReportRepository.FindAllInvestCode(tradingProviderId, partnerId, null, null);

            foreach (var item in investCodeFind)
            {
                var investCodeItem = new InvestCodeDto()
                {
                    Id = item.Id,
                    InvCode = item.InvCode,
                    OwnerName = item.OwnerName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    OpenCellDate = item.OpenCellDate,
                    TotalInvestment = item.TotalInvestment,
                    TotalAmountRemain = item.TotalAmountRemain,
                };

                listInvestCode.Add(investCodeItem);
            }

            listInvestCode = listInvestCode.Where(l => l.OpenCellDate?.Date >= (startDate?.Date ?? DateTime.MinValue) && l.OpenCellDate?.Date <= (endDate?.Date ?? DateTime.MaxValue)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("InvestCode");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã dự án";
            worksheet.Cell(currentRow, 3).Value = "Chủ đầu tư";
            worksheet.Cell(currentRow, 4).Value = "Ngày mở bán";
            worksheet.Cell(currentRow, 5).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 6).Value = "Ngày phân phối đầu tư";
            worksheet.Cell(currentRow, 7).Value = "Hạn mức đầu tư";
            worksheet.Cell(currentRow, 8).Value = "Tổng giá trị còn lại";
            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 30;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 20;
            worksheet.Column("E").Width = 20;
            worksheet.Column("F").Width = 20;
            worksheet.Column("G").Width = 20;
            worksheet.Column("H").Width = 20;

            foreach (var item in listInvestCode)
            {

                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item.InvCode;
                worksheet.Cell(currentRow, 3).Value = item.OwnerName;
                worksheet.Cell(currentRow, 4).Value = item.StartDate;
                worksheet.Cell(currentRow, 5).Value = item.EndDate;
                worksheet.Cell(currentRow, 6).Value = item.OpenCellDate;
                worksheet.Cell(currentRow, 7).Value = ((double?)item.TotalInvestment);
                worksheet.Cell(currentRow, 8).Value = ((double?)item.TotalAmountRemain);

            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo tổng quan các khoản đầu tư<br/>
        /// CurrentInvestment tính bằng cách lấy từ hàm FUNC_CURRENT_PAYMENT_AMOUNT của INV_ORDER
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto ExportInvestmentReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var investmentList = new List<InvestmentsReportDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            //lấy tradingProviderId nếu user type là đại lý
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            //lấy partnerId nếu user type là đối tác
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;

            var investmentFind = _exportExcelReportRepository.FindExportInvestmentReport(tradingProviderId, null, null, partnerId);
            investmentFind = investmentFind.Where(i => i.TranDate != null && (startDate == null || startDate.Value.Date <= i.TranDate.Value.Date)
                                && (endDate == null || i.TranDate.Value.Date <= (endDate.Value.Date))).ToList();

            foreach (var item in investmentFind)
            {
                var investMentData = new InvestmentsReportDto();
                var orderFind = _investOrderEFRepository.FindById(item.OrderId);
                var policyFind = _policyRepository.FindPolicyById(item.PolicyId);
                var policyDetailFind = _policyRepository.FindPolicyDetailById(item.PolicyDetailId);
                var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyFind.DistributionId && r.Deleted == YesNo.NO);
                //nếu là lệnh của khách hàng cá nhân
                if (item.InvestorId != null && item.BusinessCustomerId == null)
                {
                    var saleInfo = _saleRepository.GetSaleInfoByRefferalCode(item.TradingProviderId, item.SaleReferralCode);
                    var saleInfoBusiness = _saleRepository.GetSaleInfoByRefferalCodeBusiness(item.TradingProviderId, item.SaleReferralCode);
                    var customerType = UserTypes.INVESTOR;
                    var investorIdentificationFind = _managerInvestorRepository.GetIdentificationById(item?.InvestorIdenId ?? 0);

                    investMentData.CustomerName = investorIdentificationFind?.Fullname;
                    investMentData.Sex = investorIdentificationFind?.Sex;
                    investMentData.IdNo = investorIdentificationFind?.IdNo;
                    investMentData.IdType = investorIdentificationFind?.IdType;
                    investMentData.PlaceOfResident = investorIdentificationFind?.PlaceOfResidence;

                    try
                    {
                        var investorBankFind = _investorBankAccountRepository.GetById(item?.InvestorBankAccId ?? 0);
                        var bankInfo = _bankRepository.GetById(investorBankFind?.BankId ?? 0);

                        //lấy thông tin ngân hàng của khách hàng cá nhân
                        investMentData.CustomerBankName = bankInfo?.BankName;
                        investMentData.BankAccNo = investorBankFind?.BankAccount;
                        investMentData.OwnerBankName = investorBankFind?.OwnerAccount;
                    }
                    catch
                    {
                    }

                    investMentData.CustomerType = customerType;

                    if (saleInfo != null && saleInfoBusiness == null)
                    {
                        investMentData.SaleName = saleInfo.Fullname;
                        investMentData.DepartmentName = saleInfo.DepartmentName;
                    }
                    else if (saleInfoBusiness != null && saleInfo == null)
                    {
                        investMentData.SaleName = saleInfoBusiness.Name;
                        investMentData.DepartmentName = saleInfoBusiness.DepartmentName;
                    }

                    try
                    {
                        investMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        investMentData.EndDate = null;
                    }
                }

                //nếu là lệnh của khách hàng doanh nghiệp
                else if (item.InvestorId == null && item.BusinessCustomerId != null)
                {
                    var saleInfo = _saleRepository.GetSaleInfoByRefferalCode(item.TradingProviderId, item.SaleReferralCode);
                    var saleInfoBusiness = _saleRepository.GetSaleInfoByRefferalCodeBusiness(item.TradingProviderId, item.SaleReferralCode);
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(item.BusinessCustomerId ?? 0);
                    var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(item.BusinessCustomerId ?? 0);

                    //lấy thông tin ngân hàng của khách hàng doanh nghiệp
                    investMentData.BankAccNo = businessCustomerBankFind?.BankAccNo;
                    investMentData.OwnerBankName = businessCustomerBankFind?.BankAccName;
                    investMentData.CustomerName = businessCustomer?.Name;

                    //nếu là sale tổ chức
                    if (saleInfoBusiness != null && saleInfo == null)
                    {
                        investMentData.SaleName = saleInfoBusiness.Name;
                        investMentData.DepartmentName = saleInfoBusiness.DepartmentName;
                    }
                    //nếu là sale cá nhân
                    else if (saleInfo != null && saleInfoBusiness == null)
                    {
                        investMentData.SaleName = saleInfo.Fullname;
                        investMentData.DepartmentName = saleInfo.DepartmentName;
                    }

                    try
                    {
                        investMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        investMentData.EndDate = null;
                    }
                }


                if (item.Source == SourceOrder.ONLINE)
                {
                    investMentData.OrderSource = ExcelDataUtils.OrderSourceDisplay(SourceOrderFE.KHACH_HANG);
                }
                else if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId != null)
                {
                    investMentData.OrderSource = ExcelDataUtils.OrderSourceDisplay(SourceOrderFE.SALE);
                }
                else if (item.Source == SourceOrder.OFFLINE && item.SaleOrderId == null)
                {
                    investMentData.OrderSource = ExcelDataUtils.OrderSourceDisplay(SourceOrderFE.QUAN_TRI_VIEN);
                }

                investMentData.TradingType = ExcelDataUtils.TradingTypeDisplay(item.Source);

                // Check thông tin xem có đang phong toả hay không
                investMentData.IsBlockage = item.Status == OrderStatus.PHONG_TOA ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;
                investMentData.InvCode = item.InvCode;
                //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                investMentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);
                investMentData.SaleReferralCode = item.SaleReferralCode;
                investMentData.ContractCode = _investOrderEFRepository.GetContractCodeGen(item.OrderId) ?? item.ContractCode;
                investMentData.CifCode = item.CifCode;
                investMentData.TranDate = item.TranDate;
                investMentData.Area = item.Area;
                investMentData.InvName = item.InvName;
                investMentData.PolicyCode = item.PolicyCode;
                investMentData.InvestDate = item.InvestDate;
                investMentData.Profit = item.Profit + "%";
                investMentData.InitTotalValue = item.InitTotalValue;
                investMentData.PaymentAmnount = item.PaymentAmnount;
                investMentData.Status = item.Status;
                investMentData.CurrentInvestment = item?.Status == OrderStatus.TAT_TOAN ? 0 : item.CurrentInvestment;
                investMentData.CalculateType = ExcelDataUtils.CaculateTypeDisplay(item.CalculateType);
                investmentList.Add(investMentData);
            }

            investmentList = investmentList.Where(i => (startDate?.Date ?? DateTime.MinValue) <= i.TranDate?.Date && i.TranDate?.Date <= (endDate?.Date ?? DateTime.MaxValue)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 5).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Địa chỉ thường trú";
            worksheet.Cell(currentRow, 7).Value = "Mã KH";
            worksheet.Cell(currentRow, 8).Value = "Giới tính";
            worksheet.Cell(currentRow, 9).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 10).Value = "Tài khoản nhận tiền KH";
            worksheet.Cell(currentRow, 11).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 12).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 13).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 14).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 15).Value = "Phòng ban";
            worksheet.Cell(currentRow, 16).Value = "Kiểu giao dịch";
            worksheet.Cell(currentRow, 17).Value = "Nguồn đặt";
            worksheet.Cell(currentRow, 18).Value = "Dự án";
            worksheet.Cell(currentRow, 19).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 20).Value = "Phong toả";
            worksheet.Cell(currentRow, 21).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 22).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 23).Value = "Thời hạn";
            worksheet.Cell(currentRow, 24).Value = "Tình trạng";
            worksheet.Cell(currentRow, 25).Value = "Lợi nhuận";
            worksheet.Cell(currentRow, 26).Value = "Gross/Net";
            worksheet.Cell(currentRow, 27).Value = "Giá trị đầu tư theo HĐ";
            worksheet.Cell(currentRow, 28).Value = "Số tiền KH chuyển";
            worksheet.Cell(currentRow, 29).Value = "Giá trị đầu tư hiện tại";
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
            worksheet.Column("Z").Width = 40;
            worksheet.Column("AA").Width = 40;
            worksheet.Column("AB").Width = 40;
            worksheet.Column("AC").Width = 40;

            foreach (var item in investmentList)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.TranDate?.Date;
                worksheet.Cell(currentRow, 3).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 5).Value = item?.IdType;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.PlaceOfResident;
                worksheet.Cell(currentRow, 7).Value = "'" + item?.CifCode;
                worksheet.Cell(currentRow, 8).Value = (item.CustomerType == UserTypes.INVESTOR && item.Sex != null) ? ExcelDataUtils.GenderDisplay(item?.Sex) : "";
                worksheet.Cell(currentRow, 9).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAccNo;
                worksheet.Cell(currentRow, 11).Value = item?.OwnerBankName;
                worksheet.Cell(currentRow, 12).Value = item?.CustomerBankName;
                worksheet.Cell(currentRow, 13).Value = "'" + item?.SaleReferralCode;
                worksheet.Cell(currentRow, 14).Value = item?.SaleName;
                worksheet.Cell(currentRow, 15).Value = item?.DepartmentName;
                worksheet.Cell(currentRow, 16).Value = item?.TradingType;
                worksheet.Cell(currentRow, 17).Value = item?.OrderSource;
                worksheet.Cell(currentRow, 18).Value = item?.InvName;
                worksheet.Cell(currentRow, 19).Value = "'" + item?.PolicyCode;
                worksheet.Cell(currentRow, 20).Value = item?.IsBlockage;
                worksheet.Cell(currentRow, 21).Value = item?.InvestDate?.Date;
                worksheet.Cell(currentRow, 22).Value = item?.EndDate;
                worksheet.Cell(currentRow, 23).Value = item?.PeriodTime;
                worksheet.Cell(currentRow, 24).Value = ExcelDataUtils.StatusOrder(item.Status ?? 0);
                worksheet.Cell(currentRow, 25).Value = item?.Profit;
                worksheet.Cell(currentRow, 26).Value = item?.CalculateType;
                worksheet.Cell(currentRow, 27).Value = item?.InitTotalValue;
                worksheet.Cell(currentRow, 28).Value = item?.PaymentAmnount;
                worksheet.Cell(currentRow, 29).Value = item?.CurrentInvestment;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo thống kê dự chi đến hạn
        /// </summary>
        /// <returns></returns>
        public async Task<ExportResultDto> DueExpendExcelReport(DateTime? startDate, DateTime? endDate)
        {
            //lấy trading provider id của tài khoản hiện tại
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            List<int> tradingProviderIds = new List<int>();

            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                                     ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;

            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                                    ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            if (tradingProviderId != null)
            {
                tradingProviderIds.Add(tradingProviderId ?? 0);
            }
            else
            {
                tradingProviderIds.AddRange(_tradingProviderEFRepository.GetAllByPartnerId(partnerId ?? 0).Select(o => o.TradingProviderId).ToList());
            }

            var listOrderFind = await _orderServices.LayDanhSachNgayChiTra(new FilterCalulationInterestPayment
            {
                NgayChiTra = startDate,
                TradingProviderChildIds = tradingProviderIds
            });
            listOrderFind = listOrderFind.Where(r => tradingProviderIds.Contains(r.TradingProviderId ?? 0)
                            && ((startDate == null ? DateTime.Now.Date : r.PayDate.Date) >= startDate.Value.Date) && (endDate != null && r.PayDate.Date <= endDate.Value.Date)).ToList();

            var duLieuBaoCaoDuChi = new List<DueExpendReportDto>();
            var result = new ExportResultDto();

            // lấy list danh sách hợp đồng
            foreach (var dataItem in listOrderFind)
            {
                var orderFind = _investOrderRepository.FindById(dataItem.OrderId);
                var projectFind = _projectRepository.FindById(orderFind?.ProjectId ?? 0);
                var dueExpend = new DueExpendReportDto();
                var policyFind = _policyRepository.FindPolicyById(dataItem.PolicyId ?? 0);
                var policyDetailFind = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);

                if (policyFind == null)
                {
                    continue;
                }
                dueExpend.Status = ExcelDataUtils.StatusOrder(orderFind.Status);
                dueExpend.InvestDate = orderFind.InvestDate;
                dueExpend.CurrentInvestment = orderFind.TotalValue; //Tổng giá trị đầu tư
                dueExpend.CifCode = dataItem.CifCode; //Mã khách hàng
                dueExpend.ProjectCode = projectFind?.InvCode; //mã dự án
                dueExpend.CurrentPeriodPaymentDate = dataItem.PayDate;
                dueExpend.LastPeriodPayment = (dataItem.IsLastPeriod) ? orderFind.TotalValue : 0;

                //dueExpend.ProjectBankAccNo = dataItem.ProjectBankAccNo; // số tài khoản đại lý nhận tiền của dự án đó
                //dueExpend.ContractCode = dataItem.ContractCode;
                var contractCode = _dbContext.InvestOrderContractFile.FirstOrDefault(o => o.OrderId == orderFind.Id)?.ContractCodeGen;
                dueExpend.ContractCode = contractCode ?? dataItem.ContractCode;
                dueExpend.PeriodIndex = dataItem.PeriodIndex;
                dueExpend.Profit = dataItem?.Profit + "%";
                dueExpend.PeriodTime = policyDetailFind?.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(policyDetailFind.PeriodType);
                dueExpend.PreviousPeriodPaymentDate = dataItem.InvestLastPayDate;

                decimal? loiTucKyNay = 0;
                decimal? thue = 0;
                decimal? tongTienThucNhan = 0;
                decimal thueLoiNhuan = 0;
                var soTienCuaKyTruoc = 0; //item.TotalValue * (item.Profit / 100) * item.PayDate.Date.Subtract(item.LastPayDate.Value.Date).Days / 365;

                var cifCodeFind = _cifCodeRepository.GetByCifCode(dataItem.CifCode);

                if (cifCodeFind.InvestorId != null && cifCodeFind.BusinessCustomerId == null)
                {
                    thueLoiNhuan = (policyFind.IncomeTax ?? 0) / 100;

                    var investorIdentification = _managerInvestorRepository
                        .GetIdentificationById(orderFind.InvestorIdenId ?? 0); //lấy thông tin của khách hàng cá nhân thông qua giấy tờ mặc định

                    dueExpend.CustomerName = investorIdentification?.Fullname;
                    dueExpend.IdNo = investorIdentification?.IdNo;
                    dueExpend.IdType = investorIdentification?.IdType;

                    // lấy kì trả lớn nhất theo orderId và tradingProviderId trong interestPayment
                    var ngayDaTraKiGanNhat = _interestPaymentRepository
                        .GetInterestPaymentPayDateClosestPeriod(orderFind.TradingProviderId, (int)dataItem.OrderId); //lấy thông tin ngày chi trả của kì gần nhất

                    var investorBankFind = _managerInvestorRepository.GetBankById(orderFind.InvestorBankAccId ?? 0);
                    var bankInfo = _bankRepository.GetById(investorBankFind?.BankId ?? 0);

                    dueExpend.BankAccNo = investorBankFind?.BankAccount;
                    dueExpend.BankAccName = investorBankFind?.OwnerAccount;
                    dueExpend.BankName = bankInfo?.BankName;

                }
                else if (cifCodeFind.InvestorId == null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    dueExpend.CustomerName = businessCustomer?.Name;
                    dueExpend.IdNo = businessCustomer.TaxCode;
                    dueExpend.IdType = TaxCode.MA_SO_THUE;

                    var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(orderFind?.InvestorBankAccId ?? 0);
                    dueExpend.BankAccNo = businessCustomerBank?.BankAccNo;
                    dueExpend.BankAccName = businessCustomerBank?.BankAccName;

                    var bankInfo = _bankRepository.GetById(businessCustomerBank?.BankId ?? 0);
                    dueExpend.BankName = bankInfo?.BankName;
                }
                //Chi trả lợi tức chưa bao gồm thuế`
                if (policyFind.CalculateType == CalculateTypes.GROSS)
                {
                    loiTucKyNay = (dataItem.TotalValue * (dataItem.Profit / 100) * dataItem.SoNgay / 365) - soTienCuaKyTruoc;
                    thue = loiTucKyNay * thueLoiNhuan;
                    tongTienThucNhan = loiTucKyNay - thue;
                }

                //Chi trả lợi tức đã bao gồm thuế
                else if (policyFind.CalculateType == CalculateTypes.NET)
                {
                    tongTienThucNhan = (dataItem.TotalValue * (dataItem.Profit / 100) * dataItem.SoNgay / 365) - soTienCuaKyTruoc;
                    loiTucKyNay = tongTienThucNhan / (1 - thueLoiNhuan);
                    thue = loiTucKyNay - tongTienThucNhan;
                }
                //Nếu là kỳ cuối
                if (dataItem.IsLastPeriod)
                {
                    decimal? loiTucCaKyThucTe = 0;

                    //Chi trả lợi tức chưa bao gồm thuế
                    if (policyFind.CalculateType == CalculateTypes.GROSS)
                    {
                        loiTucKyNay = (dataItem.TotalValue * (dataItem.Profit / 100) * dataItem.SoNgay / 365) - soTienCuaKyTruoc;
                        thue = loiTucKyNay * thueLoiNhuan;
                        loiTucCaKyThucTe = loiTucKyNay - thue;
                    }

                    //Chi trả lợi tức đã bao gồm thuế
                    else if (policyFind.CalculateType == CalculateTypes.NET)
                    {
                        loiTucCaKyThucTe = (dataItem.TotalValue * (dataItem.Profit / 100) * dataItem.SoNgay / 365) - soTienCuaKyTruoc;
                        loiTucKyNay = loiTucCaKyThucTe / (1 - thueLoiNhuan);
                        thue = loiTucKyNay - loiTucCaKyThucTe;
                    }

                    //số tiền kỳ cuối nhận được
                    tongTienThucNhan = loiTucCaKyThucTe + dataItem.TotalValue;
                }

                dueExpend.InterestDue = Math.Round(loiTucKyNay ?? 0);
                dueExpend.IncomeTax = Math.Round(thue ?? 0);
                dueExpend.ActualReceiveMoney = Math.Round(tongTienThucNhan ?? 0);

                duLieuBaoCaoDuChi.Add(dueExpend);
            }

            duLieuBaoCaoDuChi = duLieuBaoCaoDuChi.OrderBy(dc => dc.CurrentPeriodPaymentDate).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 3).Value = "Ngày đến hạn trả";
            worksheet.Cell(currentRow, 4).Value = "Ngày trả kỳ trước";
            worksheet.Cell(currentRow, 5).Value = "Số hợp đồng";
            worksheet.Cell(currentRow, 6).Value = "Mã KH";
            worksheet.Cell(currentRow, 7).Value = "Tên KH";
            worksheet.Cell(currentRow, 8).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 9).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 10).Value = "Mã dự án đầu tư";
            worksheet.Cell(currentRow, 11).Value = "Thời hạn đầu tư";
            worksheet.Cell(currentRow, 12).Value = "Lợi tức";
            worksheet.Cell(currentRow, 13).Value = "Tổng giá trị đầu tư hiện tại";
            worksheet.Cell(currentRow, 14).Value = "Gốc đến hạn trả";
            worksheet.Cell(currentRow, 15).Value = "Lãi đến hạn trả trước thuế";
            worksheet.Cell(currentRow, 16).Value = "Thuế TNCN";
            worksheet.Cell(currentRow, 17).Value = "Số tiền KH thực nhận";
            worksheet.Cell(currentRow, 18).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 19).Value = "Số tài khoản";
            worksheet.Cell(currentRow, 20).Value = "Ngân hàng nhận";
            worksheet.Cell(currentRow, 21).Value = "Trạng thái";

            worksheet.Column("A").Width = 20;
            worksheet.Column("B").Width = 20;
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
            worksheet.Column("U").Width = 40;
            worksheet.Column("V").Width = 40;
            worksheet.Column("X").Width = 40;
            worksheet.Column("W").Width = 40;

            int stt = 0;
            foreach (var item in duLieuBaoCaoDuChi)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = ++stt; // Số thứ tự trong excel
                worksheet.Cell(currentRow, 2).Value = item.InvestDate?.Date; //Ngày đầu tư
                worksheet.Cell(currentRow, 3).Value = item?.CurrentPeriodPaymentDate; //Ngày chi trả của kì hiện tại
                worksheet.Cell(currentRow, 4).Value = item?.PreviousPeriodPaymentDate; //Ngày chi trả của kì trước
                worksheet.Cell(currentRow, 5).Value = "'" + item?.ContractCode; //Số hợp đồng
                worksheet.Cell(currentRow, 6).Value = "'" + item?.CifCode; //Mã của khách hàng
                worksheet.Cell(currentRow, 7).Value = item?.CustomerName; //Tên khách hàng
                worksheet.Cell(currentRow, 8).Value = "'" + item?.IdNo; //Số giấy tờ
                worksheet.Cell(currentRow, 9).Value = item?.IdType; //Loại giấy tờ
                worksheet.Cell(currentRow, 10).Value = "'" + item?.ProjectCode; //Mã dự án
                worksheet.Cell(currentRow, 11).Value = "'" + item?.PeriodTime; //Mã dự án
                worksheet.Cell(currentRow, 12).Value = "'" + item?.Profit; //Mã dự án
                worksheet.Cell(currentRow, 13).Value = item?.CurrentInvestment; //Số tiền đầu tư hiện tại
                worksheet.Cell(currentRow, 14).Value = item?.LastPeriodPayment; //gốc đến hạn trả 
                worksheet.Cell(currentRow, 15).Value = item?.InterestDue; //Lãi đến hạn trả
                worksheet.Cell(currentRow, 16).Value = item?.IncomeTax; //Thuế thu nhập cá nhân
                worksheet.Cell(currentRow, 17).Value = item?.ActualReceiveMoney; // số tiền khách hàng thực nhận
                worksheet.Cell(currentRow, 18).Value = item?.BankAccName; //Tên chủ tài khoản ngân hàng
                worksheet.Cell(currentRow, 19).Value = "'" + item?.BankAccNo; //Số tài khoản
                worksheet.Cell(currentRow, 20).Value = item?.BankName; //Tên ngân hàng
                worksheet.Cell(currentRow, 21).Value = item?.Status;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Rút vốn
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="soTienRut"></param>
        /// <param name="ngayRut"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public RutVonDto RutVon(long orderId, decimal soTienRut, DateTime ngayRut)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            #region Lấy Id từ các bảng
            //lấy thông tin lệnh
            var orderFind = _orderRepository.FindById(orderId, tradingProviderId);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }

            if (orderFind.Status != InvestOrderStatus.DANG_DAU_TU)
            {
                throw new FaultException(new FaultReason($"Hợp đồng không trong trạng thái hoạt động không thể rút vốn"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var cifFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
            if (cifFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mã cif"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }

            bool khachCaNhan = cifFind.InvestorId != null;

            var policyDetail = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }

            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == orderFind.DistributionId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
            #endregion

            //Số tiền đang còn được đầu tư của hợp đồng
            var soTienDangDauTu = orderFind.TotalValue;

            var result = _investSharedServices.RutVonInvest(orderFind, policy, policyDetail, soTienDangDauTu, soTienRut, ngayRut, khachCaNhan, distribution.CloseCellDate);
            return result;
        }

        /// <summary>
        /// Báo cáo thực chi
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto ActualExpendReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var listBaoCaoThucChi = new List<ActualExpendReportDto>();
            var actualExpendList = _exportExcelReportRepository.ActualExpendReport(null, null, tradingProviderId).GroupBy(x => x.ContractCode).Select(x => x.First()).ToList();

            foreach (var data in actualExpendList)
            {
                var rutVonFind = new RutVonDto();
                var actualExpend = new ActualExpendReportDto();
                var orderFind = _orderRepository.FindById(data.OrderId);
                var policyDetail = _policyRepository.FindPolicyDetailById(orderFind?.PolicyDetailId ?? 0);
                var policyFind = _policyRepository.FindPolicyById(policyDetail?.PolicyId ?? 0, policyDetail?.TradingProviderId ?? 0);
                var distribution = _distributionRepository.FindById(policyFind?.DistributionId ?? 0, policyFind?.TradingProviderId);
                var projectFind = _projectRepository.FindById(distribution?.ProjectId ?? 0);
                var calculateListInterest = new ProfitAndInterestDto();

                DateTime ngayBatDauTinhLai = data.InvestDate != null ? data?.InvestDate ?? new DateTime() : DateTime.Now.Date;

                if (data.InvestorId != null && data.BusinessCustomerId == null)
                {
                    var investorFind = _managerInvestorRepository.GetDefaultIdentification(data.InvestorId ?? 0, false);
                    var investorBankFind = _investorBankAccountRepository.GetByInvestorId(data.InvestorId ?? 0);

                    //Tính toán thông tin lãi trước thuế
                    calculateListInterest = _investSharedServices.CalculateListInterest(projectFind, policyFind, policyDetail, ngayBatDauTinhLai, data.TotalValue, true, distribution.CloseCellDate, data.OrderId);

                    actualExpend.CustomerName = investorFind?.Fullname;
                    actualExpend.IdNo = investorFind?.IdNo;
                    actualExpend.IdType = investorFind?.IdType;
                    actualExpend.ProjectBankAccount = investorBankFind?.BankAccount;
                }
                else if (data.InvestorId != null && data.BusinessCustomerId == null)
                {
                    //Tính toán thông tin lãi trước thuế
                    calculateListInterest = _investSharedServices.CalculateListInterest(projectFind, policyFind, policyDetail, ngayBatDauTinhLai, data.TotalValue, false, distribution.CloseCellDate, data.OrderId);

                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(data.BusinessCustomerId ?? 0);
                    var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(data.BusinessCustomerId ?? 0);
                    actualExpend.ProjectBankAccount = businessCustomerBank?.BankAccNo;
                    actualExpend.CustomerName = businessCustomer.Name;
                }

                actualExpend.ContractCode = data.ContractCode;
                //nếu đã rút vốn thì tính toán lại thuế thu nhập cá nhân, lãi theo rút vốn

                actualExpend.ExpendDate = data.PayDate;
                actualExpend.CifCode = data.CifCode;
                actualExpend.ProjectCode = projectFind?.InvCode; //Mã dự án
                actualExpend.TotalInvestment = projectFind?.TotalInvestment ?? 0;//Tổng giá trị đầu tư 
                actualExpend.PrincipalAmount = data.InitTotalValue;
                var listThucChi = new List<InvestOrderInterestAndWithdrawalDto>();
                try
                {
                    listThucChi = _orderServices.GetProfitInfo(data.OrderId).ActualCashFlow;
                }
                catch
                {
                }

                for (int i = 0; i <= listThucChi.Count - 1; i++)
                {
                    var actualExpendCopy = new ActualExpendReportDto(actualExpend)
                    {
                        ExpendDate = listThucChi[i].PayDate?.Date,
                        ExpendType = listThucChi[i].Description,
                        IncomeTax = listThucChi[i].Tax ?? 0,
                        DeductibleProfit = listThucChi[i].DeductibleProfit ?? 0,
                        ReceivedMoney = listThucChi[i].ActuallyAmount ?? 0,
                        Interest = listThucChi[i].Profit ?? 0,
                        WithdrawalSettlement = listThucChi[i].WithdrawalMoney ?? 0,
                        CurrentInvestment = listThucChi[i].Surplus, //Tổng giá trị đầu tư hiện tại = tổng số tiền lũy kế - số tiền gốc đã chi
                    };
                    listBaoCaoThucChi.Add(actualExpendCopy);
                }
            }
            listBaoCaoThucChi = listBaoCaoThucChi.OrderBy(x => x.ExpendDate).ToList();
            listBaoCaoThucChi = listBaoCaoThucChi.Where(l => l.ExpendDate?.Date >= (startDate?.Date ?? DateTime.MinValue) && l.ExpendDate?.Date <= (endDate?.Date ?? DateTime.MaxValue)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày chi";
            worksheet.Cell(currentRow, 3).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 4).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 7).Value = "Mã KH";
            worksheet.Cell(currentRow, 8).Value = "Mã dự án đầu tư";
            worksheet.Cell(currentRow, 9).Value = "Tổng giá trị đầu tư";
            worksheet.Cell(currentRow, 10).Value = "Tổng giá trị đầu tư hiện tại";
            worksheet.Cell(currentRow, 11).Value = "Loại chi";
            worksheet.Cell(currentRow, 12).Value = "Tiền gốc";
            worksheet.Cell(currentRow, 13).Value = "Giá trị rút vốn/ tất toán";
            worksheet.Cell(currentRow, 14).Value = "Tiền lãi";
            worksheet.Cell(currentRow, 15).Value = "Thuế TNCN";
            worksheet.Cell(currentRow, 16).Value = "Lợi nhuận khấu trừ";
            worksheet.Cell(currentRow, 17).Value = "Số tiền nhận";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
            worksheet.Column("E").Width = 40;
            worksheet.Column("F").Width = 40;
            worksheet.Column("G").Width = 40;
            worksheet.Column("H").Width = 40;
            worksheet.Column("I").Width = 40;
            worksheet.Column("I").Style.NumberFormat.Format = "0";
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

            var stt = 0;

            foreach (var item in listBaoCaoThucChi)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt; //Số thứ tự trong excel
                worksheet.Cell(currentRow, 2).Value = item?.ExpendDate;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 4).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 6).Value = item?.IdType;
                worksheet.Cell(currentRow, 7).Value = "'" + item?.CifCode;
                worksheet.Cell(currentRow, 8).Value = "'" + item?.ProjectCode;
                worksheet.Cell(currentRow, 9).Value = item?.TotalInvestment;
                worksheet.Cell(currentRow, 10).Value = item?.CurrentInvestment;
                worksheet.Cell(currentRow, 11).Value = item?.ExpendType;
                worksheet.Cell(currentRow, 12).Value = item?.PrincipalAmount;
                worksheet.Cell(currentRow, 13).Value = item?.WithdrawalSettlement;
                worksheet.Cell(currentRow, 14).Value = item?.Interest;
                worksheet.Cell(currentRow, 15).Value = item?.IncomeTax;
                worksheet.Cell(currentRow, 16).Value = item?.DeductibleProfit;
                worksheet.Cell(currentRow, 17).Value = item?.ReceivedMoney;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo tổng hợp vận hành HVF
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<TotalOperationReportDto> TotalOperationReport(DateTime? startDate, DateTime? endDate)
        {
            var dates = new List<DateTime?>();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new List<TotalOperationReportDto>();

            for (DateTime? date = startDate; date <= endDate; date = date?.AddDays(1))
            {
                dates.Add(date);
            }

            foreach (var date in dates)
            {
                var totalOperation = _exportExcelReportRepository.GetTotalOperationReport(date, tradingProviderId);
                result.Add(totalOperation);
            }
            return result;
        }

        /// <summary>
        /// Báo cáo tổng tiền chi trả đầu tư của tất cả hợp đồng trong ngày<br>
        /// thuế lợi nhuận = thuế thu nhâp cá nhân / 100 
        /// CurrentInvestment( Tổng giá trị đầu tư hiện tại) = tổng số tiền lũy kế - số tiền gốc đã chi
        /// PrincipalAmount( Số tiền gốc ) = giá trị đầu tư hiện tại - giá trị rút vốn( tất toán)
        /// giá trị rút vốn > 0 thì: số tiền thực nhận = giá trị rút vốn,tất toán - tiền lãi - thuế thu nhập cá nhân - lợi nhuận khấu trừ
        /// nếu số kỳ mà bằng tổng số lần có lợi nhuận tức là (data.PeriodIndex == calculateListInterest.ProfitInfo.Count) thì:  số tiền thực nhận = tiền gốc + tiền lãi - thuế thu nhập cá nhân
        /// nếu 2 điều kiện trên đều không phải thì số tiền thực nhân = tiền lãi - thuế thu nhập cá nhân
        /// </br>
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto ActualTotalExpendInDay(DateTime? startDate, DateTime? endDate)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var actualExpendList = _exportExcelReportRepository.ActualExpendReport(null, null, tradingProviderId);
            var listBaoCaoThucChi = new List<ActualExpendReportDto>();
            var listBaoCaoChiTraTrongNgay = new List<ActualTotalSpend>();

            foreach (var data in actualExpendList)
            {
                var rutVonFind = new RutVonDto();
                var actualExpend = new ActualExpendReportDto();
                var orderFind = _orderRepository.FindById(data.OrderId);
                var policyDetail = _policyRepository.FindPolicyDetailById(orderFind?.PolicyDetailId ?? 0);
                var policyFind = _policyRepository.FindPolicyById(policyDetail?.PolicyId ?? 0, policyDetail?.TradingProviderId ?? 0);
                var distribution = _distributionRepository.FindById(policyFind?.DistributionId ?? 0, policyFind?.TradingProviderId);
                var projectFind = _projectRepository.FindById(distribution?.ProjectId ?? 0);
                var calculateListInterest = new ProfitAndInterestDto();

                // != null thì trả về data.investDate nếu null thì trả về thời gian hiện tại
                DateTime ngayBatDauTinhLai = data.InvestDate != null ? data.InvestDate ?? new DateTime() : DateTime.Now.Date;

                if (data.InvestorId != null && data.BusinessCustomerId == null)
                {

                    var investorFind = _managerInvestorRepository.GetDefaultIdentification(data.InvestorId ?? 0, false);
                    var investorBankFind = _investorBankAccountRepository.GetByInvestorId(data.InvestorId ?? 0);

                    //Tính toán thông tin lãi trước thuế
                    calculateListInterest = _investSharedServices.CalculateListInterest(projectFind, policyFind, policyDetail, ngayBatDauTinhLai, data.TotalValue, true, distribution.CloseCellDate, data.OrderId);

                    actualExpend.CustomerName = investorFind?.Fullname;
                    actualExpend.IdNo = investorFind?.IdNo;
                    actualExpend.IdType = investorFind?.IdType;
                    actualExpend.ProjectBankAccount = investorBankFind?.BankAccount;
                }
                else if (data.InvestorId != null && data.BusinessCustomerId == null)
                {

                    //Tính toán thông tin lãi trước thuế
                    calculateListInterest = _investSharedServices.CalculateListInterest(projectFind, policyFind, policyDetail, ngayBatDauTinhLai, data.TotalValue, false, distribution.CloseCellDate, data.OrderId);

                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(data.BusinessCustomerId ?? 0);
                    var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(data.BusinessCustomerId ?? 0);
                    actualExpend.ProjectBankAccount = businessCustomerBank?.BankAccNo;
                    actualExpend.CustomerName = businessCustomer.Name;
                }

                var thueLoiNhuan = (data.IncomeTax) / 100;
                var soTienChi = _orderRepository.InterestPaymentSumMoney(data.PayDate.Date, data.OrderId, tradingProviderId);

                if (data.WithdrawalDate != new DateTime())
                {
                    try
                    {
                        rutVonFind = RutVon(data.OrderId, data.AmountMoney ?? 0, data.WithdrawalDate);
                    }
                    catch
                    {
                        rutVonFind = null;
                    }
                }

                actualExpend.ContractCode = data.ContractCode;

                for (int i = 0; i < calculateListInterest.ProfitInfo.Count; i++)
                {
                    if (i < data.PeriodIndex)
                    {
                        actualExpend.IncomeTax += calculateListInterest.ProfitInfo[i].Tax ?? 0;
                        actualExpend.Interest += calculateListInterest.ProfitInfo[i].Profit ?? 0;
                    }
                }

                if (data.PeriodIndex == 0)
                {
                    actualExpend.IncomeTax = calculateListInterest.ProfitInfo.Sum(p => p.Tax) ?? 0;
                    actualExpend.Interest = calculateListInterest.ProfitInfo.Sum(p => p.Profit) ?? 0;
                }

                //nếu đã rút vốn thì tính toán lại thuế thu nhập cá nhân, lãi theo rút vốn
                if (rutVonFind?.IncomeTax > 0 || rutVonFind?.ProfitReceived > 0 || rutVonFind?.WithdrawalProfit > 0)
                {
                    actualExpend.IncomeTax = rutVonFind?.IncomeTax ?? 0; //Thuế thu nhập cá nhân
                    actualExpend.DeductibleProfit = rutVonFind?.ProfitReceived ?? 0;
                    actualExpend.Interest = rutVonFind?.WithdrawalProfit ?? 0;
                }

                actualExpend.ExpendDate = data.PayDate;
                actualExpend.CifCode = data.CifCode;
                actualExpend.ProjectCode = data.InvCode; //Mã dự á
                actualExpend.TotalInvestment = data.TotalInvestment;//Tổng giá trị đầu tư 
                actualExpend.CurrentInvestment = data.CurrentInvestment; //Tổng giá trị đầu tư hiện tại = tổng số tiền lũy kế - số tiền gốc đã chi
                actualExpend.PrincipalAmount = data.CurrentInvestment - (actualExpend.WithdrawalSettlement);

                var theoDoiRutVon = _orderServices.TheoDoiRutTruocHan(data.OrderId);

                var thongTinChiTietRutVon = theoDoiRutVon.FirstOrDefault(rv => rv.WithdrawlDate == data.WithdrawalDate);

                if (data.AmountMoney != null && data.ExpendType == ExpendTypes.RUT_VON && thongTinChiTietRutVon != null)
                {
                    actualExpend.ExpendType = "Rút vốn lần " + thongTinChiTietRutVon.WithdrawalIndex;
                }
                else if (data.AmountMoney == data.TotalInvestment && data.IsLastPeriod == YesNo.NO && data.ExpendType == ExpendTypes.TAT_TOAN)
                {
                    actualExpend.ExpendType = "Tất toán trước hạn";
                }
                else if (data.IsLastPeriod == YesNo.YES)
                {
                    actualExpend.ExpendType = "Tất toán";
                    actualExpend.WithdrawalSettlement = actualExpend.PrincipalAmount;
                }
                else if (data.PeriodIndex != null)
                {
                    actualExpend.ExpendType = "Lợi nhuận kỳ " + data.PeriodIndex;
                }

                //int soNgayDaDauTu = (data.PayDate.Date - data.InvestDate.Value.Date).Days;

                actualExpend.ExpendAmount = soTienChi + (data?.AmountMoney ?? 0);
                if (actualExpend.WithdrawalSettlement > 0)
                {
                    actualExpend.ReceivedMoney = actualExpend.WithdrawalSettlement + actualExpend.Interest - actualExpend.IncomeTax - actualExpend.DeductibleProfit;
                }
                else if (data.PeriodIndex == calculateListInterest.ProfitInfo.Count)
                {
                    actualExpend.ReceivedMoney = actualExpend.PrincipalAmount + actualExpend.Interest - actualExpend.IncomeTax;
                }
                else
                {
                    actualExpend.ReceivedMoney = actualExpend.Interest - actualExpend.IncomeTax;
                }

                listBaoCaoThucChi.Add(actualExpend);
            }

            listBaoCaoThucChi = listBaoCaoThucChi.Where(l => l.ExpendDate?.Date >= (startDate?.Date ?? DateTime.MinValue) && l.ExpendDate?.Date <= (endDate?.Date ?? DateTime.MaxValue)).ToList();

            var listBaoCao = listBaoCaoThucChi
                .GroupBy(l => l.ExpendDate?.Date)
                .Select(sl => new ActualExpendReportDto
                {
                    ExpendDate = sl.First().ExpendDate,
                    ReceivedMoney = sl.Sum(s => s.ReceivedMoney)
                }).ToList();

            var result = new ExportResultDto();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("BaoCaoTongChiTraDauTu");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày chi trả";
            worksheet.Cell(currentRow, 3).Value = "Số tiền thực chi";
            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 40;

            var stt = 0;
            foreach (var item in listBaoCao.OrderBy(l => l.ExpendDate))
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt; //Số thứ tự trong excel
                worksheet.Cell(currentRow, 2).Value = item?.ExpendDate; //Ngày chi trả
                worksheet.Cell(currentRow, 3).Value = item?.ReceivedMoney; //Tổng số tiền chi trả tất cả hợp đồng trong ngày
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
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

        /// <summary>
        /// Báo cáo tổng các khoản đầu tư của đối tác HVF
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto ExportInvestmentHVFReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var investmentList = new List<InvestmentsReportDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER) ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var investmentFind = _exportExcelReportRepository.FindExportInvestmentReportHVF(tradingProviderId, null, null, partnerId);

            foreach (var item in investmentFind)
            {
                var investMentData = new InvestmentsReportDto();
                var orderFind = _investOrderEFRepository.FindById(item.OrderId);
                var cifCodeFind = _cifCodeRepository.GetByCifCode(item.CifCode);
                var policyFind = _policyRepository.FindPolicyById(item.PolicyId);
                var policyDetailFind = _policyRepository.FindPolicyDetailById(item.PolicyDetailId);
                var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyFind.DistributionId && r.Deleted == YesNo.NO);

                if (cifCodeFind.InvestorId != null && cifCodeFind.BusinessCustomerId == null)
                {
                    var customerType = UserTypes.INVESTOR;
                    var investorIdentificationFind = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);

                    investMentData.CustomerName = investorIdentificationFind?.Fullname;
                    investMentData.Sex = investorIdentificationFind?.Sex;
                    investMentData.IdNo = investorIdentificationFind?.IdNo;
                    investMentData.IdType = investorIdentificationFind?.IdType;
                    investMentData.PlaceOfResident = investorIdentificationFind?.PlaceOfResidence;

                    try
                    {
                        var investorBankFind = _investorBankAccountRepository.GetById(item?.InvestorBankAccId ?? 0);
                        var bankInfo = _bankRepository.GetById(investorBankFind.BankId);
                        investMentData.BankAccNo = investorBankFind?.BankAccount;
                        investMentData.OwnerBankName = investorBankFind?.OwnerAccount;
                        investMentData.CustomerBankName = bankInfo?.BankName;

                        //lấy thông tin ngân hàng của khách hàng cá nhân
                        investMentData.CustomerBankName = bankInfo?.BankName;
                        investMentData.BankAccNo = investorBankFind?.BankAccount;
                        investMentData.OwnerBankName = investorBankFind?.OwnerAccount;
                    }
                    catch
                    {
                    }

                    //check souce nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    investMentData.TradingType = ExcelDataUtils.TradingTypeDisplay(item.Source);

                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    investMentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);

                    // Check thông tin xem có đang phong toả hay không
                    investMentData.IsBlockage = item.Status == 6 ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;

                    investMentData.CustomerType = customerType;
                    investMentData.TranDate = item.TranDate;
                    investMentData.CifCode = item.CifCode;
                    investMentData.ContractCode = item.ContractCode;
                    investMentData.SaleReferralCode = item.SaleReferralCode?.ToString();
                    investMentData.Area = item.Area;
                    investMentData.InvName = item.InvName;
                    investMentData.PolicyCode = item.PolicyCode;
                    investMentData.InvestDate = item.InvestDate;
                    investMentData.Status = item.Status;
                    investMentData.Profit = item.Profit + "%";
                    investMentData.TotalValue = item.TotalValue;
                    investMentData.PaymentAmnount = item.PaymentAmnount;
                    investMentData.InvCode = item.InvCode;
                    try
                    {
                        investMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        investMentData.EndDate = null;
                    }

                    investMentData.CurrentInvestment = item.CurrentInvestment;
                }
                else if (cifCodeFind.InvestorId == null && cifCodeFind.BusinessCustomerId != null)
                {

                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);

                    investMentData.CustomerName = businessCustomer?.Name;
                    investMentData.IdNo = businessCustomer.TaxCode;
                    investMentData.IdType = TaxCode.MA_SO_THUE;
                    try
                    {
                        var investorBankFind = _investorBankAccountRepository.GetById(item?.InvestorBankAccId ?? 0);
                        var bankInfo = _bankRepository.GetById(investorBankFind.BankId);
                        investMentData.BankAccNo = investorBankFind?.BankAccount;
                        investMentData.OwnerBankName = investorBankFind?.OwnerAccount;
                        investMentData.CustomerBankName = bankInfo?.BankName;

                        //lấy thông tin ngân hàng của khách hàng cá nhân
                        investMentData.CustomerBankName = bankInfo?.BankName;
                        investMentData.BankAccNo = investorBankFind?.BankAccount;
                        investMentData.OwnerBankName = investorBankFind?.OwnerAccount;
                    }
                    catch
                    {
                    }

                    //var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(cifCodeFind.BusinessCustomerId ?? 0);
                    //var bankInfo = _bankRepository.GetById(businessCustomerBankFind?.BankId ?? 0);
                    /*
                                        investMentData.BankAccNo = businessCustomerBankFind?.BankAccNo;
                                        investMentData.OwnerBankName = businessCustomerBankFind?.BankAccName;*/

                    //check source nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    investMentData.TradingType = ExcelDataUtils.TradingTypeDisplay(item.Source);

                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    investMentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);

                    //check thông tin phong toả hay không phong toả theo status
                    investMentData.IsBlockage = item.Status == 6 ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;

                    investMentData.TranDate = item.TranDate;
                    investMentData.CifCode = item.CifCode;
                    investMentData.ContractCode = _investOrderEFRepository.GetContractCodeGen(item.OrderId) ?? item.ContractCode;
                    investMentData.SaleReferralCode = item.SaleReferralCode?.ToString();
                    investMentData.Area = item.Area;
                    investMentData.InvName = item.InvName;
                    investMentData.PolicyCode = item.PolicyCode;
                    investMentData.InvestDate = item.InvestDate;

                    try
                    {
                        investMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        investMentData.EndDate = null;
                    }

                    investMentData.Status = item.Status;
                    investMentData.Profit = item.Profit + "%";
                    investMentData.TotalValue = item.TotalValue;
                    investMentData.PaymentAmnount = item.PaymentAmnount;
                    investMentData.CurrentInvestment = item.CurrentInvestment;
                    investMentData.InvCode = item.InvCode;
                }

                var saleInfo = _saleRepository.GetSaleInfoByRefferalCode(item.TradingProviderId, item.SaleReferralCode);
                var saleInfoBusiness = _saleRepository.GetSaleInfoByRefferalCodeBusiness(item.TradingProviderId, item.SaleReferralCode);

                var departmentId = new int?();
                var listDepartment = new List<Department>();

                if (saleInfoBusiness != null && saleInfo == null)
                {
                    var saleBusinessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(saleInfoBusiness?.BusinessCustomerId ?? 0);
                    investMentData.ManagerIdNo = saleBusinessCustomerFind?.TaxCode;
                    investMentData.ManagerIdType = TaxCode.MA_SO_THUE;
                    departmentId = saleInfoBusiness?.DepartmentId;
                    investMentData.SaleName = saleInfoBusiness?.Name;
                    investMentData.DepartmentName = saleInfoBusiness?.DepartmentName;
                }
                else if (saleInfo != null && saleInfoBusiness == null)
                {
                    var saleIdentification = _managerInvestorRepository.GetDefaultIdentification(saleInfo.InvestorId ?? 0, false);

                    investMentData.ManagerIdType = saleIdentification?.IdType;
                    investMentData.ManagerIdNo = saleIdentification?.IdNo;
                    departmentId = saleInfo?.DepartmentId;
                    investMentData.SaleName = saleInfo?.Fullname;
                    investMentData.DepartmentName = saleInfo?.DepartmentName;
                }

                //Đệ quy lấy danh sách phòng ban node cha của phòng ban hiện tại đang xét
                DeQuyDepartmentParent(item.TradingProviderId, departmentId ?? 0, ref listDepartment);

                if (listDepartment.Count > 0 && listDepartment[0] != null)
                {
                    foreach (var department in listDepartment)
                    {
                        if (department?.DepartmentLevel == 4)
                        {
                            var saleManagerLv1 = _saleRepository.SaleGetById(department?.ManagerId ?? 0, item.TradingProviderId);
                            var saleIdentification = _managerInvestorRepository.GetDefaultIdentification(saleManagerLv1?.InvestorId ?? 0, false);

                            investMentData.ManagerIdType1 = saleIdentification?.IdType;
                            investMentData.ManagerIdNo1 = saleIdentification?.IdNo;
                            investMentData.ManagerNameLv1 = saleManagerLv1?.Fullname;
                            investMentData.ManagerCodeLv1 = saleManagerLv1?.ReferralCode;
                        }
                        else if (department?.DepartmentLevel == 3)
                        {
                            var saleManagerLv2 = _saleRepository.SaleGetById(department?.ManagerId ?? 0, item.TradingProviderId);
                            var saleIdentification = _managerInvestorRepository.GetDefaultIdentification(saleManagerLv2?.InvestorId ?? 0, false);

                            investMentData.ManagerIdType2 = saleIdentification?.IdType;
                            investMentData.ManagerIdNo2 = saleIdentification?.IdNo;
                            investMentData.ManagerNameLv2 = saleManagerLv2?.Fullname;
                            investMentData.ManagerCodeLv2 = saleManagerLv2?.ReferralCode;
                        }
                        else if (department.DepartmentLevel == 2)
                        {
                            var saleManagerLv3 = _saleRepository.SaleGetById(department?.ManagerId ?? 0, item.TradingProviderId);
                            var saleIdentification = _managerInvestorRepository.GetDefaultIdentification(saleManagerLv3?.InvestorId ?? 0, false);

                            investMentData.ManagerIdType3 = saleIdentification?.IdType;
                            investMentData.ManagerIdNo3 = saleIdentification?.IdNo;
                            investMentData.ManagerNameLv3 = saleManagerLv3?.Fullname;
                            investMentData.ManagerCodeLv3 = saleManagerLv3?.ReferralCode;
                        }
                        else if (department.DepartmentLevel == 1)
                        {
                            var saleManagerLv4 = _saleRepository.SaleGetById(department?.ManagerId ?? 0, item.TradingProviderId);
                            var saleIdentification = _managerInvestorRepository.GetDefaultIdentification(saleManagerLv4?.InvestorId ?? 0, false);

                            investMentData.ManagerIdType4 = saleIdentification?.IdType;
                            investMentData.ManagerIdNo4 = saleIdentification?.IdNo;
                            investMentData.ManagerNameLv4 = saleManagerLv4?.Fullname;
                            investMentData.ManagerCodeLv4 = saleManagerLv4?.ReferralCode;
                        }
                    }
                }

                investMentData.CreatedDate = item.CreatedDate;
                investMentData.CreatedBy = (item?.CreatedBy != null) ? item?.CreatedBy : investMentData.CustomerName;
                investMentData.ApproveDate = item.ApproveDate;
                investMentData.ApproveBy = item.ApproveBy;
                investMentData.PendingDate = item.PendingDate;
                investMentData.DeliveryDate = item.DeliveryDate;
                investMentData.ReceivedDate = item.ReceivedDate;
                investMentData.FinishedDate = item.FinishedDate;

                investmentList.Add(investMentData);
            }

            investmentList = investmentList.Where(i => i.TranDate?.Date >= (startDate?.Date ?? DateTime.MinValue) && i.TranDate?.Date <= (endDate?.Date ?? DateTime.MaxValue)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 5).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Địa chỉ liên hệ";
            worksheet.Cell(currentRow, 7).Value = "Mã KH";
            worksheet.Cell(currentRow, 8).Value = "Giới tính";
            worksheet.Cell(currentRow, 9).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 10).Value = "Tài khoản nhận tiền KH";
            worksheet.Cell(currentRow, 11).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 12).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 13).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 14).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 15).Value = "Số giấy tờ của người giới thiệu";
            worksheet.Cell(currentRow, 16).Value = "Loại giấy tờ của người giới thiệu";
            worksheet.Cell(currentRow, 17).Value = "Phòng ban";
            worksheet.Cell(currentRow, 18).Value = "Mã giới thiệu cấp quản lý cấp 1";
            worksheet.Cell(currentRow, 19).Value = "Tên giới thiệu cấp quản lý cấp 1";
            worksheet.Cell(currentRow, 20).Value = "Số giấy tờ của người giới thiệu cấp 1";
            worksheet.Cell(currentRow, 21).Value = "Loại giấy tờ của người giới thiệu cấp 1";
            worksheet.Cell(currentRow, 22).Value = "Mã giới thiệu cấp quản lý cấp 2";
            worksheet.Cell(currentRow, 23).Value = "Tên giới thiệu cấp quản lý cấp 2";
            worksheet.Cell(currentRow, 24).Value = "Số giấy tờ của người giới thiệu cấp 2";
            worksheet.Cell(currentRow, 25).Value = "Loại giấy tờ của người giới thiệu cấp 2";
            worksheet.Cell(currentRow, 26).Value = "Mã giới thiệu cấp quản lý cấp 3";
            worksheet.Cell(currentRow, 27).Value = "Tên giới thiệu cấp quản lý cấp 3";
            worksheet.Cell(currentRow, 28).Value = "Số giấy tờ của người giới thiệu cấp 3";
            worksheet.Cell(currentRow, 29).Value = "Loại giấy tờ của người giới thiệu cấp 3";
            worksheet.Cell(currentRow, 30).Value = "Kiểu giao dịch";
            worksheet.Cell(currentRow, 31).Value = "Mã dự án";
            worksheet.Cell(currentRow, 32).Value = "Tên dự án";
            worksheet.Cell(currentRow, 33).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 34).Value = "Phong toả";
            worksheet.Cell(currentRow, 35).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 36).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 37).Value = "Thời hạn";
            worksheet.Cell(currentRow, 38).Value = "Trạng thái hợp đồng";
            worksheet.Cell(currentRow, 39).Value = "Lợi nhuận";
            worksheet.Cell(currentRow, 40).Value = "Giá trị đầu tư theo HĐ";
            worksheet.Cell(currentRow, 41).Value = "Tổng số tiền KH chuyển";
            worksheet.Cell(currentRow, 42).Value = "Giá trị đầu tư hiện tại";
            worksheet.Cell(currentRow, 43).Value = "Người tạo hợp đồng";
            worksheet.Cell(currentRow, 44).Value = "Thời gian tạo";
            worksheet.Cell(currentRow, 45).Value = "Người duyệt hợp đồng";
            worksheet.Cell(currentRow, 46).Value = "Thời gian duyệt";
            worksheet.Cell(currentRow, 47).Value = "Thời gian gửi yêu cầu";
            worksheet.Cell(currentRow, 48).Value = "Thời gian giao";
            worksheet.Cell(currentRow, 49).Value = "Thời gian nhận";
            worksheet.Cell(currentRow, 50).Value = "Thời gian hoàn thành";
            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 40;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
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
            worksheet.Column("Z").Width = 40;
            worksheet.Column("AA").Width = 40;
            worksheet.Column("AB").Width = 40;
            worksheet.Column("AC").Width = 40;
            worksheet.Column("AD").Width = 40;
            worksheet.Column("AE").Width = 40;
            worksheet.Column("AF").Width = 40;
            worksheet.Column("AG").Width = 40;
            worksheet.Column("AH").Width = 40;
            worksheet.Column("AI").Width = 40;
            worksheet.Column("AJ").Width = 40;
            worksheet.Column("AK").Width = 40;
            worksheet.Column("AL").Width = 40;
            worksheet.Column("AM").Width = 40;
            worksheet.Column("AN").Width = 40;
            worksheet.Column("AO").Width = 40;
            worksheet.Column("AP").Width = 40;
            worksheet.Column("AQ").Width = 40;
            worksheet.Column("AR").Width = 40;
            worksheet.Column("AS").Width = 40;
            worksheet.Column("AT").Width = 40;

            foreach (var item in investmentList)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.TranDate;
                worksheet.Cell(currentRow, 3).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 5).Value = item?.IdType;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.PlaceOfResident;
                worksheet.Cell(currentRow, 7).Value = "'" + item?.CifCode;

                if (item.CustomerType == UserTypes.INVESTOR && item.Sex != null) //nếu là khách hàng cá nhân thì mới cho hiển thị giá trị của trường sex
                {
                    worksheet.Cell(currentRow, 8).Value = ExcelDataUtils.GenderDisplay(item?.Sex);
                }

                worksheet.Cell(currentRow, 9).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAccNo;
                worksheet.Cell(currentRow, 11).Value = item?.OwnerBankName;
                worksheet.Cell(currentRow, 12).Value = item?.CustomerBankName;
                worksheet.Cell(currentRow, 13).Value = "'" + item?.SaleReferralCode;
                worksheet.Cell(currentRow, 14).Value = item?.SaleName;
                worksheet.Cell(currentRow, 15).Value = "'" + item?.ManagerIdNo;
                worksheet.Cell(currentRow, 16).Value = item?.ManagerIdType;
                worksheet.Cell(currentRow, 17).Value = item?.DepartmentName;
                worksheet.Cell(currentRow, 18).Value = "'" + item?.ManagerCodeLv1;
                worksheet.Cell(currentRow, 19).Value = item?.ManagerNameLv1;
                worksheet.Cell(currentRow, 20).Value = "'" + item?.ManagerIdNo1;
                worksheet.Cell(currentRow, 21).Value = item?.ManagerIdType1;
                worksheet.Cell(currentRow, 22).Value = "'" + item?.ManagerCodeLv2;
                worksheet.Cell(currentRow, 23).Value = item?.ManagerNameLv2;
                worksheet.Cell(currentRow, 24).Value = "'" + item?.ManagerIdNo2;
                worksheet.Cell(currentRow, 25).Value = item?.ManagerIdType2;
                worksheet.Cell(currentRow, 26).Value = "'" + item?.ManagerCodeLv3;
                worksheet.Cell(currentRow, 27).Value = item?.ManagerNameLv3;
                worksheet.Cell(currentRow, 28).Value = "'" + item?.ManagerIdNo3;
                worksheet.Cell(currentRow, 29).Value = item?.ManagerIdType3;
                worksheet.Cell(currentRow, 30).Value = item?.TradingType;
                worksheet.Cell(currentRow, 31).Value = "'" + item?.InvCode;
                worksheet.Cell(currentRow, 32).Value = item?.InvName;
                worksheet.Cell(currentRow, 33).Value = "'" + item?.PolicyCode;
                worksheet.Cell(currentRow, 34).Value = item?.IsBlockage;
                worksheet.Cell(currentRow, 35).Value = item?.InvestDate?.Date;
                worksheet.Cell(currentRow, 36).Value = item?.EndDate?.Date;
                worksheet.Cell(currentRow, 37).Value = item?.PeriodTime;
                worksheet.Cell(currentRow, 38).Value = ExcelDataUtils.StatusOrder(item?.Status);
                worksheet.Cell(currentRow, 39).Value = item?.Profit;
                worksheet.Cell(currentRow, 40).Value = item?.CurrentInvestment - 0;
                worksheet.Cell(currentRow, 41).Value = item?.PaymentAmnount;
                worksheet.Cell(currentRow, 42).Value = item?.TotalValue;
                worksheet.Cell(currentRow, 43).Value = item?.CreatedBy;
                worksheet.Cell(currentRow, 44).Value = item?.CreatedDate;
                worksheet.Cell(currentRow, 45).Value = item?.ApproveBy;
                worksheet.Cell(currentRow, 46).Value = item?.ApproveDate;
                worksheet.Cell(currentRow, 47).Value = item?.PendingDate;
                worksheet.Cell(currentRow, 48).Value = item?.DeliveryDate;
                worksheet.Cell(currentRow, 49).Value = item?.ReceivedDate;
                worksheet.Cell(currentRow, 50).Value = item?.FinishedDate;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư bán hộ
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ExportResultDto SalesInvestmentReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var saleInvestmentList = new List<SalesInvestmentReportDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER) ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var listSaleInvestmentFind = _exportExcelReportRepository.FindListSaleInvestment(tradingProviderId, null, null, partnerId);

            foreach (var item in listSaleInvestmentFind)
            {
                var saleInvestMentData = new SalesInvestmentReportDto();
                var orderFind = _investOrderEFRepository.FindById(item.OrderId);
                var cifCodeFind = _cifCodeRepository.GetByCifCode(item.CifCode);
                var policyFind = _policyRepository.FindPolicyById(item.PolicyId);
                var policyDetailFind = _policyRepository.FindPolicyDetailById(item.PolicyDetailId);
                var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyFind.DistributionId && r.Deleted == YesNo.NO);
                var saleName = _saleEFRepository.FindSaleName(item.SaleReferralCodeSub)?.Name ?? "";
                var departmentName = _departmentRepository.FindById(item.DepartmentIdSub ?? item.DepartmentId, null)?.DepartmentName ?? "";

                if (cifCodeFind.InvestorId != null && cifCodeFind.BusinessCustomerId == null)
                {
                    var customerType = UserTypes.INVESTOR;
                    var investorIdentificationFind = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);

                    saleInvestMentData.CustomerName = investorIdentificationFind?.Fullname;
                    saleInvestMentData.Sex = investorIdentificationFind?.Sex;
                    saleInvestMentData.IdNo = investorIdentificationFind?.IdNo;
                    saleInvestMentData.IdType = investorIdentificationFind?.IdType;
                    saleInvestMentData.PlaceOfResident = investorIdentificationFind?.PlaceOfResidence;

                    var investorBankFind = _investorEFRepository.FindBankById(orderFind.InvestorBankAccId ?? 0);
                    saleInvestMentData.BankAccNo = investorBankFind?.BankAccount;
                    saleInvestMentData.OwnerBankName = investorBankFind?.OwnerAccount;
                    saleInvestMentData.CustomerBankName = investorBankFind?.CoreBankName;

                    //check souce nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    saleInvestMentData.TradingType = ExcelDataUtils.TradingTypeDisplay(item.Source);
                    saleInvestMentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);

                    // Check thông tin xem có đang phong toả hay không
                    saleInvestMentData.IsBlockage = item.Status == 6 ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;


                    saleInvestMentData.CustomerType = customerType;
                    saleInvestMentData.TranDate = item.TranDate;
                    saleInvestMentData.CifCode = item.CifCode;
                    saleInvestMentData.SaleReferralCodeSub = item.SaleReferralCodeSub;
                    saleInvestMentData.ContractCode = item.ContractCode;
                    saleInvestMentData.SaleReferralCode = item.SaleReferralCode;
                    saleInvestMentData.CalculateType = ExcelDataUtils.CaculateTypeDisplay((int)policyFind.CalculateType);

                    saleInvestMentData.SaleName = saleName;
                    saleInvestMentData.DepartmentName = departmentName;

                    saleInvestMentData.Area = item?.Area;
                    saleInvestMentData.InvCode = item?.InvCode;
                    saleInvestMentData.PolicyCode = item?.PolicyCode;
                    saleInvestMentData.InvestDate = item?.InvestDate;
                    saleInvestMentData.Status = item?.Status;
                    saleInvestMentData.Profit = item?.Profit + "%";
                    saleInvestMentData.InitTotalValue = item?.InitTotalValue;
                    saleInvestMentData.PaymentAmnount = item?.PaymentAmnount;

                    try
                    {
                        saleInvestMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        saleInvestMentData.EndDate = null;
                    }
                    saleInvestMentData.CurrentInvestment = item.Status != OrderStatus.TAT_TOAN ? item.CurrentInvestment : 0;

                }
                else if (cifCodeFind.InvestorId == null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    var businessCustomerBank = new BusinessCustomerBank();

                    saleInvestMentData.IdNo = businessCustomer?.TaxCode;
                    saleInvestMentData.IdType = TaxCode.MA_SO_THUE;

                    var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(cifCodeFind.BusinessCustomerId ?? 0);
                    var bankInfo = _bankRepository.GetById(businessCustomerBankFind?.BankId ?? 0);

                    saleInvestMentData.BankAccNo = businessCustomerBankFind?.BankAccNo;
                    saleInvestMentData.OwnerBankName = businessCustomerBankFind?.BankAccName;
                    saleInvestMentData.CustomerBankName = bankInfo?.BankName;

                    saleInvestMentData.SaleReferralCodeSub = item?.SaleReferralCodeSub;

                    //check source nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    saleInvestMentData.TradingType = ExcelDataUtils.TradingTypeDisplay(item.Source);

                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    saleInvestMentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);

                    //check thông tin phong toả hay không phong toả theo status
                    saleInvestMentData.IsBlockage = item.Status == OrderStatus.PHONG_TOA ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;

                    saleInvestMentData.CustomerName = businessCustomer?.Name;
                    saleInvestMentData.TranDate = item.TranDate;
                    saleInvestMentData.CifCode = item.CifCode;
                    saleInvestMentData.ContractCode = item.ContractCode;

                    saleInvestMentData.SaleName = saleName;
                    saleInvestMentData.DepartmentName = departmentName;

                    saleInvestMentData.Area = item.Area;
                    saleInvestMentData.InvCode = item.InvCode;
                    saleInvestMentData.PolicyCode = item.PolicyCode;
                    saleInvestMentData.InvestDate = item.InvestDate;

                    try
                    {
                        saleInvestMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        saleInvestMentData.EndDate = null;
                    }

                    saleInvestMentData.Status = item.Status;
                    saleInvestMentData.Profit = item.Profit + "%";
                    saleInvestMentData.InitTotalValue = item.InitTotalValue;
                    saleInvestMentData.PaymentAmnount = item.PaymentAmnount;
                    saleInvestMentData.CurrentInvestment = item.Status != OrderStatus.TAT_TOAN ? item.CurrentInvestment : 0;
                }

                saleInvestMentData.OrderSource = ExcelDataUtils.OrderSourceDisplay(
                                                    item.Source == SourceOrder.ONLINE ? SourceOrderFE.KHACH_HANG :
                                                    item.SaleOrderId != null ? SourceOrderFE.SALE :
                                                    SourceOrderFE.QUAN_TRI_VIEN);

                saleInvestmentList.Add(saleInvestMentData);
            }

            saleInvestmentList = saleInvestmentList.Where(s => s.InvestDate?.Date >= (startDate?.Date ?? DateTime.MinValue) && s.InvestDate?.Date <= (endDate?.Date ?? DateTime.MaxValue)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;
            var stt = 0;
            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Ngày giao dịch";
            worksheet.Cell(currentRow, 3).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 4).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 5).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 6).Value = "Địa chỉ thường trú";
            worksheet.Cell(currentRow, 7).Value = "Mã KH";
            worksheet.Cell(currentRow, 8).Value = "Giới tính";
            worksheet.Cell(currentRow, 9).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 10).Value = "Tài khoản nhận tiền KH";
            worksheet.Cell(currentRow, 11).Value = "Tên chủ tài khoản";
            worksheet.Cell(currentRow, 12).Value = "Ngân hàng";
            worksheet.Cell(currentRow, 13).Value = "Mã giới thiệu";
            worksheet.Cell(currentRow, 14).Value = "Tên người giới thiệu";
            worksheet.Cell(currentRow, 15).Value = "Phòng ban";
            worksheet.Cell(currentRow, 16).Value = "Kiểu giao dịch";
            worksheet.Cell(currentRow, 17).Value = "Nguồn đặt";
            worksheet.Cell(currentRow, 18).Value = "Dự án";
            worksheet.Cell(currentRow, 19).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 20).Value = "Phong toả";
            worksheet.Cell(currentRow, 21).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 22).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 23).Value = "Thời hạn";
            worksheet.Cell(currentRow, 24).Value = "Tình trạng";
            worksheet.Cell(currentRow, 25).Value = "Lợi nhuận";
            worksheet.Cell(currentRow, 26).Value = "Gross/Net";
            worksheet.Cell(currentRow, 27).Value = "Giá trị đầu tư theo HĐ";
            worksheet.Cell(currentRow, 28).Value = "Số tiền KH chuyển";
            worksheet.Cell(currentRow, 29).Value = "Giá trị đầu tư hiện tại";

            worksheet.Column("A").Width = 10;
            worksheet.Column("B").Width = 20;
            worksheet.Column("C").Width = 40;
            worksheet.Column("D").Width = 40;
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
            worksheet.Column("Z").Width = 40;
            worksheet.Column("AA").Width = 40;

            foreach (var item in saleInvestmentList)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.TranDate;
                worksheet.Cell(currentRow, 3).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 4).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 5).Value = item?.IdType;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.PlaceOfResident;
                worksheet.Cell(currentRow, 7).Value = "'" + item?.CifCode;

                if (item.CustomerType == UserTypes.INVESTOR && item.Sex != null) //nếu là khách hàng cá nhân thì mới cho hiển thị giá trị của trường sex
                {
                    worksheet.Cell(currentRow, 8).Value = ExcelDataUtils.GenderDisplay(item?.Sex);
                }

                worksheet.Cell(currentRow, 9).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.BankAccNo;
                worksheet.Cell(currentRow, 11).Value = item?.OwnerBankName;
                worksheet.Cell(currentRow, 12).Value = item?.CustomerBankName;
                worksheet.Cell(currentRow, 13).Value = "'" + item?.SaleReferralCodeSub;
                worksheet.Cell(currentRow, 14).Value = item?.SaleName;
                worksheet.Cell(currentRow, 15).Value = item?.DepartmentName;
                worksheet.Cell(currentRow, 16).Value = item?.TradingType;
                worksheet.Cell(currentRow, 17).Value = item?.OrderSource;
                worksheet.Cell(currentRow, 18).Value = item?.InvCode;
                worksheet.Cell(currentRow, 19).Value = "'" + item?.PolicyCode;
                worksheet.Cell(currentRow, 20).Value = item?.IsBlockage;
                worksheet.Cell(currentRow, 21).Value = item?.InvestDate;
                worksheet.Cell(currentRow, 22).Value = item?.EndDate?.Date;
                worksheet.Cell(currentRow, 23).Value = item?.PeriodTime;
                worksheet.Cell(currentRow, 24).Value = ExcelDataUtils.StatusOrder(item?.Status);
                worksheet.Cell(currentRow, 25).Value = item?.Profit;
                worksheet.Cell(currentRow, 26).Value = item?.CalculateType;
                worksheet.Cell(currentRow, 27).Value = item?.InitTotalValue;
                worksheet.Cell(currentRow, 28).Value = item?.PaymentAmnount;
                worksheet.Cell(currentRow, 29).Value = item?.CurrentInvestment;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo sao kê tài khoản nhà đầu tư
        /// </summary>
        /// <returns></returns>
        public ExportResultDto InvestmentAccountStatisticalReport(DateTime? startDate, DateTime? endDate)
        {
            var result = new ExportResultDto();
            var investmentList = new List<InvestmentAccountDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            int? tradingProviderId = (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER) ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;
            int? partnerId = (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER) ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;
            var investmentFind = _exportExcelReportRepository.GetInvestmentAccountStatisticals(partnerId, tradingProviderId, null, null);

            foreach (var item in investmentFind)
            {
                var projectFind = _projectRepository.FindById(item.ProjectId ?? 0, item.TradingProviderId);
                var investMentData = new InvestmentAccountDto();
                var orderFind = _investOrderEFRepository.FindById(item.OrderId);
                var cifCodeFind = _cifCodeRepository.GetByCifCode(item.CifCode);
                var policyFind = _policyRepository.FindPolicyById(item.PolicyId);
                var policyDetailFind = _policyRepository.FindPolicyDetailById(item.PolicyDetailId);
                var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyFind.DistributionId && r.Deleted == YesNo.NO);
                if (cifCodeFind.InvestorId != null && cifCodeFind.BusinessCustomerId == null)
                {
                    var saleInfo = _saleRepository.GetSaleInfoByRefferalCode(item.TradingProviderId, item.SaleReferralCode);
                    var saleInfoBusiness = _saleRepository.GetSaleInfoByRefferalCodeBusiness(item.TradingProviderId, item.SaleReferralCode);
                    var customerType = UserTypes.INVESTOR;
                    var investorIdentificationFind = _managerInvestorRepository.GetDefaultIdentification(cifCodeFind.InvestorId ?? 0, false);

                    investMentData.CustomerName = investorIdentificationFind?.Fullname;
                    investMentData.Sex = investorIdentificationFind?.Sex;
                    investMentData.IdNo = investorIdentificationFind?.IdNo;
                    investMentData.IdType = investorIdentificationFind?.IdType;
                    investMentData.PlaceOfResident = investorIdentificationFind?.PlaceOfResidence;

                    try
                    {
                        var investorBankFind = _investorBankAccountRepository.GetByInvestorId(cifCodeFind.InvestorId ?? 0);
                        var bankInfo = _bankRepository.GetById(investorBankFind?.BankId ?? 0);

                        investMentData.BankAccNo = investorBankFind?.BankAccount;
                        investMentData.OwnerBankName = investorBankFind?.OwnerAccount;
                        investMentData.CustomerBankName = bankInfo?.BankName;
                    }
                    catch
                    {
                    }

                    //check souce nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline 
                    investMentData.TradingType = ExcelDataUtils.TradingTypeDisplay(item.Source);

                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    investMentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);

                    // Check thông tin xem có đang phong toả hay không
                    investMentData.IsBlockage = item.Status != 6 ? IsBlockage.NOT_BLOCKAGE : IsBlockage.BLOCKAGE;

                    investMentData.CustomerType = customerType;
                    investMentData.TranDate = item.TranDate;
                    investMentData.CifCode = item.CifCode;
                    investMentData.ContractCode = item.ContractCode;
                    investMentData.SaleReferralCode = item.SaleReferralCode;

                    if (saleInfo != null && saleInfoBusiness == null)
                    {
                        investMentData.SaleName = saleInfo.Fullname;
                        investMentData.DepartmentName = saleInfo.DepartmentName;
                    }
                    else if (saleInfoBusiness != null && saleInfo == null)
                    {
                        investMentData.SaleName = saleInfoBusiness.Name;
                        investMentData.DepartmentName = saleInfoBusiness.DepartmentName;
                    }

                    investMentData.Area = item.Area;
                    investMentData.InvName = item.InvName;
                    investMentData.InvCode = item.InvCode;
                    investMentData.PolicyCode = item.PolicyCode;
                    investMentData.InvestDate = item.InvestDate;
                    investMentData.InitTotalValue = item.InitTotalValue;
                    investMentData.Status = ExcelDataUtils.StatusOrder(item.Status ?? 0);
                    investMentData.Profit = item.Profit + "%";
                    investMentData.TotalValue = item.TotalValue;
                    investMentData.PaymentAmnount = item.PaymentAmnount;

                    try
                    {
                        investMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        investMentData.EndDate = null;
                    }
                    investMentData.CurrentInvestment = item.CurrentInvestment;
                }
                else if (cifCodeFind.InvestorId == null && cifCodeFind.BusinessCustomerId != null)
                {

                    var saleInfo = _saleRepository.GetSaleInfoByRefferalCode(item.TradingProviderId, item.SaleReferralCode);
                    var saleInfoBusiness = _saleRepository.GetSaleInfoByRefferalCodeBusiness(item.TradingProviderId, item.SaleReferralCode);
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(cifCodeFind.BusinessCustomerId ?? 0);
                    var bankInfo = _bankRepository.GetById(businessCustomer?.BankId ?? 0);

                    investMentData.BankAccNo = businessCustomerBankFind?.BankAccNo;
                    investMentData.OwnerBankName = businessCustomerBankFind?.BankAccName;

                    //check source nếu bằng 1 thì thông tin kiểu giao dịch là online, là 2 thì là offline
                    investMentData.TradingType = ExcelDataUtils.TradingTypeDisplay(item.Source);

                    //check thông tin của kiểu kỳ hạn là gì rồi hiển thị tiếng việt của trường đó ra
                    investMentData.PeriodTime = item.PeriodQuantity + ExcelDataUtils.PeriodTypeDisplay(item.PeriodType);


                    //check thông tin phong toả hay không phong toả theo status
                    investMentData.IsBlockage = item.Status != 6 ? IsBlockage.BLOCKAGE : IsBlockage.NOT_BLOCKAGE;

                    investMentData.CustomerName = businessCustomer?.Name;
                    investMentData.TranDate = item.TranDate;
                    investMentData.CifCode = item.CifCode;
                    investMentData.InvCode = item.InvCode;
                    investMentData.ContractCode = item.ContractCode;
                    investMentData.CustomerBankName = bankInfo?.BankName;
                    investMentData.SaleReferralCode = item.SaleReferralCode;

                    if (saleInfoBusiness != null && saleInfo == null)
                    {
                        investMentData.SaleName = saleInfoBusiness?.Name;
                        investMentData.DepartmentName = saleInfoBusiness?.DepartmentName;
                    }
                    else if (saleInfo != null && saleInfoBusiness == null)
                    {
                        investMentData.SaleName = saleInfo?.Fullname;
                        investMentData.DepartmentName = saleInfo?.DepartmentName;
                    }

                    investMentData.Area = item.Area;
                    investMentData.InvName = item.InvName;
                    investMentData.InvCode = item.InvCode;
                    investMentData.PolicyCode = item.PolicyCode;
                    investMentData.InvestDate = item.InvestDate;
                    investMentData.InitTotalValue = item.InitTotalValue;

                    try
                    {
                        investMentData.EndDate = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetailFind, item.InvestDate ?? new DateTime(), distribution?.CloseCellDate);
                    }
                    catch
                    {
                        investMentData.EndDate = null;
                    }

                    investMentData.Status = ExcelDataUtils.StatusOrder(item.Status ?? 0);
                    investMentData.Profit = item.Profit + "%";
                    investMentData.TotalValue = item.TotalValue;
                    investMentData.PaymentAmnount = item.PaymentAmnount;
                    investMentData.CurrentInvestment = item.CurrentInvestment;
                }

                var profitCaculateFind = new ProfitAndInterestDto();
                try
                {
                    profitCaculateFind = _investSharedServices.CalculateListInterest(projectFind, policyFind, policyDetailFind,
                                                                    item.InvestDate ?? new DateTime(), item.TotalValue, true, distribution.CloseCellDate, item.OrderId);
                }
                catch
                {
                }

                investMentData.InterestDue = profitCaculateFind?.ProfitInfo.Sum(p => p.Profit) ?? 0;
                investMentData.IncomeTax = profitCaculateFind?.ProfitInfo.Sum(p => p.Tax) ?? 0;
                investMentData.TongGiaTri = investMentData.InterestDue + item.InitTotalValue;
                investMentData.ActualReceivedMoney = investMentData.TongGiaTri - investMentData.IncomeTax;

                investmentList.Add(investMentData);
            }

            investmentList = investmentList.Where(i => i.TranDate?.Date >= (startDate?.Date ?? DateTime.MinValue) && i.TranDate?.Date <= (endDate?.Date ?? DateTime.MaxValue)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Tên khách hàng";
            worksheet.Cell(currentRow, 3).Value = "Số giấy tờ";
            worksheet.Cell(currentRow, 4).Value = "Loại giấy tờ";
            worksheet.Cell(currentRow, 5).Value = "Địa chỉ thường trú";
            worksheet.Cell(currentRow, 6).Value = "Mã KH";
            worksheet.Cell(currentRow, 7).Value = "Giới tính";
            worksheet.Cell(currentRow, 8).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 9).Value = "Mã dự án";
            worksheet.Cell(currentRow, 10).Value = "Mã chính sách";
            worksheet.Cell(currentRow, 11).Value = "Ngày đầu tư";
            worksheet.Cell(currentRow, 12).Value = "Ngày kết thúc";
            worksheet.Cell(currentRow, 13).Value = "Thời hạn";
            worksheet.Cell(currentRow, 14).Value = "Lợi tức";
            worksheet.Cell(currentRow, 15).Value = "Giá trị đầu tư theo HĐ";
            worksheet.Cell(currentRow, 16).Value = "Tổng giá trị";
            worksheet.Cell(currentRow, 17).Value = "Gốc đến hạn trả";
            worksheet.Cell(currentRow, 18).Value = "Lãi đến hạn trả";
            worksheet.Cell(currentRow, 19).Value = "Thuế thu nhập cá nhân";
            worksheet.Cell(currentRow, 20).Value = "Số tiền thực nhận";
            worksheet.Cell(currentRow, 21).Value = "Tên tài khoản nhận";
            worksheet.Cell(currentRow, 22).Value = "Số tài khoản nhận";
            worksheet.Cell(currentRow, 23).Value = "Ngân hàng nhận";
            worksheet.Cell(currentRow, 24).Value = "Số tiền KH chuyển";
            worksheet.Cell(currentRow, 25).Value = "Trạng thái hợp đồng";

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
            worksheet.Column("O").Width = 30;
            worksheet.Column("P").Width = 40;
            worksheet.Column("Q").Width = 20;
            worksheet.Column("R").Width = 40;
            worksheet.Column("S").Width = 40;
            worksheet.Column("T").Width = 40;
            worksheet.Column("U").Width = 40;
            worksheet.Column("V").Width = 40;
            worksheet.Column("W").Width = 20;
            worksheet.Column("X").Width = 20;
            worksheet.Column("Y").Width = 40;
            worksheet.Column("Z").Width = 40;
            worksheet.Column("AA").Width = 40;
            worksheet.Column("AB").Width = 40;
            worksheet.Column("AC").Width = 40;

            foreach (var item in investmentList)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = item?.CustomerName;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.IdNo;
                worksheet.Cell(currentRow, 4).Value = item?.IdType;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.PlaceOfResident;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.CifCode;

                if (item.CustomerType == UserTypes.INVESTOR && item.Sex != null) //nếu là khách hàng cá nhân thì mới cho hiển thị giá trị của trường sex
                {
                    worksheet.Cell(currentRow, 7).Value = ExcelDataUtils.GenderDisplay(item?.Sex);
                }

                worksheet.Cell(currentRow, 8).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 9).Value = "'" + item?.InvCode;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.PolicyCode;
                worksheet.Cell(currentRow, 11).Value = item?.InvestDate;
                worksheet.Cell(currentRow, 12).Value = item?.EndDate;
                worksheet.Cell(currentRow, 13).Value = item?.PeriodTime;
                worksheet.Cell(currentRow, 14).Value = item?.Profit;
                worksheet.Cell(currentRow, 15).Value = item?.InitTotalValue;
                worksheet.Cell(currentRow, 16).Value = item?.TongGiaTri; //Gốc đến hạn trả
                worksheet.Cell(currentRow, 17).Value = item?.TotalValue;
                worksheet.Cell(currentRow, 18).Value = item?.InterestDue;
                worksheet.Cell(currentRow, 19).Value = item?.IncomeTax;
                worksheet.Cell(currentRow, 20).Value = item?.ActualReceivedMoney;
                worksheet.Cell(currentRow, 21).Value = item?.OwnerBankName;
                worksheet.Cell(currentRow, 22).Value = item?.BankAccNo;
                worksheet.Cell(currentRow, 23).Value = item?.CustomerBankName;
                worksheet.Cell(currentRow, 24).Value = item?.PaymentAmnount;
                worksheet.Cell(currentRow, 25).Value = item.Status;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        /// <summary>
        /// Báo cáo danh sách các các lệnh đã trả tiền
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ExportResultDto ListInterestPaymentPaid(InterestPaymentFilterDto input)
        {
            input.PageNumber = 0;
            input.PageSize = int.MaxValue;
            var interestPaymentList = _interestPaymentService.FindAll(input);
            var listInterestPaymentPaid = new List<ExcelInterestPaymentDto>();
            var result = new ExportResultDto();
            foreach (var item in interestPaymentList.Items)
            {
                var interestPaymentData = new ExcelInterestPaymentDto();
                var orderFind = _orderRepository.FindById(item.OrderId);

                interestPaymentData.ContractCode = item?.ContractCode;
                interestPaymentData.TotalValue = item?.TotalValue ?? 0;
                interestPaymentData.CifCode = item?.CifCode;
                interestPaymentData.PolicyDetailName = item.PolicyDetailName;

                var cifCodeFind = _cifCodeRepository.GetByCifCode(item?.CifCode);
                if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    var investerIden = _investorIdentificationRepository.FindById(orderFind?.InvestorIdenId ?? 0);
                    interestPaymentData.Name = investerIden?.Fullname;
                    interestPaymentData.IdNo = investerIden?.IdNo;
                    try
                    {
                        var investorBankFind = _investorBankAccountRepository.GetById(orderFind?.InvestorBankAccId ?? 0);
                        var bankInfo = _bankRepository.GetById(investorBankFind?.BankId ?? 0);

                        //lấy thông tin ngân hàng của khách hàng cá nhân
                        interestPaymentData.CustomerBank = bankInfo?.BankName;
                        interestPaymentData.CustomerBankAccount = investorBankFind?.BankAccount;
                        interestPaymentData.OwnerCustomerBankAccount = investorBankFind?.OwnerAccount;
                    }
                    catch
                    {
                    }
                }
                else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind?.BusinessCustomerId ?? 0);
                    interestPaymentData.Name = businessCustomer?.Name;
                    var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(cifCodeFind?.BusinessCustomerId ?? 0);
                    var bankInfo = _bankRepository.GetById(businessCustomerBankFind?.BankId ?? 0);

                    //lấy thông tin ngân hàng của khách hàng doanh nghiệp
                    interestPaymentData.CustomerBank = bankInfo?.BankName;
                    interestPaymentData.CustomerBankAccount = businessCustomerBankFind?.BankAccNo;
                    interestPaymentData.OwnerCustomerBankAccount = businessCustomerBankFind?.BankAccName;
                }

                var project = _projectRepository.FindByOrderId((int)item.OrderId, null);
                interestPaymentData.ActuallyProfit = item.Profit;
                interestPaymentData.Tax = item.Tax;
                interestPaymentData.PayDate = item.PayDate.Date;
                interestPaymentData.InvName = project?.InvName;
                interestPaymentData.ApproveBy = orderFind.ApproveBy;
                interestPaymentData.ApproveDate = orderFind.ApproveDate?.Date;
                interestPaymentData.IsLastPeriod = item.IsLastPeriod;
                interestPaymentData.ReceivePaymentLastPeriod = ((input.Status < PaymentType.DA_CHI_TRA && orderFind?.SettlementMethod == SettlementTypes.TAI_TUC_GOC) ? item.AmountMoney
                                                            : (input.Status < PaymentType.DA_CHI_TRA && orderFind?.SettlementMethod == SettlementTypes.TAI_TUC_GOC_VA_LOI_NHUAN) ? 0
                                                            : (input.Status < PaymentType.DA_CHI_TRA) ? item.TotalValueInvestment + item.AmountMoney
                                                            : item.AmountMoney) ?? 0;
                interestPaymentData.SettlementType = ExcelDataUtils.SettlementTypeDisplay(item.RenewalsRequest?.SettlementMethod);
                listInterestPaymentPaid.Add(interestPaymentData);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 3).Value = "Mã sản phẩm";
            worksheet.Cell(currentRow, 4).Value = "Khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Kỳ hạn";
            worksheet.Cell(currentRow, 6).Value = "Tiền đầu tư";
            worksheet.Cell(currentRow, 7).Value = "Thuế";
            worksheet.Cell(currentRow, 8).Value = "Lợi tức";
            worksheet.Cell(currentRow, 9).Value = "Ngày chi trả";
            worksheet.Cell(currentRow, 10).Value = "Loại tất toán";
            worksheet.Cell(currentRow, 11).Value = "Nhận cuối kì";
            worksheet.Cell(currentRow, 12).Value = "Tài khoản thụ hưởng";
            worksheet.Cell(currentRow, 13).Value = "Ngân hàng thụ hưởng";
            worksheet.Cell(currentRow, 14).Value = "Ngày duyệt";
            worksheet.Cell(currentRow, 15).Value = "Người duyệt";

            worksheet.Column("A").Width = 40;
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

            foreach (var item in listInterestPaymentPaid)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.InvName;
                worksheet.Cell(currentRow, 4).Value = item?.Name;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.PolicyDetailName;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.TotalValue;
                worksheet.Cell(currentRow, 7).Value = item.Tax;
                worksheet.Cell(currentRow, 8).Value = item?.ActuallyProfit;
                worksheet.Cell(currentRow, 9).Value = item?.PayDate;
                worksheet.Cell(currentRow, 10).Value = item?.SettlementType;
                worksheet.Cell(currentRow, 11).Value = item?.ReceivePaymentLastPeriod;
                worksheet.Cell(currentRow, 12).Value = "'" + item?.CustomerBankAccount;
                worksheet.Cell(currentRow, 13).Value = item?.CustomerBank;
                worksheet.Cell(currentRow, 14).Value = item?.ApproveDate;
                worksheet.Cell(currentRow, 15).Value = item?.ApproveBy;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;
        }

        public async Task<ExportResultDto> ListInterestPaymentDue(InterestPaymentFilterDto input, bool isLastPeriod)
        {
            var lapDanhSachChiTra = new List<ExcelInterestPaymentDto>();
            var result = new ExportResultDto();
            input.PageSize = int.MaxValue;
            input.PageNumber = 0;
            var danhSachNgayChiTra = await _orderServices.LapDanhSachChiTra(input, isLastPeriod);

            foreach (var item in danhSachNgayChiTra.Items)
            {
                var orderFind = _orderRepository.FindById(item.OrderId);
                var interestPaymentData = new ExcelInterestPaymentDto()
                {
                    ContractCode = item.ContractCode,
                    InvName = item.InvName,
                    Name = item.Name,
                    PolicyDetailName = item.PolicyDetailName,
                    TotalValue = item.TotalValue,
                    Tax = item.Tax,
                    AmountMoney = item.AmountMoney,
                    PayDate = item.PayDate,
                    SettlementType = ExcelDataUtils.SettlementTypeDisplay(item.RenewalsRequest.SettlementMethod),
                    ReceivePaymentLastPeriod = ((input.Status < PaymentType.DA_CHI_TRA && item.RenewalsRequest?.SettlementMethod == SettlementTypes.TAI_TUC_GOC) ? item.AmountMoney
                                                : (input.Status < PaymentType.DA_CHI_TRA && item.RenewalsRequest?.SettlementMethod == SettlementTypes.TAI_TUC_GOC_VA_LOI_NHUAN) ? 0
                                                : (input.Status < PaymentType.DA_CHI_TRA) ? item.TotalValueInvestment + item.AmountMoney
                                                : item.AmountMoney) ?? 0
                };

                try
                {
                    var cifCodeFind = _cifCodeRepository.GetByCifCode(item.CifCode);
                    if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                    {
                        var investorBankFind = _investorBankAccountRepository.GetById(orderFind?.InvestorBankAccId ?? 0);
                        var bankInfo = _bankRepository.GetById(investorBankFind?.BankId ?? 0);

                        //lấy thông tin ngân hàng của khách hàng cá nhân
                        interestPaymentData.CustomerBank = bankInfo?.BankName;
                        interestPaymentData.CustomerBankAccount = investorBankFind?.BankAccount;
                    }
                    else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                    {
                        var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                        var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankByBusiCusId(cifCodeFind.BusinessCustomerId ?? 0);
                        var bankInfo = _bankRepository.GetById(businessCustomerBankFind.BankId);

                        //lấy thông tin ngân hàng của khách hàng doanh nghiệp
                        interestPaymentData.CustomerBank = bankInfo?.BankName;
                        interestPaymentData.CustomerBankAccount = businessCustomerBankFind?.BankAccNo;
                    }
                }
                catch
                {

                }

                lapDanhSachChiTra.Add(interestPaymentData);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("INVEST");
            var currentRow = 1;
            var stt = 0;

            worksheet.Cell(currentRow, 1).Value = "STT";
            worksheet.Cell(currentRow, 2).Value = "Mã hợp đồng";
            worksheet.Cell(currentRow, 3).Value = "Mã sản phẩm";
            worksheet.Cell(currentRow, 4).Value = "Khách hàng";
            worksheet.Cell(currentRow, 5).Value = "Kỳ hạn";
            worksheet.Cell(currentRow, 6).Value = "Tiền đầu tư";
            worksheet.Cell(currentRow, 7).Value = "Thuế";
            worksheet.Cell(currentRow, 8).Value = "Lợi nhuận";
            worksheet.Cell(currentRow, 9).Value = "Ngày chi trả";
            worksheet.Cell(currentRow, 10).Value = "Loại tất toán";
            worksheet.Cell(currentRow, 11).Value = "Nhận cuối kì";
            worksheet.Cell(currentRow, 12).Value = "Tài khoản thụ hưởng";
            worksheet.Cell(currentRow, 13).Value = "Ngân hàng thụ hưởng";

            worksheet.Column("A").Width = 40;
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

            foreach (var item in lapDanhSachChiTra)
            {
                currentRow++;
                stt = ++stt;
                worksheet.Cell(currentRow, 1).Value = stt;
                worksheet.Cell(currentRow, 2).Value = "'" + item?.ContractCode;
                worksheet.Cell(currentRow, 3).Value = "'" + item?.InvName;
                worksheet.Cell(currentRow, 4).Value = item?.Name;
                worksheet.Cell(currentRow, 5).Value = "'" + item?.PolicyDetailName;
                worksheet.Cell(currentRow, 6).Value = "'" + item?.TotalValue;
                worksheet.Cell(currentRow, 7).Value = item.Tax;
                worksheet.Cell(currentRow, 8).Value = item?.AmountMoney;
                worksheet.Cell(currentRow, 9).Value = item?.PayDate;
                worksheet.Cell(currentRow, 10).Value = "'" + item?.SettlementType;
                worksheet.Cell(currentRow, 11).Value = "'" + item?.ReceivePaymentLastPeriod;
                worksheet.Cell(currentRow, 12).Value = "'" + item?.CustomerBankAccount;
                worksheet.Cell(currentRow, 13).Value = item?.CustomerBank;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            result.fileData = stream.ToArray();
            return result;

        }
    }
}
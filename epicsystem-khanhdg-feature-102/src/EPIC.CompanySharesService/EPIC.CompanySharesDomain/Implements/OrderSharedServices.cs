using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.Order;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.Dto.Department;
//using EPIC.Entities.Dto.BondOrder;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using AppPaymentInfoDto = EPIC.CompanySharesEntities.Dto.Order.AppPaymentInfoDto;

namespace EPIC.CompanySharesDomain.Implements
{
    public class OrderSharedServices : IOrderSharedServices
    {

        private readonly ILogger<OrderSharedServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly OrderRepository _orderRepository;
        private readonly CpsOrderPaymentRepository _orderPaymentRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        //private readonly BondInfoRepository _productBondInfoRepository;
        //private readonly BondPrimaryRepository _productBondPrimaryRepository;
        //private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        //private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        private readonly CompanySharesRepositories.CalendarRepository _calendarRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BankRepository _bankRepository;
        //private readonly BondContractTemplateRepository _contractTemplateRepository;
        //private readonly BondSecondaryContractRepository _bondSecondaryContractRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        //private readonly BondDistributionContractRepository _distributionContractRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly SaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ICpsSharedServices _cpsSharedServices;

        public OrderSharedServices(
            ILogger<OrderSharedServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            ICpsSharedServices bondSharedService)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _orderRepository = new OrderRepository(_connectionString, _logger);
            _orderPaymentRepository = new CpsOrderPaymentRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            //_productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            //_productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            //_productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            //_productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            _calendarRepository = new CompanySharesRepositories.CalendarRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            //_contractTemplateRepository = new BondContractTemplateRepository(_connectionString, _logger);
            //_bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _mapper = mapper;
            _cpsSharedServices = bondSharedService;
        }

        /// <summary>
        /// lấy thông tin sổ lệnh của nhà đầu tư theo mã chứng khoán
        /// </summary>
        /// <param name="securityCompany"></param>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        public List<SCOrderDto> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount)
        {
            List<SCOrderDto> result = new();
            var listOrder = _orderRepository.GetListInvestOrderByInvestor(securityCompany, stockTradingAccount);
            foreach (var orderItem in listOrder)
            {
                var resultItem = new SCOrderDto();

                resultItem = _mapper.Map<SCOrderDto>(orderItem);
                #region chờ repo cps 
                //Lấy thông tin dự án
                //var bondInfoFind = _productBondInfoRepository.FindById(orderItem.BondId ?? 0);
                //if (bondInfoFind != null)
                //{
                //    resultItem.BondInfo = _mapper.Map<SCBondInfoDto>(bondInfoFind);
                //}

                //Lấy thông tin phân phối đầu tư
                //var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryById(orderItem.Id ?? 0, orderItem.TradingProviderId);
                //if (bondSecondaryFind != null)
                //{
                //    resultItem.BondSecondary = _mapper.Map<SCBondSecondaryDto>(bondSecondaryFind);
                //}

                //Lấy thông tin chính sách
                //var policyFind = _productBondSecondaryRepository.FindPolicyById(orderItem.Id ?? 0);
                //if (policyFind != null)
                //{
                //    resultItem.BondPolicy = _mapper.Map<SCBondPolicyDto>(policyFind);
                //}

                //Lấy thông tin kỳ hạn
                //var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(orderItem.PolicyDetailId ?? 0);
                //if (policyDetailFind != null)
                //{
                //    resultItem.BondPolicyDetail = _mapper.Map<SCBondPolicyDetailDto>(policyDetailFind);
                //}
                #endregion
                //Lấy thông tin phòng ban
                var departmentFind = _departmentRepository.FindById(orderItem.DepartmentId ?? 0, orderItem.TradingProviderId);
                if (departmentFind != null)
                {
                    resultItem.Department = _mapper.Map<SCDepartmentDto>(departmentFind);
                }
                result.Add(resultItem);
            }
            return result;
        }

        public AppOrderInvestorDetailDto AppSaleViewOrder(int orderId)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);

            var findOrder = _orderRepository.AppSaleViewOrder(saleId, orderId);
            if (findOrder == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin hợp đồng với Saler"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }

            if (findOrder.InvestorId == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin nhà đầu tư của hợp đồng"), new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
            }

            return ViewOrderDetail(orderId, findOrder.InvestorId ?? 0);
        }

        public AppOrderInvestorDetailDto ViewOrderDetail(int orderId, int investorId)
        {
            #region Lấy data từ các Repo
            var orderFind = _orderRepository.AppGetOrderDetail(investorId, orderId);
            if (orderFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin lệnh trên app với orderId = {orderId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }

            //var bondInfo = _productBondInfoRepository.FindById(orderFind.BondId ?? 0);
            //if (bondInfo == null)
            //{
            //    _logger.LogError($"Không tìm thấy thông tin lô trên app với orderId = {orderId}");
            //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            //}

            //var policyFind = _productBondSecondaryRepository.FindPolicyById(orderFind.Id ?? 0, orderFind.TradingProviderId ?? 0);
            //if (policyFind == null)
            //{
            //    _logger.LogError($"Không tìm thấy thông tin chính sách trên app với orderId = {orderId}");
            //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            //}

            //var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(policyFind.Id, policyFind.TradingProviderId);
            //if (bondSecondary == null)
            //{
            //    _logger.LogError($"Không tìm thấy thông tin bán theo kỳ hạn trên app với orderId = {orderId}");
            //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            //}

            //var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(orderFind.PolicyDetailId ?? 0, orderFind.TradingProviderId ?? 0);
            //if (policyDetailFind == null)
            //{
            //    _logger.LogError($"Không tìm thấy thông tin kỳ hạn trên app với orderId = {orderId}");
            //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            //}

            var identificationFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
            if (identificationFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ nhà đầu tư trên app với orderId = {orderId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin giấy tờ nhà đầu tư"), new FaultCode(((int)ErrorCode.InvestorIdentificationNotFound).ToString()), "");
            }

            var investorBank = _managerInvestorRepository.GetBankById(orderFind.InvestorBankAccId ?? 0);
            if (investorBank == null)
            {
                _logger.LogError($"Không tìm thấy thông tin thụ hưởng trên app với orderId = {orderId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin thụ hưởng"), new FaultCode(((int)ErrorCode.InvestorBankAccNotFound).ToString()), "");
            }
            var investorContractAddress = _investorRepository.GetContactAddress(investorId, orderFind.ContractAddressId ?? 0);
            if (investorContractAddress == null)
            {
                _logger.LogError($"Không tìm thấy thông tin địa chỉ giao dịch trong chi tiết order trên app với orderId = {orderId}, investorId = {investorId}, contractAddressId = {orderFind.ContractAddressId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin địa chỉ giao dịch"), new FaultCode(((int)ErrorCode.InvestorContractAddessNotFound).ToString()), "");
            }

            var paymentInfo = _businessCustomerRepository.FindBusinessCusBankById(orderFind.BusinessCustomerBankAccId ?? 0);
            if (paymentInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin tài khoản thụ hưởng dlsc trong chi tiết order trên app với orderId = {orderId}, investorId = {investorId}, BusinessCustomerBankAccId = {orderFind.BusinessCustomerBankAccId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin ngân hàng thụ hưởng của đại lý sơ cấp"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            #endregion

            var result = _mapper.Map<CompanySharesEntities.Dto.Order.AppOrderInvestorDetailDto>(orderFind);

            if (identificationFind != null)
            {
                result.FullName = identificationFind.Fullname;
            }

            if (orderFind.SaleReferralCodeSub != null)
            {
                result.SaleReferralCode = orderFind.SaleReferralCodeSub;
            }

            var salerFind = _saleRepository.SaleGetInfoByReferralCode(result.SaleReferralCode);
            if (salerFind != null)
            {
                result.SalerName = salerFind.Fullname;
            }
            result.BondSecondaryId = orderFind.BondSecondaryId;
            //result.BondCode = bondInfo.BondCode;
            //result.PolicyName = policyFind.Name;
            //result.PeriodQuantity = policyDetailFind.PeriodQuantity;
            //result.PeriodType = policyDetailFind.PeriodType;
            //result.Profit = policyDetailFind.Profit;
            //result.InterestPeriodType = policyDetailFind.InterestPeriodType;
            //result.InterestType = policyDetailFind.InterestType;
            //string interestPeriodType = null;
            //if (policyDetailFind.InterestPeriodType == PeriodType.NGAY)
            //{
            //    interestPeriodType = "Ngày";
            //}
            //else if (policyDetailFind.InterestPeriodType == PeriodType.THANG)
            //{
            //    interestPeriodType = "Tháng";
            //}
            //else if (policyDetailFind.InterestPeriodType == PeriodType.NAM)
            //{
            //    interestPeriodType = "Năm";
            //}
            //result.InterestPeriod = policyDetailFind.InterestPeriodQuantity + " " + interestPeriodType;
            DateTime ngayDauTu = DateTime.Now.Date;
            if (orderFind.BuyDate != null && orderFind.PaymentFullDate == null && orderFind.InvestDate == null)
            {
                ngayDauTu = orderFind.BuyDate.Value;
            }

            else if (orderFind.InvestDate != null)
            {
                ngayDauTu = orderFind.InvestDate.Value.Date;
            }
            result.InvestDate = ngayDauTu;
            //result.DueDate = _bondSharedService.CalculateDueDate(policyDetailFind, ngayDauTu);

            //var profit = _bondSharedService.CalculateListInterest(bondInfo, policyFind, policyDetailFind, ngayDauTu, result.TotalValue, true);
            //if (profit == null)
            //{
            //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin trái tức"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            //}
            //result.ProfitNow = _bondSharedService.ProfitNow(ngayDauTu, result.DueDate.Value, policyDetailFind.Profit ?? 0, orderFind.TotalValue ?? 0);

            result.ContractAddress = investorContractAddress?.ContactAddress;
            result.IdNo = identificationFind?.IdNo;
            result.IdType = identificationFind?.IdType;
            result.BankName = investorBank?.BankName;
            result.BankAccount = investorBank?.BankAccount;
            result.OwnerAccount = investorBank?.OwnerAccount;

            //var cashFlow = _bondSharedService.GetCashFlowContract(result.TotalValue, ngayDauTu, policyDetailFind, policyFind, bondSecondary, bondInfo);
            //if (cashFlow == null)
            //{
            //    throw new FaultException(new FaultReason($"Không tìm thấy thông tin dòng tiền"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            //}
            //result.AllActuallyProfit = cashFlow.ActuallyProfit;
            //result.AllProfit = cashFlow.ActuallyProfit;
            //result.TotalIncome = result.TotalValue + result.AllActuallyProfit;

            //var soLuongDonGia = _bondSharedService.CalculateQuantityAndUnitPrice(orderFind.TotalValue ?? 0, bondSecondary.Id, orderFind.PaymentFullDate ?? DateTime.Now, policyFind.TradingProviderId);

            //result.Quantity = soLuongDonGia.Quantity;
            //result.UnitPrice = soLuongDonGia.UnitPrice;

            //result.AppCashFlow = cashFlow;
            result.PaymentInfo = _mapper.Map<AppPaymentInfoDto>(paymentInfo ?? new());
            result.PaymentInfo.PaymentNote = orderFind.PaymentNote;

            if (orderFind.Status == OrderStatus.DANG_DAU_TU && orderFind.PaymentFullDate == null)
            {
                result.PaymentFullDate = null;
                result.DueDate = null;
                result.AppCashFlow = new();
                result.ProfitNow = null;
                result.AllActuallyProfit = null;
                result.TotalIncome = null;
                result.Quantity = null;
                result.UnitPrice = null;
            }

            //giao dịch
            //result.TransactionList = new();
            //danh sách tiền vào
            //var listPayment = _orderPaymentRepository.FindAll(orderId, -1, -1, null, OrderPaymentStatus.DA_THANH_TOAN);
            //foreach (var payment in listPayment.Items)
            //{
            //    result.TransactionList.Add(new AppBondTransactionListDto
            //    {
            //        TranDate = payment.TranDate,
            //        Amount = payment.PaymentAmnount,
            //        Desciption = payment.Description,
            //        Type = InvestTransactionTypes.NAP_TIEN_VAO
            //    });
            //}
            ////danh sách tiền ra chưa xử lý

            //result.TransactionList = result.TransactionList.OrderByDescending(o => o.TranDate).ToList();

            return result;
        }

    }
}

using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.BondEntities.Dto.BondOrder;
using EPIC.BondEntities.Dto.BondPolicy;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Order;
using EPIC.FileEntities.Settings;
using EPIC.IdentityRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondOrderShareService : IBondOrderShareService
    {
        private readonly ILogger<BondOrderShareService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BondOrderRepository _orderRepository;
        private readonly BondOrderPaymentRepository _orderPaymentRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        private readonly BondCalendarRepository _calendarRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BankRepository _bankRepository;
        private readonly BondContractTemplateRepository _contractTemplateRepository;
        private readonly BondSecondaryContractRepository _bondSecondaryContractRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly SaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IBondSharedService _bondSharedService;

        public BondOrderShareService(
            ILogger<BondOrderShareService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IBondSharedService bondSharedService)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _orderPaymentRepository = new BondOrderPaymentRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            _calendarRepository = new BondCalendarRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _contractTemplateRepository = new BondContractTemplateRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _mapper = mapper;
            _bondSharedService = bondSharedService;
        }

        /// <summary>
        /// lấy thông tin sổ lệnh của nhà đầu tư theo mã chứng khoán
        /// </summary>
        /// <param name="securityCompany"></param>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        public List<SCBondOrderDto> GetListInvestOrderByInvestor(int securityCompany, string stockTradingAccount)
        {
            List<SCBondOrderDto> result = new();
            var listOrder = _orderRepository.GetListInvestOrderByInvestor(securityCompany, stockTradingAccount);
            foreach (var orderItem in listOrder)
            {
                var resultItem = new SCBondOrderDto();

                resultItem = _mapper.Map<SCBondOrderDto>(orderItem);

                //Lấy thông tin dự án
                var bondInfoFind = _productBondInfoRepository.FindById(orderItem.BondId);
                if (bondInfoFind != null)
                {
                    resultItem.BondInfo = _mapper.Map<SCBondInfoDto>(bondInfoFind);
                }

                //Lấy thông tin phân phối đầu tư
                var bondSecondaryFind = _productBondSecondaryRepository.FindSecondaryById(orderItem.SecondaryId, orderItem.TradingProviderId);
                if (bondSecondaryFind != null)
                {
                    resultItem.BondSecondary = _mapper.Map<SCBondSecondaryDto>(bondSecondaryFind);
                }

                //Lấy thông tin chính sách
                var policyFind = _productBondSecondaryRepository.FindPolicyById(orderItem.PolicyId);
                if (policyFind != null)
                {
                    resultItem.BondPolicy = _mapper.Map<SCBondPolicyDto>(policyFind);
                }

                //Lấy thông tin kỳ hạn
                var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(orderItem.PolicyDetailId);
                if (policyDetailFind != null)
                {
                    resultItem.BondPolicyDetail = _mapper.Map<SCBondPolicyDetailDto>(policyDetailFind);
                }

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

            var bondInfo = _productBondInfoRepository.FindById(orderFind.BondId);
            if (bondInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin lô trên app với orderId = {orderId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lô"), new FaultCode(((int)ErrorCode.BondInfoNotFound).ToString()), "");
            }

            var policyFind = _productBondSecondaryRepository.FindPolicyById(orderFind.PolicyId, orderFind.TradingProviderId);
            if (policyFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin chính sách trên app với orderId = {orderId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            var bondSecondary = _productBondSecondaryRepository.FindSecondaryById(policyFind.SecondaryId, policyFind.TradingProviderId);
            if (bondSecondary == null)
            {
                _logger.LogError($"Không tìm thấy thông tin bán theo kỳ hạn trên app với orderId = {orderId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán theo kỳ hạn"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }

            var policyDetailFind = _productBondSecondaryRepository.FindPolicyDetailById(orderFind.PolicyDetailId, orderFind.TradingProviderId);
            if (policyDetailFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin kỳ hạn trên app với orderId = {orderId}");
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            }

            var identificationFind = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
            if (identificationFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin giấy tờ nhà đầu tư trên app với orderId = {orderId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin giấy tờ nhà đầu tư"), new FaultCode(((int)ErrorCode.InvestorIdentificationNotFound).ToString()), "");
            }

            var investorBank = _managerInvestorRepository.GetBankById(orderFind.InvestorBankAccId);
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

            var paymentInfo = _businessCustomerRepository.FindBusinessCusBankById(orderFind.BusinessCustomerBankAccId);
            if (paymentInfo == null)
            {
                _logger.LogError($"Không tìm thấy thông tin tài khoản thụ hưởng dlsc trong chi tiết order trên app với orderId = {orderId}, investorId = {investorId}, BusinessCustomerBankAccId = {orderFind.BusinessCustomerBankAccId}");
                //throw new FaultException(new FaultReason($"Không tìm thấy thông tin ngân hàng thụ hưởng của đại lý sơ cấp"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            #endregion

            var result = _mapper.Map<AppOrderInvestorDetailDto>(orderFind);
            
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
            result.SecondaryId = orderFind.SecondaryId;
            result.BondCode = bondInfo.BondCode;
            result.PolicyName = policyFind.Name;
            result.PeriodQuantity = policyDetailFind.PeriodQuantity;
            result.PeriodType = policyDetailFind.PeriodType;
            result.Profit = policyDetailFind.Profit;
            result.InterestPeriodType = policyDetailFind.InterestPeriodType;
            result.InterestType = policyDetailFind.InterestType;
            string interestPeriodType = null;
            if (policyDetailFind.InterestPeriodType == PeriodType.NGAY)
            {
                interestPeriodType = "Ngày";
            }
            else if (policyDetailFind.InterestPeriodType == PeriodType.THANG)
            {
                interestPeriodType = "Tháng";
            }
            else if (policyDetailFind.InterestPeriodType == PeriodType.NAM)
            {
                interestPeriodType = "Năm";
            }
            result.InterestPeriod = policyDetailFind.InterestPeriodQuantity + " " + interestPeriodType;
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
            result.DueDate = _bondSharedService.CalculateDueDate(policyDetailFind, ngayDauTu);

            var profit = _bondSharedService.CalculateListInterest(bondInfo, policyFind, policyDetailFind, ngayDauTu, result.TotalValue, true);
            if (profit == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin trái tức"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            result.ProfitNow = _bondSharedService.ProfitNow(ngayDauTu, result.DueDate.Value, policyDetailFind.Profit, orderFind.TotalValue);

            result.ContractAddress = investorContractAddress?.ContactAddress;
            result.IdNo = identificationFind?.IdNo;
            result.IdType = identificationFind?.IdType;
            result.BankName = investorBank?.BankName;
            result.BankAccount = investorBank?.BankAccount;
            result.OwnerAccount = investorBank?.OwnerAccount;

            var cashFlow = _bondSharedService.GetCashFlowContract(result.TotalValue, ngayDauTu, policyDetailFind, policyFind, bondSecondary, bondInfo);
            if (cashFlow == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin dòng tiền"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            result.AllActuallyProfit = cashFlow.ActuallyProfit;
            result.AllProfit = cashFlow.ActuallyProfit;
            result.TotalIncome = result.TotalValue + result.AllActuallyProfit;

            var soLuongDonGia = _bondSharedService.CalculateQuantityAndUnitPrice(orderFind.TotalValue, bondSecondary.Id, orderFind.PaymentFullDate ?? DateTime.Now, policyFind.TradingProviderId);

            result.Quantity = soLuongDonGia.Quantity;
            result.UnitPrice = soLuongDonGia.UnitPrice;

            result.AppCashFlow = cashFlow;
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
            result.TransactionList = new();
            //danh sách tiền vào
            var listPayment = _orderPaymentRepository.FindAll(orderId, -1, -1, null, OrderPaymentStatus.DA_THANH_TOAN);
            foreach (var payment in listPayment.Items)
            {
                result.TransactionList.Add(new AppTransactionListDto
                {
                    TranDate = payment.TranDate,
                    Amount = payment.PaymentAmount,
                    Desciption = payment.Description,
                    Type = InvestTransactionTypes.NAP_TIEN_VAO
                });
            }
            //danh sách tiền ra chưa xử lý

            result.TransactionList = result.TransactionList.OrderByDescending(o => o.TranDate).ToList();

            return result;
        }

    }
}

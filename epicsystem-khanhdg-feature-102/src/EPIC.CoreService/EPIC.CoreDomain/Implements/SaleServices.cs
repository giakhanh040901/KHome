using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.BondDomain.Interfaces;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.CoreRepositories;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.CoreHistoryUpdate;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.SaleAppStatistical;
using EPIC.GarnerRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestRepositories;
using EPIC.Notification.Services;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Humanizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Transactions;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.CoreDomain.Implements
{
    public class SaleServices : ISaleServices
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Sale> _logger;
        private readonly IConfiguration _configuration;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly SaleRepository _saleRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApproveRepository _approveRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly EPIC.InvestRepositories.BlockadeLiberationRepository _investBlockadeLiberationRepository; //phong toả giải toả bên invest 
        private readonly EPIC.BondRepositories.BondBlockadeLiberationRepository _bondBlockadeLiberationRepository; //phong toả giải toả bên bond
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly CollabContractTemplateRepository _collabContractTemplateRepository;
        private readonly SaleCollabContractRepository _saleCollabContractRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly DistributionRepository _distributionInvestRepository;
        private readonly InvestPolicyRepository _investPolicyRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly CoreHistoryUpdateRepository _coreHistoryUpdateRepository;
        private readonly IBondOrderShareService _bondOrderShareServices;
        private readonly IInvestOrderShareServices _investOrderShareServices;
        private readonly ISaleShareServices _saleShareServices;
        private readonly IMapper _mapper;
        private readonly NotificationServices _sendEmailServices;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly ISaleExportCollapContractServices _saleExportCollapContractServices;


        public SaleServices(
            IWebHostEnvironment env,
            EpicSchemaDbContext dbContext,
            ILogger<Sale> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IInvestOrderShareServices investOrderShareServices,
            IBondOrderShareService bondOrderShareServices,
            ISaleShareServices saleShareServices,
            IMapper mapper,
            NotificationServices sendEmailServices,
            ISaleExportCollapContractServices saleExportCollapContractServices
            )
        {
            _env = env;
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _coreHistoryUpdateRepository = new CoreHistoryUpdateRepository(_connectionString, _logger);
            _bondBlockadeLiberationRepository = new BondRepositories.BondBlockadeLiberationRepository(_connectionString, _logger); // phong toả giải toả bên bond
            _investBlockadeLiberationRepository = new InvestRepositories.BlockadeLiberationRepository(_connectionString, _logger); //phong toả giải toả bên invest
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _collabContractTemplateRepository = new CollabContractTemplateRepository(_connectionString, _logger);
            _saleCollabContractRepository = new SaleCollabContractRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _distributionInvestRepository = new DistributionRepository(_connectionString, _logger);
            _investPolicyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _investOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _bondOrderShareServices = bondOrderShareServices;
            _investOrderShareServices = investOrderShareServices;
            _saleShareServices = saleShareServices;
            _mapper = mapper;
            _sendEmailServices = sendEmailServices;
            _saleExportCollapContractServices = saleExportCollapContractServices;
        }

        public int Active(int id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _saleRepository.Active(id, username, tradingProviderId);
        }

        public SaleTempDto AddSaleTemp(AddSaleDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _saleRepository.AddSaleTemp(input, username, tradingProviderId);
        }

        public void AddRequestSale(RequestSaleDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            using (var transaction = new TransactionScope())
            {
                var result = new CreateApproveRequestDto
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = input.ActionType,
                    DataType = CoreApproveDataType.SALE,
                    ReferIdTemp = input.SaleTempId,
                    Summary = input.Summary
                };
                _approveRepository.CreateApproveRequest(result, tradingProviderId);

                //Chuyển trạng thái của bảng tạm sang trình duyệt
                _saleRepository.SaleTempRequest(input.SaleTempId);
                transaction.Complete();
            }
            _saleRepository.CloseConnection();
        }

        public void ApproveSale(ApproveSaleDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            int saleId = 0;
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var approve = _approveRepository.GetOneByTemp(input.SaleTempId, CoreApproveDataType.SALE);

                if (approve == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin trình duyệt"), new FaultCode(((int)ErrorCode.CoreApproveNotFound).ToString()), "");
                }
                saleId = _saleRepository.ApproveSaler(input.SaleTempId);

                _approveRepository.ApproveRequestData(new ApproveRequestDto
                {
                    ReferId = approve.ReferId ?? 0,
                    ApproveID = approve.ApproveID,
                    ApproveNote = input.ApproveNote,
                    UserApproveId = userId
                });
                transaction.Complete();
            }
            _saleRepository.CloseConnection();

            //Sinh sale xong thì cập nhật hồ sơ luôn
            var sale = _saleEFRepository.Entity.FirstOrDefault(e => e.SaleId == saleId).ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotFound);
            _saleExportCollapContractServices.UpdateContractFile(sale.SaleId);
        }

        /// <summary>
        /// Huy khi đang trình duyệt
        /// </summary>
        public void CancelSale(CancelSaleDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByTemp(input.SaleTempId, CoreApproveDataType.SALE);
                if (approve == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin trình duyệt"), new FaultCode(((int)ErrorCode.CoreApproveNotFound).ToString()), "");
                }

                _approveRepository.CancelRequest(new CancelRequestDto
                {
                    ApproveID = approve.ApproveID,
                    CancelNote = input.CancelNote,
                });

                _saleRepository.CancelSale(input.SaleTempId);
                transaction.Complete();
            }
            _saleRepository.CloseConnection();
        }

        public int Delete(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _saleRepository.Delete(id, tradingProviderId);
        }

        public SaleInvestorDto FindAllListManager(string referralCode)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var checkInvestorIsSale = _saleRepository.AppCheckSaler(investorId);
            var result = _saleRepository.FindAllListManager(referralCode);
            if (checkInvestorIsSale.Status == 1)
            {
                var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
                //Danh sách ĐLSC TradingProviderId có trong Investor đã là saler
                var listTradingProviderByInvestor = _saleRepository.AppListTradingProviderBySale(saleId).Select(r => r.TradingProviderId).ToList();
                //Danh sách đại lý sơ cấp có trong sale theo referralCode
                var managerSaleTradingProvider = _saleRepository.AppListTradingProvider(result.SaleId);
                // danh sách đại lý của quản lý có trong sale điều hướng mà không có trong investor khi investor đã là sale
                var listSaleNotInInvestor = new List<AppListTradingProviderDto>();
                foreach (var item in managerSaleTradingProvider)
                {
                    if (!listTradingProviderByInvestor.Contains(item.TradingProviderId))
                    {
                        listSaleNotInInvestor.Add(item);
                    }
                }
                if (listSaleNotInInvestor.Count == 0)
                {
                    throw new FaultException(new FaultReason($"Saler quản lý trùng danh sách đại lý với bạn"), new FaultCode(((int)ErrorCode.CoreSaleManagerTradingProviderExistWithInvestor).ToString()), "");
                }
            }
            return result;
        }

        public SaleInvestorDto FindAllListManagerTrading(string referralCode)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _saleRepository.FindAllListManager(referralCode, tradingProviderId);
        }

        public PagingResult<SaleTempDto> FindAllSaleTemp(FilterSaleTempDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            input.TradingProviderId = tradingProviderId;
            var result = _saleEFRepository.FindAllSaleTemp(input);
            return result;
        }

        public SaleTempDto FindSaleTemp(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var saleTempFind = _saleRepository.FindSaleTemp(id, tradingProviderId);
            var result = _mapper.Map<SaleTempDto>(saleTempFind);
            if (saleTempFind.InvestorId != null)
            {
                var investorFind = _managerInvestorRepository.FindById(saleTempFind.InvestorId ?? 0, false);
                if (investorFind != null)
                {
                    var investor = _mapper.Map<InvestorDto>(investorFind);
                    result.Investor = investor;
                    var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleTempFind.InvestorId ?? 0);

                    if (investorIdenDefaultFind != null)
                    {
                        result.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                    }

                    var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleTempFind.InvestorId ?? 0);
                    result.Investor.ListBank = _mapper.Map<List<InvestorBankAccount>>(investorFindBank);
                }
            }

            else if (saleTempFind.BusinessCustomerId != null)
            {
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleTempFind.BusinessCustomerId ?? 0);
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                result.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
            }

            if (new[] { SaleTypes.EMPLOYEE, SaleTypes.COLLABORATOR }.Contains(saleTempFind.SaleType ?? 0))
            {
                var saleManagerFind = _saleRepository.SaleGetById(saleTempFind.SaleParentId ?? 0, tradingProviderId);
                if (saleManagerFind != null)
                {
                    result.SaleManagerName = saleManagerFind.Fullname;
                }

            }
            else if (saleTempFind?.SaleType == SaleTypes.SALE_REPRESENTATIVE)
            {
                var saleManagerFind = _saleRepository.SaleGetById(saleTempFind.SaleParentId ?? 0, tradingProviderId);
                if (saleManagerFind != null)
                {
                    result.SaleManagerName = saleManagerFind.Fullname;
                }
            }
            else if (saleTempFind.SaleType == SaleTypes.MANAGER)
            {
                //Tìm phòng ban của sale
                var departmentFind = _departmentRepository.FindById(saleTempFind.DepartmentId ?? 0, null);

                //Nếu sale không là quản lý của phòng ban // đổ ra thông tin sale quản lý
                if (departmentFind != null && id != departmentFind.ManagerId)
                {
                    var managerSaleFind = _saleRepository.SaleGetById(departmentFind.ManagerId ?? 0, tradingProviderId);
                    if (managerSaleFind != null)
                    {
                        result.SaleManagerName = managerSaleFind.Fullname;
                    }
                }

                /*                //Nếu sale là quản lý của phòng ban, và có phòng ban cấp trên thì lấy thông tin quản lý là sale quản lý của phòng ban cấp trên
                                if (departmentFind != null && id != departmentFind.ManagerId && departmentFind.ManagerId != null && departmentFind.ParentId != null)
                                {
                                    var departmentParentFind = _departmentRepository.FindById(departmentFind.ParentId ?? 0, null);
                                    if (departmentParentFind != null)
                                    {
                                        var managerSaleFind = _saleRepository.SaleGetById(departmentParentFind.ManagerId ?? 0, tradingProviderId);
                                        if (managerSaleFind != null)
                                        {
                                            result.SaleManagerName = managerSaleFind.Fullname;
                                        }
                                    }
                                }*/
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin chi tiết của Sale trong đại lý
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GetDataSaleDto SaleFindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var saleFind = _saleRepository.FindSaleById(id, tradingProviderId);
            var result = _mapper.Map<GetDataSaleDto>(saleFind);
            if (saleFind != null)
            {
                if (saleFind.InvestorId != null)
                {
                    var investorFind = _managerInvestorRepository.FindById(saleFind.InvestorId ?? 0, false);
                    if (investorFind != null)
                    {
                        result.ReferralCode = investorFind.ReferralCodeSelf;
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        result.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleFind.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            result.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }

                        var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleFind.InvestorId ?? 0);
                        result.Investor.ListBank = _mapper.Map<List<InvestorBankAccount>>(investorFindBank);
                    }
                }

                else if (saleFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleFind.BusinessCustomerId ?? 0);
                    result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    if (businessCustomer != null)
                    {
                        result.ReferralCode = businessCustomer.ReferralCodeSelf;
                        var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                        result.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                    }
                }

                if (new[] { SaleTypes.COLLABORATOR, SaleTypes.EMPLOYEE }.Contains(saleFind?.SaleType ?? 0))
                {
                    var saleManagerFind = _saleRepository.SaleGetById(saleFind.SaleParentId ?? 0, tradingProviderId);
                    if (saleManagerFind != null)
                    {
                        result.SaleManagerName = saleManagerFind.Fullname;
                    }
                }
                else if (saleFind?.SaleType == SaleTypes.SALE_REPRESENTATIVE)
                {
                    var saleManagerFind = _saleRepository.SaleGetById(saleFind.SaleParentId ?? 0, null);
                    if (saleManagerFind != null)
                    {
                        result.SaleManagerName = saleManagerFind.Fullname;
                    }
                }

                //Nếu Sale là nhân viên hoặc quản lý
                else if (saleFind.SaleType == SaleTypes.MANAGER)
                {
                    //Tìm phòng ban của sale
                    var departmentFind = _departmentRepository.FindById(saleFind.DepartmentId ?? 0, null);

                    //Nếu sale không là quản lý của phòng ban // đổ ra thông tin sale quản lý
                    // Nếu sale là nhân viên và không có saleParentId thì đổ ra sale quản lý của phòng ban
                    if (departmentFind != null && id != departmentFind.ManagerId)
                    {
                        var managerSaleFind = _saleRepository.SaleGetById(departmentFind.ManagerId ?? 0, tradingProviderId);
                        if (managerSaleFind != null)
                        {
                            result.SaleManagerName = managerSaleFind.Fullname;
                        }
                    }
                    else if (departmentFind != null && (id == departmentFind.ManagerId || id == departmentFind.ManagerId2))
                    {
                        var departmentParentFind = _departmentRepository.FindById(departmentFind.ParentId ?? 0, null);
                        result.IsManagerDepartmentId = true;
                        if (departmentParentFind != null)
                        {
                            var managerSaleFind = _saleRepository.SaleGetById(departmentParentFind.ManagerId ?? 0, tradingProviderId);
                            if (managerSaleFind != null)
                            {
                                string managerSaleName = managerSaleFind.Fullname;
                                if (managerSaleFind.BusinessCustomerId != null)
                                {
                                    var managerSale2Find = _saleRepository.SaleGetById(departmentParentFind.ManagerId2 ?? 0, tradingProviderId);
                                    if (managerSale2Find != null)
                                    {
                                        managerSaleName = managerSale2Find.Fullname;
                                    }
                                }
                                result.SaleManagerName = managerSaleName;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public int UpdateSaleTemp(UpdateSaleTempDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            return _saleRepository.UpdateSaleTemp(input, tradingProviderId, userName);
        }

        public int UpdateSaleTempCms(UpdateSaleTempCmsDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            return _saleRepository.UpdateSaleTempCms(input, tradingProviderId, userName);
        }
        public int DeleteSaleTemp(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _saleRepository.DeleteSaleTemp(id, tradingProviderId);
        }

        public async Task<int> AppSaleRegister(AppSaleRegisterDto input)
        {
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            //throw new Exception();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var createBy = CommonUtils.GetCurrentUsername(_httpContext);
            var resultRegister = _saleRepository.AppSaleRegister(input, investorId, ipAddress, createBy);
            await _sendEmailServices.SendEmailSaleRegisterForSaleManager(investorId, input.SaleManagerId);

            var coreSaleManagerFind = _saleRepository.FindCoreSale(input.SaleManagerId);
            //Nếu sale đang bật auto điều hướng
            //Tự động điều hướng sale đến các đại lý mà sale quản lý đang tham gia
            if (coreSaleManagerFind != null && coreSaleManagerFind.AutoDirectional == YesNo.YES)
            {
                string createdSaleTempBy = null;
                //if (coreSaleManagerFind.InvestorId != null)
                //{
                //    var investorFind = _managerInvestorRepository.FindById(coreSaleManagerFind.InvestorId ?? 0, false);
                //    if (investorFind != null)
                //    {
                //        createdSaleTempBy = investorFind.Phone;
                //    }
                //}

                //else if (coreSaleManagerFind.BusinessCustomerId != null)
                //{
                //    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(coreSaleManagerFind.BusinessCustomerId ?? 0);
                //    if (businessCustomer != null)
                //    {
                //        createdSaleTempBy = businessCustomer.ShortName;
                //    }
                //}
                var tradingProviderIds = (from sale in _dbContext.Sales
                                          join saleTradingProvider in _dbContext.SaleTradingProviders on sale.SaleId equals saleTradingProvider.SaleId
                                          join departmentSale in _dbContext.DepartmentSales on sale.SaleId equals departmentSale.SaleId
                                          where sale.Deleted == YesNo.NO && saleTradingProvider.Deleted == YesNo.NO && departmentSale.Deleted == YesNo.NO
                                          && saleTradingProvider.TradingProviderId == departmentSale.TradingProviderId && saleTradingProvider.Status == Status.ACTIVE
                                          && sale.SaleId == input.SaleManagerId && new[] { SaleTypes.MANAGER, SaleTypes.EMPLOYEE }.Contains(saleTradingProvider.SaleType)
                                          select saleTradingProvider.TradingProviderId).Distinct();

                //var listTradingOfSaleManager = _saleRepository.AppListTradingProviderBySale(input.SaleManagerId).Select(r => r.TradingProviderId);
                AppDirectionSaleDto inputDirectionSale = new()
                {
                    SaleRegisterIds = new int[] { resultRegister.Id },
                    IsCancel = false,
                    SaleType = SaleTypes.EMPLOYEE,
                    TradingProviders = tradingProviderIds.ToArray()
                };
                await DirectionSales(inputDirectionSale, createdSaleTempBy, input.SaleManagerId);
                return AutoDirectionalSale.YES;
            }
            return AutoDirectionalSale.NO;
        }

        public List<SaleRegisterWithTradingDto> AppListSaleRegister(int? tradingProviderId)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var listSaleRegisters = _saleRepository.AppListSaleRegister(saleId, tradingProviderId);
            var result = new List<SaleRegisterWithTradingDto>();
            foreach (var saleRegsiter in listSaleRegisters)
            {
                var investor = _managerInvestorRepository.FindById(saleRegsiter.InvestorId, false);
                if (investor != null)
                {
                    saleRegsiter.ReferralCode = investor.ReferralCodeSelf;
                }

                //Kiểm tra xem Investor đã là Sale hay chưa
                var checkSaleRegisterDirection = _saleRepository.FindSaleByInvestorId(saleRegsiter.InvestorId);
                if (checkSaleRegisterDirection != null)
                {
                    saleRegsiter.TradingProviders = new List<SaleRegisterDirectionToTradingProviderDto>();
                    //Lấy danh sách đại lý mà sale quản lý đang thuộc
                    var managerSaleTradingProvider = _saleRepository.AppListTradingProvider(saleId);
                    if (managerSaleTradingProvider != null)
                    {
                        //Lấy danh sách của SaleRegister nếu đã là Sale
                        var listTradingProviderByRegister = _saleRepository.AppListTradingProviderBySale(checkSaleRegisterDirection.SaleId);

                        //Lấy những đại lí nếu ĐLSC của SaleRegister trùng với ĐLSC của Sale Manager điều hướng
                        var listData = (from list1 in listTradingProviderByRegister
                                        from list2 in managerSaleTradingProvider
                                        where list1.TradingProviderId == list2.TradingProviderId
                                        select list1.TradingProviderId).ToList();
                        //Lấy ra danh sách đại lý đã được lọc
                        foreach (var item in listData)
                        {
                            var tradingProviderFind = _saleRepository.SaleGetById(checkSaleRegisterDirection.SaleId, item);
                            if (tradingProviderFind != null)
                            {
                                var tradingProviderData = _mapper.Map<SaleRegisterDirectionToTradingProviderDto>(tradingProviderFind);

                                saleRegsiter.TradingProviders.Add(tradingProviderData);
                            }
                        }
                    }
                }

                //Nếu đã điều hướng, xem sale đã được điều hướng đến những ĐLSC nào
                if (saleRegsiter.Status == SaleRegisterStatus.DA_DIEU_HUONG || saleRegsiter.Status == SaleRegisterStatus.DA_XU_LY)
                {
                    saleRegsiter.SaleTradingProviders = _saleRepository.ListTradingProviderBySaleRegister(saleRegsiter.SaleRegisterId);
                }
                result.Add(saleRegsiter);
            }
            return result;
        }

        public Sale FindSaleByInvestorId(int investorId)
        {
            return _saleRepository.FindSaleByInvestorId(investorId);
        }

        /// <summary>
        /// lấy lịch sử chỉnh sửa sale theo saleId phân trang
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public PagingResult<CoreHistorySaleUpdateDto> FindAllHistorySale(int saleId, int pageSize, int pageNumber, string keyword)
        {
            var resultPaging = new PagingResult<CoreHistorySaleUpdateDto>();
            var historySale = _coreHistoryUpdateRepository.FindAllHistoryBySaleId(saleId, pageSize, pageNumber, keyword);
            var sale = _mapper.Map<List<CoreHistorySaleUpdateDto>>(historySale.Items);
            resultPaging.Items = sale;
            resultPaging.TotalItems = historySale.Items.Count();
            return resultPaging;
        }

        public PagingResult<ViewSaleDto> FindAllSale(FilterSaleSto input)
        {

            int? tradingProviderId = -1;
            try
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            catch (Exception)
            {
                tradingProviderId = -1;
            }

            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                tradingProviderId = null;
            }
            input.TradingProviderId = tradingProviderId;
            var saleList = _saleEFRepository.FindAllSale(input);
            var result = new PagingResult<ViewSaleDto>();
            var items = new List<ViewSaleDto>();
            result.TotalItems = saleList.TotalItems;
            foreach (var saleItem in saleList.Items)
            {
                if (saleItem.InvestorId != null)
                {
                    if (saleItem.Investor != null)
                    {
                        var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleItem.InvestorId ?? 0);
                        saleItem.Investor.ListBank = _mapper.Map<List<InvestorBankAccount>>(investorFindBank);
                    }
                }

                else if (saleItem.BusinessCustomerId != null)
                {
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(saleItem.BusinessCustomerId ?? 0, -1, 0, null);
                    saleItem.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                items.Add(saleItem);
            }
            result.Items = items;
            return result;
        }

        #region App
        private async Task DirectionSales(AppDirectionSaleDto input, string createdBy = null, int? saleManagerId = null)
        {
            if (input.IsCancel) //huỷ
            {
                foreach (var saleRegisterId in input.SaleRegisterIds)
                {
                    _saleRepository.CancelRegister(saleRegisterId);
                }
            }
            else //điều hướng
            {
                if (input.TradingProviders == null)
                {
                    throw new FaultException(new FaultReason($"Danh sách đại lý không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }

                if (input.SaleType == null)
                {
                    throw new FaultException(new FaultReason($"Loại saler không được bỏ trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }

                foreach (var saleRegisterId in input.SaleRegisterIds)
                {
                    int countFail = 0;
                    foreach (var tradingProvierId in input.TradingProviders)
                    {
                        try
                        {
                            var result = _saleRepository.DirectionSale(new DirectionSaleDto
                            {
                                SaleRegisterId = saleRegisterId,
                                TradingProviderId = tradingProvierId,
                                SaleType = input.SaleType.Value,
                            }, saleManagerId, createdBy);

                            //thông báo đăng ký sale thành công
                            //await _sendEmailServices.SendEmailSaleDirectionSuccess(result.Id, result.TradingProviderId);
                        }
                        catch (Exception ex)
                        {
                            var faultEx = ex as FaultException;
                            if (faultEx != null && int.Parse(faultEx.Code.Name) == (int)ErrorCode.CoreSaleExistInTradingProvider) //nếu đã có trong đại lý đó rồi
                            {
                                countFail++;
                            }
                            _logger.LogError(ex, $"Lỗi điều hướng saler: SaleRegisterId = {saleRegisterId}, tradingProviderId = {tradingProvierId}");
                        }
                    }

                    //chạy qua tất cả các đại lý mà đều đã thuộc thì huỷ
                    if (countFail == input.TradingProviders.Count())
                    {
                        _saleRepository.CancelRegister(saleRegisterId);
                    }
                }
            }
        }

        /// <summary>
        /// Điều hướng sale đến các đại lý,và vai trò là gì
        /// với vai trò là Quản lý sale trên App
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public Task ManagerDirectionSale(AppDirectionSaleDto input)
        {
            return DirectionSales(input, CommonUtils.GetCurrentUsername(_httpContext), CommonUtils.GetCurrentSaleId(_httpContext));
        }

        /// <summary>
        /// Điều hướng sale đến các đại lý,và vai trò là gì
        /// Root thực hiện thao tác điều hướng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public Task RootDirectionSale(AppDirectionSaleDto input)
        {
            return DirectionSales(input);
        }
        public List<AppListTradingProviderDto> AppListTradingProvider()
        {
            var managerSaleId = CommonUtils.GetCurrentSaleId(_httpContext);
            return _saleRepository.AppListTradingProvider(managerSaleId);
        }

        public AppSaleInfoDto AppSaleInfo(int tradingProviderId)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var saleFind = _saleRepository.AppSaleInfo(saleId, tradingProviderId);
            var result = _mapper.Map<AppSaleInfoDto>(saleFind);
            if (saleFind != null)
            {
                if (saleFind.SaleType == SaleTypes.COLLABORATOR)
                {
                    var managerSaleFind = _saleRepository.SaleGetById(saleFind.SaleParentId ?? 0, tradingProviderId);
                    if (managerSaleFind != null)
                    {
                        result.SaleManager = _mapper.Map<AppSaleManagerDto>(managerSaleFind);
                    }
                }
                else if (saleFind.SaleType == SaleTypes.EMPLOYEE)
                {
                    var managerSaleFind = _saleRepository.SaleGetById(saleFind.ManagerId ?? 0, tradingProviderId);
                    if (managerSaleFind != null)
                    {
                        result.SaleManager = _mapper.Map<AppSaleManagerDto>(managerSaleFind);
                    }
                }
                else if (saleFind.SaleType == SaleTypes.MANAGER)
                {
                    //Tìm phòng ban của sale
                    var departmentFind = _departmentRepository.FindById(saleFind.DepartmentId, null);

                    //Nếu sale không là quản lý của phòng ban // đổ ra thông tin sale quản lý
                    if (departmentFind != null && saleId != departmentFind.ManagerId)
                    {
                        var managerSaleFind = _saleRepository.SaleGetById(departmentFind.ManagerId ?? 0, tradingProviderId);
                        if (managerSaleFind != null)
                        {
                            result.SaleManager = _mapper.Map<AppSaleManagerDto>(departmentFind);
                        }
                    }

                    //Nếu sale là quản lý của phòng ban, và có phòng ban cấp trên thì lấy thông tin quản lý là sale quản lý của phòng ban cấp trên
                    if (departmentFind != null && saleId != departmentFind.ManagerId && saleFind.DepartmentParentId != null)
                    {
                        var departmentParentFind = _departmentRepository.FindById(saleFind.DepartmentParentId ?? 0, null);
                        if (departmentParentFind != null)
                        {
                            var managerSaleFind = _saleRepository.SaleGetById(departmentParentFind.ManagerId ?? 0, tradingProviderId);
                            if (managerSaleFind != null)
                            {
                                result.SaleManager = _mapper.Map<AppSaleManagerDto>(managerSaleFind);
                            }
                        }
                    }
                }

                var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleFind.InvestorId ?? 0);
                result.InvestorBankAccounts = _mapper.Map<List<InvestorBankAccountDto>>(investorFindBank);
                foreach (var item in result.InvestorBankAccounts)
                {
                    item.IsDefault = YesNo.NO;
                    if (item.Id == result.InvestorBankAccId)
                    {
                        item.IsDefault = YesNo.YES;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra trạng thái của Sale
        /// </summary>
        /// <returns></returns>
        public AppCheckSaler AppCheckSaler()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = _saleRepository.AppCheckSaler(investorId);
            return result;
        }

        public AppSaleStatusByTrading AppSaleCheckStatusByTrading(int tradingProviderId)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            return _mapper.Map<AppSaleStatusByTrading>(_saleRepository.AppSaleInfo(saleId, tradingProviderId));
        }

        /// <summary>
        /// Lấy thông tin đại lý mà sale đang thuộc
        /// Sale đang đăng nhập
        /// </summary>
        /// <returns></returns>
        public List<AppListTradingProviderDto> AppListTradingProviderBySale()
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var result = _saleRepository.AppListTradingProviderBySale(saleId);
            var businessCustomerGroupTrading = result.GroupBy(t => t.BusinessCustomerId)
                                    .Where(t => t.Any()).ToList();
            foreach (var businessCustomerGroup in businessCustomerGroupTrading)
            {
                if (businessCustomerGroup.Key == 621 && _env.EnvironmentName == EnvironmentNames.Production)
                {
                    var removeItem = result.FirstOrDefault(t => t.TradingProviderId == 174);
                    if (removeItem != null)
                    {
                        result.Remove(removeItem);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách đại lý mà sale quản lý đang thuộc
        /// không chặn typeSale
        /// </summary>
        /// <param name="managerSaleId"></param>
        /// <returns></returns>
        public List<AppListTradingProviderDto> AppListTradingProviderByManagerSale(int managerSaleId)
        {
            return _saleRepository.AppListTradingProviderBySale(managerSaleId);
        }

        public List<AppListTradingProviderDto> AppListTradingProviderBySaleAndStatus()
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            //Danh sách ĐLSC mà Sale đang thuộc
            var listTradingProvider = _saleRepository.AppListTradingProviderBySale(saleId);
            var result = new List<AppListTradingProviderDto>();
            foreach (var item in listTradingProvider)
            {
                var statusSale = _saleRepository.AppSaleInfo(saleId, item.TradingProviderId);
                if (statusSale != null)
                {
                    item.Status = statusSale.Status;
                }
                if (statusSale.Status == Utils.Status.INACTIVE)
                {
                    item.SignDate = null;
                }
                result.Add(item);
            }
            //Danh sách Đại lý mà Sale đang chờ ký
            var listTradingProviderInTemp = _saleRepository.GetListTradingProviderBySaleInTemp(investorId);
            foreach (var item in listTradingProviderInTemp)
            {
                var infoSaleRegister = _saleRepository.SaleGetDataRegister(investorId, item.TradingProviderId);
                if (infoSaleRegister != null)
                {
                    item.RegisterDate = infoSaleRegister.CreatedDate;
                    item.SignDate = null;
                }
                item.Status = SaleStatus.WAIT_TO_SIGN;
                result.Add(item);
            }
            return result;
        }

        #endregion
        public int Update(UpdateSaleDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            return _saleRepository.Update(input, tradingProviderId, userName);
        }

        /// <summary>
        /// Hiển thị danh sách Sale đăng ký SALE_REGISTER
        /// được xem bơi EPIC
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="phone"></param>
        /// <param name="idNo"></param>
        /// <param name="investorName"></param>
        /// <returns></returns>
        public PagingResult<ViewSaleRegisterDto> FindAllSaleRegister(FilterSaleRegisterDto input)
        {
            var result = new PagingResult<ViewSaleRegisterDto>();

            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC)
            {
                var saleRegisterList = _saleEFRepository.FindAllSaleRegister(input);
                var items = new List<ViewSaleRegisterDto>();
                result.TotalItems = saleRegisterList.TotalItems;
                foreach (var saleRegisterItem in saleRegisterList.Items)
                {
                    var sale = saleRegisterItem;
                    var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleRegisterItem.InvestorId);
                    sale.Investor.ListBank = _mapper.Map<List<InvestorBankAccount>>(investorFindBank);
                    var saleManagerFind = _saleRepository.FindById(saleRegisterItem.SaleManagerId, null);
                    if (saleManagerFind != null)
                    {
                        //Nếu điều hướng là khách hàng cá nhân 
                        if (saleManagerFind.InvestorId != null)
                        {
                            var saleManagerInvestorFind = _investorRepository.FindById(saleManagerFind.InvestorId ?? 0);
                            if (saleManagerInvestorFind != null)
                            {
                                var saleManager = _mapper.Map<InvestorDto>(saleManagerInvestorFind);
                                sale.SaleInvestorManager = saleManager;
                                var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleManagerFind.InvestorId ?? 0);

                                if (investorIdenDefaultFind != null)
                                {
                                    sale.SaleInvestorManager.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                                }
                            }
                        }
                        //Nếu quản lý điều hướng là khách hàng doanh nghiệp
                        else if (saleManagerFind.BusinessCustomerId != null)
                        {
                            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleManagerFind.BusinessCustomerId ?? 0);
                            sale.SaleBusinessCustomerManager = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                            var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                            sale.SaleBusinessCustomerManager.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                        }
                    }
                    items.Add(sale);
                }
                result.Items = items;
            }
            return result;
        }

        public AppSaleByReferralCodeDto AppFindSaleByReferralCode(string referralCode, int tradingProviderId)
        {
            return _saleRepository.FindSaleByReferralCode(referralCode, tradingProviderId);
        }

        /// <summary>
        /// Lấy ra thông tin Sale qua mã giới thiệu, có thể là khách hàng cá nhân hoặc khách hàng doanh nghiệp
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public ViewSaleDto FindSaleByReferralCode(string referralCode, string phone)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var saleFind = _saleRepository.FindSaleByReferralCode(referralCode, tradingProviderId, phone);
            var result = _mapper.Map<ViewSaleDto>(saleFind);
            if (saleFind != null)
            {
                if (saleFind.InvestorId != null)
                {
                    var investorFind = _managerInvestorRepository.FindById(saleFind.InvestorId ?? 0, false);
                    if (investorFind != null)
                    {
                        var investor = _mapper.Map<InvestorDto>(investorFind);
                        result.Investor = investor;
                        var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(saleFind.InvestorId ?? 0);

                        if (investorIdenDefaultFind != null)
                        {
                            result.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                        }

                        var investorFindBank = _managerInvestorRepository.AppGetListBankByInvestor(saleFind.InvestorId ?? 0);
                        result.Investor.ListBank = _mapper.Map<List<InvestorBankAccount>>(investorFindBank);
                    }
                }

                else if (saleFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleFind.BusinessCustomerId ?? 0);
                    result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    result.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                }
                var departmentOfSaleFind = _departmentRepository.FindBySaleId(saleFind.SaleId, tradingProviderId);
                if (departmentOfSaleFind != null)
                {
                    result.DepartmentName = departmentOfSaleFind.DepartmentName;
                }
            }
            return result;
        }

        public List<AppListCollabContractDto> ListCollabContract()
        {
            var result = new List<AppListCollabContractDto>();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var saleFind = _saleRepository.ListCollabContract(investorId);
            foreach (var sale in saleFind)
            {
                var appListCollabContract = new AppListCollabContractDto();
                appListCollabContract.SaleTempId = sale.SaleTempId;
                appListCollabContract.TradingProviderId = sale.TradingProviderId;
                appListCollabContract.TradingProviderName = sale.TradingProviderName;
                var collabContracts = _collabContractTemplateRepository.FindAll(-1, 1, CollapContractTemplateStatus.ACTIVE, null, sale.TradingProviderId, CollabContractTempType.INVESTOR).Items;
                appListCollabContract.CollabContracts = new List<AppCollabContractDto>();
                foreach (var collabContract in collabContracts)
                {
                    var collap = _mapper.Map<AppCollabContractDto>(collabContract);
                    appListCollabContract.CollabContracts.Add(collap);
                }
                result.Add(appListCollabContract);
            }
            return result;
        }

        public AppSaleTempSignDto AppSaleTempSign(int saleTempId, bool isAsign)
        {
            var sign = IsSign.No;
            if (isAsign)
            {
                sign = IsSign.YES;
            };

            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = _saleRepository.AppSaleTempSign(investorId, saleTempId, sign);
            return result;
        }

        public List<AppSaleManagerSaleDto> AppManagerSaleChild(int tradingProviderId)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            return _saleRepository.AppManagerSaleChild(saleId, tradingProviderId);
        }

        public AppSaleChildDto AppFindSaleChild(int saleId, int tradingProviderId)
        {
            var saleManagerId = CommonUtils.GetCurrentSaleId(_httpContext);
            return _mapper.Map<AppSaleChildDto>(_saleRepository.AppFindSaleChild(saleId, saleManagerId, tradingProviderId));
        }

        #region Thống kê của Sale
        public AppSalerOverviewDto AppSalerOverview(int tradingProviderId, string typeTime, int? filterNumberTime = null)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var saleFind = _saleEFRepository.SaleInfoInDepartment(saleId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotExistInTradingProvider);

            List<int> listSaleIdForChart = new ();

            //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)
            var listTradingIds = _saleEFRepository.FindAllTradingUpFromSaleTrading4Cap(tradingProviderId);

            // Kết quả tổng quan của Sale đang xét
            var result = AppSalerOverview(saleId, tradingProviderId, listTradingIds);
            result.SignDate = saleFind.SignDate;

            //Tìm phòng ban của sale
            var departmentFind = _departmentRepository.FindById(saleFind.DepartmentId, tradingProviderId);

            //Sale có loại là nhân viên hoặc (quản lý nhưng ko phải là quản lý của phòng ban)
            if (departmentFind != null && (saleFind.SaleType == SaleTypes.EMPLOYEE || (saleFind.SaleType == SaleTypes.MANAGER && (saleId != departmentFind.ManagerId && saleId != departmentFind.ManagerId2))))
            {
                // Danh sách sale cấp dưới trong phòng ban
                var saleChildBySaleParentId = _saleEFRepository.GetAllSaleChild(saleId, saleFind.DepartmentId, tradingProviderId, null, saleFind.StartInDepartmentDate);
                listSaleIdForChart = saleChildBySaleParentId.Select(s => s.SaleId).ToList();
                // Thông kê hợp đồng sale cấp dưới từ ngày Sale vào Phòng ban này
                var statisticOrderBySale = AppGetAllStatisticOrderBySale(listSaleIdForChart, tradingProviderId, null, listTradingIds, saleFind.StartInDepartmentDate);
                result.TotalValueSystem = statisticOrderBySale.Sum(r => r.InitTotalValue) + result.TotalValueMoney ?? 0;
                result.TotalValueSystemToday = statisticOrderBySale.Where(o => o.InvestDate != null && o.InvestDate.Value.Date == DateTime.Now.Date).Sum(r => r.InitTotalValue);
                result.TotalSalerChild = saleChildBySaleParentId.Count();
                result.TotalSalerChildMonth = saleChildBySaleParentId.Where(s => s.StartDate != null && s.StartDate.Value.Month == DateTime.Now.Month && s.StartDate.Value.Year == DateTime.Now.Year).Count();
            }

            //Nếu sale là quản lý của phòng ban // đổ ra thông tin sale quản lý
            if (departmentFind != null && saleFind.SaleType == SaleTypes.MANAGER && (saleId == departmentFind.ManagerId || saleId == departmentFind.ManagerId2))
            {
                decimal totalSaleChild = 0;
                decimal totalSaleChildMonth = 0;
                var listSaleChildren = new List<DepartmentSaleDto>();
                //Đệ quy lấy danh sách sale cấp dưới
                _saleShareServices.DeQuyDepartmentSaleChild(tradingProviderId, saleFind.DepartmentId, null, ref totalSaleChild, ref totalSaleChildMonth, ref listSaleChildren);
                listSaleIdForChart = listSaleChildren.Select(s => s.SaleId).ToList();
                // Tổng doanh số phòng ban được tính THỜI GIAN từ lúc Sale quản lý được xét làm quản lý
                var statisticOrderBySale = AppGetAllStatisticOrderBySale(listSaleChildren.Select(s => s.SaleId).ToList(), tradingProviderId, null, listTradingIds, saleFind.ManagerStartDate);
                result.TotalValueSystem = statisticOrderBySale.Sum(r => r.InitTotalValue);
                result.TotalValueSystemToday = statisticOrderBySale.Where(o => o.InvestDate != null && o.InvestDate.Value.Date == DateTime.Now.Date).Sum(r => r.InitTotalValue);
                /*decimal? tongDoanhSo = 0;
                decimal? tongDoanhSoTrongThang = 0;
                int? tongHopDong = 0;
                int? tongHopDongTrongThang = 0;

                //Lặp doanh số và hợp đồng qua từng sale
                foreach (var item in listSaleChildren)
                {
                    try
                    {
                        var caHeThong = _saleRepository.AppSalerOverview(item.SaleId, tradingProviderId, listTradingIds);
                        tongDoanhSo += caHeThong.TotalValueMoney;
                        tongDoanhSoTrongThang += caHeThong.TotalValueMoneyMonth;
                        tongHopDong += caHeThong.TotalContract;
                        tongHopDongTrongThang += caHeThong.TotalContractMonth;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Không tìm thấy thông tin nhà đầu tư có investorId = {item.InvestorId}, saleId = {item.SaleId}");
                    }
                }
                result.TotalValueMoney = tongDoanhSo;
                result.TotalValueMoneyMonth = tongDoanhSoTrongThang;
                result.TotalContract = tongHopDong;
                result.TotalContractMonth = tongHopDongTrongThang;*/
                result.TotalSalerChild = totalSaleChild;
                result.TotalSalerChildMonth = totalSaleChildMonth;
            }

            result.TopSaleChilds = TopSaleChild(saleId, tradingProviderId, saleFind.DepartmentId, null, listTradingIds, saleFind.StartInDepartmentDate);
            listSaleIdForChart.Add(saleId);
            DateTime startDate = FilterChartStatisticOrder.StartDate(typeTime, filterNumberTime ?? 0);
            // Nếu ngày bắt đầu lớn hơn ngày vào phòng ban
            //startDate = (saleFind.StartInDepartmentDate != null && startDate.Date < saleFind.StartInDepartmentDate.Value.Date) ? saleFind.StartInDepartmentDate.Value.Date : startDate.Date;
            var dates = Enumerable.Range(0, (DateTime.Now.Date - startDate.Date).Days + 1)
                      .Select(offset => startDate.Date.AddDays(offset));
            var statisticOrderInDays = AppGetAllStatisticOrderBySale(listSaleIdForChart.ToList(), tradingProviderId, null, listTradingIds, startDate)
                .GroupBy(o => o.InvestDate.Value.Date)
                .Select(o => new AppStatisticOrderInDay
                {
                    Date = o.Key,
                    InitTotalValue = o.Sum(x => x.InitTotalValue)
                });

            result.StatisticOrderInDay = dates.GroupJoin(statisticOrderInDays, date => date, order => order.Date,
                (date, orderGroup) => new AppStatisticOrderInDay
                {
                    Date = date,
                    InitTotalValue = orderGroup.Sum(o => o.InitTotalValue)
                }).OrderBy(o => o.Date).ToList();

            return result;
        }

        /// <summary>
        /// Xem lịch sử sale đăng ký
        /// </summary>
        /// <returns></returns>
        public List<AppHistoryRegisterDto> AppHistoryRegister()
        {
            var result = new List<AppHistoryRegisterDto>();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var listRegister = _saleRepository.AppListRegisterByInvestor(investorId);
            foreach (var item in listRegister)
            {
                var itemHistory = new AppHistoryRegisterDto()
                {
                    RegisterDate = item.CreatedDate,
                    DirectionDate = item.DirectionDate,
                    CancelDate = item.CancelDate,
                    Status = item.Status,
                };

                var saleManager = _saleRepository.FindById(item.SaleManagerId);
                if (saleManager != null)
                {
                    if (saleManager.InvestorId != null)
                    {
                        var identification = _managerInvestorRepository.GetDefaultIdentification(saleManager.InvestorId ?? 0, false);
                        if (identification != null)
                        {
                            itemHistory.SaleManagerName = identification.Fullname;
                        }
                        else
                        {
                            _logger.LogError($"Không tìm thấy giấy tờ mặc định của investorId = {saleManager.InvestorId}");
                        }

                        var investor = _investorRepository.FindById(saleManager.InvestorId ?? 0);
                        if (investor != null)
                        {
                            itemHistory.SaleManagerAvatar = investor.AvatarImageUrl;
                            itemHistory.SaleManagerReferralCode = investor.ReferralCodeSelf;
                            itemHistory.SaleManagerPhone = investor.Phone;
                            itemHistory.SaleManagerEmail = investor.Email;
                        }
                        else
                        {
                            _logger.LogError($"Không tìm thấy investor với investorId = {saleManager.InvestorId}");
                        }
                    }

                    else if (saleManager.BusinessCustomerId != null)
                    {
                        var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(saleManager.BusinessCustomerId ?? 0);
                        if (businessCustomer != null)
                        {
                            itemHistory.SaleManagerName = businessCustomer.Name;
                            itemHistory.SaleManagerAvatar = businessCustomer.AvatarImageUrl;
                            itemHistory.SaleManagerReferralCode = businessCustomer.ReferralCodeSelf;
                            itemHistory.SaleManagerPhone = businessCustomer.Phone;
                            itemHistory.SaleManagerEmail = businessCustomer.Email;
                        }
                        else
                        {
                            _logger.LogError($"Không tìm thấy doanh nghiệp với businessCustomerId = {saleManager.BusinessCustomerId}");
                        }
                    }
                }
                result.Add(itemHistory);
            }
            return result;
        }

        /// <summary>
        /// Xem danh sách nhân sự
        /// </summary>
        public AppStatisticPersonnelDto StatisticPersonnelBySale(int? tradingProviderId, int? saleType, DateTime? startDate, DateTime? endDate, string keyword)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var result = new AppStatisticPersonnelDto();
            result.Personnels = new();
            IEnumerable<AppStatisticOrderBySale> revenueOfAllSale = Enumerable.Empty<AppStatisticOrderBySale>();

            var saleFind = _saleEFRepository.SaleInfoInDepartment(saleId, tradingProviderId ?? 0)
                .ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotExistInTradingProvider);

            //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)
            var listTradingIds = _saleEFRepository.FindAllTradingUpFromSaleTrading4Cap(tradingProviderId ?? 0);

            endDate = endDate ?? DateTime.Today.Date;

            // Nếu không truyền ngày thì mặc định là 1 tuần
            startDate = startDate ?? DateTime.Today.Date.AddDays(-6);

            //Nếu ngày bắt đầu lớn hơn ngày sale lên làm quản lý phòng ban hoặc vào phòng ban
            startDate = (saleFind.ManagerStartDate != null && saleFind.ManagerStartDate.Value.Date > startDate) ? saleFind.ManagerStartDate.Value.Date
                        : (saleFind.StartInDepartmentDate != null && saleFind.StartInDepartmentDate.Value.Date > startDate.Value.Date) ? saleFind.StartInDepartmentDate.Value.Date 
                        : startDate;

            //Tìm phòng ban của sale
            var departmentFind = _departmentRepository.FindById(saleFind.DepartmentId, null);

            //Sale có loại là nhân viên hoặc (quản lý nhưng ko phải là quản lý của phòng ban)
            if (saleFind.SaleType == SaleTypes.EMPLOYEE || (saleFind.SaleType == SaleTypes.MANAGER && departmentFind != null && (saleId != departmentFind.ManagerId && saleId != departmentFind.ManagerId2)))
            {
                // Danh sách Sale con
                var saleChildInDepartment = _saleEFRepository.GetAllSaleChild(saleId, saleFind.DepartmentId, tradingProviderId ?? 0, saleType, startDate, endDate);
                foreach (var nhanSu in saleChildInDepartment)
                {
                    var revenueOfSale = AppGetAllStatisticOrderBySale(new List<int> { nhanSu.SaleId }, tradingProviderId ?? 0, null, listTradingIds, startDate, endDate);
                    revenueOfAllSale = revenueOfAllSale.Union(revenueOfSale);

                    nhanSu.Surplus = revenueOfSale.Sum(r => r.TotalValue);
                    nhanSu.Sales = revenueOfSale.Sum(r => r.InitTotalValue);
                    nhanSu.TotalContract = revenueOfSale.Count();
                    result.Personnels.Add(nhanSu);
                }
            }
            // Sale là quản lý của phòng ban
            else if (saleFind.SaleType == SaleTypes.MANAGER && departmentFind != null && (saleId == departmentFind.ManagerId || saleId == departmentFind.ManagerId2))
            {
                // Đệ quy lấy tất cả danh sách sale có trong phòng ban và các phòng ban cấp dưới
                var listSaleChildren = DeQuyDepartmentSaleChild(tradingProviderId ?? 0, saleFind.DepartmentId, saleType, null, endDate);
                foreach (var nhanSu in listSaleChildren)
                {
                    var revenueOfSale = AppGetAllStatisticOrderBySale(new List<int> { nhanSu.SaleId }, tradingProviderId ?? 0, null, listTradingIds, startDate, endDate);
                    revenueOfAllSale = revenueOfAllSale.Union(revenueOfSale);
                    nhanSu.Surplus = revenueOfSale.Sum(r => r.TotalValue);
                    nhanSu.Sales = revenueOfSale.Sum(r => r.InitTotalValue);
                    nhanSu.TotalContract = revenueOfSale.Count();
                    result.Personnels.Add(nhanSu);
                }
            }

            // Lấy tổng số hợp đồng theo ngày
            var totalContract = revenueOfAllSale.GroupBy(r => r.InvestDate.Value.Date)
                .Select(g => new AppStatisticChartPersonnelDto { Date = g.Key, TotalContract = g.Count() });

            // Lấy tổng số nhân viên theo ngày
            var totalPersonnel = result.Personnels.GroupBy(r => r.StartDate.Value.Date)
                .Select(g => new AppStatisticChartPersonnelDto { Date = g.Key, TotalPersonnel = g.Count() });

            // Liệt kê ngày trong khoảng thời gian đã chọn
            var dates = Enumerable.Range(0, (endDate.Value.Date - startDate.Value.Date).Days + 1)
                      .Select(offset => startDate.Value.Date.AddDays(offset));

            //Gộp cả tổng hợp đồng và tổng nhân viên lại theo ngày
            var totalStatistic = Enumerable.Empty<AppStatisticChartPersonnelDto>();
            var statisticPersonnel = totalStatistic.Union(totalPersonnel).Union(totalContract)
                .GroupBy(r => r.Date)
                .Select(g => new AppStatisticChartPersonnelDto
                {
                    Date = g.Key,
                    TotalPersonnel = g.Sum(x => x.TotalPersonnel),
                    TotalContract = g.Sum(x => x.TotalContract),
                });

            // Chèn thêm các ngày chưa có trong dữ liệu hợp đồng
            var dataStatisticOrders = dates.GroupJoin(
                                        statisticPersonnel,
                                        date => date,
                                        order => order.Date,
                                        (date, orderGroup) => new AppStatisticChartPersonnelDto
                                        {
                                            Date = date,
                                            TotalPersonnel = orderGroup.Sum(o => o.TotalPersonnel),
                                            TotalContract = orderGroup.Sum(o => o.TotalContract)
                                        }).OrderBy(o => o.Date);

            IEnumerable<AppStatisticChartPersonnelDto> statisticChart = Enumerable.Empty<AppStatisticChartPersonnelDto>();
            // Nhóm các dữ liệu
            switch ((endDate.Value.Date - startDate.Value.Date).Days)
            {
                case <= 30: // Nếu dưới 30 ngày, group by theo ngày
                    statisticChart = dataStatisticOrders
                                        .GroupBy(o => o.Date.Date)
                                        .Select(g => new AppStatisticChartPersonnelDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Day,
                                            TotalPersonnel = g.Sum(o => o.TotalPersonnel),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.DAY
                                        });
                    break;
                case > 30 and <= 70: // Nếu từ 30 - 70 ngày, group by theo tuần
                    statisticChart = dataStatisticOrders
                                        .GroupBy(o => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                                        .Select(g => new AppStatisticChartPersonnelDto
                                        {
                                            Date = g.Min(o => o.Date),
                                            Time = g.Key,
                                            TotalPersonnel = g.Sum(o => o.TotalPersonnel),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.WEEK
                                        });
                    break;
                default: // Nếu từ 70 ngày trở lên, group by theo tháng
                    statisticChart = dataStatisticOrders
                                        .GroupBy(o => new DateTime(o.Date.Year, o.Date.Month, 1))
                                        .Select(g => new AppStatisticChartPersonnelDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Month,
                                            TotalPersonnel = g.Sum(o => o.TotalPersonnel),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.MONTH
                                        });
                    break;
            }
            result.StatisticChartPersonnels = statisticChart.OrderBy(r => r.Date).ToList();
            result.Personnels = result.Personnels.OrderByDescending(o => o.Sales).ThenBy(o => o.StartDate)
                     .Select((item, i) => { item.Index = (item.Sales != 0) ? i + 1 : null; return item; }).ToList();
            return result;
        }

        /// <summary>
        /// Sale cấp trên hoặc quản lý xem chi tiết thông tin nhân viên Sale
        /// </summary>
        public AppPersonnelSaleInfoDto FindPersonnelSaleById(int saleId, int tradingProviderId)
        {
            // Id sale xem
            var saleIdView = CommonUtils.GetCurrentSaleId(_httpContext);

            // Sale xem thông tin
            var saleView = _saleEFRepository.SaleInfoInDepartment(saleIdView, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.CoreSaleNotExistInTradingProvider);

            // Ngày bắt đầu thống kê doanh số của Sale
            DateTime? startDate = (saleView.ManagerStartDate != null) ? saleView.ManagerStartDate : saleView.StartInDepartmentDate;

            //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)
            var listTradingIds = _saleEFRepository.FindAllTradingUpFromSaleTrading4Cap(tradingProviderId);

            var saleFind = _saleRepository.AppSaleInfo(saleId, tradingProviderId);
            if (saleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sale trong đại lý"), new FaultCode(((int)ErrorCode.CoreSaleNotExistInTradingProvider).ToString()), "");
            }
            var investorFind = _dbContext.Investors.AsNoTracking().FirstOrDefault(i => i.InvestorId == saleFind.InvestorId && i.Deleted == YesNo.NO);
            var statisticOrderBySale = AppGetAllStatisticOrderBySale(new List<int> { saleId }, tradingProviderId, null, listTradingIds, startDate);
            var result = new AppPersonnelSaleInfoDto
            {
                SaleName = saleFind.Fullname,
                AvatarImageUrl = saleFind.AvatarImageUrl,
                DepartmentName = saleFind.DepartmentName,
                ReferralCode = saleFind.ReferralCode,
                StartDate = saleFind.SignDate,
                SaleType = saleFind.SaleType ?? 0,
                SaleId = saleFind.SaleId,
                Email = investorFind?.Email,
                Phone = investorFind?.Phone,
                InvestTotalValueMoney = statisticOrderBySale.Where(s => s.ProjectType == ProjectTypes.INVEST).Sum(i => i.InitTotalValue),
                GarnerTotalValueMoney = statisticOrderBySale.Where(s => s.ProjectType == ProjectTypes.GARNER).Sum(i => i.InitTotalValue),
                RealEstateTotalValueMoney = statisticOrderBySale.Where(s => s.ProjectType == ProjectTypes.REAL_ESTATE).Sum(i => i.InitTotalValue),
            };
            return result;
        }
        /// <summary>
        /// Thống kê dữ liệu hợp đồng cho nút hợp đồng sale app, productType = null: lấy tất cả, productType = 1 : lấy thông tin bond, productType = 2 : lấy thông tin invest , 
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        public AppStatisticContractBySaleDto ThongKeHopDongSaleApp(AppContractOrderFilterDto input)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var saleFind = _saleRepository.SaleGetById(saleId, input.TradingProviderId);
            if (saleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sale trong đại lý {input.TradingProviderId}"), new FaultCode(((int)ErrorCode.CoreSaleNotExistInTradingProvider).ToString()), "");
            }
            var result = new AppStatisticContractBySaleDto();
            result.ContractOrders = new();
            //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)
            var listTradingIds = _saleEFRepository.FindAllTradingUpFromSaleTrading4Cap(input.TradingProviderId);


            AppSaleFilterContractDto filter = new()
            {
                SaleReferralCode = saleFind.ReferralCode,
                TradingProviderId = input.TradingProviderId,
                Status = input.Status,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                ListTradingIds = listTradingIds,
                Keyword = input.Keyword,
            };

            DateTime endDate = input.EndDate ?? DateTime.Today.Date;

            // Nếu không truyền ngày thì mặc định là 1 tuần
            DateTime startDate = input.StartDate ?? DateTime.Today.Date.AddDays(-6);
            if (input.OrderDueSoon)
            {
                filter.Status = OrderStatus.DANG_DAU_TU;
                filter.StartDate = null;
                var orderDue = _investOrderEFRepository.AppGetAllContractOrderBySale(filter);
                foreach (var order in orderDue)
                {
                    var orderQuery = _investOrderEFRepository.FindById(order.OrderId);
                    if (orderQuery.InvestDate == null) continue;

                    var policy = _investPolicyRepository.FindPolicyById(orderQuery.PolicyId);
                    if (policy == null) continue;

                    //Lấy kỳ hạn
                    var policyDetailFind = _investPolicyRepository.FindPolicyDetailById(orderQuery.PolicyDetailId, orderQuery.TradingProviderId, false);
                    if (policyDetailFind == null) continue;
                    var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policy.DistributionId && r.Deleted == YesNo.NO);
                    if (distribution == null) continue;
                    //Lấy ngày bắt đầu tính lãi
                    var investDate = order.InvestDate.Value.Date;

                    //Tính ngày đáo hạn
                    var dueDate = order.DueDate ?? _investOrderRepository.CalculateDueDate(policyDetailFind, investDate, distribution.CloseCellDate, false);
                    if ((input.StartDate == null || input.StartDate <= dueDate) && (input.EndDate == null || input.EndDate >= dueDate))
                    {
                        order.DueDate = dueDate;
                        result.ContractOrders.Add(order);
                    }
                }
            }
            else
            {
                // Lấy hợp đồng Invest
                if (input.ProductType == ProjectTypes.INVEST || input.ProductType == null)
                {
                    result.ContractOrders.AddRange(_investOrderEFRepository.AppGetAllContractOrderBySale(filter));
                }

                // Lấy hợp đồng Garner
                if (input.ProductType == ProjectTypes.GARNER || input.ProductType == null)
                {
                    result.ContractOrders.AddRange(_garnerOrderEFRepository.GetAllOrderBySale(filter));
                }

                // Lấy hợp đồng RealEstate
                if (input.ProductType == ProjectTypes.REAL_ESTATE || input.ProductType == null)
                {
                    var realEstate = _rstOrderEFRepository.GetAllOrderBySale(filter)
                    .Select(x => new HopDongSaleAppDto
                    {
                        OrderId = x.OrderId,
                        CifCode = x.CifCode,
                        BuyDate = x.BuyDate,
                        Status = x.OrderStatus,
                        OrderStatus = x.OrderStatus,
                        ProjectType = x.ProjectType,
                        ContractCode = x.ContractCode,
                        DepositDate = x.DepositDate,
                        ProductItemPrice = x.ProductItemPrice,
                        InitTotalValue = x.ProductItemPrice,
                        CustomerName = x.CustomerName,
                        AvatarImageUrl = x.AvatarImageUrl,
                        ReferralCode = x.ReferralCode,
                    });
                    result.ContractOrders.AddRange(realEstate);
                }
            } 
            
            result.ContractOrders = result.ContractOrders.OrderByDescending(x => x.BuyDate).ToList();
            
            var contractBySale = AppGetAllStatisticOrderBySale(new List<int> { saleId }, input.TradingProviderId, input.ProductType, listTradingIds, startDate, endDate);
            // Liệt kê ngày trong khoảng thời gian đã chọn
            var dates = Enumerable.Range(0, (endDate.Date - startDate.Date).Days + 1)
                      .Select(offset => startDate.Date.AddDays(offset));

            // Chèn thêm các ngày chưa có trong dữ liệu hợp đồng
            var dataStatisticOrders = dates.GroupJoin(
                                        contractBySale.GroupBy(o => o.InvestDate.Value.Date)
                                            .Select(o => new AppStatisticChartContractBySaleDto
                                            {
                                                Date = o.Key,
                                                TotalContract = o.Count()
                                            }),
                                        date => date,
                                        order => order.Date,
                                        (date, orderGroup) => new AppStatisticChartContractBySaleDto
                                        {
                                            Date = date,
                                            TotalContract = orderGroup.Sum(o => o.TotalContract)
                                        }).OrderBy(o => o.Date);

            IEnumerable<AppStatisticChartContractBySaleDto> statisticOrders = Enumerable.Empty<AppStatisticChartContractBySaleDto>();
            // Nhóm các dữ liệu
            switch ((endDate.Date - startDate.Date).Days)
            {
                case <= 30: // Nếu dưới 30 ngày, group by theo ngày
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => o.Date.Date)
                                        .Select(g => new AppStatisticChartContractBySaleDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Day,
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.DAY
                                        });
                    break;
                case > 30 and <= 70: // Nếu từ 30 - 70 ngày, group by theo tuần
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                                        .Select(g => new AppStatisticChartContractBySaleDto
                                        {
                                            Date = g.Min(o => o.Date),
                                            Time = g.Key,
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.WEEK
                                        });
                    break;
                default: // Nếu từ 70 ngày trở lên, group by theo tháng
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => new DateTime(o.Date.Year, o.Date.Month, 1))
                                        .Select(g => new AppStatisticChartContractBySaleDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Month,
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.MONTH
                                        });
                    break;
            }
            result.ChartContractOrders = statisticOrders.OrderBy(r => r.Date).ToList();
            return result;
        }

        /// <summary>
        /// App màn thống kê doanh số
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public AppSaleProceedDto AppThongKeDoanhSo(AppSaleProceedFilterDto input)
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            //var saleFind = _saleEFRepository.SaleInfoInDepartment(saleId, input.TradingProviderId);
            var saleFind = (from sale in _dbContext.Sales.AsNoTracking()
                            join saleTradingProvider in _dbContext.SaleTradingProviders.AsNoTracking() on sale.SaleId equals saleTradingProvider.SaleId
                            join departmentSale in _dbContext.DepartmentSales.AsNoTracking() on saleTradingProvider.SaleId equals departmentSale.SaleId
                            join department in _dbContext.Departments.AsNoTracking() on departmentSale.DepartmentId equals department.DepartmentId
                            where saleTradingProvider.TradingProviderId == input.TradingProviderId && sale.SaleId == saleId && sale.InvestorId != null
                            && sale.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE && saleTradingProvider.Deleted == YesNo.NO
                            && department.Deleted == YesNo.NO && departmentSale.Deleted == YesNo.NO && departmentSale.TradingProviderId == input.TradingProviderId
                            select new                             {
                                SaleId = sale.SaleId,
                                SaleType = saleTradingProvider.SaleType,
                                ReferralCode = _dbContext.Investors.FirstOrDefault(i => i.InvestorId == sale.InvestorId).ReferralCodeSelf,
                                SignDate = saleTradingProvider.CreatedDate,
                                DepartmentId = department.DepartmentId,
                                StartInDepartmentDate = departmentSale.CreatedDate ?? saleTradingProvider.CreatedDate,
                                ManagerStartDate = (saleId == department.ManagerId) ? department.ManagerStartDate :
                                                   (saleId == department.ManagerId2) ? department.Manager2StartDate :
                                                   null,
                            }).FirstOrDefault();
            if (saleFind == null) return null;

            //Lần ngược lên danh sách đại lý là đại lý của đại lý đang xét 4 cấp (khi đại lý đang xét là saler doanh nghiệp)
            var listTradingIds = _saleEFRepository.FindAllTradingUpFromSaleTrading4Cap(input.TradingProviderId);

            // Doanh số của sale hiện tại
            var result = ThongKeDoanhSo(input, saleFind.ReferralCode, listTradingIds);

            // Đếm số nhà đầu tư của Sale
            result.TotalCustomer = _saleEFRepository.ListInvestorOfSale(new List<int> { saleFind.SaleId }).Count();
            // Ngày Sale vào đại lý
            result.SignDate = saleFind.SignDate;

            List<int> totalCustomerOfSale = new();

            DateTime endDate = input.EndDate ?? DateTime.Today.Date;

            // Nếu không truyền ngày thì mặc định là 1 tuần
            DateTime startDate = input.StartDate ?? DateTime.Today.Date.AddDays(-6);

            // Nếu ngày bắt đầu lớn hơn ngày sale lên làm quản lý nếu có
            // Hoặc lớn hơn ngày vào phòng ban
            startDate = (saleFind.ManagerStartDate != null && saleFind.ManagerStartDate.Value.Date > startDate.Date) ? saleFind.ManagerStartDate.Value.Date :
                        (saleFind.StartInDepartmentDate != null && saleFind.StartInDepartmentDate.Value.Date > startDate.Date) ? saleFind.StartInDepartmentDate.Value.Date
                        : startDate;

            // Thông tin hợp đồng doanh số các sản phẩm có mã giới thiệu của Sale Current
            var statisticOrderInDays = AppGetAllStatisticOrderBySale(new List<int> { saleId }, input.TradingProviderId, input.Project, listTradingIds, startDate);
            // Lấy top 5 khách hàng đầu tư có gắn mã giới thiệu của Sale
            if (!input.IsOverview)
            {
                result.TopInvestors = new();
                var topInvestor = statisticOrderInDays.AsParallel().GroupBy(o => o.CifCode)
                    .Select(o => new
                    {
                        CifCode = o.Key,
                        TotalValueMoney = o.Sum(x => x.InitTotalValue),
                        Balance = o.Sum(x => x.TotalValue),
                        TotalContract = o.Count(),
                    }).Where(o => o.TotalValueMoney > 0).OrderByDescending(o => o.TotalValueMoney).Take(5);
                foreach (var item in topInvestor)
                {
                    var investorInfo = (from cifCode in _dbContext.CifCodes
                                        join investor in _dbContext.Investors on cifCode.InvestorId equals investor.InvestorId
                                        where cifCode.CifCode == item.CifCode && cifCode.Deleted == YesNo.NO && investor.Deleted == YesNo.NO
                                        && investor.Status != InvestorStatus.TEMP
                                        select new
                                        {
                                            InvestorId = investor.InvestorId,
                                            ReferralCodeSelf = investor.ReferralCodeSelf,
                                            AvatarImageUrl = investor.AvatarImageUrl,
                                            FullName = _dbContext.InvestorIdentifications
                                                     .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                                                     .OrderByDescending(ii => ii.IsDefault)
                                                     .ThenByDescending(ii => ii.Id)
                                                     .Select(ii => ii.Fullname)
                                                     .FirstOrDefault()
                                        }).FirstOrDefault();
                    result.TopInvestors.Add(new AppTopInvestorOrderDto
                    {
                        InvestorId = investorInfo?.InvestorId ?? 0,
                        FullName = investorInfo?.FullName,
                        AvatarImageUrl = investorInfo?.AvatarImageUrl,
                        ReferralCodeSelf = investorInfo?.ReferralCodeSelf,
                        TotalValueMoney = item.TotalValueMoney,
                        Balance = item.Balance,
                        TotalContract = item.TotalContract,
                    });
                }
                result.TopInvestors = result.TopInvestors.Select((item, i) => { item.Index = (item.TotalValueMoney != 0) ? i + 1 : null; return item; }).ToList();
            }
            //Xem màn tổng quan cả hệ thống
            else
            {
                var listSaleIdChildren = new List<int>();
                decimal totalValueMoney = 0;
                decimal balance = 0;
                int totalContract = 0;
                // Tổng số căn hộ 
                int rstTotalProductItem = 0;

                //Tìm phòng ban của sale
                var departmentFind = _dbContext.Departments.FirstOrDefault(s => saleFind.DepartmentId == s.DepartmentId && s.Deleted == YesNo.NO) ;

                //Sale có loại là nhân viên hoặc (quản lý nhưng ko phải là quản lý của phòng ban)
                if (saleFind.SaleType == SaleTypes.EMPLOYEE || saleFind.SaleType == SaleTypes.MANAGER && departmentFind != null && (saleId != departmentFind.ManagerId && saleId != departmentFind.ManagerId2))
                {
                    totalValueMoney += result.TotalValueMoney;
                    balance += result.Balance;
                    totalContract += result.TotalContract;
                    rstTotalProductItem += result.RstTotalProductItem;

                    var listSaleByParent = _saleRepository.GetSaleByParentSale(saleFind.SaleId, input.TradingProviderId);
                    listSaleIdChildren.AddRange(listSaleByParent.Where(i => i.InvestorId != null).Select(i => i.SaleId));

                    var caHeThong = AppGetAllStatisticOrderBySale(listSaleByParent.Select(r => r.SaleId).ToList(), input.TradingProviderId, input.Project, listTradingIds, startDate);
                    statisticOrderInDays = statisticOrderInDays.Union(caHeThong);
                    totalValueMoney += caHeThong.Sum(r => r.InitTotalValue);
                    balance += caHeThong.Sum(r => r.TotalValue);
                    totalContract += caHeThong.Count();
                    rstTotalProductItem += caHeThong.Count(r => r.ProjectType == ProjectTypes.REAL_ESTATE);
                }
                
                // Sale là quản lý của phòng ban 
                else if (saleFind.SaleType == SaleTypes.MANAGER && departmentFind != null && (saleId == departmentFind.ManagerId || saleId == departmentFind.ManagerId2))
                {
                    // Danh sách sale trong phòng ban và phòng ban cấp dưới
                    var listSaleInDepartment = RecursionDepartmentSaleChild(input.TradingProviderId, saleFind.DepartmentId, null, null, null);

                    // Lấy toàn bộ hợp đồng của Sale trong phòng ban và phòng ban cấp dưới
                    var allSaleInDepartment = AppGetAllStatisticOrderBySale(listSaleInDepartment.Select(r => r.SaleId).ToList(), input.TradingProviderId, input.Project, listTradingIds, startDate);
                    statisticOrderInDays = statisticOrderInDays.Union(allSaleInDepartment);
                    totalValueMoney += allSaleInDepartment.Sum(r => r.InitTotalValue);
                    balance += allSaleInDepartment.Sum(r => r.TotalValue);
                    totalContract += allSaleInDepartment.Count();
                    rstTotalProductItem += allSaleInDepartment.Count(r => r.ProjectType == ProjectTypes.REAL_ESTATE);
                    listSaleIdChildren.AddRange(listSaleInDepartment.Where(i => i.InvestorId != null).Select(i => i.SaleId));
                }

                totalCustomerOfSale.AddRange(_saleEFRepository.ListInvestorOfSale(listSaleIdChildren));

                result.TotalValueMoney = totalValueMoney;
                result.Balance = balance;
                result.TotalContract = totalContract;
                result.RstTotalProductItem = rstTotalProductItem;
                result.TotalCustomer = totalCustomerOfSale.Distinct().Count();

                result.TopSales = TopSaleChild(saleId, input.TradingProviderId, saleFind.DepartmentId, input.Project, listTradingIds, startDate);
            }

            // Liệt kê ngày trong khoảng thời gian đã chọn
            var dates = Enumerable.Range(0, (endDate.Date - startDate.Date).Days + 1)
                      .Select(offset => startDate.Date.AddDays(offset));

            // Chèn thêm các ngày chưa có trong dữ liệu hợp đồng
            var dataStatisticOrders = dates.GroupJoin(
                                        statisticOrderInDays.GroupBy(o => o.InvestDate.Value.Date)
                                            .Select(o => new AppStatisticOrderOfSaleDto
                                            {
                                                Date = o.Key,
                                                InitTotalValue = o.Sum(x => x.InitTotalValue),
                                                Balance = o.Sum(x => x.TotalValue),
                                                TotalContract = o.Count()
                                            }),
                                        date => date,
                                        order => order.Date,
                                        (date, orderGroup) => new AppStatisticOrderOfSaleDto
                                        {
                                            Date = date,
                                            InitTotalValue = orderGroup.Sum(o => o.InitTotalValue),
                                            Balance = orderGroup.Sum(o => o.Balance),
                                            TotalContract = orderGroup.Sum(o => o.TotalContract)
                                        }).OrderBy(o => o.Date);

            IEnumerable<AppStatisticOrderOfSaleDto> statisticOrders = Enumerable.Empty<AppStatisticOrderOfSaleDto>();
            // Nhóm các dữ liệu
            switch ((endDate.Date - startDate.Date).Days)
            {
                case <= 30: // Nếu dưới 30 ngày, group by theo ngày
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => o.Date.Date)
                                        .Select(g => new AppStatisticOrderOfSaleDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Day,
                                            InitTotalValue = g.Sum(o => o.InitTotalValue),
                                            Balance = g.Sum(o => o.Balance),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.DAY
                                        });
                    break;
                case > 30 and <= 70: // Nếu từ 30 - 70 ngày, group by theo tuần
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                                        .Select(g => new AppStatisticOrderOfSaleDto
                                        {
                                            Date = g.Min(o => o.Date),
                                            Time = g.Key,
                                            InitTotalValue = g.Sum(o => o.InitTotalValue),
                                            Balance = g.Sum(o => o.Balance),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.WEEK
                                        });
                    break;
                default: // Nếu từ 70 ngày trở lên, group by theo tháng
                    statisticOrders = dataStatisticOrders
                                        .GroupBy(o => new DateTime(o.Date.Year, o.Date.Month, 1))
                                        .Select(g => new AppStatisticOrderOfSaleDto
                                        {
                                            Date = g.Key,
                                            Time = g.Key.Month,
                                            InitTotalValue = g.Sum(o => o.InitTotalValue),
                                            Balance = g.Sum(o => o.Balance),
                                            TotalContract = g.Sum(o => o.TotalContract),
                                            StartDate = g.Min(o => o.Date),
                                            EndDate = g.Max(o => o.Date),
                                            StatisticType = StatisticTypes.MONTH
                                        });
                    break;
            }
            result.StatisticOrders = statisticOrders.OrderBy(r => r.Date).ToList();
            return result;
        }

        /// <summary>
        /// Sale xem hợp đồng của khách hàng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="projectType"></param>
        /// <returns></returns>
        public AppSaleViewOrderDto SaleViewOrder(int orderId, int projectType)
        {
            var result = new AppSaleViewOrderDto();
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            if (projectType == ProjectTypes.BOND)
            {
                //result.BondOrder = _bondOrderShareServices.AppSaleViewOrder(orderId);
            }
            else if (projectType == ProjectTypes.INVEST)
            {
                //result.InvestOrder = await _investOrderShareServices.AppSaleViewOrder(orderId);
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Bật tắt tự động điều hướng
        /// </summary>
        public void ChangeAutoDirection()
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
            var findCoreSale = _saleRepository.FindCoreSale(saleId);
            //Nếu đang tắt Auto = NO
            if (findCoreSale != null && findCoreSale.AutoDirectional == YesNo.NO)
            {
                _saleRepository.UpdateAutoDirectional(saleId, YesNo.YES);
            }
            //Nếu đang bật Auto = YES
            else if (findCoreSale != null && findCoreSale.AutoDirectional == YesNo.YES)
            {
                _saleRepository.UpdateAutoDirectional(saleId, YesNo.NO);
            }
        }

        #region Thống kê: tổng hợp giữa proc và entity
        /// <summary>
        /// Tổng hợp doanh số hợp đồng mà sale bán hoặc bán hộ
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        private AppSaleProceedDto ThongKeDoanhSo(AppSaleProceedFilterDto input, string referralCodeSelf, List<int> tradingProviderUpIds = null, int? departmentId = null)
        {
            AppSaleProceedDto result = new();
            if (input.Project == null || input.Project == ProjectTypes.INVEST)
            {
                var invest = _investOrderEFRepository.AppGetAllStatisticOrderBySale(new AppContractByListSaleFilterDto
                {
                    ListSaleReferralCode = new HashSet<string> { referralCodeSelf },
                    TradingProviderId = input.TradingProviderId,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    ListTradingIds = tradingProviderUpIds,
                    OrderListStatus = new List<int> { OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN },
                    DepartmentId = departmentId
                });

                result.TotalValueMoney += invest.Sum(g => g.InitTotalValue);
                result.Balance += invest.Sum(g => g.TotalValue);
                result.TotalContract += invest?.Count() ?? 0;
            }
            if (input.Project == null || input.Project == ProjectTypes.GARNER)
            {
                var garner = _garnerOrderEFRepository.AppGetAllStatisticOrderBySale(new AppContractByListSaleFilterDto
                {
                    ListSaleReferralCode = new HashSet<string> { referralCodeSelf },
                    TradingProviderId = input.TradingProviderId,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    ListTradingIds = tradingProviderUpIds,
                    OrderListStatus = new List<int> { OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN },
                    DepartmentId = departmentId
                });

                result.TotalValueMoney += garner.Sum(g => g.InitTotalValue);
                result.Balance += garner.Sum(g => g.TotalValue);
                result.TotalContract += garner?.Count() ?? 0;
            }

            if (input.Project == null || input.Project == ProjectTypes.REAL_ESTATE)
            {
                var realEstate = _rstOrderEFRepository.AppGetAllStatisticOrderBySale(new AppContractByListSaleFilterDto
                {
                    ListSaleReferralCode = new HashSet<string> { referralCodeSelf },
                    TradingProviderId = input.TradingProviderId,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    ListTradingIds = tradingProviderUpIds,
                    OrderListStatus = new List<int> { OrderStatus.DANG_DAU_TU },
                    DepartmentId = departmentId
                });
                result.TotalValueMoney += realEstate.Sum(g => g.ProductItemPrice);
                result.Balance += realEstate.Sum(g => g.ProductItemPrice);
                result.TotalContract += realEstate.Count();
                result.RstTotalProductItem = realEstate.Count();
            }
            return result;
        }

        /// <summary>
        /// Tổng quan sale màn 4 nút
        /// </summary>
        private AppSalerOverviewDto AppSalerOverview(int saleId, int tradingProviderId, List<int> listTradingIds = null)
        {
            var saleFind = _saleRepository.SaleGetById(saleId, tradingProviderId);
            if (saleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sale trong đại lý {tradingProviderId}"), new FaultCode(((int)ErrorCode.CoreSaleNotExistInTradingProvider).ToString()), "");
            }
            var result = _saleRepository.AppSalerOverview(saleId, tradingProviderId, listTradingIds);

            result.ProductTypes = ProductTypeOfTradingProvider(tradingProviderId);

            // GARNER :Lọc ra các hợp đồng active để tính tổng đưa ra tổng quan
            var garner = _garnerOrderEFRepository.GetAllOrderBySale(new AppSaleFilterContractDto
            {
                SaleReferralCode = saleFind.ReferralCode,
                TradingProviderId = tradingProviderId,
                ListTradingIds = listTradingIds,
                OrderListStatus = new List<int> { OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN },
            });

            int ganTotalContractMonth = garner.Where(g => (g.BuyDate != null && g.BuyDate.Value.Month == DateTime.Now.Month)).Count();
            decimal ganTotalValueMonth = (decimal)garner.Where(g => (g.BuyDate != null && g.BuyDate.Value.Month == DateTime.Now.Month)).Sum(g => g.TotalValue);

            // REAL_ESTATE
            var realEstate = _rstOrderEFRepository.GetAllOrderBySale(new AppSaleFilterContractDto
            {
                SaleReferralCode = saleFind.ReferralCode,
                TradingProviderId = tradingProviderId,
                Status = RstOrderStatus.DA_COC,
                ListTradingIds = listTradingIds,
            });
            decimal rstTotalValue = realEstate.Sum(r => r.ProductItemPrice);
            int rstTotalContractMonth = realEstate.Where(g => (g.BuyDate != null && g.BuyDate.Value.Month == DateTime.Now.Month)).Count();
            decimal rstTotalValueMonth = realEstate.Where(g => (g.BuyDate != null && g.BuyDate.Value.Month == DateTime.Now.Month)).Sum(g => g.ProductItemPrice);

            result.TotalValueMoney += (garner?.Sum(g => g.InitTotalValue) ?? 0) + rstTotalValue;
            result.TotalContract += (garner?.Count() ?? 0) + realEstate.Count();
            result.TotalContractMonth += ganTotalContractMonth + rstTotalContractMonth;
            result.TotalValueMoneyMonth += ganTotalValueMonth + rstTotalValueMonth;
            return result;
        }

        /// <summary>
        /// Các sản phẩm (Bond, Invest,...) mà đại lý đang bán
        /// </summary>
        private List<int> ProductTypeOfTradingProvider(int tradingProviderId)
        {
            List<int> productTypes = new();
            HashSet<int> tradingProviderIds = _saleEFRepository.Check(tradingProviderId).ToHashSet();
            tradingProviderIds.Add(tradingProviderId);
            if (_distributionInvestRepository.CheckTradingHaveDistributionInvest(tradingProviderIds) == YesNo.YES)
            {
                productTypes.Add(GeneralProductTypes.INVEST);
            }

            if (_garnerDistributionEFRepository.CheckTradingHaveDistributionGarner(tradingProviderIds))
            {
                productTypes.Add(GeneralProductTypes.GARNER);
            }
            if (_rstOpenSellEFRepository.CheckTradingHaveOpenSell(tradingProviderIds))
            {
                productTypes.Add(GeneralProductTypes.REAL_ESTATE);
            }
            return productTypes;
        }

        /// <summary>
        /// Top 10 sale con (SaleParentId) có doanh số cao nhất theo đại lý và phòng ban
        /// </summary>
        private List<AppTopSaleDto> TopSaleChild(int saleId, int tradingProviderId, int departmentId, int? projectType, List<int> listTradingIds = null, DateTime? startDate = null)
        {
            List<AppTopSaleDto> topSaleChild = new List<AppTopSaleDto>();
            var saleChild = from sale in _dbContext.Sales.AsNoTracking()
                            join investor in _dbContext.Investors.AsNoTracking() on sale.InvestorId equals investor.InvestorId
                            join saleTradingProvider in _dbContext.SaleTradingProviders.AsNoTracking() on sale.SaleId equals saleTradingProvider.SaleId
                            join departmentSale in _dbContext.DepartmentSales.AsNoTracking() on saleTradingProvider.SaleId equals departmentSale.SaleId
                            where sale.Deleted == YesNo.NO && saleTradingProvider.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE
                            && saleTradingProvider.SaleParentId == saleId && saleTradingProvider.TradingProviderId == tradingProviderId 
                            && departmentSale.Deleted == YesNo.NO && departmentSale.DepartmentId == departmentId && departmentSale.TradingProviderId == tradingProviderId
                            select new AppTopSaleDto
                            {
                                SaleId = sale.SaleId,
                                SignDate = saleTradingProvider.CreatedDate,
                                AvatarImageUrl = investor.AvatarImageUrl,
                                SaleType = saleTradingProvider.SaleType,
                                ReferralCode = investor.ReferralCodeSelf,
                                FullName = _dbContext.InvestorIdentifications.AsNoTracking()
                                             .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                                             .OrderByDescending(ii => ii.IsDefault)
                                             .ThenByDescending(ii => ii.Id)
                                             .Select(ii => ii.Fullname)
                                             .FirstOrDefault()
                            };
            foreach (var item in saleChild)
            {
                var listOrder = AppGetAllStatisticOrderBySale(new List<int> { item.SaleId }, tradingProviderId, projectType, listTradingIds, startDate);
                if (!listOrder.Any()) continue;
                item.TotalValueMoney = listOrder.Sum(r => r.InitTotalValue);
                item.Balance = listOrder.Sum(r => r.TotalValue);
                item.TotalContract = listOrder.Count();
                topSaleChild.Add(item);
            }
            return topSaleChild.OrderByDescending(r => r.TotalValueMoney).Take(10)
                .Select((item, i) => { item.Index = (item.TotalValueMoney != 0) ? i + 1 : null; return item; })
                .ToList();
        }

        /// <summary>
        /// Lấy danh sách hợp đồng đã đầu tư hoặc đã cọc BĐS có mã giới thiệu của sale INVEST, GARNER, REAL_ESTATE
        /// </summary>
        private IEnumerable<AppStatisticOrderBySale> AppGetAllStatisticOrderBySale(List<int> saleIds, int tradingProviderId, int? productType, List<int> listTradingIds = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var referralCodeOfSales = (from sale in _dbContext.Sales
                                      join investor in _dbContext.Investors on sale.InvestorId equals investor.InvestorId
                                      where sale.Deleted == YesNo.NO && investor.Deleted == YesNo.NO && saleIds.Contains(sale.SaleId)
                                      select investor.ReferralCodeSelf).ToHashSet();

            IEnumerable<AppStatisticOrderBySale> result = Enumerable.Empty<AppStatisticOrderBySale>();
            //INVEST lấy ra các hợp đồng Invest Active
            if (productType == null || productType == ProjectTypes.INVEST)
            {
                var invest = _investOrderEFRepository.AppGetAllStatisticOrderBySale(new AppContractByListSaleFilterDto
                {
                    ListSaleReferralCode = referralCodeOfSales,
                    TradingProviderId = tradingProviderId,
                    ListTradingIds = listTradingIds,
                    OrderListStatus = new List<int> { OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN },
                    StartDate = startDate,
                    EndDate = endDate
                });
                result = result.Union(invest);
            }

            // GARNER :Lấy ra các hợp đồng active
            if (productType == null || productType == ProjectTypes.GARNER)
            {
                var garner = _garnerOrderEFRepository.AppGetAllStatisticOrderBySale(new AppContractByListSaleFilterDto
                {
                    ListSaleReferralCode = referralCodeOfSales,
                    TradingProviderId = tradingProviderId,
                    ListTradingIds = listTradingIds,
                    OrderListStatus = new List<int> { OrderStatus.DANG_DAU_TU, OrderStatus.TAT_TOAN },
                    StartDate = startDate,
                    EndDate = endDate
                });
                result = result.Union(garner);
            }

            // REAL_ESTATE: Lấy ra các hợp đồng đã cọc
            if (productType == null || productType == ProjectTypes.REAL_ESTATE)
            {
                var realEstate = _rstOrderEFRepository.AppGetAllStatisticOrderBySale(new AppContractByListSaleFilterDto
                {
                    ListSaleReferralCode = referralCodeOfSales,
                    TradingProviderId = tradingProviderId,
                    Status = RstOrderStatus.DA_COC,
                    ListTradingIds = listTradingIds,
                    StartDate = startDate,
                    EndDate = endDate
                });
                result = result.Union(realEstate);
            }
            return result;
        }

        /// <summary>
        /// Đệ quy lấy các sale trong phòng ban và các phòng ban cấp dưới
        /// </summary>
        public IEnumerable<SaleInfoAppDto> DeQuyDepartmentSaleChild(int tradingProviderId, int departmentId, int? saleType, DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<SaleInfoAppDto> saleChild = Enumerable.Empty<SaleInfoAppDto>();

            //Danh sách Sale trong phòng ban đấy
            var departmentSale = _saleEFRepository.GetAllSaleInfoInDepartment(departmentId, tradingProviderId, saleType, startDate, endDate);
            saleChild = saleChild.Union(departmentSale);
            ///Đệ quy với các phòng ban cấp dưới
            var departmentChild = _dbContext.Departments.Where(d => d.ParentId == departmentId && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            foreach (var item in departmentChild)
            {
                var saleChildSub = DeQuyDepartmentSaleChild(tradingProviderId, item.DepartmentId, saleType, startDate, endDate);
                saleChild = saleChild.Union(saleChildSub);
            }
            return saleChild;
        }

        public IEnumerable<RecursionSaleInDepartmentDto> RecursionDepartmentSaleChild(int tradingProviderId, int departmentId, int? saleType, DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<RecursionSaleInDepartmentDto> saleChild = Enumerable.Empty<RecursionSaleInDepartmentDto>();

            //Danh sách Sale trong phòng ban đấy
            var departmentSale = _saleEFRepository.GetAllSaleIdDepartment(departmentId, tradingProviderId, startDate, endDate);
            saleChild = saleChild.Union(departmentSale);
            ///Đệ quy với các phòng ban cấp dưới
            var departmentChild = _dbContext.Departments.Where(d => d.ParentId == departmentId && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            foreach (var item in departmentChild)
            {
                var saleChildSub = RecursionDepartmentSaleChild(tradingProviderId, item.DepartmentId, saleType, startDate, endDate);
                saleChild = saleChild.Union(saleChildSub);
            }
            return saleChild;
        }
        #endregion
    }
}

using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CoreAPI.Controllers;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.FileDomain.Services;
using EPIC.GarnerAPI.Controllers;
using EPIC.GarnerAPI.Controllers.AppController;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerOrderPayment;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using EPIC.GarnerEntities.Dto.GarnerPolicyTemp;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.ImageAPI.Controllers;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.MSB.Dto.Notification;
using EPIC.Notification.Services;
using EPIC.PaymentAPI.Controllers;
using EPIC.PaymentDomain.Interfaces;
using EPIC.UnitTestsBase;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Security;
using IdentityModel.Client;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EPIC.GarnerUnitTests.Order
{
    public class GarnerOrderTestAPI : UnitTestBase
    {
        private readonly ITestOutputHelper _output;

        public GarnerOrderTestAPI(ITestOutputHelper output)
        {
            _baseUrl = "https://api-dev.epicpartner.vn";
            _output = output;
        }

        /// <summary>
        /// Cài đặt các thông tin chung (Chính sách mẫu, cấu trúc mẫu, hợp đồng mẫu)
        /// </summary>
        [Theory()]
        [InlineData(1808, 181)]
        public void AddGeneralSetting(int tradingUserId, int tradingProviderId)
        {
            IHost mockRequestTrading = GetHost<GarnerAPI.Startup>(CreateHttpContextTrading(tradingUserId, tradingProviderId));
            GarnerConfigContractCodeController garnerConfigContractCodeController = new(mockRequestTrading.GetService<ILogger<GarnerConfigContractCodeController>>(),
                mockRequestTrading.GetService<IGarnerDistributionServices>());
            GarnerContractTemplateTempController garnerContractTemplateTempController = new(mockRequestTrading.GetService<ILogger<GarnerContractTemplateTempController>>(),
                mockRequestTrading.GetService<IGarnerContractTemplateTempServices>());
            GarnerPolicyTempController garnerPolicyTempController = new(mockRequestTrading.GetService<ILogger<GarnerPolicyTempController>>(),
                mockRequestTrading.GetService<IGarnerPolicyTempServices>());
            FileController fileController = new(mockRequestTrading.GetService<IFileServices>());
            var dbContext = mockRequestTrading.GetService<EpicSchemaDbContext>();
            #region Tạo Cấu trúc mã hợp đồng
            var configContractCodeDetail = new List<CreateConfigContractCodeDetailDto>
            {
                new CreateConfigContractCodeDetailDto
                {
                    SortOrder = 0,
                    Key = ConfigContractCode.FIX_TEXT,
                    Value = ContractCodes.INVEST
                },
                new CreateConfigContractCodeDetailDto
                {
                    SortOrder = 1,
                    Key = ConfigContractCode.ORDER_ID_PREFIX_0
                },
                new CreateConfigContractCodeDetailDto
                {
                    SortOrder = 2,
                    Key = ConfigContractCode.FIX_TEXT,
                    Value = "xUNIT"
                }
            };
            var addConfigContractCode = garnerConfigContractCodeController.AddConfigContractCode(new CreateConfigContractCodeDto
            {
                Name = $"xUNIT Config Ngày {DateTime.Now:dd-MM-yyyy}",
                Description = "Cấu trúc mã hợp đồng uXnit Test",
                ConfigContractCodeDetails = configContractCodeDetail
            });
            if (addConfigContractCode.Status != StatusCode.Success)
            {
                Assert.Fail(addConfigContractCode.Message);
            }
            _output.WriteLine("Thêm cấu trúc mã hợp đồng thành công");
            #endregion

            var filePathInvestor = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "investor-contract-order.docx");
            var fileStreamInvestor = System.IO.File.OpenRead(filePathInvestor);
            var investorFile = new FormFile(fileStreamInvestor, 0, fileStreamInvestor.Length, null, Path.GetFileName(filePathInvestor));
            var uploadFileForInvestor = fileController.UploadFile(investorFile, "garner");
            if (uploadFileForInvestor.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForInvestor.Message);
            }

            var filePathBusinessCustomer = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "business-customer-contract-order.docx");
            var fileStreamBusinessCustomer = System.IO.File.OpenRead(filePathBusinessCustomer);
            var bussinessCustomerFile = new FormFile(fileStreamBusinessCustomer, 0, fileStreamBusinessCustomer.Length, null, Path.GetFileName(filePathBusinessCustomer));
            var uploadFileForBusiness = fileController.UploadFile(bussinessCustomerFile, "invest");
            if (uploadFileForBusiness.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForBusiness.Message);
            }

            // Thêm mẫu hợp đồng
            var addContractTemplateTemp = garnerContractTemplateTempController.Add(new CreateGarnerContractTemplateTempDto
            {
                Name = $"Hợp đồng đặt lệnh xUNIT Ngày {DateTime.Now:dd-MM-yyyy}",
                ContractType = 1, // HĐ đặt lệnh
                ContractSource = 3, //Online
                FileInvestor = uploadFileForInvestor.Data.ToString(),
                FileBusinessCustomer = uploadFileForBusiness.Data.ToString(),
                Description = "Hợp đồng test đặt lệnh"
            });
            if (addContractTemplateTemp.Status != StatusCode.Success)
            {
                Assert.Fail(addContractTemplateTemp.Message);
            }
            _output.WriteLine("Thêm mẫu hợp đồng đặt lệnh thành công");

            // Thêm chính sách mẫu
            var addPolicyTemp = garnerPolicyTempController.Add(new CreateGarnerPolicyTempDto
            {
                Name = $"Chính sách xUNIT {DateTime.Now:HH-mm} Ngày {DateTime.Now:dd-MM-yyyy}",
                Code = $"xUnit {DateTime.Now:yyyyMMddHHmm}",
                CalculateType = CalculateTypes.GROSS,
                Classify = InvestPolicyClassify.FLEX,
                Description = " xUnit Test Thêm chính sách",
                GarnerType = PolicyGarnerTypes.LINH_HOAT,
                IncomeTax = 5,
                InterestType = InterestTypes.CUOI_KY,
                InvestorType = InvestorType.ALL,
                IsTransferAssets = YesNo.YES,
                MaxMoney = 1000000000,
                MaxWithdraw = 100000000,
                MinWithdraw = 50000,
                MinInvestDay = 5,
                MinMoney = 100000,
                OrderOfWithdrawal = 1,
                SortOrder = 1,
                TransferAssetsFee = 15,
                WithdrawFee = 5,
                WithdrawFeeType = WithdrawFeeTypes.SO_TIEN,
                PolicyDetails = new List<CreateGarnerPolicyDetailTempDto>
                {
                    new CreateGarnerPolicyDetailTempDto
                    {
                        SortOrder = 1,
                        Name = "3 Tháng",
                        ShortName = "xUnit3MM",
                        InterestPeriodQuantity = 3,
                        InterestPeriodType = PeriodType.THANG,
                        InterestType = InterestTypes.CUOI_KY,
                        PeriodType = PeriodType.THANG,
                        PeriodQuantity = 3,
                        Profit = 10,
                    }
                },
                ContractTemplateTemps = new List<CreateGarnerContractTemplateTempDto>()
            });
            if (addPolicyTemp.Status != StatusCode.Success)
            {
                Assert.Fail(addPolicyTemp.Message);
            }
            _output.WriteLine("Thêm Chính sách mẫu thành công");
        }

        /// <summary>
        /// Them phan phoi
        /// </summary>
        private int AddDistribution(IHost mockRequestTrading, EpicSchemaDbContext dbContext)
        {
            GarnerProductController garnerProductController = new(mockRequestTrading.GetService<ILogger<GarnerProductController>>(),
                mockRequestTrading.GetService<IGarnerProductServices>());
            GarnerProductTradingProviderController garnerProductTradingProviderController = new(mockRequestTrading.GetService<ILogger<GarnerProductTradingProviderController>>(),
                mockRequestTrading.GetService<IGarnerProductTradingProviderServices>());
            GarnerDistributionController garnerDistributionController = new(mockRequestTrading.GetService<ILogger<GarnerDistributionController>>(),
                mockRequestTrading.GetService<IGarnerDistributionServices>(), mockRequestTrading.GetService<IGarnerContractTemplateServices>());
            GarnerPolicyTempController garnerPolicyTempController = new(mockRequestTrading.GetService<ILogger<GarnerPolicyTempController>>(),
                mockRequestTrading.GetService<IGarnerPolicyTempServices>());
            GarnerContractTemplateTempController garnerContractTemplateTempController = new(mockRequestTrading.GetService<ILogger<GarnerContractTemplateTempController>>(),
                mockRequestTrading.GetService<IGarnerContractTemplateTempServices>());
            GarnerConfigContractCodeController garnerConfigContractCodeController = new(mockRequestTrading.GetService<ILogger<GarnerConfigContractCodeController>>(),
                mockRequestTrading.GetService<IGarnerDistributionServices>());
            GarnerContractTemplateController garnerContractTemplateController = new(mockRequestTrading.GetService<ILogger<GarnerContractTemplateController>>(),
                mockRequestTrading.GetService<IGarnerContractTemplateServices>());
            FileController fileController = new(mockRequestTrading.GetService<IFileServices>());

            // Dự án có thể phân phối cho đại lý
            string keywordProject = "xUnit";
            var productCanDistribution = garnerProductController.GetListProductByTradingProvider();
            if (productCanDistribution.Status != StatusCode.Success)
            {
                Assert.Fail(productCanDistribution.Message);
            }
            var productCanDistributionData = JsonSerializer.Deserialize<List<GarnerProductByTradingProviderDto>>(JsonSerializer.Serialize(productCanDistribution.Data));
            var productFirst = productCanDistributionData.Where(p => p.Code.ToLower().Contains(keywordProject.ToLower())).FirstOrDefault();
            if (productFirst == null) Assert.Fail("Không tìm thấy dự án phân phối xUnit cho đại lý");

            // Danh sách tài khoảng ngân hàng của đại lý
            var listBankOfTradingProvider = garnerProductTradingProviderController.FindBankByTrading(null, null);
            var listBankOfTradingProviderData = JsonSerializer.Deserialize<List<BusinessCustomerBankDto>>(JsonSerializer.Serialize(productCanDistribution.Data));
            if (listBankOfTradingProvider.Status != StatusCode.Success)
            {
                Assert.Fail(listBankOfTradingProvider.Message);
            }
            if (listBankOfTradingProviderData.Count() == 0) Assert.Fail("Không tìm thấy ngân hàng của đại lý");

            // Thêm phân phối cho đại lý
            var insertDistribution = garnerDistributionController.Add(new CreateGarnerDistributionDto
            {
                ProductId = productFirst.Id,
                OpenCellDate = DateTime.Now.Date,
                CloseCellDate = DateTime.Now.Date.AddYears(1),
                TradingBankAccountCollects = new List<int> { 1781 },
                TradingBankAccountPays = new List<int>{ 1781 },
            });
                
            if (insertDistribution.Status != StatusCode.Success)
            {
                Assert.Fail(insertDistribution.Message);
            }
            var distributionData = JsonSerializer.Deserialize<GarnerDistributionDto>(JsonSerializer.Serialize(insertDistribution.Data));

            #region Thêm chính sách vào phân phối
            // Lấy danh sách chính sách mẫu của đại lý đã cài trước đó
            var policyTempFind = garnerPolicyTempController.FindAllNoPermission(Status.ACTIVE);
            var policyTempFindData = JsonSerializer.Deserialize<List<GarnerPolicyTempDto>>(JsonSerializer.Serialize(policyTempFind.Data));
            if (policyTempFind.Status != StatusCode.Success)
            {
                Assert.Fail(policyTempFind.Message);
            }
            if (policyTempFindData.Count() == 0) Assert.Fail("Không tìm chính sách mẫu để cấu hình cho phân phối");

            // Lấy chính sách mẫu đầu tiên để tạo
            var policyTempFirst = policyTempFindData.OrderByDescending(x => x.Id).FirstOrDefault();

            // Thêm chính sách vào phân phối
            var insertPolicy = garnerDistributionController.AddPolicy(new CreatePolicyDto
            {
                DistributionId = distributionData.Id,
                PolicyTempId = policyTempFirst.Id,
                Code = policyTempFirst.Code,
                Name = policyTempFirst.Name,
                Classify = policyTempFirst.Classify,
                Description = "xUnit Test Thêm chính sách",
                IsShowApp = YesNo.YES,
                CalculateType = policyTempFirst.CalculateType,
                GarnerType = policyTempFirst.GarnerType,
                IncomeTax = policyTempFirst.IncomeTax,
                InterestPeriodQuantity = policyTempFirst.InterestPeriodQuantity,
                InterestPeriodType = policyTempFirst.InterestPeriodType,
                MinInvestDay = policyTempFirst.MinInvestDay,
                InterestType = policyTempFirst.InterestType,
                InvestorType = policyTempFirst.InvestorType,
                MaxMoney = policyTempFirst.MaxMoney,
                MinMoney = policyTempFirst.MinMoney,
                MaxWithdraw = policyTempFirst.MaxWithdraw,
                MinWithdraw = policyTempFirst.MinWithdraw,
                OrderOfWithdrawal = policyTempFirst.OrderOfWithdrawal,
                SortOrder = policyTempFirst.SortOrder,
                RepeatFixedDate = policyTempFirst.RepeatFixedDate,
                TransferAssetsFee = policyTempFirst.TransferAssetsFee,
                WithdrawFeeType = policyTempFirst.WithdrawFeeType,
                WithdrawFee = policyTempFirst.WithdrawFee,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(5),
                IsTransferAssets = policyTempFirst.IsTransferAssets
            });
            if (insertPolicy.Status != StatusCode.Success)
            {
                Assert.Fail(insertPolicy.Message);
            }
            var insertPolicyData = JsonSerializer.Deserialize<GarnerPolicyDto>(JsonSerializer.Serialize(insertPolicy.Data));
            var showApp = garnerDistributionController.PolicyIsShowApp(insertPolicyData.Id);
            if (showApp.Status != StatusCode.Success)
            {
                Assert.Fail(showApp.Message);
            }
            #endregion

            #region Thêm mẫu hợp đồng
            // Lấy mẫu hợp đồng Temp với ContractSource = 3
            var contractTemplateTemp = garnerContractTemplateTempController.GetAllContractTemplateTemp(ContractSources.ALL);
            if (contractTemplateTemp.Status != StatusCode.Success)
            {
                Assert.Fail(contractTemplateTemp.Message);
            }
            var contractTemplateTempData = JsonSerializer.Deserialize<List<GarnerContractTemplateTempDto>>(JsonSerializer.Serialize(contractTemplateTemp.Data));
            var contractTemplateTempFirst = contractTemplateTempData.OrderByDescending(x => x.Id).Where(x => x.FileInvestor != null).FirstOrDefault();
            if (contractTemplateTempFirst == null) Assert.Fail($"Không tìm thấy mẫu hợp đồng với kiểu Tất cả");

            var filePathInvestor = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "investor-contract-order.docx");
            var fileStreamInvestor = System.IO.File.OpenRead(filePathInvestor);
            var investorFile = new FormFile(fileStreamInvestor, 0, fileStreamInvestor.Length, null, Path.GetFileName(filePathInvestor));
            var uploadFileForInvestor = fileController.UploadFile(investorFile, "garner");
            if (uploadFileForInvestor.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForInvestor.Message);
            }

            var filePathBusinessCustomer = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "business-customer-contract-order.docx");
            var fileStreamBusinessCustomer = System.IO.File.OpenRead(filePathBusinessCustomer);
            var bussinessCustomerFile = new FormFile(fileStreamBusinessCustomer, 0, fileStreamBusinessCustomer.Length, null, Path.GetFileName(filePathBusinessCustomer));
            var uploadFileForBusiness = fileController.UploadFile(bussinessCustomerFile, "invest");
            if (uploadFileForBusiness.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForBusiness.Message);
            }

            // Thêm mẫu hợp đồng
            var addContractTemplateTemp = garnerContractTemplateTempController.Add(new CreateGarnerContractTemplateTempDto
            {
                Name = $"Hợp đồng đặt lệnh xUNIT Ngày {DateTime.Now:dd-MM-yyyy}",
                ContractType = 1, // HĐ đặt lệnh
                ContractSource = 3, //Online
                FileInvestor = uploadFileForInvestor.Data.ToString(),
                FileBusinessCustomer = uploadFileForBusiness.Data.ToString(),
                Description = "Hợp đồng test đặt lệnh"
            });
            if (addContractTemplateTemp.Status != StatusCode.Success)
            {
                Assert.Fail(addContractTemplateTemp.Message);
            }
            var insertContractTemplateTemp = JsonSerializer.Deserialize<GarnerContractTemplateTempDto>(JsonSerializer.Serialize(addContractTemplateTemp.Data));

            var policyFind = garnerDistributionController.FindPolicyByDistribution(distributionData.Id, new GarnerDistributionFilterDto { TradingProviderIds = null, Status = Status.ACTIVE });
            if (policyFind.Status != StatusCode.Success)
            {
                Assert.Fail(policyFind.Message);
            }
            var policyFindData = JsonSerializer.Deserialize<List<GarnerPolicyMoreInfoDto>>(JsonSerializer.Serialize(policyFind.Data));
            if (policyFindData.Count == 0) Assert.Fail($"Không tìm thấy chính sách Active trong phân phối");

            var configContractCode = garnerConfigContractCodeController.GetAllConfigContractCodeStatusActive();
            if (configContractCode.Status != StatusCode.Success)
            {
                Assert.Fail(configContractCode.Message);
            }
            var configContractCodeData = JsonSerializer.Deserialize<List<GarnerConfigContractCodeDto>>(JsonSerializer.Serialize(configContractCode.Data));
            if (configContractCodeData.Count() == 0) Assert.Fail($"Không tìm thấy cấu trúc mẫu hợp đồng");

            // displayType Loại hiển thị: B: Trước khi duyệt hợp đồng, A: Sau khi duyệt hợp đồng
            // contractSource: Kiểu hợp đồng: 3 All
            var garnerContractTemplate = garnerContractTemplateController.Add(new CreateGarnerContractTemplateDto
            {
                PolicyIds = new List<int>{ insertPolicyData.Id },
                ContractSource = ContractSources.ALL,
                ContractTemplateTempId = insertContractTemplateTemp.Id,
                ConfigContractId = configContractCodeData.OrderByDescending(x => x.Id).FirstOrDefault().Id,
                DisplayType = DisplayType.TRUOC_KHI_DUYET,
                StartDate = DateTime.Now.Date
            });
                
            if (garnerContractTemplate.Status != StatusCode.Success)
            {
                Assert.Fail(garnerContractTemplate.Message);
            }
            #endregion

            #region Phê duyệt phân phối
            // Trình duyệt phân phối
            var requestApproveDistribution = garnerDistributionController.RequestProduct(new CreateGarnerRequestDto
            {
                Id = distributionData.Id,
                Summary = "xUNIT -  Phân phối đầu tư  bán theo kỳ hạn",
                RequestNote = "xUnit test"
            });
            // Phê duyệt phân phối
            var approveDistribution = garnerDistributionController.ApproveProduct(new GarnerApproveDto
            {
                Id = distributionData.Id,
                ApproveNote = "xUNIT - Phê duyệt Phân phối đầu tư bán theo kỳ hạn",
            });
            // ROOT duyệt phân phối / Duyệt mới có thể hiển thị trên App
            var checkDistribution = garnerDistributionController.CheckProduct(new GarnerCheckDto
            {
                Id = distributionData.Id,
            });
            #endregion
            _output.WriteLine("Thêm phân phối thành công");

            //dbcontext.garnerpolicies.remove(dbcontext.garnerpolicies.firstordefault(p => p.id == insertpolicydata.id));
            //dbcontext.garnerpolicydetails.removerange(dbcontext.garnerpolicydetails.where(p => p.policyid == insertpolicydata.id));
            //dbcontext.garnerdistributions.remove(dbcontext.garnerdistributions.firstordefault(p => p.id == distributiondata.id));
            //dbcontext.garnercontracttemplates.remove(dbcontext.garnercontracttemplates.firstordefault(p => p.policyid == insertpolicydata.id));
            //dbcontext.savechanges();

            //_output.writeline($"xóa phân phối id: {distributiondata.id} thành công");
            return distributionData.Id;
        }

        [Theory()]
        [InlineData(1808, 181)]
        public void AddDistributionTest(int tradingUserId, int tradingProviderId)
        {
            IHost mockRequestTrading = GetHost<GarnerAPI.Startup>(CreateHttpContextTrading(tradingUserId, tradingProviderId));

            var dbContext = mockRequestTrading.GetService<EpicSchemaDbContext>();

            var distributionId = AddDistribution(mockRequestTrading, dbContext);

            dbContext.GarnerPolicyDetails.RemoveRange(dbContext.GarnerPolicyDetails.Where(p => p.DistributionId == distributionId));
            dbContext.GarnerContractTemplates.Remove(dbContext.GarnerContractTemplates.FirstOrDefault(p => dbContext.GarnerPolicies.Any(pl => pl.DistributionId == distributionId && p.PolicyId == pl.Id)));
            dbContext.GarnerContractTemplateTemps.Remove(dbContext.GarnerContractTemplateTemps.FirstOrDefault(c => dbContext.GarnerContractTemplates.Any(p => p.ContractTemplateTempId == c.Id &&
                                                            dbContext.GarnerPolicies.Any(pl => pl.DistributionId == distributionId && p.PolicyId == pl.Id))));
            dbContext.GarnerApproves.Remove(dbContext.GarnerApproves.FirstOrDefault(p => p.DataType == GarnerApproveDataTypes.GAN_DISTRIBUTION && p.ReferId == distributionId));
            dbContext.GarnerDistributions.Remove(dbContext.GarnerDistributions.FirstOrDefault(p => p.Id == distributionId));
            dbContext.GarnerPolicies.Remove(dbContext.GarnerPolicies.FirstOrDefault(p => p.DistributionId == distributionId));

            dbContext.SaveChanges();

            _output.WriteLine($"Xóa phân phối Id: {distributionId} thành công");
        }

        [Theory()]
        [InlineData(1808, 181)]
        public async void TradingAddOrder(int tradingUserId, int tradingProviderId)
        {
            IHost mockRequestTrading = GetHost<GarnerAPI.Startup>(CreateHttpContextTrading(tradingUserId, tradingProviderId));
            IHost mockCoreRequestTrading = GetHost<CoreAPI.Startup>(CreateHttpContextTrading(tradingUserId, tradingProviderId));
            ManagerInvestorController managerInvestorController = new(mockCoreRequestTrading.GetService<ILogger<ManagerInvestorController>>(),
                mockCoreRequestTrading.GetService<IManagerInvestorServices>(), mockCoreRequestTrading.GetService<NotificationServices>(),
                mockCoreRequestTrading.GetService<IHttpContextAccessor>(), mockCoreRequestTrading.GetService<IInvestorV2Services>());
            GarnerDistributionController garnerDistributionController = new(mockRequestTrading.GetService<ILogger<GarnerDistributionController>>(),
                mockRequestTrading.GetService<IGarnerDistributionServices>(),
                mockRequestTrading.GetService<IGarnerContractTemplateServices>());
            GarnerOrderController garnerOrderController = new(mockRequestTrading.GetService<ILogger<GarnerOrderController>>(),
                mockRequestTrading.GetService<IGarnerOrderServices>(),
                mockRequestTrading.GetService<IGarnerOrderContractFileServices>());
            GarnerProductTradingProviderController garnerProductTradingProviderController = new(mockRequestTrading.GetService<ILogger<GarnerProductTradingProviderController>>(),
                mockRequestTrading.GetService<IGarnerProductTradingProviderServices>());
            GarnerOrderPaymentController garnerOrderPaymentController = new(mockRequestTrading.GetService<ILogger<GarnerOrderPaymentController>>(),
                mockRequestTrading.GetService<IGarnerOrderPaymentServices>(), mockRequestTrading.GetService<GarnerNotificationServices>());
            var dbContext = mockRequestTrading.GetService<EpicSchemaDbContext>();
            string investorPhone = "0987654321";

            // Tìm kiếm nhà đầu tư bằng số điện thoại
            var findInvestor = managerInvestorController.GetListInvestorfilter(new Entities.Dto.ManagerInvestor.FilterManagerInvestorDto
            {
                PageSize = -1,
                Keyword = investorPhone,
                RequireKeyword = true
            });
            if (findInvestor.Status != StatusCode.Success)
            {
                Assert.Fail(findInvestor.Message);
            }
            var findInvestorData = JsonSerializer.Deserialize<PagingResult<ViewManagerInvestorDto>>(JsonSerializer.Serialize(findInvestor.Data));
            if (findInvestorData.Items.Count() == 0)
            {
                Assert.Fail($"Không tìm thấy thông tin nhà đầu tư bằng số điện thoại {investorPhone}");
            }
            var investor = findInvestorData.Items.FirstOrDefault();
            var distributionNewId = AddDistribution(mockRequestTrading, dbContext);
            GarnerPolicy policyDelete = new GarnerPolicy();

            // Tìm các phân phối để đặt hợp đồng
            var findListDistribution = garnerDistributionController.GetAllDistibution( new GarnerDistributionFilterDto 
            { 
                TradingProviderIds = null,
                Status = Status.ACTIVE 
            });
            if (findListDistribution.Status != StatusCode.Success)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(findListDistribution.Message);
            }
            var distributionData = JsonSerializer.Deserialize<List<GarnerDistributionDto>>(JsonSerializer.Serialize(findListDistribution.Data));
            distributionData = distributionData.Where(x => x.Id == distributionNewId).OrderByDescending(x => x.Id).ToList();
            if (distributionData.Count() == 0)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail("Không tìm thấy thông tin phân phối đặt lệnh");
            }
            int distributionId = 0;
            int productId = 0;
            int policyId = 0;
            double minMoneyPolicy = 0;
            foreach (var item in distributionData)
            {
                distributionId = item.Id;
                productId = item.ProductId;
                if (item.Policies.Count == 0)
                {
                    _output.WriteLine($"Không tìm thấy thông tin chính sách của phân phối {item.GarnerProduct.Name}");
                    continue;
                }
                var policyQuery = item.Policies.Where(x => x.Status == Status.ACTIVE);
                foreach (var policy in policyQuery)
                {
                    var contractTemplates = (from ct in dbContext.GarnerContractTemplates
                                             join ctt in dbContext.GarnerContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id
                                             where ct.PolicyId == policy.Id && ct.Deleted == YesNo.NO && ctt.ContractType == ContractTypes.DAT_LENH
                                             && (ctt.ContractSource == ContractSources.ONLINE || ctt.ContractSource == ContractSources.ALL)
                                             && ct.DisplayType == ContractTemplateDisplayType.BEFORE && ct.Status == Status.ACTIVE && ct.Deleted == YesNo.NO && ctt.Deleted == YesNo.NO
                                             select ct).Count();
                    if (contractTemplates == 0)
                    {
                        _output.WriteLine($"Chính sách {policy.Name} không tìm thấy mẫu hợp đồng dành cho nhà đầu tư cá nhân");
                        continue;
                    }
                    if (policy.PolicyDetails.Where(x => x.Status == Status.ACTIVE).Count() == 0)
                    {
                        _output.WriteLine($"Không tìm thấy thông tin kỳ hạn của chính sách {policy.Name}");
                        continue;
                    }
                    policyId = policy.Id;
                    minMoneyPolicy = (double)policy.MinMoney;
                    _output.WriteLine($"Sử dụng chính sách {policy.Name} để đặt lệnh");
                    break;
                }
            }
            if (policyId == 0) Assert.Fail("Không tìm thấy thông tin chính sách hợp lệ để đặt lệnh");
            // Tạo hợp đồng
            var restAddOrder = await garnerOrderController.Add(new CreateGarnerOrderDto
            {
                CifCode = investor.CifCode,
                PolicyId = policyId,
                TotalValue = (decimal)minMoneyPolicy,
                InvestorBankAccId = investor.DefaultBank.Id,
            });
            if (restAddOrder.Status != StatusCode.Success)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(restAddOrder.Message);
            }
            var order = JsonSerializer.Deserialize<GarnerOrderDto>(JsonSerializer.Serialize(restAddOrder.Data));
            var bankTradingProvider = garnerProductTradingProviderController.FindBankByTrading(distributionId, DistributionTradingBankAccountTypes.THU);
            if (bankTradingProvider.Status != StatusCode.Success)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(bankTradingProvider.Message);
            }
            var bankTradingProviderData = JsonSerializer.Deserialize<List<BusinessCustomerBankDto>>(JsonSerializer.Serialize(bankTradingProvider.Data));

            // Tạo OrderPayment
            var orderPayment = garnerOrderPaymentController.Add(new CreateGarnerOrderPaymentDto
            {
                OrderId = order.Id,
                Description = $"xUnit TT {order.ContractCode}",
                PaymentAmount = order.TotalValue,
                TranDate = DateTime.Now,
                TranClassify = 1,
                PaymentType = 1,
                TradingBankAccId = bankTradingProviderData.FirstOrDefault().BusinessCustomerBankAccId,
                TranType = 1,
            });
                
            if (orderPayment.Status != StatusCode.Success)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(restAddOrder.Message);
            }
            
            var orderPaymentData = JsonSerializer.Deserialize<GarnerOrderPaymentDto>(JsonSerializer.Serialize(orderPayment.Data));

            // Duyệt OrderPayment
            //var approvePayment = await garnerOrderPaymentController.OrderPaymentApprove(orderPaymentData.Id, OrderPaymentStatus.DA_THANH_TOAN);
            //if (approvePayment.Status != StatusCode.Success)
            //{
            //    Assert.Fail(restAddOrder.Message);
            //}

            // Duyệt Order
            //var approveOrder = await garnerOrderController.OrderApprove(order.Id);
            //if (approveOrder.Status != StatusCode.Success)
            //{
            //    Assert.Fail(restAddOrder.Message);
            //}
            _output.WriteLine("Hợp đồng Active thành công");

            dbContext.GarnerOrderContractFiles.RemoveRange(dbContext.GarnerOrderContractFiles.Where(o => o.OrderId == order.Id));
            dbContext.GarnerOrders.Remove(dbContext.GarnerOrders.FirstOrDefault(o => o.Id == order.Id));
            dbContext.GarnerOrderPayments.Remove(dbContext.GarnerOrderPayments.FirstOrDefault(o => o.Id == (int)orderPaymentData.Id));
            DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
            dbContext.SaveChanges();
            _output.WriteLine($"Xóa thông tin hợp đồng {order.ContractCode} thành công");
        }

        private void RunTaskHost()
        {
            IHost mockGarnerRequestTrading = GetHost<GarnerAPI.Startup>();
            IHost mockPayment = GetHost<PaymentAPI.Startup>();
            IHost mockShared = GetHost<SharedAPI.Startup>();
            Task.Run(() => mockGarnerRequestTrading.Run());
            Task.Run(() => mockPayment.Run());
            Task.Run(() => mockShared.Run());
        }

        [Theory]
        [InlineData(4001, 3999)]
        public async void InvestorAddOrders(int userId, int investorId)
        {
            IHost mockRequestInvestor = GetHost<GarnerAPI.Startup>(CreateHttpContextInvestor(userId, investorId));
            IHost mockRequestPayment = GetHost<PaymentAPI.Startup>(CreateHttpContextInvestor(userId, investorId));
            AppGarnerProductController garnerProductController = new(mockRequestInvestor.GetService<ILogger<AppGarnerProductController>>(),
                mockRequestInvestor.GetService<IGarnerDistributionServices>());
            AppInvestorOrderController investorOrderController = new(mockRequestInvestor.GetService<ILogger<AppInvestorOrderController>>(),
                mockRequestInvestor.GetService<IGarnerOrderServices>(), mockRequestInvestor.GetService<IGarnerContractDataServices>(),
                mockRequestInvestor.GetService<IGarnerContractTemplateServices>(), mockRequestInvestor.GetService<IGarnerWithdrawalServices>(),
                mockRequestInvestor.GetService<IGarnerRatingServices>());
            MsbPaymentController msbPaymentController = new(mockRequestPayment.GetService<ILogger<MsbPaymentController>>(),
                mockRequestPayment.GetService<IMsbPaymentServices>());
            // DL_EMIR
            IHost mockTradingProvider = GetHost<GarnerAPI.Startup>(CreateHttpContextTrading(1808, 181));

            var dbContext = mockRequestInvestor.GetService<EpicSchemaDbContext>();

            RunTaskHost();

            // Lấy thông tin investor
            var investor = dbContext.Investors.FirstOrDefault(x => x.InvestorId == investorId && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO);
            var investorBank = dbContext.InvestorBankAccounts.Where(x => x.InvestorId == investor.InvestorId && x.Deleted == YesNo.NO)
                                    .OrderByDescending(x => x.IsDefault).ThenBy(x => x.Id).FirstOrDefault();
            if (investorBank == null)
            {
                Assert.Fail("Không tìm thấy thông tin ngân hàng nhà đầu tư");
            }
            var investorAddress = dbContext.InvestorContactAddresses.Where(x => x.InvestorId == investor.InvestorId && x.Deleted == YesNo.NO)
                                    .OrderByDescending(x => x.IsDefault).ThenBy(x => x.ContactAddressId).FirstOrDefault();

            // Thêm phân phối cho đại lý
            var distributionNewId = AddDistribution(mockTradingProvider, dbContext);
            GarnerPolicy policyDelete = new GarnerPolicy();

            var policies = dbContext.GarnerPolicies.Where(p => p.DistributionId == distributionNewId && p.Deleted == YesNo.NO);
            var policy = new GarnerPolicy();
            if (policies.Any())
            {
                policy = policies.FirstOrDefault();
            }
            // Kiểm tra trước khi tạo hợp đồng
            var checkOrder = investorOrderController.CheckOrder(new AppCheckGarnerOrderDto
            {
                PolicyId = policy.Id,
                TotalValue = policy.MinMoney,
                InvestorBankAccId = investorBank.Id,
            });
            if (checkOrder.Status != StatusCode.Success)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(checkOrder.Message);
            }

            // Tạo hợp đồng
            var resDataOrderInsert = await investorOrderController.InvestorOrderAdd(new AppCreateGarnerOrderDto
            {
                PolicyId = policy.Id,
                TotalValue = policy.MinMoney,
                InvestorBankAccId = investorBank.Id
            });
            if (resDataOrderInsert.Status != StatusCode.Success)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(resDataOrderInsert.Message);
            }
            var resDataOrderInsertData = JsonSerializer.Deserialize<AppGarnerOrderDto>(JsonSerializer.Serialize(resDataOrderInsert.Data));
            _output.WriteLine($"Đặt lệnh thành công với hợp đồng có orderId: {resDataOrderInsertData.Id}");
            long orderId = resDataOrderInsertData.Id;

            int contractTemplates = (from ct in dbContext.GarnerContractTemplates
                                 join ctt in dbContext.GarnerContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id
                                 where ct.PolicyId == policy.Id && ct.Deleted == YesNo.NO && ctt.ContractType == ContractTypes.DAT_LENH
                                 && (ctt.ContractSource == ContractSources.ONLINE || ctt.ContractSource == ContractSources.ALL)
                                 && ct.DisplayType == ContractTemplateDisplayType.BEFORE && ct.Status == Status.ACTIVE && ct.Deleted == YesNo.NO && ctt.Deleted == YesNo.NO
                                 select ct).Count();

            // Kiểm tra sinh đủ số lượng hợp đồng không, delay 6s chờ backgroundjob
            int tryCount = 5;
            while (true)
            {
                try
                {
                    await Task.Delay(6000);
                    // Kiểm tra sinh đủ số lượng hợp đồng không, delay 6s chờ backgroundjob
                    var totalContactOrder = dbContext.GarnerOrderContractFiles.Where(o => o.OrderId == orderId && o.Deleted == YesNo.NO);
                    if (totalContactOrder.Count() != contractTemplates)
                    {
                        Assert.Fail("Hợp đồng chưa sinh đủ số file");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (--tryCount == 0)
                    {
                        DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                        Assert.Fail(ex.Message);
                    }
                }
            };

            // Thanh toán tự động giả lập noti bank gửi về
            string tranSeq = $"{DateTime.Now:yyMMddHHmmss}VASML247";
            var notiPayment = new ReceiveNotificationDto
            {
                TranSeq = tranSeq,
                VaCode = "968668",
                VaNumber = $"968668868EG{orderId}",
                FromAccountName = "nguyen van test",
                FromAccountNumber = "68686868",
                ToAccountName = "CONG TY CO PHAN TEST XUNIT",
                ToAccountNumber = "6868686868",
                TranAmount = $"{resDataOrderInsertData.TotalValue}",
                TranRemark = $"xUNIT-TEST {resDataOrderInsertData.ContractCode} EG{orderId}",
                TranDate = $"{DateTime.Now:yyMMddHHmmss}",
                Signature = $"unitTest"
            };
            notiPayment.Signature = CryptographyUtils.ComputeSha256Hash("Ep1c@2022", notiPayment.TranSeq, notiPayment.TranDate, notiPayment.VaNumber, notiPayment.TranAmount, notiPayment.FromAccountNumber, notiPayment.ToAccountNumber);

            var approveOrder = msbPaymentController.ReceiveNotification(notiPayment);
            if (resDataOrderInsert.Status != StatusCode.Success)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(resDataOrderInsert.Message);
            }

            var checkPaymentOrderAuto = dbContext.MsbNotifications.FirstOrDefault(p => p.TranSeq == tranSeq);
            // Kiểm tra xem có exception từ hệ thống khi nhận noti không
            if (checkPaymentOrderAuto.Exception != null)
            {
                DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                Assert.Fail(checkPaymentOrderAuto.Exception);
            }

            int tryCountPayment = 10;
            while (true)
            {
                try
                {
                    await Task.Delay(6000);
                    // Kiểm tra hợp đồng sau khi active thành công
                    var orderFind = dbContext.GarnerOrders.AsNoTracking().FirstOrDefault(o => o.Id == orderId);
                    if (orderFind == null) Assert.Fail("Không tìm thấy hợp đồng");
                    if (orderFind.Status != OrderStatus.DANG_DAU_TU && orderFind.Status != OrderStatus.CHO_DUYET_HOP_DONG)
                    {
                        Assert.Fail("Hợp đồng chưa active thành công");
                    }
                    // Kiểm tra xem các file đã ký hết chưa
                    if (dbContext.GarnerOrderContractFiles.Any(o => o.OrderId == orderId && o.IsSign == YesNo.NO && o.Deleted == YesNo.NO))
                    {
                        Assert.Fail("Tồn tại file hợp đồng chưa được ký điện tử");
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (--tryCountPayment == 0)
                    {
                        DeleteDistributionAndPolicy(policyDelete, distributionNewId, dbContext);
                        Assert.Fail(ex.Message);
                    }
                }
            };

            _output.WriteLine($"Hợp đồng {resDataOrderInsertData.ContractCode} Active thành công");

            dbContext.GarnerOrderContractFiles.RemoveRange(dbContext.GarnerOrderContractFiles.Where(o => o.OrderId == orderId));
            dbContext.GarnerOrders.Remove(dbContext.GarnerOrders.FirstOrDefault(o => o.Id == orderId));
            dbContext.GarnerOrderPayments.RemoveRange(dbContext.GarnerOrderPayments.Where(o => o.OrderId == orderId));
            dbContext.MsbNotifications.Remove(dbContext.MsbNotifications.FirstOrDefault(o => o.TranSeq == tranSeq));
            dbContext.GarnerDistributions.Remove(dbContext.GarnerDistributions.FirstOrDefault(o => o.Id == distributionNewId));
            policyDelete = dbContext.GarnerPolicies.FirstOrDefault(e => e.DistributionId == distributionNewId);
            if (policyDelete != null)
            {
                dbContext.GarnerPolicies.Remove(dbContext.GarnerPolicies.FirstOrDefault(o => o.Id == policyDelete.Id));
                dbContext.GarnerPolicyDetails.Remove(dbContext.GarnerPolicyDetails.FirstOrDefault(o => o.PolicyId == policyDelete.Id));
            }
            dbContext.SaveChanges();
            _output.WriteLine($"Xóa thông tin hợp đồng {orderId} thành công");
        }

        /// Test Investor thêm hợp đồng mà active tự động
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /*[Theory]
        [InlineData("0987654321", "Test6789")]*/
        private async void InvestorRequestWithdrawal(string username, string password)
        {
            IHost host = GetHost<EPIC.GarnerAPI.Startup>();
            var dbContext = host.GetService<EpicSchemaDbContext>();
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_baseUrl),
            };

            var garnerAPI = new UnitTestsBase.Garner.GarnerAPI(_baseUrl, httpClient);
            var identityServerAPI = new UnitTestsBase.IdentityServer.IdentityServerAPI(_baseUrl, httpClient);
            var paymentAPI = new UnitTestsBase.Payment.PaymentAPI(_baseUrl, httpClient);

            var resInvestor = await httpClient.RequestPhoneEmailTokenAsync(username, password);
            string tokenInvestor = resInvestor.AccessToken;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, tokenInvestor);

            // Lấy thông tin investor
            var investor = dbContext.Investors.FirstOrDefault(x => x.Phone == username && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO);
            var investorBank = dbContext.InvestorBankAccounts.Where(x => x.InvestorId == investor.InvestorId && x.Deleted == YesNo.NO)
                                    .OrderByDescending(x => x.IsDefault).ThenBy(x => x.Id).FirstOrDefault();
            if (investorBank == null)
            {
                Assert.Fail("Không tìm thất thông tin ngân hàng nhà đầu tư");
            }
            var investorAddress = dbContext.InvestorContactAddresses.Where(x => x.InvestorId == investor.InvestorId && x.Deleted == YesNo.NO)
                                    .OrderByDescending(x => x.IsDefault).ThenBy(x => x.ContactAddressId).FirstOrDefault();
            var cifCode = dbContext.CifCodes.FirstOrDefault(x => x.InvestorId == investor.InvestorId && x.Deleted == YesNo.NO);

            var orderByPolicyId = dbContext.GarnerOrders.Where(o => o.CifCode == cifCode.CifCode && o.Status == OrderStatus.DANG_DAU_TU && o.InvestDate != null && o.Deleted == YesNo.NO)
                .GroupBy(x => new { x.PolicyId, x.TradingProviderId }).Select(x => new
                {
                    PolicyId = x.Key.PolicyId,
                    TradingProviderId = x.Key.TradingProviderId,
                    DistributionId = dbContext.GarnerPolicies.FirstOrDefault(p => p.Id == x.Key.PolicyId && p.Deleted == YesNo.NO).DistributionId,
                    TotalValue = x.Sum(x => x.TotalValue),
                    TotalContract = x.Count()
                });

            var withdrawalFirstPolicy = orderByPolicyId.FirstOrDefault();
            // Kiểm tra trước khi yêu cầu rút vốn
            /*var checkRequestWithdrawal = await garnerAPI.ViewChangeWithdrawalRequestAsync(withdrawalFirstPolicy.PolicyId, 50000);
            if (checkRequestWithdrawal.Status != UnitTestsBase.Garner.StatusCode._1)
            {
                Assert.Fail(checkRequestWithdrawal.Message);
            }*/
            // Kiểm tra xem có đang cài chi tự động khi yêu cầu rút vốn không
            var bankPaymentAuto = (from distributionTradingBankAccount in dbContext.GarnerDistributionTradingBankAccounts
                                   join tradingMsbPrefixAccount in dbContext.TradingMSBPrefixAccounts on distributionTradingBankAccount.BusinessCustomerBankAccId equals tradingMsbPrefixAccount.TradingBankAccountId
                                   where distributionTradingBankAccount.DistributionId == withdrawalFirstPolicy.DistributionId && distributionTradingBankAccount.Status == Status.ACTIVE
                                     && distributionTradingBankAccount.Type == DistributionTradingBankAccountTypes.CHI && distributionTradingBankAccount.Deleted == YesNo.NO
                                     && tradingMsbPrefixAccount.Deleted == YesNo.NO && tradingMsbPrefixAccount.TradingProviderId == withdrawalFirstPolicy.TradingProviderId
                                     && distributionTradingBankAccount.IsAuto == YesNo.YES && tradingMsbPrefixAccount.TIdWithoutOtp != null
                                   select distributionTradingBankAccount).FirstOrDefault();
            if (bankPaymentAuto != null)
            {
                _output.WriteLine("Đang có chi tự động khi có yêu cầu rút vốn");
            }
            // Tạo yêu cầu rút vốn
            var withdrawalRequest = await garnerAPI.WithdrawalRequestAsync(JsonSerializer.Deserialize<UnitTestsBase.Garner.AppWithdrawalRequestDto>
                ($"{{ \"policyId\": {withdrawalFirstPolicy.PolicyId}, \"bankAccountId\": {investorBank.Id}, \"amountMoney\": 50000 }}"));
            if (withdrawalRequest.Status != UnitTestsBase.Garner.StatusCode._1)
            {
                Assert.Fail(withdrawalRequest.Message);
            }

            var withdrawalFirst = dbContext.GarnerWithdrawals.OrderByDescending(i => i.Id).Where(x => x.CifCode == cifCode.CifCode && x.Status == 1).FirstOrDefault();
            if (bankPaymentAuto == null && withdrawalFirst != null)
            {
                // Tìm kiếm user ROOT_TRADING_PROVIDER của đại lý theo lệnh rút vốn
                var userFind = (from tradingProvider in dbContext.TradingProviders
                                join user in dbContext.Users on tradingProvider.TradingProviderId equals user.TradingProviderId
                                where tradingProvider.Deleted == YesNo.NO && user.IsDeleted == YesNo.NO && user.UserType == UserTypes.ROOT_TRADING_PROVIDER
                                && tradingProvider.TradingProviderId == withdrawalFirst.TradingProviderId
                                select new
                                {
                                    Username = user.UserName,
                                    BusinessCustomerId = tradingProvider.BusinessCustomerId,
                                }).FirstOrDefault();
                if (userFind == null)
                {
                    Assert.Fail($"Không tìm thấy tài khoản của đại lý tradingProviderId: {withdrawalFirst.TradingProviderId} của yêu cầu rút vốn trên");
                }
                // Login bằng tài khoản TradingProvider 
                var resTrading = await httpClient.RequestPasswordTokenAsync(userFind.Username, "123456");
                string tokenTrading = resTrading.AccessToken;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, tokenTrading);

                // Tìm kiếm ngân hàng chi tiền
                var bankPay = (from businessCustomerBank in dbContext.BusinessCustomerBanks
                               join tradingMSBPrefixAccount in dbContext.TradingMSBPrefixAccounts on businessCustomerBank.BusinessCustomerBankAccId equals tradingMSBPrefixAccount.TradingBankAccountId
                               join distributionTradingBankAccount in dbContext.GarnerDistributionTradingBankAccounts on tradingMSBPrefixAccount.TradingBankAccountId equals distributionTradingBankAccount.BusinessCustomerBankAccId
                               where businessCustomerBank.BusinessCustomerId == userFind.BusinessCustomerId && distributionTradingBankAccount.Deleted == YesNo.NO
                               && distributionTradingBankAccount.DistributionId == withdrawalFirstPolicy.DistributionId && distributionTradingBankAccount.Status == Status.ACTIVE
                               && distributionTradingBankAccount.Type == DistributionTradingBankAccountTypes.CHI
                               select businessCustomerBank).FirstOrDefault();
                if (bankPay == null)
                {
                    Assert.Fail($"Không tìm thấy tài khoản ngân hàng chi tiền của đại lý tradingProviderId: {withdrawalFirst.TradingProviderId} của yêu cầu rút vốn trên");
                }
                // Yêu cầu từ App Đại lý duyệt rút vốn
                var prepareApproveWithdrawal = await garnerAPI.PrepareApprove2Async(JsonSerializer.Deserialize<UnitTestsBase.Garner.PrepareApproveRequestWithdrawalDto>
                    ($"{{ \"tradingBankAccId\": {bankPay.BusinessCustomerBankAccId}, \"withdrawalIds\": [ {withdrawalFirst.Id} ] }}"));
                if (prepareApproveWithdrawal.Status != UnitTestsBase.Garner.StatusCode._1)
                {
                    Assert.Fail(prepareApproveWithdrawal.Message);
                }

                // Duyệt rút vốn
                var approveWithdrawal = await garnerAPI.Approve6Async(JsonSerializer.Deserialize<UnitTestsBase.Garner.GarnerApproveRequestWithdrawalDto>
                    ($"{{ \"tradingBankAccId\": {bankPay.BusinessCustomerBankAccId}, \"withdrawalIds\": [ {withdrawalFirst.Id} ], \"otp\": \"456789\", \"status\": {WithdrawalStatus.DUYET_DI_TIEN}, \"prepare\": {JsonSerializer.Serialize(prepareApproveWithdrawal.Data)}}}"));
                if (approveWithdrawal.Status != UnitTestsBase.Garner.StatusCode._1)
                {
                    Assert.Fail(approveWithdrawal.Message);
                }
            }
        }

        private void DeleteDistributionAndPolicy(GarnerPolicy policyDelete, int distributionNewId, EpicSchemaDbContext dbContext)
        {
            dbContext.GarnerDistributions.Remove(dbContext.GarnerDistributions.FirstOrDefault(o => o.Id == distributionNewId));
            policyDelete = dbContext.GarnerPolicies.FirstOrDefault(e => e.DistributionId == distributionNewId);
            if (policyDelete != null)
            {
                dbContext.GarnerPolicies.Remove(dbContext.GarnerPolicies.FirstOrDefault(o => o.Id == policyDelete.Id));
                dbContext.GarnerPolicyDetails.Remove(dbContext.GarnerPolicyDetails.FirstOrDefault(o => o.PolicyId == policyDelete.Id));
            }
        }
    }
}

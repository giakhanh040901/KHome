using EPIC.CoreAPI.Controllers;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.FileDomain.Services;
using EPIC.FileEntities.Settings;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityServer.Controllers;
using EPIC.ImageAPI.Controllers;
using EPIC.InvestAPI.Controllers;
using EPIC.InvestAPI.Controllers.AppControllers;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplate;
using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestEntities.Dto.InvConfigContractCodeDetail;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.OrderPayment;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.PolicyTemp;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestUnitTest.Order;
using EPIC.MSB.Dto.Notification;
using EPIC.Notification.Services;
using EPIC.PaymentAPI.Controllers;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.DataEntities;
using EPIC.UnitTestsBase;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EPIC.InvestUnitTests.Order
{
    public class InvestOrderTestAPI : UnitTestBase
    {
        private ITestOutputHelper _output;

        public InvestOrderTestAPI(ITestOutputHelper output)
        {
            _output = output;
            _baseUrl = "https://api-dev.epicpartner.vn";
        }

        /// <summary>
        /// Cài đặt các thông tin chung (Chính sách mẫu, cấu trúc mẫu, hợp đồng mẫu)
        /// </summary>
        [Theory]
        [InlineData(1808, 181)]
        public void AddGeneralSetting(int tradingUserId, int tradingProviderId)
        {
            IHost mockRequestTrading = GetHost<InvestAPI.Startup>(CreateHttpContextTrading(tradingUserId, tradingProviderId));
            DistributionController distributionController = new(mockRequestTrading.GetService<ILogger<DistributionController>>(),
                mockRequestTrading.GetService<IDistributionServices>(), mockRequestTrading.GetService<IConfigContractCodeServices>());
            InvestContractTemplateTempController contractTemplateTempController = new(mockRequestTrading.GetService<ILogger<InvestContractTemplateTempController>>(),
               mockRequestTrading.GetService<IInvestContractTemplateTempServices>());
            PolicyTempController policyTempController = new(mockRequestTrading.GetService<ILogger<PolicyTempController>>(),
               mockRequestTrading.GetService<IPolicyTempServices>());
            FileController fileController = new(mockRequestTrading.GetService<IFileServices>());
            var dbContext = mockRequestTrading.GetService<EpicSchemaDbContext>();

            // Thêm Cấu hình mã hợp đồng
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

            var addConfigContractCode = distributionController.AddConfigContractCode(new CreateConfigContractCodeDto
            {
                Name = $"xUNIT Config Ngày {DateTime.Now:dd-MM-yyyy}",
                Description = "Cấu trúc mã hợp đồng uXnit Test",
                ConfigContractCodeDetails = configContractCodeDetail
            });
            if (addConfigContractCode.Status != StatusCode.Success)
            {
                Assert.Fail(addConfigContractCode.Message);
            }
            var insertConfigContractCode = JsonSerializer.Deserialize<InvConfigContractCodeDto>(JsonSerializer.Serialize(addConfigContractCode.Data));
            dbContext.InvestConfigContractCodes.Remove(dbContext.InvestConfigContractCodes.FirstOrDefault(x => x.Id == insertConfigContractCode.Id));
            dbContext.InvestConfigContractCodeDetails.RemoveRange(dbContext.InvestConfigContractCodeDetails.Where(x => x.ConfigContractCodeId == insertConfigContractCode.Id));

            // Thêm mẫu hợp đồng
            var filePathInvestor = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "investor-contract-order.docx");
            var investorFile = new FormFile(File.OpenRead(filePathInvestor), 0, File.OpenRead(filePathInvestor).Length, null, Path.GetFileName(filePathInvestor));
            var uploadFileForInvestor = fileController.UploadFile(investorFile, "invest");
            if (uploadFileForInvestor.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForInvestor.Message);
            }

            var filePathBusinessCustomer = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "business-customer-contract-order.docx");
            var bussinessCustomerFile = new FormFile(File.OpenRead(filePathBusinessCustomer), 0, File.OpenRead(filePathBusinessCustomer).Length, null, Path.GetFileName(filePathBusinessCustomer));
            var uploadFileForBusiness = fileController.UploadFile(bussinessCustomerFile, "invest");
            if (uploadFileForBusiness.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForBusiness.Message);
            }

            var addContractTemplateTemp = contractTemplateTempController.Add(new CreateInvestContractTemplateTempDto
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
            var insertContractTemplateTemp = JsonSerializer.Deserialize<InvestContractTemplateTempDto>(JsonSerializer.Serialize(addContractTemplateTemp.Data));
            dbContext.InvestContractTemplateTemps.Remove(dbContext.InvestContractTemplateTemps.FirstOrDefault(c => c.Id == insertContractTemplateTemp.Id));

            // Thêm chính sách mẫu
            var addPolicyTemp = policyTempController.Add(new CreatePolicyTempDto
            {
                Name = $"Chính sách xUNIT {DateTime.Now:HH-mm} Ngày {DateTime.Now:dd-MM-yyyy}",
                Code = $"xUnit {DateTime.Now:yyyyMMddHHmmss}",
                CalculateType = CalculateTypes.GROSS,
                Classify = InvestPolicyClassify.FLEX,
                Description = " xUnit Test Thêm chính sách",
                ExitFee = 2,
                ExitFeeType = ExitFeeTypes.RUT_THEO_SO_TIEN,
                ExpirationRenewals = ContractExpireTypes.DAO_HAN,
                IncomeTax = 5,
                IsTransfer = YesNo.YES,
                MaxWithDraw = 100,
                MinInvestDay = 5,
                MinMoney = 100000,
                MinTakeContract = 2,
                MinWithdraw = 100000,
                PolicyDisplayOrder = PolicyDisplayOrder.SHORT_TO_LONG,
                RemindRenewals = 1,
                RenewalsType = 1,
                TransferTax = 1,
                Type = InvPolicyType.FLEXIBLE,
                PolicyDetailTemp = new List<CreatePolicyDetailTempDto>
                {
                    new CreatePolicyDetailTempDto
                    {
                        Stt = 1,
                        Name = "3 Tháng",
                        ShortName = "xUnit3MM",
                        InterestPeriodQuantity = 3,
                        InterestPeriodType = PeriodType.THANG,
                        InterestType = InterestTypes.DINH_KY,
                        PeriodType = PeriodType.THANG,
                        PeriodQuantity = 3,
                        Profit = 10,
                        FixedPaymentDate = 5
                    }
                },
            });
            if (addPolicyTemp.Status != StatusCode.Success)
            {
                Assert.Fail(addPolicyTemp.Message);
            }
            var insertPolicyTemp = JsonSerializer.Deserialize<ViewPolicyTempDto>(JsonSerializer.Serialize(addPolicyTemp.Data));
            dbContext.InvestPolicyTemps.Remove(dbContext.InvestPolicyTemps.FirstOrDefault(p => p.Id == insertPolicyTemp.Id));
            dbContext.InvestPolicyDetailTemps.Remove(dbContext.InvestPolicyDetailTemps.FirstOrDefault(p => p.PolicyTempId == insertPolicyTemp.Id));
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Test thêm phân phối và xóa phân phối khi thành công
        /// </summary>
        [Theory()]
        [InlineData(1808, 181)]
        public void AddDistributionTest(int tradingUserId, int tradingProviderId)
        {
            IHost mockRequestTrading = GetHost<InvestAPI.Startup>(CreateHttpContextTrading(tradingUserId, tradingProviderId));

            var dbContext = mockRequestTrading.GetService<EpicSchemaDbContext>();

            var distributionId = AddDistribution(mockRequestTrading, dbContext);

            dbContext.InvestDistributions.Remove(dbContext.InvestDistributions.FirstOrDefault(p => p.Id == distributionId));
            dbContext.InvestPolicies.Remove(dbContext.InvestPolicies.FirstOrDefault(p => p.DistributionId == distributionId));
            dbContext.InvestPolicyDetails.RemoveRange(dbContext.InvestPolicyDetails.Where(p => p.DistributionId == distributionId));
            dbContext.InvestContractTemplates.Remove(dbContext.InvestContractTemplates.FirstOrDefault(p => dbContext.InvestPolicies.Any(pl => pl.DistributionId == distributionId && p.PolicyId == pl.Id)));
            dbContext.InvestContractTemplateTemps.Remove(dbContext.InvestContractTemplateTemps.FirstOrDefault(c => dbContext.InvestContractTemplates.Any(p => p.ContractTemplateTempId == c.Id &&
                                                            dbContext.InvestPolicies.Any(pl => pl.DistributionId == distributionId && p.PolicyId == pl.Id))));
            dbContext.InvestApproves.Remove(dbContext.InvestApproves.FirstOrDefault(p => p.DataType == InvestApproveDataTypes.EP_INV_DISTRIBUTION && p.ReferId == distributionId));
            dbContext.SaveChanges();

            _output.WriteLine($"Xóa phân phối Id: {distributionId} thành công");
        }

        private void AddGeneralSettingData(IHost mockRequestTrading, EpicSchemaDbContext dbContext)
        {
            ProjectController projectController = new(mockRequestTrading.GetService<ILogger<ProjectController>>(),
                mockRequestTrading.GetService<IProjectServices>());
            DistributionController distributionController = new(mockRequestTrading.GetService<ILogger<DistributionController>>(),
                mockRequestTrading.GetService<IDistributionServices>(), mockRequestTrading.GetService<IConfigContractCodeServices>());
            InvestContractTemplateTempController contractTemplateTempController = new(mockRequestTrading.GetService<ILogger<InvestContractTemplateTempController>>(),
               mockRequestTrading.GetService<IInvestContractTemplateTempServices>());
            ContractTemplateController contractTemplateController = new(mockRequestTrading.GetService<ILogger<ContractTemplateController>>(),
               mockRequestTrading.GetService<IContractTemplateServices>());
            PolicyTempController policyTempController = new(mockRequestTrading.GetService<ILogger<PolicyTempController>>(),
               mockRequestTrading.GetService<IPolicyTempServices>());
            FileController fileController = new(mockRequestTrading.GetService<IFileServices>());

            // Thêm Cấu hình mã hợp đồng
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

            var addConfigContractCode = distributionController.AddConfigContractCode(new CreateConfigContractCodeDto
            {
                Name = $"xUNIT Config Ngày {DateTime.Now:dd-MM-yyyy}",
                Description = "Cấu trúc mã hợp đồng uXnit Test",
                ConfigContractCodeDetails = configContractCodeDetail
            });
            if (addConfigContractCode.Status != StatusCode.Success)
            {
                Assert.Fail(addConfigContractCode.Message);
            }
            var insertConfigContractCode = JsonSerializer.Deserialize<InvConfigContractCodeDto>(JsonSerializer.Serialize(addConfigContractCode.Data));
            dbContext.InvestConfigContractCodes.Remove(dbContext.InvestConfigContractCodes.FirstOrDefault(x => x.Id == insertConfigContractCode.Id));
            dbContext.InvestConfigContractCodeDetails.RemoveRange(dbContext.InvestConfigContractCodeDetails.Where(x => x.ConfigContractCodeId == insertConfigContractCode.Id));

            // Thêm mẫu hợp đồng
            var filePathInvestor = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "investor-contract-order.docx");
            var investorFile = new FormFile(File.OpenRead(filePathInvestor), 0, File.OpenRead(filePathInvestor).Length, null, Path.GetFileName(filePathInvestor));
            var uploadFileForInvestor = fileController.UploadFile(investorFile, "invest");
            if (uploadFileForInvestor.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForInvestor.Message);
            }

            var filePathBusinessCustomer = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "business-customer-contract-order.docx");
            var bussinessCustomerFile = new FormFile(File.OpenRead(filePathBusinessCustomer), 0, File.OpenRead(filePathBusinessCustomer).Length, null, Path.GetFileName(filePathBusinessCustomer));
            var uploadFileForBusiness = fileController.UploadFile(bussinessCustomerFile, "invest");
            if (uploadFileForBusiness.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForBusiness.Message);
            }

            var addContractTemplateTemp = contractTemplateTempController.Add(new CreateInvestContractTemplateTempDto
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
            var insertContractTemplateTemp = JsonSerializer.Deserialize<InvestContractTemplateTempDto>(JsonSerializer.Serialize(addContractTemplateTemp.Data));
            dbContext.InvestContractTemplateTemps.Remove(dbContext.InvestContractTemplateTemps.FirstOrDefault(c => c.Id == insertContractTemplateTemp.Id));

            // Thêm chính sách mẫu
            var addPolicyTemp = policyTempController.Add(new CreatePolicyTempDto
            {
                Name = $"Chính sách xUNIT {DateTime.Now:HH-mm} Ngày {DateTime.Now:dd-MM-yyyy}",
                Code = $"xUnit {DateTime.Now:yyyyMMddHHmmss}",
                CalculateType = CalculateTypes.GROSS,
                Classify = InvestPolicyClassify.FLEX,
                Description = " xUnit Test Thêm chính sách",
                ExitFee = 2,
                ExitFeeType = ExitFeeTypes.RUT_THEO_SO_TIEN,
                ExpirationRenewals = ContractExpireTypes.DAO_HAN,
                IncomeTax = 5,
                IsTransfer = YesNo.YES,
                MaxWithDraw = 100,
                MinInvestDay = 5,
                MinMoney = 100000,
                MinTakeContract = 2,
                MinWithdraw = 100000,
                PolicyDisplayOrder = PolicyDisplayOrder.SHORT_TO_LONG,
                RemindRenewals = 1,
                RenewalsType = 1,
                TransferTax = 1,
                Type = InvPolicyType.FLEXIBLE,
                PolicyDetailTemp = new List<CreatePolicyDetailTempDto>
                {
                    new CreatePolicyDetailTempDto
                    {
                        Stt = 1,
                        Name = "3 Tháng",
                        ShortName = "xUnit3MM",
                        InterestPeriodQuantity = 3,
                        InterestPeriodType = PeriodType.THANG,
                        InterestType = InterestTypes.DINH_KY,
                        PeriodType = PeriodType.THANG,
                        PeriodQuantity = 3,
                        Profit = 10,
                        FixedPaymentDate = 5
                    }
                },
            });
            if (addPolicyTemp.Status != StatusCode.Success)
            {
                Assert.Fail(addPolicyTemp.Message);
            }
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Thêm các thông tin của phân phối
        /// </summary>
        /// <param name="mockRequestTrading"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        private int AddDistribution(IHost mockRequestTrading, EpicSchemaDbContext dbContext)
        {
            ProjectController projectController = new(mockRequestTrading.GetService<ILogger<ProjectController>>(),
                mockRequestTrading.GetService<IProjectServices>());
            DistributionController distributionController = new(mockRequestTrading.GetService<ILogger<DistributionController>>(),
                mockRequestTrading.GetService<IDistributionServices>(), mockRequestTrading.GetService<IConfigContractCodeServices>());
            InvestContractTemplateTempController contractTemplateTempController = new(mockRequestTrading.GetService<ILogger<InvestContractTemplateTempController>>(),
               mockRequestTrading.GetService<IInvestContractTemplateTempServices>());
            ContractTemplateController contractTemplateController = new(mockRequestTrading.GetService<ILogger<ContractTemplateController>>(),
               mockRequestTrading.GetService<IContractTemplateServices>());
            PolicyTempController policyTempController = new(mockRequestTrading.GetService<ILogger<PolicyTempController>>(),
               mockRequestTrading.GetService<IPolicyTempServices>());
            FileController fileController = new(mockRequestTrading.GetService<IFileServices>());

            // Dự án có thể phân phối cho đại lý
            string keywordProject = "xUnit";
            var projectCanDistribution = projectController.GetAllList(0, -1, keywordProject, DistributionStatus.HOAT_DONG);
            if (projectCanDistribution.Status != StatusCode.Success)
            {
                Assert.Fail(projectCanDistribution.Message);
            }
            var projectCanDistributionData = JsonSerializer.Deserialize<PagingResult<ProjectDto>>(JsonSerializer.Serialize(projectCanDistribution.Data));
            if (projectCanDistributionData.Items.Count() == 0) Assert.Fail("Không tìm thấy dự án phân phối xUnit cho đại lý");

            var projectFirst = projectCanDistributionData.Items.FirstOrDefault();

            // Danh sách tài khoảng ngân hàng của đại lý
            var listBankOfTradingProvider = distributionController.FindBankByTradingInvest(null, null);
            if (listBankOfTradingProvider.Status != StatusCode.Success)
            {
                Assert.Fail(listBankOfTradingProvider.Message);
            }
            var listBankOfTradingProviderData = JsonSerializer.Deserialize<List<BusinessCustomerBankDto>>(JsonSerializer.Serialize(listBankOfTradingProvider.Data));
            if (listBankOfTradingProviderData.Count() == 0) Assert.Fail("Không tìm thấy ngân hàng của đại lý");

            // Thêm phân phối cho đại lý
            var insertDistribution = distributionController.AddDistribution(new CreateDistributionDto
            {
                ProjectId = projectFirst.Id,
                CloseCellDate = DateTime.Now.AddYears(1),
                OpenCellDate = DateTime.Now,
                TradingBankAcc = listBankOfTradingProviderData.Select(x => (int?)x.BusinessCustomerBankAccId).ToList(),
                TradingBankAccPays = listBankOfTradingProviderData.Select(x => (int?)x.BusinessCustomerBankAccId).ToList(),
            });
            if (insertDistribution.Status != StatusCode.Success)
            {
                Assert.Fail(insertDistribution.Message);
            }
            var distributionData = JsonSerializer.Deserialize<Distribution>(JsonSerializer.Serialize(insertDistribution.Data));

            AddGeneralSettingData(mockRequestTrading, dbContext);

            #region Thêm chính sách vào phân phối
            // Lấy danh sách chính sách mẫu của đại lý đã cài trước đó
            var policyTempFind = policyTempController.FindAllPolicyTempNoPermission(Status.ACTIVE);
            if (policyTempFind.Status != StatusCode.Success)
            {
                Assert.Fail(policyTempFind.Message);
            }
            var polictTempData = JsonSerializer.Deserialize<PagingResult<ViewPolicyTemp>>(JsonSerializer.Serialize(policyTempFind.Data));
            if (polictTempData.Items.Count() == 0) Assert.Fail("Không tìm chính sách mẫu để cấu hình cho phân phối");

            // Lấy chính sách mẫu đầu tiên để tạo
            var policyTempFirst = polictTempData.Items.Where(e => e.PolicyDetailTemps.Count() > 0).OrderByDescending(x => x.Id).FirstOrDefault();
            if (policyTempFirst.PolicyDetailTemps.Count() == 0) Assert.Fail($"Không tìm thấy kỳ hạn trong chính sách mẫu: {policyTempFirst.Name}");

            // Thêm chính sách vào phân phối
            var insertPolicy = distributionController.AddPolicy(new CreatePolicySpecificDto
            {
                DistributionId = distributionData.Id,
                PolicyTempId = policyTempFirst.Id,
                Code = policyTempFirst.Code,
                Name = policyTempFirst.Name,
                Type = policyTempFirst.Type ?? 0,
                IncomeTax = policyTempFirst.IncomeTax ?? 0,
                TransferTax = policyTempFirst.TransferTax ?? 0,
                MinMoney = policyTempFirst.MinMoney ?? 0,
                IsTransfer = policyTempFirst.IsTransfer,
                Classify = (int)policyTempFirst.Classify,
                Description = policyTempFirst.Description,
                IsShowApp = YesNo.NO,
                MinWithDraw = policyTempFirst.MinWithdraw,
                CalculateType = (int)policyTempFirst.CalculateType,
                ExitFee = (int)policyTempFirst.ExitFee,
                ExitFeeType = (int)policyTempFirst.ExitFeeType,
                PolicyDisplayOrder = (int)policyTempFirst.PolicyDisplayOrder,
                RenewalsType = policyTempFirst.RenewalsType,
                RemindRenewals = policyTempFirst.RemindRenewals,
                ExpirationRenewals = policyTempFirst.ExpirationRenewals,
                MaxWithDraw = policyTempFirst.MaxWithDraw,
                MinTakeContract = policyTempFirst.MinTakeContract,
                MinInvestDay = policyTempFirst.MinInvestDay,
            });
            if (insertPolicy.Status != StatusCode.Success)
            {
                Assert.Fail(insertPolicy.Message);
            }
            var policyData = JsonSerializer.Deserialize<ViewPolicyDto>(JsonSerializer.Serialize(insertPolicy.Data));
            // Bật Show App cho Chính sách
            var openShowApp = distributionController.PolicyIsShowApp(policyData.Id);
            if (openShowApp.Status != StatusCode.Success)
            {
                Assert.Fail(openShowApp.Message);
            }
            #endregion

            #region Thêm mẫu hợp đồng
            // Lấy mẫu hợp đồng Temp với ContractSource = 3
            var contractTemplateTemp = contractTemplateTempController.GetAllContractTemplateTemp(ContractSources.ALL);
            if (contractTemplateTemp.Status != StatusCode.Success)
            {
                Assert.Fail(contractTemplateTemp.Message);
            }
            var contractTemplateTempData = JsonSerializer.Deserialize<List<InvestContractTemplateTempDto>>(JsonSerializer.Serialize(contractTemplateTemp.Data));
            var contractTemplateTempFirst = contractTemplateTempData.OrderByDescending(x => x.Id).Where(x => x.FileInvestor != null)?.FirstOrDefault();
            if (contractTemplateTempFirst == null) Assert.Fail($"Không tìm thấy mẫu hợp đồng tạm với kiểu Tất cả");

            #region Thêm mẫu hợp đồng mới, khi test xong thì xóa nên ko tồn tại file cứng
            // Thêm mẫu hợp đồng
            var filePathInvestor = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "investor-contract-order.docx");
            var investorFile = new FormFile(File.OpenRead(filePathInvestor), 0, File.OpenRead(filePathInvestor).Length, null, Path.GetFileName(filePathInvestor));
            var uploadFileForInvestor = fileController.UploadFile(investorFile, "invest");
            if (uploadFileForInvestor.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForInvestor.Message);
            }

            var filePathBusinessCustomer = Path.Combine(Environment.CurrentDirectory, "Data", "ContractTemplate", "business-customer-contract-order.docx");
            var bussinessCustomerFile = new FormFile(File.OpenRead(filePathBusinessCustomer), 0, File.OpenRead(filePathBusinessCustomer).Length, null, Path.GetFileName(filePathBusinessCustomer));
            var uploadFileForBusiness = fileController.UploadFile(bussinessCustomerFile, "invest");
            if (uploadFileForBusiness.Status != StatusCode.Success)
            {
                Assert.Fail(uploadFileForBusiness.Message);
            }

            var addContractTemplateTemp = contractTemplateTempController.Add(new CreateInvestContractTemplateTempDto
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
            var insertContractTemplateTemp = JsonSerializer.Deserialize<InvestContractTemplateTempDto>(JsonSerializer.Serialize(addContractTemplateTemp.Data));
            #endregion

            var policyFind = distributionController.FindPolicyByDistribution(distributionData.Id, Status.ACTIVE);
            if (policyFind.Status != StatusCode.Success)
            {
                Assert.Fail(policyFind.Message);
            }
            var policyFindData = JsonSerializer.Deserialize<IEnumerable<ViewPolicyDto>>(JsonSerializer.Serialize(policyFind.Data));
            if (policyFindData.Count() == 0) Assert.Fail($"Không tìm thấy chính sách Active trong phân phối");

            var configContractCode = distributionController.GetAllConfigContractCodeStatusActive();
            if (configContractCode.Status != StatusCode.Success)
            {
                Assert.Fail(configContractCode.Message);
            }
            var configContractCodeData = JsonSerializer.Deserialize<List<InvConfigContractCodeDto>>(JsonSerializer.Serialize(configContractCode.Data));
            if (configContractCodeData.Count() == 0) Assert.Fail($"Không tìm thấy cấu trúc mẫu hợp đồng");

            // displayType Loại hiển thị: B: Trước khi duyệt hợp đồng, A: Sau khi duyệt hợp đồng
            // contractSource: Kiểu hợp đồng: 3 All
            var insertContractTemplate = contractTemplateController.AddContractTemplate(new CreateContractTemplateDto
            {
                ContractSource = ContractSources.ALL,
                DisplayType = DisplayType.TRUOC_KHI_DUYET,
                ConfigContractId = configContractCodeData.OrderByDescending(x => x.Id).FirstOrDefault().Id,
                //ContractTemplateTempId = insertContractTemplateTemp.Id,
                StartDate = DateTime.Now.Date,
                PolicyIds = new List<int> { policyData.Id },
            });
            #endregion

            // Trình duyệt phân phối
            var requestApproveDistribution = distributionController.AddRequest(new RequestStatusDto
            {
                Id = distributionData.Id,
                Summary = "xUNIT -  Phân phối đầu tư  bán theo kỳ hạn",
            });
            // Phê duyệt phân phối
            var approveDistribution = distributionController.Approve(new InvestApproveDto
            {
                Id = distributionData.Id,
                ApproveNote = "xUNIT - Phê duyệt Phân phối đầu tư bán theo kỳ hạn",
            });
            // ROOT duyệt phân phối / Duyệt mới có thể hiển thị trên App
            var checkDistribution = distributionController.Check(new CheckStatusDto
            {
                Id = distributionData.Id,
            });
            _output.WriteLine($"Thêm phân phối thành công, Id: {distributionData.Id}");

            /*dbContext.InvestPolicies.Remove(dbContext.InvestPolicies.FirstOrDefault(p => p.Id == policyData.Id));
            dbContext.InvestDistributions.Remove(dbContext.InvestDistributions.FirstOrDefault(p => p.Id == distributionData.Id));
            dbContext.InvestPolicyDetails.RemoveRange(dbContext.InvestPolicyDetails.Where(p => p.PolicyId == policyData.Id));
            dbContext.InvestContractTemplates.Remove(dbContext.InvestContractTemplates.FirstOrDefault(p => p.PolicyId == policyData.Id));
            dbContext.InvestContractTemplateTemps.Remove(dbContext.InvestContractTemplateTemps.FirstOrDefault(c => c.Id == insertContractTemplateTemp.Id));
            dbContext.InvestApproves.Remove(dbContext.InvestApproves.FirstOrDefault(p => p.DataType == InvestApproveDataTypes.EP_INV_DISTRIBUTION && p.ReferId == distributionData.Id));
            dbContext.SaveChanges();

            _output.WriteLine($"Xóa phân phối Id: {distributionData.Id} thành công");*/
            return distributionData.Id;
        }

        /// <summary>
        /// Đại lý thêm hợp đồng cho khách
        /// </summary>
        [Theory]
        [InlineData(1808, 181)]
        public async void TradingAddOrder(int tradingUserId, int tradingProviderId)
        {
            var orderResult = new ViewOrderDto();
            var orderPaymentResult = new OrderPaymentDto();
            string investorPhone = "0987654321";
            var httpContext = CreateHttpContextTrading(tradingUserId, tradingProviderId);
            IHost mockInvestRequestTrading = GetHost<InvestAPI.Startup>(httpContext);
            IHost mockCoreRequestTrading = GetHost<CoreAPI.Startup>(httpContext);

            ManagerInvestorController managerInvestorController = new(mockCoreRequestTrading.GetService<ILogger<ManagerInvestorController>>(),
                mockCoreRequestTrading.GetService<IManagerInvestorServices>(), mockCoreRequestTrading.GetService<NotificationServices>(),
                mockCoreRequestTrading.GetService<IHttpContextAccessor>(), mockCoreRequestTrading.GetService<IInvestorV2Services>());

            DistributionController distributionController = new(mockInvestRequestTrading.GetService<ILogger<DistributionController>>(),
                mockInvestRequestTrading.GetService<IDistributionServices>(), mockInvestRequestTrading.GetService<IConfigContractCodeServices>());
            OrderController orderController = new(mockInvestRequestTrading.GetService<ILogger<OrderController>>(), mockInvestRequestTrading.GetService<IOrderServices>(),
                mockInvestRequestTrading.GetService<IInvestOrderContractFileServices>(), mockInvestRequestTrading.GetService<IConfiguration>(),
                mockInvestRequestTrading.GetService<IOptions<FileConfig>>(), mockInvestRequestTrading.GetService<InvestNotificationServices>());
            var dbContext = mockInvestRequestTrading.GetService<EpicSchemaDbContext>();

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
            var investorData = JsonSerializer.Deserialize<PagingResult<ViewManagerInvestorDto>>(JsonSerializer.Serialize(findInvestor.Data));
            if (investorData.Items.Count() == 0)
            {
                Assert.Fail($"Không tìm thấy thông tin nhà đầu tư bằng số điện thoại {investorPhone}");
            }
            var investor = investorData.Items.FirstOrDefault();

            var distributionNewId = AddDistribution(mockInvestRequestTrading, dbContext);
            // Tìm các phân phối để đặt hợp đồng
            var findListDistribution = distributionController.FindAllOrder(new FilterInvestDistributionDto
            {
                Status = DistributionStatus.HOAT_DONG
            });
            if (findListDistribution.Status != StatusCode.Success)
            {
                Assert.Fail(findListDistribution.Message);
            }
            var distributionData = JsonSerializer.Deserialize<List<ViewDistributionDto>>(JsonSerializer.Serialize(findListDistribution.Data));
            distributionData = distributionData.Where(x => x.Project.InvCode.Contains("xUnit") && x.Id == distributionNewId).OrderByDescending(x => x.Id).ToList();
            if (distributionData.Count() == 0) Assert.Fail("Không tìm thấy thông tin phân phối đặt lệnh");

            int distributionId = 0;
            int projectId = 0;
            int policyId = 0;
            int policyDetailId = 0;
            decimal minMoneyPolicy = 0;
            foreach (var item in distributionData)
            {
                if (item.HanMucToiDa <= 0) continue;

                distributionId = item.Id;
                projectId = item.ProjectId;
                if (item.Policies.Count == 0)
                {
                    _output.WriteLine($"Không tìm thấy thông tin chính sách của phân phối {item.InvName}");
                    continue;
                }
                var policyQuery = item.Policies.Where(x => x.Status == Status.ACTIVE);
                foreach (var policy in policyQuery)
                {
                    var contractTemplates = (from ct in dbContext.InvestContractTemplates
                                             join ctt in dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id
                                             where ct.PolicyId == policy.Id && ct.Deleted == YesNo.NO && ctt.ContractType == ContractTypes.DAT_LENH
                                             && (ctt.ContractSource == ContractSources.ONLINE || ctt.ContractSource == ContractSources.ALL)
                                             && ct.DisplayType == ContractTemplateDisplayType.BEFORE && ct.Status == Status.ACTIVE && ct.Deleted == YesNo.NO && ctt.Deleted == YesNo.NO
                                             select ct).Count();
                    if (contractTemplates == 0)
                    {
                        _output.WriteLine($"Chính sách {policy.Name} không tìm thấy mẫu hợp đồng dành cho nhà đầu tư cá nhân");
                        continue;
                    }

                    if (policy.PolicyDetail.Where(x => x.Status == Status.ACTIVE).Count() == 0)
                    {
                        _output.WriteLine($"Không tìm thấy thông tin kỳ hạn của chính sách {policy.Name}");
                        continue;
                    }
                    policyId = policy.Id;
                    minMoneyPolicy = policy.MinMoney ?? 0;
                    var policyDetail = policy.PolicyDetail.Where(x => x.Status == Status.ACTIVE).FirstOrDefault();
                    policyDetailId = policyDetail.Id;
                    _output.WriteLine($"Sử dụng chính sách {policy.Name} và kỳ hạn {policyDetail.Name}");
                    break;
                }
                if (policyDetailId != 0) break;
            }

            try
            {
                if (policyDetailId == 0) Assert.Fail("Không tìm thấy thông tin kỳ hạn hợp lệ để đặt lệnh");
                // Tạo hợp đồng
                var restAddOrder = orderController.AddOrder(new CreateOrderDto
                {
                    CifCode = investor.CifCode,
                    DistributionId = distributionId,
                    PolicyId = policyId,
                    PolicyDetailId = policyDetailId,
                    TotalValue = minMoneyPolicy,
                    InvestorBankAccId = investor.DefaultBank.Id,
                    IsInterest = YesNo.YES,
                    ProjectId = projectId,
                    ContractAddressId = investor.DefaultContactAddress?.ContactAddressId
                });
                if (restAddOrder.Status != StatusCode.Success)
                {
                    Assert.Fail(restAddOrder.Message);
                }
                var order = JsonSerializer.Deserialize<ViewOrderDto>(JsonSerializer.Serialize(restAddOrder.Data));
                orderResult = order;
                // Tạo OrderPayment
                var orderPayment = orderController.OrderPaymentAdd(new CreateOrderPaymentDto
                {
                    OrderId = order.Id,
                    PaymentAmnount = order.TotalValue,
                    TranDate = DateTime.Now,
                    TranClassify = TranClassifies.THANH_TOAN,
                    TranType = TranTypes.THU,
                    PaymentType = PaymentTypes.TIEN_MAT,
                    TradingBankAccId = order.BusinessCustomerBankAccId ?? 0,
                    Description = $"xUnit TT {order.ContractCode}",
                });
                if (orderPayment.Status != StatusCode.Success)
                {
                    Assert.Fail(orderPayment.Message);
                }
                var orderPaymentData = JsonSerializer.Deserialize<OrderPaymentDto>(JsonSerializer.Serialize(orderPayment.Data));
                orderPaymentResult = orderPaymentData;
                // Duyệt OrderPayment
                var approvePayment = await orderController.ApproveOrderPayment((int)orderPaymentData.Id, OrderPaymentStatus.DA_THANH_TOAN);
                if (approvePayment.Status != StatusCode.Success)
                {
                    Assert.Fail(approvePayment.Message);
                }

                // Duyệt Order
                var approveOrder = await orderController.ContractApprove((int)order.Id);
                if (approveOrder.Status != StatusCode.Success)
                {
                    Assert.Fail(approveOrder.Message);
                }
                _output.WriteLine($"Hợp đồng {order.ContractCode} Active thành công");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                var policyTemp = dbContext.InvestPolicyTemps.Where(pt => pt.Code.Contains("xUnit")).OrderByDescending(pt => pt.Id).FirstOrDefault();
                var policyDetailTemp = dbContext.InvestPolicyDetailTemps.Where(pt => pt.PolicyTempId == policyTemp.Id).OrderByDescending(pt => pt.Id).FirstOrDefault();
                dbContext.InvestPolicyTemps.Remove(policyTemp);
                dbContext.InvestPolicyDetailTemps.Remove(policyDetailTemp);
                var orderContractFile = dbContext.InvestOrderContractFile.Where(o => o.OrderId == orderResult.Id);
                if (orderContractFile.Any())
                {
                    dbContext.InvestOrderContractFile.RemoveRange(orderContractFile);
                }
                var orderRemove = dbContext.InvOrders.FirstOrDefault(o => o.Id == orderResult.Id);
                if (orderRemove != null)
                {
                    dbContext.InvOrders.Remove(orderRemove);
                }

                var paymentRemove = dbContext.InvestOrderPayments.FirstOrDefault(o => o.Id == (int)orderPaymentResult.Id);
                if (paymentRemove != null)
                {
                    dbContext.InvestOrderPayments.Remove(paymentRemove);
                }
                _output.WriteLine($"Xóa thông tin hợp đồng {orderResult.ContractCode} thành công");
                // Xóa thông tin phân phối vừa được tạo
                dbContext.InvestDistributions.Remove(dbContext.InvestDistributions.FirstOrDefault(p => p.Id == distributionId));
                dbContext.InvestPolicies.Remove(dbContext.InvestPolicies.FirstOrDefault(p => p.DistributionId == distributionId));
                dbContext.InvestPolicyDetails.RemoveRange(dbContext.InvestPolicyDetails.Where(p => p.DistributionId == distributionId));
                dbContext.InvestContractTemplates.Remove(dbContext.InvestContractTemplates.FirstOrDefault(p => dbContext.InvestPolicies.Any(pl => pl.DistributionId == distributionId && p.PolicyId == pl.Id)));
                dbContext.InvestContractTemplateTemps.Remove(dbContext.InvestContractTemplateTemps.FirstOrDefault(c => dbContext.InvestContractTemplates.Any(p => p.ContractTemplateTempId == c.Id &&
                                                                dbContext.InvestPolicies.Any(pl => pl.DistributionId == distributionId && p.PolicyId == pl.Id))));
                dbContext.InvestApproves.Remove(dbContext.InvestApproves.FirstOrDefault(p => p.DataType == InvestApproveDataTypes.EP_INV_DISTRIBUTION && p.ReferId == distributionId));
                dbContext.SaveChanges();
            }
            _output.WriteLine($"Xóa phân phối Id: {distributionId} thành công");
        }


        private void RunTaskHost()
        {
            IHost mockInvestRequestTrading = GetHost<InvestAPI.Startup>();
            IHost mockPayment = GetHost<PaymentAPI.Startup>();
            IHost mockShared = GetHost<SharedAPI.Startup>();
            Task.Run(() => mockInvestRequestTrading.Run());
            Task.Run(() => mockPayment.Run());
            Task.Run(() => mockShared.Run());
        }

        /// <summary>
        /// Test Investor thêm hợp đồng mà active tự động
        /// </summary>
        [Theory]
        [InlineData(4001, 3999)]
        public async void InvestorAdd(int userId, int investorId)
        {
            var httpContext = CreateHttpContextInvestor(userId, investorId);
            IHost mockInvestRequestTrading = GetHost<InvestAPI.Startup>(httpContext);
            IHost mockIdentityRequestTrading = GetHost<IdentityServer.Startup>(httpContext);
            IHost mockPayment = GetHost<PaymentAPI.Startup>(httpContext);
            IHost mockGarner = GetHost<GarnerAPI.Startup>(httpContext);

            // DL_EMIR
            IHost mockInvestTradingProvider = GetHost<InvestAPI.Startup>(CreateHttpContextTrading(1808, 181));

            RunTaskHost();

            MsbPaymentController paymentController = new(mockPayment.GetService<ILogger<MsbPaymentController>>(), mockPayment.GetService<IMsbPaymentServices>());

            UserInvestorController userInvestorController = new(mockIdentityRequestTrading.GetService<ILogger<UserInvestorController>>(),
                mockIdentityRequestTrading.GetService<IUserServices>(), mockIdentityRequestTrading.GetService<IInvestorServices>(),
                mockIdentityRequestTrading.GetService<NotificationServices>(), mockIdentityRequestTrading.GetService<IHttpContextAccessor>());

            AppInvestorProjectController appInvestorProjectController = new(mockInvestRequestTrading.GetService<ILogger<AppInvestorProjectController>>(),
                mockInvestRequestTrading.GetService<IDistributionServices>(), mockInvestRequestTrading.GetService<IInvestSharedServices>());

            AppInvestorOrderController appInvestorOrderController = new(mockInvestRequestTrading.GetService<ILogger<AppInvestorOrderController>>(),
                mockInvestRequestTrading.GetService<IOrderServices>(), mockInvestRequestTrading.GetService<IInvestSharedServices>(),
                mockInvestRequestTrading.GetService<IContractTemplateServices>(), mockInvestRequestTrading.GetService<IWithdrawalServices>(),
                mockInvestRequestTrading.GetService<IInvestRenewalsRequestServices>(), mockInvestRequestTrading.GetService<IContractDataServices>(),
                mockInvestRequestTrading.GetService<IConfiguration>(), mockInvestRequestTrading.GetService<IInvestRatingServices>());
            var dbContext = mockInvestRequestTrading.GetService<EpicSchemaDbContext>();


            // Lấy thông tin investor
            var investor = dbContext.Investors.FirstOrDefault(x => x.InvestorId == investorId && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO);
            var investorIdentity = dbContext.InvestorIdentifications.Where(x => x.InvestorId == investor.InvestorId && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO)
                                    .OrderByDescending(x => x.IsDefault).ThenBy(x => x.Id).FirstOrDefault();
            if (investorIdentity == null)
            {
                Assert.Fail("Không tìm thấy thông tin giấy tờ nhà đầu tư");
            }
            var investorBank = dbContext.InvestorBankAccounts.Where(x => x.InvestorId == investor.InvestorId && x.Deleted == YesNo.NO)
                                    .OrderByDescending(x => x.IsDefault).ThenBy(x => x.Id).FirstOrDefault();
            if (investorBank == null)
            {
                Assert.Fail("Không tìm thấy thông tin ngân hàng nhà đầu tư");
            }
            var investorAddress = dbContext.InvestorContactAddresses.Where(x => x.InvestorId == investor.InvestorId && x.Deleted == YesNo.NO)
                                    .OrderByDescending(x => x.IsDefault).ThenBy(x => x.ContactAddressId).FirstOrDefault();

            // Thêm phân phối cho đại lý
            var distributionNewId = AddDistribution(mockInvestTradingProvider, dbContext);

            string keyword = "xUnit";
            // Tìm kiếm trong bảng hàng
            var findDistribution = appInvestorProjectController.FindAllProjectDistribution(new AppFilterProjectDistribution
            {
                Keyword = keyword,
            });
            if (findDistribution.Status != StatusCode.Success)
            {
                Assert.Fail(findDistribution.Message);
            }

            var distributionData = JsonSerializer.Deserialize<List<ProjectDistributionDto>>(JsonSerializer.Serialize(findDistribution.Data));
            distributionData = distributionData.Where(d => d.DistributionId == distributionNewId).ToList();
            if (distributionData.Count() == 0)
            {
                Assert.Fail("Không tìm thấy bảng hàng đầu tư của khách hàng cá nhân");
            }
            var distributionDataFirst = distributionData.FirstOrDefault();

            // Lấy chính sách
            var findPolicy = appInvestorProjectController.FindAllPolicy(distributionDataFirst.DistributionId);
            if (findPolicy.Status != StatusCode.Success)
            {
                Assert.Fail(findPolicy.Message);

            }
            var policyData = JsonSerializer.Deserialize<List<AppInvestPolicyDto>>(JsonSerializer.Serialize(findPolicy.Data));
            var policyFirst = policyData.FirstOrDefault();

            // Kiểm tra xem chính sách có mẫu hợp đồng để đặt lệnh
            int contractTemplates = 0;
            foreach (var item in policyData)
            {
                policyFirst = item;
                int policyId = policyFirst.Id;
                // Mau hop dong
                contractTemplates = (from ct in dbContext.InvestContractTemplates
                                     join ctt in dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id
                                     where ct.PolicyId == policyId && ct.Deleted == YesNo.NO && ctt.ContractType == ContractTypes.DAT_LENH
                                     && (ctt.ContractSource == ContractSources.ONLINE || ctt.ContractSource == ContractSources.ALL)
                                     && ct.DisplayType == ContractTemplateDisplayType.BEFORE && ct.Status == Status.ACTIVE && ct.Deleted == YesNo.NO && ctt.Deleted == YesNo.NO
                                     select ct).Count();
                if (contractTemplates == 0)
                {
                    _output.WriteLine($"Chính sách {policyFirst.Name} không tìm thấy mẫu hợp đồng dành cho nhà đầu tư cá nhân");
                }
                if (contractTemplates > 0) break;
            }


            var orderResult = new AppOrderDto();
            var msbNoti = new MsbNotification();
            string tranSeqResult = "";
            try
            {
                // Lấy kỳ hạn
                var findPolicyDetail = appInvestorProjectController.FindAllPolicyDetail(policyFirst.Id, policyFirst.MinMoney ?? 0);
                if (findPolicyDetail.Status != StatusCode.Success)
                {
                    Assert.Fail(findDistribution.Message);
                }
                var policyDetailData = JsonSerializer.Deserialize<List<AppInvestPolicyDetailDto>>(JsonSerializer.Serialize(findPolicyDetail.Data));

                var policyDetailFirst = policyDetailData.FirstOrDefault();

                // Kiểm tra trước khi tạo hợp đồng
                var checkOrder = appInvestorOrderController.CheckOrder(new AppCheckOrderDto
                {
                    PolicyDetailId = policyDetailFirst.Id,
                    TotalValue = policyFirst.MinMoney,
                    BankAccId = investorBank.Id,
                    IdentificationId = investorIdentity.Id,
                    IsReceiveContract = false,
                    //TranAddess = investorAddress?.ContactAddressId
                });
                if (checkOrder.Status != StatusCode.Success)
                {
                    Assert.Fail(checkOrder.Message);
                }

                // Request Otp
                await userInvestorController.GenerateOtpMail();
                // Tạo hợp đồng
                var restAddOrder = await appInvestorOrderController.OrderAdd(new AppCreateOrderDto
                {
                    PolicyDetailId = policyDetailFirst.Id,
                    TotalValue = policyFirst.MinMoney,
                    BankAccId = investorBank.Id,
                    IdentificationId = investorIdentity.Id,
                    IsReceiveContract = false,
                    //TranAddess = investorAddress?.ContactAddressId,
                    OTP = "625489"
                });
                if (restAddOrder.Status != StatusCode.Success)
                {
                    Assert.Fail(restAddOrder.Message);
                }
                var insertOrder = JsonSerializer.Deserialize<AppOrderDto>(JsonSerializer.Serialize(restAddOrder.Data));
                orderResult = insertOrder;
                _output.WriteLine($"Hợp đồng {insertOrder.ContractCode} được tạo thành công");
                int tryCount = 5;
                while (true)
                {
                    try
                    {
                        await Task.Delay(6000);
                        // Kiểm tra sinh đủ số lượng hợp đồng không, delay 6s chờ backgroundjob
                        var totalContactOrder = dbContext.InvestOrderContractFile.Where(o => o.OrderId == insertOrder.Id && o.Deleted == YesNo.NO).Count();
                        if (contractTemplates != totalContactOrder)
                        {
                            Assert.Fail("Hợp đồng chưa sinh đủ số file");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (--tryCount == 0)
                            Assert.Fail(ex.Message);
                    }
                };

                // Thanh toán tự động giả lập noti bank gửi về
                string tranSeq = $"{DateTime.Now:yyMMddHHmmss}VASML247";
                tranSeqResult = tranSeq;
                var notiPayment = new ReceiveNotificationDto
                {
                    TranSeq = tranSeq,
                    VaCode = "968668",
                    VaNumber = $"968668868EI{insertOrder.Id}",
                    FromAccountName = "nguyen van test unit",
                    FromAccountNumber = "68686868",
                    ToAccountName = "CONG TY CO PHAN TEST XUNIT",
                    ToAccountNumber = "6868686868",
                    TranAmount = insertOrder.TotalValue?.ToString(),
                    TranRemark = $"xUNIT-TEST {insertOrder.ContractCode} EI{insertOrder.Id}",
                    TranDate = $"{DateTime.Now:yyMMddHHmmss}",
                    Signature = "unitTest"
                };
                notiPayment.Signature = CryptographyUtils.ComputeSha256Hash("Ep1c@2022", notiPayment.TranSeq, notiPayment.TranDate, notiPayment.VaNumber, notiPayment.TranAmount, notiPayment.FromAccountNumber, notiPayment.ToAccountNumber);
                var approveOrder = paymentController.ReceiveNotification(notiPayment);

                if (restAddOrder.Status != StatusCode.Success)
                {
                    Assert.Fail(restAddOrder.Message);
                }

                var checkPaymentOrderAuto = dbContext.MsbNotifications.FirstOrDefault(p => p.TranSeq == tranSeq);
                msbNoti = checkPaymentOrderAuto;
                // Kiểm tra xem có exception từ hệ thống khi nhận noti không
                if (checkPaymentOrderAuto.Exception != null)
                {
                    Assert.Fail(checkPaymentOrderAuto.Exception);
                }

                int tryCountPayment = 5;
                while (true)
                {
                    try
                    {
                        await Task.Delay(6000);
                        // Kiểm tra hợp đồng sau khi active thành công
                        var orderFind = dbContext.InvOrders.FirstOrDefault(o => o.Id == insertOrder.Id);
                        if (orderFind == null) Assert.Fail("Không tìm thấy hợp đồng");
                        if (orderFind.Status != OrderStatus.DANG_DAU_TU && orderFind.Status != OrderStatus.CHO_DUYET_HOP_DONG)
                        {
                            Assert.Fail("Hợp đồng chưa active thành công");
                        }
                        // Kiểm tra xem các file đã ký hết chưa
                        if (dbContext.InvestOrderContractFile.Any(o => o.OrderId == insertOrder.Id && o.IsSign == YesNo.NO && o.Deleted == YesNo.NO))
                        {
                            Assert.Fail(" Tồn tại file hợp đồng chưa được ký điện tử");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (--tryCountPayment == 0)
                            Assert.Fail(ex.Message);
                    }
                };
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                _output.WriteLine($"Hợp đồng {orderResult.ContractCode} Active thành công");
                var orderContractFile = dbContext.InvestOrderContractFile.Where(o => o.OrderId == orderResult.Id);
                if (orderContractFile.Any())
                {
                    dbContext.InvestOrderContractFile.RemoveRange(orderContractFile);
                }

                var orderRemove = dbContext.InvOrders.FirstOrDefault(o => o.Id == orderResult.Id);
                if (orderRemove != null)
                {
                    dbContext.InvOrders.Remove(orderRemove);
                }

                var paymentRemove = dbContext.InvestOrderPayments.FirstOrDefault(o => o.OrderId == orderResult.Id);
                if (paymentRemove != null)
                {
                    dbContext.InvestOrderPayments.Remove(paymentRemove);
                }

                var tranSeqMsbNotification = dbContext.MsbNotifications.FirstOrDefault(o => o.TranSeq == tranSeqResult);
                if (tranSeqMsbNotification != null)
                {
                    dbContext.MsbNotifications.Remove(tranSeqMsbNotification);
                }

                // Xóa thông tin phân phối vừa được tạo
                dbContext.InvestDistributions.Remove(dbContext.InvestDistributions.FirstOrDefault(p => p.Id == distributionNewId));
                dbContext.InvestPolicies.Remove(dbContext.InvestPolicies.FirstOrDefault(p => p.DistributionId == distributionNewId));
                dbContext.InvestPolicyDetails.RemoveRange(dbContext.InvestPolicyDetails.Where(p => p.DistributionId == distributionNewId));
                dbContext.InvestContractTemplates.Remove(dbContext.InvestContractTemplates.FirstOrDefault(p => dbContext.InvestPolicies.Any(pl => pl.DistributionId == distributionNewId && p.PolicyId == pl.Id)));
                dbContext.InvestContractTemplateTemps.Remove(dbContext.InvestContractTemplateTemps.FirstOrDefault(c => dbContext.InvestContractTemplates.Any(p => p.ContractTemplateTempId == c.Id &&
                                                                dbContext.InvestPolicies.Any(pl => pl.DistributionId == distributionNewId && p.PolicyId == pl.Id))));
                dbContext.InvestApproves.Remove(dbContext.InvestApproves.FirstOrDefault(p => p.DataType == InvestApproveDataTypes.EP_INV_DISTRIBUTION && p.ReferId == distributionNewId));

                dbContext.SaveChanges();
                _output.WriteLine($"Xóa thông tin hợp đồng {orderResult.ContractCode} thành công");
            }
        }
    }
}

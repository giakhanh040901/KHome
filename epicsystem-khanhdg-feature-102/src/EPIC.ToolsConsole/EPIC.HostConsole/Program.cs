using EPIC.BondRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.MapperProfiles;
using EPIC.GarnerSharedDomain.Implements;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.HostConsole.Services;
using EPIC.InvestDomain.Implements;
using EPIC.MSB.Configs;
using EPIC.MSB.Dto.CollectMoney;
using EPIC.MSB.Dto.PayMoney;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.SharedEntities.MapperProfiles;
using EPIC.Utils;
using EPIC.Utils.Security;
using EPIC.Utils.SharedApiService;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.HostConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var hash = CryptographyUtils.ComputeSha256Hash("abc");

            var aes = CryptographyUtils.CreateAES();

            var encryStr = CryptographyUtils.EncryptString_Aes("Xin chào các bạn", aes.Item1, aes.Item2);
            var decyStr = CryptographyUtils.DecryptString_Aes(encryStr, aes.Item1, aes.Item2);

            IHost host = CreateHostBuilder(args).Build();
            var dbOptions = host.Services.GetService(typeof(DatabaseOptions)) as DatabaseOptions;
            var logger = host.Services.GetService(typeof(ILogger<Program>)) as ILogger;

            var dbContext = host.Services.GetService(typeof(EpicSchemaDbContext)) as EpicSchemaDbContext;
            var msbCollectMoneyServices = host.Services.GetService(typeof(MsbCollectMoneyServices)) as MsbCollectMoneyServices;
            var msbPayMoneyServices = host.Services.GetService(typeof(MsbPayMoneyServices)) as MsbPayMoneyServices;
            //var sendEmailService = host.Services.GetService(typeof(GarnerNotificationServices)) as GarnerNotificationServices;
            var msbInquiryServices = host.Services.GetService(typeof(MsbInquiryServices)) as MsbInquiryServices;

            //await msbInquiryServices.InquiryAccount(new MSB.Dto.Inquiry.InquiryAccountDto
            //{
            //    BankAccount = "11001018956459",
            //    Bin = "970426"
            //});

            var bondPrimaryRepository = new BondPrimaryRepository(dbOptions.ConnectionString, logger);

            //var transaction = bondPrimaryRepository.BeginTransaction();
            //bondPrimaryRepository.Add(new BondPrimary
            //{
            //    PartnerId = 342,
            //    BondId = 413004,
            //    TradingProviderId = 181,
            //    BusinessCustomerBankAccId = 949,
            //    Code = "Test",
            //    Name = "Test",
            //    ContractCode = "code",
            //    BondTypeId = 1,
            //    OpenCellDate = DateTime.Now,
            //    CloseCellDate = DateTime.Now,
            //    Quantity = 1,
            //    MinMoney = 1,
            //    PriceType = 1,
            //    MaxInvestor = 1,
            //    CreatedBy = "admin"
            //}, false);
            //transaction.Commit();

            /*
            bank ngoài
            "receiveAccount": "0129837294",
            "receiveName": "nguyen van test",
            "receiveBank": "970406"
             
            msb
            "receiveAccount": "11001018956459",
            "receiveName": "nguyen van test",
            "receiveBank": "970426",
            */
            //{"transId":"54","transDate":"20221208154323","tId":"R0000102","mId":"M00000082","note":"test chi","senderName":"","receiveName":"nguyen van test","senderAccount":null,"receiveAccount":"0129837294","receiveBank":"970406","amount":"1000","fee":"0","secureHash":"420661e31bf32130befc8ea91f7eca06a02d5b8a703e1fe39fab256f94c639cf","status":"SUCCESS","napasTransId":"134799","rrn":"CH08154327148"}
            long testId = 90;
            long testDetailId = testId;
            //var result = await msbPayMoneyServices.RequestPayMoney(new MsbRequestPayMoneyDto()
            //{
            //    RequestId = testId,
            //    PrefixAccount = "968668868",
            //    TId = "R0000102",
            //    MId = "M00000082",
            //    AccessCode = "Ep1c@2022",
            //    Details = new List<MsbRequestPayMoneyItemDto>
            //    {
            //        //bank liên kết
            //        new MsbRequestPayMoneyItemDto
            //        {
            //            DetailId = testDetailId,
            //            BankAccount = "0129837294",
            //            OwnerAccount = "nguyen van test",
            //            ReceiveBankBin = "970406", //970406
            //            AmountMoney = 1000,
            //            Note = "test chi",
            //        },
            //    }
            //});

            //Receive account name not correct : tên chủ tài khoản, số tài khoản hoặc mã BIN không chính xác
            //

            //if (result.ErrorDetails.Count > 0)
            //{
            //    //return;
            //}

            //giả sử gửi lại otp
            //await msbPayMoneyServices.TransferProcess(testId);
            //await msbPayMoneyServices.TransferProcessOtp(new ProcessRequestPayDto
            //{
            //    RequestId = testId,
            //    Otp = "456789",
            //    TId = "R0000102",
            //    MId = "M00000082",
            //    AccessCode = "Ep1c@2022"
            //});
            //nếu thành công thì lưu lại

            //test truy vấn
            //var inquiryBatch = await msbPayMoneyServices.InquiryBatch(new InquiryBatchDto
            //{
            //    RequestId = testId,
            //    TId = "R0000102",
            //    MId = "M00000082",
            //    AccessCode = "Ep1c@2022",
            //});

            var import = host.Services.GetService(typeof(ImportInvestorServices)) as ImportInvestorServices;
            await import.ImportInvestorExcel();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //nếu có cấu hình redis
                    string redisConnectionString = hostContext.Configuration.GetConnectionString("Redis");

                    string connectionString = hostContext.Configuration.GetConnectionString("EPIC");
                    services.AddSingleton(new DatabaseOptions { ConnectionString = connectionString });

                    // Add Hangfire services.
                    services.AddHangfire(configuration =>
                    {
                        configuration.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        configuration
                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseSerializerSettings(new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
                        configuration.UseInMemoryStorage();
                        // Add the processing server as IHostedService
                        services.AddHangfireServer((service, options) =>
                        {
                            options.ServerName = $"CONSOLE";
                            options.WorkerCount = 100;
                        });
                    });

                    //entity framework
                    services.AddDbContext<EpicSchemaDbContext>(options =>
                    {
                        options.UseOracle(connectionString, option => option.MigrationsAssembly("EPIC.HostConsole"));
                        options.ConfigureWarnings(b => b.Ignore(OracleEventId.DecimalTypeDefaultWarning));
                    }, ServiceLifetime.Scoped);

                    services.AddHttpContextAccessor();
                    services.AddSingleton<IWebHostEnvironment, HostEnvironment>();
                    services.Configure<MsbConfiguration>(hostContext.Configuration.GetSection("Msb"));
                    services.AddScoped<MsbCollectMoneyServices>();
                    services.AddScoped<ImportInvestorServices>();
                });
    }

    public class HostEnvironment : IWebHostEnvironment
    {
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string EnvironmentName { get; set; } = "Development";
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}

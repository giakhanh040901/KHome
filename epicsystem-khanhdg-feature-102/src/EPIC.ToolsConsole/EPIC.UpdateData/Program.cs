using EPIC.UpdateData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Oracle.EntityFrameworkCore.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EPIC.UpdateData
{
    internal class Program
    {
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

                //entity framework
                services.AddDbContext<ModelContext>(options =>
                {
                    options.UseOracle(connectionString, option => option.MigrationsAssembly("EPIC.UpdateData"));
                }, ServiceLifetime.Transient);
            });

        public static decimal NextKey(DbContext dbContext, string seqName)
        {
            if (seqName == null)
            {
                throw new Exception($"Chưa cấu hình sequence");
            }

            OracleParameter[] @params =
            {
                new OracleParameter("SEQ_NAME", OracleDbType.Varchar2) { Direction = ParameterDirection.Input, Value = seqName },
                new OracleParameter("SEQ_OUT", OracleDbType.Decimal) { Direction = ParameterDirection.Output }
            };
            dbContext.Database.ExecuteSqlRaw("BEGIN GET_SEQ(:SEQ_NAME, :SEQ_OUT); END;", @params);
            var result = @params[1].Value;
            decimal valueResult = decimal.Parse(result.ToString());
            return valueResult;
        }

        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            var _dbContext = host.Services.GetService(typeof(ModelContext)) as ModelContext;

            var transaction = _dbContext.Database.BeginTransaction();

            /*
            CREATE SEQUENCE EPIC.SEQ_INVEST_CONTRACT_TEMPLATE_TEMP INCREMENT BY 1 MINVALUE 1;
            DROP SEQUENCE EPIC.SEQ_INVEST_CONTRACT_TEMPLATE;
            CREATE SEQUENCE EPIC.SEQ_INVEST_CONTRACT_TEMPLATE INCREMENT BY 1 MINVALUE 1;
            DROP SEQUENCE SEQ_INVEST_CONFIG_CONTRACT_CODE;
            CREATE SEQUENCE EPIC.SEQ_INVEST_CONFIG_CONTRACT_CODE INCREMENT BY 1 MINVALUE 1;
            DROP SEQUENCE SEQ_INVEST_CONFIG_CONTRACT_CODE_DETAIL;
            CREATE SEQUENCE EPIC.SEQ_INVEST_CONFIG_CONTRACT_CODE_DETAIL INCREMENT BY 1 MINVALUE 1;
            */

            var tradingProviderIds = _dbContext.EpInvDistributions.Select(d => d.TradingProviderId).Distinct().ToList();
            List<EpInvConfigContractCode> epInvConfigContractCodeList = new();
            foreach (var tradingProviderId in tradingProviderIds)
            {
                var config = new EpInvConfigContractCode
                {
                    Id = (int)NextKey(_dbContext, "SEQ_INVEST_CONFIG_CONTRACT_CODE"),
                    TradingProviderId = (int)tradingProviderId,
                    Name = "Mã hệ thống EIxxx",
                    Description = "",
                    Status = "A",
                };
                epInvConfigContractCodeList.Add(config);
                _dbContext.EpInvConfigContractCodes.Add(config);

                _dbContext.EpInvConfigContractCodeDetails.Add(new EpInvConfigContractCodeDetail
                {
                    Id = (int)NextKey(_dbContext, "SEQ_INVEST_CONFIG_CONTRACT_CODE_DETAIL"),
                    ConfigContractCodeId = config.Id,
                    SortOrder = 1,
                    Key = "FIX_TEXT",
                    Value = null
                });

                _dbContext.EpInvConfigContractCodeDetails.Add(new EpInvConfigContractCodeDetail
                {
                    Id = (int)NextKey(_dbContext, "SEQ_INVEST_CONFIG_CONTRACT_CODE_DETAIL"),
                    ConfigContractCodeId = config.Id,
                    SortOrder = 2,
                    Key = "ORDER_ID_PREFIX_0",
                    Value = null
                });
            }

            var distributions = _dbContext.EpInvDistributions.ToList();
            foreach (var distribution in distributions)
            {
                int tradingProviderId = (int)(distribution.TradingProviderId ?? 0);
                int configContractCodeId = epInvConfigContractCodeList.FirstOrDefault(o => o.TradingProviderId == tradingProviderId).Id;
                var contractTemplates = _dbContext.EpInvContractTemplates.Where(o => o.DistributionId == distribution.Id).ToList();

                List<(EpInvContractTemplate1, decimal)> EpInvContractTemplate1List = new();
                foreach (var contractTemplate in contractTemplates)
                {
                    var insertTempOn = new EpInvContractTemplateTemp
                    {
                        Id = (int)NextKey(_dbContext, "SEQ_INVEST_CONTRACT_TEMPLATE_TEMP"),
                        Name = contractTemplate.Name + "",
                        TradingProviderId = tradingProviderId,
                        ContractType = 1,
                        Status = contractTemplate.Status,
                        ContractSource = 3,  //cả online offline
                        FileInvestor = contractTemplate.ContractTempUrl,
                        FileBusinessCustomer = contractTemplate.ContractTempUrl,
                        CreatedBy = contractTemplate.CreatedBy,
                        CreatedDate = contractTemplate.CreatedDate,
                        ModifiedBy = contractTemplate.ModifiedBy,
                        ModifiedDate = contractTemplate.ModifiedDate,
                        Deleted = contractTemplate.Deleted,
                    };
                    _dbContext.EpInvContractTemplateTemps.Add(insertTempOn);

                    //danh sách chính sách
                    var policies = _dbContext.EpInvPolicies.Where(p => p.DistributionId == contractTemplate.DistributionId).ToList();
                    foreach (var policy in policies)
                    {
                        var invContractTemp1 = new EpInvContractTemplate1
                        {
                            Id = (int)NextKey(_dbContext, "SEQ_INVEST_CONTRACT_TEMPLATE"),
                            PolicyId = (int)policy.Id,
                            TradingProviderId = tradingProviderId,
                            ConfigContractId = configContractCodeId,
                            ContractSource = 3, //cả online offline
                            Status = contractTemplate.Status,
                            ContractTemplateTempId = insertTempOn.Id, //lấy mẫu bên trên
                            CreatedBy = contractTemplate.CreatedBy,
                            CreatedDate = contractTemplate.CreatedDate,
                            ModifiedBy = contractTemplate.ModifiedBy,
                            ModifiedDate = contractTemplate.ModifiedDate,
                            StartDate = contractTemplate.CreatedDate ?? new DateTime(2022, 1, 1),
                            DisplayType = contractTemplate.DisplayType
                        };
                        EpInvContractTemplate1List.Add(new(invContractTemp1, contractTemplate.Id)); // invContractTemp1 - ContractTemplateId cũ
                        _dbContext.EpInvContractTemplate1s.Add(invContractTemp1);
                    }
                }
                //_dbContext.SaveChanges();

                //xử lý đổi sang cho lệnh
                //lệnh của chính sách nào thì đổi sang của chính sách đó
                var orders = _dbContext.EpInvOrders.AsNoTracking().Where(o => o.TradingProviderId == tradingProviderId && o.DistributionId == distribution.Id && o.Deleted == "N").ToList();
                foreach (var order in orders)
                {
                    var contractFiles = _dbContext.EpInvOrderContractFiles.AsNoTracking().Where(c => c.OrderId == order.Id && c.Deleted == "N").ToList();
                    foreach (var contractFile in contractFiles)
                    {
                        var invContractTemp1s = EpInvContractTemplate1List.Where(o => o.Item1.PolicyId == order.PolicyId && o.Item2 == contractFile.ContractTempId);
                        if (invContractTemp1s.Count() > 1)
                        {

                        }
                        var invContractTemp1 = invContractTemp1s.FirstOrDefault().Item1;
                        _dbContext.EpInvOrderContractFile1s.Add(new EpInvOrderContractFile1
                        {
                            Id = contractFile.Id,
                            TradingProviderId = tradingProviderId,
                            OrderId = contractFile.OrderId,
                            ContractTempId = invContractTemp1.Id,
                            FileTempUrl = contractFile.FileTempUrl,
                            FileScanUrl = contractFile.FileScanUrl,
                            FileSignatureUrl = contractFile.FileSignatureUrl,
                            FileTempPdfUrl = contractFile.FileTempPdfUrl,
                            IsSign = contractFile.IsSign,
                            Deleted = contractFile.Deleted,
                            PageSign = contractFile.PageSign,
                            CreatedBy = contractFile.CreatedBy,
                            CreatedDate = contractFile.CreatedDate,
                            ModifiedBy = contractFile.ModifiedBy,
                            ModifiedDate = contractFile.ModifiedDate,
                        });
                    }
                }
            }
            
            _dbContext.SaveChanges();
            transaction.Commit();
        }
    }
}

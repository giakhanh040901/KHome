using ClosedXML.Excel;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateRepositories;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using EPIC.Entities;
using EPIC.DataAccess.Base;
using System.Text.Json;
using EPIC.Entities.DataEntities;
using Humanizer;
using EPIC.CoreRepositories;
using Microsoft.AspNetCore.Server.IISIntegration;
using EPIC.Utils.DataUtils;
using EPIC.HostConsole.Dto;
using EPIC.IdentityRepositories;
using EPIC.IdentityEntities.DataEntities;
using EPIC.Utils.Security;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.MSB.Dto.Inquiry;
using EPIC.MSB.Services;
using DocumentFormat.OpenXml.Office2013.Word;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Web;

namespace EPIC.HostConsole.Services
{
    public class ImportInvestorServices
    {
        private readonly ILogger<ImportInvestorServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly InvestorIdentificationEFRepository _investorIdentificationRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvestorBankAccountEFRepository _investorBankAccountEFRepository;
        private readonly BankEFRepository _bankEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly UserEFRepository _userEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;

        public ImportInvestorServices(
            ILogger<ImportInvestorServices> logger,
            IHttpContextAccessor httpContext,
            DatabaseOptions databaseOptions,
            EpicSchemaDbContext dbContext
            )
        {
            _logger = logger;
            _httpContext = httpContext;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _investorIdentificationRepository = new InvestorIdentificationEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _investorBankAccountEFRepository = new InvestorBankAccountEFRepository(_dbContext, _logger);
            _bankEFRepository = new BankEFRepository(_dbContext, _logger);
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _userEFRepository = new UserEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, logger);
        }

        /// <summary>
        /// Thêm khách hàng từ file excel
        /// </summary>
        public async Task ImportInvestorExcel()
        {
            var errorList = new List<ErrorListDto>();
            var fileStream = new FileStream(@"C:\Users\STE MYSTIC\Desktop\Import_data1.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var wb = new XLWorkbook(fileStream);

            var rows = wb.Worksheet(1).RowsUsed().Skip(1); // Skip header row
            // Thêm vào productItem
            foreach (var row in rows)
            {
                var error = new ErrorListDto();
                var phoneValue = row.Cell(1)?.Value.ToString().Trim();
                var transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    Console.WriteLine("Phone: " + phoneValue);
                    var investorId = (int)_investorEFRepository.NextKey(Investor.SEQ);
                    var investorIdenId = (int)_investorIdentificationRepository.NextKey();
                    var investorGroupId = (int)_investorEFRepository.NextKey(Investor.SEQ_GROUP);
                    var referralCode = row.Cell(23)?.Value.ToString().Trim();
                    var phoneCheck = _investorEFRepository.Entity.FirstOrDefault(o => o.Phone == phoneValue && o.Deleted == YesNo.NO);
                    if (phoneCheck != null)
                    {
                        _investorEFRepository.ThrowException(ErrorCode.InvestorPhoneExisted);
                    }

                    // check mã giới thiệu có tồn tại trong bảng investor ko. Loại ra mã giới thiệu của chính investor này.
                    // check mã giới thiệu có tồn tại trong bảng business customer ko
                    //var investorCheck = _investorEFRepository.Entity.FirstOrDefault(i => i.ReferralCodeSelf == referralCode && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
                    //var bcCheck = _businessCustomerEFRepository.FindByReferralCodeSelf(referralCode);

                    //if (investorCheck == null && bcCheck == null)
                    //{
                    //    _investorEFRepository.ThrowException(ErrorCode.InvestorRefCodeNotFound);
                    //}

                    var investor = new Investor
                    {
                        InvestorId = investorId,
                        Phone = phoneValue,
                        Email = row.Cell(2)?.Value.ToString().ToLower().Trim(),
                        ReferralCode = referralCode,
                        ReferralCodeSelf = phoneValue,
                        Status = Status.ACTIVE,
                        Isonline = YesNo.YES,
                        Deleted = YesNo.NO,
                        InvestorGroupId = investorGroupId,
                        IsCheck = YesNo.NO,
                        Source = InvestorSource.OFFLINE,
                        Step = InvestorAppStep.DA_ADD_BANK,
                        EkycStatus = YesNo.NO,
                    };
                    _investorEFRepository.Entity.Add(investor);
                    _dbContext.SaveChanges();
                    var frontImage = await UploadFile(row.Cell(21)?.Value.ToString().Trim(), phoneValue + "_1.jpg");
                    var backImage = await UploadFile(row.Cell(22)?.Value.ToString().Trim(), phoneValue + "_2.jpg");
                    var investorIdentification = new InvestorIdentification
                    {
                        Id = investorIdenId,
                        IdType = row.Cell(3)?.Value.ToString().Trim(),
                        IdNo = row.Cell(7)?.Value.ToString().Trim(),
                        Fullname = row.Cell(4)?.Value.ToString().Trim(),
                        DateOfBirth = DateTimeUtils.FromStrToDate(row.Cell(5)?.Value.ToString().Trim()),
                        Nationality = row.Cell(11)?.Value.ToString().Trim(),
                        IdIssuer = row.Cell(9)?.Value.ToString().Trim(),
                        IdDate = DateTimeUtils.FromStrToDate(row.Cell(8)?.Value.ToString().Trim()),
                        IdExpiredDate = DateTimeUtils.FromStrToDate(row.Cell(10)?.Value.ToString().Trim()),
                        PlaceOfOrigin = row.Cell(12)?.Value.ToString().Trim(),
                        PlaceOfResidence = row.Cell(13)?.Value.ToString().Trim(),
                        Status = row.Cell(14)?.Value.ToString().Trim(),
                        IsDefault = row.Cell(15)?.Value.ToString().Trim(),
                        IsVerifiedIdentification = YesNo.YES,
                        InvestorId = investorId,
                        InvestorGroupId = investorGroupId,
                        IsVerifiedFace = YesNo.YES,
                        Deleted = YesNo.NO,
                        Sex = row.Cell(6)?.Value.ToString().Trim(),
                        EkycInfoIsConfirmed = YesNo.YES,
                        IdFrontImageUrl = frontImage,
                        IdBackImageUrl = backImage,
                    };
                    _investorIdentificationRepository.Entity.Add(investorIdentification);
                    _dbContext.SaveChanges();

                    var bankBranch = row.Cell(17)?.Value.ToString().Trim();
                    var bankFind = _bankEFRepository.EntityNoTracking.FirstOrDefault(b => b.BankName.ToLower() == bankBranch.ToLower()) 
                        ?? throw new Exception($"Không tìm thấy thông tin ngân hàng {bankBranch}");
                    var investorBankAccount = new InvestorBankAccount
                    {
                        Id = (int)_investorBankAccountEFRepository.NextKey(),
                        BankAccount = row.Cell(18)?.Value.ToString().Trim(),
                        IsDefault = row.Cell(20)?.Value.ToString().Trim(),
                        OwnerAccount = row.Cell(19)?.Value.ToString().Trim(),
                        BankId = bankFind.BankId,
                        InvestorGroupId = investorGroupId,
                        InvestorId = investorId,
                        Deleted = YesNo.NO
                    };
                    _investorBankAccountEFRepository.Entity.Add(investorBankAccount);
                    _dbContext.SaveChanges();

                    var cifCode = new CifCodes
                    {
                        CifId = (int)_cifCodeEFRepository.NextKey(),
                        InvestorId = investorId,
                        CifCode = _cifCodeEFRepository.GenerateCifCode(10),
                    };
                    _cifCodeEFRepository.Entity.Add(cifCode);
                    _dbContext.SaveChanges();

                    var user = new Users
                    {
                        UserId = (int)_userEFRepository.NextKey(Users.SEQ),
                        UserName = phoneValue,
                        Password = CommonUtils.CreateMD5("123456"),
                        Status = Status.ACTIVE,
                        IsDeleted = YesNo.NO,
                        SupperUser = YesNo.NO,
                        Email = row.Cell(2)?.Value.ToString().ToLower().Trim(),
                        UserType = UserTypes.INVESTOR,
                        InvestorId = investorId,
                        PinCode = CommonUtils.CreateMD5("123456"),
                        IsFirstTime = YesNo.YES,
                        IsVerifiedEmail = YesNo.YES,
                        CreatedDate = DateTime.Now,
                    };
                    _userEFRepository.Entity.Add(user);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    error.Reason = ex.Message;
                    error.Phone = phoneValue;
                    errorList.Add(error);
                }
            }
            string json = JsonConvert.SerializeObject(errorList, Formatting.Indented);
            //Console.WriteLine(json);
            string filePath = "C:\\Users\\STE MYSTIC\\Desktop\\error.json";
            File.WriteAllText(filePath, json);
        }

        static async Task<string> UploadFile(string urlImage, string fileName)
        {
            //if (!File.Exists(filePath))
            //{
            //    //log lỗi tại đây
            //    throw new Exception($"File \"{filePath}\" không tồn tại");
            //}
            string accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjhGREUxRkU1QUIwRUY0OUExRTMyRURFOTBDOTI1Nzg1IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2ODk4NDIxMjYsImV4cCI6MTY4OTg0OTMyNiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAxIiwiY2xpZW50X2lkIjoiY2xpZW50MSIsInN1YiI6IjY3NyIsImF1dGhfdGltZSI6MTY4OTg0MjEyNiwiaWRwIjoibG9jYWwiLCJpcF9hZGRyZXNzX2xvZ2luIjoiMTAuMjEyLjEzNC4xMDIiLCJ1c2VyX3R5cGUiOiJSRSIsInVzZXJuYW1lIjoibWluaGx2IiwiZGlzcGxheV9uYW1lIjoiTMOqIFbEg24gTWluaCIsImVtYWlsIjoidXNlckBleGFtcGxlLmNvbSIsImp0aSI6IjQyMkU2MDk3QzFFNjhBNDZBNDZENDBDQkM0MTM0NjhDIiwiaWF0IjoxNjg5ODQyMTI2LCJzY29wZSI6WyJCb25kQVBJIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdfQ.AJyMZdWOFAXWRIictsry4oQno31K9OFmK3gLHCcf1PhGBdmBztT5GmMIb8JcWdl1aMolkf7srHAUpIXBqhyt_s16JSFXSls1H6q9Teg3noFRXcYgwgeYvk63uo8ByWrzn-HdaP5WJgU3nh5PkszQcrxuOWzcUhDy-Y_ySHJB7E04QjJALrlf34mWk_Yb9dP7A0IYcHfKukl90KZrD23hIub-z1_yViuxYEgItjlzO04TUq3I_loiHiL2I9-GDIeNWUJCMRvGXiunSYIHp6oLUjbWSL54ynQslQkq8whzHQMqlYgDTF4xbZGYo-VwRPHzroXcKfZ9klNwipES_a7V8A";
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.epicpartner.vn"),
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var multipartFormContent = new MultipartFormDataContent();

            WebClient client = new();
            Stream imageStream = client.OpenRead(urlImage);

            //Load the file and set the file's Content-Type header
            var fileStreamContent = new StreamContent(imageStream);

            multipartFormContent.Add(fileStreamContent, name: "file", fileName: Path.GetFileName(fileName));

            var response = await httpClient.PostAsync($"/api/file/upload?folder=investor", multipartFormContent);
            var resStr = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"tải file bị lỗi: urlImage = {urlImage}, statusCode = {response.StatusCode}, resBody = {resStr}");
            }

            dynamic resBody = JsonConvert.DeserializeObject(resStr);
            if (resBody.status != 1) 
            {
                throw new Exception("Lỗi tải file");
            }
            return resBody.data;
        }
    }
}

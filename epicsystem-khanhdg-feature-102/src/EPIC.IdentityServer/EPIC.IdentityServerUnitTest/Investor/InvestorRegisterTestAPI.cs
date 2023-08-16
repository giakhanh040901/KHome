using EPIC.DataAccess.Base;
using EPIC.UnitTestsBase;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using EPIC.UnitTestsBase.File;
using System.IO;

namespace EPIC.IdentityServerUnitTest.Investor
{
    public class InvestorRegisterTestAPI : UnitTestBase
    {
        private readonly ITestOutputHelper _output;

        public InvestorRegisterTestAPI(ITestOutputHelper output)
        {
            _baseUrl = "https://api-dev.epicpartner.vn";
            _output = output;
        }

        /// <summary>
        /// Đăng ký tài khoản Investor
        /// </summary>
        [Fact]
        public async void InvestorRegister()
        {
            string email = "investor123@gmail.com";
            string phone = "032985468";
            string password = "Password123";
            IHost host = GetHost<EPIC.IdentityServer.Startup>();
            var dbContext = host.GetService<EpicSchemaDbContext>();
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_baseUrl),
            };

            var identityServerAPI = new UnitTestsBase.IdentityServer.IdentityServerAPI(_baseUrl, httpClient);

            DeleteData(dbContext, email, phone, password);

            //Đăng ký bằng emial, sđt, password (APP)
            var resultRegister = await identityServerAPI.Register3Async(new UnitTestsBase.IdentityServer.RegisterInvestorDto()
            {
                Email = email,
                Phone = phone,
                Password = password
            });

            if (resultRegister.Status != UnitTestsBase.IdentityServer.StatusCode._1)
            {
                Assert.Fail(resultRegister.Message);
            }

            //Tạo otp
            var resultCreateOtp = await identityServerAPI.CreateVerificationCodeSendSms2Async(new UnitTestsBase.IdentityServer.InvestorEmailPhoneDto()
            {
                Email = email,
                Phone = phone
            });

            if (resultCreateOtp.Status != UnitTestsBase.IdentityServer.StatusCode._1)
            {
                Assert.Fail(resultCreateOtp.Message);
            }

            //Xác thực otp
            var resultVerifyOtp = await identityServerAPI.VerifyCode2Async(new UnitTestsBase.IdentityServer.VerificationCodeDto()
            {
                Email = email,
                Phone = phone,
                VerificationCode = "625489"
            });

            if (resultVerifyOtp.Status != UnitTestsBase.IdentityServer.StatusCode._1)
            {
                Assert.Fail(resultVerifyOtp.Message);
            }

            var frontIden = Path.Combine(Environment.CurrentDirectory, "Data", "mattruoc_1.jpg");
            var behindIden = Path.Combine(Environment.CurrentDirectory, "Data", "matsau_1.jpg");
            //Tải lên ảnh giấy tờ
            var frontImage = new UnitTestsBase.IdentityServer.FileParameter(System.IO.File.OpenRead(frontIden), Path.GetFileName(frontIden));
            var backImagr = new UnitTestsBase.IdentityServer.FileParameter(System.IO.File.OpenRead(behindIden), Path.GetFileName(behindIden));

            var resulteKYC = await identityServerAPI.EkycOcr2Async(phone, frontImage, backImagr, "cmnd");
            if (resulteKYC.Status != UnitTestsBase.IdentityServer.StatusCode._1)
            {
                Assert.Fail(resulteKYC.Message);
            }

            //Xác nhận thông tin
            var resulteKYCConfirm = await identityServerAPI.EkycConfirmInfo2Async(new UnitTestsBase.IdentityServer.EKYCConfirmInfoDto()
            {
                IsConfirmed = true,
                Phone = phone,
                IncorrectFields = new List<string>()
            });

            if (resulteKYCConfirm.Status != UnitTestsBase.IdentityServer.StatusCode._1)
            {
                Assert.Fail(resulteKYCConfirm.Message);
            }
            
            //Tải lên lại ảnh giấy giấy tờ mặt trước (ảnh mặt trước bên trên bị đóng nên phải tải lên lại)
            var faceIden = Path.Combine(Environment.CurrentDirectory, "Data", "face.jpg");
            var frontImageFaceMatch = new UnitTestsBase.IdentityServer.FileParameter(System.IO.File.OpenRead(frontIden), Path.GetFileName(frontIden));
            var faceImage = new UnitTestsBase.IdentityServer.FileParameter(System.IO.File.OpenRead(faceIden), Path.GetFileName(faceIden));

            //Face match
            var resulteKYCFaceMatch = await identityServerAPI.EkycFaceMatch2Async(frontImageFaceMatch, faceImage, phone);
            if (resulteKYCFaceMatch.Status != UnitTestsBase.IdentityServer.StatusCode._1)
            {
                Assert.Fail(resulteKYCFaceMatch.Message);
            }

            //Add Bank Account
            var resultAddBankAccount = await identityServerAPI.AddBankAccount2Async(new UnitTestsBase.IdentityServer.BankAccountDto()
            {
                BankId = 3,
                BankAccount = "108429406",
                Phone = phone,
                OwnerAccount = "NGUYEN THI BUOI"
            });

            if (resultAddBankAccount.Status != UnitTestsBase.IdentityServer.StatusCode._1)
            {
                Assert.Fail(resultAddBankAccount.Message);
            }

            DeleteData(dbContext, email, phone, password);
            _output.WriteLine("Đăng ký thành công");
        }

        private void DeleteData(EpicSchemaDbContext dbContext, string email, string phone, string password)
        {

            var user = dbContext.Users.FirstOrDefault(e => e.UserName == phone);
            if (user != null)
            {
                dbContext.Users.Remove(user);
            }
            var investor = dbContext.Investors.FirstOrDefault(e => e.Phone == phone && e.Email == email);
            if (investor != null)
            {
                dbContext.Investors.Remove(investor);
                var investorIdent = dbContext.InvestorIdentifications.FirstOrDefault(e => e.InvestorId == investor.InvestorId);
                if (investorIdent != null)
                {
                    dbContext.InvestorIdentifications.Remove(investorIdent);
                }

                var cifCode = dbContext.CifCodes.FirstOrDefault(e => e.InvestorId == investor.InvestorId);
                if (cifCode != null)
                {
                    dbContext.CifCodes.Remove(cifCode);
                }
            }
            dbContext.SaveChanges();
        }
    }
}

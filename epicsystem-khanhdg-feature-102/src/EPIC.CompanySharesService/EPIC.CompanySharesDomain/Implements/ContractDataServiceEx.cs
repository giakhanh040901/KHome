using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CpsShared;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.EnumType;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.CompanySharesDomain.Implements
{
    public partial class ContractDataServices : IContractDataServices
    {
        private void HopDongSoVaNgayLap(List<ReplaceTextDto> replaceTexts, string contractCode, DateTime createdDate)
        {
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto()
                {
                    FindText = "{{ContractCode}}",
                    ReplaceText = contractCode
                },
                new ReplaceTextDto
                {
                    FindText = "{{TranContent}}",
                    ReplaceText = contractCode
                },
                new ReplaceTextDto
                {
                    FindText = "{{DayContract}}",
                    ReplaceText = createdDate.ToString("dd")
                },
                new ReplaceTextDto
                {
                    FindText = "{{MonthContract}}",
                    ReplaceText = createdDate.ToString("MM")
                },
                new ReplaceTextDto
                {
                    FindText = "{{YearContract}}",
                    ReplaceText = createdDate.ToString("yyyy")
                },
                 new ReplaceTextDto
                {
                    FindText = "{{ContractDate}}",
                    ReplaceText = createdDate.ToString("dd/MM/yyyy")
                }
            });
        }

        /// <summary>
        /// Thông tin nhà đầu tư
        /// </summary>
        /// <param name="replaceTexts"></param>
        /// <param name="order"></param>
        /// <param name="isSignature"></param>
        private void ThongTinNhaDauTu(List<ReplaceTextDto> replaceTexts, Order order, bool isSignature)
        {
            string customerName = null;
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy cif code: {cifCode}"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            if (cifCode.InvestorId != null)
            {
                var investorId = cifCode.InvestorId ?? 0;
                var identification = _investorIdentificationRepository.FindById(order.InvestorIdenId ?? 0);
                if (identification == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy giầy tờ tùy thân của nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.InvestorIdentificationNotFound).ToString()), "");
                }
                var investorBankId = order.InvestorBankAccId ?? 0;
                customerName = identification.Fullname;
                ThongTinNhaDauTuCaNhan(replaceTexts, investorId, order.InvestorIdenId ?? 0, investorBankId);
            }
            else
            {
                var businessCustomerId = cifCode.BusinessCustomerId ?? 0;
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(businessCustomerId);
                if (businessCustomer == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin doanh nghiệp: {businessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
                }
                var businessCustomerBankAccId = order.InvestorBankAccId ?? 0;
                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(businessCustomerBankAccId);
                if (businessCustomerBank == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin ngân hàng của doanh nghiệp: {businessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBusinessCustomerBankNotFound).ToString()), "");
                }
                var investBank = _bankRepository.GetById(businessCustomerBank.BankId);
                if (investBank == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy ngân hàng: {businessCustomerBank.BankId}"), new FaultCode(((int)ErrorCode.CoreBankNotFound).ToString()), "");
                }
                customerName = businessCustomer.RepName;
                replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ADDRESS, businessCustomer.Address),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_POSITION, businessCustomer.RepPosition),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_ADDRESS, businessCustomer.RepAddress),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TRADING_ADDRESS, businessCustomer.TradingAddress),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, businessCustomer.RepName,EnumReplaceTextFormat.UpperCase),
                    new ReplaceTextDto(PropertiesContractFile.BUSINESS_CUSTOMER_NAME, businessCustomer.Name,EnumReplaceTextFormat.UpperCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_NO, businessCustomer.RepIdNo),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_DATE, businessCustomer.RepIdDate,"dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_ISSUER, businessCustomer.RepIdIssuer,EnumReplaceTextFormat.TitleCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_EXPIRED_DATE, ""),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_PHONE, businessCustomer.Phone),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SEX, businessCustomer.RepSex),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BIRTH_DATE, businessCustomer.RepBirthDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, businessCustomer.Address, EnumReplaceTextFormat.TitleCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_EMAIL, businessCustomer.Email),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_NO, businessCustomer.DecisionNo),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_DATE, businessCustomer.DecisionDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_DATE, businessCustomer.LicenseDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_ISSUER, businessCustomer.LicenseIssuer),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TAX_CODE, businessCustomer.TaxCode),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NO, businessCustomerBank.BankAccNo),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NAME, businessCustomerBank.BankAccName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_BRANCH, businessCustomerBank.BankBranchName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_NAME, investBank.BankName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_FULL_NAME, investBank.FullBankName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_CONTRACT_ADDRESS, businessCustomer.Address),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NATIONNALITY, businessCustomer.Nation),
                });
            }

            if (isSignature == true && order.Source == SourceOrder.ONLINE)
            {
                replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.SIGNATURE, $"Đã ký {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME_SIGNATURE, customerName),
                });
            }
            else
            {
                replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.SIGNATURE, $""),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME_SIGNATURE, ""),
                });
            }
        }

        /// <summary>
        /// Thông tin nhà đầu tư cá nhân
        /// </summary>
        /// <param name="replaceTexts"></param>
        /// <param name="investorId"></param>
        /// <param name="identificationId"></param>
        /// <param name="bankAccId"></param>
        /// <exception cref="FaultException"></exception>
        private void ThongTinNhaDauTuCaNhan(List<ReplaceTextDto> replaceTexts, int investorId, int identificationId, int bankAccId)
        {
            var identification = _investorIdentificationRepository.FindById(identificationId);
            if (identification == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin cá nhân của nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.InvestorIdentificationNotFound).ToString()), "");
            }
            var investor = _investorRepository.FindById(investorId);
            if (investor == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
            }
            var bankAccount = _investorBankAccountRepository.GetById(bankAccId);
            if (bankAccount == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy tài khoản thụ hưởng của nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.InvestorBankAccNotFound).ToString()), "");
            }
            var investBank = _bankRepository.GetById(bankAccount.BankId);
            if (investBank == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy ngân hàng: {bankAccount.BankId}"), new FaultCode(((int)ErrorCode.CoreBankNotFound).ToString()), "");
            }
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, identification.Fullname, EnumReplaceTextFormat.UpperCase),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_NO, identification.IdNo),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_TYPE, identification.IdType, EnumReplaceTextFormat.IdType),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_DATE, identification.IdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_ISSUER, identification.IdIssuer, EnumReplaceTextFormat.TitleCase),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_EXPIRED_DATE, identification.IdExpiredDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_PHONE, investor.Phone),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SEX, identification.Sex, EnumReplaceTextFormat.Gender),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BIRTH_DATE, identification.DateOfBirth, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, identification.PlaceOfResidence, EnumReplaceTextFormat.TitleCase),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_EMAIL, investor.Email),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TAX_CODE, investor.TaxCode),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NO, bankAccount.BankAccount),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NAME, bankAccount.OwnerAccount),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_NAME, investBank.BankName),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_FULL_NAME, investBank.FullBankName),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_BRANCH, bankAccount.BankBranch),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NATIONNALITY, identification.Nationality),
            });
        }

        /// <summary>
        /// Chủ sở hưu
        /// </summary>
        /// <param name="replaceTexts"></param>
        /// <param name="tradingProviderId"></param>
        /// <exception cref="FaultException"></exception>
        private void DaiLySoCap(List<ReplaceTextDto> replaceTexts, int tradingProviderId)
        {
            //đại lý sơ cấp
            var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId);
            if (tradingProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đại lý sơ cấp"), new FaultCode(((int)ErrorCode.TradingProviderNotFound).ToString()), "");
            }

            var businessCustomerTrading = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomerTrading == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin đại lý sơ cấp: {tradingProvider.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_NAME, businessCustomerTrading.Name),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_SHORT_NAME, businessCustomerTrading.ShortName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_TAX_CODE, businessCustomerTrading.TaxCode),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_PHONE, businessCustomerTrading.Phone),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_FAX, ""),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_ISSUER, businessCustomerTrading.LicenseIssuer),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_DATE, businessCustomerTrading.LicenseDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_ADDRESS, businessCustomerTrading.Address),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_NAME, businessCustomerTrading.RepName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_POSITION, businessCustomerTrading.RepPosition),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_DECISION_NO, businessCustomerTrading.DecisionNo),
            });
        }

        private void TaiKhoanThuHuongDaiLySoCap(List<ReplaceTextDto> replaceTexts, int businessCustomerBankAccId)
        {
            //tài khoản thụ hưởng của dlsc trong bán theo kỳ hạn
            var tradingBankAcc = _businessCustomerRepository.FindBusinessCusBankById(businessCustomerBankAccId);
            if (tradingBankAcc == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy tài khoản thụ hưởng đại lý sơ cấp"), new FaultCode(((int)ErrorCode.CoreBusinessCustomerBankNotFound).ToString()), "");
            }

            var tradingBank = _bankRepository.GetById(tradingBankAcc.BankId);
            if (tradingBank == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy bank: {tradingBankAcc.BankId}"), new FaultCode(((int)ErrorCode.CoreBankNotFound).ToString()), "");
            }
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NAME, tradingBankAcc.BankAccName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NO, tradingBankAcc.BankAccNo),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_NAME, tradingBank.BankName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_FULL_NAME, tradingBank.FullBankName),
            });
        }

        /// <summary>
        /// CPS Info
        /// </summary>
        /// <param name="replaceTexts"></param>
        /// <param name="cpsInfo"></param>
        /// <exception cref="FaultException"></exception>
        private void DuAn(List<ReplaceTextDto> replaceTexts, CpsInfo cpsInfo)
        {
            replaceTexts.Add(new ReplaceTextDto
            {
                FindText = "{{CpsName}}",
                ReplaceText = cpsInfo.CpsName
            });
        }


        ///// <summary>
        ///// Dòng tiền
        ///// </summary>
        ///// <param name="replaceTexts"></param>
        ///// <exception cref="FaultException"></exception>
        //private void DongTien(List<ReplaceTextDto> replaceTexts, CashFlowDto cashFlow)
        //{
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{Interest}}",
        //        ReplaceText = cashFlow.InterestRate.ToString("#.#").Replace(".", ",")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{StartDate}}",
        //        ReplaceText = cashFlow.StartDate?.ToString("dd/MM/yyyy")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{EndDate}}",
        //        ReplaceText = cashFlow.EndDate?.ToString("dd/MM/yyyy")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{InvestMoney}}",
        //        ReplaceText = cashFlow.TotalValue.ToString("N0").Replace(",", ".")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{TaxProfit}}",
        //        ReplaceText = cashFlow.TaxProfit.ToString("N0").Replace(",", ".")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{Tax}}",
        //        ReplaceText = cashFlow.Tax?.ToString("N0").Replace(",", ".")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{TotalReceiveValue}}",
        //        ReplaceText = cashFlow.TotalReceiveValue.ToString("N0").Replace(",", ".")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{ActuallyProfit}}",
        //        ReplaceText = cashFlow.ActuallyProfit.ToString("N0").Replace(",", ".")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{BeforeTaxProfit}}",
        //        ReplaceText = cashFlow.BeforeTaxProfit.ToString("N0").Replace(",", ".")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{TotalReceiveValue}}",
        //        ReplaceText = cashFlow.TotalReceiveValue.ToString("N0").Replace(",", ".")
        //    });
        //    replaceTexts.Add(new ReplaceTextDto
        //    {
        //        FindText = "{{InvestMoneyText}}",
        //        ReplaceText = NumberToText.ConvertNumberToText((double)cashFlow.TotalValue)
        //    });
        //}
    }
}

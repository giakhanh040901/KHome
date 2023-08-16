using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.UploadFile;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public partial class BondContractDataService : IBondContractDataService
    {
        private void HopDongSoVaNgayLap(List<ReplaceTextDto> replateTexts, string contractCode, DateTime createdDate)
        {
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
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
                }
            });
        }

        /// <summary>
        /// Thông tin nhà đầu tư
        /// </summary>
        /// <param name="replateTexts"></param>
        /// <param name="order"></param>
        /// <param name="isSignature"></param>
        private void ThongTinNhaDauTu(List<ReplaceTextDto> replateTexts, BondOrder order, bool isSignature)
        {
            string customerName = null;
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy cif code: {cifCode}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            if (cifCode.InvestorId != null)
            {
                var investorId = cifCode.InvestorId ?? 0;
                var identification = _investorIdentificationRepository.FindById(order.InvestorIdenId ?? 0);
                if (identification == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy giầy tờ tùy thân của nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                var investorBankId = order.InvestorBankAccId ?? 0;
                customerName = identification.Fullname;
                ThongTinNhaDauTuCaNhan(replateTexts, investorId, order.InvestorIdenId ?? 0, investorBankId);
            }
            else
            {
                var businessCustomerId = cifCode.BusinessCustomerId ?? 0;
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(businessCustomerId);
                if (businessCustomer == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin doanh nghiệp: {businessCustomerId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                var businessCustomerBankAccId = order.InvestorBankAccId ?? 0;
                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(businessCustomerBankAccId);
                if (businessCustomerBank == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin ngân hàng của doanh nghiệp: {businessCustomerId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                var investBank = _bankRepository.GetById(businessCustomerBank.BankId);
                if (investBank == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy ngân hàng: {businessCustomerBank.BankId}"), new FaultCode(((int)ErrorCode.CoreBankNotFound).ToString()), "");
                }
                customerName = businessCustomer.RepName;
                replateTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerName}}",
                         ReplaceText = businessCustomer.RepName.ToUpper()
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerAddress}}",
                         ReplaceText = businessCustomer.Address
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerRepAddress}}",
                         ReplaceText = businessCustomer.RepAddress
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerRepPosition}}",
                         ReplaceText = businessCustomer.RepPosition
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerTradingAddress}}",
                         ReplaceText = businessCustomer.TradingAddress
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{BusinessCustomerName}}",
                         ReplaceText = businessCustomer.Name.ToUpper()
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdNo}}",
                         ReplaceText = businessCustomer.RepIdNo
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdDate}}",
                         ReplaceText = businessCustomer.RepIdDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerIdIssuer}}",
                         ReplaceText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(businessCustomer.RepIdIssuer?.ToLower() ?? " ")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerPhone}}",
                         ReplaceText = businessCustomer.Phone
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerSex}}",
                         ReplaceText = ContractDataUtils.GetNameGender(businessCustomer.RepSex)
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBirthDate}}",
                         ReplaceText = businessCustomer.RepBirthDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerResidentAddress}}",
                         ReplaceText =  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(businessCustomer.Address?.ToLower() ?? " ")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerEmail}}",
                         ReplaceText = businessCustomer.Email
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerTaxCode}}",
                         ReplaceText = businessCustomer.TaxCode
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankAccNo}}",
                         ReplaceText = businessCustomerBank.BankAccNo
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankAccName}}",
                         ReplaceText = businessCustomerBank.BankAccName
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankName}}",
                         ReplaceText = investBank.BankName
                    }, 
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerFullBankName}}",
                         ReplaceText = investBank.FullBankName
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerBankBranch}}",
                         ReplaceText = businessCustomerBank.BankBranchName
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerContractAddress}}",
                         ReplaceText = businessCustomer.Address
                    },new ReplaceTextDto
                    {
                         FindText = "{{CustomerDecisionNo}}",
                         ReplaceText = businessCustomer.DecisionNo
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerDecisionDate}}",
                         ReplaceText = businessCustomer.DecisionDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerLicenseIssuer}}",
                         ReplaceText = businessCustomer.LicenseIssuer
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerLicenseDate}}",
                         ReplaceText = businessCustomer.LicenseDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerNationality}}",
                         ReplaceText = businessCustomer.Nation
                    }

                });   
            }

            if (isSignature == true && order.Source == SourceOrder.ONLINE)
            {
                replateTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto
                    {
                         FindText = "{{Signature}}",
                         ReplaceText = $"Đã ký {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}"
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerNameSignature}}",
                         ReplaceText = customerName.ToUpper()
                    }
                });
            }
            else
            {
                replateTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto
                    {
                         FindText = "{{Signature}}",
                         ReplaceText = ""
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{CustomerNameSignature}}",
                         ReplaceText = ""
                    }
                });
            }
        }

        /// <summary>
        /// Thông tin nhà đầu tư cá nhân
        /// </summary>
        /// <param name="replateTexts"></param>
        /// <param name="investorId"></param>
        /// <param name="identificationId"></param>
        /// <param name="bankAccId"></param>
        /// <exception cref="FaultException"></exception>
        private void ThongTinNhaDauTuCaNhan(List<ReplaceTextDto> replateTexts, int investorId, int identificationId, int bankAccId)
        {
            var identification = _investorIdentificationRepository.FindById(identificationId);
            if (identification == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin cá nhân của nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var investor = _investorRepository.FindById(investorId);
            if (investor == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var bankAccount = _investorBankAccountRepository.GetById(bankAccId);
            if (bankAccount == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy tài khoản thụ hưởng của nhà đầu tư: {investorId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var investBank = _bankRepository.GetById(bankAccount.BankId);
            if (investBank == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy ngân hàng: {bankAccount.BankId}"), new FaultCode(((int)ErrorCode.CoreBankNotFound).ToString()), "");
            }
           
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{CustomerName}}",
                    ReplaceText = identification.Fullname.ToUpper()
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerIdNo}}",
                    ReplaceText = identification.IdNo
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerIdType}}",
                    ReplaceText = ContractDataUtils.GetIdType(identification.IdType)
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerIdDate}}",
                    ReplaceText = identification.IdDate?.ToString("dd/MM/yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerIdIssuer}}",
                    ReplaceText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(identification.IdIssuer?.ToLower() ?? " ")
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerPhone}}",
                    ReplaceText = investor.Phone
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerSex}}",
                    ReplaceText = ContractDataUtils.GetNameGender(identification.Sex)
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerBirthDate}}",
                    ReplaceText = identification.DateOfBirth?.ToString("dd/MM/yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerResidentAddress}}",
                    ReplaceText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(identification.PlaceOfResidence?.ToLower() ?? " ")
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerEmail}}",
                    ReplaceText = investor.Email
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerTaxCode}}",
                    ReplaceText = investor.TaxCode
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerBankAccNo}}",
                    ReplaceText = bankAccount.BankAccount
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerBankAccName}}",
                    ReplaceText = bankAccount.OwnerAccount
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerBankName}}",
                    ReplaceText = investBank.BankName
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerFullBankName}}",
                    ReplaceText = investBank.FullBankName
                },
                new ReplaceTextDto
                {
                    FindText = "{{CustomerBankBranch}}",
                    ReplaceText = bankAccount.BankBranch
                }
            });
        }

        /// <summary>
        /// Chủ sở hưu
        /// </summary>
        /// <param name="replateTexts"></param>
        /// <param name="tradingProviderId"></param>
        /// <exception cref="FaultException"></exception>
        private void DaiLySoCap(List<ReplaceTextDto> replateTexts, int tradingProviderId)
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

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderName}}",
                ReplaceText = businessCustomerTrading.Name
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderNameUpperCase}}",
                ReplaceText = businessCustomerTrading.Name.ToUpper()
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderShortName}}",
                ReplaceText = businessCustomerTrading.ShortName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderPhone}}",
                ReplaceText = businessCustomerTrading.Phone
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderTaxCode}}",
                ReplaceText = businessCustomerTrading.TaxCode
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseIssuer}}",
                ReplaceText = businessCustomerTrading.LicenseIssuer
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseDate}}",
                ReplaceText = businessCustomerTrading.LicenseDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderAddress}}",
                ReplaceText = businessCustomerTrading.Address
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepName}}",
                ReplaceText = businessCustomerTrading.RepName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepPosition}}",
                ReplaceText = businessCustomerTrading.RepPosition
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderDecisionNo}}",
                ReplaceText = businessCustomerTrading.DecisionNo
            });
        }

        private void TaiKhoanThuHuongDaiLySoCap(List<ReplaceTextDto> replateTexts, int businessCustomerBankAccId)
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

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccName}}",
                ReplaceText = tradingBankAcc.BankAccName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccNo}}",
                ReplaceText = tradingBankAcc.BankAccNo
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankName}}",
                ReplaceText = tradingBank.BankName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingFullBankName}}",
                ReplaceText = tradingBank.FullBankName
            });
        }

        private void LoaiSanPham(List<ReplaceTextDto> replateTexts, int classify)
        {
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{PolicyClassifyName}}",
                ReplaceText = BondPolicyClassify.KeyValues[classify]
            });
        }

        /// <summary>
        /// Thông tin đại lý lưu ký
        /// </summary>
        /// <param name="replateTexts"></param>
        /// <param name="depositProviderId"></param>
        /// <exception cref="FaultException"></exception>
        private void DaiLyLuKy(List<ReplaceTextDto> replateTexts, int depositProviderId)
        {
            //đại lý lưu ký
            var depositProvider = _depositProviderRepository.FindById(depositProviderId);
            if (depositProvider == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy đại lý lưu ký: {depositProviderId}"), new FaultCode(((int)ErrorCode.BondDepositProviderNotFound).ToString()), "");
            }
            var bussinessCustomerDeposit = _businessCustomerRepository.FindBusinessCustomerById(depositProvider.BusinessCustomerId);
            if (bussinessCustomerDeposit == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin đại lý lưu ký: {depositProvider.BusinessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
            }

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DepositName}}",
                ReplaceText = bussinessCustomerDeposit.Name
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DepositNameUpperCase}}",
                ReplaceText = bussinessCustomerDeposit.Name.ToUpper()
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{DepositShortName}}",
                ReplaceText = bussinessCustomerDeposit.ShortName
            });
        }
    }
}

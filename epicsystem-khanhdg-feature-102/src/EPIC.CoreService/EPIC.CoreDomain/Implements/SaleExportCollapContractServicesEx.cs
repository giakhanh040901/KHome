using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.UploadFile;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public partial class SaleExportCollapContractServices : ISaleExportCollapContractServices
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

        private void ThongTinNhaDauTu(List<ReplaceTextDto> replateTexts, int saleId, int tradingProviderId, bool isSignature)
        {
            var sale = _saleRepository.FindSaleById(saleId, tradingProviderId);
            if(sale.InvestorId != null)
            {
                ThongTinNhaDauTuCaNhan(replateTexts, sale.InvestorId ?? 0, sale.InvestorBankAccId ?? 0, isSignature);
            }
            else
            {
                var businessCustomerId = sale.BusinessCustomerId ?? 0;
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(businessCustomerId);
                if (businessCustomer == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin doanh nghiệp: {businessCustomerId}"), new FaultCode(((int)ErrorCode.CoreBussinessCustomerNotFound).ToString()), "");
                }
                var businessCustomerBankAccId = sale.BusinessCustomerBankAccId ?? 0;
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
                replateTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleName}}",
                         ReplaceText = businessCustomer.RepName.ToUpper()
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleAddress}}",
                         ReplaceText = businessCustomer.Address
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleRepAddress}}",
                         ReplaceText = businessCustomer.RepAddress
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleRepPosition}}",
                         ReplaceText = businessCustomer.RepPosition
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleTradingAddress}}",
                         ReplaceText = businessCustomer.TradingAddress
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleCustomerName}}",
                         ReplaceText = businessCustomer.Name.ToUpper()
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdNo}}",
                         ReplaceText = businessCustomer.RepIdNo
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdDate}}",
                         ReplaceText = businessCustomer.RepIdDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleIdIssuer}}",
                         ReplaceText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(businessCustomer.RepIdIssuer?.ToLower() ?? " ")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SalePhone}}",
                         ReplaceText = businessCustomer.Phone
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleSex}}",
                         ReplaceText = ContractDataUtils.GetNameGender(businessCustomer.RepSex)
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBirthDate}}",
                         ReplaceText = businessCustomer.RepBirthDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleResidentAddress}}",
                         ReplaceText =  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(businessCustomer.Address?.ToLower() ?? " ")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleEmail}}",
                         ReplaceText = businessCustomer.Email
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleTaxCode}}",
                         ReplaceText = businessCustomer.TaxCode
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankAccNo}}",
                         ReplaceText = businessCustomerBank.BankAccNo
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankAccName}}",
                         ReplaceText = businessCustomerBank.BankAccName
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankName}}",
                         ReplaceText = investBank.BankName
                    },
                     new ReplaceTextDto
                    {
                         FindText = "{{SaleFullBankName}}",
                         ReplaceText = investBank.FullBankName
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleBankBranch}}",
                         ReplaceText = businessCustomerBank.BankBranchName
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleContactAddress}}",
                         ReplaceText = businessCustomer.Address
                    },new ReplaceTextDto
                    {
                         FindText = "{{SaleDecisionNo}}",
                         ReplaceText = businessCustomer.DecisionNo
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleDecisionDate}}",
                         ReplaceText = businessCustomer.DecisionDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleLicenseIssuer}}",
                         ReplaceText = businessCustomer.LicenseIssuer
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleLicenseDate}}",
                         ReplaceText = businessCustomer.LicenseDate?.ToString("dd/MM/yyyy")
                    },
                    new ReplaceTextDto
                    {
                         FindText = "{{SaleNationality}}",
                         ReplaceText = businessCustomer.Nation
                    }

                });
                if (isSignature == true)
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
                         FindText = "{{SaleNameSignature}}",
                         ReplaceText = businessCustomer.RepName?.ToUpper()
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
                         FindText = "{{SaleNameSignature}}",
                         ReplaceText = ""
                    }
                });
                }
            }
        }

        /// <summary>
        /// Thông tin nhà đầu tư cá nhân
        /// </summary>
        /// <param name="replateTexts"></param>
        /// <param name="investorId"></param>
        /// <param name="bankAccId"></param>
        /// <exception cref="FaultException"></exception>
        private void ThongTinNhaDauTuCaNhan(List<ReplaceTextDto> replateTexts, int investorId, int bankAccId, bool isSignature)
        {
            var identification = _managerInvestorRepository.GetDefaultIdentification(investorId, false);
            var investor = _investorRepository.FindById(investorId);
            var bankAccount = _investorBankAccountRepository.GetById(bankAccId);
            var investBank = _bankRepository.GetById(bankAccount?.BankId ?? 0);
            replateTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto
                {
                    FindText = "{{SaleName}}",
                    ReplaceText = identification?.Fullname.ToUpper()
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleIdNo}}",
                    ReplaceText = identification?.IdNo
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleIdType}}",
                    ReplaceText = ContractDataUtils.GetNameGender(identification?.IdType)
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleIdDate}}",
                    ReplaceText = identification?.IdDate?.ToString("dd/MM/yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleIdIssuer}}",
                    ReplaceText = identification?.IdIssuer
                },
                new ReplaceTextDto
                {
                    FindText = "{{SalePhone}}",
                    ReplaceText = investor?.Phone
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleFax}}",
                    ReplaceText = investor?.Fax
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleSex}}",
                    ReplaceText = ContractDataUtils.GetNameGender(identification?.Sex)
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleBirthDate}}",
                    ReplaceText = identification?.DateOfBirth?.ToString("dd/MM/yyyy")
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleResidentAddress}}",
                    ReplaceText = identification?.PlaceOfResidence
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleEmail}}",
                    ReplaceText = investor?.Email
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleTaxCode}}",
                    ReplaceText = investor?.TaxCode
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleBankAccNo}}",
                    ReplaceText = bankAccount?.BankAccount
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleBankAccName}}",
                    ReplaceText = bankAccount?.OwnerAccount
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleBankName}}",
                    ReplaceText = investBank?.BankName
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleFullBankName}}",
                    ReplaceText = investBank?.FullBankName
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleBankBranch}}",
                    ReplaceText = bankAccount?.BankBranch
                },
                new ReplaceTextDto
                {
                    FindText = "{{SaleContactAddress}}",
                    ReplaceText = investor?.ContactAddress
                },
            });
            if (isSignature == true)
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
                         FindText = "{{SaleNameSignature}}",
                         ReplaceText = identification?.Fullname.ToUpper()
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
                         FindText = "{{SaleNameSignature}}",
                         ReplaceText = ""
                    }
                });
            }
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
            var businessCustomerTrading = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider?.BusinessCustomerId ?? 0);
            var businessCustomerTradingBank = _businessCustomerRepository.FindBusinessCusBankDefault(businessCustomerTrading?.BusinessCustomerId ?? 0, IsTemp.NO);
            var tradingBank = _bankRepository.GetById(businessCustomerTradingBank?.BankId ?? 0);
            

            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderName}}",
                ReplaceText = businessCustomerTrading?.Name
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderNameUpperCase}}",
                ReplaceText = businessCustomerTrading?.Name.ToUpper()
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderShortName}}",
                ReplaceText = businessCustomerTrading?.ShortName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderTaxCode}}",
                ReplaceText = businessCustomerTrading?.TaxCode
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseIssuer}}",
                ReplaceText = businessCustomerTrading?.LicenseIssuer
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderLicenseDate}}",
                ReplaceText = businessCustomerTrading?.LicenseDate?.ToString("dd/MM/yyyy")
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderAddress}}",
                ReplaceText = businessCustomerTrading?.Address
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderPhone}}",
                ReplaceText = businessCustomerTrading?.Phone
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderFax}}",
                ReplaceText = ""
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepName}}",
                ReplaceText = businessCustomerTrading?.RepName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderRepPosition}}",
                ReplaceText = businessCustomerTrading?.RepPosition
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingProviderDecisionNo}}",
                ReplaceText = businessCustomerTrading?.DecisionNo
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccNo}}",
                ReplaceText = businessCustomerTradingBank?.BankAccNo
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankName}}",
                ReplaceText = tradingBank?.BankName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingFullBankName}}",
                ReplaceText = tradingBank?.FullBankName
            });
        }

        private void TaiKhoanThuHuongDaiLySoCap(List<ReplaceTextDto> replateTexts, int businessCustomerBankAccId)
        {
            //tài khoản thụ hưởng của dlsc trong bán theo kỳ hạn
            var tradingBankAcc = _businessCustomerRepository.FindBusinessCusBankById(businessCustomerBankAccId);         
            var tradingBank = _bankRepository.GetById(tradingBankAcc?.BankId ?? 0);  
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccName}}",
                ReplaceText = tradingBankAcc?.BankAccName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankAccNo}}",
                ReplaceText = tradingBankAcc?.BankAccNo
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingBankName}}",
                ReplaceText = tradingBank?.BankName
            });
            replateTexts.Add(new ReplaceTextDto
            {
                FindText = "{{TradingFullBankName}}",
                ReplaceText = tradingBank?.FullBankName
            });
        }


    }
}

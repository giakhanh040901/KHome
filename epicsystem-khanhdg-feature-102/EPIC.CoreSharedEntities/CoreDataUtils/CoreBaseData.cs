using EPIC.CoreSharedEntities.Dto.BusinessCustomer;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.CoreSharedEntities.CoreDataUtils
{
    public static class CoreBaseData
    {
        /// <summary>
        /// Get base data cho hợp đồng
        /// </summary>
        /// <param name="replaceTexts">List các biến fill hợp đồng</param>
        /// <param name="contractCode">mã hợp đồng</param>
        /// <param name="contractDate">ngày hợp đồng</param>
        /// <param name="investor">khách hàng cá nhân (trong trường hợp dành cho khách hàng cá nhân)</param>
        /// <param name="businessCustomer">khách hàng doanh nghiệp (trong trường hợp là khách hàng doanh nghiệp)</param>
        /// <param name="tradingProvider">đại lý sơ cấp</param>
        /// <param name="orderSource">sổ lệnh</param>
        /// <param name="isSignature">có fill chữ đã ký không</param>
        public static List<ReplaceTextDto> GetBaseDataForContract(DateTime contractDate, InvestorDataForContractDto investor, BusinessCustomerForContractDto businessCustomer, TradingProviderDataForContractDto tradingProvider, int? orderSource, bool isSignature)
        {
            List<ReplaceTextDto> replaceTexts = new();
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                #region data chung
                new ReplaceTextDto(PropertiesContractFile.DAY_CONTRACT, contractDate, "dd"),
                new ReplaceTextDto(PropertiesContractFile.MONTH_CONTRACT, contractDate, "MM"),
                new ReplaceTextDto(PropertiesContractFile.YEAR_CONTRACT, contractDate, "yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_DATE, contractDate, "dd/MM/yyyy"),
                #endregion
                #region trding provider data
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_NAME, tradingProvider.BusinessCustomerTrading?.Name),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_SHORT_NAME, tradingProvider.BusinessCustomerTrading?.ShortName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_TAX_CODE, tradingProvider.BusinessCustomerTrading?.TaxCode),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_PHONE, tradingProvider.BusinessCustomerTrading?.Phone),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_FAX),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_ISSUER, tradingProvider.BusinessCustomerTrading?.LicenseIssuer),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_DATE, tradingProvider.BusinessCustomerTrading?.LicenseDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_ADDRESS, tradingProvider.BusinessCustomerTrading?.Address),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_NAME, tradingProvider.BusinessCustomerTrading?.RepName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_POSITION, tradingProvider.BusinessCustomerTrading?.RepPosition),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_DECISION_NO, tradingProvider.BusinessCustomerTrading?.DecisionNo),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NAME, tradingProvider.TradingBank?.BankAccName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NO, tradingProvider.TradingBank?.BankAccNo),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_NAME, tradingProvider.TradingBank?.BankName),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_FULL_NAME, tradingProvider.TradingBank?.FullBankName),
                #endregion
                //new ReplaceTextDto(PropertiesContractFile.INTEREST_PERIOD, GetInterestPeriodTypeName(policyDetail.InterestType ?? 1, policyDetail.InterestPeriodQuantity, policyDetail.InterestPeriodType)),
                //new ReplaceTextDto(PropertiesContractFile.TENOR, $"{policyDetail.PeriodQuantity} {GetNameDateType(policyDetail.PeriodType)}"),
            });
            var customerName = string.Empty;
            if (investor != null)
            {
                customerName = investor.InvestorIdentification?.Fullname;
                replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    #region investor data
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, investor.InvestorIdentification?.Fullname, EnumReplaceTextFormat.UpperCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_NO, investor.InvestorIdentification?.IdNo),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_TYPE, investor.InvestorIdentification?.IdType, EnumReplaceTextFormat.IdType),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_DATE, investor.InvestorIdentification?.IdDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_ISSUER, investor.InvestorIdentification?.IdIssuer, EnumReplaceTextFormat.TitleCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_DATE, investor.InvestorIdentification?.IdDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_EXPIRED_DATE, investor.InvestorIdentification?.IdExpiredDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_PHONE, investor.Investor?.Phone),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SEX, investor.InvestorIdentification?.Sex, EnumReplaceTextFormat.Gender),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BIRTH_DATE, investor.InvestorIdentification?.DateOfBirth, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, investor.InvestorIdentification?.PlaceOfResidence, EnumReplaceTextFormat.TitleCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_EMAIL, investor.Investor?.Email),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TAX_CODE, investor.Investor?.TaxCode),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NO, investor.InvestorBankAccount?.BankAccount),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NAME, investor.InvestorBankAccount?.OwnerAccount),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_NAME, investor.InvestorBankAccount?.CoreBankName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_FULL_NAME, investor.InvestorBankAccount?.CoreFullBankName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_BRANCH, investor.InvestorBankAccount?.BankBranch),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NATIONNALITY, investor.InvestorIdentification?.Nationality),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_CONTACT_ADDRESS, investor.InvestorContactAddress?.ContactAddress),
                    #endregion
                });

            }
            else
            {
                customerName = businessCustomer.BusinessCustomer?.RepName;
                replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ADDRESS, businessCustomer.BusinessCustomer?.Address),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_POSITION, businessCustomer.BusinessCustomer?.RepPosition),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_ADDRESS, businessCustomer.BusinessCustomer?.RepAddress),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TRADING_ADDRESS, businessCustomer.BusinessCustomer?.TradingAddress),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, businessCustomer.BusinessCustomer?.RepName,EnumReplaceTextFormat.UpperCase),
                    new ReplaceTextDto(PropertiesContractFile.BUSINESS_CUSTOMER_NAME, businessCustomer.BusinessCustomer?.Name,EnumReplaceTextFormat.UpperCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_NO, businessCustomer.BusinessCustomer?.RepIdNo),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_DATE, businessCustomer.BusinessCustomer?.RepIdDate,"dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_ISSUER, businessCustomer.BusinessCustomer?.RepIdIssuer,EnumReplaceTextFormat.TitleCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_EXPIRED_DATE, ""),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_PHONE, businessCustomer.BusinessCustomer?.Phone),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SEX, businessCustomer.BusinessCustomer?.RepSex),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BIRTH_DATE, businessCustomer.BusinessCustomer?.RepBirthDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, businessCustomer.BusinessCustomer?.Address, EnumReplaceTextFormat.TitleCase),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_EMAIL, businessCustomer.BusinessCustomer?.Email),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_NO, businessCustomer.BusinessCustomer?.DecisionNo),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_DATE, businessCustomer.BusinessCustomer?.DecisionDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_DATE, businessCustomer.BusinessCustomer?.LicenseDate, "dd/MM/yyyy"),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_ISSUER, businessCustomer.BusinessCustomer?.LicenseIssuer),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TAX_CODE, businessCustomer.BusinessCustomer?.TaxCode),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NO, businessCustomer.BusinessCustomerBank?.BankAccNo),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NAME, businessCustomer.BusinessCustomerBank?.BankAccName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_BRANCH, businessCustomer.BusinessCustomerBank?.BankBranchName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_NAME, businessCustomer.BusinessCustomerBank?.BankName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_FULL_NAME, businessCustomer.BusinessCustomerBank?.FullBankName),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_CONTRACT_ADDRESS, businessCustomer.BusinessCustomer?.Address),
                    new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NATIONNALITY, businessCustomer.BusinessCustomer?.Nation),
                });
            }
            if (isSignature == true && orderSource == SourceOrder.ONLINE)
            {
                replaceTexts.AddRange(new List<ReplaceTextDto>()
                {
                    new ReplaceTextDto(PropertiesContractFile.SIGNATURE, $"Đã ký {contractDate.ToString("dd-MM-yyyy HH:mm:ss")}"),
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

            return replaceTexts;
        }

        /// <summary>
        /// Lấy data test cho hợp đồng test
        /// </summary>
        /// <returns></returns>
        public static List<ReplaceTextDto> GetDataContractFileTest()
        {
            List<ReplaceTextDto> replaceTexts = new List<ReplaceTextDto>();
            var createdDate = DateTime.Now;
            replaceTexts.AddRange(new List<ReplaceTextDto>()
            {
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME, "Nguyễn Văn A", EnumReplaceTextFormat.UpperCase),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_NO, "0123456789123"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_TYPE, "CCCD", EnumReplaceTextFormat.IdType),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_DATE, DateTime.Now, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_ISSUER, "CA Thành Phố Hà Nội", EnumReplaceTextFormat.TitleCase),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ID_EXPIRED_DATE, DateTime.Now, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_PHONE, "0123456789"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SEX, "Nam"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BIRTH_DATE,  DateTime.Now, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_RESIDENT_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội", EnumReplaceTextFormat.TitleCase),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_EMAIL, "example@gmail.com"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TAX_CODE, "12345676"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NO, "23219217132141"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_ACC_NAME,  "Nguyen Van A"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_NAME, "BankName"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_FULL_NAME, "Ngân hàng TMCP Xuất Nhập khẩu Việt Nam"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_BANK_BRANCH, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NATIONNALITY, "Việt Nam"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_CONTACT_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_POSITION, "Nguyễn Văn B"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_REP_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_TRADING_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.BUSINESS_CUSTOMER_NAME, "Công ty TNHH ABC", EnumReplaceTextFormat.UpperCase),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_NO, "A123456"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_DECISION_DATE, DateTime.Now, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_DATE, DateTime.Now , "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_LICENSE_ISSUER, "TP Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_CONTRACT_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.SIGNATURE, $"Đã ký {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_NAME_SIGNATURE, "Nguyen Van A"),
                
                #region data chung
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_CODE,"EI00000000"),
                new ReplaceTextDto(PropertiesContractFile.DAY_CONTRACT, createdDate, "dd"),
                new ReplaceTextDto(PropertiesContractFile.MONTH_CONTRACT, createdDate, "MM"),
                new ReplaceTextDto(PropertiesContractFile.YEAR_CONTRACT, createdDate, "yyyy"),
                new ReplaceTextDto(PropertiesContractFile.CONTRACT_DATE, createdDate, "dd/MM/yyyy"),
                #endregion
               
                #region trding provider data
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_NAME, "Công ty cổ phần đầu tư ABC"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_SHORT_NAME, "ABC"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_TAX_CODE, "2423535745"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_PHONE, "0123456789"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_FAX, "387492382423"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_ISSUER, "TP Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_LICENSE_DATE, createdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_NAME, "Nguyễn Văn C"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_REP_POSITION, "Giám đốc"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_PROVIDER_DECISION_NO, "73627563223"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NAME, "Nguyen Van C"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_ACC_NO, "37264236756"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_NAME, "BankName"),
                new ReplaceTextDto(PropertiesContractFile.TRADING_BANK_FULL_NAME, "Ngân hàng phát triển nông thôn AGRIBANK"),
                #endregion
               
                new ReplaceTextDto(PropertiesContractFile.TRAN_DATE, createdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.INTEREST_PERIOD, "Cuối kỳ"),
                
                #region chủ đầu tư
                new ReplaceTextDto(PropertiesContractFile.OWNER_NAME, "Công ty ABC"),
                new ReplaceTextDto(PropertiesContractFile.OWNER_SHORT_NAME, "ABC"),    
                #endregion
                
                #region tổng thầu
                new ReplaceTextDto(PropertiesContractFile.GENERAL_CONTRACTOR_NAME, "Công ty DEF"),
                new ReplaceTextDto(PropertiesContractFile.GENERAL_CONTRACTOR_SHORT_NAME, "DEF"),    
                #endregion
                
                #region Dự án
                new ReplaceTextDto(PropertiesContractFile.PROJECT_NAME, "Căn hộ cao cấp XYZ"),
                new ReplaceTextDto(PropertiesContractFile.PROJECT_ADDRESS, "123 Trung Kính, Yên Hoà, Cầu Giấy, Hà Nội"),    
                #endregion 
                
                #region Dự án
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SALE_NAME, "Nguyễn Văn D"),
                new ReplaceTextDto(PropertiesContractFile.CUSTOMER_SALE_PHONE, "2003284324893"),    
                #endregion

                #region tính toán, dòng tiền
                new ReplaceTextDto(PropertiesContractFile.INTEREST_CASHFLOW, 10, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.START_DATE_CASHFLOW, createdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.END_DATE_CASHFLOW, createdDate, "dd/MM/yyyy"),
                new ReplaceTextDto(PropertiesContractFile.DAY_END_DATE_CASHFLOW, createdDate, "dd"),
                new ReplaceTextDto(PropertiesContractFile.MONTH_END_DATE_CASHFLOW, createdDate, "MM"),
                new ReplaceTextDto(PropertiesContractFile.YEAR_END_DATE_CASHFLOW, createdDate, "yyyy"),
                new ReplaceTextDto(PropertiesContractFile.INVEST_MONEY_CASHFLOW,1000000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX_PROFIT_CASHFLOW, 10000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX, 1000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TOTAL_RECEIVE_VALUE_CASHFLOW,1000000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.ACTUALLY_PROFIT_CASHFLOW, 1000000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.BEFORE_TAX_PROFIT_CASHFLOW, 1000000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.INVEST_MONEY_TEXT_CASHFLOW, 1000000000, EnumReplaceTextFormat.NumberToWord),
                new ReplaceTextDto(PropertiesContractFile.TENOR, "24 Tháng"),
                #endregion

                #region rút vốn
                new ReplaceTextDto(PropertiesContractFile.ACTUALLY_PROFIT, 10000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_RECEIVED, 100000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.AMOUNT_MONEY, 1000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.TAX, 10, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.DEDUCTIBLE_PROFIT, 500000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PROFIT, 500000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.WITH_DRAWAL_FEE, 100000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_PRICE, 5000000, EnumReplaceTextFormat.NumberVietNam),
                new ReplaceTextDto(PropertiesContractFile.PRODUCT_PRICE_TEXT, 5000000, EnumReplaceTextFormat.NumberToWord),
                #endregion

                #region Tái tục
                new ReplaceTextDto(PropertiesContractFile.INTEREST_PERIOD_RENEWALS, "Cuối kỳ"),
                #endregion
            });
            return replaceTexts;
        }

    }
}
